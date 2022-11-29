using BFF.Tests.Support;
using BFF.Users;
using Xunit;

namespace BFF.Tests.Users;

public class NewUserRequestValidatorTest : ValidationTest<NewUserRequestValidator, NewUserRequest>
{
    [Fact]
    public void HasNoErrorWhenValidExample() => ValidExempleHasNoError();
    
    [Fact]
    public void ValidatesPresenceOfEachRequiredFields()
    {
        ValidateFieldCannotBeNullOrEmpty(r=> r.Name);
        ValidateFieldCannotBeNullOrEmpty(r=> r.Email);
        ValidateFieldCannotBeNullOrEmpty(r=> r.Gender);
        ValidateFieldCannotBeNullOrEmpty(r=> r.Status);
    }

    [Fact]
    public void ValidatesGendersIsInValidChoices()
    {
        ValidateValuesAreValidForType(r=> r.Gender, NewUserRequest.ValidGenders);
        ValidateValueIsInvalidForField(r=> r.Gender, "SomeRandopmGender", "Invalid gender");
    }
    
    protected override NewUserRequest ValidExample() => 
        new("tony", "tony@gmail.com", "male", "active");
}