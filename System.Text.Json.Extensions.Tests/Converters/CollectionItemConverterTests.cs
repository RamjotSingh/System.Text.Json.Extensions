// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json.Extensions.Tests.Converters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="CollectionItemConverter{TElement}"/>.
    /// </summary>
    [TestClass]
    public class CollectionItemConverterTests
    {
        /// <summary>
        /// Test enum.
        /// </summary>
        private enum TestEnumModel
        {
            /// <summary>
            /// Value 1.
            /// </summary>
            Val1,

            /// <summary>
            /// Value 2.
            /// </summary>
            Val2,
        }

        /// <summary>
        /// Ensures that converter serializes the enum correctly.
        /// </summary>
        [TestMethod]
        public void CollectionItemConverter_SerializesEnumsCorrectly()
        {
            TestModel testModel = new TestModel
            {
                EnumVals = new List<TestEnumModel> { TestEnumModel.Val1, TestEnumModel.Val2 },
            };

            string serializedString = JsonSerializer.Serialize(testModel);
            Assert.AreEqual("{\"enumVals\":[\"Val1\",\"Val2\"]}", serializedString);
        }

        /// <summary>
        /// Ensures that converter deserializes the enum correctly.
        /// </summary>
        [TestMethod]
        public void CollectionItemConverter_DeserializesEnumsCorrectly()
        {
            string serializedJson = "{\"enumVals\":[\"Val1\",\"Val2\"]}";

            TestModel testModel = JsonSerializer.Deserialize<TestModel>(serializedJson);

            Assert.IsNotNull(testModel);
            Assert.IsNotNull(testModel.EnumVals);
            Assert.AreEqual(2, testModel.EnumVals.Count());
            Assert.AreEqual(TestEnumModel.Val1, testModel.EnumVals.First());
            Assert.AreEqual(TestEnumModel.Val2, testModel.EnumVals.Last());
        }

        /// <summary>
        /// Ensures that converter serializes null correctly.
        /// </summary>
        [TestMethod]
        public void CollectionItemConverter_SerializesNullsCorrectly()
        {
            TestModel testModel = new TestModel();

            string serializedString = JsonSerializer.Serialize(testModel);
            Assert.AreEqual("{\"enumVals\":null}", serializedString);
        }

        /// <summary>
        /// Ensures that converter deserializes null correctly.
        /// </summary>
        [TestMethod]
        public void CollectionItemConverter_DeserializesNullCorrectly()
        {
            string serializedJson = "{\"enumVals\":null}";

            TestModel testModel = JsonSerializer.Deserialize<TestModel>(serializedJson);

            Assert.IsNotNull(testModel);
            Assert.IsNull(testModel.EnumVals);
        }

        /// <summary>
        ///  Ensures that converter does not work on non-collections.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CollectionItemConverter_FailsIfConverterIsUsedOnNonCollections()
        {
            string serializedJson = "{\"enumVals\":null}";

            JsonSerializer.Deserialize<WrongTestModel>(serializedJson);
        }

        /// <summary>
        /// Test model.
        /// </summary>
        private class TestModel
        {
            /// <summary>
            /// Gets or sets EnumValues.
            /// </summary>
            [JsonPropertyName("enumVals")]
            [JsonConverter(typeof(CollectionItemConverter<JsonStringEnumConverter>))]
            public IEnumerable<TestEnumModel> EnumVals { get; set; }
        }

        /// <summary>
        /// Wrong test model.
        /// </summary>
        private class WrongTestModel
        {
            /// <summary>
            /// Gets or sets EnumValues.
            /// </summary>
            [JsonPropertyName("enumVals")]
            [JsonConverter(typeof(CollectionItemConverter<JsonStringEnumConverter>))]
            public TestEnumModel EnumVals { get; set; }
        }
    }
}
