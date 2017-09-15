﻿using System.Collections.Generic;

namespace GeoGen.Core.Utilities.Combinator
{
    /// <summary>
    /// Represents a carthesian product generator from a given dictionary mapping TKey to the enumerable
    /// of elements of a given type. Each generated element is then a dictionary mapping TKeys to TValues.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    public interface ICombinator<TKey, TValue>
    {
        /// <summary>
        /// Generates all possible combinations of elements provided in the possibilities map. This method should
        /// work in a lazy way. For example: For { a, [1, 2] }, { b, [2, 3] } it will generate 4 dictionaries:
        /// { {a, 1}, {b, 2} }, { {a, 1}, {b, 3} }, { {a, 2}, {b, 2} }, { {a, 2}, {b, 3} }.  If there is key with 
        /// no possibilities, the result would be an empty enumerable.
        /// </summary>
        /// <param name="possibilities">The possibilities dictionary.</param>
        /// <returns>The lazy enumerable of resulting combinations.</returns>
        IEnumerable<IReadOnlyDictionary<TKey, TValue>> Combine(IReadOnlyDictionary<TKey, IEnumerable<TValue>> possibilities);
    }
}