using HealthcareManagementSystem.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.ConsoleApp
{
    public  class Helper: IHelper
    {
        public  bool ConfirmExit()
        {
            Console.Write("Are you sure you want to exit? (Y/N): ");
            var input = Console.ReadLine()?.Trim().ToUpper();
            return input != "Y";
        }

        public  void DisplayValidationErrors(ValidateModelException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var error in ex.Errors)
            {
                Console.WriteLine($"Field: {error.Key}, Error: {error.Value}");
            }
            Console.ResetColor();
        }

        public  int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out var result))
                {
                    return result;
                }
                
                DisplayError("Invalid input. Please enter a valid number.");
              
            }
        }


        public  void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public  void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }


        public  string ReadNonEmptyInput(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim()??string.Empty;
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        public  int ReadIntOptional(string prompt, int currentValue)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            return int.TryParse(input, out var result) ? result : currentValue;
        }



        public  string ReadOptionalInput(string prompt, string currentValue = null)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? currentValue : input.Trim();
        }


        public  string ReadGender(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine()?.Trim()?.ToUpper() ?? string.Empty;
                if (input == "M" || input == "F")
                {
                    return input;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please enter 'M' or 'F'.");
                Console.ResetColor();
            }
        }

        public string ReadPhone(string prompt, string currentValue = "")
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine()?.Trim()?.ToUpper() ?? currentValue;
                input = (string.IsNullOrEmpty(input) ? currentValue : input).Trim();
                string pattern = @"^(0\d{9}|(?:\+|00)\d{3}\d{9})$";
                var isValid = Regex.IsMatch(input, pattern);
                if (isValid)
                {
                    return input;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Phone No. Please enter  Valid No (0xxxxxxxx | 00xxxxxxxxxxx | +xxxxxxxxxxx).");
                Console.ResetColor();
            }
        }

    }
}
