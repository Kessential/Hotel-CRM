using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Business.Queries;

/// <summary>
/// Answers the ticket subsystem's questions about customers. Built on the customer repository only
/// (not on <c>CustomerService</c>), so the two subsystems' services never depend on each other.
/// </summary>
public sealed class CustomerDirectory : ICustomerDirectory
{
    private readonly IRepository<Customer> _customers;

    /// <summary>Creates the directory.</summary>
    /// <param name="customers">Customer repository.</param>
    public CustomerDirectory(IRepository<Customer> customers)
    {
        _customers = customers;
    }

    /// <inheritdoc />
    public IReadOnlyList<CustomerSummary> Search(string keyword)
    {
        string trimmed = (keyword ?? string.Empty).Trim();
        return _customers.GetAll()
            .Where(c => trimmed.Length == 0
                || c.FullName.Contains(trimmed, StringComparison.OrdinalIgnoreCase)
                || c.Phone.Contains(trimmed, StringComparison.OrdinalIgnoreCase)
                || c.Email.Contains(trimmed, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.FullName, StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(c => c.Id)
            .Select(ToSummary)
            .ToList();
    }

    /// <inheritdoc />
    public CustomerSummary GetSummary(int customerId)
    {
        return ToSummary(_customers.GetRequired(customerId));
    }

    /// <summary>
    /// Returns <see cref="MembershipTier.SlaFactor"/> and <see cref="MembershipTier.PriorityBoost"/> of the
    /// customer's tier. Uses <see cref="RepositoryExtensions.GetRequired"/>, so an unknown customer fails
    /// with a business error.
    /// </summary>
    public CustomerSlaProfile GetSlaProfile(int customerId)
    {
        MembershipTier tier = _customers.GetRequired(customerId).Tier;
        return new CustomerSlaProfile(tier.SlaFactor, tier.PriorityBoost);
    }

    private static CustomerSummary ToSummary(Customer customer)
    {
        return new CustomerSummary(customer.Id, customer.FullName, customer.Phone, customer.Tier.Name);
    }
}
