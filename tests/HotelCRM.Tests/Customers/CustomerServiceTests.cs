using HotelCRM.Business.Services;
using HotelCRM.Data;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Tests.Fakes;

namespace HotelCRM.Tests.Customers;

/// <summary>
/// Rule 6 (no duplicate phone or email) and rule 10 (no deletion while tickets are open), checked
/// through <see cref="CustomerService"/> with an in-memory repository and a fake ticket query.
/// </summary>
public class CustomerServiceTests
{
    private readonly InMemoryRepository<Customer> _customers = new();
    private readonly FakeTicketQuery _tickets = new();
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _service = new CustomerService(_customers, _tickets);
    }

    [Fact]
    public void Register_DuplicatePhone_ThrowsBusinessRule()
    {
        // Arrange: one customer already uses the phone number.
        _service.Register("Emma Stone", CustomerTestData.ValidPhone, CustomerTestData.ValidEmail, "P1234567", "British", "");

        // Act + Assert: a second customer with the same phone but another email is refused.
        Assert.Throws<BusinessRuleException>(
            () => _service.Register("Liam Brown", CustomerTestData.ValidPhone, "liam.brown@example.com", "P7654321", "Irish", ""));
    }

    [Fact]
    public void Delete_CustomerWithOpenTicket_ThrowsBusinessRule()
    {
        // Arrange: the customer still has a ticket that is not closed.
        Customer customer = _service.Register(
            "Emma Stone", CustomerTestData.ValidPhone, CustomerTestData.ValidEmail, "P1234567", "British", "");
        _tickets.AddOpenTicketFor(customer.Id);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => _service.Delete(customer.Id));
    }
}
