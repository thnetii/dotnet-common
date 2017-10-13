using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.IO
{
    /// <summary>
    /// A wrapper Stream implementation around another <see cref="Stream"/> instance that supports automatic
    /// copying of read and written data into a secondary Copy-Stream.
    /// </summary>
    /// <remarks>
    /// Usage of this type is useful in situations where a non-seekable stream is consumed, but applications need to keep a record
    /// of the processed data.
    /// </remarks>
    public class CopyIOStream : Stream
    {
        /// <summary>
        /// Creates a new stream wrapper around the specified base stream, writing data to the copy stream on every read operation.
        /// </summary>
        /// <param name="baseStream">The origin stream to wrap. Must not be <c>null</c>.</param>
        /// <param name="copyStream">The copy stream to which data from the base stream will be written on read operations.</param>
        /// <returns>A new initialized <see cref="CopyIOStream"/> instance that wraps around the specified base stream.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="baseStream"/> is <c>null</c>.</exception>
        public static CopyIOStream CreateReadCopy(Stream baseStream, Stream copyStream) => CreateReadCopy(baseStream, copyStream, closeStreams: true);

        /// <summary>
        /// Creates a new stream wrapper around the specified base stream, writing data to the copy stream on every read operation.
        /// </summary>
        /// <param name="baseStream">The origin stream to wrap. Must not be <c>null</c>.</param>
        /// <param name="copyStream">The copy stream to which data from the base stream will be written on read operations.</param>
        /// <param name="closeStreams">A boolean value that indicates whether to close the base- and copy-stream when the wrapper is closed.</param>
        /// <returns>A new initialized <see cref="CopyIOStream"/> instance that wraps around the specified base stream.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="baseStream"/> is <c>null</c>.</exception>
        public static CopyIOStream CreateReadCopy(Stream baseStream, Stream copyStream, bool closeStreams)
            => CreateReadCopy(baseStream, copyStream, closeStreams, closeStreams);

        private static CopyIOStream CreateReadCopy(Stream baseStream, Stream copyStream, bool closeBaseStream, bool closeCopyStream)
            => new CopyIOStream(baseStream, copyStream, null, closeBaseStream, closeCopyStream, false);

        /// <summary>
        /// Creates a new stream wrapper around the specified base stream, writing data to the copy stream on every write operation.
        /// </summary>
        /// <param name="baseStream">The origin stream to wrap. Must not be <c>null</c>.</param>
        /// <param name="copyStream">The copy stream to which data from the base stream will be written on write operations.</param>
        /// <returns>A new initialized <see cref="CopyIOStream"/> instance that wraps around the specified base stream.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="baseStream"/> is <c>null</c>.</exception>
        public static CopyIOStream CreateWriteCopy(Stream baseStream, Stream copyStream) => CreateWriteCopy(baseStream, copyStream, closeStreams: true);

        /// <summary>
        /// Creates a new stream wrapper around the specified base stream, writing data to the copy stream on every write operation.
        /// </summary>
        /// <param name="baseStream">The origin stream to wrap. Must not be <c>null</c>.</param>
        /// <param name="copyStream">The copy stream to which data from the base stream will be written on write operations.</param>
        /// <param name="closeStreams">A boolean value that indicates whether to close the base- and copy-stream when the wrapper is closed.</param>
        /// <returns>A new initialized <see cref="CopyIOStream"/> instance that wraps around the specified base stream.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="baseStream"/> is <c>null</c>.</exception>
        public static CopyIOStream CreateWriteCopy(Stream baseStream, Stream copyStream, bool closeStreams) =>
            CreateWriteCopy(baseStream, copyStream, closeStreams, closeStreams);

        /// <exception cref="ArgumentNullException"><paramref name="baseStream"/> is <c>null</c>.</exception>
        private static CopyIOStream CreateWriteCopy(Stream baseStream, Stream copyStream, bool closeBaseStream, bool closeCopyStream)
            => new CopyIOStream(baseStream, null, copyStream, closeBaseStream, false, closeCopyStream);

        /// <summary>
        /// Creates a new stream wrapper around the specified base stream, writing data to either the read-copy stream or write-copy stream on every IO operation.
        /// </summary>
        /// <param name="baseStream">The origin stream to wrap. Must not be <c>null</c>.</param>
        /// <param name="readCopy">The copy stream to which data from the base stream will be written on read operations.</param>
        /// <param name="writeCopy">The copy stream to which data from the base stream will be written on write operations.</param>
        /// <exception cref="ArgumentNullException"><paramref name="baseStream"/> is <c>null</c>.</exception>
        public CopyIOStream(Stream baseStream, Stream readCopy, Stream writeCopy) : this(baseStream, readCopy, writeCopy, closeStreams: true) { }

        /// <summary>
        /// Creates a new stream wrapper around the specified base stream, writing data to either the read-copy stream or write-copy stream on every IO operation.
        /// </summary>
        /// <param name="baseStream">The origin stream to wrap. Must not be <c>null</c>.</param>
        /// <param name="readCopy">The copy stream to which data from the base stream will be written on read operations.</param>
        /// <param name="writeCopy">The copy stream to which data from the base stream will be written on write operations.</param>
        /// <param name="closeStreams">A boolean value that indicates whether to close the base- and copy-streams when the wrapper is closed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="baseStream"/> is <c>null</c>.</exception>
        public CopyIOStream(Stream baseStream, Stream readCopy, Stream writeCopy, bool closeStreams)
            : this(baseStream, readCopy, writeCopy, closeStreams, closeStreams, closeStreams) { }

        /// <summary>
        /// Creates a new stream wrapper around the specified base stream, writing data to either the read-copy stream or write-copy stream on every IO operation.
        /// </summary>
        /// <param name="baseStream">The origin stream to wrap. Must not be <c>null</c>.</param>
        /// <param name="readCopy">The copy stream to which data from the base stream will be written on read operations.</param>
        /// <param name="writeCopy">The copy stream to which data from the base stream will be written on write operations.</param>
        /// <param name="closeBaseStream">A boolean value that indicates whether to close the base stream when the wrapper is closed.</param>
        /// <param name="closeCopyStreams">A boolean value that indicates whether to close the copy streams when the wrapper is closed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="baseStream"/> is <c>null</c>.</exception>
        public CopyIOStream(Stream baseStream, Stream readCopy, Stream writeCopy, bool closeBaseStream, bool closeCopyStreams)
            : this(baseStream, readCopy, writeCopy, closeBaseStream, closeCopyStreams, closeCopyStreams) { }

        /// <summary>
        /// Creates a new stream wrapper around the specified base stream, writing data to either the read-copy stream or write-copy stream on every IO operation.
        /// </summary>
        /// <param name="baseStream">The origin stream to wrap. Must not be <c>null</c>.</param>
        /// <param name="readCopy">The copy stream to which data from the base stream will be written on read operations.</param>
        /// <param name="writeCopy">The copy stream to which data from the base stream will be written on write operations.</param>
        /// <param name="closeBaseStream">A boolean value that indicates whether to close the base stream when the wrapper is closed.</param>
        /// <param name="closeReadCopy">A boolean value that indicates whether to close the read-copy stream when the wrapper is closed.</param>
        /// <param name="closeWriteCopy">A boolean value that indicates whether to close the write-copy stream when the wrapper is closed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="baseStream"/> is <c>null</c>.</exception>
        public CopyIOStream(Stream baseStream, Stream readCopy, Stream writeCopy, bool closeBaseStream, bool closeReadCopy, bool closeWriteCopy)
        {
            BaseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
            CloseBaseStream = closeBaseStream;

            ReadCopy = readCopy;
            CloseReadCopy = readCopy != null ? closeReadCopy : false;

            WriteCopy = writeCopy;
            CloseWriteCopy = writeCopy != null ? closeWriteCopy : false;
        }

        /// <summary>
        /// Gets the origin stream on which IO operations are performed when IO operations on this instance are called.
        /// </summary>
        /// <value>The non-<c>null</c> <see cref="Stream"/> instance that was passed to the <see cref="CopyIOStream"/> constructor when this instance was created.</value>
        public Stream BaseStream { get; }

        /// <summary>
        /// Gets the stream to which the wrapper writes data that is read from the origin stream.
        /// </summary>
        /// <value>
        /// The <see cref="Stream"/> instance that was passed to the <see cref="CopyIOStream"/> constructor when this instance was created; 
        /// or <c>null</c> if the wrapper does not copy data on read operations.
        /// </value>
        public Stream ReadCopy { get; }

        /// <summary>
        /// Gets the stream to which the wrapper writes data that is written to the origin stream.
        /// </summary>
        /// <value>
        /// The <see cref="Stream"/> instance that was passed to the <see cref="CopyIOStream"/> constructor when this instance was created; 
        /// or <c>null</c> if the wrapper does not copy data on write operations.
        /// </value>
        public Stream WriteCopy { get; }

        /// <summary>
        /// Gets a value that determines whether the origin stream is closed when this instance is closed.
        /// </summary>
        public bool CloseBaseStream { get; }

        /// <summary>
        /// Gets a value that determines whether the read-copy stream is closed when this instance is closed.
        /// </summary>
        public bool CloseReadCopy { get; }

        /// <summary>
        /// Gets a value that determines whether the write-copy stream is closed when this instance is closed.
        /// </summary>
        public bool CloseWriteCopy { get; }

        /// <summary>
        /// Gets a value indicating whether the stream wrapper supports reading.
        /// </summary>
        /// <value><c>true</c> if the origin steam is readable and the read-copy stream is either <c>null</c> or writable; otherwise <c>false</c>.</value>
        public override bool CanRead => BaseStream.CanRead && (ReadCopy?.CanWrite ?? true);

        /// <summary>
        /// Gets a value indicating whether the stream wrapper supports seeking.
        /// <para>The <see cref="CopyIOStream"/> wrapper class does not support seeking, even if the wrapped origin stream does. This property always returns <c>false</c></para>
        /// </summary>
        /// <value><c>false</c> as <see cref="CopyIOStream"/> does not support seeking.</value>
        /// <remarks>
        /// Seeking is not supported by the <see cref="CopyIOStream"/> class to ensure consistent write and read copies or the origin stream.
        /// However, if <see cref="BaseStream"/> supports seeking, <see cref="Stream.Seek(long, SeekOrigin)"/> can still be called on the origin stream.
        /// </remarks>
        public override bool CanSeek => false;

        /// <summary>
        /// Gets a value indicating whether the stream wrapper supports writing.
        /// </summary>
        /// <value><c>true</c> if the origin steam is writable and the write-copy stream is either <c>null</c> or writable; otherwise <c>false</c>.</value>
        public override bool CanWrite => BaseStream.CanWrite && (WriteCopy?.CanWrite ?? true);

        /// <summary>
        /// Gets a value that determines whether the stream wrapper can time out.
        /// </summary>
        /// <value><c>true</c> if the origin stream or either one of the copy streams can time out; otherwise <c>false</c>.</value>
        public override bool CanTimeout => BaseStream.CanTimeout || (ReadCopy?.CanTimeout ?? false) || (WriteCopy?.CanTimeout ?? false);

        /// <summary>
        /// Gets the length, in bytes, of the orgin stream.
        /// </summary>
        /// <value>A long value representing the length of the stream in bytes.</value>
        /// <exception cref="NotSupportedException">The origin stream does not support seeking.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the origin stream was closed.</exception>
        public override long Length => BaseStream.Length;

        /// <summary>
        /// Gets the position within the origin stream. 
        /// Setting this property is not supported and will cause a <see cref="NotSupportedException"/> to be thrown.
        /// </summary>
        /// <value>The current position within the stream as a <see cref="long"/> valued offset from the beginning of the stream.</value>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="NotSupportedException">The origin stream does not support seeking.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the origin stream was closed.</exception>
        public override long Position
        {
            get => BaseStream.Position;
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the wrapper will attempt to read from the origin stream
        /// before timing out.
        /// </summary>
        /// <value>The <see cref="Stream.ReadTimeout"/> of the orgin stream or the <see cref="Stream.WriteTimeout"/> of the read-copy stream, whichever is smallest.</value>
        public override int ReadTimeout
        {
            get => GetReadTimeout();
            set
            {
                if (BaseStream.CanTimeout)
                    BaseStream.ReadTimeout = value;
                if (ReadCopy?.CanTimeout ?? false)
                    ReadCopy.WriteTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the wrapper will attempt to write to the origin stream
        /// before timing out.
        /// </summary>
        /// <value>The <see cref="Stream.WriteTimeout"/> of the orgin stream or the <see cref="Stream.WriteTimeout"/> of the write-copy stream, whichever is smallest.</value>
        public override int WriteTimeout
        {
            get => GetWriteTimeout();
            set
            {
                if (BaseStream.CanTimeout)
                    BaseStream.WriteTimeout = value;
                if (ReadCopy?.CanTimeout ?? false)
                    WriteCopy.WriteTimeout = value;
            }
        }

        private int f_disposed = 0;

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && Interlocked.Exchange(ref f_disposed, 1) == 0)
            {
                if (CloseBaseStream)
                    BaseStream.Dispose();
                if (CloseReadCopy)
                    ReadCopy?.Dispose();
                if (CloseWriteCopy)
                    WriteCopy?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Clears all buffers in the origin stream and both copy streams and causes any buffered data to be written to the underlying I/O medium.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        public override void Flush()
        {
            BaseStream.Flush();
            ReadCopy?.Flush();
            WriteCopy?.Flush();
        }

        /// <inheritdoc />
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            if (ReadCopy != null)
            {
                if (WriteCopy != null)
                    return Task.WhenAll(BaseStream.FlushAsync(cancellationToken), ReadCopy.FlushAsync(cancellationToken), WriteCopy.FlushAsync(cancellationToken));
                return Task.WhenAll(BaseStream.FlushAsync(cancellationToken), ReadCopy.FlushAsync(cancellationToken));
            }
            else if (WriteCopy != null)
                return Task.WhenAll(BaseStream.FlushAsync(cancellationToken), WriteCopy.FlushAsync(cancellationToken));
            return BaseStream.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// Reads a sequence of bytes from the base stream and advances the position within the stream by the number of bytes read.
        /// <para>A copy of the read bytes is written to the <see cref="ReadCopy"/> stream if it is specified.</para>
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes. When this method returns, the buffer contains the specified
        /// byte array with the values between <paramref name="offset"/> and <c>(<paramref name="offset"/> + <paramref name="count"/> - 1)</c> replaced by
        /// the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read
        /// from the current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to be read from the stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number
        /// of bytes requested if that many bytes are not currently available, or 0 (zero)
        /// if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The source stream does not support reading.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = BaseStream.Read(buffer, offset, count);
            ReadCopy?.Write(buffer, offset, bytesRead);
            return bytesRead;
        }

        /// <inheritdoc />
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var bytesRead = await BaseStream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
            if (ReadCopy != null)
                await ReadCopy.WriteAsync(buffer, offset, bytesRead, cancellationToken).ConfigureAwait(false);
            return bytesRead;
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
            WriteCopy?.Write(buffer, offset, count);
        }

        /// <inheritdoc />
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (WriteCopy != null)
                return Task.WhenAll(BaseStream.WriteAsync(buffer, offset, count, cancellationToken), WriteCopy.WriteAsync(buffer, offset, count, cancellationToken));
            return BaseStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        /// <summary>
        /// Always throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="offset">Ignored. A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">Ignored. A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <remarks>
        /// Instances derived from <see cref="CopyIOStream"/> by default do not support seeking in order to keep the read and write copies consistent.
        /// However, if the source stream is seekable you can call <see cref="Stream.Seek"/> on the <see cref="BaseStream"/> member. Note that doing so
        /// can potentially create garbled or discontinuous content in the read and/or write copies on subsequent I/O operations.
        /// </remarks>
        /// <exception cref="NotSupportedException">Instances of <see cref="CopyIOStream"/> do not support seeking.</exception>
        public override long Seek(long offset, SeekOrigin origin)
            => throw GetGeneralNotSupportedExceptionException(nameof(Seek));

        /// <summary>
        /// Always throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="value">Ignored. The desired length of the current stream in bytes.</param>
        /// <remarks>
        /// Instances derived from <see cref="CopyIOStream"/> by default do not support setting the length.
        /// However, if the underlying streams support it, you can call <see cref="Stream.SetLength"/> individually on the 
        /// <see cref="BaseStream"/>, <see cref="ReadCopy"/> and <see cref="WriteCopy"/> members. Note that doing so
        /// must take into account that <see cref="SetLength"/> might not succeed on all three members.
        /// </remarks>
        /// <exception cref="NotSupportedException">Instances of <see cref="CopyIOStream"/> do not support setting the length of the stream.</exception>
        public override void SetLength(long value)
            => throw GetGeneralNotSupportedExceptionException(nameof(SetLength));

        private int GetReadTimeout() => GetTimeout(s => s.ReadTimeout, ReadCopy);
        private int GetWriteTimeout() => GetTimeout(s => s.WriteTimeout, WriteCopy);
        private int GetTimeout(Func<Stream, int> timeoutGetter, Stream copyStream)
        {
            if (BaseStream.CanTimeout)
            {
                // Casts to uint to ensure that negative timeout values (infinite timeouts)
                // are valued greater than a specified non-negative timeout.
                uint baseTimeout = unchecked((uint)timeoutGetter(BaseStream));
                if (copyStream?.CanTimeout ?? false)
                {
                    uint copyTimeout = unchecked((uint)copyStream.WriteTimeout);
                    uint minimumTimeout = Math.Min(baseTimeout, copyTimeout);
                    return unchecked((int)minimumTimeout);
                }
                else
                    return unchecked((int)baseTimeout);
            }
            else if (copyStream?.CanTimeout ?? false)
                return copyStream.WriteTimeout;
            else
                throw GetGeneralNotSupportedExceptionException("Timeout");
        }

        [SuppressMessage("Microsoft.Globalization", "CA1305", Justification = "String Formatting not affected by Globalization.")]
        private NotSupportedException GetGeneralNotSupportedExceptionException(string memberName)
            => new NotSupportedException($"This operation is not supported on the {GetType()} type. Invoke the {memberName} member on the underlying stream.");
    }
}
