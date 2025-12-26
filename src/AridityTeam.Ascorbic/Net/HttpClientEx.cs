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
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

using AridityTeam.Threading;

namespace AridityTeam.Net
{
    /// <summary>
    /// Custom <seealso cref="HttpClient"/> extensions.
    /// </summary>
    public static class HttpClientEx
    {
        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a stream
        /// in an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <seealso cref="HttpClient"/> instance.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<Stream> GetStreamAsync(this HttpClient httpClient, string? requestUri, CancellationToken cancellationToken = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NET8_0_OR_GREATER
            return httpClient.GetStreamAsync(requestUri, cancellationToken);
#else
            return httpClient.GetStreamAsync(requestUri)
                            .WithCancellation(cancellationToken);
#endif
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a stream
        /// in an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <seealso cref="HttpClient"/> instance.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<Stream> GetStreamAsync(this HttpClient httpClient, Uri? requestUri, CancellationToken cancellationToken = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NET8_0_OR_GREATER
            return httpClient.GetStreamAsync(requestUri, cancellationToken);
#else
            return httpClient.GetStreamAsync(requestUri)
                            .WithCancellation(cancellationToken);
#endif
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a string
        /// in an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <seealso cref="HttpClient"/> instance.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<string> GetStringAsync(this HttpClient httpClient, string? requestUri, CancellationToken cancellationToken = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NET8_0_OR_GREATER
            return httpClient.GetStringAsync(requestUri, cancellationToken);
#else
            return httpClient.GetStringAsync(requestUri)
                            .WithCancellation(cancellationToken);
#endif
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a string
        /// in an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <seealso cref="HttpClient"/> instance.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<string> GetStringAsync(this HttpClient httpClient, Uri? requestUri, CancellationToken cancellationToken = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NET8_0_OR_GREATER
            return httpClient.GetStringAsync(requestUri, cancellationToken);
#else
            return httpClient.GetStringAsync(requestUri)
                            .WithCancellation(cancellationToken);
#endif
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a byte array
        /// in an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <seealso cref="HttpClient"/> instance.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<byte[]> GetByteArrayAsync(this HttpClient httpClient, string? requestUri, CancellationToken cancellationToken = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NET8_0_OR_GREATER
            return httpClient.GetByteArrayAsync(requestUri, cancellationToken);
#else
            return httpClient.GetByteArrayAsync(requestUri)
                            .WithCancellation(cancellationToken);
#endif
        }

        /// <summary>
        /// Send a GET request to the specified Uri and return the response body as a byte array
        /// in an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <seealso cref="HttpClient"/> instance.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<byte[]> GetByteArrayAsync(this HttpClient httpClient, Uri? requestUri, CancellationToken cancellationToken = default)
        {
            // CancellationToken.ThrowIfCancellationRequested throws InvalidOperationException
            // instead of TaskCanceledException.
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

#if NET8_0_OR_GREATER
            return httpClient.GetByteArrayAsync(requestUri, cancellationToken);
#else
            return httpClient.GetByteArrayAsync(requestUri)
                            .WithCancellation(cancellationToken);
#endif
        }
    }
}
