//====================================================================================================
// Copyright (c) 2025 The Aridity Team, all rights reserved.
//
// The Aridity Game Client installer utility.
// Licensed under the "libapcf" (The Aridity Platform Core Framework) proprietary license.
//
// SPDX-FileCopyrightText: 2025 PracticeMedicine for The Aridity Team
// SPDX-License-Identifier: LicenseRef-libapcf
// SPDX-FileType: SOURCE
//====================================================================================================

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
