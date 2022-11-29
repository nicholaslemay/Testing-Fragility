using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using TestingFragility.Time;
using Xunit;

namespace TestProject;

public class ReceiptCalculatorMockableTest
{
    private readonly ReceiptCalculatorMockable _sut = Substitute.ForPartsOf<ReceiptCalculatorMockable>();

    [Fact]
    public void ProducesReceiptForPurchase_MadeInQuebec()
    {
        var quebecPurchase = new Purchase(10.0m, province: Province.QC);
       
        _sut.FederalTaxes(quebecPurchase).Returns(25.0m);
        _sut.ProvincialTaxes(quebecPurchase).Returns(15.0m);
        
        var receipt = _sut.ProduceReceipt(quebecPurchase);

        using (new AssertionScope())
        {
            receipt.AmountBeforeTaxes.Should().Be(10.0m);
            receipt.ProvincialTaxes.Should().Be(15.0m);
            receipt.FederalTaxes.Should().Be(25m);
            receipt.TotalAmount.Should().Be(10m+15m+25m);
        }
    }
}