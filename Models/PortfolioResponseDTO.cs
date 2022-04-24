using System.Collections.Generic;

namespace challenge_FT.Models
{
    public class PortfolioResponseDTO
    {
        public bool Success { set; get; }
        public decimal Amount { set; get; }
        public List<Error> ErrorList { set; get; }
    }
}
