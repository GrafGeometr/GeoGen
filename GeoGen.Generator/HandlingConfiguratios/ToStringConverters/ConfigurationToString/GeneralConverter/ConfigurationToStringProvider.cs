﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeoGen.Core.Configurations;
using GeoGen.Utilities;

namespace GeoGen.Generator
{
    /// <summary>
    /// A default implementation of <see cref="IConfigurationToStringProvider"/>.
    /// It simply converts each object to string and join these strings using the default
    /// separator. This sealed class is thread-safe.
    /// </summary>
    internal sealed class ConfigurationToStringProvider : IConfigurationToStringProvider
    {
        #region Private constants

        /// <summary>
        /// The objects separator.
        /// </summary>
        private const string ObjectsSeparator = "|";

        #endregion

        #region Private fields

        /// <summary>
        /// The cache dictionary mapping ids of object converters to the dictionary 
        /// mapping converted configurations ids to the sorted set of their
        /// constructed objects.
        /// </summary>
        private readonly Dictionary<int, Dictionary<int, SortedSet<string>>> _cachedSets;

        /// <summary>
        /// The initial configuration.
        /// </summary>
        private readonly Configuration _initialConfiguration;

        /// <summary>
        /// The composer of object id resolvers.
        /// </summary>
        private readonly IResolversComposer _composer;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a configuration to string provider that has the reference
        /// to the initial configuration which it uses as the base for recursive
        /// to string conversion.
        /// </summary>
        /// <param name="initialConfiguration">The initial configuration.</param>
        public ConfigurationToStringProvider(Configuration initialConfiguration, IResolversComposer composer)
        {
            _initialConfiguration = initialConfiguration ?? throw new ArgumentNullException(nameof(initialConfiguration));
            _composer = composer ?? throw new ArgumentNullException(nameof(composer));
            _cachedSets = new Dictionary<int, Dictionary<int, SortedSet<string>>>();
        }

        #endregion

        #region IConfigurationToStringProvider implementation

        /// <summary>
        /// Converts a given configuration to string, using a given 
        /// configuration object to string provider.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="objectToString">The configuration object to string provider.</param>
        /// <returns>The string representation of the configuration.</returns>
        public string ConvertToString(ConfigurationWrapper configuration, IObjectToStringConverter objectToString)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (objectToString == null)
                throw new ArgumentNullException(nameof(objectToString));

            // Resolve the cached value
            var cachedValue = ResolveCache(configuration, objectToString);

            // If there's any, return it
            if (cachedValue != string.Empty)
                return cachedValue;

            // Get the dictionary for the converter
            var dictionary = GetDictionaryForResolver(objectToString.Resolver);

            // Pull initial configuration
            var previousConfiguration = configuration.PreviousConfiguration;

            // Unwrap configuration.
            var unwraped = previousConfiguration?.Configuration ?? throw new GeneratorException("Attempt to convert initial configuration directly");

            // Find out if it's initial configuration (which is not supposed be converted)
            var isInitial = _initialConfiguration == unwraped;

            // We should convert the initial configuration only as the first converted one
            var shouldWeConvertInitialFirst = isInitial && dictionary.Empty();

            // If it's initial, we need to convert this one first
            if (shouldWeConvertInitialFirst)
            {
                // Initialize the new result
                var initialResult = new SortedSet<string>();

                // Add all constructed objects to the set
                foreach (var newObject in unwraped.ConstructedObjects)
                {
                    // Convert object to string
                    var stringValue = objectToString.ConvertToString(newObject);

                    // Add it to the set
                    initialResult.Add(stringValue);
                }

                // Pull id of the configuration that should be the initial one
                var id = previousConfiguration.Id ?? throw GeneratorException.ConfigurationIdNotSet();

                // Cache the result
                dictionary.Add(id, initialResult);
            }

            // Let the helper method find the set of strings corresponding to the
            // original configuration (with regards to current converter)
            var originalConfigurationSet = FindOriginalConfigurationSet(configuration, objectToString);

            // Copy the set. This should be O(n)
            var result = new SortedSet<string>(originalConfigurationSet);

            // Add new objects to the new set
            foreach (var newObject in configuration.LastAddedObjects)
            {
                // Convert object to string
                var stringValue = objectToString.ConvertToString(newObject);

                // Add it to the set
                result.Add(stringValue);
            }

            // Pull id of the current configuration
            var currentId = configuration.Id ?? throw GeneratorException.ConfigurationIdNotSet();

            // Cache the result
            dictionary.Add(currentId, result);

            // Construct joined result
            var resultString = string.Join(ObjectsSeparator, result);

            // Return the result
            return resultString;
        }

        private string ResolveCache(ConfigurationWrapper configuration, IObjectToStringConverter objectToString)
        {
            // Pull id of the current configuration
            var currentId = configuration.Id ?? throw GeneratorException.ConfigurationIdNotSet();

            // Compose the real converter
            var converter = _composer.Compose(objectToString.Resolver, configuration.ResolverToMinimalForm);

            // Get dictionary for it
            var dictionary = GetDictionaryForResolver(converter);

            // If there is the current id
            if (dictionary.ContainsKey(currentId))
            {
                // Return joined strings
                return string.Join(ObjectsSeparator, dictionary[currentId]);
            }

            // Otherwise return empty string
            return string.Empty;
        }

        private SortedSet<string> FindOriginalConfigurationSet(ConfigurationWrapper configuration, IObjectToStringConverter objectToString)
        {
            // Pull id of previous configuration
            var previousId = configuration.PreviousConfiguration.Id ?? throw GeneratorException.ConfigurationIdNotSet();

            // Pull the minimal form resolver of the previous configuration
            var previousResolver = configuration.PreviousConfiguration.ResolverToMinimalForm;

            // Compose it with the current resolver
            var originalConfigurationResolver = _composer.Compose(previousResolver, objectToString.Resolver);

            // And now we can return the original configuration
            return _cachedSets[originalConfigurationResolver.Id][previousId];
        }

        private Dictionary<int, SortedSet<string>> GetDictionaryForResolver(IObjectIdResolver resolver)
        {
            // Pull resolver id
            var resolverId = resolver.Id;

            // If the cache doesn't contain this id
            if (!_cachedSets.ContainsKey(resolverId))
            {
                // Add new dictionary to the cache
                _cachedSets.Add(resolverId, new Dictionary<int, SortedSet<string>>());
            }

            // Return the cache dictionary
            return _cachedSets[resolverId];
        }

        #endregion
    }
}