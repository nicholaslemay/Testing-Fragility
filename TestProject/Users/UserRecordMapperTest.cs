using BFF.Users;
using FluentAssertions;
using Xunit;
using static System.Enum;
using static FluentAssertions.FluentActions;

namespace BFF.Tests.Users;

public class UserRecordMapperTest
{
    [Fact]
    public void MapsUserToRecord()
    {
        var user = AValidUser();
        
        user.AsRecord().Name.Should().Be(user.Name);
        user.AsRecord().Email.Should().Be(user.Email);
    }
    
    [Theory]
    [InlineData(GenderType.Female, "F")]
    [InlineData(GenderType.Male, "M")]
    [InlineData(GenderType.Other, "O")]
    public void MapsEachGenderToDbType(GenderType type, string expectedValue)
    {
        var record = (AValidUser() with {Gender = type}).AsRecord();
        record.Gender.Should().Be(expectedValue);
    }

    [Fact]
    public void ShouldMappAllGenders()
    {
        foreach (var gender in GetValues<GenderType>())
        {
            Invoking(() => (AValidUser() with { Gender = gender }).AsRecord())
                .Should().NotThrow();

        }
    }

    private static User AValidUser()
    {
        return new User("tony", "tony@hotmail.com", GenderType.Male);
    }
}