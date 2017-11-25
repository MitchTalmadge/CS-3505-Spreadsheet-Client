using System.Collections.Generic;

namespace Properties
{
    /// <summary>
    /// This interface is used to allow serialization into and from a PropertiesFile.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public interface IPropertySerializable
    {
        /// <summary>
        /// Converts this object to properties, to be written to a PropertiesFile.
        /// </summary>
        /// <returns>All Properties that represent this object.</returns>
        IEnumerable<Property> ToProperties();

        /// <summary>
        /// Gives properties which should be used to populate this object.
        /// </summary>
        /// <param name="properties">The properties from a previously serialized object.</param>
        void FromProperties(IEnumerable<Property> properties);
    }
}