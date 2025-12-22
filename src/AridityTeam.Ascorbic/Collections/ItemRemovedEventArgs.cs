// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;

namespace AridityTeam.Collections
{
    /// <summary>
    /// An event argument every time an item is added to the collection.
    /// </summary>
    public class ItemRemovedEventArgs : EventArgs
    {
        /// <summary>
        /// The current type removed from the collection.
        /// </summary>
        public object Type;

        /// <summary>
        /// An event argument every time an item is added to the collection.
        /// </summary>
        /// <remarks>
        /// Initializes a new <seealso cref="ItemRemovedEventArgs"/> with an specific type.
        /// </remarks>
        /// <param name="itemType"></param>
        public ItemRemovedEventArgs(object itemType)
        {
            Type = itemType;
        }
    }
}
