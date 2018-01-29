using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Media;

//namespace PolygonEditor
//{
//    public class GPolygon : GeometryBase
//    {
//        Point startpoint, point;
//        Size size;
//        bool largearc;
//        SweepDirection sweepArcDirection;
//        double xrotation;


//        public GPolygon(FrameworkElement pane)
//        : base(pane)
//        {
//            controlPoints = new ArrayList();
//            geometryType = "Arc";
//            Parse();
//        }

//        public override Geometry CreateGeometry()
//        {
//            PathGeometry ret_val = new PathGeometry();
//        //    PathFigure pf = new PathFigure();
//        //    pf.StartPoint = startpoint;
//        //    pf.Segments.Add(new PointSegment(point, size, xrotation, largearc, sweepArcDirection, true));
//        //    ret_val.Figures.Add(pf);
//            return ret_val;
//        }

//        public override void Parse()
//        {
//            TextBox tb_startpoint = LogicalTreeHelper.FindLogicalNode(parentPane, "ArcStartPoint") as TextBox;
//            TextBox tb_point = LogicalTreeHelper.FindLogicalNode(parentPane, "ArcPoint") as TextBox;
//            TextBox tb_size = LogicalTreeHelper.FindLogicalNode(parentPane, "ArcSize") as TextBox;
//            TextBox tb_xrotation = LogicalTreeHelper.FindLogicalNode(parentPane, "ArcXRotation") as TextBox;
//            ComboBox cb_sweeparc = LogicalTreeHelper.FindLogicalNode(parentPane, "ArcSweepArc") as ComboBox;
//            ComboBox cb_largearc = LogicalTreeHelper.FindLogicalNode(parentPane, "ArcLargeArc") as ComboBox;

//            startpoint = pointParser(tb_startpoint.Text);
//            point = pointParser(tb_point.Text);
//            size = sizeParser(tb_size.Text);
//            xrotation = doubleParser(tb_xrotation.Text);
//            sweepArcDirection = (SweepDirection)Enum.Parse(

//                typeof(SweepDirection),
//                (
//                    (string)
//                    (
//                        (ComboBoxItem)cb_sweeparc.SelectedItem
//                    ).Content
//                )
//            );

//            largearc = Boolean.Parse(((String)((ComboBoxItem)cb_largearc.SelectedItem).Content));
//            controlPoints.Add(startpoint);
//            controlPoints.Add(point);
//        }
//    }
//}
