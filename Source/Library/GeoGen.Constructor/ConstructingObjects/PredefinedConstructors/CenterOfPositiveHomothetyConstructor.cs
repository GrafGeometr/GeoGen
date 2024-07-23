using GeoGen.AnalyticGeometry;
using GeoGen.Core;

namespace GeoGen.Constructor
{
    /// <summary>
    /// The <see cref="IObjectConstructor"/> for <see cref="PredefinedConstructionType.CenterOfPositiveHomothety"/>>.
    /// </summary>
    public class CenterOfPositiveHomothetyConstructor : PredefinedConstructorBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InversionOfPointConstructor"/> class.
        /// </summary>
        /// <param name="tracer">The tracer for unexpected analytic exceptions.</param>
        public CenterOfPositiveHomothetyConstructor(IConstructorFailureTracer tracer)
            : base(tracer)
        {
        }

        #endregion

        #region Construct implementation

        /// <inheritdoc/>
        protected override IAnalyticObject Construct(IAnalyticObject[] input)
        {
            // Get the points
            var c1 = (Circle)input[0];
            var c2 = (Circle)input[1];

            // Construct the result
            var center1 = c1.Center;
            var center2 = c2.Center;

            var r1 = c1.Radius;
            var r2 = c2.Radius;

            return (center1 * r2 - center2 * r1) / (- r1 + r2);
        }

        #endregion
    }
}