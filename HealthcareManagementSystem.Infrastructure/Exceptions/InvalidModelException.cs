using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Infrastructure.Exceptions
{
    [Serializable]
    public class InvalidModelException : ValidateModelException
    {
        public InvalidModelException()
        {
        }

        public InvalidModelException(string message) : base(message)
        {
        }

        public InvalidModelException(Dictionary<string, string> errors) : base(errors)
        {
        }

        public InvalidModelException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }

        public InvalidModelException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
