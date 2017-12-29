﻿using System.Collections.Generic;
using System.Linq;
using GeoGen.Analyzer;
using GeoGen.Core.Configurations;
using GeoGen.Utilities;

namespace GeoGen.Generator
{
    internal sealed class ConfigurationsResolver : IConfigurationsResolver
    {
        private readonly IGeometryRegistrar _registrar;

        private readonly IConfigurationObjectsContainer _container;

        private readonly HashSet<int> _duplicateObjectsIds;

        private readonly HashSet<int> _unconstructibleObjectsIds;

        public ConfigurationsResolver(IGeometryRegistrar registrar, IConfigurationObjectsContainer container)
        {
            _registrar = registrar;
            _container = container;
            _duplicateObjectsIds = new HashSet<int>();
            _unconstructibleObjectsIds = new HashSet<int>();
        }

        public bool ResolveNewOutput(ConstructorOutput output)
        {
            // Let the helper method construct new objects and find out if some 
            // of them aren't forbidden or duplicate
            var newObjects = ConstructNewObjects(output, out var correctObjects);

            // If the new objects aren't correct, return failure
            if (!correctObjects)
                return false;

            // Otherwise re-assign the output (we need to do this because the interior
            // objects might have changed, because the original ones had no ids wheres
            // these have been identified by the container)
            output.ConstructedObjects = newObjects;

            // And return success
            return true;
        }

        public bool ResolveMappedOutput(ConfigurationWrapper configuration)
        {
            // Iterate over constructed objects representing an output
            // of a single construction
            foreach (var constructedObjects in configuration.Configuration.GroupConstructedObjects())
            {
                // Find ids
                var ids = constructedObjects
                          .Select(o => o.Id ?? throw GeneratorException.ObjectsIdNotSet())
                          .ToSet();

                // Find out if there is a duplicate
                var duplicate = ids.Any(_duplicateObjectsIds.Contains);

                // If it is duplicate, return failure
                if (duplicate)
                    return false;

                // Find out if it is unconstructible
                var unconstructible = ids.Any(_unconstructibleObjectsIds.Contains);

                // If it is unconstructible, then we have a geometrical inconsistency,
                // because the original object was resolved as OK
                if (unconstructible)
                    throw new GeneratorException("Geometrical inconsistency between original object and mapped object");

                // Otherwise we register the objects using the method
                var registrationResult = Register(constructedObjects);

                // Switch over the result
                switch (registrationResult)
                {
                    case RegistrationResult.Unconstructible:
                        // If the objects are not constructible, then again we have the inconsistency
                        throw new GeneratorException("Geometrical inconsistency between original object and mapped object");
                    case RegistrationResult.Duplicates:
                        // If the objects are duplicates, we'll return the failure
                        return false;
                }
            }

            // If we got here, we have correct objects
            return true;
        }

        public void ResolveInitialConfiguration(ConfigurationWrapper configuration)
        {
            // Iterate over all constructed objects
            foreach (var constructedObjects in configuration.Configuration.GroupConstructedObjects())
            {
                // Otherwise we register the objects using the method
                var registrationResult = Register(constructedObjects);

                // Switch over the result
                switch (registrationResult)
                {
                    case RegistrationResult.Unconstructible:
                        throw new GeneratorException("Initial configuration contains unconstructible objects.");
                    case RegistrationResult.Duplicates:
                        throw new GeneratorException("Initial configuration contains duplicates.");
                }
            }
        }

        /// <summary>
        /// Constructs new objects from a given output and determines if we
        /// correct objects (i.e. not duplicate or forbidden ones). It might 
        /// happen that some object from the output won't be added to the
        /// container at all. 
        /// </summary>
        /// <param name="output">The constructor output.</param>
        /// <param name="correctObjects">The correct objects flag.</param>
        /// <returns>The list of new constructed objects.</returns>
        private List<ConstructedConfigurationObject> ConstructNewObjects(ConstructorOutput output, out bool correctObjects)
        {
            // Initialize result
            var result = new List<ConstructedConfigurationObject>();

            // Pull ids of initial objects and cast them to set
            var initialIds = output
                             .OriginalConfiguration
                             .Configuration
                             .ConstructedObjects
                             .Select(obj => obj.Id ?? throw GeneratorException.ObjectsIdNotSet())
                             .ToSet();

            // Iterate over all constructed objects
            foreach (var constructedObject in output.ConstructedObjects)
            {
                // Get the result from the container
                var containerResult = _container.Add(constructedObject);

                // Pull id
                var id = containerResult.Id ?? throw GeneratorException.ObjectsIdNotSet();

                // If the object is currently in the configuration
                if (initialIds.Contains(id))
                {
                    // Set the flag indicating that there is an incorrect object
                    correctObjects = false;

                    // Terminate
                    return null;
                }

                // Find out if it's forbidden or unconstructible
                var isIncorrect = _duplicateObjectsIds.Contains(id);

                // If the object with this id is incorrect
                if (isIncorrect)
                {
                    // Set the flag indicating that there is an incorrect object
                    correctObjects = false;

                    // Terminate
                    return null;
                }

                // Otherwise we add the result to the new objects
                result.Add(containerResult);
            }

            // Let the helper method register the objects and find out if it's correct
            var isCorrectAfterRegistration = Register(result) == RegistrationResult.Ok;

            // If the object is incorrect
            if (!isCorrectAfterRegistration)
            {
                // Set the flag indicating that there is an incorrect object
                correctObjects = false;

                // Terminate
                return null;
            }

            // If we got here, then we have only correct objects
            correctObjects = true;

            // Therefore we can return the objects
            return result;
        }

        private RegistrationResult Register(List<ConstructedConfigurationObject> constructedObjects)
        {
            // Call the registrar
            var result = _registrar.Register(constructedObjects);

            // Find out if objects are correct
            var correct = result == RegistrationResult.Ok;

            // If the objects are correctly drawable, return immediately
            if (correct)
                return result;

            // Otherwise pull ids
            var ids = constructedObjects.Select(o => o.Id ?? throw GeneratorException.ObjectsIdNotSet());

            // Choose the right set to update
            var setToUpdate = result == RegistrationResult.Duplicates ? _duplicateObjectsIds : _unconstructibleObjectsIds;

            // Update the set
            foreach (var id in ids)
            {
                setToUpdate.Add(id);
            }

            // And return failure
            return result;
        }
    }
}