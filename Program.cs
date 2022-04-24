using challenge_FT.Business;
using System;
using System.Linq;

namespace challenge_FT
{
    class Program
    {
        private static readonly int integer_Two = 2;
        
        static void Main(string[] args)
        { 
            Console.WriteLine("Enter stock's current pricipal value");
            var currentPrincipal = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Enter interest rate's value [1..99]");
            var interestRate = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Enter years to grow [1..99]");
            var yearsToGrow = int.Parse(Console.ReadLine());

            var portfolio = new PortfolioBusiness();
            var responseAddStock = portfolio.AddStock(currentPrincipal, yearsToGrow, interestRate);
            if (responseAddStock.Success)
            {
                Console.WriteLine("Increase list are");
                portfolio.Calculate();
                var stockList = portfolio.GetPortfolioList();

                foreach (var stock in stockList)
                {
                    Console.WriteLine($"Amount Interest: {Math.Round(stock.AmountInterest, integer_Two)} " +
                        $"Amount: {Math.Round(stock.AmountOriginal)} Interest rate: {Math.Round(stock.InterestRate, integer_Two)}: " +
                        $"Date: {stock.Date.ToShortDateString()} ");
                }

                Console.WriteLine("Enter 2 dates to know the profit of the Portfolio\nEnter Date Start (yyyy-mm-dd):  ");
                var dateStartString = Console.ReadLine();
                if (DateTime.TryParse(dateStartString, out DateTime dateStartDate))
                {
                    Console.WriteLine("Enter Date end (yyyy-mm-dd):  ");
                    var dateEndString = Console.ReadLine();
                    if (DateTime.TryParse(dateEndString, out DateTime dateEndSDate))
                    {
                        var portfolioResponse = portfolio.GetProfit(dateStartDate, dateEndSDate);
                        if (portfolioResponse.Success)
                        {
                            Console.WriteLine("\n\n\t ***Profit of the Portfolio is: {0} between {1} and {2} ", portfolioResponse.Amount, dateStartDate.ToShortDateString(), dateEndSDate.ToShortDateString());
                        }
                        else
                        {
                            Console.WriteLine("Occurred the next errors: {0}", string.Join(",", portfolioResponse.ErrorList.Select(err => err.Message)));
                        }

                        Console.WriteLine("\n\nEnter a date specific (yyyy-mm-dd) to know its price:  ");
                        var dateSpecificString = Console.ReadLine();
                        if (DateTime.TryParse(dateSpecificString, out DateTime dateSpecificDate))
                        {
                            var priceResponse = portfolio.GetPrice(dateSpecificDate);
                            if (priceResponse.Success)
                            {
                                Console.WriteLine("\n\n\t ***Price is {0} to:{1}", priceResponse.Amount, dateSpecificDate.ToShortDateString());
                            }
                            else
                            {
                                Console.WriteLine("Occurred the next errors: {0}", string.Join(",", priceResponse.ErrorList.Select(err => err.Message)));
                            }
                        }
                        else
                        {
                            Console.WriteLine("The parameter format is wrong: {0}", dateSpecificString);
                        }

                    }
                    else
                    {
                        Console.WriteLine("The parameter format is wrong: {0}", dateEndString);
                    }

                }
                else
                {
                    Console.WriteLine("The parameter format is wrong: {0}", dateStartString);
                }

            }
            else
            {
                Console.WriteLine("The paramts are any error such as: {0}", string.Join(",", responseAddStock.ErrorList.Select(err => err.Message)));
            }

            Console.ReadKey();
        }
    }
}
