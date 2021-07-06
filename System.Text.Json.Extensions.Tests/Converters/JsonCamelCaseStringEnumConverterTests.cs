// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json.Extensions.Tests.Converters
{
    using System.Text.Json.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="JsonCamelCaseStringEnumConverter"/>.
    /// </summary>
    [TestClass]
    public class JsonCamelCaseStringEnumConverterTests
    {
        private enum TestEnum
        {
            EnumValue1,

            EnumValue2,
        }

        /// <summary>
        /// Ensures that <see cref="JsonCamelCaseStringEnumConverter"/> uses camel case strings.
        /// </summary>
        [TestMethod]
        public void JsonCamelCaseStringEnumConverter_EnsureCamelCaseStringsAreUsed()
        {
            TestModel testModel = new TestModel()
            {
                TestEnumVal = TestEnum.EnumValue2,
            };

            string serializedObject = JsonSerializer.Serialize(testModel);

            Assert.IsTrue(serializedObject.Contains("enumValue2", StringComparison.Ordinal));
            Assert.IsFalse(serializedObject.Contains("EnumValue2", StringComparison.Ordinal));
        }

        private class TestModel
        {
            [JsonConverter(typeof(JsonCamelCaseStringEnumConverter))]
            public TestEnum TestEnumVal { get; set; }
        }
    }
}
