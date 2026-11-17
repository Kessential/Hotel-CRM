using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;

namespace HotelCRM.Tests.Customers;

/// <summary>Rule 6 (phone and email format) and rule 8 (points and tier upgrade).</summary>
public class CustomerTests
{
    [Fact]
    public void Constructor_InvalidEmail_ThrowsValidation()
    {
        // Arrange: no '@' in the address.
        const string email = "emma.stone.example.com";

        // Act + Assert
        Assert.Throws<ValidationException>(
            () => CustomerTestData.NewCustomer(email: email));
    }

    [Fact]
    public void Constructor_InvalidPhone_ThrowsValidation()
    {
        // Arrange: nine digits and no leading 0.
        const string phone = "912345678";

        // Act + Assert
        Assert.Throws<ValidationException>(
            () => CustomerTestData.NewCustomer(phone: phone));
    }

    [Fact]
    public void AddPoints_PositiveAmount_IncreasesPoints()
    {
        // Arrange
        Customer customer = CustomerTestData.NewCustomer();

        // Act
        customer.AddPoints(120);

        // Assert
        Assert.Equal(120, customer.Points);
    }

    [Fact]
    public void AddPoints_CrossingThresholds_UpgradesTier()
    {
        // Arrange: a Standard customer; Silver starts at 1000 points and Gold at 5000.
        Customer customer = CustomerTestData.NewCustomer();

        // Act: one award passes both thresholds.
        customer.AddPoints(6000);

        // Assert: the customer skipped Silver and is Gold.
        MembershipTier tier = customer.Tier;
        Assert.IsType<GoldTier>(tier);
    }
}
