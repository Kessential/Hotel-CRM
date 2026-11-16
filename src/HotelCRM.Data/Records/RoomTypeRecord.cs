namespace HotelCRM.Data.Records;

/// <summary>One entry of <c>roomtypes.json</c>.</summary>
public sealed class RoomTypeRecord
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }
}
