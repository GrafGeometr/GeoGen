using GeoGen.AnalyticGeometry;
using GeoGen.Core;

namespace GeoGen.Constructor
{
    /// <summary>
    /// The <see cref="IObjectConstructor"/> for <see cref="PredefinedConstructionType.InversionOfPoint"/>>.
    /// </summary>
    public class InversionOfPointConstructor : PredefinedConstructorBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InversionOfPointConstructor"/> class.
        /// </summary>
        /// <param name="tracer">The tracer for unexpected analytic exceptions.</param>
        public InversionOfPointConstructor(IConstructorFailureTracer tracer)
            : base(tracer)
        {
        }

        #endregion

        #region Construct implementation

        /// <inheritdoc/>
        protected override IAnalyticObject Construct(IAnalyticObject[] input)
        {
            // Get the points
            var A = (Point)input[0];
            var c = (Circle)input[1];

            // Construct the result
            return c.Invert(A);
        }

        #endregion
    }
}