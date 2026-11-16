namespace HotelCRM.Data.Records;

/// <summary>One entry of <c>rooms.json</c>. The room type is stored by Id.</summary>
public sealed class RoomRecord
{
    public int Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public int Floor { get; set; }

    public int RoomTypeId { get; set; }
}
