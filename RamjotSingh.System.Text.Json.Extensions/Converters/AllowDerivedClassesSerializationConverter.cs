// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// Json serialization converter supporting polymorphic serialization.
    /// Read more about the issue at https://github.com/dotnet/runtime/issues/29937.
    /// </summary>
    public class AllowDerivedClassesSerializationConverter : JsonConverter<object>
    {
        /// <summary>
        /// Returns a value indicating whether current converter supports the type being operated upon.
        /// </summary>
        /// <param name="typeToConvert">Type to convert.</param>
        /// <returns>True always.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        /// <summary>
        /// Reads json representation of the object to object.
        /// </summary>
        /// <param name="reader">Json reader.</param>
        /// <param name="typeToConvert">Type to convert.</param>
        /// <param name="options">Serialization options.</param>
        /// <returns>Read object.</returns>
        /// <remarks>Throws not implemented exception.</remarks>
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes json representation of the object.
        /// </summary>
        /// <param name="writer">Json writer.</param>
        /// <param name="value">Value to serialize.</param>
        /// <param name="options">Serialization options.</param>
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

#pragma warning disable CA1508 // Avoid dead conditional code - The code isnt dead conditional.
            if ((value as IEnumerable<object>) != null)
#pragma warning restore CA1508 // Avoid dead conditional code - The code isnt dead conditional.
            {
                writer.WriteStartArray();
                foreach (object objectToSerialize in value as IEnumerable)
                {
                    JsonSerializer.Serialize<object>(writer, objectToSerialize, options);
                }

                writer.WriteEndArray();
            }
            else
            {
                JsonSerializer.Serialize<object>(writer, value, options);
            }
        }
    }
}
