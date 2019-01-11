﻿using GeoGen.Utilities;
using System;
using System.Linq;

namespace GeoGen.AnalyticGeometry
{
    /// <summary>
    /// A static helper class for manipulation with analytic objects.
    /// </summary>
    public static class AnalyticHelpers
    {
        /// <summary>
        /// Finds all intersections of given analytic objects. They must not
        /// contain <see cref="Point"/>s and duplicate objects.
        /// </summary>
        /// <param name="objects">The objects to be intersected.</param>
        /// <returns>The intersections array.</returns>
        public static Point[] Intersect(params IAnalyticObject[] objects)
        {
            // Find which of the passed objects are lines and circles
            var lines = objects.OfType<Line>().ToArray();
            var circles = objects.OfType<Circle>().ToArray();

            // Prepare an array of possible intersections
            Point[] intersections;

            // Prepare an array or intersected objects
            IAnalyticObject[] intersected;

            // First check if we have two lines...
            if (lines.Length >= 2)
            {
                // If yes, then we intersect them
                var intersection = lines[0].IntersectionWith(lines[1]);

                // If there is no intersection, then we can immediately return an empty array
                if (intersection == null)
                    return new Point[0];

                // Otherwise we set the intersections to this single one
                intersections = new[] { intersection.Value };

                // And set the intersected objects
                intersected = new IAnalyticObject[] { lines[0], lines[1] };
            }
            // If we don't have two lines, check if we have at best one line
            else if (lines.Length == 1)
            {
                // If yes, we'll intersect this line with a circle (there must be at least one)
                intersections = circles[0].IntersectWith(lines[0]);

                // And set the intersected objects
                intersected = new IAnalyticObject[] { lines[0], circles[0] };
            }
            // If we don't have any line...
            else
            {
                // We intersect the first two circles
                intersections = circles[0].IntersectWith(circles[1]);

                // And set the intersected objects
                intersected = new IAnalyticObject[] { circles[0], circles[1] };
            }

            // Now we find the remaining (non-intersected) objects
            var remaningObjects = objects.Where(obj => !intersected.Contains(obj)).ToArray();

            // And select those intersection points that lie on all the remaining objects
            return intersections.Where(point => remaningObjects.All(obj => LiesOn(obj, point))).ToArray();
        }

        /// <summary>
        /// Checks if a given point lies on a given analytic object. The object must not be a point.
        /// </summary>
        /// <param name="analyticObject">The analytic object.</param>
        /// <param name="point">The point.</param>
        /// <returns>true, if the point lies on the object; false otherwise.</returns>
        public static bool LiesOn(IAnalyticObject analyticObject, Point point)
        {
            switch (analyticObject)
            {
                // The line case
                case Line line:
                    return line.Contains(point);

                // The circle case
                case Circle circle:
                    return circle.Contains(point);

                // Otherwise we have a problem....
                default:
                    throw new AnalyticException("Incorrect analytic object.");
            }
        }

        /// <summary>
        /// Finds out if given points are collinear.
        /// </summary>
        /// <param name="points">The points to be checked.</param>
        /// <returns>true, if all the passed points are collinear; false otherwise.</returns>
        public static bool AreCollinear(params Point[] points)
        {
            // Take two points and construct their line
            var line = new Line(points[0], points[1]);

            // Return if all other points lie on this line
            return points.Skip(2).All(line.Contains);
        }

