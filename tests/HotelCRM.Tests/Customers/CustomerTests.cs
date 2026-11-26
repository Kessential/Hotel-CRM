using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;

namespace HotelCRM.Tests.Customers;

/// <summary>
/// TC2-01 (Q7 formats, boundary values), TC2-02 (Q7 duplicates), TC2-05 (Q9 tier upgrade, hand-calculated)
/// and the Domain part of TC2-10 (Q13 deletion).
/// </summary>
public class CustomerTests
{
    // ---- TC2-01: Q7 formats ----

    [Theory]
    [InlineData("091234567")]   // 9 digits
    [InlineData("09123456789")] // 11 digits
    [InlineData("1912345678")]  // 10 digits, not starting with 0
    public void Constructor_InvalidPhone_ThrowsValidation(string phone)
    {
        Assert.Throws<ValidationException>(() => CustomerTestData.NewCustomer(phone: phone));
    }

    [Fact]
    public void Constructor_EmailWithoutAt_ThrowsValidation()
    {
        Assert.Throws<ValidationException>(() => CustomerTestData.NewCustomer(email: "nguyen.an.example.com"));
    }

    [Theory]
    [InlineData("00120300456")]   // 11 digits
    [InlineData("0012030045678")] // 13 digits
    [InlineData("00120300456A")]  // 12 characters, one is a letter
    public void Constructor_VietnameseWithInvalidCitizenId_ThrowsValidation(string identityNumber)
    {
        Assert.Throws<ValidationException>(() => CustomerTestData.NewCustomer(identityNumber: identityNumber));
    }

    [Fact]
    public void Constructor_VietnameseWithTwelveDigits_IsCreated()
    {
        Customer customer = CustomerTestData.NewCustomer(identityNumber: "001203004567");

        Assert.Equal("001203004567", customer.IdentityNumber);
    }

    [Theory]
    [InlineData("TK1234")]    // 6 characters: lower bound
    [InlineData("TK1234567")] // 9 characters: upper bound
    public void Constructor_ForeignerWithPassportAtBounds_IsCreated(string passport)
    {
        Customer customer = CustomerTestData.NewCustomer(identityNumber: passport, nationality: "Nhật Bản");

        Assert.Equal(passport, customer.IdentityNumber);
    }

    [Theory]
    [InlineData("TK123")]      // 5 characters
    [InlineData("TK12345678")] // 10 characters
    [InlineData("TK-12345")]   // 8 characters with a symbol
    public void Constructor_ForeignerWithInvalidPassport_ThrowsValidation(string passport)
    {
        Assert.Throws<ValidationException>(
            () => CustomerTestData.NewCustomer(identityNumber: passport, nationality: "Nhật Bản"));
    }

    [Fact]
    public void Constructor_ForeignerWithLowerCasePassport_StoresUpperCase()
    {
        Customer customer = CustomerTestData.NewCustomer(identityNumber: " tk1234567 ", nationality: "Nhật Bản");

        Assert.Equal("TK1234567", customer.IdentityNumber);
    }

    [Fact]
    public void Constructor_DateOfBirthToday_IsCreated()
    {
        // Now is 16/11/2026 09:00; a birth date on the same day is allowed.
        Customer customer = CustomerTestData.NewCustomer(dateOfBirth: new DateTime(2026, 11, 16));

        Assert.Equal(new DateTime(2026, 11, 16), customer.DateOfBirth);
    }

    [Fact]
    public void Constructor_DateOfBirthTomorrow_ThrowsValidation()
    {
        Assert.Throws<ValidationException>(
            () => CustomerTestData.NewCustomer(dateOfBirth: new DateTime(2026, 11, 17)));
    }

    [Fact]
    public void Constructor_NewCustomer_IsStandardWithZeroPoints()
    {
        Customer customer = CustomerTestData.NewCustomer();

        Assert.IsType<StandardTier>(customer.Tier);
        Assert.Equal(0, customer.LifetimePoints);
        Assert.Equal(CustomerTestData.Now, customer.CreatedAt);
    }

