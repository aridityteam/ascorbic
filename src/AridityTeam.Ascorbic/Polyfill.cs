﻿/*
 * Copyright (c) 2026 The Aridity Team
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
using AridityTeam;

#if !NET9_0_OR_GREATER
#if NET8_0
namespace AridityTeam
#else
namespace System.Diagnostics.CodeAnalysis
#endif
{
    /// <summary>
    ///  Indicates that an API is experimental and it may change in the future.
    /// </summary>
    /// <remarks>
    ///   This attribute allows call sites to be flagged with a diagnostic that indicates that an experimental
    ///   feature is used. Authors can use this attribute to ship preview features in their assemblies.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Assembly |
                    AttributeTargets.Module |
                    AttributeTargets.Class |
                    AttributeTargets.Struct |
                    AttributeTargets.Enum |
                    AttributeTargets.Constructor |
                    AttributeTargets.Method |
                    AttributeTargets.Property |
                    AttributeTargets.Field |
                    AttributeTargets.Event |
                    AttributeTargets.Interface |
                    AttributeTargets.Delegate, Inherited = false)]
    internal partial class ExperimentalAttribute : Attribute
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="ExperimentalAttribute"/> class, specifying the ID that the compiler will use
        ///  when reporting a use of the API the attribute applies to.
        /// </summary>
        /// <param name="diagnosticId">The ID that the compiler will use when reporting a use of the API the attribute applies to.</param>
        public ExperimentalAttribute(string diagnosticId)
        {
            DiagnosticId = diagnosticId;
        }

        /// <summary>
        ///  Gets the ID that the compiler will use when reporting a use of the API the attribute applies to.
        /// </summary>
        /// <value>The unique diagnostic ID.</value>
        /// <remarks>
        ///  The diagnostic ID is shown in build output for warnings and errors.
        ///  <para>This property represents the unique ID that can be used to suppress the warnings or errors, if needed.</para>
        /// </remarks>
        public string DiagnosticId { get; }

        /// <summary>
        ///  Gets or sets an optional message associated with the experimental attribute.
        /// </summary>
        /// <value>The message that provides additional information about the experimental feature.</value>
        /// <remarks>
        ///  This message can be used to provide more context or guidance about the experimental feature.
        /// </remarks>
        public string? Message { get; set; }

        /// <summary>
        ///  Gets or sets the URL for corresponding documentation.
        ///  The API accepts a format string instead of an actual URL, creating a generic URL that includes the diagnostic ID.
        /// </summary>
        /// <value>The format string that represents a URL to corresponding documentation.</value>
        /// <remarks>An example format string is <c>https://contoso.com/obsoletion-warnings/{0}</c>.</remarks>
        public string? UrlFormat { get; set; }
    }
}
#endif

#if !NET5_0_OR_GREATER || !NETCOREAPP3_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    internal class IsExternalInit { }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }

    /// <summary>Indicates the attributed type is to be used as an interpolated string handler.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    internal sealed class InterpolatedStringHandlerAttribute : Attribute
    {
        /// <summary>Initializes the <see cref="InterpolatedStringHandlerAttribute"/>.</summary>
        public InterpolatedStringHandlerAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class InterpolatedStringHandlerArgumentAttribute : Attribute
    {
        public InterpolatedStringHandlerArgumentAttribute(string argument) => this.Arguments = new string[] { argument };

        public InterpolatedStringHandlerArgumentAttribute(params string[] arguments) => this.Arguments = arguments;

        public string[] Arguments { get; }
    }
}
#endif

#if !NET8_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>Specifies the syntax used in a string.</summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal sealed class StringSyntaxAttribute : Attribute
    {
        /// <summary>Initializes the <see cref="StringSyntaxAttribute"/> with the identifier of the syntax used.</summary>
        /// <param name="syntax">The syntax identifier.</param>
        public StringSyntaxAttribute(string syntax)
        {
            Syntax = syntax;
            Arguments = new object[] { };
        }

        /// <summary>Initializes the <see cref="StringSyntaxAttribute"/> with the identifier of the syntax used.</summary>
        /// <param name="syntax">The syntax identifier.</param>
        /// <param name="arguments">Optional arguments associated with the specific syntax employed.</param>
        public StringSyntaxAttribute(string syntax, params object[] arguments)
        {
            Syntax = syntax;
            Arguments = arguments;
        }

        /// <summary>Gets the identifier of the syntax used.</summary>
        public string Syntax { get; }

        /// <summary>Optional arguments associated with the specific syntax employed.</summary>
        public object[] Arguments { get; }

        /// <summary>The syntax identifier for strings containing composite formats for string formatting.</summary>
        public const string CompositeFormat = nameof(CompositeFormat);

        /// <summary>The syntax identifier for strings containing date format specifiers.</summary>
        public const string DateOnlyFormat = nameof(DateOnlyFormat);

        /// <summary>The syntax identifier for strings containing date and time format specifiers.</summary>
        public const string DateTimeFormat = nameof(DateTimeFormat);

        /// <summary>The syntax identifier for strings containing <see cref="Enum"/> format specifiers.</summary>
        public const string EnumFormat = nameof(EnumFormat);

        /// <summary>The syntax identifier for strings containing <see cref="Guid"/> format specifiers.</summary>
        public const string GuidFormat = nameof(GuidFormat);

        /// <summary>The syntax identifier for strings containing JavaScript Object Notation (JSON).</summary>
        public const string Json = nameof(Json);

        /// <summary>The syntax identifier for strings containing numeric format specifiers.</summary>
        public const string NumericFormat = nameof(NumericFormat);

        /// <summary>The syntax identifier for strings containing regular expressions.</summary>
        public const string Regex = nameof(Regex);

        /// <summary>The syntax identifier for strings containing time format specifiers.</summary>
        public const string TimeOnlyFormat = nameof(TimeOnlyFormat);

        /// <summary>The syntax identifier for strings containing <see cref="TimeSpan"/> format specifiers.</summary>
        public const string TimeSpanFormat = nameof(TimeSpanFormat);

        /// <summary>The syntax identifier for strings containing URIs.</summary>
        public const string Uri = nameof(Uri);

        /// <summary>The syntax identifier for strings containing XML.</summary>
        public const string Xml = nameof(Xml);
    }
}
#endif

#if !NET8_0_OR_GREATER

namespace System.Runtime.Versioning
{
    /// <summary>
    /// stubbed
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class SupportedOSPlatformAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformName"></param>
        public SupportedOSPlatformAttribute(string platformName)
        {
        }
    }
}

namespace System.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue>? dict, TKey key, TValue value) where TKey : class
        {
            Requires.NotNull(dict);

            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
                return true;
            }

            return false;
        }
    }
}
#endif
