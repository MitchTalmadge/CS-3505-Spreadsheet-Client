using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Properties;

namespace PropertiesTests
{
    /// <summary>
    /// Tests the Properties library.
    /// </summary>
    [TestClass]
    public class PropertiesTests
    {
        /// <summary>
        /// Ensures that a properties file is created.
        /// </summary>
        [TestMethod]
        public void TestCreatePropertiesFile()
        {
            const string fileName = "test_creation.xml";

            // Delete existing file.
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Create new properties.
            var properties = new PropertiesFile(fileName);

            // Check file existence.
            Assert.IsTrue(File.Exists(fileName));
        }

        /// <summary>
        /// Ensures that loading invalid properties files throws an exception.
        /// </summary>
        [TestMethod]
        public void TestLoadInvalidPropertiesFiles()
        {
            Assert.ThrowsException<IOException>(() => new PropertiesFile("invalid_properties.xml"));
            Assert.ThrowsException<IOException>(() => new PropertiesFile("not_properties.txt"));
        }

        /// <summary>
        /// Ensures that loading valid properties files is successful.
        /// </summary>
        [TestMethod]
        public void TestLoadValidPropertiesFiles()
        {
            var properties = new PropertiesFile("valid_properties.xml");

            // Read properties
            var test1Properties = properties.GetPropertiesByKey("test1").ToArray();
            var test2Properties = properties.GetPropertiesByKey("test2").ToArray();

            // Check lengths
            Assert.AreEqual(1, test1Properties.Length);
            Assert.AreEqual(1, test2Properties.Length);
        }

        /// <summary>
        /// Makes sure that properties can be saved and loaded properly when they have a value and attributes.
        /// </summary>
        [TestMethod]
        public void TestAddAndRetrieveProperties()
        {
            const string fileName = "save_load.xml";

            // Delete existing file.
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Create new properties.
            var properties = new PropertiesFile(fileName);

            var attributes1 = new Dictionary<string, string> {["test1Attr"] = "test1AttrValue"};
            var attributes2 = new Dictionary<string, string> {["test2Attr"] = "test2AttrValue"};

            var test1Property = new Property("test1", "test1Value", attributes1);
            var test2Property = new Property("test2", "test2Value", attributes2);

            // Write properties
            properties.WriteProperty(test1Property);
            properties.WriteProperty(test2Property);

            // Read properties
            var test1Properties = properties.GetPropertiesByKey(test1Property.Key).ToArray();
            var test2Properties = properties.GetPropertiesByKey(test2Property.Key).ToArray();

            // Check lengths
            Assert.AreEqual(1, test1Properties.Length);
            Assert.AreEqual(1, test2Properties.Length);

            // Check contents
            Assert.AreEqual(test1Property, test1Properties[0]);
            Assert.AreEqual(test2Property, test2Properties[0]);
        }

        /// <summary>
        /// Makes sure that properties can be saved and loaded properly when they have attributes but no value.
        /// </summary>
        [TestMethod]
        public void TestAddAndRetrievePropertiesWithoutValue()
        {
            const string fileName = "no_values.xml";

            // Delete existing file.
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Create new properties.
            var properties = new PropertiesFile(fileName);

            var attributes1 = new Dictionary<string, string> {["test1Attr"] = "test1AttrValue"};
            var attributes2 = new Dictionary<string, string> {["test2Attr"] = "test2AttrValue"};

            var test1Property = new Property("test1", attributes: attributes1);
            var test2Property = new Property("test2", attributes: attributes2);

            // Write properties
            properties.WriteProperty(test1Property);
            properties.WriteProperty(test2Property);

            // Read properties
            var test1Properties = properties.GetPropertiesByKey(test1Property.Key).ToArray();
            var test2Properties = properties.GetPropertiesByKey(test2Property.Key).ToArray();

            // Check lengths
            Assert.AreEqual(1, test1Properties.Length);
            Assert.AreEqual(1, test2Properties.Length);

            // Check contents
            Assert.AreEqual(test1Property, test1Properties[0]);
            Assert.AreEqual(test2Property, test2Properties[0]);
        }

        /// <summary>
        /// Makes sure that properties can be saved and loaded properly when they have a value but no attributes.
        /// </summary>
        [TestMethod]
        public void TestAddAndRetrievePropertiesWithoutAttributes()
        {
            const string fileName = "no_attributes.xml";

            // Delete existing file.
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Create new properties.
            var properties = new PropertiesFile(fileName);

            var test1Property = new Property("test1", "test1Value");
            var test2Property = new Property("test2", "test2Value");

            // Write properties
            properties.WriteProperty(test1Property);
            properties.WriteProperty(test2Property);

            // Read properties
            var test1Properties = properties.GetPropertiesByKey(test1Property.Key).ToArray();
            var test2Properties = properties.GetPropertiesByKey(test2Property.Key).ToArray();

            // Check lengths
            Assert.AreEqual(1, test1Properties.Length);
            Assert.AreEqual(1, test2Properties.Length);

            // Check contents
            Assert.AreEqual(test1Property, test1Properties[0]);
            Assert.AreEqual(test2Property, test2Properties[0]);
        }

        /// <summary>
        /// Makes sure that multiple properties of the same key can be saved and loaded properly.
        /// </summary>
        [TestMethod]
        public void TestAddAndRetrieveMultipleOfSameProperties()
        {
            const string fileName = "save_load_multiple.xml";

            // Delete existing file.
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Create new properties.
            var properties = new PropertiesFile(fileName);

            var attributes1 = new Dictionary<string, string> { ["test1Attr"] = "test1AttrValue" };
            var attributes2 = new Dictionary<string, string> { ["test2Attr"] = "test2AttrValue" };

            var test1Property = new Property("test1", "test1Value", attributes1);
            var test2Property = new Property("test2", "test2Value", attributes2);
            var test3Property = new Property("test1", "test1Value2");

            // Write properties
            properties.WriteProperty(test1Property);
            properties.WriteProperty(test2Property);
            properties.WriteProperty(test3Property);

            // Read properties
            var test1Properties = properties.GetPropertiesByKey(test1Property.Key).ToArray();
            var test2Properties = properties.GetPropertiesByKey(test2Property.Key).ToArray();

            // Check lengths
            Assert.AreEqual(2, test1Properties.Length);
            Assert.AreEqual(1, test2Properties.Length);

            // Check contents
            Assert.AreEqual(test1Property, test1Properties[0]);
            Assert.AreEqual(test2Property, test2Properties[0]);
            Assert.AreEqual(test3Property, test1Properties[1]);
        }
    }
}