// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json.Serialization
{
    using System;
    using System.Text.Json;

    /// <summary>
    /// Since SystemTextJson at the moment does not support specifying constructor arguments to converters, this converter just
    /// forces <see cref="JsonStringEnumConverter"/> to use camel casing.
    /// </summary>
    public class JsonCamelCaseStringEnumConverter : JsonConverterFactory
    {
        private readonly JsonStringEnumConverter jsonStringEnumConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonCamelCaseStringEnumConverter"/> class.
        /// </summary>
        public JsonCamelCaseStringEnumConverter()
        {
            this.jsonStringEnumConverter = new JsonStringEnumConverter(
                JsonNamingPolicy.CamelCase,
                allowIntegerValues: true);
        }

        /// <summary>
        /// Determines if the current converter can convert the type.
        /// </summary>
        /// <param name="typeToConvert">Type to convert.</param>
        /// <returns>True if the type can be converted, false otherwise.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return this.jsonStringEnumConverter.CanConvert(typeToConvert);
        }

        /// <summary>
        /// Creates a converter to perform conversion.
        /// </summary>
        /// <param name="typeToConvert">Type to convert.</param>
        /// <param name="options">Serializer options to use.</param>
        /// <returns>Json converter instance.</returns>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return this.jsonStringEnumConverter.CreateConverter(typeToConvert, options);
        }
    }
}
