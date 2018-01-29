using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Media;
namespace PolygonEditor
{
    public class GLine : GeometryBase
    {
        private Point startPoint;
        private Point endPoint;

        public GLine(FrameworkElement pane, string[] textboxName)
            : base(pane)
        {
            controlPoints = new ArrayList();
            geometryType = "Line";
            Parse(textboxName);
        }

#region Реализация абстрактных методов базового класса

        public override void Parse(string[] textboxName)
        {
            TextBox tb_startPoint = LogicalTreeHelper.FindLogicalNode(parentPane, textboxName[0]) as TextBox;
            TextBox tb_endPoint = LogicalTreeHelper.FindLogicalNode(parentPane, textboxName[1]) as TextBox;

            if (tb_startPoint != null && tb_endPoint != null)
            {
                startPoint = pointParser(tb_startPoint.Text);
                endPoint = pointParser(tb_endPoint.Text);

                controlPoints.Add(startPoint);
                controlPoints.Add(endPoint);
            }
        }

        public override Geometry CreateGeometry()
        {
            return (new LineGeometry(startPoint, endPoint));
        }
#endregion
    
    }
}
