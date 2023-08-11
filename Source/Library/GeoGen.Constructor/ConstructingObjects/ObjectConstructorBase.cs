﻿using GeoGen.AnalyticGeometry;
using GeoGen.Core;

namespace GeoGen.Constructor
{
    /// <summary>
    /// A base class for <see cref="IObjectConstructor"/>s that simplify implementors 
    /// so they can deal directly with analytic objects.
    /// </summary>
    public abstract class ObjectConstructorBase : IObjectConstructor
    {
        #region Private fields

        /// <summary>
        /// The tracer for unexpected analytic exceptions.
        /// </summary>
        private readonly IConstructorFailureTracer _tracer;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConstructorBase"/> class.
        /// </summary>
        /// <param name="tracer">The tracer for unexpected analytic exceptions.</param>
        protected ObjectConstructorBase(IConstructorFailureTracer tracer)
        {
            _tracer = tracer ?? throw new ArgumentNullException(nameof(tracer));
        }

        #endregion

        #region IObjectConstructor implementation

        /// <inheritdoc/>
        public Func<Picture, IAnalyticObject> Construct(ConstructedConfigurationObject configurationObject)
            // Return the function that takes a picture as an input and returns the result of a construction
            => picture =>
            {
                // Get the analytic versions of the objects passed in the arguments       
                var analyticObjects = configurationObject.PassedArguments.FlattenedList.Select(picture.Get).ToArray();

                try
                {
                    // Try to call the abstract function to get the actual result
                    return Construct(analyticObjects);
                }
                // Just in case, if there is an analytic exception
                catch (AnalyticException e)
                {
                    // We trace it
                    _tracer.TraceUnexpectedConstructionFailure(configurationObject, analyticObjects, e.Message);

                    // And return null indicating the constructor didn't work out
                    return null;
                }
            };

        #endregion

        #region Protected abstract methods

        /// <summary>
        /// Performs the actual construction of an analytic object based on the analytic objects given as an input.
        /// The order of the objects of the input is based on the <see cref="Arguments.FlattenedList"/>.
        /// </summary>
        /// <param name="input">The analytic objects to be used as an input.</param>
        /// <returns>The constructed analytic object, if the construction was successful; or null otherwise.</returns>
        protected abstract IAnalyticObject Construct(IAnalyticObject[] input);

        #endregion
    }
}