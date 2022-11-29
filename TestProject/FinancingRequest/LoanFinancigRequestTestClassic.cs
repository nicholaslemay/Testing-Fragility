using FluentAssertions;
using Xunit;
using static TestProject.Builders.AnnualRevenues;
using static TestProject.Builders.CompanySize;

namespace TestProject.Builders;

public class LoanFinancigRequestTestClassic
{
    [Fact]
    public void TestSomethingRelativeToYearsInBusiness()
    {
        var request = new LoanFinancingRequest(YearsInBusiness: 33, AnnualRevenues: LessThan100k, CompanieName: "IDontCare", CompanySize: medium);

        var financingOffer = new LoanFinancingRequestProcessor().Process(request);
        financingOffer.IsApproved.Should().BeTrue();
    }
    
    [Fact]
    public void TestSomethingRelativeToAnnualRevenues()
    {
        var request = new LoanFinancingRequest(YearsInBusiness: 33, AnnualRevenues: Between100kand250k, CompanieName: "IDontCare", CompanySize: medium);

        var financingOffer = new LoanFinancingRequestProcessor().Process(request);
        financingOffer.IsApproved.Should().BeTrue();
    }
    
        
    [Fact]
    public void TestSomethingelseRelativeToAnnualRevenues()
    {
        var request = new LoanFinancingRequest(YearsInBusiness: 33, AnnualRevenues: OneMillionOrMore, CompanieName: "IDontCare", CompanySize: medium);

        var financingOffer = new LoanFinancingRequestProcessor().Process(request);
        financingOffer.IsApproved.Should().BeTrue();
    }
    
    [Fact]
    public void TestSAnotherThingRelativeToAnnualRevenues()
    {
        var request = new LoanFinancingRequest(YearsInBusiness: 33, AnnualRevenues: OneMillionOrMore, CompanieName: "IDontCare", CompanySize: medium);

        var financingOffer = new LoanFinancingRequestProcessor().Process(request);
        financingOffer.IsApproved.Should().BeTrue();
    }
}

