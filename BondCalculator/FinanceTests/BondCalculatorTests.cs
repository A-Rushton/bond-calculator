using Xunit;
using Finance;

namespace FinanceTests
{
    public class BondCalculatorTests
    {
        [Fact]
        public void CalculateYieldToMaturity_WhenPriceEqualsFaceValue_ShouldReturnCouponRate()
        {
            // If the bond price = face value, YTM ≈ coupon rate
            double faceValue = 100;
            double couponRate = 0.05;
            int years = 10;
            double marketPrice = 100;

            double result = BondCalculator.CalculateYieldToMaturity(faceValue, couponRate, years, marketPrice);

            Assert.InRange(result, 0.049, 0.051); // approximately 5%
        }

        [Fact]
        public void CalculateYieldToMaturity_WhenPriceBelowFaceValue_ShouldReturnHigherYield()
        {
            double result = BondCalculator.CalculateYieldToMaturity(100, 0.05, 10, 90);

            // Price below par → yield > coupon rate
            Assert.True(result > 0.05);
        }

        [Fact]
        public void CalculateYieldToMaturity_WhenPriceAboveFaceValue_ShouldReturnLowerYield()
        {
            double result = BondCalculator.CalculateYieldToMaturity(100, 0.05, 10, 110);

            // Price above par → yield < coupon rate
            Assert.True(result < 0.05);
        }
    }
}