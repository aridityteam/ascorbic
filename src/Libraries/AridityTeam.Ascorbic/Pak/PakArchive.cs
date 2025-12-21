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
        private const string MAGIC = "APAK";
        private const uint FOOTER_MAGIC = 0x4B41504B; // 'KAPK'
        private const int VERSION = 1;
        private const int BLOCK_SIZE = 1024 * 1024;

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
            Requires.NotNullOrEmpty(data);

            var chunk = new PakChunk()
            {
                FilePath = fullPath.Replace('\\', '/'),
                Data = data
            };

            int offset = 0;

            while (offset < data.Length)
            {
                int size = Math.Min(BLOCK_SIZE, data.Length - offset);

                byte[] slice = new byte[size];
                Buffer.BlockCopy(data, offset, slice, 0, size);

                slice = Compress(slice);
                slice = Obfuscate(slice);

                chunk.Blocks.Add(new PakBlock
                {
                    Offset = offset,
                    Length = slice.Length,
                });

                offset += size;
            }

            _chunks.Add(chunk);
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

        private static byte[] Decompress(byte[] data)
        {
            using var input = new MemoryStream(data);
            using var deflate = new DeflateStream(
                input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            deflate.CopyTo(output);
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

            writer.Write(Encoding.ASCII.GetBytes(MAGIC));
            writer.Write(VERSION);
            writer.Write(_chunks.Count);

            foreach (var chunk in _chunks)
            {
                chunk.Blocks.Clear();
                chunk.OriginalSize = chunk.Data.Length;

                int pos = 0;
                while (pos < chunk.Data.Length)
                {
                    int size = Math.Min(BLOCK_SIZE, chunk.Data.Length - pos);
                    byte[] blockData = new byte[size];
                    Buffer.BlockCopy(chunk.Data, pos, blockData, 0, size);

                    blockData = Obfuscate(Compress(blockData));

                    var block = new PakBlock();
                    block.Offset = fs.Position;
                    block.Length = blockData.Length;
                    block.Size = size;

                    writer.Write(blockData);
                    chunk.Blocks.Add(block);

                    pos += size;
                }
            }

            long indexOffset = fs.Position;

            foreach (var chunk in _chunks)
            {
                byte[] pathBytes = Encoding.UTF8.GetBytes(chunk.FilePath);
                writer.Write(pathBytes.Length);
                writer.Write(pathBytes);

                writer.Write(chunk.OriginalSize);
                writer.Write(chunk.Blocks.Count);

                foreach (var block in chunk.Blocks)
                {
                    writer.Write(block.Offset);
                    writer.Write(block.Length);
                    writer.Write(block.Size);
                }
            }

            writer.Write(indexOffset);
            writer.Write(FOOTER_MAGIC);
        }

        /// <summary>
        /// Saves the current Aridity PAK archive into a file.
        /// </summary>
        /// <param name="path">The absolute path of the created file.</param>
        /// <param name="token">The value of the cancellation token.</param>
        public Task SaveAsync(string path, CancellationToken token = default) =>
            Task.Factory.StartNew(() => Save(path), token);

        /// <summary>
        /// Loads a new Aridity PAK archive from a file.
        /// </summary>
        /// <param name="path">The absolute path to the PAK file.</param>
        /// <returns>A new <seealso cref="PakArchive"/> instance with the information received from the archive file.</returns>
        /// <exception cref="PakException">Thrown if one of the read data in the archive is incorrect/invalid.</exception>
        public static PakArchive Load(string path)
        {
            var pak = new PakArchive();

            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(fs);

            var magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
            var version = reader.ReadInt32();
            var count = reader.ReadInt32();

            if (magic != MAGIC)
                throw new PakException(SR.PakErr_InvalidSignature);

            if (version != VERSION)
                throw new PakException(SR.PakErr_VersionMismatch);

            fs.Seek(-12, SeekOrigin.End);
            long indexOffset = reader.ReadInt64();
            uint footerMagic = reader.ReadUInt32();

            if (footerMagic != FOOTER_MAGIC)
                throw new PakException(SR.PakErr_InvalidSignature);

            fs.Seek(indexOffset, SeekOrigin.Begin);

            for (int i = 0; i < count; i++)
            {
                int pathLen = reader.ReadInt32();
                string pathName = Encoding.UTF8.GetString(reader.ReadBytes(pathLen));

                int originalSize = reader.ReadInt32();
                int blockCount = reader.ReadInt32();

                var chunk = new PakChunk
                {
                    FilePath = pathName,
                    OriginalSize = originalSize
                };

                for (int b = 0; b < blockCount; b++)
                {
                    var block = new PakBlock();
                    block.Offset = reader.ReadInt64();
                    block.Length = reader.ReadInt32();
                    block.Size = reader.ReadInt32();

                    chunk.Blocks.Add(block);
                }

                pak._chunks.Add(chunk);
            }

            foreach (var c in pak._chunks)
            {
                byte[] result = new byte[c.OriginalSize];
                int writePos = 0;

                foreach (var block in c.Blocks)
                {
                    if (block.Offset < 0 || block.Offset >= fs.Length)
                        throw new PakException("Invalid block offset");

                    fs.Seek(block.Offset, SeekOrigin.Begin);
                    byte[] compressed = reader.ReadBytes(block.Length);

                    byte[] data = Decompress(Obfuscate(compressed));
                    Buffer.BlockCopy(data, 0, result, writePos, block.Size);
                    writePos += block.Size;
                }

                c.Data = result;
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
        public static Task<PakArchive> LoadAsync(string path, CancellationToken token = default) =>
            Task.Factory.StartNew(() => Load(path), token);
    }
}
