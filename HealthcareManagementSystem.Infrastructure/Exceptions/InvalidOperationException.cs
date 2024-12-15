using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Infrastructure.Exceptions
{
    [Serializable]
    public class InvalidOperationException : ValidateModelException
    {
        public InvalidOperationException()
        {
        }

        public InvalidOperationException(string message) : base(message)
        {
        }

        public InvalidOperationException(Dictionary<string, string> errors) : base(errors)
        {
        }

        public InvalidOperationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidOperationException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }
    }
}
