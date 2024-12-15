using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Infrastructure.Exceptions
{
    [Serializable]
    public class ConflictException : ValidateModelException
    {
        public ConflictException()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public ConflictException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }

        public ConflictException(Dictionary<string, string> errors)
            : base(errors)
        {
            Errors = errors;
        }
    }
}