    [Fact]
    public void UpdateProfile_InvalidCitizenId_LeavesCustomerUnchanged()
    {
        // Arrange
        Customer customer = CustomerTestData.NewCustomer();

        // Act: the new name is valid but the citizen ID has 11 digits, so nothing may change.
        Assert.Throws<ValidationException>(() => customer.UpdateProfile(
            "Trần Thị Bình", "0987654321", "binh@example.com", "00120300456", Customer.VietnameseNationality,
            null, false, "", CustomerTestData.Now));

        // Assert
        Assert.Equal("Nguyễn Văn An", customer.FullName);
        Assert.Equal(CustomerTestData.ValidPhone, customer.Phone);
        Assert.Equal(CustomerTestData.ValidCitizenId, customer.IdentityNumber);
        Assert.True(customer.MarketingConsent);
    }

    // ---- TC2-02: Q7 duplicates (the service looks the flags up, leaving the edited customer out) ----

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    public void EnsureUnique_AnyFieldTaken_ThrowsBusinessRule(bool phoneTaken, bool emailTaken, bool identityTaken)
    {
        Customer customer = CustomerTestData.NewCustomer();

        Assert.Throws<BusinessRuleException>(() => customer.EnsureUnique(phoneTaken, emailTaken, identityTaken));
    }

    [Fact]
    public void EnsureUnique_NothingTaken_DoesNotThrow()
    {
        Customer customer = CustomerTestData.NewCustomer();

        customer.EnsureUnique(false, false, false);
    }

    // ---- TC2-05: Q9 tier upgrade (thresholds Silver 1.000, Gold 5.000, VIP 15.000) ----

    [Fact]
    public void EarnPoints_From900To5100_SkipsSilverAndBecomesGold()
    {
        // Arrange: Standard customer with 900 lifetime points.
        Customer customer = CustomerTestData.NewCustomer();
        customer.EarnPoints(900);

        // Act: a 42.000.000đ stay at Standard rate earns 4.200 points.
        bool upgraded = customer.EarnPoints(4200);

        // Assert: 900 + 4.200 = 5.100 >= 5.000.
        Assert.True(upgraded);
        Assert.Equal(5100, customer.LifetimePoints);
        Assert.IsType<GoldTier>(customer.Tier);
    }

    [Fact]
    public void EarnPoints_ReachingExactly999_StaysStandard()
    {
        Customer customer = CustomerTestData.NewCustomer();

        bool upgraded = customer.EarnPoints(999);

        Assert.False(upgraded);
        Assert.IsType<StandardTier>(customer.Tier);
    }

    [Fact]
    public void EarnPoints_ReachingExactly1000_BecomesSilver()
    {
        Customer customer = CustomerTestData.NewCustomer();
        customer.EarnPoints(999);

        bool upgraded = customer.EarnPoints(1);

        Assert.True(upgraded);
        Assert.IsType<SilverTier>(customer.Tier);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void EarnPoints_NotPositive_ThrowsValidation(int points)
    {
        Customer customer = CustomerTestData.NewCustomer();

        Assert.Throws<ValidationException>(() => customer.EarnPoints(points));
    }

    // ---- TC2-10 (Domain part): Q13 deletion ----

    [Theory]
    [InlineData(true, false, false)]  // one booking, even a cancelled one
    [InlineData(false, true, false)]  // one ticket, even a closed one
    [InlineData(false, false, true)]  // only a campaign target: no stay, no ticket
    public void EnsureCanBeDeleted_AnyHistory_ThrowsBusinessRule(bool hasStays, bool hasTickets, bool inCampaigns)
    {
        Customer customer = CustomerTestData.NewCustomer();

        Assert.Throws<BusinessRuleException>(() => customer.EnsureCanBeDeleted(hasStays, hasTickets, inCampaigns));
    }

    [Fact]
    public void EnsureCanBeDeleted_NoHistory_DoesNotThrow()
    {
        Customer customer = CustomerTestData.NewCustomer();

        customer.EnsureCanBeDeleted(false, false, false);
    }
}
