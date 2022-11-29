namespace TestProject.Builders;

public class LoanFinancingRequestProcessor
{
    public FinancingOffer Process(LoanFinancingRequest request)
    {
        return new FinancingOffer
        {
            IsApproved = request.AnnualRevenues != AnnualRevenues.LessThan100k
        };
    }

    public class FinancingOffer
    {
        public bool IsApproved { get; set; } = true;
    }
}