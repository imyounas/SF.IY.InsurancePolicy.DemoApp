using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Common
{
    public static class Constants
    {
        public const int MAX_RETAINED_MQ_CONNECTIONS = 5;

        public const string LICENSE_REGEX = @"^[A-Z](?:\d[- ]*){14}$";

        public const string INVALID_EFFECTIVE_DATE = "E101";
        public const string INVALID_VEHICLE_REG_YEAR = "E102";
        public const string INVALID_LICENSE_NUMBER = "E103";
        public const string INVALID_USA_ADDRESS = "E104";
        public const string INVALID_FIRSTNAME = "E105";
        public const string INVALID_LASTNAME = "E106";
        public const string INVALID_EXPIRATION_DATE = "E107";
        public const string INVALID_PREMIUM_PRICE = "E108";

        public static readonly Dictionary<string, string> ErrorCodeMessages;
        static Constants()
        {
            ErrorCodeMessages = new Dictionary<string, string>();
            ErrorCodeMessages.Add(INVALID_EFFECTIVE_DATE, "Effective Date should be atleast 30 days in future");
            ErrorCodeMessages.Add(INVALID_VEHICLE_REG_YEAR, "Vechile Registeration Date should be before 1998");
            ErrorCodeMessages.Add(INVALID_LICENSE_NUMBER, "Invalid License Number");
            ErrorCodeMessages.Add(INVALID_USA_ADDRESS, "Provided address is not a valid USA Address");
            ErrorCodeMessages.Add(INVALID_FIRSTNAME, "Invalid First Name");
            ErrorCodeMessages.Add(INVALID_LASTNAME, "Invalid Last Name");
            ErrorCodeMessages.Add(INVALID_EXPIRATION_DATE, "Invalid Policy Expiration Date");
            ErrorCodeMessages.Add(INVALID_PREMIUM_PRICE, "Invalid Policy Premium Price");
        }
    }
}
