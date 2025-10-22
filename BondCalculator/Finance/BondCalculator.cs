using System;

namespace Finance
{
    public static class BondCalculator
    {
        /// <summary>
        /// Calculates the approximate Yield to Maturity (YTM) of a bond.
        /// </summary>
        /// <param name="faceValue">The face (par) value of the bond, e.g. 100.</param>
        /// <param name="couponRate">Annual coupon rate (as a decimal), e.g. 0.05 for 5%.</param>
        /// <param name="yearsToMaturity">Years remaining until maturity.</param>
        /// <param name="marketPrice">The current market price of the bond.</param>
        /// <returns>Estimated yield to maturity as a decimal (e.g. 0.05 for 5%).</returns>
        public static double CalculateYieldToMaturity(double faceValue, double couponRate, int yearsToMaturity, double marketPrice)
        {
            // Annual coupon payment
            double coupon = faceValue * couponRate;

            // Start with an initial guess for YTM (e.g., 5%)
            double ytm = 0.05;
            double tolerance = 1e-6;
            int maxIterations = 1000;

            // Newton-Raphson iteration to find YTM that makes PV of cashflows = price
            for (int i = 0; i < maxIterations; i++)
            {
                double f = 0.0;  // f(ytm)
                double df = 0.0; // f'(ytm)

                for (int t = 1; t <= yearsToMaturity; t++)
                {
                    // Discount factor for year t
                    double discount = Math.Pow(1 + ytm, t);

                    // f(ytm): difference between PV of cashflows and market price
                    f += coupon / discount;

                    // Derivative of f(ytm)
                    df += -t * coupon / (discount * (1 + ytm));
                }

                // Add final payment (face value)
                f += faceValue / Math.Pow(1 + ytm, yearsToMaturity);
                df += -yearsToMaturity * faceValue / (Math.Pow(1 + ytm, yearsToMaturity + 1));

                // Subtract the actual market price
                f -= marketPrice;

                // Newton-Raphson step
                double newYtm = ytm - f / df;

                // Stop if the change is very small
                if (Math.Abs(newYtm - ytm) < tolerance)
                    return newYtm;

                ytm = newYtm;
            }

            // If we didn't converge, return the last guess
            return ytm;
        }
    }
}
