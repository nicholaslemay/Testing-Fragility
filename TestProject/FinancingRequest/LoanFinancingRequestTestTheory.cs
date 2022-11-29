using FluentAssertions;
using Xunit;
using static TestProject.Builders.AnnualRevenues;

namespace TestProject.Builders;

public class LoanFinancingRequestTestTheory
{
    [Theory]
    [InlineData(LessThan100k, false)]
    [InlineData(Between100kand250k, true)]
    [InlineData(Between250kand1million, true)]
    public void FinancingApprovalBasedOnAnnualRevenues(AnnualRevenues annualRevenues, bool isExpectedToBeApproved)
    {
        ApprovalOf(AValidFinancingRequest() with {AnnualRevenues = annualRevenues})
            .Should().Be(isExpectedToBeApproved);
    }

    private static bool ApprovalOf(LoanFinancingRequest request) => 
        new LoanFinancingRequestProcessor().Process(request).IsApproved;


    private static LoanFinancingRequest AValidFinancingRequest()
    {
        var request = new LoanFinancingRequest(YearsInBusiness: 33, AnnualRevenues: Between100kand250k, CompanieName: "IDontCare", CompanySize: CompanySize.medium);
        return request;
    }
}