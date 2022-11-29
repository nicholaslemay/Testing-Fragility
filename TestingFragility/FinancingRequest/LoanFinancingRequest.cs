namespace TestProject.Builders;

public record LoanFinancingRequest(int YearsInBusiness, 
    AnnualRevenues AnnualRevenues, 
    string CompanieName, 
    CompanySize CompanySize
    //,Sector Sector
    );

public enum CompanySize
{
    small,
    medium,
    large
}

public enum AnnualRevenues
{
    LessThan100k,
    Between100kand250k,
    Between250kand1million,
    OneMillionOrMore
}

public enum Sector
{
    technologies,
    lumber,
    other
}