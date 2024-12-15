using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Infrastructure.Attributes
{
    public class PropertyEqualityComparer<TObject, TProperty> : EqualityComparer<TObject>
    {
        readonly Func<TObject, TProperty> _getter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEqualityComparer&lt;T, TP&gt;"/> class.
        /// </summary>
        /// <param name="getter">The getter.</param>
        public PropertyEqualityComparer(Func<TObject, TProperty> getter)
        {
            this._getter = getter ?? throw new ArgumentNullException(nameof(getter));
        }

        /// <summary>
        /// Determines whether two objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public override bool Equals(TObject x, TObject y)
        {
            return Equals(_getter(x), _getter(y));
        }

        /// <summary>
        /// Serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        /// </exception>
        public override int GetHashCode(TObject obj)
        {
            return _getter(obj).GetHashCode();
        }
    }
}
