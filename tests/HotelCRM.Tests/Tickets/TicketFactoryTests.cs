using HotelCRM.Business.Factories;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Tests.Tickets;

/// <summary>Factory Method: each factory creates its own kind of ticket behind the common creator type.</summary>
public class TicketFactoryTests
{
    private const int RoomId = 305;

    [Fact]
    public void Create_EachFactory_ReturnsMatchingSubclass()
    {
        // Arrange: every factory is used through the abstract creator.
        TicketFactory complaintFactory = new ComplaintTicketFactory();
        TicketFactory serviceRequestFactory = new ServiceRequestTicketFactory();
        TicketFactory maintenanceFactory = new MaintenanceTicketFactory();

        // Act
        SupportTicket complaint = complaintFactory.Create(
            1, TicketTestData.CustomerId, "Noisy room", "Loud music next door.", TicketPriority.Medium,
            TicketTestData.StandardProfile, TicketTestData.Now, roomId: null);
        SupportTicket serviceRequest = serviceRequestFactory.Create(
            2, TicketTestData.CustomerId, "Extra towels", "Two bath towels, please.", TicketPriority.Low,
            TicketTestData.StandardProfile, TicketTestData.Now, roomId: null);
        SupportTicket maintenance = maintenanceFactory.Create(
            3, TicketTestData.CustomerId, "Air conditioner broken", "It does not cool the room.", TicketPriority.High,
            TicketTestData.StandardProfile, TicketTestData.Now, RoomId);

        // Assert
        Assert.IsType<ComplaintTicket>(complaint);
        Assert.IsType<ServiceRequestTicket>(serviceRequest);
        Assert.IsType<MaintenanceTicket>(maintenance);
    }
}
