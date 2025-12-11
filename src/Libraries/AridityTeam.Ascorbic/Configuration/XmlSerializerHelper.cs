/*
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
using System.Xml;
using System.Xml.Serialization;

namespace AridityTeam.Configuration
{
    /// <summary>
    /// https://stackoverflow.com/a/34813985
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlSerializerHelper<T> where T : class
    {
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <seealso cref="XmlSerializerHelper{T}"/> class.
        /// </summary>
        public XmlSerializerHelper()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        /// <summary>
        /// Converts bytes from an XML file into an .NET object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public T? BytesToObject(byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var reader = new XmlTextReader(memoryStream);
            return _serializer.Deserialize(reader) as T;
        }
    }
}
