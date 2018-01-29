using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

namespace PolygonEditor
{
    public class FigurePoint : GPoint
    {
        public Path path;                       //  Фигура-контейнер с возможностью отрисовки любых геометр.фигур входящих в ее состав
        public Canvas PaneControl;              //  Панель на которой размещаются точки-контролы 
        public MouseControler mouseControler;   //

        public FigurePoint(Canvas paneDraw,     //  1.Панель типа "Канва" на которой рисуем фигуру
                           Canvas paneControl,  //  2.Панель типа "Канва" на которой рисуем точки-контролы 
                           Point pnt,           //  3.Координаты точки 
                           ref int count)       //  4.Количество точек уже нарисованных на канве
            : base(paneDraw, pnt)
        {
            PaneControl = paneControl;

            path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;

            count++;
            
            path.Name = "Point" + count;
            path.ToolTip = path.Name + " {" + pnt.ToString() + "}";

            path.Data = CreateGeometry();
            
            paneDraw.Children.Add(path);

            mouseControler = new MouseControler(PaneControl, path, controlPoints, UpdatePointGeometry);

            ((Ellipse)mouseControler.Points[0]).Name = path.Name + "_Center";
        }

        public void UpdatePointGeometry(string controlName, Point endLocation)
        {
            string[] str= controlName.Split('_');
            string controlPointType = str[1];
            
            switch (controlPointType)
            {
                case "Center":
                    ((EllipseGeometry)path.Data).Center = endLocation;
                    path.ToolTip = path.Name + " {" + endLocation.ToString() + "}";
                    break;
                default:
                    throw new System.ApplicationException("Error: Incorrect controlpoint type, '" + controlPointType + "' in UpdatePointGeometry()");
            }
        }

    }
}
