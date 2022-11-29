using Xunit;
using static TestProject.Builders.LoanFinancingRequestBuilderUsingRecords;

namespace TestProject.Builders;

public class LoanFinancingRequestTestUsingRecordBuilder
{
    [Fact]
    public void SomeTest()
    {
        var r = AValidLoanFinancingRequest();
        var y = AFinancingRequestForALargeClient();
        var x = AValidLoanFinancingRequest() with { YearsInBusiness = 33 };
    }
    
    
    [Fact]
    public void SomeTestInDB()
    {
        AValidLoanFinancingRequest().StoredInDB();
    }
}