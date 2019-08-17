﻿using GeoGen.Constructor;
using GeoGen.Core;
using GeoGen.DependenciesResolver;
using Ninject;
using System.Collections.Generic;
using System.Linq;

namespace GeoGen.TheoremsFinder.Tests
{
    /// <summary>
    /// A base class for tests of <see cref="ITheoremsFinder"/>.
    /// </summary>
    /// <typeparam name="T">The type of theorems finder being tested.</typeparam>
    public abstract class TheoremsFinderTestBase<T> where T : ITheoremsFinder, new()
    {
        /// <summary>
        /// Runs the algorithm on the configuration to find new and all theorems. 
        /// </summary>
        /// <param name="configuration">The configuration where we're looking for theorems.</param>
        /// <returns>The new and all theorems.</returns>
        protected (List<Theorem> newTheorems, List<Theorem> allTheorems) FindTheorems(Configuration configuration)
        {
            // Prepare the kernel
            var kernel = IoC.CreateKernel().AddConstructor(new PicturesSettings
            {
                NumberOfPictures = 5,
                MaximalAttemptsToReconstructAllPictures = 0,
                MaximalAttemptsToReconstructOnePicture = 0
            });

            // Create the pictures
            var pictures = kernel.Get<IGeometryConstructor>().Construct(configuration).pictures;

            // Create the contextual picture
            var contextualPicture = kernel.Get<IContextualPictureFactory>().CreateContextualPicture(pictures);

            // Get the service instance
            var finder = new T();

            // Run both algorithms
            return (finder.FindNewTheorems(contextualPicture).ToList(), finder.FindAllTheorems(contextualPicture).ToList());
        }
    }
}