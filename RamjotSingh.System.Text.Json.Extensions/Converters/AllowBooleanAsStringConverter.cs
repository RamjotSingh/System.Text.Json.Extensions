// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json.Serialization
{
    using System;
    using System.Text.Json;

    /// <summary>
    /// Allows string values to be read as boolean.
    /// </summary>
    public class AllowBooleanAsStringConverter : JsonConverter<bool>
    {
        /// <summary>
        /// Reads the value from json contents.
        /// </summary>
        /// <param name="reader">Json reader.</param>
        /// <param name="typeToConvert">Type to convert.</param>
        /// <param name="options">Serializer options.</param>
        /// <returns>Read value.</returns>
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return bool.Parse(reader.GetString());
            }
            else
            {
                return reader.GetBoolean();
            }
        }

        /// <summary>
        /// Writes a value to json.
        /// </summary>
        /// <param name="writer">Json writer.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="options">Serializer options.</param>
        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
