using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace POV.Comun.Service
{
    public class ValidatorCtrl
    {
        private List<string> errors;

        private string NUMBER_EXPRESSION = @"^[0-9]+$";

        private string DECIMAL_EXPRESSION = @"^([0-9])|([0-9]{1,5}(\.?[0-9]{0,2}))$";

        private string EMAIL_EXPRESSION = @"^(([^<>()[\]\\.,;:\s@\""]+"
                        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                        + @"[a-zA-Z]{2,}))$";

        public ValidatorCtrl()
        {
            this.errors = new List<string>();
        }


        public string getErrors()
        {
            string myerror = "";

            foreach (string error in errors)
            {
                myerror += error;
            }
            return myerror;
        }
        public bool ValidIntegerParam(string param, string key)
        {
            bool isValid = true;
            param = string.IsNullOrEmpty(param) ? string.Empty : param;
            param = param.Trim();

            isValid = isValid && param.Length > 0;
            Match match = Regex.Match(param, NUMBER_EXPRESSION, RegexOptions.IgnoreCase);

            isValid = isValid && match.Success;
            if (!isValid)
            {
                errors.Add(key + ": Debe ser un número.");
            }

            return isValid;
        }

        public bool ValidDecimalParam(string param, string key)
        {
            bool isValid = true;
            param = string.IsNullOrEmpty(param) ? string.Empty : param;
            param = param.Trim();

            isValid = isValid && param.Length > 0;
            Match match = Regex.Match(param, DECIMAL_EXPRESSION, RegexOptions.IgnoreCase);

            isValid = isValid && match.Success;
            if (!isValid)
            {
                errors.Add(key + ": Debe ser un número con dos decimales.");
            }

            return isValid;
        }

        public bool ValidEmailParam(string param, string key)
        {
            bool isValid = true;

            param = string.IsNullOrEmpty(param) ? string.Empty : param;
            param = param.Trim();

            isValid = isValid && param.Length > 0;
            Match match = Regex.Match(param, EMAIL_EXPRESSION, RegexOptions.IgnoreCase);

            isValid = isValid && match.Success;
            if (!isValid)
            {
                errors.Add(key + ": Debe tener un formato correcto p.e user@mail.com.");
            }

            return isValid;
        }

        public bool ValidLengthParam(string param, string key, int minLength, int maxLength)
        {
            bool isValid = true;

            param = string.IsNullOrEmpty(param) ? string.Empty : param;
            param = param.Trim();

            isValid = isValid && param.Length >= minLength;
            isValid = isValid && param.Length <= maxLength;
            if (!isValid)
            {
                errors.Add(key + ": El largo debe ser entre " + minLength + " y " + maxLength + "." );
            }

            return isValid;
        }


        public List<string> Errors
        {
            get { return this.errors; }
            set {}
        }
    }
}
