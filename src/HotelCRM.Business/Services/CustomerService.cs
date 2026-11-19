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
        Customer customer = new(_customers.NextId(), fullName, phone, email, identityNumber, nationality, notes);
        customer.EnsureUnique(
            IsPhoneTaken(customer.Phone, customer.Id),
            IsEmailTaken(customer.Email, customer.Id));
        _customers.Save(customer);
        return customer;
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
        Customer customer = _customers.GetRequired(customerId);
        customer.EnsureUnique(
            IsPhoneTaken(phone, customerId),
            IsEmailTaken(email, customerId));
        customer.UpdateProfile(fullName, phone, email, identityNumber, nationality, notes);
        _customers.Save(customer);
    }

    /// <summary>
    /// UC6: deletes a customer. Steps: find the customer; ask <see cref="ITicketQuery.HasOpenTickets"/>
    /// and call <see cref="Customer.EnsureCanBeDeleted"/> (rule 10); delete from the repository.
    /// </summary>
    /// <exception cref="BusinessRuleException">The customer does not exist, or still has a ticket that is not closed.</exception>
    public void Delete(int customerId)
    {
        Customer customer = _customers.GetRequired(customerId);
        customer.EnsureCanBeDeleted(_tickets.HasOpenTickets(customerId));
        _customers.Delete(customerId);
    }

    /// <summary>
    /// Returns the customers whose name, phone or email contains the keyword (case-insensitive),
    /// ordered by name. An empty keyword returns every customer.
    /// </summary>
    public IReadOnlyList<Customer> Search(string keyword)
    {
        string trimmed = (keyword ?? string.Empty).Trim();
        return _customers.GetAll()
            .Where(c => trimmed.Length == 0
                || c.FullName.Contains(trimmed, StringComparison.OrdinalIgnoreCase)
                || c.Phone.Contains(trimmed, StringComparison.OrdinalIgnoreCase)
                || c.Email.Contains(trimmed, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.FullName, StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(c => c.Id)
            .ToList();
    }

    /// <summary>Returns one customer for the edit and history forms.</summary>
    /// <exception cref="BusinessRuleException">The customer does not exist.</exception>
    public Customer GetById(int customerId)
    {
        return _customers.GetRequired(customerId);
    }

    /// <summary>UC9: returns the customer's interactions, newest first.</summary>
    /// <exception cref="BusinessRuleException">The customer does not exist.</exception>
    public IReadOnlyList<Interaction> GetHistory(int customerId)
    {
        return _customers.GetRequired(customerId).History.Items.Reverse().ToList();
    }

    /// <summary>Whether a customer other than <paramref name="exceptCustomerId"/> already has this phone.</summary>
    private bool IsPhoneTaken(string phone, int exceptCustomerId)
    {
        string trimmed = (phone ?? string.Empty).Trim();
        return _customers.GetAll().Any(c => c.Id != exceptCustomerId && c.Phone == trimmed);
    }

    /// <summary>Whether a customer other than <paramref name="exceptCustomerId"/> already has this email (case-insensitive).</summary>
    private bool IsEmailTaken(string email, int exceptCustomerId)
    {
        string trimmed = (email ?? string.Empty).Trim();
        return _customers.GetAll().Any(c => c.Id != exceptCustomerId
            && string.Equals(c.Email, trimmed, StringComparison.OrdinalIgnoreCase));
    }
}
