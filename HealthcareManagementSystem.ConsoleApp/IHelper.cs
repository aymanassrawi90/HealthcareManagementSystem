using HealthcareManagementSystem.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.ConsoleApp
{
    public interface IHelper
    {
        string ReadNonEmptyInput(string prompt);
        int ReadInt(string prompt);
        string ReadGender(string prompt);
        void DisplayError(string message);
        void DisplaySuccess(string message);
        void DisplayValidationErrors(ValidateModelException ex);
        bool ConfirmExit();
        string ReadOptionalInput(string prompt, string currentValue = null);
        int ReadIntOptional(string prompt, int currentValue);

        string ReadPhone(string prompt, string currentValue = "");
    }


}
