using System;
using System.Collections.Generic;
using System.Text;

namespace challenge_FT.Models.Constants
{
    public static class ErrorMessage
    {
        public const string GET_ERROR_PERIODSTART_GREATER_THAN_PERIODEND = "The period initial is greater than period end";
        public const string GET_ERROR_PERIODSTART = "The period initial must greater than one month from today";
        public const string GET_ERROR_PERIODEND = "The period end mustn't greater than years to grow";
        public const string GET_ERROR_INTERESTRATE = "The interest rate mustn't one value greater than 100 and less to 1";
        public const string GET_ERROR_YEARSTOGROW = "The years to grow mustn't one value greater than 100 and less to 1";
        public const string GET_ERROR_CURRENTPRINCIPAL = "The current principal mustn't one value greater than 1,000,000 and less to 1";
    }
}
