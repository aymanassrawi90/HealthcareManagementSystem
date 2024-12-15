using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain.DataAnnotations
{
    //
    // Summary:
    //     Provides a way for an object to be validated.
    public interface IValidatableObject
    {
        //
        // Summary:
        //     Determines whether the specified object is valid.
        //
        // Parameters:
        //   validationContext:
        //     The validation context.
        //
        // Returns:
        //     A collection that holds failed-validation information.
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}
