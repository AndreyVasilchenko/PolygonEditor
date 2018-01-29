using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;


namespace PolygonEditor
{
    public class ShapeLine : ShapeBase
    {
        // Создание фигуры-линии по параметрам заданным в текстовых полях ввода
        public ShapeLine(object params_source,                                                      // Источник координат для построения фигуры
                         EditorModel editor)                                                    // Ссылка на экземляр модели редактора
            : base(editor)
        {
            if (params_source is List<TextBox>)                                                     //  Если источник параметров для создания фигуры - текстовые поля,
            {
                _List_tBox = (List<TextBox>)params_source;                                          //      Задаем список текстовых полей ввода параметров фигуры

                ShapeName = "Line" + _Editor.LinesCount;                                            //      Присвоим фигуре название
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
                throw new System.ApplicationException("ShapeLine.ShapeLine(): Unknow param_source type!");

            paramNames.Add("StartPoint");                                                           //  Добавим название первого параметра в список с названиями параметров
            paramTypes.Add("Point");                                                                //  Добавим название типа первого параметра в список с названиями типов параметров
            paramNames.Add("EndPoint");                                                             //  Добавим название второго параметра в список с названиями параметров
            paramTypes.Add("Point");                                                                //  Добавим название типа второго параметра в список с названиями типов параметров

            shapeType = SHAPE_TYPE.Line;                                                            //  Установим тип фигуры - Линия
            ShapeType = shapeType.ToString();

            Parse(_List_tBox);                                                                      //  Проверяем правильность ввода параметров в текстовых полях и переводим эти параметры данные для построения фигуры
            shapeBase = new Path();                                                                 //  Cоздаем экземпляр фигуры
            ((Path)shapeBase).Data = CreateShapeGeometry();                                         //  Создадим геометрию фигуры

            //shapeBase.Name = ShapeName;                                                             //  Присвоим фигуре название

            shapeBase.StrokeThickness = StrokeWeight;                                               //  Зададим толшину обводки фигуры
            shapeBase.StrokeStartLineCap = PenLineCap.Round;                                        //  закругление в начале линии
            shapeBase.StrokeEndLineCap = PenLineCap.Round;                                          //  закругление в конце линии
            //RenderOptions.SetEdgeMode(shapeBase, EdgeMode.Aliased);                                 //  Прорисовка линии без визуального сглаживания пикселей

            SolidColorBrush scb = new SolidColorBrush();                                            //  Зададим цвет обводки фигуры
            scb.Color = (Color)ColorConverter.ConvertFromString(StrokeColor);
            shapeBase.Stroke = scb;

            shapeBase.ToolTip = ShapeName;                                                     //  Установим "подсказку" о фигуре

            ContextMenu context_menu = new ContextMenu();                                           //  Установим для фигуры контекстное меню
            context_menu.Items.Add(new MenuItem() { Header = "RenameShape", Command = new ShapeContextMenuViewModel(_Editor, this).ShapeRename });
            context_menu.Items.Add(new MenuItem() { Header = "DeleteShape", Command = new ShapeContextMenuViewModel(_Editor, this).ShapeDelete });
            shapeBase.ContextMenu = context_menu;

            _Editor.DrawPanel.Children.Add(shapeBase);                                              //  Добавим фигуру на панель рисования редактора
            _Editor.LinesCount++;                                                                   //  Фигур-линий в редакторе стало на одну больше

            ControlPointsCount = 2;
            visualControler = new VisualControler(_Editor, this, ControlPointsCount);               //  Добавим к фигуре контроллер управления ее параметрами
            ((Ellipse)visualControler.ControlPoints[0]).Name = shapeBase.Name + "_StartPoint";      //  Зададим название первому контролу
            ((Ellipse)visualControler.ControlPoints[1]).Name = shapeBase.Name + "_EndPoint";        //  Зададим название второму контролу
        }


        #region Реализация абстрактных методов базового класса

            //  Парсинг текстовых полей ввода координат
            public override void Parse(List<TextBox> list_tBox)
            {
                if (list_tBox != null)
                {
                    if (list_tBox.Count == 2)
                    {
                        if (GeometricParams.Count > 0)
                            GeometricParams.RemoveRange(0, GeometricParams.Count);

                        namedParameter param1 = new namedParameter();
                        param1.ParamName = paramNames[0];
                        param1.ParamValue = pointParser(list_tBox[0].Text);
                        namedParameter param2 = new namedParameter();
                        param2.ParamName = paramNames[1];
                        param2.ParamValue = pointParser(list_tBox[1].Text);

                        GeometricParams.Add(param1);
                        GeometricParams.Add(param2);
                    }
                    else
                    {
                        throw new System.ApplicationException("There should be two text boxes for entering the coordinates of the Line !");
                    }
                }
                else
                {
                    throw new System.ApplicationException("There is no list of text fields for entering coordinates of the Line !");
                }

            }

            //  Создание визуальной реализации фигуры 
            public override Geometry CreateShapeGeometry()
            {
                LineGeometry line = new LineGeometry((Point)GeometricParams[0].ParamValue, (Point)GeometricParams[1].ParamValue);
                
                return (line);
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
                    case "StartPoint":
                        ((LineGeometry)((Path)shapeBase).Data).StartPoint = endLocation;

                        GeometricParams.RemoveAt(0);
                        namedParameter param1 = new namedParameter();
                        param1.ParamName = paramNames[0];
                        param1.ParamValue = endLocation;
                        GeometricParams.Insert(0, param1);

                        _List_tBox[0].Text = endLocation.X.ToString() + " " + endLocation.Y.ToString();
                        break;
                    case "EndPoint":
                        ((LineGeometry)((Path)shapeBase).Data).EndPoint = endLocation;

                        GeometricParams.RemoveAt(1);
                        namedParameter param2 = new namedParameter();
                        param2.ParamName = paramNames[1];
                        param2.ParamValue = endLocation;
                        GeometricParams.Insert(1, param2);

                        _List_tBox[1].Text = endLocation.X.ToString() + " " + endLocation.Y.ToString();
                        break;
                    default:
                        throw new System.ApplicationException("Error: Incorrect controlpoint type, '" + controlPointType + "' in UpdateGeometryShapeByControl()");
                }
            }

