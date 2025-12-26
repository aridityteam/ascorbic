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
using System.Text;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_0_OR_GREATER || NET472_OR_GREATER
using System.Runtime.InteropServices;

using AridityTeam.Net;
#endif

namespace AridityTeam.IO
{
    /// <summary>
    /// Custom <seealso cref="Stream"/> extensions.
    /// </summary>
    public static partial class StreamEx
    {
        /// <summary>
        /// Resets the position of the stream to the beginning.
        /// </summary>
        /// <param name="stream">The stream to rewind.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown when the stream does not support seeking.
        /// </exception>
        public static void Rewind(this Stream stream)
        {
            var seekable = stream.EnsureSeekable();
            seekable.Position = 0;
        }

        /// <summary>
        /// Ensures that the stream supports seeking.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <returns>
        /// A seekable stream. If the original stream is already seekable, it is returned unchanged;
        /// otherwise, a new <see cref="MemoryStream"/> containing the stream data is returned.
        /// </returns>
        public static Stream EnsureSeekable(this Stream stream)
        {
            if (stream.CanSeek)
                return stream;

            var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Copies data from one stream to another and reports progress.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <param name="destination">The destination stream.</param>
        /// <param name="progress">
        /// An optional progress reporter that receives the total number of bytes copied.
        /// </param>
        /// <param name="bufferSize">The size of the copy buffer.</param>
        /// <param name="token">A cancellation token.</param>
        /// <remarks>
        /// Neither stream is closed or disposed by this method.
        /// </remarks>
        public static async Task CopyToAsync(
            this Stream source,
            Stream destination,
            IProgress<long>? progress,
            int bufferSize = 81920,
            CancellationToken token = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

            var buffer = new byte[bufferSize];
            long total = 0;
            int read;

            while ((read = await source.ReadAsync(buffer, token)) > 0)
            {
                await destination.WriteAsync(buffer, token);
                total += read;
                progress?.Report(total);
            }
        }

        /// <summary>
        /// Gets a new <seealso cref="Stream"/> on a web response asynchronously.
        /// </summary>
        /// <param name="uri">The value of the web response URL.</param>
        /// <returns>The current stream that has been replaced with the newly response stream.</returns>
        public static Task<MemoryStream> GetResponseStreamAsync(Uri uri) =>
            GetResponseStreamAsync(uri, CancellationToken.None);

        /// <summary>
        /// Gets a new <seealso cref="Stream"/> on a web response asynchronously.
        /// </summary>
        /// <param name="uri">The value of the web response URL.</param>
        /// <param name="token">The value of the cancellation token.</param>
        /// <returns>The current stream that has been replaced with the newly response stream.</returns>
        public static async Task<MemoryStream> GetResponseStreamAsync(Uri uri, CancellationToken token = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

            using var httpClient = new HttpClient();
            using var responseStream = await httpClient.GetStreamAsync(uri, token);
            var ms = new MemoryStream();
            await responseStream.CopyToAsync(ms, token);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Gets the response content from a web request asynchronously.
        /// </summary>
        /// <param name="uri">The value of the request URL.</param>
        /// <returns>The main response if available.</returns>
        public static Task<string> GetResponseContentAsync(Uri uri) =>
            GetResponseContentAsync(uri, CancellationToken.None);

        /// <summary>
        /// Gets the response content from a web request asynchronously.
        /// </summary>
        /// <param name="uri">The value of the request URL.</param>
        /// <param name="token">The value of the cancellation token.</param>
        /// <returns>The main response if available.</returns>
        public static async Task<string> GetResponseContentAsync(Uri uri, CancellationToken token = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

            using var stream = await GetResponseStreamAsync(uri, token);
            using var sr = new StreamReader(stream, Encoding.UTF8, true, -1, leaveOpen: true);
            return await sr.ReadToEndAsync(token);
        }

        /// <summary>
        /// Gets a new <seealso cref="Stream"/> using the Base64 data from an URL asynchronously.
        /// </summary>
        /// <param name="uri">The value of the data URL.</param>
        /// <returns>The current stream that has been replaced with the newly data stream.</returns>
        public static Task<MemoryStream> GetDataStreamAsync(Uri uri) =>
            GetDataStreamAsync(uri, CancellationToken.None);

        /// <summary>
        /// Gets a new <seealso cref="Stream"/> using the Base64 data from an URL asynchronously.
        /// </summary>
        /// <param name="uri">The value of the data URL.</param>
        /// <param name="token">The value of the cancellation token.</param>
        /// <returns>The current stream that has been replaced with the newly data stream.</returns>
        public static async Task<MemoryStream> GetDataStreamAsync(Uri uri, CancellationToken token = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

            var match = Regex.Match(uri.AbsoluteUri, @"^data:(?<mime>[^;]+);base64,(?<data>[A-Za-z0-9+/=]+)$");
            if (!match.Success)
                throw new ArgumentException("Invalid data URI format.", nameof(uri));

            var base64Data = match.Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);
            var ms = new MemoryStream(binData);
            ms.Position = 0;
            return ms;
        }
    }
}
