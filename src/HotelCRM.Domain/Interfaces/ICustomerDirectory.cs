using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Interfaces;

/// <summary>
/// Boundary interface: what the ticket subsystem (share A) needs from the customer subsystem (share B).
/// Implemented by <c>CustomerDirectory</c> in <c>Business/Queries</c>; tests use <c>FakeCustomerDirectory</c>.
/// </summary>
public interface ICustomerDirectory
{
    /// <summary>
    /// Finds customers whose name, phone or email contains the keyword (case-insensitive).
    /// An empty keyword returns every customer.
    /// </summary>
    IReadOnlyList<CustomerSummary> Search(string keyword);

    /// <summary>Returns the summary of one customer.</summary>
    /// <exception cref="BusinessRuleException">The customer does not exist.</exception>
    CustomerSummary GetSummary(int customerId);

    /// <summary>Returns the SLA factor and priority boost of the customer's current membership tier (rule 4).</summary>
    /// <exception cref="BusinessRuleException">The customer does not exist.</exception>
    CustomerSlaProfile GetSlaProfile(int customerId);
}
