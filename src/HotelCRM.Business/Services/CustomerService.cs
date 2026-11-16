using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Business.Services;

/// <summary>
/// UC6 and UC9: manage customers and read their interaction history. Reaches the ticket subsystem
/// only through <see cref="ITicketQuery"/>.
/// </summary>
public sealed class CustomerService
{
    private readonly IRepository<Customer> _customers;
    private readonly ITicketQuery _tickets;

    /// <summary>Creates the service.</summary>
    /// <param name="customers">Customer repository.</param>
    /// <param name="tickets">Ticket lookups of the other subsystem.</param>
    public CustomerService(IRepository<Customer> customers, ITicketQuery tickets)
    {
        _customers = customers;
        _tickets = tickets;
    }

    /// <summary>
    /// UC6: registers a new customer. Steps: take the next Id; create the <see cref="Customer"/>
    /// (format checks of rule 6); look up whether another customer has the same phone or email and
    /// call <see cref="Customer.EnsureUnique"/> (rule 6); save.
    /// </summary>
    /// <returns>The saved customer: Standard tier, 0 points.</returns>
    /// <exception cref="ValidationException">A required field is empty or phone or email is malformed.</exception>
    /// <exception cref="BusinessRuleException">Another customer already uses the phone or the email.</exception>
    public Customer Register(
        string fullName,
        string phone,
        string email,
        string identityNumber,
        string nationality,
        string notes)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// UC6: changes a customer's profile. Steps: find the customer; look up whether a different
    /// customer has the new phone or email and call <see cref="Customer.EnsureUnique"/> (rule 6);
    /// <see cref="Customer.UpdateProfile"/>; save.
    /// </summary>
    /// <exception cref="ValidationException">A required field is empty or phone or email is malformed.</exception>
    /// <exception cref="BusinessRuleException">The customer does not exist, or another customer already uses the phone or the email.</exception>
    public void Update(
        int customerId,
        string fullName,
        string phone,
        string email,
        string identityNumber,
        string nationality,
        string notes)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// UC6: deletes a customer. Steps: find the customer; ask <see cref="ITicketQuery.HasOpenTickets"/>
    /// and call <see cref="Customer.EnsureCanBeDeleted"/> (rule 10); delete from the repository.
    /// </summary>
    /// <exception cref="BusinessRuleException">The customer does not exist, or still has a ticket that is not closed.</exception>
    public void Delete(int customerId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the customers whose name, phone or email contains the keyword (case-insensitive),
    /// ordered by name. An empty keyword returns every customer.
    /// </summary>
    public IReadOnlyList<Customer> Search(string keyword)
    {
        throw new NotImplementedException();
    }

    /// <summary>Returns one customer for the edit and history forms.</summary>
    /// <exception cref="BusinessRuleException">The customer does not exist.</exception>
    public Customer GetById(int customerId)
    {
        throw new NotImplementedException();
    }

    /// <summary>UC9: returns the customer's interactions, newest first.</summary>
    /// <exception cref="BusinessRuleException">The customer does not exist.</exception>
    public IReadOnlyList<Interaction> GetHistory(int customerId)
    {
        throw new NotImplementedException();
    }
}
