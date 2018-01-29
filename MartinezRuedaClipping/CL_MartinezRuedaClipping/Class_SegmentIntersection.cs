using System;
using System.Windows;


namespace CL_MartinezRuedaClipping
{
    public partial class MartinezRuedaClipping
    {
        const double EPSILON = 1e-9;

        // Finds the magnitude of the cross product of two vectors (if we pretend* they're in three dimensions)
        internal static double crossProduct(Vector a, Vector b)
        {
          return (a.X * b.Y - a.Y * b.X);       //  The magnitude of the cross product
        }

        // Finds the dot product of two vectors.
        internal static double dotProduct(Vector a, Vector b)
        {
          return a.X * b.X + a.Y * b.Y;         //  The dot product
        }

        /* Finds the intersection (if any) between two line segments a and b, given the line segments end points a1, a2 and b1, b2.
        *
        * This algorithm is based on Schneider and Eberly.
        * http://www.cimec.org.ar/~ncalvo/Schneider_Eberly.pdf
        * Page 244.
        *
        * @param {Array.<Number>} a1 point of first line
        * @param {Array.<Number>} a2 point of first line
        * @param {Array.<Number>} b1 point of second line
        * @param {Array.<Number>} b2 point of second line
        * @param {Boolean=}       noEndpointTouch whether to skip single touchpoints(meaning connected segments) as intersections
        * @returns {Array.<Array.<Number>>|Null} If the lines intersect, the point of intersection. If they overlap, the two end points of the overlapping segment.
        *                                        Otherwise, null. */
        internal static Point[] SegmentIntersection(Point a1, Point a2, Point b1, Point b2, bool noEndpointTouch = false)
        {
            // The algorithm expects our lines in the form P + sd, where P is a point,
            // s is on the interval [0, 1], and d is a vector.
            // We are passed two points. P can be the first point of each pair. The
            // vector, then, could be thought of as the distance (in x and y components)
            // from the first point to the second point.
            // So first, let's make our vectors:
            Vector va = new Vector(a2.X - a1.X, a2.Y - a1.Y);
            Vector vb = new Vector(b2.X - b1.X, b2.Y - b1.Y);

            // The rest is pretty much a straight port of the algorithm.
            Vector e = new Vector(b1.X - a1.X, b1.Y - a1.Y);
            var kross    = crossProduct(va, vb);
            var sqrKross = kross * kross;
            var sqrLenA  = dotProduct(va, va);
            var sqrLenB  = dotProduct(vb, vb);

            // Check for line intersection. This works because of the properties of the
            // cross product -- specifically, two vectors are parallel if and only if the
            // cross product is the 0 vector. The full calculation involves relative error
            // to account for possible very small line segments. See Schneider & Eberly
            // for details.
            if (sqrKross > EPSILON * sqrLenA * sqrLenB) 
            {
                // If they're not parallel, then (because these are line segments) they
                // still might not actually intersect. This code checks that the
                // intersection point of the lines is actually on both line segments.
                var s = crossProduct(e, vb) / kross;
                if (s < 0 || s > 1) // not on line segment a
                    return null;

                var t = crossProduct(e, va) / kross;
                if (t < 0 || t > 1) // not on line segment b
                    return null;
                
                return (noEndpointTouch ? null : new Point[]{ toPoint(a1, s, va) });
            }

            // If we've reached this point, then the lines are either parallel or the
            // same, but the segments could overlap partially or fully, or not at all.
            // So we need to find the overlap, if any. To do that, we can use e, which is
            // the (vector) difference between the two initial points. If this is parallel
            // with the line itself, then the two lines are the same line, and there will
            // be overlap.
            var sqrLenE = dotProduct(e, e);
            kross = crossProduct(e, va);
            sqrKross = kross * kross;

            if (sqrKross > EPSILON * sqrLenA * sqrLenE) // Lines are just parallel, not the same. No overlap.
                return null;

            var sa = dotProduct(va, e) / sqrLenA;
            var sb = sa + dotProduct(va, vb) / sqrLenA;
            var smin = Math.Min(sa, sb);
            var smax = Math.Max(sa, sb);

            // this is, essentially, the FindIntersection acting on floats from
            // Schneider & Eberly, just inlined into this function.
            if (smin <= 1 && smax >= 0) 
            {
                // overlap on an end point
                if (smin == 1) 
                    return noEndpointTouch ? null : new Point[]{toPoint(a1, smin > 0 ? smin : 0, va)};

                if (smax == 0)
                    return noEndpointTouch ? null : new Point[]{toPoint(a1, smax < 1 ? smax : 1, va)};

                if (noEndpointTouch && smin == 0 && smax == 1)
                    return null;

                // There's overlap on a segment -- two points of intersection. Return both.
                return new Point[]{ toPoint(a1, smin > 0 ? smin : 0, va),  toPoint(a1, smax < 1 ? smax : 1, va) };
            }

            return null;
        }
        
        // We also define a function to convert back to regular point form:
        internal static Point toPoint(Point p, double s, Vector d)
        {
            return (new Point(p.X + s * d.X, p.Y + s * d.Y));
        }
    }
}