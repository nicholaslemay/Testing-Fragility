using System.Diagnostics;

namespace TestingFragility.Time;

public record Receipt(decimal AmountBeforeTaxes, decimal ProvincialTaxes, decimal FederalTaxes, decimal TotalAmount);

public record Purchase(decimal AmountBeforeTaxes, Province province);

public enum Province
{
    QC,
    Ont,
    BC
}