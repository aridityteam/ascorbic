// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AridityTeam.Collections.Concurrent
{
    /// <summary>
    /// *sigh*
    /// A dumb implementation of <seealso cref="ConcurrentBag{T}"/>.
    /// Too bad!
    /// </summary>
    /// <remarks>
    /// This implementation provides support for removing and clearing items, which
    /// <see cref="ConcurrentBag{T}"/> does not natively support.
    /// </remarks>
    public class RemovableConcurrentBag<T> : IEnumerable<T> where T : class
    {
        private ConcurrentDictionary<T, int> _dict = new();

        /// <inheritdoc cref="ConcurrentBag{T}.Add(T)"/>
        public virtual void Add(T item)
        {
            _dict.AddOrUpdate(item, 1, (_, count) => count + 1);
        }

        /// <summary>
        /// Clears the whole collection by deleting every item currently in the collection.
        /// </summary>
        public virtual void Clear() => _dict = new ConcurrentDictionary<T, int>();

        /// <summary>
        /// Removes an item from the <seealso cref="ConcurrentBag{T}"/>.
        /// </summary>
        /// <param name="item">The value of the item.</param>
        /// <returns>
        /// <see langword="true"/> if an item was removed;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public virtual bool Remove(T item)
        {
            while (true)
            {
                if (!_dict.TryGetValue(item, out int count))
                    return false;

                if (count > 1)
                {
                    if (_dict.TryUpdate(item, count - 1, count))
                        return true;
                }
                else
                {
                    if (_dict.TryRemove(item, out _))
                        return true;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the bag.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var kv in _dict)
                for (int i = 0; i < kv.Value; i++)
                    yield return kv.Key;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
