using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PolygonEditor
{
    public class GPoint : GeometryBase
    {
        private Point Coords;                           // Координаты точки
        private double Radius = 1;                      // Радиус для более наглядной визуализации точки

  //--- Конструкторы

        // Создание точки при ручном вводе параметров в поля ввода
        public GPoint(FrameworkElement pane, Point point) //  Создание точки по координатам преданным в параметре
            : base(pane)
        {
            //ShapeName = shapeName;
            //ShapeType = "Point";
            //StrokeWeight = 1;
            //StrokeColor = "#FF000000";
            //FillColor = "#FF808080";
            //GeometricParams = new List<namedParameter>();

            //GeometricParams.Add(new namedParameter { ParamName = "PointCordinates", ParamValue = new Point(x, y) });

            //controlPoints = new ArrayList();
            //controlPoints.Add(Coords);
        }

        public GPoint(FrameworkElement pane, string pointName, string[] textboxNames)
            : base(pane)
        {
            //ShapeName = pointName;
            //Parse(textboxNames);
            //SetParams(GeometricParams);
        }

        private void SetParams()
        {

        }

#region Реализация абстрактных методов базового класса

        //  Парсинг поля ввода из массива textboxNames, в которое пользователь ввел параметры точки
        public override void Parse(string[] textboxNames)
        {
            TextBox tb_PointCoords = LogicalTreeHelper.FindLogicalNode(parentPane, textboxNames[0]) as TextBox;
            if (tb_PointCoords != null)
            {
                if (controlPoints.Count > 0)
                    controlPoints.RemoveRange(0, controlPoints.Count);

                Coords = pointParser(tb_PointCoords.Text);
                controlPoints.Add(Coords);
            }
        }

        //  Создание визуальной реализации фигуры типа Точка
        public override Geometry CreateGeometry()
        {
            return (new EllipseGeometry(Coords, Radius, Radius));
        }

#endregion

  //--- Варианты установки координат точки

        public void SetCoords(string str)               // Установка координат точки по строке с координатами
        {
            if (controlPoints.Count > 0)
                controlPoints.RemoveRange(0, controlPoints.Count);

            Coords = pointParser(str);
            controlPoints.Add(Coords);
        }

        public void SetCoords(Point pnt)               // Установка координат точки по структуре Point
        {
            if (controlPoints.Count > 0)
                controlPoints.RemoveRange(0, controlPoints.Count);

            Coords = pnt;
            controlPoints.Add(Coords);
        }

    }
}
