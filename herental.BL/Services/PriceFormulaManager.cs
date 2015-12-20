using herental.BL.Interfaces;
using System;
using System.Collections.Generic;

namespace herental.BL.Services
{
    public class PriceFormulaManager : Dictionary<string, Func<int, decimal>>, IPriceFormulaManager
    {
        public PriceFormulaManager()
        {
            Add("Heavy", CalculateHeavy);
            Add("Regular", CalculateRegular);
            Add("Specialized", CalculateSpecialized);
        }

        public static decimal OneTimeRental = 100M;
        public static decimal PremiumDaily = 60M;
        public static decimal RegularDaily = 40M;
        
        public static decimal CalculateHeavy(int period)
        {
            return OneTimeRental + PremiumDaily * period;
        }

        public static decimal CalculateRegular(int period)
        {
            return OneTimeRental + PremiumDaily * Math.Min(period, 2) + RegularDaily * Math.Max(period - 2, 0);
        }

        public static decimal CalculateSpecialized(int period)
        {
            return PremiumDaily * Math.Min(period, 3) + RegularDaily * Math.Max(period - 3, 0);
        }

        public decimal CalculatePrice(string name, int period)
        {
            return this[name](period);
        }
    }
}
