using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dependency_Injection___Containers
{
    public interface IPasswordValidator
    {
        bool PasswordValidate(string password);
    }

    public class PasswordValidator : IPasswordValidator
    {
        string PasswordPattern = @"(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}";

        public bool PasswordValidate(string password)
        {
            Match passwordmatchresult = Regex.Match(password, PasswordPattern);

            if (passwordmatchresult.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
