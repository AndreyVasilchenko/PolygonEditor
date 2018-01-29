using System;
using System.Windows;


namespace CL_MartinezRuedaClipping
{
    public partial class MartinezRuedaClipping
    {
        internal enum EdgeType : int
        {
            NORMAL = 0,
            NON_CONTRIBUTING = 1,
            SAME_TRANSITION = 2,
            DIFFERENT_TRANSITION = 3
        }

        internal static int? compareEvents(SweepEvent e1, SweepEvent e2)
        {
            // Different x-coordinate
            if (e1.point.X != e2.point.X)
                return (e1.point.X > e2.point.X) ? 1 : -1;

            // Different points, but same x-coordinate
            // Event with lower y-coordinate is processed first
            if (e1.point.Y != e2.point.Y)
                return e1.point.Y > e2.point.Y ? 1 : -1;

            return specialCases(e1, e2);
        }

        // eslint-disable no-unused-vars 
        internal static int specialCases(SweepEvent e1, SweepEvent e2)
        {
            // Same coordinates, but one is a left endpoint and the other is
            // a right endpoint. The right endpoint is processed first
            if (e1.left != e2.left)
                return e1.left ? 1 : -1;

            // Same coordinates, both events
            // are left endpoints or right endpoints.
            // not collinear
            if (signedArea(e1.point, e1.otherEvent.point, e2.otherEvent.point) != 0)
            // the event associate to the bottom segment is processed first
                return (!e1.isBelow(e2.otherEvent.point)) ? 1 : -1;

            // uncomment this if you want to play with multipolygons
            // if (e1.isSubject === e2.isSubject) {
            //   if(equals(e1.point, e2.point) && e1.contourId === e2.contourId) {
            //     return 0;
            //   } else {
            //     return e1.contourId > e2.contourId ? 1 : -1;
            //   }
            // }

            return (!e1.isSubject && e2.isSubject) ? 1 : -1;
        }

        internal static int signedArea(Point p0, Point p1, Point p2)
        {
            return (int)((p0.X - p2.X) * (p1.Y - p2.Y) - (p1.X - p2.X) * (p0.Y - p2.Y));
        }

        internal static bool equals(Point p1, Point p2)
        {
            return (p1.X == p2.X  &&  p1.Y == p2.Y);
        }
    }
}
