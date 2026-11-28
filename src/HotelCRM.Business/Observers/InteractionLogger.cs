using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.Business.Observers;

/// <summary>
/// Observer that will record one <see cref="Interaction"/> every time a ticket of a customer changes state:
/// channel <see cref="ContactChannel.System"/>, time <see cref="SupportTicket.UpdatedAt"/>, the ticket Id,
/// and a summary naming the ticket and both states. Not implemented yet: <see cref="OnStateChanged"/> does
/// nothing until it is rewritten on the SQLite repositories in P3.
/// </summary>
public sealed class InteractionLogger : ITicketObserver
{
    private readonly IRepository<Customer> _customers;

    /// <summary>Creates the observer.</summary>
    /// <param name="customers">Customer repository.</param>
    public InteractionLogger(IRepository<Customer> customers)
    {
        _customers = customers;
    }

    /// <inheritdoc />
    public void OnStateChanged(SupportTicket ticket, TicketState from, TicketState to)
    {
        // To be written in P3. Left empty instead of throwing so that ticket transitions already work
        // while the customer subsystem is unfinished.
    }
}
