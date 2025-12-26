﻿/*
 * Copyright (c) 2025 The Aridity Team
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

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
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided memory is not backed by a character array.
        /// </exception>
        public static Task WriteAsync(
            this TextWriter writer,
            ReadOnlyMemory<char> buffer,
            CancellationToken cancellationToken = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            if (!MemoryMarshal.TryGetArray(buffer, out var segment))
                throw new ArgumentException(
                    "The provided ReadOnlyMemory<char> must be backed by an array.",
                    nameof(buffer));

            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                writer.Write(segment.Array!, segment.Offset, segment.Count);
            }, cancellationToken);
#else
            return writer.WriteAsync(buffer, cancellationToken);
#endif
        }

        /// <summary>
        /// Asynchronously writes a sequence of characters followed by a line terminator to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="buffer">The buffer containing the characters to write.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided memory is not backed by a character array.
        /// </exception>
        public static Task WriteLineAsync(
            this TextWriter writer,
            ReadOnlyMemory<char> buffer,
            CancellationToken cancellationToken = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

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
                cancellationToken.ThrowIfCancellationRequested();
                writer.Write(segment.Array!, segment.Offset, segment.Count);
                writer.WriteLine();
            }, cancellationToken);
#else
            return writer.WriteLineAsync(buffer, cancellationToken);
#endif
        }

        /// <summary>
        /// Asynchronously writes a slice of a <see cref="Memory{Char}"/> buffer to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="buffer">The buffer containing the characters to write.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <remarks>
        /// This overload exists for API symmetry with modern .NET versions.
        /// </remarks>
        public static Task WriteAsync(
            this TextWriter writer,
            Memory<char> buffer,
            CancellationToken cancellationToken = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            return writer.WriteAsync((ReadOnlyMemory<char>)buffer, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a character to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="value">The character to write.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteAsync(
            this TextWriter writer,
            char value,
            CancellationToken cancellationToken)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                writer.Write(value);
            }, cancellationToken);
#else
            return writer.WriteAsync(value, cancellationToken);
#endif
        }

        /// <summary>
        /// Asynchronously writes a string to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="value">The string to write.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteAsync(
            this TextWriter writer,
            string? value,
            CancellationToken cancellationToken)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                writer.Write(value);
            }, cancellationToken);
#else
            return writer.WriteAsync(value, cancellationToken);
#endif
        }

        /// <summary>
        /// Asynchronously writes a subarray of characters to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="buffer">The character buffer.</param>
        /// <param name="index">The starting index in the buffer.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteAsync(
            this TextWriter writer,
            char[] buffer,
            int index,
            int count,
            CancellationToken cancellationToken)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                writer.Write(buffer, index, count);
            }, cancellationToken);
#else
            return writer.WriteAsync(buffer, index, count, cancellationToken);
#endif
        }

        /// <summary>
        /// Asynchronously writes a line terminator to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteLineAsync(
            this TextWriter writer,
            CancellationToken cancellationToken)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                writer.WriteLine();
            }, cancellationToken);
#else
            return writer.WriteLineAsync(cancellationToken);
#endif
        }

        /// <summary>
        /// Asynchronously writes a string followed by a line terminator to the text stream.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="value">The string to write.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteLineAsync(
            this TextWriter writer,
            string? value,
            CancellationToken cancellationToken)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                writer.WriteLine(value);
            }, cancellationToken);
#else
            return writer.WriteLineAsync(value, cancellationToken);
#endif
        }

        /// <summary>
        /// Asynchronously flushes all buffers for the text writer.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous flush operation.</returns>
        public static Task FlushAsync(
            this TextWriter writer,
            CancellationToken cancellationToken)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                writer.Flush();
            }, cancellationToken);
#else
            return writer.FlushAsync(cancellationToken);
#endif
        }
    }
}
