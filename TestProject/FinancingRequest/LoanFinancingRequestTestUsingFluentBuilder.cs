using Xunit;
using static TestProject.Builders.LoanFinancingRequestBuilderFluent;

namespace TestProject.Builders;

public class LoanFinancingRequestTestUsingFluentBuilder
{
    [Fact]
    public void SomeTest()
    {
        GivenALoanFinancingRequest().WithYearInBusiness(5).Build();

        GivenALoanFinancingRequest().ForALargeAccount().Build();
        
        GivenALoanFinancingRequest().ForALargeAccount().WithYearInBusiness(1).Build();
    }
    
    [Fact]
    public void SomeDatabaseTest()
    {
        GivenALoanFinancingRequest().WithYearInBusiness(5).StoredInDB();
        
        //repository.Get
        //Assert.
    }
    
    [Fact]
    public void SomeWiureMockTest()
    {
        GivenALoanFinancingRequest().WithYearInBusiness(5).StoredInDB();
        
        //repository.Get
        //Assert.
    }
}