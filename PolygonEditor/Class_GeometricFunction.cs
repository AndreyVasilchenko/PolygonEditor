using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor
{
    public class GeometricFunction
    {
        public static bool IntersectionTwoLineSegments(Point start1, Point end1, Point start2, Point end2, ref Point intersectionPoint)
        {
/*--------------------------------------------------------------------------------------------------------------------------------------------------------------------*
  Теория:

        Уравнение прямой, проходящей через две заданные точки (x1,y1) и (x2,y2), имеет вид:     (y-y1) / (y2-y1) = (x-x1) / (x2-x1)
        или в общем виде:                                                                       (y1-y2)*x + (x2-x1)*y + (x1*y2 - x2*y1) = 0
        Т.е. получили общее уравнение прямой линии на плоскости в декартовых координатах:       A*x + B*y + C = 0
        где A и B одновременно не равны нулю.

 
            Если у нас есть отрезок P + Q*t, то тогда нормаль к прямой проходящей через этот отрезок это perp(Q) = (-Qy, +Qx), а её уравнение это 
            ((-Qy*x + Qx*y) - (-Qy*Px + Qx*Py) = 0). Переобозначив -Qy как A и т.п., получим привычное уравнение прямой (A*x + B*y + С = 0) 
            Это же уравнение записанное как неравенство даст уравнение полуплоскости.
 
            Если подставить какую-то точку (x,y) в выражение A*x + B*y + С, то если оно получилось положительным - значит точка с одной стороны прямой, 
            а если отрицательным - то с другой. Если ноль - значит, ровно на прямой. Степень отклонения от нуля этого выражения - это то, на сколько точка 
            далеко от прямой (это длина проекции отклонения на нормаль к прямой).

            Если оба отрезка пересекают прямую своего отрезка-напарника(то есть концы лежат в разных полуплоскостях этой прямой), значит пересечение есть. 
            Если концы какого-то отрезка лежат в одной полуплоскости - значит пересечения нет. 
            
            Так как выражение A*x + B*y + С характеризует насколько точка удалена от края полуплоскости, то значит для двух концов отрезка два значения показывают
            на какие части делится отрезок пересечением. А это нам сразу даёт чему равно V в точке пересечения - V делит отрезок [0, 1] точно так же,
            как точка пересечения делит отрезок AB, который задаётся как A+(B-A)*V.
*--------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            //считаем уравнения прямых проходящих через отрезки ( A*x + B*y + C = 0 или C = -(A*x + B*y)  )
            Vector dir1 = end1 - start1;                                                                    // Направленный отрезок, где X=x2-x1  и  Y=y2-y1
            double A1 = -dir1.Y;
            double B1 = +dir1.X;
            double C1 = -(A1 * start1.X + B1 * start1.Y);

            Vector dir2 = end2 - start2;
            double A2 = -dir2.Y;
            double B2 = +dir2.X;
            double C2 = -(A2 * start2.X + B2 * start2.Y);

            
            //подставляем координаты концов отрезков, для выяснения в каких полуплоскоcтях находятся эти концы
            double seg_line2_start1 = A2 * start1.X + B2 * start1.Y + C2;
            double seg_line2_end1 = A2 * end1.X + B2 * end1.Y + C2;

            double seg_line1_start2 = A1 * start2.X + B1 * start2.Y + C1;
            double seg_line1_end2 = A1 * end2.X + B1 * end2.Y + C1;


            double l1 = seg_line1_start2 * seg_line1_end2;
            double l2 = seg_line2_start1 * seg_line2_end1;

            //если концы любого из двух рассматриваемых отрезков имеют один знак, значит они  находятся в одной полуплоскости и пересечения нет.
            if ((l2 < 0 && l1 < 0) || (l2 < 0 && l1 == 0) || (l2 == 0 && l1 < 0))
            {
                double V = seg_line2_start1 / (seg_line2_start1 - seg_line2_end1);

                intersectionPoint = start1 + V * dir1;
                return (true);
            }


            return (false);
            ////если концы любого из двух рассматриваемых отрезков имеют один знак, значит они  находятся в одной полуплоскости и пересечения нет.
            //if (seg_line2_start1 * seg_line2_end1 >= 0 || seg_line1_start2 * seg_line1_end2 >= 0)
            //{
            //    return (false);
            //}

            //double V = seg_line2_start1 / (seg_line2_start1 - seg_line2_end1);

            //intersectionPoint = start1 + V * dir1;

            //return (true);
        }



        public double DistancePointToPoint(Point point1, Point point2)
        {
            return (point2 - point1).Length;
        }
        
        
        public void TestDecartToPolar()
        {
            Decart d = new Decart(10, 6);
            Polar p = d;
            Decart dd = p;

            MessageBox.Show(p.ToString());
            MessageBox.Show(dd.ToString());
        }


        public class Decart
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Decart(double x, double y)
            {
                X = x;
                Y = y;
            }

            public static implicit/*=*/ operator Polar(Decart decart)
            {
                double r = Math.Sqrt(decart.X * decart.X + decart.Y * decart.Y);
                double phi = Math.Acos(decart.X / r);

                return new Polar(r, phi);
            }

            public override string ToString()
            {
                return string.Format("X: {0}, Y: {1}", X, Y);
            }
        }

        public class Polar
        {
            public double R { get; set; }
            public double Phi { get; set; }

            public Polar(double r, double phi)
            {
                R = r;
                Phi = phi;
            }

            public static implicit/*=*/ operator Decart(Polar polar)
            {
                double x = polar.R * Math.Cos(polar.Phi);
                double y = polar.R * Math.Sin(polar.Phi);
                return new Decart(x, y);
            }

            public override string ToString()
            {
                return string.Format("R: {0}, Phi: {1}", R, Phi);
            }
        }
    }
}
