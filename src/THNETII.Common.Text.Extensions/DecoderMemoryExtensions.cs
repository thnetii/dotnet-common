using System;
using System.Text;

#if !NETCOREAPP
using THNETII.Common.Buffers;
#endif // !NETCOREAPP

namespace THNETII.Common.Text
{
    /// <summary>
    /// Provides extensions methods on <see cref="Decoder"/> instance that use
    /// <see cref="Memory{T}"/> and <see cref="ReadOnlyMemory{T}"/> arguments.
    /// </summary>
    public static class DecoderMemoryExtensions
    {
        /// <summary>
        /// Calculates the number of characters produced by decoding a set of
        /// sequence of bytes from the specified byte memory. A parameter
        /// indicates whether to clear the internal state of the encoder after
        /// the calculation.
        /// </summary>
        /// <param name="decoder">The decoder to use for decoding.</param>
        /// <param name="bytes">The read-only memory containing the sequence of bytes to decode.</param>
        /// <param name="flush"><see langword="true"/> to simulate clearing the internal state of the decoder after the calculation; otherwise, <see langword="false"/>.</param>
        /// <returns>The number of characters produced by decoding the specified bytes and any bytes in the internal buffer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="decoder"/> is <see langword="null"/>.</exception>
        /// <exception cref="DecoderFallbackException">
        /// <para>
        /// A fallback occurred (see Character Encoding in the .NET Framework
        /// for fuller explanation)
        /// </para>
        /// <para>-and-</para>
        /// <para>
        /// The <see cref="Decoder.Fallback"/> property on <paramref name="decoder"/>
        /// is set to <see cref="DecoderExceptionFallback"/>.
        /// </para>
        /// </exception>
        public static int GetCharCount(this Decoder decoder, ReadOnlyMemory<byte> bytes,
            bool flush)
#if NETCOREAPP
        {
            return (decoder ?? throw new ArgumentNullException(nameof(decoder)))
                .GetCharCount(bytes.Span, flush);
        }
#else // !NETCOREAPP
        {
            if (decoder is null)
                throw new ArgumentNullException(nameof(decoder));

            using (var bytesDisposable = ArrayMemoryMarshal.GetMemoryArray(bytes))
            {
                var segment = bytesDisposable.Segment;
                return decoder.GetCharCount(segment.Array, segment.Offset,
                    segment.Count, flush);
            }
        }
#endif // !NETCOREAPP

        /// <summary>
        /// Decodes a sequence of bytes from the specified byte memory and any
        /// bytes in the internal buffer into the specified character memory. A
        /// parameter indicates whether to clear the internal state of the
        /// decoder after the conversion.
        /// </summary>
        /// <param name="decoder">The decoder to use for decoding.</param>
        /// <param name="bytes">The read-only memory containing the sequence of bytes to decode.</param>
        /// <param name="chars">The memory of characters to contain the resulting set of characters.</param>
        /// <param name="flush"><see langword="true"/> to clear the internal state of the decoder after the conversion; otherwise, <see langword="false"/>.</param>
        /// <returns>The actual number of characters written into <paramref name="chars"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="decoder"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="chars"/> does not have enough capacity to accommodate the resulting characters.</exception>
        /// <exception cref="DecoderFallbackException">
        /// <para>
        /// A fallback occurred (see Character Encoding in the .NET Framework
        /// for fuller explanation)
        /// </para>
        /// <para>-and-</para>
        /// <para>
        /// The <see cref="Decoder.Fallback"/> property on <paramref name="decoder"/>
        /// is set to <see cref="DecoderExceptionFallback"/>.
        /// </para>
        /// </exception>
        public static int GetChars(this Decoder decoder, ReadOnlyMemory<byte> bytes,
            Memory<char> chars, bool flush)
#if NETCOREAPP
        {
            return (decoder ?? throw new ArgumentNullException(nameof(decoder)))
                .GetChars(bytes.Span, chars.Span, flush);
        }
#else // !NETCOREAPP
        {
            if (decoder is null)
                throw new ArgumentNullException(nameof(decoder));

            using (var charsDisposable = ArrayMemoryMarshal.GetMemoryArray(bytes))
            using (var bytesDisposable = ArrayMemoryMarshal.GetMemoryArray(chars, noCopy: true, noCopyBack: false))
            {
                var charsSegment = charsDisposable.Segment;
                var bytesSegment = bytesDisposable.Segment;

                return decoder.GetChars(charsSegment.Array, charsSegment.Offset,
                    charsSegment.Count, bytesSegment.Array, bytesSegment.Offset,
                    flush);
            }
        }
#endif // !NETCOREAPP

    }
}
