﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace GeoGen.Core
{
    /// <summary>
    /// Represents a list of <see cref="ConstructionArgument"/> that should be passed 
    /// to a <see cref="Construction"/> to create a <see cref="ConstructedConfigurationObject"/>.
    /// It holds the flattened list of interior objects of the arguments that are evaluated lazily.
    /// </summary>
    public class Arguments : IEnumerable<ConstructionArgument>
    {
        #region Public properties

        /// <summary>
        /// Gets the arguments list wraped by this object.
        /// </summary>
        public IReadOnlyList<ConstructionArgument> ArgumentsList { get; }

        /// <summary>
        /// Gets the list of configuration objects that are obtained within the arguments
        /// in the order that we get if we recursively search through them from left to right. 
        /// For example: With { {A,B}, {C,D} } we might get A,B,C,D; or D,C,B,A. The order of objects
        /// witin a set and sets itself is not deterministic. This list is lazily evaluated.
        /// </summary>
        public IReadOnlyList<ConfigurationObject> FlattenedList => _flattenedListInitializer.Value;

        #endregion

        #region Private fields

        /// <summary>
        /// The lazy evaluator of flattened arguments.
        /// </summary>
        private readonly Lazy<IReadOnlyList<ConfigurationObject>> _flattenedListInitializer;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs arguments wrapping a given arguments list. 
        /// </summary>
        /// <param name="argumentsList">The arguments list.</param>
        public Arguments(IReadOnlyList<ConstructionArgument> argumentsList)
        {
            ArgumentsList = argumentsList ?? throw new ArgumentNullException(nameof(argumentsList));
            _flattenedListInitializer = new Lazy<IReadOnlyList<ConfigurationObject>>(ExtraxtInputObject);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Finds all objects in the arguments and flattens them to the list.
        /// </summary>
        /// <returns>The objects list.</returns>
        private List<ConfigurationObject> ExtraxtInputObject()
        {
            // Prepare the result
            var result = new List<ConfigurationObject>();

            // Local function to extract object from an argument
            void Extract(ConstructionArgument argument)
            {
                // If we have an object argument
                if (argument is ObjectConstructionArgument objectArgument)
                {
                    // Then we simply add the internal object to the result
                    result.Add(objectArgument.PassedObject);

                    // And terminate
                    return;
                }

                // Otherwise we have a set argument
                var setArgument = (SetConstructionArgument)argument;

                // We recursively call this function for internal arguments
                foreach (var passedArgument in setArgument.PassedArguments)
                {
                    Extract(passedArgument);
                }
            }

            // Now we just call our local function for all arguments
            foreach (var argument in ArgumentsList)
            {
                Extract(argument);
            }

            // And return the result
            return result;
        }

        #endregion

        #region IEnumerable implementation
        
        /// <summary>
        /// Gets a generic enumerator.
        /// </summary>
        /// <returns>The generic enumerator.</returns>
        public IEnumerator<ConstructionArgument> GetEnumerator()
        {
            return ArgumentsList.GetEnumerator();
        }

        /// <summary>
        /// Gets a non-generic enumerator.
        /// </summary>
        /// <returns>The non-generic enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        } 

        #endregion
    }
}