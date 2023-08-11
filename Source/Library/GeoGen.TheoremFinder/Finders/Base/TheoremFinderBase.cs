﻿using GeoGen.Constructor;
using GeoGen.Core;
using GeoGen.Utilities;

namespace GeoGen.TheoremFinder
{
    /// <summary>
    /// The base class for <see cref="ITypedTheoremFinder"/>s. 
    /// </summary>
    public abstract class TheoremFinderBase : ITypedTheoremFinder
    {
        #region ITypedTheoremFinder properties

        /// <inheritdoc/>
        public TheoremType Type { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TheoremFinderBase"/> class.
        /// </summary>
        protected TheoremFinderBase()
        {
            // Find the type
            Type = FindTypeFromClassName();
        }

        #endregion

        #region Finding type from the class name

        /// <summary>
        /// Infers the type of the theorem finder from the class name. 
        /// The class name should be in the form {type}TheoremFinder.
        /// </summary>
        /// <returns>The inferred type.</returns>
        private TheoremType FindTypeFromClassName()
            // Call the utility helper that does the job
            => EnumUtilities.ParseEnumValueFromClassName<TheoremType>(GetType(), classNamePrefix: "TheoremFinder");

        #endregion

        #region Protected methods

        /// <summary>
        /// Converts given objects to theorems holding in a given configuration. These theorems
        /// represent the same statement, provided we can justify the used collinear and concyclic points.
        /// </summary>
        /// <param name="geometricObjects">Flattened geometric objects that are converted to theorems.</param>
        /// <returns>The theorems.</returns>
        protected IEnumerable<Theorem> ToTheorems(IEnumerable<GeometricObject> geometricObjects)
        {
            // Map geometric objects to theorem objects
            return geometricObjects.Select(geometricObject =>
            {
                // Switch on the object type
                switch (geometricObject)
                {
                    // In point case we have just the object
                    case PointObject point:
                        return new List<TheoremObject> { new PointTheoremObject(point.ConfigurationObject) };

                    // In case of an object with points
                    case DefinableByPoints objectWithPoints:

                        // Get the possible point definitions
                        var pointDefinitions = objectWithPoints.Points
                            // Take the subsets of the needed number of defining points
                            .Subsets(objectWithPoints.NumberOfNeededPoints)
                            // Get the inner configuration objects
                            .Select(points => points.Select(point => point.ConfigurationObject).ToArray());

                        // Prepare the resulting theorem objects list
                        var objectsList = new List<TheoremObject>();

                        // For every definition create the right theorem object
                        pointDefinitions.Select(points =>
                        {
                            // Switch base on the object type to find the right constructor
                            return objectWithPoints switch
                            {
                                // In line case we have 2 points
                                LineObject _ => new LineTheoremObject(points[0], points[1]) as TheoremObject,

                                // In circle case we have 3 points
                                CircleObject _ => new CircleTheoremObject(points[0], points[1], points[2]),

                                // Unhandled cases
                                _ => throw new TheoremFinderException($"Unhandled type of {nameof(TheoremObjectWithPoints)}: {objectWithPoints.GetType()}")
                            };
                        })
                        // Add each newly created object to the resulting list
                        .ForEach(objectsList.Add);

                        // If the object has the configuration object set
                        if (objectWithPoints.ConfigurationObject != null)
                        {
                            // Then we have one more definition, an explicit one
                            var explicitObject = objectWithPoints switch
                            {
                                // Line case
                                LineObject _ => new LineTheoremObject(objectWithPoints.ConfigurationObject) as TheoremObject,

                                // Circle case
                                CircleObject _ => new CircleTheoremObject(objectWithPoints.ConfigurationObject),

                                // Unhandled cases
                                _ => throw new TheoremFinderException($"Unhandled type of {nameof(TheoremObjectWithPoints)}: {objectWithPoints.GetType()}")
                            };

                            // Add this newly created object to the resulting list
                            objectsList.Add(explicitObject);
                        }

                        // Finally return the resulting list
                        return objectsList;

                    // Unhandled cases
                    default:
                        throw new TheoremFinderException($"Unhandled type of {nameof(GeometricObject)}: {geometricObject.GetType()}");
                }
            })
            // Combine every possible definition of each object
            .Combine()
            // Now we can finally use the helper method to get the theorem from these objects
            .Select(objects => Theorem.DeriveFromFlattenedObjects(Type, objects));
        }

        #endregion

        #region ITypedTheoremFinder implementation

        /// <inheritdoc/>
        public abstract IEnumerable<Theorem> FindAllTheorems(ContextualPicture contextualPicture);

        /// <inheritdoc/>
        public abstract IEnumerable<Theorem> FindNewTheorems(ContextualPicture contextualPicture);

        /// <inheritdoc/>
        public abstract bool ValidateOldTheorem(ContextualPicture contextualPicture, Theorem oldTheorem);

        #endregion
    }
}
