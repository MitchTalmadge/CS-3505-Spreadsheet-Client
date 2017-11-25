using System.Collections.Generic;
using System.Linq;

namespace Properties
{
    /// <summary>
    /// Represents a single property in a properties file.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// The key of the property.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The value of the property.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The attributes of the property.
        /// </summary>
        public Dictionary<string, string> Attributes { get; }

        /// <summary>
        /// A comment to display above the property in the properties file.
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Creates a property with the given attributes and value.
        /// </summary>
        /// <param name="key">The key of the property.</param>
        /// <param name="value">The value of the property.</param>
        /// <param name="attributes">The attributes to store in the property.</param>
        /// <param name="comment">A comment to display about the property in the properties file.</param>
        public Property(string key, string value = null, Dictionary<string, string> attributes = null, string comment = null)
        {
            Key = key;
            Value = value ?? "";
            Attributes = attributes ?? new Dictionary<string, string>();
            Comment = comment;
        }

        protected bool Equals(Property other)
        {
            return string.Equals(Key, other.Key) && string.Equals(Value, other.Value) &&
                   Attributes.Count == other.Attributes.Count && !Attributes.Except(other.Attributes).Any();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Property) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Attributes != null ? Attributes.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}