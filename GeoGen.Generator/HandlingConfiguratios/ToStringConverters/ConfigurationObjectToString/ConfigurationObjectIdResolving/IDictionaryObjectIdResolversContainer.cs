﻿using System.Collections.Generic;
using GeoGen.Core.Configurations;

namespace GeoGen.Generator
{
    /// <summary>
    /// Represents a container of <see cref="DictionaryObjectIdResolver"/> 
    /// objects. They are supposed to be initialized from an enumerable of 
    /// <see cref="LooseConfigurationObject"/>, using all possible permutations 
    /// of their ids. This is used during the process of handling symmetric 
    /// configurations, see the description of <see cref="LeastConfigurationFinder"/>.
    /// It implements the <see cref="IEnumerable{T}"/> interface, where T
    /// is <see cref="DictionaryObjectIdResolver"/>.
    /// </summary>
    internal interface IDictionaryObjectIdResolversContainer : IEnumerable<DictionaryObjectIdResolver>
    {
        DictionaryObjectIdResolver Compose(DictionaryObjectIdResolver first, DictionaryObjectIdResolver second);

        IEnumerable<DictionaryObjectIdResolver> GetNonIdenticalResolvers();
    }
}