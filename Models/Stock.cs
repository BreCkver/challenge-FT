using System;

namespace challenge_FT.Models
{
    public class Stock
    {
        public DateTime Date { get; set; }
        public decimal AmountOriginal { set; get; }

        public decimal AmountInterest { set; get; }

        public decimal InterestRate { set; get; }

    }
}
