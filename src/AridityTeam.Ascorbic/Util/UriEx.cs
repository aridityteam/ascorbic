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

namespace AridityTeam.Util
{
    /// <summary>
    /// Custom <seealso cref="Uri"/> extensions.
    /// </summary>
    public static class UriEx
    {
        /// <summary>
        /// Combines multiple endpoint strings into one new <seealso cref="Uri"/> instance.
        /// </summary>
        /// <param name="uri">The absolute or relative root URL value.</param>
        /// <param name="extraEndpoints">The extra endpoint strings.</param>
        /// <returns>The newly initialized <seealso cref="Uri"/> instance with the new combined endpoints.</returns>
        public static Uri Combine(string uri, params string[] extraEndpoints)
        {
            var joinedStrings = string.Join("/", extraEndpoints);
            if (!uri.EndsWith('/'))
                uri += '/';

            return new Uri(uri + joinedStrings);
        }

        /// <summary>
        /// Combines multiple endpoint strings into one new <seealso cref="Uri"/> instance.
        /// </summary>
        /// <param name="uri">The current <seealso cref="Uri"/> instance.</param>
        /// <param name="extraEndpoints">The extra endpoint strings.</param>
        /// <returns>The newly initialized <seealso cref="Uri"/> instance with the new combined endpoints.</returns>
        public static Uri Combine(this Uri uri, params string[] extraEndpoints)
        {
            var absoluteUrl = uri.AbsoluteUri;
            var joinedStrings = string.Join("/", extraEndpoints);
            if (!absoluteUrl.EndsWith('/'))
                absoluteUrl += '/';

            return new Uri(absoluteUrl + joinedStrings);
        }
    }
}
