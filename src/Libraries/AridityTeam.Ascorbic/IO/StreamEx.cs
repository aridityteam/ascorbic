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

namespace AridityTeam.IO
{
    /// <summary>
    /// Custom <seealso cref="Stream"/> extensions.
    /// </summary>
    public static class StreamEx
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Gets a new <seealso cref="Stream"/> on a web response asynchronously.
        /// </summary>
        /// <param name="stream">The value of the current stream.</param>
        /// <param name="uri">The value of the web response URL.</param>
        /// <returns>The current stream that has been replaced with the newly response stream.</returns>
        public static Task<MemoryStream> GetResponseStreamAsync(this Stream stream, Uri uri) =>
            GetResponseStreamAsync(stream, uri, CancellationToken.None);

        /// <summary>
        /// Gets a new <seealso cref="Stream"/> on a web response asynchronously.
        /// </summary>
        /// <param name="_">The value of the current stream.</param>
        /// <param name="uri">The value of the web response URL.</param>
        /// <param name="token">The value of the cancellation token.</param>
        /// <returns>The current stream that has been replaced with the newly response stream.</returns>
        public static async Task<MemoryStream> GetResponseStreamAsync(this Stream _, Uri uri, CancellationToken token = default)
        {
            using (var responseStream = await _httpClient.GetStreamAsync(uri))
            {
                var ms = new MemoryStream();
                token.ThrowIfCancellationRequested();
                await responseStream.CopyToAsync(ms);
                ms.Position = 0;
                return ms;
            }
        }

        /// <summary>
        /// Gets the response content from a web request asynchronously.
        /// </summary>
        /// <param name="stream">The value of the current stream.</param>
        /// <param name="uri">The value of the request URL.</param>
        /// <returns>The main response if available.</returns>
        public static Task<string> GetResponseContentAsync(this Stream stream, Uri uri) =>
            GetResponseContentAsync(stream, uri, CancellationToken.None);

        /// <summary>
        /// Gets the response content from a web request asynchronously.
        /// </summary>
        /// <param name="_">The value of the current stream.</param>
        /// <param name="uri">The value of the request URL.</param>
        /// <param name="token">The value of the cancellation token.</param>
        /// <returns>The main response if available.</returns>
        public static async Task<string> GetResponseContentAsync(this Stream _, Uri uri, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            using (var stream = await GetResponseStreamAsync(_, uri, token))
            using (var sr = new StreamReader(stream, Encoding.UTF8, true, 1024, leaveOpen: true))
            {
                token.ThrowIfCancellationRequested();
                return await sr.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Gets a new <seealso cref="Stream"/> using the Base64 data from an URL asynchronously.
        /// </summary>
        /// <param name="stream">The value of the current stream.</param>
        /// <param name="uri">The value of the data URL.</param>
        /// <returns>The current stream that has been replaced with the newly data stream.</returns>
        public static Task<MemoryStream> GetDataStreamAsync(this Stream stream, Uri uri) =>
            GetDataStreamAsync(stream, uri, CancellationToken.None);

        /// <summary>
        /// Gets a new <seealso cref="Stream"/> using the Base64 data from an URL asynchronously.
        /// </summary>
        /// <param name="_">The value of the current stream.</param>
        /// <param name="uri">The value of the data URL.</param>
        /// <param name="token">The value of the cancellation token.</param>
        /// <returns>The current stream that has been replaced with the newly data stream.</returns>
        public static async Task<MemoryStream> GetDataStreamAsync(this Stream _, Uri uri, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var match = Regex.Match(uri.AbsoluteUri, @"^data:(?<mime>[^;]+);base64,(?<data>[A-Za-z0-9+/=]+)$");
            if (!match.Success)
                throw new ArgumentException("Invalid data URI format.", nameof(uri));

            var base64Data = match.Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);
            var stream = new MemoryStream();
            using (var newStream = new MemoryStream(binData))
            {
                stream.SetLength(0);
                token.ThrowIfCancellationRequested();
                await newStream.CopyToAsync(stream);
                stream.Position = 0;
                return stream;
            }
        }
    }
}
