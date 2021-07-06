// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Json.Serialization
{
    using System;
    using System.Linq;
    using System.Text.Json;

    /// <summary>
    /// Json collection converter.
    /// Alternate way to solve https://github.com/dotnet/runtime/issues/54189.
    /// </summary>
    /// <typeparam name="TConverterType">Converter to use for individual items.</typeparam>
    public class CollectionItemConverter<TConverterType> : JsonConverterFactory
        where TConverterType : JsonConverter
    {
        /// <summary>
        /// Checks if the converter can convert the given type.
        /// </summary>
        /// <param name="typeToConvert">Type to convert.</param>
        /// <returns>True if the type can be converted, false otherwise.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert is null)
            {
                throw new ArgumentNullException(nameof(typeToConvert));
            }

            return typeToConvert.GetInterface("System.Collections.IEnumerable") != null;
        }

        /// <summary>
        /// Creates a converter instance to use.
        /// </summary>
        /// <param name="typeToConvert">Type to convert.</param>
        /// <param name="options">Json serializer options.</param>
        /// <returns>Json converter instance.</returns>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert is null)
            {
                throw new ArgumentNullException(nameof(typeToConvert));
            }

            Type elementInCollectionType = typeToConvert.GetGenericArguments().Single();

            Type converterType = typeof(JsonCollectionItemConverter<,>).MakeGenericType(elementInCollectionType, typeof(TConverterType));

            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }
}
