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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AridityTeam.IO
{
    /// <summary>
    /// A <seealso cref="TextWriter"/> that can write to all outputs.
    /// </summary>
    public class MultiTextWriter : TextWriter
    {
        /// <inheritdoc/>
        public override Encoding Encoding => Encoding.ASCII;

        private readonly ConcurrentBag<TextWriter> _writers;

        /// <summary>
        /// Initializes a new <seealso cref="MultiTextWriter"/> without any initial writers.
        /// </summary>
        public MultiTextWriter()
        {
            _writers = new ConcurrentBag<TextWriter>();
        }

        /// <summary>
        /// Initializes a new <seealso cref="MultiTextWriter"/> with multiple writers.
        /// </summary>
        public MultiTextWriter(params TextWriter[] writers)
        {
            _writers = new ConcurrentBag<TextWriter>(writers);
        }

        /// <summary>
        /// Initializes a new <seealso cref="MultiTextWriter"/> with multiple writers.
        /// </summary>
        public MultiTextWriter(IEnumerable<TextWriter> writers)
        {
            _writers = new ConcurrentBag<TextWriter>(writers);
        }

        /// <summary>
        /// Initializes a new <seealso cref="MultiTextWriter"/> without initial writers,
        /// but with an new <seealso cref="IFormatProvider"/>.
        /// </summary>
        public MultiTextWriter(IFormatProvider formatProvider) : base(formatProvider)
        {
            _writers = new ConcurrentBag<TextWriter>();
        }

        /// <summary>
        /// Adds an existing <seealso cref="TextWriter"/> into the <seealso cref="MultiTextWriter"/>.
        /// </summary>
        /// <param name="writer">The value of the writer.</param>
        public void AddWriter(TextWriter writer)
        {
            _writers.Add(writer);
        }

        /// <inheritdoc/>
        public override void Write(bool value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(char value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(char[]? buffer)
        {
            foreach (var writer in _writers)
                writer.Write(buffer);
        }

        /// <inheritdoc/>
        public override void Write(char[] buffer, int index, int count)
        {
            foreach (var writer in _writers)
                writer.Write(buffer, index, count);
        }

        /// <inheritdoc/>
        public override void Write(int value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(string? value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
        {
            foreach (var writer in _writers)
                writer.Write(format, arg0);
        }

        /// <inheritdoc/>
        public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
        {
            foreach (var writer in _writers)
                writer.Write(format, arg0);
        }

        /// <inheritdoc/>
        public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1,
            object? arg2)
        {
            foreach (var writer in _writers)
                writer.Write(format, arg0, arg1, arg2);
        }

        /// <inheritdoc/>
        public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args)
        {
            foreach (var writer in _writers)
                writer.Write(format, args);
        }

        /// <inheritdoc/>
        public override void Write(object? value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(long value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(ulong value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(uint value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(float value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(double value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void Write(decimal value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        /// <inheritdoc/>
        public override void WriteLine()
        {
            foreach (var writer in _writers)
                writer.WriteLine();
        }

        /// <inheritdoc/>
        public override void WriteLine(char value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(char[]? buffer)
        {
            foreach (var writer in _writers)
                writer.WriteLine(buffer);
        }

        /// <inheritdoc/>
        public override void WriteLine(char[] buffer, int index, int count)
        {
            foreach (var writer in _writers)
                writer.WriteLine(buffer, index, count);
        }

        /// <inheritdoc/>
        public override void WriteLine(bool value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(int value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(uint value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(long value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(ulong value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(float value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(double value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(decimal value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(string? value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine(object? value)
        {
            foreach (var writer in _writers)
                writer.WriteLine(value);
        }

        /// <inheritdoc/>
        public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
        {
            foreach (var writer in _writers)
                writer.WriteLine(format, arg0);
        }

        /// <inheritdoc/>
        public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
        {
            foreach (var writer in _writers)
                writer.WriteLine(format, arg0, arg1);
        }

        /// <inheritdoc/>
        public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
        {
            foreach (var writer in _writers)
                writer.WriteLine(format, arg0, arg1, arg2);
        }

        /// <inheritdoc/>
        public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args)
        {
            foreach (var writer in _writers)
                writer.WriteLine(format, args);
        }


        /// <inheritdoc/>
        public override void Flush()
        {
            foreach (var writer in _writers)
                writer.Flush();
        }

        /// <inheritdoc/>
        public override async Task FlushAsync()
        {
            foreach (var writer in _writers)
                await writer.FlushAsync();
        }

        /// <inheritdoc/>
        public override async Task WriteAsync(char value)
        {
            foreach (var writer in _writers)
                await writer.WriteAsync(value);
        }

        /// <inheritdoc/>
        public override async Task WriteAsync(char[] buffer, int index, int count)
        {
            foreach (var writer in _writers)
                await writer.WriteAsync(buffer, index, count);
        }

        /// <inheritdoc/>
        public override async Task WriteAsync(string? value)
        {
            foreach (var writer in _writers)
                await writer.WriteAsync(value);
        }

        /// <inheritdoc/>
        public override async Task WriteLineAsync()
        {
            foreach (var writer in _writers)
                await writer.WriteLineAsync();
        }

        /// <inheritdoc/>
        public override async Task WriteLineAsync(char value)
        {
            foreach (var writer in _writers)
                await writer.WriteLineAsync(value);
        }

        /// <inheritdoc/>
        public override async Task WriteLineAsync(string? value)
        {
            foreach (var writer in _writers)
                await writer.WriteLineAsync(value);
        }

        /// <inheritdoc/>
        public override async Task WriteLineAsync(char[] buffer, int index, int count)
        {
            foreach (var writer in _writers)
                await writer.WriteLineAsync(buffer, index, count);
        }

        /// <inheritdoc/>
        public override void Close()
        {
            foreach (var writer in _writers)
                writer.Close();

            _writers.TryTake(out var _);
        }
    }
}