            //  Обновление Фигуры-Точки по кординатам заданным в поле ввода
            public override void UpdateShapeGeometryByTextCoords()
            {
                Parse(_List_tBox);
                ((Path)shapeBase).Data = CreateShapeGeometry();                                         //  Создадим геометрию фигуры

                shapeBase.StrokeThickness = StrokeWeight = _Editor.ShapeStrokeWidth;                    //  Зададим толшину обводки фигуры
                StrokeColor = _Editor.ShapeStrokeColor.ToString();                                      //  Установим текстовое значение цвета обводки фигуры
                SolidColorBrush scb = new SolidColorBrush();                                            //  Зададим цвет обводки фигуры
                scb.Color = _Editor.ShapeStrokeColor;
                shapeBase.Stroke = scb;

                visualControler.UpdateAllControlsCoords();
            }

            //  Обновление текстовых кординат по фактическому расположению фигуры на панели рисования
            public override void UpdateTextCoords()
            {
                _List_tBox[0].Text = ((Point)GeometricParams[0].ParamValue).X.ToString() + " " + ((Point)GeometricParams[0].ParamValue).Y.ToString();
                _List_tBox[1].Text = ((Point)GeometricParams[1].ParamValue).X.ToString() + " " + ((Point)GeometricParams[1].ParamValue).Y.ToString();
            }

            //  Обновление текстовых кординат по фактическому расположению фигуры на панели рисования
            public override void SetTextCoords(ShapeParams param)
            {
                if (_List_tBox == null)                                                                 //  Если списка текстовых полей для ввода параметров нет
                {
                    _List_tBox = new List<TextBox>();                                                   //  Создаем его

                    for (int i = 0; i < _Editor.ParamPanels.Children.Count; i++)                        //  Перебираем весь список панелей с текстовыми полями для задания параметров фигур
                    {
                        if (((Panel)_Editor.ParamPanels.Children[i]).Name == "LinePanel")               //      Если эта пенель для линии, то
                        {
                            foreach (var elem in ((Panel)_Editor.ParamPanels.Children[i]).Children)     //          перебираем все элементы панели
                            {
                                if (elem is TextBox)                                                    //              если этот элемент текс-бокс,
                                    _List_tBox.Add((TextBox)elem);                                      //                  добавляем его в список полей текст-боксов
                            }

                            break;                                                                      //          выходим из цикла
                        }
                    }
                }

                if (param.GeometricParams[0].ParamValue is string)
                    _List_tBox[0].Text = (string)param.GeometricParams[0].ParamValue;
                else if (param.GeometricParams[0].ParamValue is Point)
                {
                    _List_tBox[0].Text = ((Point)param.GeometricParams[0].ParamValue).X.ToString() + "," + ((Point)param.GeometricParams[0].ParamValue).Y.ToString();
                }
                else
                    throw new System.ApplicationException("ShapeLine.SetTextCoords(): Unknown Type of ParamValue - the reason must be clarified by programmer!");

                if (param.GeometricParams[1].ParamValue is string)
                    _List_tBox[1].Text = (string)param.GeometricParams[1].ParamValue;
                else if (param.GeometricParams[1].ParamValue is Point)
                {
                    _List_tBox[1].Text = ((Point)param.GeometricParams[1].ParamValue).X.ToString() + "," + ((Point)param.GeometricParams[1].ParamValue).Y.ToString();
                }
                else
                    throw new System.ApplicationException("ShapeLine.SetTextCoords(): Unknown Type of ParamValue - the reason must be clarified by programmer!");
            }

            //  Очистка содержимого текстовых полей для ввода координат фигуры
            public override void ClearTextCoords()
            {
                if (_List_tBox != null)                                                                 //  Если список с полями-боксами есть, то
                {
                    _List_tBox[0].Text = "";
                    _List_tBox[1].Text = "";
                }
            }

            //  Удаление визуальной реализации фигуры из панелей содержащих фигуру и ее контролы
            public override void RemoveShapeFromPanels()
            {
                for (int i = 0; i < visualControler.ControlPoints.Count; i++)
                {
                    controlPane.Children.Remove((Ellipse)visualControler.ControlPoints[i]);
                    controlPane.Children.Remove((Rectangle)visualControler.MarkerPoints[i]);
                }
                drawingPane.Children.Remove(this.shapeBase);
            }

            // Переименовать фигуру
            public override void Rename(string new_name)
            {
                ShapeName = new_name;
                shapeBase.ToolTip = new_name;
                ((Ellipse)visualControler.ControlPoints[0]).ToolTip = new_name;
                ((Ellipse)visualControler.ControlPoints[1]).ToolTip = new_name;
            }
            
            public override void AddTextFieldToParamPanel()
            { }
            public override void RemoveTextFieldFromParamPanelAt(int field_index)
            { }
        #endregion
    }
}
