using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.Business.Observers;

/// <summary>
/// Observer that writes one entry into the customer's interaction history every time one of their
/// tickets changes state: channel <see cref="ContactChannel.System"/>, time
/// <see cref="SupportTicket.UpdatedAt"/>, the ticket Id, and a summary naming the ticket and both states.
/// It changes the customer and saves it through the repository itself.
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
        // Filled in at stage 4. Left empty instead of throwing so that ticket transitions already work
        // while the customer subsystem is unfinished.
    }
}
