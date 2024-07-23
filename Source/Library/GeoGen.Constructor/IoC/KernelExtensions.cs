﻿using Ninject;
using Ninject.Extensions.Factory;

namespace GeoGen.Constructor
{
    /// <summary>
    /// The extension methods for <see cref="IKernel"/>.
    /// </summary>
    public static class KernelExtensions
    {
        /// <summary>
        /// Bindings for the dependencies from the Constructor module.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <returns>The kernel for chaining.</returns>
        public static IKernel AddConstructor(this IKernel kernel)
        {
            // Bind the services
            kernel.Bind<IGeometryConstructor>().To<GeometryConstructor>();
            kernel.Bind<IConstructorResolver>().To<ConstructorResolver>();
            kernel.Bind<IGeometricTheoremVerifier>().To<GeometricTheoremVerifier>();
            kernel.Bind<IComposedConstructor>().To<ComposedConstructor>();
            kernel.Bind<IComposedConstructorFactory>().ToFactory();

            // Bind the predefined constructors
            kernel.Bind<IPredefinedConstructor>().To<CenterOfNegativeHomothetyConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<CenterOfPositiveHomothetyConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<InversionOfPointConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<CenterOfCircleConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<CircleWithCenterThroughPointConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<CircumcircleConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<InternalAngleBisectorConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<IntersectionOfLinesConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<LineFromPointsConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<MidpointConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<PerpendicularLineConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<ParallelLineConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<PerpendicularProjectionConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<PointReflectionConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<SecondIntersectionOfCircleAndLineFromPointsConstructor>();
            kernel.Bind<IPredefinedConstructor>().To<SecondIntersectionOfTwoCircumcirclesConstructor>();

            // Bind the tracer
            kernel.Bind<IConstructorFailureTracer>().To<EmptyConstructorFailureTracer>();

            // Return the kernel for chaining
            return kernel;
        }
    }
}