﻿using System.Collections.Generic;

namespace GeoGen.Utilities.Subsets
{
    public interface ISubsetsProvider
    {
        IEnumerable<IEnumerable<T>> GetSubsets<T>(IReadOnlyList<T> list, int numberOfElements);
    }
}