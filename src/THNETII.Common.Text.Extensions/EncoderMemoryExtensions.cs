using System;
using System.Text;

#if !NETCOREAPP
using THNETII.Common.Buffers;
#endif // !NETCOREAPP

namespace THNETII.Common.Text
{
    /// <summary>
    /// Provides extensions methods on <see cref="Encoder"/> instance that use
    /// <see cref="Memory{T}"/> and <see cref="ReadOnlyMemory{T}"/> arguments.
    /// </summary>
    public static class EncoderMemoryExtensions
    {
        /// <summary>
        /// Calculates the number of bytes produced by encoding a set of
        /// characters from the specified character memory. A parameter
        /// indicates whether to clear the internal state of the encoder after
        /// the calculation.
        /// </summary>
        /// <param name="encoder">The encoder to use for encoding.</param>
        /// <param name="chars">The read-only memory containing the set of characters to encode.</param>
        /// <param name="flush"><see langword="true"/> to simulate clearing the internal state of the encoder after the calculation; otherwise, <see langword="false"/>.</param>
        /// <returns>The number of bytes produced by encoding the specified characters and any characters in the internal buffer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="encoder"/> is <see langword="null"/>.</exception>
        /// <exception cref="EncoderFallbackException">
        /// <para>
        /// A fallback occurred (see Character Encoding in the .NET Framework
        /// for fuller explanation)
        /// </para>
        /// <para>-and-</para>
        /// <para>
        /// The <see cref="Encoder.Fallback"/> property on <paramref name="encoder"/>
        /// is set to <see cref="EncoderExceptionFallback"/>.
        /// </para>
        /// </exception>
        public static int GetByteCount(this Encoder encoder, ReadOnlyMemory<char> chars,
            bool flush)
#if NETCOREAPP
        {
            return (encoder ?? throw new ArgumentNullException(nameof(encoder)))
                .GetByteCount(chars.Span, flush);
        }
#else // !NETCOREAPP
        {
            if (encoder is null)
                throw new ArgumentNullException(nameof(encoder));

            using (var charsDisposable = ArrayMemoryMarshal.GetMemoryArray(chars))
            {
                var segment = charsDisposable.Segment;
                return encoder.GetByteCount(segment.Array, segment.Offset,
                    segment.Count, flush);
            }
        }
#endif // !NETCOREAPP

        /// <summary>
        /// Encodes a set of characters from the specified character memory and
        /// any characters in the internal buffer into the specified byte
        /// memory. A parameter indicates whether to clear the internal state of
        /// the encoder after the conversion.
        /// </summary>
        /// <param name="encoder">The encoder to use for encoding.</param>
        /// <param name="chars">The read-only memory containing the set of characters to encode.</param>
        /// <param name="bytes">The memory of bytes to contain resulting sequence of bytes.</param>
        /// <param name="flush"><see langword="true"/> to clear the internal state of the encoder after the conversion; otherwise, <see langword="false"/>.</param>
        /// <returns>The actual number of bytes written into <paramref name="bytes"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="encoder"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="bytes"/> does not have enough capacity to accommodate the resulting bytes.</exception>
        /// <exception cref="EncoderFallbackException">
        /// <para>
        /// A fallback occurred (see Character Encoding in the .NET Framework
        /// for fuller explanation)
        /// </para>
        /// <para>-and-</para>
        /// <para>
        /// The <see cref="Encoder.Fallback"/> property on <paramref name="encoder"/>
        /// is set to <see cref="EncoderExceptionFallback"/>.
        /// </para>
        /// </exception>
        public static int GetBytes(this Encoder encoder, ReadOnlyMemory<char> chars,
            Memory<byte> bytes, bool flush)
#if NETCOREAPP
        {
            return (encoder ?? throw new ArgumentNullException(nameof(encoder)))
                .GetBytes(chars.Span, bytes.Span, flush);
        }
#else // !NETCOREAPP
        {
            if (encoder is null)
                throw new ArgumentNullException(nameof(encoder));

            using (var charsDisposable = ArrayMemoryMarshal.GetMemoryArray(chars))
            using (var bytesDisposable = ArrayMemoryMarshal.GetMemoryArray(bytes, noCopy: true, noCopyBack: false))
            {
                var charsSegment = charsDisposable.Segment;
                var bytesSegment = bytesDisposable.Segment;

                return encoder.GetBytes(charsSegment.Array, charsSegment.Offset,
                    charsSegment.Count, bytesSegment.Array, bytesSegment.Offset,
                    flush);
            }
        }  
#endif // !NETCOREAPP
    }
}
