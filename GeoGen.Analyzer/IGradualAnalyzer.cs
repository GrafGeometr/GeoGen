﻿using System.Collections.Generic;
using GeoGen.Core.Configurations;
using GeoGen.Core.Theorems;

namespace GeoGen.Analyzer
{
    /// <summary>
    /// Represents a service that performs geometrical analysis of a configuration.
    /// It's supposed be used in a single generation process. The idea behind this
    /// analyzer is that it assumes that we have already analyzed a given configuration
    /// without new objects and therefore we just need to analyze the new objects.
    /// </summary>
    public interface IGradualAnalyzer
    {
        /// <summary>
        /// Analyses a given configuration provided as two lists, old and new configuration
        /// objects.
        /// </summary>
        /// <param name="oldObjects">The old objects.</param>
        /// <param name="newObjects">The new objects.</param>
        /// <returns>The analyzer output.</returns>
        List<Theorem> Analyze(IReadOnlyList<ConfigurationObject> oldObjects, IReadOnlyList<ConstructedConfigurationObject> newObjects);
    }
}