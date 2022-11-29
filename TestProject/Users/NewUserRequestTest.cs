using BFF.Users;
using FluentAssertions;
using Xunit;

namespace BFF.Tests.Users;

public class NewUserRequestTest
{
    [Fact]
    public void MapsRequest()
    {
        var request = ValidUserRequest();
        request.AsUser().Email.Should().Be(request.Email);
        request.AsUser().Name.Should().Be(request.Name);
    }

    [Theory]
    [InlineData("female", GenderType.Female)]
    [InlineData("male", GenderType.Male)]
    [InlineData("other", GenderType.Other)]
    public void MapsEachGender(string gender, GenderType expectedGender)
    {
        var request = ValidUserRequest() with { Gender = gender };
        request.AsUser().Gender.Should().Be(expectedGender);
    }
    
    private static NewUserRequest ValidUserRequest() => new("tony", "tony@hotmail.com", "female", "");
}

