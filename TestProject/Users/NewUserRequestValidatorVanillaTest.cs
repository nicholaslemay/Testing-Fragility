using System.Threading.Tasks;
using BFF.Support;
using BFF.Users;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace BFF.Tests.Users;

public class NewUserRequestValidatorVanillaTest
{
    [Fact]
    public async Task HasNoErrorWhenValid()
    {
        var validator = new NewUserRequestValidator();

        var validationResult =  validator.TestValidateAsync(AValidRequest()).Result;
        validationResult.Errors.Should().BeEmpty();
    }
    
    [Fact]
    public async Task EmailCannotBeEmptyOrnull()
    {
        var validator = new NewUserRequestValidator();

        var newUserRequest = AValidRequest() with{Email = null};
        var validationResult =  validator.TestValidateAsync(newUserRequest).Result;
        validationResult.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(Messages.RequiredValue);
        
        newUserRequest = AValidRequest() with{Email = ""};
        validationResult =  validator.TestValidateAsync(newUserRequest).Result;
        validationResult.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(Messages.RequiredValue);
    }
    
    [Fact]
    public async Task NameCannotBeEmptyOrnull()
    {
        var validator = new NewUserRequestValidator();

        var newUserRequest = AValidRequest() with{Name = null};
        var validationResult =  validator.TestValidateAsync(newUserRequest).Result;
        validationResult.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage(Messages.RequiredValue);;
        
        newUserRequest = AValidRequest() with{Name = ""};
        validationResult =  validator.TestValidateAsync(newUserRequest).Result;
        validationResult.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage(Messages.RequiredValue);;
    }
    
    private NewUserRequest AValidRequest() => 
        new("tony", "tony@gmail.com", "male", "active");
}