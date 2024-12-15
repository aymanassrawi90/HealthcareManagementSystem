using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.ConsoleApp
{
    public static class DiagnosisHelper
    {
        public static async Task<List<Diagnosis>> GetDiagnosesFromUserInput(IDiagnosisService _diagnosisService, IHelper helper)
        {
            var diagnosesList = (await _diagnosisService.GetAllDiagnosesAsync() ?? Enumerable.Empty<Diagnosis>()).ToList();
           if(diagnosesList!=null && diagnosesList.Count > 0)
            {
               helper.DisplaySuccess("\nAvailable Diagnoses:");
            }
            int index = 1;
            foreach (var diagnosis in diagnosesList)
            {
                Console.WriteLine($"{index++} - {diagnosis.Name} ({diagnosis.Severity})");
            }

            var selectedDiagnoses = new List<Diagnosis>();
            var newDiagnoses = new List<Diagnosis>();

            // Prompt the user until they type 'done' to stop selecting diagnoses
            var message = (2 != null && diagnosesList.Count > 0) ? "Enter the number of the diagnosis to assign, or type 'new' to add a new diagnosis, or type 'done' to finish:" : "No Available Diagnoses on system Enter new' to add a new diagnosis, or type 'done' to finish:";

            while (true)
            {
                var input = helper.ReadOptionalInput(message, "done");

                if (input?.ToLower() == "done")
                {
                    break; // Exit the loop when the user is done
                }

                if (input?.ToLower() == "new")
                {
                    // Add a new diagnosis
                    var name = helper.ReadNonEmptyInput("Enter Diagnosis Name: ");
                    var severity = helper.ReadNonEmptyInput("Enter Diagnosis Severity: ");
                    newDiagnoses.Add(new Diagnosis(name, severity));
                }
                else
                {
                    // Try to parse the input as a diagnosis number
                    if (int.TryParse(input, out int indexInput) && indexInput > 0 && indexInput <= diagnosesList.Count)
                    {
                        selectedDiagnoses.Add(diagnosesList[indexInput - 1]);
                    }
                    else
                    {
                        helper.DisplayError("Invalid selection, please enter a valid diagnosis number or 'new' to add a new one.");
                    }
                }
            }

            // If new diagnoses are added, create them and check for duplicates
            if (newDiagnoses.Any())
            {
                var result = await _diagnosisService.CreateDiagnosesAsync(newDiagnoses);
                selectedDiagnoses.AddRange(newDiagnoses);

                if (result?.Any() == true)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("The following diagnoses already exist and were skipped:");
                    foreach (var diagnosis in result)
                    {
                        Console.WriteLine(diagnosis.ToString());
                    }
                    Console.ResetColor();
                }
            }

            return selectedDiagnoses;
        }
    }
}
