﻿using System;
using System.Collections.Generic;
using GeoGen.Core.Configurations;

namespace GeoGen.Generator.ConfigurationHandling.ConfigurationObjectToString.ObjectIdResolving
{
    /// <summary>
    /// An implementation of <see cref="IObjectIdResolver"/> that uses a dictionary
    /// mapping actual ids to resolved ids.
    /// </summary>
    internal class DictionaryObjectIdResolver : IObjectIdResolver
    {
        /// <summary>
        /// The id of the resolver.
        /// </summary>
        public int Id { get; }

        #region Private fields

        /// <summary>
        /// The real id to the resolved id dictionary
        /// </summary>
        private readonly Dictionary<int, int> _realIdToResolvedId;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new dictionary based resolver with a given ids dictionary
        /// and with a given id.
        /// </summary>
        /// <param name="realIdToResolvedId">The dictionary mapping real ids to resolved ids.</param>
        /// <param name="id">The id.</param>
        public DictionaryObjectIdResolver(Dictionary<int, int> realIdToResolvedId, int id)
        {
            Id = id;
            _realIdToResolvedId = realIdToResolvedId ?? throw new ArgumentNullException(nameof(realIdToResolvedId));
        }

        /// <summary>
        /// Constructs a new dictionary based resolver with an empty dictionary.
        /// </summary>
        public DictionaryObjectIdResolver()
            : this(new Dictionary<int, int>(), 0)
        {
        }

        #endregion

        #region IObjectIdResolver implementation

        /// <summary>
        /// Resolve the id of a given configuration object.
        /// </summary>
        /// <param name="configurationObject">The configuration object.</param>
        /// <returns>The id.</returns>
        public int ResolveId(ConfigurationObject configurationObject)
        {
            if (configurationObject == null)
                throw new ArgumentNullException(nameof(configurationObject));

            var id = configurationObject.Id ?? throw new GeneratorException("Configuration object without id");

            return _realIdToResolvedId[id];
        }

        #endregion
    }
}