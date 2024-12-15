using System.Runtime.Serialization;

namespace HealthcareManagementSystem.Infrastructure.Exceptions
{
    [Serializable]
    public class ValidateModelException : System.Exception
    {
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
        public ValidateModelException()
        {
            // Empty constructor required to compile.
        }

        public ValidateModelException(string message)
            : base(message)
        {
        }

        public ValidateModelException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public ValidateModelException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }

        protected ValidateModelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Reset the property value using the GetValue method.
        }

    
       

        public string GetErrorString(
            string key)
        {
            Errors.TryGetValue(key, out var errorString);
            return errorString;
        }

        public void Add(
            string key,
            string value)
        {
            Errors.Add(key, value);
        }

    }
}
