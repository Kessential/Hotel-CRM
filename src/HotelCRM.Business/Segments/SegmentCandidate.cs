using HotelCRM.Domain.Customers;

namespace HotelCRM.Business.Segments;

/// <summary>
/// What a segment strategy may look at for one customer. Filled by the campaign service from the
/// customer and booking data, so a strategy never queries the database itself.
/// </summary>
/// <param name="CustomerId">The customer.</param>
/// <param name="Tier">Current membership tier.</param>
/// <param name="DateOfBirth">Date of birth, or null when the customer did not give it.</param>
/// <param name="StayCheckOutDates">Check-out dates of the customer's completed stays, in any order. May be empty.</param>
public sealed record SegmentCandidate(
    int CustomerId,
    MembershipTier Tier,
    DateTime? DateOfBirth,
    IReadOnlyList<DateTime> StayCheckOutDates)
{
    /// <summary>Check-out date of the latest completed stay, or null when the customer has never stayed.</summary>
    public DateTime? LastCheckOut => StayCheckOutDates.Count == 0 ? null : StayCheckOutDates.Max();
}
