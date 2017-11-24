using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Properties
{
    /// <summary>
    /// Reads and writes properties to xml files.
    /// </summary>
    /// <authors>Jiahui Chen, Mitch Talmadge</authors>
    public class PropertiesFile
    {
        /// <summary>
        /// The settings to use when writing xml.
        /// </summary>
        private static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings
        {
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace
        };

        /// <summary>
        /// The settings to use when reading xml.
        /// </summary>
        private static readonly XmlReaderSettings ReaderSettings = new XmlReaderSettings
        {
            IgnoreWhitespace = true,
            IgnoreComments = true
        };

        /// <summary>
        /// The name of the root tag for all properties files.
        /// </summary>
        private const string RootElementName = "properties";

        /// <summary>
        /// The path to this file.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Loads or creates the file at the specified path as a properties file.
        /// </summary>
        /// <param name="filePath">The path to the properties file.</param>
        /// <exception cref="IOException">If the file is invalid or could not be created.</exception>
        public PropertiesFile(string filePath)
        {
            FilePath = filePath;

            if (File.Exists(filePath))
                VerifyExistingPropertiesFile();
            else
                CreatePropertiesFile();
        }

        /// <summary>
        /// Ensures that the file at the file path is really a properties file.
        /// </summary>
        /// <exception cref="IOException">If the file is not a valid properties file.</exception>
        private void VerifyExistingPropertiesFile()
        {
            try
            {
                using (var reader = XmlReader.Create(FilePath, ReaderSettings))
                {
                    reader.MoveToContent();
                    reader.ReadStartElement(RootElementName);
                    reader.ReadEndElement();
                }
            }
            catch (XmlException e)
            {
                throw new IOException("The properties file does not appear to be valid.", e);
            }
        }

        /// <summary>
        /// Creates a new properties file at the file path.
        /// </summary>
        private void CreatePropertiesFile()
        {
            // Write no properties.
            WriteProperties(new Property[0]);
        }

        /// <summary>
        /// Writes the given property to the file.
        /// </summary>
        /// <param name="property">The property to write.</param>
        public void WriteProperty(Property property)
        {
            //TODO: Get all properties, then use WriteProperties to write all properties + new property.
        }

        /// <summary>
        /// Overwrites the properties file contents with the given properties.
        /// </summary>
        /// <param name="properties">The properties to write.</param>
        private void WriteProperties(IEnumerable<Property> properties)
        {
            using (var writer = XmlWriter.Create(FilePath, WriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(RootElementName);

                // Write each property.
                foreach (var property in properties)
                {
                    // Write opening tag
                    writer.WriteStartElement(property.Key);

                    // Write attributes
                    foreach (var attribute in property.Attributes)
                    {
                        writer.WriteAttributeString(attribute.Key, attribute.Value);
                    }

                    // Write value
                    writer.WriteString(property.Value);

                    // Write closing tag
                    writer.WriteEndElement();
                }

                writer.WriteFullEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// Gets all properties that match the given key.
        /// </summary>
        /// <param name="key">The key of the properties to find.</param>
        /// <returns>All properties matching the key.</returns>
        public IEnumerable<Property> GetProperties(string key)
        {
            var properties = new List<Property>();
            using (var reader = XmlReader.Create(FilePath, ReaderSettings))
            {
                reader.MoveToContent();
                reader.ReadStartElement(RootElementName);

                while (reader.NodeType == XmlNodeType.Element)
                {
                    while (reader.ReadToNextSibling(key))
                    {
                        // Get attributes of property.
                        Dictionary<string, string> attributes = null;
                        if (reader.HasAttributes)
                        {
                            attributes = new Dictionary<string, string>();
                            while (reader.MoveToNextAttribute())
                            {
                                attributes.Add(reader.Name, reader.Value);
                            }
                        }

                        // Get value of property.
                        var value = reader.ReadElementContentAsString();

                        // Create property object.
                        properties.Add(new Property(key, value, attributes));
                    }
                }

                reader.ReadEndElement();
            }

            return properties;
        }
    }
}