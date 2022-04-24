using challenge_FT.Models;
using challenge_FT.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace challenge_FT.Business
{
    public class PortfolioBusiness
    {
        protected decimal interestRate;
        protected int yearsToGrow;
        protected decimal currentPrincipal;
        protected decimal currentPrincipalWithInterest;
        protected List<Stock> stockList;

        private readonly int integer_One;
        private readonly int integer_Two;
        private readonly int integer_Twelve;
        private readonly int integer_OneHundred;
        private readonly int integer_OneMillion;


        public PortfolioBusiness()
        {
            stockList = new List<Stock>();
            integer_One = 1;
            integer_Two = 2;
            integer_Twelve = 12;
            integer_OneHundred = 100;
            integer_OneMillion = 1000000;
        }

        public PortfolioResponseDTO AddStock(decimal currentPrincipal, int yearsToGrow, decimal interestRate)
        {
            var response = new PortfolioResponseDTO { ErrorList = new List<Error>() };
            response = ValidateInitialParams(currentPrincipal, yearsToGrow, interestRate);
            if (response.Success)
            {
                this.interestRate = (interestRate / integer_OneHundred);
                this.yearsToGrow = yearsToGrow;
                this.currentPrincipal = currentPrincipal;
            }
            
            response.Success = !response.ErrorList.Any();
            return response;
        }

        public virtual void Calculate()
        {
            var dateCurrent = DateTime.Now;
            var dateEnd = dateCurrent.AddYears(this.yearsToGrow);
            var currentValue = this.currentPrincipal;
            int monthCalculate = integer_One;

            for (int yearCurrent = dateCurrent.Year; yearCurrent <= dateEnd.Year; yearCurrent++)
            {
                var interestRateMontly = (currentValue * interestRate) / integer_Twelve;
                for (int month = integer_One; month <= integer_Twelve; month++)
                {
                    var elemntLast = stockList.LastOrDefault();
                    var stock = new Stock
                    {
                        AmountInterest = elemntLast == null ? interestRateMontly : interestRateMontly + elemntLast.AmountInterest,
                        Date = DateTime.Now.AddMonths(monthCalculate),
                        InterestRate = interestRateMontly,
                        AmountOriginal = currentValue,
                    };

                    stockList.Add(stock);
                    monthCalculate++;
                }

                var AmountInterestYear = stockList.Sum(s => s.InterestRate);
                currentValue = currentPrincipal + AmountInterestYear;

                if (yearCurrent + integer_One == dateEnd.Year)
                {
                    currentPrincipalWithInterest = currentValue;
                    break;
                }
            }

        }

        private PortfolioResponseDTO ValidateInitialParams(decimal currentPrincipal, int yearsToGrow, decimal interestRate)
        {
            var response = new PortfolioResponseDTO { ErrorList = new List<Error>() };
            if (interestRate == default || (interestRate / integer_OneHundred) > integer_One)
            {
                response.ErrorList.Add(new Error { Code = ErrorCode.GET_ERROR_INTERESTRATE, Message = ErrorMessage.GET_ERROR_INTERESTRATE });
            }
            if (yearsToGrow == default && yearsToGrow > integer_OneHundred)
            {
                response.ErrorList.Add(new Error { Code = ErrorCode.GET_ERROR_YEARSTOGROW, Message = ErrorMessage.GET_ERROR_YEARSTOGROW });
            }
            if (currentPrincipal == default && currentPrincipal > integer_OneMillion)
            {
                response.ErrorList.Add(new Error { Code = ErrorCode.GET_ERROR_CURRENTPRINCIPAL, Message = ErrorMessage.GET_ERROR_CURRENTPRINCIPAL });
            }

            response.Success = !response.ErrorList.Any();
            return response;
        }

        private PortfolioResponseDTO ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            var response = new PortfolioResponseDTO { ErrorList = new List<Error>() };

            if (startDate > endDate)
            {
                response.ErrorList.Add(new Error { Code = ErrorCode.GET_ERROR_PERIODSTART_GREATER_THAN_PERIODEND, Message = ErrorMessage.GET_ERROR_PERIODSTART_GREATER_THAN_PERIODEND });
            }

            if (startDate <= DateTime.Now.AddMonths(1))
            {
                response.ErrorList.Add(new Error { Code = ErrorCode.GET_ERROR_PERIODSTART, Message = ErrorMessage.GET_ERROR_PERIODSTART });
            }

            if (endDate >= DateTime.Now.AddYears(yearsToGrow))
            {
                response.ErrorList.Add(new Error { Code = ErrorCode.GET_ERROR_PERIODEND, Message = ErrorMessage.GET_ERROR_PERIODEND });
            }

            response.Success = !response.ErrorList.Any();
            return response;
        }

        public PortfolioResponseDTO GetProfit(DateTime startDate, DateTime endDate)
        {
            var response = new PortfolioResponseDTO { ErrorList = new List<Error>() };
            response = ValidateDateRange(startDate, endDate);
            if (response.Success)
            {
                var elementFirst = stockList.FirstOrDefault(s => s.Date.Month == startDate.Month && s.Date.Year == startDate.Year);
                var elementLast = stockList.FirstOrDefault(s => s.Date.Month == endDate.Month && s.Date.Year == endDate.Year);
                var profit = elementLast?.AmountInterest - elementFirst?.AmountInterest;
                response.Amount = Math.Round(profit.Value, integer_Two);
            }

            response.Success = !response.ErrorList.Any();
            return response;
        }

        public PortfolioResponseDTO GetPrice(DateTime date)
        {
            var response = new PortfolioResponseDTO { ErrorList = new List<Error>() };
            response = ValidateDateRange(date, DateTime.Now.AddYears(yearsToGrow).AddMonths(-integer_One));

            if (response.Success)
            {
                var filter = stockList.FirstOrDefault(s => s.Date.Month == date.Month && s.Date.Year == date.Year);
                response.Amount = filter != null
                    ? Math.Round(filter.AmountInterest + currentPrincipal, integer_Two)
                    : default;
            }
            response.Success = !response.ErrorList.Any();
            return response;
        }

        public List<Stock> GetPortfolioList()
        {
            return stockList;
        }
    }
}
