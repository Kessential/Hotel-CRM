using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.Business.Observers;

/// <summary>
/// Observer that rewards the customer when one of their tickets is closed (rule 8): when the new
/// state's <see cref="TicketState.IsClosed"/> is true it adds <see cref="SupportTicket.GoodwillPoints"/>
/// with <see cref="Customer.EarnPoints"/> and saves the customer through the repository itself.
/// Other state changes are ignored.
/// </summary>
public sealed class LoyaltyObserver : ITicketObserver
{
    private readonly IRepository<Customer> _customers;

    /// <summary>Creates the observer.</summary>
    /// <param name="customers">Customer repository.</param>
    public LoyaltyObserver(IRepository<Customer> customers)
    {
        _customers = customers;
    }

    /// <inheritdoc />
    public void OnStateChanged(SupportTicket ticket, TicketState from, TicketState to)
    {
        // Filled in at stage 4. Left empty instead of throwing so that ticket transitions already work
        // while the customer subsystem is unfinished.
    }
}
