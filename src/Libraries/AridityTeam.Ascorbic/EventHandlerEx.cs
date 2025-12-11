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

namespace AridityTeam
{
    /// <summary>
    /// Extension methods to make it easier to safely invoke events.
    /// </summary>
    public static class EventHandlerEx
    {
        /// <summary>
        /// Invokes any event handlers that are hooked to the specified event.
        /// </summary>
        /// <param name="handler">The event.  Null is allowed.</param>
        /// <param name="sender">The value to pass as the sender of the event.</param>
        /// <param name="e">Event arguments to include.</param>
        public static void Raise(this Delegate handler, object sender, EventArgs e)
        {
            Requires.NotNull(sender, nameof(sender));
            Requires.NotNull(e, nameof(e));

            handler?.DynamicInvoke(sender, e);
        }

        /// <summary>
        /// Invokes any event handlers that are hooked to the specified event.
        /// </summary>
        /// <param name="handler">The event.  Null is allowed.</param>
        /// <param name="sender">The value to pass as the sender of the event.</param>
        /// <param name="e">Event arguments to include.</param>
        public static void Raise(this EventHandler handler, object sender, EventArgs e)
        {
            Requires.NotNull(sender, nameof(sender));
            Requires.NotNull(e, nameof(e));

            handler?.Invoke(sender, e);
        }

        /// <summary>
        /// Invokes any event handlers that are hooked to the specified event.
        /// </summary>
        /// <typeparam name="T">The type of EventArgs.</typeparam>
        /// <param name="handler">The event.  Null is allowed.</param>
        /// <param name="sender">The value to pass as the sender of the event.</param>
        /// <param name="e">Event arguments to include.</param>
        public static void Raise<T>(this EventHandler<T> handler, object sender, T e)
        {
            Requires.NotNull(sender, nameof(sender));
            Requires.NotNullAllowStructs(e, nameof(e));

            handler?.Invoke(sender, e);
        }
    }
}
