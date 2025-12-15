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
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AridityTeam.Pak
{
    /// <summary>
    /// The Aridity PAK archive.
    /// </summary>
    public class PakArchive
    {
        private const string MAGIC = "APRK";
        private const uint FOOTER_MAGIC = 0x4B41504B; // 'KAPK'
        private const int VERSION = 1;

        private readonly List<PakChunk> _chunks = [];

        /// <summary>
        /// Gets the collection of chuns in the archive.
        /// </summary>
        public IReadOnlyList<PakChunk> Chunks => _chunks.AsReadOnly();

        /// <summary>
        /// Adds a file into the archive by it's raw data in byes.
        /// </summary>
        /// <param name="fullPath">The path of the file to be added in the archive.</param>
        /// <param name="data">The raw data in bytes.</param>
        public void AddFile(string fullPath, byte[] data)
        {
            Requires.NotNullOrEmpty(fullPath);
            _chunks.Add(new PakChunk
            {
                FilePath = fullPath.Replace('\\', '/'),
                Data = data ?? []
            });
        }

        /// <summary>
        /// Adds a file into the archive from local disk.
        /// </summary>
        /// <param name="filePath">The absolute path to the file in the local machine.</param>
        /// <param name="archivePath">The path of the file to be added in the archive.</param>
        public void AddFileFromDisk(string filePath, string? archivePath = null)
        {
            var data = File.ReadAllBytes(filePath);
            AddFile(archivePath ?? Path.GetFileName(filePath), data);
        }

        private static byte[] Compress(byte[] data)
        {
            using var output = new MemoryStream();
            using (var deflate = new DeflateStream(
                output, CompressionLevel.Optimal, leaveOpen: true))
            {
                deflate.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        private static async Task<byte[]> CompressAsync(byte[] data, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            using var output = new MemoryStream();
            using (var deflate = new DeflateStream(
                output, CompressionLevel.Optimal, leaveOpen: true))
            {
                await deflate.WriteAsync(data, 0, data.Length, token);
            }
            return output.ToArray();
        }

        private static byte[] Decompress(byte[] data)
        {
            using var input = new MemoryStream(data);
            using var deflate = new DeflateStream(
                input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            deflate.CopyTo(output);
            return output.ToArray();
        }

        private static async Task<byte[]> DecompressAsync(byte[] data, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            using var input = new MemoryStream(data);
            using var deflate = new DeflateStream(
                input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            await deflate.CopyToAsync(output);
            return output.ToArray();
        }

        private static byte[] Obfuscate(byte[] data)
        {
            const byte key = 0xA7;
            for (int i = 0; i < data.Length; i++)
                data[i] ^= key;
            return data;
        }

        /// <summary>
        /// Saves the current Aridity PAK archive into a file.
        /// </summary>
        /// <param name="path">The absolute path of the created file.</param>
        public void Save(string path)
        {
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var writer = new BinaryWriter(fs);

            writer.Write(MAGIC);
            writer.Write(VERSION);
            writer.Write(_chunks.Count);

            foreach (var chunk in _chunks)
            {
                chunk.Offset = fs.Position;

                var data = Compress(chunk.Data);
                data = Obfuscate(data);

                writer.Write(data);
                chunk.Length = data.Length;
            }

            long tableOffset = fs.Position;

            foreach (var chunk in _chunks)
            {
                var pathBytes = Encoding.UTF8.GetBytes(chunk.FilePath);
                writer.Write(pathBytes.Length);
                writer.Write(pathBytes);
                writer.Write(chunk.Offset);
                writer.Write(chunk.Length);
            }

            writer.Write(tableOffset);
            writer.Write(FOOTER_MAGIC);
        }

        /// <summary>
        /// Saves the current Aridity PAK archive into a file.
        /// </summary>
        /// <param name="path">The absolute path of the created file.</param>
        /// <param name="token">The value of the cancellation token.</param>
        public async Task SaveAsync(string path, CancellationToken token = default)
        {
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var writer = new BinaryWriter(fs);

            writer.Write(MAGIC);
            writer.Write(VERSION);
            writer.Write(_chunks.Count);

            foreach (var chunk in _chunks)
            {
                chunk.Offset = fs.Position;

                var data = await CompressAsync(chunk.Data);
                data = Obfuscate(data);

                writer.Write(data);
                chunk.Length = data.Length;
            }

            long tableOffset = fs.Position;

            foreach (var chunk in _chunks)
            {
                var pathBytes = Encoding.UTF8.GetBytes(chunk.FilePath);
                writer.Write(pathBytes.Length);
                writer.Write(pathBytes);
                writer.Write(chunk.Offset);
                writer.Write(chunk.Length);
            }

            writer.Write(tableOffset);
            writer.Write(FOOTER_MAGIC);
        }

        /// <summary>
        /// Loads a new Aridity PAK archive from a file.
        /// </summary>
        /// <param name="path">The absolute path to the PAK file.</param>
        /// <returns>A new <seealso cref="PakArchive"/> instance with the information received from the archive file.</returns>
        /// <exception cref="PakException">Thrown if one of the data in the archive is invalid.</exception>
        public static PakArchive Load(string path)
        {
            var pak = new PakArchive();

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(fs))
            {
                var magic = reader.ReadString();
                var version = reader.ReadInt32();
                var count = reader.ReadInt32();

                if (magic != MAGIC)
                    throw new PakException(SR.PakErr_InvalidSignature);

                if (version != VERSION)
                    throw new PakException(SR.PakErr_VersionMismatch);

                fs.Seek(-12, SeekOrigin.End);
                var tableOffset = reader.ReadInt64();
                var footerMagic = reader.ReadUInt32();

                if (footerMagic != FOOTER_MAGIC)
                    throw new PakException(SR.PakErr_InvalidSignature);

                fs.Seek(tableOffset, SeekOrigin.Begin);

                for (int i = 0; i < count; i++)
                {
                    int len = reader.ReadInt32();
                    string pathName = Encoding.UTF8.GetString(reader.ReadBytes(len));
                    long offset = reader.ReadInt64();
                    long size = reader.ReadInt64();

                    pak._chunks.Add(new PakChunk
                    {
                        FilePath = pathName,
                        Offset = offset,
                        Length = size
                    });
                }

                foreach (var c in pak._chunks)
                {
                    fs.Seek(c.Offset, SeekOrigin.Begin);
                    var data = reader.ReadBytes((int)c.Length);
                    data = Obfuscate(data);
                    c.Data = Decompress(data);
                }
            }

            return pak;
        }

        /// <summary>
        /// Loads a new Aridity PAK archive from a file asynchronously.
        /// </summary>
        /// <param name="path">The absolute path to the PAK file.</param>
        /// <param name="token">The value of the cancellation token.</param>
        /// <returns>A new <seealso cref="PakArchive"/> instance with the information received from the archive file.</returns>
        /// <exception cref="PakException">Thrown if one of the data in the archive is invalid.</exception>
        public static async Task<PakArchive> LoadAsync(string path, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            var pak = new PakArchive();

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(fs))
            {
                var magic = reader.ReadString();
                var version = reader.ReadInt32();
                var count = reader.ReadInt32();

                if (magic != MAGIC)
                    throw new PakException(SR.PakErr_InvalidSignature);

                if (version != VERSION)
                    throw new PakException(SR.PakErr_VersionMismatch);

                fs.Seek(-12, SeekOrigin.End);
                var tableOffset = reader.ReadInt64();
                var footerMagic = reader.ReadUInt32();

                if (footerMagic != FOOTER_MAGIC)
                    throw new PakException(SR.PakErr_InvalidSignature);

                fs.Seek(tableOffset, SeekOrigin.Begin);

                for (int i = 0; i < count; i++)
                {
                    int len = reader.ReadInt32();
                    string pathName = Encoding.UTF8.GetString(reader.ReadBytes(len));
                    long offset = reader.ReadInt64();
                    long size = reader.ReadInt64();

                    pak._chunks.Add(new PakChunk
                    {
                        FilePath = pathName,
                        Offset = offset,
                        Length = size
                    });
                }

                foreach (var c in pak._chunks)
                {
                    fs.Seek(c.Offset, SeekOrigin.Begin);
                    var data = reader.ReadBytes((int)c.Length);
                    data = Obfuscate(data);
                    c.Data = await DecompressAsync(data, token);
                }
            }

            return pak;
        }
    }
}
