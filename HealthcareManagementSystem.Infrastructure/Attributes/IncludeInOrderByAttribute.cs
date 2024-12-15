using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Infrastructure.Attributes
{
    public class IncludeInOrderByAttribute : Attribute
    {
        public string EntityPropertyName { get; set; }

        /// <summary>
        /// use entityPropertyName to specify the porperty name in the entity
        /// </summary>
        /// <param name="entityPropertyName">the name of the property in the entity if different</param>
        /// <param name="propertyName"> the property name</param>
        public IncludeInOrderByAttribute(
            string entityPropertyName = null,
            [CallerMemberName] string propertyName = null)
        {
            EntityPropertyName = entityPropertyName;

            if (entityPropertyName == null)
                EntityPropertyName = propertyName;
        }
    }
}
