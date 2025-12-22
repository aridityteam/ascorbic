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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AridityTeam.Util
{
    /// <summary>
    /// Types of security hashes.
    /// 
    /// Hash generator methods from https://gist.github.com/rmacfie/828054/a2ed7ed1c023fbb7781e8f11ac69f2b0d780a717.
    /// </summary>
    public enum HashType
    {
        /// <summary>
        /// MD5
        /// </summary>
        Md5,

        /// <summary>
        /// SHA-1
        /// </summary>
        Sha1,

        /// <summary>
        /// SHA-256
        /// </summary>
        Sha256,

        /// <summary>
        /// SHA-384
        /// </summary>
        Sha384,

        /// <summary>
        /// SHA-512
        /// </summary>
        Sha512
    }
}
