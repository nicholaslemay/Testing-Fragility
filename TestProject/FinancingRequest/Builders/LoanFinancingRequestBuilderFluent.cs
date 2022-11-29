using static TestProject.Builders.AnnualRevenues;
using static TestProject.Builders.CompanySize;

namespace TestProject.Builders;


public class LoanFinancingRequestBuilderUsingRecords
{
    public static LoanFinancingRequest AValidLoanFinancingRequest() => new(5, Between250kand1million, "SomeCieName", medium);
    public static LoanFinancingRequest AFinancingRequestForALargeClient() => new(20, OneMillionOrMore, "SomeLargeCieName", large);
}


public static class LoanFinancingRequestBuilderUsingRecordsExtensions
{
    public static void StoredInDB(this LoanFinancingRequest r)
    {
        //dosomething to save it in db
    }
    
    public static void StoredInCRM(this LoanFinancingRequest r)
    {
        //dosomething to save it in CRM ex: wiremock
    }
}
public class LoanFinancingRequestBuilderFluent
{
    private int _yearsInBusiness = 5;
    private AnnualRevenues _annualRevenues = Between250kand1million;
    private string _name = "SomeCieName";
    private CompanySize _size = medium;

    public static LoanFinancingRequestBuilderFluent GivenALoanFinancingRequest()
    {
        return new LoanFinancingRequestBuilderFluent();
    }

    public LoanFinancingRequest Build() => new(_yearsInBusiness, _annualRevenues, _name, _size);

    public LoanFinancingRequestBuilderFluent WithYearInBusiness(int y) {
        _yearsInBusiness = y;
        return this;
    }
    
    public LoanFinancingRequestBuilderFluent ForALargeAccount() {
        _yearsInBusiness = 20;
        _name = "SomeLargeCie";
        _size = large;
        _annualRevenues = OneMillionOrMore;
        return this;
    }
    
}

public static class LoanFinancingRequestBuilderFluentExtensions
{
    public static void StoredInDB(this LoanFinancingRequestBuilderFluent builder)
    {
        var request = builder.Build();
        //dosomething to save it in db
    }
    
    public static void StoredInCRM(this LoanFinancingRequestBuilderFluent builder)
    {
        var request = builder.Build();
        //dosomething to save it in CRM ex: wiremock
    }
}