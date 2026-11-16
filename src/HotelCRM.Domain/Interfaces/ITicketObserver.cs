using HotelCRM.Domain.Tickets;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.Domain.Interfaces;

/// <summary>
/// Observer of ticket state changes (Observer pattern). The customer subsystem reacts to events of
/// the ticket subsystem through this interface without the ticket knowing who is listening.
/// </summary>
public interface ITicketObserver
{
    /// <summary>
    /// Called after the ticket has moved to a new state. <see cref="SupportTicket.UpdatedAt"/> holds
    /// the time of the change. Exceptions are not swallowed: they propagate to the caller of the transition.
    /// </summary>
    /// <param name="ticket">Ticket that changed, already in the new state.</param>
    /// <param name="from">State before the change.</param>
    /// <param name="to">State after the change.</param>
    void OnStateChanged(SupportTicket ticket, TicketState from, TicketState to);
}
