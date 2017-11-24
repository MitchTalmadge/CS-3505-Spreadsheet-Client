using System.Collections.Generic;

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
        /// Creates a property with the given attributes and value.
        /// </summary>
        /// <param name="key">The key of the property.</param>
        /// <param name="value">The value of the property.</param>
        /// <param name="attributes">The attributes to store in the property.</param>
        public Property(string key, string value = null, Dictionary<string, string> attributes = null)
        {
            Key = key;
            Value = value ?? "";
            Attributes = attributes ?? new Dictionary<string, string>();
        }

    }
}
