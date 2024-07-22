using System.Collections.Generic;
using static GeoGen.Core.ConfigurationObjectType;
using static GeoGen.Core.LooseObjectLayout;
using static GeoGen.Core.PredefinedConstructionType;

namespace GeoGen.Core
{
    /// <summary>
    /// A static class containing concrete <see cref="ComposedConstruction"/>.
    /// </summary>
    public static class ComposedConstructions
    {
        /// <summary>
        /// Intersection of line l and other line AB (signature l, {A, B}).
        /// </summary>
        public static ComposedConstruction IntersectionOfLineAndLineFromPoints
        {
            get
            {
                // Create objects
                var l = new LooseConfigurationObject(Line);
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var lineAB = new ConstructedConfigurationObject(LineFromPoints, A, B);
                var intersection = new ConstructedConfigurationObject(IntersectionOfLines, l, lineAB);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(LineAndTwoPoints, intersection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Line),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(IntersectionOfLineAndLineFromPoints), configuration, parameters);
            }
        }

        /// <summary>
        /// Intersection of lines AB and CD (signature {{A, B}, {C, D}}).
        /// </summary>
        public static ComposedConstruction IntersectionOfLinesFromPoints
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var D = new LooseConfigurationObject(Point);
                var lineAB = new ConstructedConfigurationObject(LineFromPoints, A, B);
                var lineCD = new ConstructedConfigurationObject(LineFromPoints, C, D);
                var intersection = new ConstructedConfigurationObject(IntersectionOfLines, lineAB, lineCD);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Quadrilateral, A, B, C, D, intersection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                     new SetConstructionParameter(new SetConstructionParameter(new ObjectConstructionParameter(Point), 2), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(IntersectionOfLinesFromPoints), configuration, parameters);
            }
        }

        /// <summary>
        /// Perpendicular projection of points A on line BC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction PerpendicularProjectionOnLineFromPoints
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var lineBC = new ConstructedConfigurationObject(LineFromPoints, B, C);
                var projection = new ConstructedConfigurationObject(PerpendicularProjection, A, lineBC);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, projection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                     new ObjectConstructionParameter(Point),
                     new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(PerpendicularProjectionOnLineFromPoints), configuration, parameters);
            }
        }

        /// <summary>
        /// Perpendicular line to line AB passing through A (signature A, B).
        /// </summary>
        public static ComposedConstruction PerpendicularLineAtPointOfLine
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var lineAB = new ConstructedConfigurationObject(LineFromPoints, A, B);
                var perpendicularLine = new ConstructedConfigurationObject(PerpendicularLine, A, lineAB);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(LineSegment, A, B, perpendicularLine);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                     new ObjectConstructionParameter(Point),
                     new ObjectConstructionParameter(Point)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(PerpendicularLineAtPointOfLine), configuration, parameters);
            }
        }

        /// <summary>
        /// Perpendicular line to line BC passing through A (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction PerpendicularLineToLineFromPoints
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var lineBC = new ConstructedConfigurationObject(LineFromPoints, B, C);
                var perpendicularLine = new ConstructedConfigurationObject(PerpendicularLine, A, lineBC);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, perpendicularLine);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(PerpendicularLineToLineFromPoints), configuration, parameters);
            }
        }

        /// <summary>
        /// Reflection of point A in line l (signature l, A).
        /// </summary>
        public static ComposedConstruction ReflectionInLine
        {
            get
            {
                // Create objects
                var l = new LooseConfigurationObject(Line);
                var A = new LooseConfigurationObject(Point);
                var projectionA = new ConstructedConfigurationObject(PerpendicularProjection, A, l);
                var reflection = new ConstructedConfigurationObject(PointReflection, A, projectionA);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(LineAndPoint, l, A, reflection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Line),
                    new ObjectConstructionParameter(Point)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(ReflectionInLine), configuration, parameters);
            }
        }

        /// <summary>
        /// Reflection of point A in line BC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction ReflectionInLineFromPoints
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var lineBC = new ConstructedConfigurationObject(LineFromPoints, B, C);
                var reflection = new ConstructedConfigurationObject(ReflectionInLine, lineBC, A);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, reflection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(ReflectionInLineFromPoints), configuration, parameters);
            }
        }

        /// <summary>
        /// The external angle bisector of angle BAC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction ExternalAngleBisector
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var I = new ConstructedConfigurationObject(Incenter, A, B, C);
                var l = new ConstructedConfigurationObject(PerpendicularLineAtPointOfLine, A, I);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, l);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(ExternalAngleBisector), configuration, parameters);
            }
        }

        /// <summary>
        /// Perpendicular bisector of line segment AB (signature {A, B}).
        /// </summary>
        public static ComposedConstruction PerpendicularBisector
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var midpointAB = new ConstructedConfigurationObject(Midpoint, A, B);
                var bisector = new ConstructedConfigurationObject(PerpendicularLineAtPointOfLine, midpointAB, A);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(LineSegment, bisector);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(PerpendicularBisector), configuration, parameters);
            }
        }

        /// <summary>
        /// Line parallel to line BC passing through A (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction ParallelLineToLineFromPoints
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var lineBC = new ConstructedConfigurationObject(LineFromPoints, B, C);
                var parallelLine = new ConstructedConfigurationObject(ParallelLine, A, lineBC);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, parallelLine);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(ParallelLineToLineFromPoints), configuration, parameters);
            }
        }

        /// <summary>
        /// Reflection of point A in the midpoint of BC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction ParallelogramPoint
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var midpointBC = new ConstructedConfigurationObject(Midpoint, B, C);
                var reflection = new ConstructedConfigurationObject(PointReflection, A, midpointBC);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, reflection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(ParallelogramPoint), configuration, parameters);
            }
        }

        /// <summary>
        /// Tangent point or incircle with BC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction IncircleTangentPoint
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var I = new ConstructedConfigurationObject(Incenter, A, B, C);
                var D = new ConstructedConfigurationObject(PerpendicularProjectionOnLineFromPoints, I, B, C);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, D);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(IncircleTangentPoint), configuration, parameters);
            }
        }

        /// <summary>
        /// The A-Humpty point of triangle ABC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction HumptyPoint
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var orthocenter = new ConstructedConfigurationObject(Orthocenter, A, B, C);
                var median = new ConstructedConfigurationObject(Median, A, B, C);
                var projection = new ConstructedConfigurationObject(PerpendicularProjection, orthocenter, median);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, projection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(HumptyPoint), configuration, parameters);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public static ComposedConstruction HOPoint
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var projection = new ConstructedConfigurationObject(PerpendicularProjectionOnLineFromPoints, A, B, C);
                var M = new ConstructedConfigurationObject(Midpoint, B, C);
                var X = new ConstructedConfigurationObject(PointReflection, projection, M);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, X);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(HOPoint), configuration, parameters);
            }
        }

        /// <summary>
        /// The A-Dumpty point of triangle ABC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction DumptyPoint
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var circumcenter = new ConstructedConfigurationObject(Circumcenter, A, B, C);
                var symedian = new ConstructedConfigurationObject(Symedian, A, B, C);
                var projection = new ConstructedConfigurationObject(PerpendicularProjection, circumcenter, symedian);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, projection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(DumptyPoint), configuration, parameters);
            }
        }

        /// <summary>
        /// The A-Tangent lines intersection in triangle ABC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction TangentLinesIntersection
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var tB = new ConstructedConfigurationObject(TangentLine, B, A, C);
                var tC = new ConstructedConfigurationObject(TangentLine, C, A, B);
                var intersection = new ConstructedConfigurationObject(IntersectionOfLines, tB, tC);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, intersection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(TangentLinesIntersection), configuration, parameters);
            }
        }

        /// <summary>
        /// The end of A-symedian of triangle ABC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction SymedianEnd
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var symedian = new ConstructedConfigurationObject(Symedian, A, B, C);
                var S = new ConstructedConfigurationObject(IntersectionOfLineAndLineFromPoints, symedian, B, C);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, S);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(SymedianEnd), configuration, parameters);
            }
        }

        /// <summary>
        /// The isogonal conjugate of P in triangle ABC (signature P, {A, B, C}). 
        /// </summary>
        public static ComposedConstruction Isogonal
        {
            get
            {
                // Create objects
                var P = new LooseConfigurationObject(Point);
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var aBisector = new ConstructedConfigurationObject(InternalAngleBisector, A, B, C);
                var bBisector = new ConstructedConfigurationObject(InternalAngleBisector, B, A, C);

                var Pa = new ConstructedConfigurationObject(ReflectionInLine, aBisector, P);
                var Pb = new ConstructedConfigurationObject(ReflectionInLine, bBisector, P);

                var S = new ConstructedConfigurationObject(IntersectionOfLinesFromPoints, A, Pa, B, Pb);
                

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Quadrilateral, P, A, B, C, S);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Isogonal), configuration, parameters);
            }
        }

        /// <summary>
        /// The isotomic conjugate of P in triangle ABC (signature P, {A, B, C}). 
        /// </summary>
        public static ComposedConstruction Isotomic
        {
            get
            {
                // Create objects
                var P = new LooseConfigurationObject(Point);
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var Pa = new ConstructedConfigurationObject(IntersectionOfLinesFromPoints, A, P, B, C);
                var Pb = new ConstructedConfigurationObject(IntersectionOfLinesFromPoints, B, P, A, C);

                var Ma = new ConstructedConfigurationObject(Midpoint, B, C);
                var Mb = new ConstructedConfigurationObject(Midpoint, A, C);

                var Pa_ = new ConstructedConfigurationObject(PointReflection, Pa, Ma);
                var Pb_ = new ConstructedConfigurationObject(PointReflection, Pb, Mb);

                var S = new ConstructedConfigurationObject(IntersectionOfLinesFromPoints, A, Pa_, B, Pb_);
                

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Quadrilateral, P, A, B, C, S);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Isotomic), configuration, parameters);
            }
        }

        /// <summary>
        /// The antigonal conjugate of P in triangle ABC (signature P, {A, B, C}). 
        /// </summary>
        public static ComposedConstruction Antigonal
        {
            get
            {
                // Create objects
                var P = new LooseConfigurationObject(Point);
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var Pa = new ConstructedConfigurationObject(ReflectionInLineFromPoints, P, B, C);
                var Pb = new ConstructedConfigurationObject(ReflectionInLineFromPoints, P, A, C);

                var S = new ConstructedConfigurationObject(SecondIntersectionOfTwoCircumcircles, C, A, Pb, B, Pa);
                

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Quadrilateral, P, A, B, C, S);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Isotomic), configuration, parameters);
            }
        }

        /// <summary>
        /// The middle of A-symedian of triangle ABC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction SymedianMiddle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var symedian = new ConstructedConfigurationObject(Symedian, A, B, C);
                var S = new ConstructedConfigurationObject(IntersectionOfLineAndLineFromPoints, symedian, B, C);
                var M = new ConstructedConfigurationObject(Midpoint, A, S);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, M);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(SymedianMiddle), configuration, parameters);
            }
        }

        /// <summary>
        /// The middle of A-median of triangle ABC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction MedianMiddle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var M = new ConstructedConfigurationObject(Midpoint, B, C);
                var P = new ConstructedConfigurationObject(Midpoint, A, M);
                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, P);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(MedianMiddle), configuration, parameters);
            }
        }

        /// <summary>
        /// The end of A-bisector of triangle ABC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction BisectorEnd
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var bisector = new ConstructedConfigurationObject(InternalAngleBisector, A, B, C);
                var P = new ConstructedConfigurationObject(IntersectionOfLineAndLineFromPoints, bisector, B, C);
                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, P);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(BisectorEnd), configuration, parameters);
            }
        }

        /// <summary>
        /// The end of A-exbisector of triangle ABC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction ExternalBisectorEnd
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var bisector = new ConstructedConfigurationObject(ExternalAngleBisector, A, B, C);
                var P = new ConstructedConfigurationObject(IntersectionOfLineAndLineFromPoints, bisector, B, C);
                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, P);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(ExternalBisectorEnd), configuration, parameters);
            }
        }


        /// <summary>
        /// Reflection of point A in the perpendicular bisector of BC (signature A, {B, C}). 
        /// </summary>
        public static ComposedConstruction IsoscelesTrapezoidPoint
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var bisectorBC = new ConstructedConfigurationObject(PerpendicularBisector, B, C);
                var reflection = new ConstructedConfigurationObject(ReflectionInLine, bisectorBC, A);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, reflection);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(IsoscelesTrapezoidPoint), configuration, parameters);
            }
        }

        /// <summary>
        /// Orthocenter of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction Orthocenter
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var altitudeB = new ConstructedConfigurationObject(PerpendicularLineToLineFromPoints, B, A, C);
                var altitudeC = new ConstructedConfigurationObject(PerpendicularLineToLineFromPoints, C, A, B);
                var H = new ConstructedConfigurationObject(IntersectionOfLines, altitudeB, altitudeC);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, H);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Orthocenter), configuration, parameters);
            }
        }

        /// <summary>
        /// Incenter of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction Incenter
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var bisectorA = new ConstructedConfigurationObject(InternalAngleBisector, A, B, C);
                var bisectorB = new ConstructedConfigurationObject(InternalAngleBisector, B, A, C);
                var I = new ConstructedConfigurationObject(IntersectionOfLines, bisectorA, bisectorB);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, I);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Incenter), configuration, parameters);
            }
        }

        /// <summary>
        /// Midpoint of bisector of angle BAC in triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction BisectorMiddle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var bisectorA = new ConstructedConfigurationObject(InternalAngleBisector, A, B, C);
                var D = new ConstructedConfigurationObject(IntersectionOfLineAndLineFromPoints, bisectorA, B, C);
                var X = new ConstructedConfigurationObject(Midpoint, A, D);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, X);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(BisectorMiddle), configuration, parameters);
            }
        }

        /// <summary>
        /// Midpoint of exbisector of angle BAC in triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction ExternalBisectorMiddle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var bisectorA = new ConstructedConfigurationObject(ExternalAngleBisector, A, B, C);
                var D = new ConstructedConfigurationObject(IntersectionOfLineAndLineFromPoints, bisectorA, B, C);
                var X = new ConstructedConfigurationObject(Midpoint, A, D);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, X);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(ExternalBisectorMiddle), configuration, parameters);
            }
        }

        /// <summary>
        /// Midpoint of A-height  in triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction HeightMiddle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var proj = new ConstructedConfigurationObject(PerpendicularProjectionOnLineFromPoints, A, B, C);
                var X = new ConstructedConfigurationObject(Midpoint, A, proj);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, X);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(HeightMiddle), configuration, parameters);
            }
        }

        /// <summary>
        /// A-radius line in triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction RadiusLine
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var O = new ConstructedConfigurationObject(Circumcenter, A, B, C);
                var l = new ConstructedConfigurationObject(LineFromPoints, A, O);
                

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, l);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(RadiusLine), configuration, parameters);
            }
        }

        /// <summary>
        /// End of A-radius line in triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction RadiusLineEnd
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var O = new ConstructedConfigurationObject(Circumcenter, A, B, C);
                var X = new ConstructedConfigurationObject(IntersectionOfLinesFromPoints, A, O, B, C);
                

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, X);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(RadiusLineEnd), configuration, parameters);
            }
        }

        /// <summary>
        /// Middle of A-radius line in triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction RadiusLineMiddle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var O = new ConstructedConfigurationObject(Circumcenter, A, B, C);
                var T = new ConstructedConfigurationObject(IntersectionOfLinesFromPoints, A, O, B, C);
                var X = new ConstructedConfigurationObject(Midpoint, A, T);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, X);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(RadiusLineMiddle), configuration, parameters);
            }
        }


        /// <summary>
        /// Centroid of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction Centroid
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var medianA = new ConstructedConfigurationObject(Median, A, B, C);
                var medianB = new ConstructedConfigurationObject(Median, B, A, C);
                var G = new ConstructedConfigurationObject(IntersectionOfLines, medianA, medianB);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, G);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Centroid), configuration, parameters);
            }
        }

        /// <summary>
        /// Gergonne point of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction Gergonne
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var Pa = new ConstructedConfigurationObject(IncircleTangentPoint, A, B, C);
                var Pb = new ConstructedConfigurationObject(IncircleTangentPoint, B, A, C);
                var Ge = new ConstructedConfigurationObject(IntersectionOfLinesFromPoints, A, Pa, B, Pb);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, Ge);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Gergonne), configuration, parameters);
            }
        }

        /// <summary>
        /// Nagel point of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction Nagel
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var Ge = new ConstructedConfigurationObject(Gergonne, A, B, C);
                var N = new ConstructedConfigurationObject(Isotomic, Ge, A, B, C);
                

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, N);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Nagel), configuration, parameters);
            }
        }

        /// <summary>
        /// Lemoine point of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction Lemoine
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var symedianA = new ConstructedConfigurationObject(Symedian, A, B, C);
                var symedianB = new ConstructedConfigurationObject(Symedian, B, A, C);
                var L = new ConstructedConfigurationObject(IntersectionOfLines, symedianA, symedianB);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, L);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Lemoine), configuration, parameters);
            }
        }

        /// <summary>
        /// Poncelet point of quadrilateral ABCD (signature {A, B, C, D}).
        /// </summary>
        public static ComposedConstruction PonceletPoint
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var D = new LooseConfigurationObject(Point);
                var ab = new ConstructedConfigurationObject(Midpoint, A, B);
                var bc = new ConstructedConfigurationObject(Midpoint, B, C);
                var ca = new ConstructedConfigurationObject(Midpoint, C, A);
                var ad = new ConstructedConfigurationObject(Midpoint, A, D);
                var bd = new ConstructedConfigurationObject(Midpoint, B, D);
                
                var P = new ConstructedConfigurationObject(SecondIntersectionOfTwoCircumcircles, ab, bc, ca, ad, bd);


                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Quadrilateral, P);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 4)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(PonceletPoint), configuration, parameters);
            }
        }
        

        /// <summary>
        /// Feuerbach point of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction FeuerbachPoint
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var I = new ConstructedConfigurationObject(Incenter, A, B, C);
                var F = new ConstructedConfigurationObject(PonceletPoint, A, B, C, I);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, F);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(FeuerbachPoint), configuration, parameters);
            }
        }


        
        /// <summary>
        /// Centroid of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction QuadrilateralCentroid
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var D = new LooseConfigurationObject(Point);
                var ab = new ConstructedConfigurationObject(Midpoint, A, B);
                var cd = new ConstructedConfigurationObject(Midpoint, C, D);
                var G = new ConstructedConfigurationObject(Midpoint, ab, cd);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Quadrilateral, G);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 4)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(QuadrilateralCentroid), configuration, parameters);
            }
        }

        

        /// <summary>
        /// Incircle of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction Incircle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var I = new ConstructedConfigurationObject(Incenter, A, B, C);
                var projection = new ConstructedConfigurationObject(PerpendicularProjectionOnLineFromPoints, I, B, C);
                var incircle = new ConstructedConfigurationObject(CircleWithCenterThroughPoint, I, projection);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, incircle);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                     new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Incircle), configuration, parameters);
            }
        }

        /// <summary>
        /// Circumcircle of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction Circumcenter
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var c = new ConstructedConfigurationObject(Circumcircle, A, B, C);
                var O = new ConstructedConfigurationObject(CenterOfCircle, c);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, O);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Circumcenter), configuration, parameters);
            }
        }

        

        /// <summary>
        /// The midpoint of arc opposite to BAC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction MidpointOfOppositeArc
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var I = new ConstructedConfigurationObject(Incenter, A, B, C);
                var M = new ConstructedConfigurationObject(Circumcenter, B, C, I);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, M);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(MidpointOfOppositeArc), configuration, parameters);
            }
        }

        /// <summary>
        /// The midpoint of arc BAC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction MidpointOfArc
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var Ib = new ConstructedConfigurationObject(Excenter, B, A, C);
                var Ic = new ConstructedConfigurationObject(Excenter, C, A, B);
                var M = new ConstructedConfigurationObject(Midpoint, Ib, Ic);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, M);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(MidpointOfArc), configuration, parameters);
            }
        }

        /// <summary>
        /// The A-excenter of triangle ABC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction Excenter
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var I = new ConstructedConfigurationObject(Incenter, A, B, C);
                var M = new ConstructedConfigurationObject(MidpointOfOppositeArc, A, B, C);
                var J = new ConstructedConfigurationObject(PointReflection, I, M);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, J);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Excenter), configuration, parameters);
            }
        }

        /// <summary>
        /// The A-excircle of triangle ABC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction Excircle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var J = new ConstructedConfigurationObject(Excenter, A, B, C);
                var D = new ConstructedConfigurationObject(PerpendicularProjectionOnLineFromPoints, J, B, C);
                var c = new ConstructedConfigurationObject(CircleWithCenterThroughPoint, J, D);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, c);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Excircle), configuration, parameters);
            }
        }

        /// <summary>
        /// The circle with a given diameter (signature {A, B}).
        /// </summary>
        public static ComposedConstruction CircleWithDiameter
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var M = new ConstructedConfigurationObject(Midpoint, A, B);
                var c = new ConstructedConfigurationObject(CircleWithCenterThroughPoint, M, A);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(LineSegment, A, B, c);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(CircleWithDiameter), configuration, parameters);
            }
        }

        /// <summary>
        /// The A-midline of triangle ABC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction Midline
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var D = new ConstructedConfigurationObject(Midpoint, A, B);
                var E = new ConstructedConfigurationObject(Midpoint, A, C);
                var l = new ConstructedConfigurationObject(LineFromPoints, D, E);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, l);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Midline), configuration, parameters);
            }
        }

        /// <summary>
        /// The A-median of triangle ABC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction Median
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var D = new ConstructedConfigurationObject(Midpoint, B, C);
                var l = new ConstructedConfigurationObject(LineFromPoints, A, D);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, l);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Median), configuration, parameters);
            }
        }

        /// <summary>
        /// The A-symedian of triangle ABC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction Symedian
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var tb = new ConstructedConfigurationObject(TangentLine, B, A, C);
                var tc = new ConstructedConfigurationObject(TangentLine, C, A, B);
                var K = new ConstructedConfigurationObject(IntersectionOfLines, tb, tc);
                var l = new ConstructedConfigurationObject(LineFromPoints, A, K);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, l);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(Symedian), configuration, parameters);
            }
        }

        /// <summary>
        /// The tangent line at point A of the circumcircle ABC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction TangentLine
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var O = new ConstructedConfigurationObject(Circumcenter, A, B, C);
                var l = new ConstructedConfigurationObject(PerpendicularLineAtPointOfLine, A, O);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, l);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(TangentLine), configuration, parameters);
            }
        }

        /// <summary>
        /// The line through point A and the circumcenter of ABC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction LineThroughCircumcenter
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var O = new ConstructedConfigurationObject(Circumcenter, A, B, C);
                var l = new ConstructedConfigurationObject(LineFromPoints, A, O);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, l);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(LineThroughCircumcenter), configuration, parameters);
            }
        }

        /// <summary>
        /// The point lying opposite to point A of the circumcircle of triangle ABC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction OppositePointOnCircumcircle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var O = new ConstructedConfigurationObject(Circumcenter, A, B, C);
                var D = new ConstructedConfigurationObject(PointReflection, A, O);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, A, B, C, D);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(OppositePointOnCircumcircle), configuration, parameters);
            }
        }

        /// <summary>
        /// The Nine-Point circle of triangle ABC (signature {A, B, C}).
        /// </summary>
        public static ComposedConstruction NinePointCircle
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var P = new ConstructedConfigurationObject(Midpoint, A, B);
                var Q = new ConstructedConfigurationObject(Midpoint, B, C);
                var R = new ConstructedConfigurationObject(Midpoint, C, A);
                var c = new ConstructedConfigurationObject(Circumcircle, P, Q, R);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, c);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 3)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(NinePointCircle), configuration, parameters);
            }
        }

        /// <summary>
        /// The A-Five-Point circle of triangle ABC (signature A, {B, C}).
        /// </summary>
        public static ComposedConstruction FPC
        {
            get
            {
                // Create objects
                var A = new LooseConfigurationObject(Point);
                var B = new LooseConfigurationObject(Point);
                var C = new LooseConfigurationObject(Point);
                var P = new ConstructedConfigurationObject(Orthocenter, A, B, C);
                var Q = new ConstructedConfigurationObject(Midpoint, B, C);
                var R = new ConstructedConfigurationObject(PerpendicularProjectionOnLineFromPoints, A, B, C);
                var c = new ConstructedConfigurationObject(Circumcircle, P, Q, R);

                // Create the actual configuration
                var configuration = Configuration.DeriveFromObjects(Triangle, c);

                // Create the parameters
                var parameters = new List<ConstructionParameter>
                {
                    new ObjectConstructionParameter(Point),
                    new SetConstructionParameter(new ObjectConstructionParameter(Point), 2)
                };

                // Create the actual construction
                return new ComposedConstruction(nameof(FPC), configuration, parameters);
            }
        }
    }
}
