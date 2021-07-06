// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json.Extensions.Tests.Encoders
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Encodings.Web.NewtonsoftJson;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="NewtonsoftJsonCompatibleEncoder"/>.
    /// </summary>
    [TestClass]
    public class NewtonsoftJsonCompatibleEncoderTests
    {
        private static readonly JsonSerializerOptions NewtonsoftJsonCompatibleOptions = new JsonSerializerOptions
        {
            Encoder = NewtonsoftJsonCompatibleEncoder.UnsafeNewtonsoftJsonCompatibleEncoder,
        };

        /// <summary>
        /// Test to ensure that Emojis are not escaped with newtonsoft.json compatible encoder.
        /// </summary>
        [TestMethod]
        public void NewtonsoftJsonCompatibleEncoder_EnsureEmojisAreNotEscaped()
        {
            NewtonsoftJsonCompatibleEncoderTests.AssertSerializedContentsAreSame<Dictionary<string, string>>("Emoji.json");
        }

        /// <summary>
        /// Asserts that serialized contents are same when deserialized using Newtonsoft.Json and serialized using System.Text.Json.
        /// </summary>
        /// <typeparam name="T">Type to deserialize into.</typeparam>
        /// <param name="fileName">Test file name containing json content.</param>
        private static void AssertSerializedContentsAreSame<T>(string fileName)
        {
            string originalText = NewtonsoftJsonCompatibleEncoderTests.GetTestJsonContents(fileName);

            // Clean out all whitespaces.
            originalText = string.Join(string.Empty, originalText.Split(Array.Empty<string>(), StringSplitOptions.RemoveEmptyEntries));

            T newtonsoftDeserializedObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(originalText);

            string systemTextJsonTextFromNewtonsoftObject = JsonSerializer.Serialize(
                newtonsoftDeserializedObject,
                typeof(T),
                NewtonsoftJsonCompatibleEncoderTests.NewtonsoftJsonCompatibleOptions);

            T systemTextJsonDeserializedObject = JsonSerializer.Deserialize<T>(
                originalText,
                NewtonsoftJsonCompatibleEncoderTests.NewtonsoftJsonCompatibleOptions);

            string systemTextJsonTextFromSystemTextJsonObject = JsonSerializer.Serialize(
                systemTextJsonDeserializedObject,
                typeof(T),
                NewtonsoftJsonCompatibleEncoderTests.NewtonsoftJsonCompatibleOptions);

            Assert.AreEqual(originalText, systemTextJsonTextFromNewtonsoftObject);
            Assert.AreEqual(originalText, systemTextJsonTextFromSystemTextJsonObject);
        }

        /// <summary>
        /// Reads entire contents of the passed test json content file.
        /// </summary>
        /// <param name="fileName">Name of the file to read.</param>
        /// <returns>File's content.</returns>
        private static string GetTestJsonContents(string fileName)
        {
            return File.ReadAllText($@"Encoders/TestJsons/{fileName}");
        }
    }
}
