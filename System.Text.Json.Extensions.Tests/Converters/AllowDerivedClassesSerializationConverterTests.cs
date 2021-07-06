// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json.Extensions.Tests.Converters
{
    using System.Collections.Generic;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="AllowDerivedClassesSerializationConverter"/>.
    /// </summary>
    [TestClass]
    public class AllowDerivedClassesSerializationConverterTests
    {
        /// <summary>
        /// Ensures derived class properties are serialized.
        /// </summary>
        [TestMethod]
        public void AllowDerivedClassesSerializationConverter_EnsureDerivedObjectsAreSerialized()
        {
            TestModel testModel = new TestModel
            {
                ListBasedObjects = new DerivedClass[] { new DerivedClass { BaseProp = "B1", DerivedProp = "D1" }, new DerivedClass { BaseProp = "B2", DerivedProp = "D2" } },
                ObjectToSerialize = new DerivedClass { BaseProp = "B", DerivedProp = "D" },
            };

            string serializedContent = JsonSerializer.Serialize(testModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            Assert.AreEqual(
                "{\"objectToSerialize\":{\"derivedProp\":\"D\",\"baseProp\":\"B\"},\"listBasedObjects\":[{\"derivedProp\":\"D1\",\"baseProp\":\"B1\"},{\"derivedProp\":\"D2\",\"baseProp\":\"B2\"}]}",
                serializedContent);
        }

        private class BaseClass
        {
            [JsonPropertyName("baseProp")]
            public string BaseProp { get; set; }
        }

        private class DerivedClass : BaseClass
        {
            [JsonPropertyName("derivedProp")]
            public string DerivedProp { get; set; }
        }

        private class TestModel
        {
            [JsonPropertyName("objectToSerialize")]
            [JsonConverter(typeof(AllowDerivedClassesSerializationConverter))]
            public BaseClass ObjectToSerialize { get; set; }

            [JsonPropertyName("listBasedObjects")]
            [JsonConverter(typeof(AllowDerivedClassesSerializationConverter))]
            public IEnumerable<BaseClass> ListBasedObjects { get; set; }
        }
    }
}
