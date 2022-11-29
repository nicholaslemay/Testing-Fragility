using FluentAssertions;
using FluentAssertions.Execution;
using TestingFragility.Time;
using Xunit;

namespace TestProject;

public class ReceiptCalculatorTest
{
    private readonly ReceiptCalculator _sut = new();

    [Fact]
    public void ProducesReceiptForPurchase_MadeInQuebec()
    {
        var quebecPurchase = new Purchase(10.0m, province: Province.QC);
        var receipt = _sut.ProduceReceipt(quebecPurchase);

        using (new AssertionScope())
        {
            receipt.AmountBeforeTaxes.Should().Be(10.0m);
            receipt.ProvincialTaxes.Should().Be(1.0m);
            receipt.FederalTaxes.Should().Be(0.5m);
            receipt.TotalAmount.Should().Be(11.5m);
        }
    }
}