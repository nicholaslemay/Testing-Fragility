namespace TestingFragility.Time;

public interface ITaxCalculator
{
    public Decimal ProvincialTaxes(Purchase p);

    public decimal FederalTaxes(Purchase p);
}

public class TaxCalculator : ITaxCalculator
{
    public Decimal ProvincialTaxes(Purchase p) => p.province switch
    {
        Province.QC => AsAmount(p.AmountBeforeTaxes * 0.09975m),
        Province.Ont => AsAmount(p.AmountBeforeTaxes * 0.08m),
        Province.BC => AsAmount(p.AmountBeforeTaxes * 0.07m),
        _ => 0
    };


    public decimal FederalTaxes(Purchase p) => AsAmount(p.AmountBeforeTaxes * 0.05m);

    private static decimal AsAmount(decimal amount) => decimal.Round(amount, 2);
}

public class ReceiptCalculatorDI
{
    private readonly ITaxCalculator _taxCalculator;

    public ReceiptCalculatorDI(ITaxCalculator taxCalculator) => _taxCalculator = taxCalculator;
    
    public Receipt ProduceReceipt(Purchase p)
    {
        var provincialTaxes = _taxCalculator.ProvincialTaxes(p);
        var federalTaxes = _taxCalculator.FederalTaxes(p);
        var total = p.AmountBeforeTaxes + provincialTaxes + federalTaxes;
        return new Receipt(p.AmountBeforeTaxes, provincialTaxes, federalTaxes, total);
    }
    
}