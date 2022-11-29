using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using BFF.Communications;
using BFF.Component.Tests.Support;
using BFF.Users;
using FluentAssertions;
using WireMock.FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace BFF.Component.Tests.Users.Creation;

public class ValidUserCreationUserTest : ComponentTest
{
    private readonly NewUserRequest _userToBeCreated = new("tony", "tony@hotmail.com", "male", "active");
    private readonly HttpResponseMessage _response;

    public ValidUserCreationUserTest(CompontentTestFixture fixture) : base(fixture)
    {
        GivenCreatingCommunicationSucceeds();
        
        _response =  WhenCreating(_userToBeCreated);
    }
    
    [Fact]
    public void ReturnsA_20OK() => 
        _response.Should().HaveStatusCode(HttpStatusCode.OK);

    [Fact]
    public void StoresCreatedUserInDb()
    {
        Db.Users.Count().Should().Be(1);
        var storedUser = Db.Users.First();
        storedUser.Email.Should().Be("tony@hotmail.com");
        storedUser.Id.Should().Be(_response.ContentAs<NewUserResponse>().Id);
    }
    
    [Fact]
    public void CreatesConfirmationCommunication()
    {
        FakeCommunicationService.Should().HaveReceivedACall().AtAbsoluteUrl("http://localhost:777/accountCreationConfirmation");
        
        var body = FakeCommunicationService.LogEntries.Last().RequestMessage.Body;

        var sentConfirmation = body.AsDeserializedJson<AccountCreationConfirmation>();
        sentConfirmation!.Email.Should().Be("tony@hotmail.com");
        sentConfirmation.Name.Should().Be("tony");
    }

    private HttpResponseMessage WhenCreating(NewUserRequest userToBeCreated) =>
        Client.PostAsJsonAsync("/public/v2/users", userToBeCreated).Result;

    private void GivenCreatingCommunicationSucceeds() => FakeCommunicationService.Given(Request
        .Create()
        .WithPath("/accountCreationConfirmation")
        .UsingPost())
        .RespondWith(Response
            .Create()
            .WithStatusCode(201));
}