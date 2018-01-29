using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace PolygonEditor
{
    public class ShapePoint : ShapeBase
    {
        private double Radius = 1;                                                                  //  Радиус круга с помощью которого изображается фигура-точка

        // Создание фигуры-точки по параметрам заданным в текстовых полях ввода
        public ShapePoint(object params_source,                                                     // Источник координат для построения фигуры
                            EditorModel editor)                                                     // Ссылка на экземляр модели редактора
            : base(editor)   
        {
            if (params_source is List<TextBox>)                                                     //  Если источник параметров для создания фигуры - текстовые поля,
            {
                _List_tBox = (List<TextBox>)params_source;                                          //      Задаем список текстовых полей ввода параметров фигуры

                ShapeName = "Point" + _Editor.PointsCount;                                          //      Присвоим фигуре название
                StrokeWeight = _Editor.ShapeStrokeWidth;                                            //      Зададим толшину обводки фигуры
                StrokeColor = _Editor.ShapeStrokeColor.ToString();                                  //      Установим текстовое значение цвета обводки фигуры
            }
            else if (params_source is ShapeParams)                                                  //  Если источник параметров для создания фигуры - загрузочная информация,
            {
                ClearTextCoords();                                                                  //      Очищаем текст-боксы на панели ввода параметров и удаляем лишние
                SetTextCoords((ShapeParams)params_source);                                          //      Заполняем текст-боксы полученной информацией о геометрических параметрах

                ShapeName = ((ShapeParams)params_source).ShapeName;                                 //      Зададаем остальные свойства фигуры....
                StrokeWeight = ((ShapeParams)params_source).StrokeWeight;
                StrokeColor = ((ShapeParams)params_source).StrokeColor;
            }
            else                                                                                    //  Иначе ...
                throw new System.ApplicationException("ShapePoint.ShapePoint(): Unknow param_source type!");

            paramNames.Add("PointCordinates");                                                      //  Добавим название параметра в список с названиями параметров
            paramTypes.Add("Point");                                                                //  Добавим название типа типа параметра в список с названиями типов параметров

            shapeType = SHAPE_TYPE.Point;
            ShapeType = shapeType.ToString();                                                       //  Установим тип фигуры - Точка

            Parse(_List_tBox);
            shapeBase = new Path();                                                                 //  Cоздаем экземпляр фигуры
            ((Path)shapeBase).Data = CreateShapeGeometry();                                         //  Создадим геометрию фигуры

            shapeBase.StrokeThickness = StrokeWeight;                                               //  Зададим толшину обводки фигуры

            SolidColorBrush scb = new SolidColorBrush();                                            //  Зададим цвет обводки фигуры
            scb.Color = (Color)ColorConverter.ConvertFromString(StrokeColor);
            shapeBase.Stroke = scb;

            shapeBase.ToolTip = ShapeName;                                                          //  Установим "подсказку" о фигуре

            ContextMenu context_menu = new ContextMenu();                                           //  Установим для фигуры контекстное меню
            context_menu.Items.Add(new MenuItem() { Header = "RenameShape", Command = new ShapeContextMenuViewModel(_Editor, this).ShapeRename });
            context_menu.Items.Add(new MenuItem() { Header = "DeleteShape", Command = new ShapeContextMenuViewModel(_Editor, this).ShapeDelete });
            shapeBase.ContextMenu = context_menu;

            _Editor.DrawPanel.Children.Add(shapeBase);                                              //  Добавим фигуру на панель рисования редактора
            _Editor.PointsCount++;                                                                  //  Фигур-точек в редакторе стало на одну больше

            ControlPointsCount = 1;
            visualControler = new VisualControler(_Editor, this, ControlPointsCount);               //  Добавим к фигуре контроллер управления ее параметрами
            ((Ellipse)visualControler.ControlPoints[0]).Name = shapeBase.Name + "_Center";          //  Зададим название первому и единственному контролу
            ClearTextCoords();                                                                      //  Очищаем текстовые поля ввода на панели ввода параметров фигуры
        }


        #region Реализация абстрактных методов базового класса

            //  Парсинг текстового поля ввода координат фигуры
            public override void Parse(List<TextBox> list_tBox)
            {
                if (list_tBox != null && list_tBox.Count > 0)
                {
                    if (GeometricParams.Count > 0)
                        GeometricParams.RemoveRange(0, GeometricParams.Count);

                    namedParameter param = new namedParameter();
                    param.ParamName = paramNames[0];
                    param.ParamValue = pointParser(list_tBox[0].Text);

                    GeometricParams.Add(param);
                }
            }

            //  Создание визуальной реализации фигуры типа Точка
            public override Geometry CreateShapeGeometry()
            {
                return (new EllipseGeometry((Point)GeometricParams[0].ParamValue, Radius, Radius));
            }

            // Обновление визуального отображения фигуры при изменении свойств фигуры
            public override void UpdateShapeGeometryByProperty(string property_name)
            {
                switch (property_name)
                {
                    case "ShapeStrokeWidth":
                        shapeBase.StrokeThickness = StrokeWeight = _Editor.ShapeStrokeWidth;                    //  Зададим толшину обводки фигуры
                        break;
                    case "ShapeStrokeColor":
                        StrokeColor = _Editor.ShapeStrokeColor.ToString();                                      //  Установим текстовое значение цвета обводки фигуры
                        SolidColorBrush scb = new SolidColorBrush();                                            //  Зададим цвет обводки фигуры
                        scb.Color = _Editor.ShapeStrokeColor;
                        shapeBase.Stroke = scb;
                        break;
                    //case "ShapeFillColor":
                    //    break;
                    default: return;
                }
            }

            // Обновление визуального отображения фигуры при изменении положения на заданный вектор
            public override void UpdateShapeGeometryByVector(Vector locationChange)
            {
                for (int i = 0; i < GeometricParams.Count; i++)
                {
                    namedParameter param = new namedParameter();
                    Point p = new Point();
                    p.X = ((Point)GeometricParams[i].ParamValue).X + locationChange.X;
                    p.Y = ((Point)GeometricParams[i].ParamValue).Y + locationChange.Y;
                    param.ParamValue = p;
                    param.ParamName = GeometricParams[i].ParamName;
                    GeometricParams.RemoveAt(i);
                    GeometricParams.Insert(i, param);
                }
                ((Path)shapeBase).Data = CreateShapeGeometry();                                                 //  Создадим геометрию фигуры
                visualControler.UpdateAllControlsCoords();
                UpdateTextCoords();
            }

            // Обновление визуального отображения геометрической фигуры при изменении положения контрола
            public override void UpdateShapeGeometryByControl(string controlName, Point endLocation)
            {
                string[] str = controlName.Split('_');
                string controlPointType = str[1];

                switch (controlPointType)
                {
                    case "Center":
                        ((EllipseGeometry)((Path)shapeBase).Data).Center = endLocation;
                            
                        GeometricParams.RemoveAt(0);
                        namedParameter param = new namedParameter();
                        param.ParamName = paramNames[0];
                        param.ParamValue = endLocation;
                        GeometricParams.Insert(0, param);

                        //shapeBase.ToolTip = shapeBase.Name + " {" + endLocation.ToString() + "}";
                        _List_tBox[0].Text = endLocation.X.ToString() + " " + endLocation.Y.ToString();
                        break;
                    default:
                        throw new System.ApplicationException("Error: Incorrect controlpoint type, '" + controlPointType + "' in UpdateGeometryShapeByControl()");
                }
            }

            //  Обновление Фигуры-Точки по кординатам заданным в поле ввода
            public override void UpdateShapeGeometryByTextCoords()
            {
                Parse(_List_tBox);
                ((Path)shapeBase).Data = CreateShapeGeometry();                                                 //  Создадим геометрию фигуры
                visualControler.UpdateAllControlsCoords();
            }

            //  Обновление текстовых кординат по фактическому расположению фигуры на панели рисования
            public override void UpdateTextCoords()
            {
                _List_tBox[0].Text = ((Point)GeometricParams[0].ParamValue).X.ToString() + " " + ((Point)GeometricParams[0].ParamValue).Y.ToString();
            }

            //  Обновление текстовых кординат по фактическому расположению фигуры на панели рисования
            public override void SetTextCoords(ShapeParams param)
            {
                if (_List_tBox == null)                                                                 //  Если списка текстовых полей для ввода параметров нет
                {
                    _List_tBox = new List<TextBox>();                                                   //  Создаем его

                    for (int i = 0; i < _Editor.ParamPanels.Children.Count; i++)                        //  Перебираем весь список панелей с текстовыми полями для задания параметров фигур
                    {
                        if (((Panel)_Editor.ParamPanels.Children[i]).Name == "PointPanel")               //      Если эта пенель для линии, то
                        {
                            foreach (var elem in ((Panel)_Editor.ParamPanels.Children[i]).Children)     //          перебираем все элементы панели
                            {
                                if (elem is TextBox)                                                    //              если этот элемент текс-бокс,
                                {
                                    _List_tBox.Add((TextBox)elem);                                      //                  добавляем его в список полей текст-боксов
                                    break;
                                }
                            }

                            break;                                                                      //          выходим из цикла
                        }
                    }
                }

                if (param.GeometricParams[0].ParamValue is string)
                    _List_tBox[0].Text = (string)param.GeometricParams[0].ParamValue;
                else if(param.GeometricParams[0].ParamValue is Point)
                {
                    _List_tBox[0].Text = ((Point)param.GeometricParams[0].ParamValue).X.ToString() + "," + ((Point)param.GeometricParams[0].ParamValue).Y.ToString();
                }
                else
                    throw new System.ApplicationException("ShapePoint.SetTextCoords(): Unknown Type of ParamValue - the reason must be clarified by programmer!");
            }

            //  Очистка содержимого текстовых полей для ввода координат фигуры
            public override void ClearTextCoords()
            {
                if (_List_tBox != null)                                                                 //  Если список с полями-боксами есть, то
                    _List_tBox[0].Text = "";
            }

            //  Удаление визуальной реализации фигуры из панелей содержащих фигуру и ее контролы
            public override void RemoveShapeFromPanels()
            {
                for (int i = 0; i < visualControler.ControlPoints.Count; i++)
                {
                    controlPane.Children.Remove( (Ellipse)visualControler.ControlPoints[i] );
                    controlPane.Children.Remove( (Rectangle)visualControler.MarkerPoints[i]);
                }
                drawingPane.Children.Remove(this.shapeBase);
            }

            // Переименовать фигуру
            public override void Rename(string new_name)
            {
                ShapeName = new_name;
                shapeBase.ToolTip = new_name;
                ((Ellipse)visualControler.ControlPoints[0]).ToolTip = new_name;
            }

            public override void AddTextFieldToParamPanel()
            { }
            public override void RemoveTextFieldFromParamPanelAt(int field_index)
            { }
        #endregion


        //--- Варианты установки координат точки

        public void SetGeometricCoords(string str)               // Установка координат точки по строке с координатами
        {
            if (GeometricParams.Count > 0)
                GeometricParams.RemoveRange(0, GeometricParams.Count);

            namedParameter param = new namedParameter();
            param.ParamName = paramNames[0];
            param.ParamValue = pointParser(str);
            GeometricParams.Add(param);
        }

        public void SetGeometricCoords(Point pnt)               // Установка координат точки по структуре Point
        {
            if (GeometricParams.Count > 0)
                GeometricParams.RemoveRange(0, GeometricParams.Count);

            namedParameter param = new namedParameter();
            param.ParamName = paramNames[0];
            param.ParamValue = pnt;
            GeometricParams.Add(param);
        }

    }
}
