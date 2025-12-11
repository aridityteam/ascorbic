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
using System.Threading;
using System.Threading.Tasks;

namespace AridityTeam.Configuration
{
    /// <summary>
    /// Manages the current configuration by loading a XML configuration file from a file.
    /// </summary>
    public interface IConfigurationManager<T> where T : class
    {
        /// <summary>
        /// The current loaded configuration.
        /// </summary>
        T CurrentConfig { get; }

        /// <summary>
        /// Loads the configuration from a file.
        /// </summary>
        /// <param name="configurationPath">The value of the configuration file path.</param>
        void LoadConfig(string configurationPath);

        /// <summary>
        /// Loads the configuration from a file asynchronously.
        /// </summary>
        /// <param name="configurationPath">The value of the configuration file path.</param>
        Task LoadConfigAsync(string configurationPath);

        /// <summary>
        /// Loads the configuration from a file asynchronously.
        /// </summary>
        /// <param name="configurationPath">The value of the configuration file path.</param>
        /// <param name="token">The value of the cancellation token.</param>
        Task LoadConfigAsync(string configurationPath, CancellationToken token = default);

        /// <summary>
        /// Saves the new configuration, and sets that new one as the current global configuration.
        /// </summary>
        /// <param name="newConfig">The value of the new configuration</param>
        void SaveConfig(T newConfig);

        /// <summary>
        /// Saves the new configuration asynchronously, and sets that new one as the current global configuration.
        /// </summary>
        /// <param name="newConfig">The value of the new configuration</param>
        Task SaveConfigAsync(T newConfig);

        /// <summary>
        /// Saves the new configuration asynchronously, and sets that new one as the current global configuration.
        /// </summary>
        /// <param name="newConfig">The value of the new configuration</param>
        /// <param name="token">The value of the cancellation token.</param>
        Task SaveConfigAsync(T newConfig, CancellationToken token = default);
    }
}
