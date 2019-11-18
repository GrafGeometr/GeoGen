﻿using GeoGen.Constructor;
using GeoGen.Core;

namespace GeoGen.Algorithm
{
    /// <summary>
    /// Represents a tracer of geometry failures usually caused by an <see cref="InconsistentPicturesException"/>
    /// during a run of <see cref="SequentialAlgorithm"/>.
    /// </summary>
    public interface IGeometryFailureTracer
    {
        /// <summary>
        /// Traces that there is an object that cannot be consistently drawn to the pictures holding all the objects
        /// that the algorithm has generated.
        /// </summary>
        /// <param name="constructedObject">The object that couldn't be drawn consistently.</param>
        /// <param name="pictures">The pictures to which the object was attempted to be drawn.</param>
        /// <param name="exception">The inner inconsistency exception that caused the issue.</param>
        void UndrawableObjectInBigPicture(ConstructedConfigurationObject constructedObject, Pictures pictures, InconsistentPicturesException exception);

        /// <summary>
        /// Traces that a given contextual picture couldn't be cloned and extended with the new object
        /// already drawn in pictures representing some configuration.
        /// </summary>
        /// <param name="previousContextualPicture">The contextual picture that was correct and failed to add the new object.</param>
        /// <param name="newConfigurationPictures">The pictures holding geometry data of the new object that was added.</param>
        /// <param name="exception">The inner inconsistency exception that caused the issue.</param>
        void InconstructibleContextualPictureByCloning(ContextualPicture previousContextualPicture, PicturesOfConfiguration newConfigurationPictures, InconsistentPicturesException exception);

        /// <summary>
        /// Traces that an object couldn't be drawn consistently to the pictures holding only the objects
        /// of some configuration.
        /// </summary>
        /// <param name="constructedObject">The object that couldn't be drawn consistently.</param>
        /// <param name="pictures">The pictures to which the object was attempted to be drawn.</param>
        /// <param name="exception">The inner inconsistency exception that caused the issue.</param>
        void UndrawableObjectInPicturesOfConfiguration(ConstructedConfigurationObject constructedObject, PicturesOfConfiguration pictures, InconsistentPicturesException exception);

        /// <summary>
        /// Traces that after adding a new object to the pictures the object has an invalid data, i.e.
        /// either there is a duplicate in the configuration, or the object is inconstructible.
        /// </summary>
        /// <param name="pictures">The pictures where the new object has been added.</param>
        /// <param name="constructionData">The incorrect construction data.</param>
        void IncorrectPicturesAfterAddingNewObject(PicturesOfConfiguration pictures, ConstructionData constructionData);
    }
}