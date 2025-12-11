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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AridityTeam.Configuration
{
    /// <inheritdoc/>
    public class ConfigurationManager<T> : IConfigurationManager<T> where T : class
    {
        private readonly XmlSerializerHelper<T> _helper = new();

        private T _currentConfig = null!;
        /// <inheritdoc/>
        public T CurrentConfig => _currentConfig;

        private string? _configPath;

        /// <inheritdoc/>
        public async void LoadConfig(string configurationPath) =>
            await LoadConfigAsync(configurationPath);

        /// <inheritdoc/>
        public Task LoadConfigAsync(string configurationPath)
            => LoadConfigAsync(configurationPath, CancellationToken.None);

        /// <inheritdoc/>
        public async Task LoadConfigAsync(string configurationPath, CancellationToken token = default)
        {
            Requires.FileExists(configurationPath);
            token.ThrowIfCancellationRequested();
            if (!File.Exists(configurationPath) || string.IsNullOrEmpty(File.ReadAllText(configurationPath)))
            {
                await SaveConfigAsync((T)new object(), token);
            }
            _currentConfig = _helper.BytesToObject(File.ReadAllBytes(configurationPath))!;
            _configPath = configurationPath;
        }

        /// <inheritdoc/>
        public async void SaveConfig(T newConfig) =>
            await SaveConfigAsync(newConfig);

        /// <inheritdoc/>
        public Task SaveConfigAsync(T newConfig) =>
            SaveConfigAsync(newConfig, default);

        /// <inheritdoc/>
        public async Task SaveConfigAsync(T newConfig, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            using var stream = File.Open(_configPath!, FileMode.OpenOrCreate, FileAccess.Write);
            using var sw = new StreamWriter(stream);

            var serializer = new XmlSerializer(typeof(T));
            using var writer = XmlWriter.Create(sw, new XmlWriterSettings
            {
                Indent = true,
                Async = true
            });
            serializer.Serialize(writer, newConfig);

            await sw.FlushAsync();
        }
    }
}
