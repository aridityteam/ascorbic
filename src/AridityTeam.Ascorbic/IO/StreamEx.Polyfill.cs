using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER // so that it doesn't complain about an unused using directive.
using System.Runtime.InteropServices;
#endif

namespace AridityTeam.IO
{
    /// <summary>
    /// Custom <seealso cref="Stream"/> extensions that polyfills newer APIs for .NET Framework 4.7.2 and .NET Standard 2.0.
    /// </summary>
    public static partial class StreamEx
    {
        /// <summary>
        /// Asynchronously reads a sequence of bytes from the stream into a <see cref="Memory{T}"/> buffer.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A task that represents the asynchronous read operation.
        /// The value of the task is the total number of bytes read into the buffer.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided buffer is not backed by a byte array.
        /// </exception>
        public static Task<int> ReadAsync(
            this Stream stream,
            Memory<byte> buffer,
            CancellationToken token = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            if (!MemoryMarshal.TryGetArray<byte>(buffer, out var segment))
                throw new ArgumentException(SR.BufferMustBeBackedByArray, nameof(buffer));

            return stream.ReadAsync(segment.Array!, segment.Offset, segment.Count, token);
#else
            return stream.ReadAsync(buffer, token).AsTask();
#endif
        }

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the stream from a <see cref="ReadOnlyMemory{T}"/> buffer.
        /// </summary>
        /// <param name="stream">The destination stream.</param>
        /// <param name="buffer">The buffer containing the data to write.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A task that represents the asynchronous write operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided buffer is not backed by a byte array.
        /// </exception>
        public static Task WriteAsync(
            this Stream stream,
            ReadOnlyMemory<byte> buffer,
            CancellationToken token = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            if (!MemoryMarshal.TryGetArray(buffer, out var segment))
                throw new ArgumentException(SR.BufferMustBeBackedByArray, nameof(buffer));

            return stream.WriteAsync(segment.Array!, segment.Offset, segment.Count, token);
#else
            return stream.WriteAsync(buffer, token).AsTask();
#endif
        }

        /// <summary>
        /// Asynchronously copies the contents of the stream to another stream.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <param name="destination">The destination stream.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A task that represents the asynchronous copy operation.
        /// </returns>
        /// <remarks>
        /// Neither stream is closed or disposed by this method.
        /// </remarks>
        public static Task CopyToAsync(
            this Stream source,
            Stream destination,
            CancellationToken token)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return source.CopyToAsync(destination, 81920, token);
#else
            return source.CopyToAsync(destination, token);
#endif
        }

        /// <summary>
        /// Reads the specified number of bytes from the stream.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <param name="offset">The zero-based offset in <paramref name="buffer"/>.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <param name="token">A cancellation token.</param>
        /// <exception cref="EndOfStreamException">
        /// Thrown when the end of the stream is reached before reading the requested number of bytes.
        /// </exception>
        public static async Task ReadExactlyAsync(
            this Stream stream,
            byte[] buffer,
            int offset,
            int count,
            CancellationToken token = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

#if NET7_0_OR_GREATER
            await stream.ReadExactlyAsync(buffer, offset, count, token);
#else
            int read;
            while (count > 0 &&
                   (read = await stream.ReadAsync(buffer, offset, count, token)) > 0)
            {
                offset += read;
                count -= read;
            }

            if (count != 0)
                throw new EndOfStreamException();
#endif
        }

        /// <summary>
        /// Asynchronously reads at least a specified number of bytes from the stream.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <param name="offset">The zero-based offset in <paramref name="buffer"/>.</param>
        /// <param name="count">The minimum number of bytes to read.</param>
        /// <param name="throwOnEndOfStream">
        /// <see langword="true"/> to throw an exception if the end of the stream is reached before
        /// reading the requested number of bytes; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// The total number of bytes read.
        /// </returns>
        /// <exception cref="EndOfStreamException">
        /// Thrown when <paramref name="throwOnEndOfStream"/> is <see langword="true"/> and
        /// the end of the stream is reached prematurely.
        /// </exception>
        public static async Task<int> ReadAtLeastAsync(
            this Stream stream,
            byte[] buffer,
            int offset,
            int count,
            bool throwOnEndOfStream,
            CancellationToken token = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

#if NET7_0_OR_GREATER
            return await stream.ReadAtLeastAsync(buffer, offset, count, throwOnEndOfStream, token);
#else
            int totalRead = 0;
            int read;

            while (totalRead < count &&
                  (read = await stream.ReadAsync(buffer, offset + totalRead, count - totalRead, token)) > 0)
            {
                totalRead += read;
            }

            if (throwOnEndOfStream && totalRead < count)
                throw new EndOfStreamException();

            return totalRead;
#endif
        }

        /// <summary>
        /// Asynchronously clears all buffers for the stream.
        /// </summary>
        /// <param name="stream">The stream to flush.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A task that represents the asynchronous flush operation.
        /// </returns>
        public static Task FlushAsync(
            this Stream stream,
            CancellationToken token)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

            return stream.FlushAsync(token);
        }
    }
}
