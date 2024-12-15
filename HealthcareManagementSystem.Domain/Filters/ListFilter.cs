using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain.Filters
{
    public class ListFilter : IValidatableObject
    {
        public int? Id { get; set; }
        /// <summary>
        /// Skip first 'skip' user(s)
        /// </summary>
        [Range(0, 9999)]
        public int? Skip { get; set; }

        /// <summary>
        /// Take first 'take' users(s)
        /// </summary>
        [Range(1, 9999)]
        public int? Take { get; set; }

        /// <summary>
        /// Comma separated properties names. Use '+' with column name for ascending and '-' for descending order
        /// example: +col1,-col2,+col3,...
        /// Supported names are: createdOn, createdBy, modifiedOn, modifiedBy
        /// </summary>
        [RegularExpression(@"^[0-9A-Za-z+,\- ]*$", ErrorMessage = "Only letters, numbers and '+', '-', ',' are allowed for order by property.")]
        public virtual string OrderBy { get; set; }
        public T Clone<T>() where T : ListFilter
        {
            return (T)MemberwiseClone();
        }

        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.5/index-modules.html
        /// The maximum value of from + size for searches to this index. Defaults to 10000.
        /// Search requests take heap memory and time proportional to from + size and this limits that memory.
        /// See Scroll or Search After for a more efficient alternative to raising this.
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            const int max = 10000;
            var result = new ValidationResult[1];
            if ((Skip ?? 0) + (Take ?? 0) > max)
            {
                result[0] = new ValidationResult($"Sum of skip and take cannot be greater than {max}", new[] { "Skip", "Take" });
            }

            return result;
        }
    }
}
