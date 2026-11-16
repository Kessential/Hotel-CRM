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
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public CustomerSummary GetSummary(int customerId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns <see cref="MembershipTier.SlaFactor"/> and <see cref="MembershipTier.PriorityBoost"/> of the
    /// customer's tier. Uses <see cref="RepositoryExtensions.GetRequired"/>, so an unknown customer fails
    /// with a business error.
    /// </summary>
    public CustomerSlaProfile GetSlaProfile(int customerId)
    {
        throw new NotImplementedException();
    }
}
