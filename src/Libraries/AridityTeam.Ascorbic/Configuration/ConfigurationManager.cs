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
