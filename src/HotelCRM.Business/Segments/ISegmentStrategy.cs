namespace HotelCRM.Business.Segments;

/// <summary>
/// One way of choosing the customers of a care campaign (Strategy pattern). The campaign service is the
/// context: it knows only this interface, and the manager picks the strategy on the campaign form at run time.
/// A new segment is a new class plus one line in the list built in <c>Program.cs</c>; no existing class changes.
/// </summary>
public interface ISegmentStrategy
{
    /// <summary>Short code stored in <c>campaigns.SegmentCode</c>. Unique among the registered strategies.</summary>
    string Code { get; }

    /// <summary>Name shown in the segment list of the campaign form.</summary>
    string DisplayName { get; }

    /// <summary>Whether <paramref name="customer"/> belongs to this segment.</summary>
    /// <param name="customer">Data of one customer.</param>
    /// <param name="referenceDate">Day the segment is evaluated for: the start date of the campaign.</param>
    bool Includes(SegmentCandidate customer, DateTime referenceDate);
}
