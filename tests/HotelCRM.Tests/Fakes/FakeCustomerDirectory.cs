using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Tests.Fakes;

/// <summary>
/// Hand-written stand-in for the customer subsystem. Lets the ticket tests run without any class of
/// <c>Domain/Customers</c>: a customer is just an Id with an SLA profile.
/// </summary>
internal sealed class FakeCustomerDirectory : ICustomerDirectory
{
    private readonly Dictionary<int, CustomerSlaProfile> _profiles = new();

    /// <summary>Registers a customer with the SLA numbers of their tier.</summary>
    public void Add(int customerId, CustomerSlaProfile profile)
    {
        _profiles[customerId] = profile;
    }

    public IReadOnlyList<CustomerSummary> Search(string keyword)
    {
        return _profiles.Keys.Select(GetSummary).ToList();
    }

    public CustomerSummary GetSummary(int customerId)
    {
        EnsureKnown(customerId);
        return new CustomerSummary(customerId, $"Customer {customerId}", "0900000000", "Standard");
    }

    public CustomerSlaProfile GetSlaProfile(int customerId)
    {
        EnsureKnown(customerId);
        return _profiles[customerId];
    }

    private void EnsureKnown(int customerId)
    {
        if (!_profiles.ContainsKey(customerId))
        {
            throw new BusinessRuleException($"Customer {customerId} does not exist.");
        }
    }
}
