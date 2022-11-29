using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BFF.Component.Tests.Support;
using BFF.Users;
using FluentAssertions;
using Xunit;

namespace BFF.Component.Tests.Users.Creation;

public class InvalidUserCreationTest : ComponentTest
{
    public InvalidUserCreationTest(CompontentTestFixture fixture) : base(fixture) { }

    [Fact]
    public async Task CreatingAUserWithAnInvalidRequest_ReturnsA_400BadRequest()
    {
        using var response = await Client.PostAsJsonAsync("/public/v2/users", new NewUserRequest("", "", "", ""));
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}