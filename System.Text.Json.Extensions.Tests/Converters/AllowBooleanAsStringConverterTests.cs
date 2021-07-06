// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json.Extensions.Tests.Converters
{
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="AllowBooleanAsStringConverter"/>.
    /// </summary>
    [TestClass]
    public class AllowBooleanAsStringConverterTests
    {
        /// <summary>
        /// Ensure that booleans serialized as string works.
        /// </summary>
        [TestMethod]
        public void AllowBooleanAsStringConverter_EnsureBooleanSerializedAsStringWorks()
        {
            string jsonString = "{ \"boolAsStringProp\": \"true\"}";

            TestModel testModel =
                JsonSerializer.Deserialize<TestModel>(jsonString, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            Assert.AreEqual(true, testModel.BoolAsStringProp);
        }

        /// <summary>
        /// Ensure that booleans serialized as boolean works.
        /// </summary>
        [TestMethod]
        public void AllowBooleanAsStringConverter_EnsureBooleanSerializedAsBooleanWorks()
        {
            string jsonString = "{ \"boolAsStringProp\": true}";

            TestModel testModel =
                JsonSerializer.Deserialize<TestModel>(jsonString, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            Assert.AreEqual(true, testModel.BoolAsStringProp);
        }

        /// <summary>
        /// Ensure that booleans are serialized as boolean when writing.
        /// </summary>
        [TestMethod]
        public void AllowBooleanAsStringConverter_EnsureBooleansAreSerializedAsBoolean()
        {
            TestModel testModel = new TestModel
            {
                BoolAsStringProp = true,
            };

            string serializedJson = JsonSerializer.Serialize(testModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            string jsonString = "{\"boolAsStringProp\":true}";

            Assert.AreEqual(jsonString, serializedJson);
        }

        /// <summary>
        /// Test model.
        /// </summary>
        private class TestModel
        {
            [JsonPropertyName("boolAsStringProp")]
            [JsonConverter(typeof(AllowBooleanAsStringConverter))]
            public bool BoolAsStringProp { get; set; }
        }
    }
}
