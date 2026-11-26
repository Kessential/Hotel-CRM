using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;

namespace HotelCRM.Tests.Customers;

/// <summary>An interaction has content, and an employee exactly when it was not recorded by the system.</summary>
public class InteractionTests
{
    [Fact]
    public void Constructor_EmptySummary_ThrowsValidation()
    {
        Assert.Throws<ValidationException>(
            () => new Interaction(1, 1, ContactChannel.Phone, "   ", CustomerTestData.Now, employeeId: 3, ticketId: null));
    }

    [Fact]
    public void Constructor_ManualChannelWithoutEmployee_ThrowsValidation()
    {
        Assert.Throws<ValidationException>(
            () => new Interaction(1, 1, ContactChannel.FrontDesk, "Khách hỏi giờ trả phòng.", CustomerTestData.Now, employeeId: null, ticketId: null));
    }

    [Fact]
    public void Constructor_SystemChannelWithEmployee_ThrowsValidation()
    {
        Assert.Throws<ValidationException>(
            () => new Interaction(1, 1, ContactChannel.System, "Ticket 7 đã đóng.", CustomerTestData.Now, employeeId: 3, ticketId: 7));
    }

    [Fact]
    public void Constructor_SystemChannelFromTicket_KeepsTicketAndTrimsSummary()
    {
        Interaction interaction = new(1, 1, ContactChannel.System, "  Ticket 7 đã đóng. ", CustomerTestData.Now, employeeId: null, ticketId: 7);

        Assert.Equal("Ticket 7 đã đóng.", interaction.Summary);
        Assert.Equal(7, interaction.TicketId);
        Assert.Null(interaction.EmployeeId);
    }
}
