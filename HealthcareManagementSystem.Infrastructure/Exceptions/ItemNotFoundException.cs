using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Infrastructure.Exceptions
{
    [Serializable]
    public class ItemNotFoundException : ValidateModelException
    {
        public ItemNotFoundException()
        {
        }

        public ItemNotFoundException(string message) : base(message)
        {
        }

        public ItemNotFoundException(Dictionary<string, string> errors) : base(errors)
        {
        }

        public ItemNotFoundException(
            string key,
            string error)
            : base(new Dictionary<string, string>())
        {
            Errors.Add(key, error);
        }

        public ItemNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                var builder = new StringBuilder();
                builder.AppendLine(base.Message);

                foreach (var error in Errors)
                {
                    builder.AppendLine($"{error.Key}: {string.Join(". ", error.Value)}.");
                }

                return builder.ToString();
            }
        }
    }
}
