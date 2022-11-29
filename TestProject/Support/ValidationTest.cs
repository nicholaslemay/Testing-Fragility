using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BFF.Support;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BFF.Tests.Support;

public abstract class ValidationTest<TValidator, TValidated> where TValidator : IValidator<TValidated>, new() where TValidated : class
{
    private static TValidator Validator() => new();
    protected abstract TValidated ValidExample();

    protected void ValidExempleHasNoError()
    {
        var result = Validator().Validate(ValidExample());
        result.Errors.Should().BeEmpty("Exemple valide devrait etre sans erreur de validation");
    }

    protected void ValidateFieldCannotBeNullOrEmpty(Expression<Func<TValidated, object?>> expression)
    {
        ValidateValueIsInvalidForField(expression, null, Messages.RequiredValue);
    }    
    
    protected void ValidateValuesAreValidForType<TProperty>(Expression<Func<TValidated, TProperty>> expression, IEnumerable<TProperty?> validValues)
    {
        foreach (var validValue in validValues)
        {
            ValidateValueValidForField(expression, validValue);
        }
       
    }

    private void ValidateValueValidForField<TProperty>(Expression<Func<TValidated, TProperty>> expression, TProperty? value)
    {
        var exampleToValidate = ValidExample();
        try
        {
            WhenValidating(expression, exampleToValidate, value).Errors.Should().BeEmpty();
        }
        catch (ValidationTestException e)
        {
            ThrowFormattedException(value, expression, e);
        }
    }
    protected void ValidateValueIsInvalidForField<TProperty>(Expression<Func<TValidated, TProperty>> expression, TProperty? value, string expectedErrorMessage)
    {
        var exampleToValidate = ValidExample();
        try
        {
            WhenValidating(expression, exampleToValidate, value).ShouldHaveValidationErrorFor(expression).WithErrorMessage(expectedErrorMessage);
        }
        catch (ValidationTestException e)
        {
            ThrowFormattedException(value, expression, e);
        }
    }

    private static TestValidationResult<TValidated> WhenValidating(LambdaExpression expression, TValidated exampleToValidate, object? value)
    {
        var nameOfTheField = NameOfTheFieldFrom(expression);
        SetProperty(nameOfTheField, exampleToValidate, value);
        return Validator().TestValidateAsync(exampleToValidate).Result;
    }

    private static void ThrowFormattedException(object? value, LambdaExpression expression, ValidationTestException e)
    {
        var formattedValue = value?.ToString() ?? "null";
        if (formattedValue == "")
        {
            formattedValue = "<empty string>";
        }

        var message = $"When setting '{NameOfTheFieldFrom(expression)}' to {formattedValue} :\n" + e.Message;
        throw new ValidationTestException(message, e.Errors);
    }

    private static string NameOfTheFieldFrom(LambdaExpression expression)
    {
        //exemple d=> d.Form.AFieldName, Object) -> Form.AFieldName
        var value = expression.Body.ToString();
            
        var indexOfFirstDot = value.IndexOf(".", StringComparison.InvariantCultureIgnoreCase);
        var indexOfSeparator = value.IndexOf(",",StringComparison.InvariantCultureIgnoreCase);

        var containsASeparator = indexOfSeparator != -1;

        if (containsASeparator)
            return value.Substring(indexOfFirstDot + 1, indexOfSeparator - (indexOfFirstDot + 1));
            
        return value.Substring(indexOfFirstDot + 1);
    }

    private static void SetProperty(string compoundProperty, object target, object? value)
    {
        var bits = compoundProperty.Split('.');
        for (var i = 0; i < bits.Length - 1; i++)
        {
            var propertyToGet = target!.GetType().GetProperty(bits[i]);
            if(propertyToGet == null)
                throw new Exception($"{bits[i]} de {compoundProperty} n'est pas définie");
            target = propertyToGet.GetValue(target, null);
        }
        var propertyToSet = target!.GetType().GetProperty(bits.Last());
        if(propertyToSet == null)
            throw new Exception($"{bits.Last()} de {compoundProperty} n'est pas définie");
            
        propertyToSet.SetValue(target, value, null);
    }
    
}