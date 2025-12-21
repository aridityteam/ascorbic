using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AridityTeam.IO
{
    /// <summary>
    /// Custom <seealso cref="TextWriter"/> extensions.
    /// </summary>
    public static class TextWriterEx
    {
        /// <summary>
        /// Asynchronously writes a sequence of characters to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="buffer">The buffer containing the characters to write.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided memory is not backed by a character array.
        /// </exception>
        public static Task WriteAsync(
            this TextWriter writer,
            ReadOnlyMemory<char> buffer,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            if (!MemoryMarshal.TryGetArray(buffer, out var segment))
                throw new ArgumentException(
                    "The provided ReadOnlyMemory<char> must be backed by an array.",
                    nameof(buffer));

            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                writer.Write(segment.Array!, segment.Offset, segment.Count);
            }, token);
#else
            return writer.WriteAsync(buffer, token);
#endif
        }

        /// <summary>
        /// Asynchronously writes a sequence of characters followed by a line terminator to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="buffer">The buffer containing the characters to write.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided memory is not backed by a character array.
        /// </exception>
        public static Task WriteLineAsync(
            this TextWriter writer,
            ReadOnlyMemory<char> buffer,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            if (!MemoryMarshal.TryGetArray(buffer, out var segment))
                throw new ArgumentException(
                    PrivateErrorHelpers.Format(
                        SR.ProvidedTypeMustBeBackedByArray,
                        typeof(ReadOnlyMemory<char>).Name
                    ),
                    nameof(buffer));

            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                writer.Write(segment.Array!, segment.Offset, segment.Count);
                writer.WriteLine();
            }, token);
#else
            return writer.WriteLineAsync(buffer, token);
#endif
        }

        /// <summary>
        /// Asynchronously writes a slice of a <see cref="Memory{Char}"/> buffer to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="buffer">The buffer containing the characters to write.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <remarks>
        /// This overload exists for API symmetry with modern .NET versions.
        /// </remarks>
        public static Task WriteAsync(
            this TextWriter writer,
            Memory<char> buffer,
            CancellationToken token = default)
        {
            return writer.WriteAsync((ReadOnlyMemory<char>)buffer, token);
        }

        /// <summary>
        /// Asynchronously writes a character to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="value">The character to write.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteAsync(
            this TextWriter writer,
            char value,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                writer.Write(value);
            }, token);
#else
            return writer.WriteAsync(value, token);
#endif
        }

        /// <summary>
        /// Asynchronously writes a string to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="value">The string to write.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteAsync(
            this TextWriter writer,
            string? value,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                writer.Write(value);
            }, token);
#else
            return writer.WriteAsync(value, token);
#endif
        }

        /// <summary>
        /// Asynchronously writes a subarray of characters to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="buffer">The character buffer.</param>
        /// <param name="index">The starting index in the buffer.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteAsync(
            this TextWriter writer,
            char[] buffer,
            int index,
            int count,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                writer.Write(buffer, index, count);
            }, token);
#else
            return writer.WriteAsync(buffer, index, count, token);
#endif
        }

        /// <summary>
        /// Asynchronously writes a line terminator to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteLineAsync(
            this TextWriter writer,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                writer.WriteLine();
            }, token);
#else
            return writer.WriteLineAsync(token);
#endif
        }

        /// <summary>
        /// Asynchronously writes a string followed by a line terminator to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="value">The string to write.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteLineAsync(
            this TextWriter writer,
            string? value,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                writer.WriteLine(value);
            }, token);
#else
            return writer.WriteLineAsync(value, token);
#endif
        }

        /// <summary>
        /// Asynchronously flushes all buffers for the text writer.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous flush operation.</returns>
        public static Task FlushAsync(
            this TextWriter writer,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                writer.Flush();
            }, token);
#else
            return writer.FlushAsync(token);
#endif
        }
    }
}
