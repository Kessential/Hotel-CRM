using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.WinForms;

/// <summary>
/// Observer that tells the user interface about ticket state changes. It raises
/// <see cref="NotificationRaised"/> with a short text such as "Ticket 12: Open to InProgress";
/// <c>MainForm</c> listens and shows the text in its status bar. The domain and business layers
/// know this class only as an <see cref="ITicketObserver"/>.
/// </summary>
public sealed class UiNotificationObserver : ITicketObserver
{
    /// <summary>Raised with the text to show whenever a ticket changes state.</summary>
    public event Action<string>? NotificationRaised;

    /// <inheritdoc />
    public void OnStateChanged(SupportTicket ticket, TicketState from, TicketState to)
    {
        // Filled in at stage 4: build the text and call Publish. Left empty instead of throwing so
        // that ticket transitions already work while the customer subsystem is unfinished.
    }

    /// <summary>Raises <see cref="NotificationRaised"/>.</summary>
    private void Publish(string message)
    {
        NotificationRaised?.Invoke(message);
    }
}
