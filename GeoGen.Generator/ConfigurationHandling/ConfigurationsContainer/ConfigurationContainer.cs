﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeoGen.Core.Configurations;
using GeoGen.Core.Utilities;
using GeoGen.Core.Utilities.StringBasedContainer;
using GeoGen.Generator.ConfigurationHandling.ConfigurationsConstructing;
using GeoGen.Generator.ConfigurationHandling.ConfigurationToString;
using GeoGen.Generator.ConfigurationHandling.ObjectsContainer;
using GeoGen.Generator.Constructing;
using GeoGen.Generator.Constructing.Arguments.Container;

namespace GeoGen.Generator.ConfigurationHandling.ConfigurationsContainer
{
    /// <summary>
    /// A default implementation of <see cref="IConfigurationContainer"/>.
    /// </summary>
    internal class ConfigurationContainer : StringBasedContainer<Configuration>, IConfigurationContainer
    {
        #region Private fields

        /// <summary>
        /// The arguments container factory
        /// </summary>
        private readonly IArgumentsContainerFactory _argumentsContainerFactory;

        /// <summary>
        /// The symetric configurations handler
        /// </summary>
        private readonly IConfigurationConstructor _configurationConstructor;

        /// <summary>
        /// The configuration to string provider
        /// </summary>
        private readonly IConfigurationToStringProvider _configurationToStringProvider;

        /// <summary>
        /// The configurations container
        /// </summary>
        private readonly IConfigurationObjectsContainer _configurationObjectsContainer;

        #endregion

        #region IConfigurationContainer properties

        /// <summary>
        /// Gets the current layer of unprocessed configurations
        /// </summary>
        public List<ConfigurationWrapper> CurrentLayer { get; } = new List<ConfigurationWrapper>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new configuration container with a given
        /// arguments container facory, a symetric configurations 
        /// handler, a configuration to string provider,
        /// and a configuration objects container.
        /// </summary>
        /// <param name="argumentsContainerFactory">The arguments container factory.</param>
        /// <param name="configurationConstructor">The symetrc configurations handler.</param>
        /// <param name="configurationToStringProvider">The configuration to string provider.</param>
        /// <param name="configurationObjectsContainer">The configuration objects container.</param>
        public ConfigurationContainer
        (
            IArgumentsContainerFactory argumentsContainerFactory,
            IConfigurationConstructor configurationConstructor,
            IConfigurationToStringProvider configurationToStringProvider,
            IConfigurationObjectsContainer configurationObjectsContainer
        )
        {
            _argumentsContainerFactory = argumentsContainerFactory ?? throw new ArgumentNullException(nameof(argumentsContainerFactory));
            _configurationConstructor = configurationConstructor ?? throw new ArgumentNullException(nameof(argumentsContainerFactory));
            _configurationToStringProvider = configurationToStringProvider ?? throw new ArgumentNullException(nameof(argumentsContainerFactory));
            _configurationObjectsContainer = configurationObjectsContainer ?? throw new ArgumentNullException(nameof(argumentsContainerFactory));
        }

        #endregion

        #region IConfigurationContainer methods

        /// <summary>
        /// Initializes the container with a given initial configuration.
        /// </summary>
        /// <param name="initialConfiguration">The initial configuration.</param>
        public void Initialize(Configuration initialConfiguration)
        {
            if (initialConfiguration == null)
                throw new ArgumentNullException(nameof(initialConfiguration));

            // Initialize container with the loose objects
            _configurationObjectsContainer.Initialize(initialConfiguration.LooseObjects);

            // Add all constructed objects. 
            foreach (var constructedObject in initialConfiguration.ConstructedObjects)
            {
                // Resulting object should be the same as this one
                var result = _configurationObjectsContainer.Add(constructedObject);

                // If it's not, we have corruped data
                if (result != constructedObject)
                    throw new GeneratorException("Constructed objects contain two equal objects.");
            }

            // Let the base method add the initial configuration
            Add(initialConfiguration);

            // Create type object map
            var objectsMap = new ConfigurationObjectsMap(initialConfiguration);

            // Create forbidden arguments dictionary
            var forbiddenArguments = CreateForbiddenArguments(initialConfiguration);

            // Create wrapper
            var configurationWrapper = new ConfigurationWrapper
            {
                Configuration = initialConfiguration,
                ConfigurationObjectsMap = objectsMap,
                ForbiddenArguments = forbiddenArguments
            };

            // Set the current layer items
            CurrentLayer.SetItems(configurationWrapper.SingleItemAsEnumerable());
        }

        public static Stopwatch s_newObjects = new Stopwatch();
        public static Stopwatch s_constructingWrapper = new Stopwatch();
        public static Stopwatch s_AddingConfiguration = new Stopwatch();

        /// <summary>
        /// Processes a new layer of a constructor output.
        /// </summary>
        /// <param name="newLayerOutput">The new layer output.</param>
        public void AddLayer(List<ConstructorOutput> newLayerOutput)
        {
            if (newLayerOutput == null)
                throw new ArgumentNullException(nameof(newLayerOutput));

            // take the output
            var newLayer = newLayerOutput
                    // get new configurations
                    .Select
                    (
                        output =>
                        {
                            s_newObjects.Start();
                            // Add objects to container and get identified versions
                            var newObjects = output.ConstructedObjects
                                    .Select(o => _configurationObjectsContainer.Add(o))
                                    .ToList();
                            s_newObjects.Stop();

                            // Re-assign the output
                            output.ConstructedObjects = newObjects;


                            s_constructingWrapper.Start();
                            // Create a new configuration wrapper
                            var configuration = _configurationConstructor.ConstructWrapper(output);
                            s_constructingWrapper.Stop();

                            s_AddingConfiguration.Start();
                            // Add the representant to the container
                            var result = Add(configuration.Configuration);
                            //Console.WriteLine(result);
                            s_AddingConfiguration.Stop();

                            // return the anonymous type wrapping the change result and the object
                            return new {Change = result, Object = configuration};
                        }
                    )
                    // take only objects that caused change of the container (i.e. new ones)
                    .Where(arg => arg.Change)
                    // take the resulting wrapper from them
                    .Select(arg => arg.Object);

            // Set the new layer (which will enumerate the query)
            CurrentLayer.SetItems(newLayer);
        }

        #endregion

        #region StringBasedContainer abstract methods

        /// <summary>
        /// Converts a given item to string.
        /// </summary>
        /// <param name="item">The given item.</param>
        /// <returns>The string representation.</returns>
        protected override string ItemToString(Configuration item)
        {
            var objectToStringProvider = _configurationObjectsContainer.ConfigurationObjectToStringProvider;

            return _configurationToStringProvider.ConvertToString(item, objectToStringProvider);
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Creates a dictionary mapping a construction id to an arguments container
        /// that contains all forbidden arguments for this construction. This is 
        /// supposed to be used for an initial configuration. At that stage we can
        /// only forbid all constructed objects that are already contained within
        /// the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The dictionary.</returns>
        private Dictionary<int, IArgumentsContainer> CreateForbiddenArguments(Configuration configuration)
        {
            var result = new Dictionary<int, IArgumentsContainer>();

            foreach (var constructedObject in configuration.ConstructedObjects)
            {
                var id = constructedObject.Construction.Id ?? throw new GeneratorException("Construction id must be set");

                if (!result.ContainsKey(id))
                {
                    var container = _argumentsContainerFactory.CreateContainer();
                    result.Add(id, container);
                }

                result[id].AddArguments(constructedObject.PassedArguments);
            }

            return result;
        }

        #endregion
    }
}