        /// <summary>
        /// Constructs a random scalene acute-angled triangle.
        /// </summary>
        /// <returns>An array of three points that make the triangle.</returns>
        public static Point[] ConstructRandomScaleneAcuteTriangle()
        {
            // First we normally place two points
            var a = new Point(0, 0);
            var b = new Point(1, 0);

            // In order to generate a third point C, we need to establish the rules. We want each two 
            // angles <A, <B, <C to have the absolute difference at least d, where d is a constant. Now 
            // we WLOG assume that <A is the largest angle and <C the smallest angle. We also want to have 
            // <C to be at least 'd' and 90 - <A to be at least 'd'. In that way we get a triangle that is 
            // acute, isn't too flat, isn't too close to a right-angled triangle and isn't close to an 
            // isosceles triangle. It can be shown that if we generate <A from the interval (60+d, 90-d)
            // and then <B from the interval ((180+d-A)/2, A-d), that we will get the wanted result (simple
            // math). In order to be able to generate <A, we need to have d from the interval (0,15). 
            // Generally the bigger, the better, but not very close to 15, because in that case  we might 
            // limit the variety of possible triangles (alpha would always be very close to 75). 
            const double d = 5;

            // Let us generate angles according to our formulas
            var alpha = RandomnessHelper.NextDouble(60 + d, 90 - d);
            var beta = RandomnessHelper.NextDouble((180 + d - alpha) / 2, alpha - d);

            // Now we need to construct a triangle with these angles (and our starting points A, B)
            // A simple way to achieve this is to use tangents. Let C = (x, y). Then the vector
            // C - A = (x, y) has the slope <A and the vector C - B = (x-1, y) has the slope 180 - <B.
            // From this we have two equations:
            //
            // y/x = tan(<A)
            // y/(x-1) = tan(180-<B)
            //
            // One can easy derive that 
            //
            // x = tan(<A) / (tan(<A) - tan(180-<B))
            // y = tan(<A) tan(180-<B) / (tan(<A) - tan(180-<B))
            //
            // It's also easy to see that the denominator can't be 0.
            // (in short, we would have either <A + <B = 180, or 180 - <B = 90 - <A, both are impossible)
            // 
            // Therefore we may happily generate point C

            // First calculate tangents
            var tanAlpha = Math.Tan(MathematicalHelpers.ToRadians(alpha));
            var tan180MinusBeta = Math.Tan(MathematicalHelpers.ToRadians(180 - beta));

            // Then calculate the coordinates
            var x = tan180MinusBeta / (tan180MinusBeta - tanAlpha);
            var y = tanAlpha * tan180MinusBeta / (tan180MinusBeta - tanAlpha);

            // Construct the point
            var c = new Point(x, y);

            // And return all of them
            return new[] { a, b, c };
        }

        /// <summary>
        /// Constructs the perpendicular bisector of the line defined by given distinct points.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The perpendicular bisector of the line [point1][point2].</returns>
        public static Line PerpendicularBisector(Point point1, Point point2)
        {
            // First we get the midpoint between these two points
            // This is a first needed point lying on the desired line
            var midpoint = (point1 + point2) / 2;

            // Rotate the first point around the midpoint by 90 degrees.
            // This is a second needed point lying on the desired line
            var rotatedPoint = point1.Rotate(midpoint, 90);

            // We can construct the line from these two points
            return new Line(midpoint, rotatedPoint);
        }

        /// <summary>
        /// Constructs the internal angle bisector of the angle [rayPoint1][vertex][rayPoint2]
        /// (or equivalently [rayPoint2][vertex][rayPoint1]). All three points must be distinct
        /// and not collinear. 
        /// </summary>
        /// <param name="vertex">The vertex of the angle.</param>
        /// <param name="rayPoint1">The first point on the ray starting from the vertex.</param>
        /// <param name="rayPoint2">The second point on the ray starting from the vertex.</param>
        /// <returns>The internal angle bisector of the angle [rayPoint1][vertex][rayPoint2].</returns>
        public static Line InternalAngleBisector(Point vertex, Point rayPoint1, Point rayPoint2)
        {
            // Denote A = [vertex], B = [rayPoint1], C = [rayPoint2]
            // We're going to construct line AX, where X is the intersection
            // of the internal angle bisector of BAC with BC. It's known that
            // BX / CX = AB / AC. One can use this to calculate the relation
            // X = B + |AB|/(|AB|+|AC|) . (C-B) (this is a vector relation)

            // First calculate the distances
            var distanceAB = vertex.DistanceTo(rayPoint1);
            var distanceAC = vertex.DistanceTo(rayPoint2);

            // Make sure their sum is not null...
            if ((distanceAB + distanceAC).Rounded() == 0)
                throw new AnalyticException("Cannot construct the angle bisector using equal points");

            // Construct point X according to the formula
            var X = rayPoint1 + distanceAB / (distanceAB + distanceAC) * (rayPoint2 - rayPoint1);

            // And finally line AX
            return new Line(vertex, X);
        }

        /// <summary>
        /// Calculates the angle between two lines.
        /// </summary>
        /// <param name="line1">The first line.</param>
        /// <param name="line2">The second line.</param>
        /// <returns>The angle between the lines, in radians. The value will in the interval [0, PI/2].</returns>
        public static double AngleBetweenLines(Line line1, Line line2)
        {
            // Check the parallel case. 
            if (line1.IsParallelTo(line2))
                return 0;

            // Check the perpendicular case
            if (line1.IsPerpendicularTo(line2))
                return Math.PI / 2;

            // Otherwise use the well-known formula that the angle between two vectors
            // u,v is arccos(|u.v| / (||u|| ||v||)). In our case (A, B) is the normal
            // vector of our line, and in our case it's normalized, i.e. ||u|| = ||v|| = 1. 
            // In order to find the angle between two lines we can simply find the angle
            // between its normal vectors. This yields a very simple formula:
            return Math.Acos(Math.Abs(line1.A * line2.A + line1.B * line2.B));
        }
    }
}