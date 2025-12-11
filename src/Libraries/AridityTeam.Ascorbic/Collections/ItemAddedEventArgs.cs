// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;

namespace AridityTeam.Collections
{
    /// <summary>
    /// An event argument every time an item is added to the collection.
    /// </summary>
    public class ItemAddedEventArgs : EventArgs
    {
        /// <summary>
        /// The current type added to the collection.
        /// </summary>
        public object Type;

        /// <summary>
        /// An event argument every time an item is added to the collection.
        /// </summary>
        /// <remarks>
        /// Initializes a new <seealso cref="ItemAddedEventArgs"/> with an specific type.
        /// </remarks>
        /// <param name="itemType"></param>
        public ItemAddedEventArgs(object itemType)
        {
            Type = itemType;
        }
    }
}
