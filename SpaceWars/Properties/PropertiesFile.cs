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
        private static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings()
        {
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace
        };

        /// <summary>
        /// The settings to use when reading xml.
        /// </summary>
        private static readonly XmlReaderSettings ReaderSettings = new XmlReaderSettings()
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
                    reader.ReadStartElement();
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
            using (var writer = XmlWriter.Create(FilePath, WriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(RootElementName);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}