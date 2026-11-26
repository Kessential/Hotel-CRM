using HotelCRM.Business.Assignment;
using HotelCRM.Business.Factories;
using HotelCRM.Business.Observers;
using HotelCRM.Business.Queries;
using HotelCRM.Business.Services;
using HotelCRM.Data;
using HotelCRM.Data.Mappers;
using HotelCRM.Data.Records;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Hotel;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Staff;
using HotelCRM.Domain.Tickets;
using HotelCRM.WinForms.Forms;

namespace HotelCRM.WinForms;

/// <summary>
/// Application entry point and composition root: the only place that creates the configuration, the
/// repositories, the observers, the strategy and factory tables and the services, and hands them to
/// the forms through constructors. No dependency injection library is used.
/// </summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => ErrorHandler.Handle(e.Exception);

        AppServices services;
        try
        {
            services = Compose();
        }
        catch (Exception ex)
        {
            ErrorHandler.ReportStartupFailure(ex);
            return;
        }

        // Log in, work, and come back to the login form after Log Out; leave on Exit.
        while (true)
        {
            using (LoginForm loginForm = new(services.Auth))
            {
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            using MainForm mainForm = new(
                services.Auth,
                services.Tickets,
                services.Surveys,
                services.Reports,
                services.Notifications);
            Application.Run(mainForm);

            if (!mainForm.LoggedOut)
            {
                return;
            }
        }
    }

    /// <summary>
    /// Builds every object of the application in dependency order:
    /// configuration, repositories, boundary queries, observers, lookup tables, services.
    /// </summary>
    private static AppServices Compose()
    {
        // 1. Configuration (Singleton).
        AppConfig config = AppConfig.Instance;

        // 2. Repositories, one per entity type, loaded in this order because later files refer to earlier ones.
        IRepository<Department> departments = new JsonRepository<Department, DepartmentRecord>(
            config.GetDataFilePath("departments.json"), new DepartmentMapper());
        IRepository<Employee> employees = new JsonRepository<Employee, EmployeeRecord>(
            config.GetDataFilePath("employees.json"), new EmployeeMapper(departments));
        // Customer subsystem (TV2): the v1 JSON files were removed; empty in-memory repositories keep
        // CustomerDirectory, StayLookup and the observers working until the SQLite repositories of P3.
        IRepository<Room> rooms = new InMemoryRepository<Room>();
        IRepository<Customer> customers = new InMemoryRepository<Customer>();
        IRepository<Booking> bookings = new InMemoryRepository<Booking>();
        IRepository<SupportTicket> tickets = new JsonRepository<SupportTicket, TicketRecord>(
            config.GetDataFilePath("tickets.json"), new TicketMapper(employees));
        IRepository<SatisfactionSurvey> surveys = new JsonRepository<SatisfactionSurvey, SurveyRecord>(
            config.GetDataFilePath("surveys.json"), new SurveyMapper());

        // 3. Boundary queries: the only way one subsystem reads the other's data.
        ICustomerDirectory customerDirectory = new CustomerDirectory(customers);
        IStayLookup stayLookup = new StayLookup(bookings, rooms);
        ITicketQuery ticketQuery = new TicketQuery(tickets);

        // 4. Observers of ticket state changes.
        UiNotificationObserver notifications = new();
        IReadOnlyList<ITicketObserver> observers = new ITicketObserver[]
        {
            new InteractionLogger(customers),
            new LoyaltyObserver(customers),
            notifications,
        };

        // 5. Lookup tables. Adding a strategy or a ticket kind means adding one line here.
        Dictionary<string, IAssignmentStrategy> strategies = new(StringComparer.OrdinalIgnoreCase)
        {
            ["LeastLoaded"] = new LeastLoadedStrategy(),
            ["DepartmentMatch"] = new DepartmentMatchStrategy(),
            ["RoundRobin"] = new RoundRobinStrategy(),
        };
        if (!strategies.TryGetValue(config.DefaultAssignmentStrategy, out IAssignmentStrategy? strategy))
        {
            throw new InvalidDataException(
                $"Unknown assignment strategy '{config.DefaultAssignmentStrategy}' in appsettings.json. "
                + $"Valid names: {string.Join(", ", strategies.Keys)}.");
        }

        Dictionary<string, TicketFactory> factories = new()
        {
            [ComplaintTicket.TypeKey] = new ComplaintTicketFactory(),
            [ServiceRequestTicket.TypeKey] = new ServiceRequestTicketFactory(),
            [MaintenanceTicket.TypeKey] = new MaintenanceTicketFactory(),
        };

        // 6. Services.
        return new AppServices(
            new AuthService(employees),
            new TicketService(tickets, employees, customerDirectory, stayLookup, strategy, factories, observers, config.Rules),
            new SurveyService(surveys, ticketQuery),
            new ReportService(ticketQuery, employees, departments, surveys),
            notifications);
    }

    /// <summary>The objects the forms need, bundled so that <see cref="Compose"/> can return them together.</summary>
    private sealed record AppServices(
        AuthService Auth,
        TicketService Tickets,
        SurveyService Surveys,
        ReportService Reports,
        UiNotificationObserver Notifications);
}
