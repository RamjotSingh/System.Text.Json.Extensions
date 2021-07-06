// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Encodings.Web.NewtonsoftJson
{
    using System;
    using System.Buffers;
    using global::Internal.System.Text;
    using RamjotSingh.EmojiNet;

    /// <summary>
    /// JavaScript encoder built on top of <see cref="JavaScriptEncoder.UnsafeRelaxedJsonEscaping"/> built to be compatible with
    /// Newtonsoft.Json.
    /// </summary>
    public class NewtonsoftJsonCompatibleEncoder : JavaScriptEncoder
    {
        /// <summary>
        /// Gets a JavaScript encoder instance that is compatible with Newtonsoft.Json's way of processing jsons. This is built on top of
        /// <see cref="JavaScriptEncoder.UnsafeRelaxedJsonEscaping"/>.
        /// </summary>
        public static readonly JavaScriptEncoder UnsafeNewtonsoftJsonCompatibleEncoder = new NewtonsoftJsonCompatibleEncoder();

        private readonly JavaScriptEncoder defaultEncoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonCompatibleEncoder"/> class.
        /// </summary>
        private NewtonsoftJsonCompatibleEncoder()
        {
        }

        /// <summary>
        /// Gets the maximum number of characters that this encoder can generate for each input code point.
        /// </summary>
        public override int MaxOutputCharactersPerInputCharacter => this.defaultEncoder.MaxOutputCharactersPerInputCharacter;

        /// <summary>
        /// Finds the index of the first character to encode.
        /// </summary>
        /// <param name="text"> The text buffer to search.</param>
        /// <param name="textLength">The number of characters in text.</param>
        /// <returns>The index of the first character to encode.</returns>
        public override unsafe int FindFirstCharacterToEncode(char* text, int textLength)
        {
            ReadOnlySpan<char> input = new ReadOnlySpan<char>(text, textLength);
            int idx = 0;

            // Enumerate until we're out of data or saw invalid input
            while (Rune.DecodeFromUtf16(input.Slice(idx), out Rune result, out int charsConsumed) == OperationStatus.Done)
            {
                if (this.WillEncode(result.Value))
                {
                    // found a char that needs to be escaped
                    break;
                }

                idx += charsConsumed;
            }

            if (idx == input.Length)
            {
                // walked entire input without finding a char which needs escaping
                return -1;
            }

            return idx;
        }

        /// <summary>
        /// Encodes a Unicode scalar value and writes it to a buffer.
        /// </summary>
        /// <param name="unicodeScalar">A Unicode scalar value.</param>
        /// <param name="buffer">A pointer to the buffer to which to write the encoded text.</param>
        /// <param name="bufferLength">The length of the destination buffer in characters.</param>
        /// <param name="numberOfCharactersWritten">When the method returns, indicates the number of characters written to the buffer.</param>
        /// <returns>false if bufferLength is too small to fit the encoded text; otherwise, returns true.</returns>
        public override unsafe bool TryEncodeUnicodeScalar(int unicodeScalar, char* buffer, int bufferLength, out int numberOfCharactersWritten)
        {
            return this.defaultEncoder.TryEncodeUnicodeScalar(unicodeScalar, buffer, bufferLength, out numberOfCharactersWritten);
        }

        /// <summary>
        /// Determines if a given Unicode scalar value will be encoded.
        /// </summary>
        /// <param name="unicodeScalar">A Unicode scalar value.</param>
        /// <returns>true if the unicodeScalar value will be encoded by this encoder; otherwise, returns false.</returns>
        public override bool WillEncode(int unicodeScalar)
        {
            if (UnicodeScalarHelper.IsUnicodeScalarAnEmoji(unicodeScalar))
            {
                return false;
            }
            else
            {
                return this.defaultEncoder.WillEncode(unicodeScalar);
            }
        }
    }
}
