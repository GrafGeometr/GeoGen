﻿using System;
using System.Collections.Generic;
using GeoGen.Generator.ConfigurationHandling.ConfigurationObjectToString.ObjectIdResolving;
using GeoGen.Generator.Constructing.Arguments.ArgumentsToString;

namespace GeoGen.Generator.ConfigurationHandling.ConfigurationObjectToString
{
    /// <summary>
    /// A default implementation of <see cref="ICustomFullObjectToStringProviderFactory"/>.
    /// </summary>
    internal class CustomFullObjectToStringProviderFactory : ICustomFullObjectToStringProviderFactory
    {
        #region Private fields

        /// <summary>
        /// The arguments to string provider.
        /// </summary>
        private readonly IArgumentsToStringProvider _argumentsToStringProvider;

        /// <summary>
        /// The dictionary mapping ids of dictionary object id resolvers
        /// to particular custom full object to string providers.
        /// </summary>
        private readonly Dictionary<int, CustomFullObjectToStringProvider> _cache;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new custom full object to string provider factory
        /// with a given arguments to string provider.
        /// </summary>
        /// <param name="provider">The arguments to string provider.</param>
        public CustomFullObjectToStringProviderFactory(IArgumentsToStringProvider provider)
        {
            _argumentsToStringProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            _cache = new Dictionary<int, CustomFullObjectToStringProvider>();
        }

        #endregion

        #region IObjectToStringProviderFactory implementation

        /// <summary>
        /// Creates an instance of <see cref="CustomFullObjectToStringProvider"/>
        /// with a given dictionary object id resolver.
        /// </summary>
        /// <param name="resolver">The dictionary object id resolver.</param>
        /// <returns>The custom full object to string provider.</returns>
        public CustomFullObjectToStringProvider GetCustomProvider(DictionaryObjectIdResolver resolver)
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver));

            var id = resolver.Id;

            if (_cache.ContainsKey(id))
                return _cache[id];

            var newResolver = new CustomFullObjectToStringProvider(_argumentsToStringProvider, resolver);
            _cache.Add(id, newResolver);

            return newResolver;
        }

        #endregion
    }
}