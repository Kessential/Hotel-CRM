using HotelCRM.Data.Records;
using HotelCRM.Domain.Customers;

namespace HotelCRM.Data.Mappers;

/// <summary>
/// Converts between <see cref="Customer"/> and <see cref="CustomerRecord"/>, including the nested
/// interaction history. The tier is stored by name and rebuilt with <see cref="MembershipTier.FromName"/>.
/// </summary>
public sealed class CustomerMapper : IMapper<Customer, CustomerRecord>
{
    /// <inheritdoc />
    public CustomerRecord ToRecord(Customer entity)
    {
        return new CustomerRecord
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Phone = entity.Phone,
            Email = entity.Email,
            IdentityNumber = entity.IdentityNumber,
            Nationality = entity.Nationality,
            Notes = entity.Notes,
            Tier = entity.Tier.Name,
            Points = entity.Points,
            Interactions = entity.History.Items
                .Select(item => new InteractionRecord
                {
                    OccurredAt = item.OccurredAt,
                    Channel = item.Channel,
                    Summary = item.Summary,
                    TicketId = item.TicketId,
                })
                .ToList(),
        };
    }

    /// <inheritdoc />
    public Customer ToEntity(CustomerRecord record)
    {
        IEnumerable<Interaction> interactions = record.Interactions
            .Select(item => Interaction.Restore(item.Channel, item.Summary, item.OccurredAt, item.TicketId));

        return Customer.Restore(
            record.Id,
            record.FullName,
            record.Phone,
            record.Email,
            record.IdentityNumber,
            record.Nationality,
            record.Notes,
            MembershipTier.FromName(record.Tier),
            record.Points,
            InteractionHistory.Restore(interactions));
    }
}
