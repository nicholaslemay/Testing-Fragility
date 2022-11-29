namespace TestingFragility.Time;

public class TaxCalculatorMockable
{
    public virtual Decimal ProvincialTaxes(Purchase p) => p.province switch
    {
        Province.QC => AsAmount(p.AmountBeforeTaxes * 0.09975m),
        Province.Ont => AsAmount(p.AmountBeforeTaxes * 0.08m),
        Province.BC => AsAmount(p.AmountBeforeTaxes * 0.07m),
        _ => 0
    };

    public virtual decimal FederalTaxes(Purchase p) => AsAmount(p.AmountBeforeTaxes * 0.05m);
    private static decimal AsAmount(decimal amount) => decimal.Round(amount, 2);
}

public class ReceiptCalculatorMockable
{
    private readonly TaxCalculatorMockable _taxCalculatorMockable = new();

    public Receipt ProduceReceipt(Purchase p)
    {
        var provincialTaxes = ProvincialTaxes(p);
        var federalTaxes = FederalTaxes(p);
        var total = p.AmountBeforeTaxes + provincialTaxes + federalTaxes;
        return new Receipt(p.AmountBeforeTaxes, provincialTaxes, federalTaxes, total);
    }

    public virtual Decimal ProvincialTaxes(Purchase p)
    {
        return _taxCalculatorMockable.ProvincialTaxes(p);
    }

    public virtual decimal FederalTaxes(Purchase p)
    {
        return _taxCalculatorMockable.FederalTaxes(p);
    }
}