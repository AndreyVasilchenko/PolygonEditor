using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;


namespace PolygonEditor
{
    public class ShapePolyLine : ShapeBase
    {
        private bool IsCreateWithMouse;                //  Флаг - в каком режиме происходит создание экземпляра(Ручной ввод или рисование мышью) - необходимо для парсинга
                                                                                                               

        //--- Создание фигуры-линии по параметрам заданным в текстовых полях ввода
        public ShapePolyLine(object params_source,                                                  // Источник координат для построения фигуры
                             EditorModel editor)                                                    // Ссылка на экземляр модели редактора
            : base(editor)
        {
            if (params_source is List<TextBox>)                                                     //  Если источник параметров для создания фигуры - текстовые поля,
            {
                _List_tBox = (List<TextBox>)params_source;                                          //      Задаем список текстовых полей ввода параметров фигуры

                ShapeName = "PolyLine" + _Editor.PolyLinesCount;                                    //      Присвоим фигуре название
                StrokeWeight = _Editor.ShapeStrokeWidth;                                            //      Зададим толшину обводки фигуры
                StrokeColor = _Editor.ShapeStrokeColor.ToString();                                  //      Установим текстовое значение цвета обводки фигуры
            }
            else if (params_source is ShapeParams)                                                  //  Если источник параметров для создания фигуры - загрузочная информация,
            {
                ClearTextCoords();                                                                  //      Очищаем текст-боксы на панели ввода параметров
                SetTextCoords((ShapeParams)params_source);                                          //      Заполняем их полученной информацией о геометрических параметрах

                ShapeName = ((ShapeParams)params_source).ShapeName;                                 //      Зададаем остальные свойства фигуры....
                StrokeWeight = ((ShapeParams)params_source).StrokeWeight;
                StrokeColor = ((ShapeParams)params_source).StrokeColor;
            }
            else                                                                                    //  Иначе ...
                throw new System.ApplicationException("ShapePolyLine.ShapePolyLine(): Unknow param_source type!");

            
            for (int i = (editor.IsMouseDraws ? 2 : _List_tBox.Count); i > -1; i--)
            {
                paramNames.Add("Point" + (i + 1).ToString());                                       //  Добавим название параметра в список с названиями параметров
                paramTypes.Add("Point");                                                            //  Добавим название типа параметра в список с названиями типов параметров
            }

            shapeType = SHAPE_TYPE.PolyLine;                                                        //  Установим тип фигуры - ПолиЛиния
            ShapeType = shapeType.ToString();

            IsCreateWithMouse = _Editor.IsMouseDraws;                                               //  Создание фигуры начато при помощи мыши или иначе

            Parse(_List_tBox);                                                                      //  Проверяем правильность ввода параметров в текстовых полях и переводим эти параметры данные для построения фигуры

            if (params_source is List<TextBox> && _Editor.prop_service.properties.SelfIntersectionCheck) //  Если источник параметров для создания фигуры - текстовые поля и надо проверять самопересечение,
            {
                if (IsSelfIntersection())                                                           //      Если есть самопересечение, то
                {
                    GeometricParams.RemoveRange(0,GeometricParams.Count);                           //          чистим ресурсы фигуры и выходим
                    paramNames.RemoveRange(0, paramNames.Count);
                    paramTypes.RemoveRange(0, paramTypes.Count);
                    return;
                }
            }


            shapeBase = new Path();                                                                 //  Cоздаем экземпляр фигуры
            ((Path)shapeBase).Data = CreateShapeGeometry();                                         //  Создадим геометрию фигуры


            shapeBase.StrokeLineJoin = PenLineJoin.Round;                                           //  закругление в месте пересечения линий
            shapeBase.StrokeStartLineCap = PenLineCap.Round;                                        //  закругление в начале линии
            shapeBase.StrokeEndLineCap = PenLineCap.Round;                                          //  закругление в конце линии
            //RenderOptions.SetEdgeMode(shapeBase, EdgeMode.Aliased);                               //  Прорисовка линии без визуального сглаживания пикселей

            SolidColorBrush scb = new SolidColorBrush();                                            //  Зададим цвет обводки фигуры
            scb.Color = (Color)ColorConverter.ConvertFromString(StrokeColor);
            shapeBase.Stroke = scb;

            shapeBase.StrokeThickness = StrokeWeight;                                               //  Зададим толщину обводки фигуры

            shapeBase.ToolTip = ShapeName;                                                          // Установим "подсказку" о фигуре

            ContextMenu context_menu = new ContextMenu();                                           //  Установим для фигуры контекстное меню
            context_menu.Items.Add(new MenuItem() { Header = "RenameShape", Command = new ShapeContextMenuViewModel(_Editor, this).ShapeRename });
            context_menu.Items.Add(new MenuItem() { Header = "DeleteShape", Command = new ShapeContextMenuViewModel(_Editor, this).ShapeDelete });
            shapeBase.ContextMenu = context_menu;

            _Editor.DrawPanel.Children.Add(shapeBase);                                              //  Добавим фигуру на панель рисования редактора
            _Editor.PolyLinesCount++;                                                               //  Фигур-полиЛиний в редакторе стало на одну больше

            if (_Editor.IsMouseDraws)                                                               //  Если фигура создается при помощи мыши,
            {
                _Editor.IsPrompt = true;                                                            //  Выводим подсказку как завершить создание фигуры
                ControlPointsCount = 2;                                                             //  Количество контролов пока 2
            }
            else
                ControlPointsCount = _List_tBox.Count;                                              //  Количество контролов равно кол-ву текстовых полей

            visualControler = new VisualControler(_Editor, this, ControlPointsCount);               //  Добавим к фигуре контроллер управления ее параметрами

            if (_Editor.IsMouseDraws)                                                               //  Если фигура создается при помощи мыши,
            {
                ((Ellipse)visualControler.ControlPoints[0]).Name = shapeBase.Name + "_Point1";      //      Зададим название первому контролу
                ((Ellipse)visualControler.ControlPoints[1]).Name = shapeBase.Name + "_Point2";      //      и второму контролу
            }
            else
            {
                for (int i = 0; i < _List_tBox.Count; i++)                                          //  Зададим название всем контролам
                    ((Ellipse)visualControler.ControlPoints[i]).Name = shapeBase.Name + "_Point" + (i + 1).ToString();
            }
        }


        #region Реализация абстрактных методов базового класса ShapeBase

        //  Парсинг текстовых полей ввода координат
        public override void Parse(List<TextBox> list_tBox)
        {
            if (list_tBox != null)
            {
                int parse_count;
                if (IsCreateWithMouse)
                {
                    parse_count = 2;
                    IsCreateWithMouse = false;
                }
                else
                    parse_count = list_tBox.Count;

                if (parse_count >= 2)
                {
                    if (GeometricParams.Count > 0)
                    {
                        GeometricParams.RemoveRange(0, GeometricParams.Count);
                        paramNames.RemoveRange(0, paramNames.Count);
                        paramTypes.RemoveRange(0, paramTypes.Count);
                    }

                    for (int i = 0; i < parse_count; i++)
                    {
                        AddGeometryPoint(pointParser(list_tBox[i].Text));
                    }
                    if (visualControler != null && visualControler.ControlPoints != null && !_Editor.IsMouseDraws)
                    {
                        if (visualControler.ControlPoints.Count < GeometricParams.Count)
                            visualControler.AddControls();
                        else if (visualControler.ControlPoints.Count > GeometricParams.Count)
                            visualControler.RemoveControls();
                    }
                }
                else
                {
                    throw new System.ApplicationException("There should be minimum two text boxes for entering the coordinates of the PolyLine in Polyline.Parse()!");
                }
            }
            else
            {
                throw new System.ApplicationException("There is no list of text fields for entering coordinates of the PolyLine in Polyline.Parse()!");
            }

        }

        //  Создание визуальной реализации фигуры-полиЛинии
        public override Geometry CreateShapeGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = (Point)GeometricParams[0].ParamValue;
                PolyLineSegment polyline = new PolyLineSegment();
                    polyline.Points = new PointCollection();
                    for (int i = 0; i < GeometricParams.Count; i++)
                        polyline.Points.Add((Point)GeometricParams[i].ParamValue);
                pathFigure.Segments.Add(polyline);
            pathGeometry.Figures.Add(pathFigure);

            return (pathGeometry);
        }

        //  Обновление визуального отображения фигуры при изменении свойств фигуры
        public override void UpdateShapeGeometryByProperty(string property_name)
        {
            switch (property_name)
            {
                case "ShapeStrokeWidth":
                    shapeBase.StrokeThickness = StrokeWeight = _Editor.ShapeStrokeWidth;                    //  Зададим толшину обводки фигуры
                    break;
                case "ShapeStrokeColor":
                    SolidColorBrush scb = new SolidColorBrush();                                            //  Зададим цвет обводки фигуры
                    scb.Color = _Editor.ShapeStrokeColor;
                    shapeBase.Stroke = scb;
                    StrokeColor = _Editor.ShapeStrokeColor.ToString();                                      //  Установим текстовое значение цвета обводки фигуры
                    break;
            }
        }

        //  Обновление визуального отображения фигуры при изменении положения на заданный вектор
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

        //  Обновление визуального отображения геометрической фигуры при изменении положения контрола
        public override void UpdateShapeGeometryByControl(string controlName, Point endLocation)
        {
            string[] str = controlName.Split('_');

            if (str[1].Substring(0, 5) != "Point")
            {
                MessageBox.Show("The name of the control is unknown in UpdateGeometryShapeByControl()!");
                return;
            }

            int controlPointNumber = int.Parse(str[1].Substring(5, str[1].Length - 5));
            if (controlPointNumber < 1 || controlPointNumber > GeometricParams.Count)
            {
                MessageBox.Show("Control has an incorrect index in UpdateGeometryShapeByControl()!");
                return;
            }

            controlPointNumber--;

            ((PolyLineSegment)((PathFigure)((PathGeometry)((Path)shapeBase).Data).Figures[0]).Segments[0]).Points[controlPointNumber] = endLocation;
            if (controlPointNumber == 0)
                ((PathFigure)((PathGeometry)((Path)shapeBase).Data).Figures[0]).StartPoint = endLocation;

            GeometricParams.RemoveAt(controlPointNumber);
            namedParameter param = new namedParameter();
            param.ParamName = paramNames[controlPointNumber];
            param.ParamValue = endLocation;
            GeometricParams.Insert(controlPointNumber, param);

            _List_tBox[controlPointNumber].Text = endLocation.X.ToString() + " " + endLocation.Y.ToString();
        }

        //  Обновление Фигуры-ПолиЛиния по кординатам заданным в поле ввода
        public override void UpdateShapeGeometryByTextCoords()
        {
            // Создаем копию параметров фигуры до изменения(на случай обратного отката)
            List<Point> points = new List<Point>();
            for (int i = 0; i < GeometricParams.Count; i++)
            {
                Point p = new Point();
                p.X = ((Point)GeometricParams[i].ParamValue).X;
                p.Y = ((Point)GeometricParams[i].ParamValue).Y;
                points.Add(p);
            }

            Parse(_List_tBox);                                                                      //  Проверим заполнение текстовых полей и возьмем из них координаты фигуры
            
            if (_Editor.prop_service.properties.SelfIntersectionCheck)                              //  Если надо проверять самопересечение,
            {
                if (IsSelfIntersection())                                                           //      Если есть самопересечение, то
                {
                    // Стираем "новые" значения
                    if (GeometricParams.Count > 0)
                    {
                        GeometricParams.RemoveRange(0, GeometricParams.Count);
                        paramNames.RemoveRange(0, paramNames.Count);
                        paramTypes.RemoveRange(0, paramTypes.Count);
                    }
                    // восстанавливаем старые значения
                    for (int i = 0; i < points.Count; i++)
                    {
                        AddGeometryPoint(points[i]);
                    }
                    visualControler.RemoveControls();
                    visualControler.UpdateAllControlsCoords();                                              //  Приведем в сответствие контролы фигуры
                    return;
                }
            }
            
            
            ((Path)shapeBase).Data = CreateShapeGeometry();                                         //  Создадим геометрию фигуры

            shapeBase.StrokeThickness = StrokeWeight = _Editor.ShapeStrokeWidth;                    //  Зададим толшину обводки фигуры
            StrokeColor = _Editor.ShapeStrokeColor.ToString();                                      //  Установим текстовое значение цвета обводки фигуры
            SolidColorBrush scb = new SolidColorBrush();                                            //  Зададим цвет обводки фигуры
            scb.Color = _Editor.ShapeStrokeColor;
            shapeBase.Stroke = scb;

            visualControler.UpdateAllControlsCoords();                                              //  Приведем в сответствие контролы фигуры
        }

        //  Обновление текстовых параметров фигуры в панели ввода по фактическому расположению фигуры на панели рисования
        public override void UpdateTextCoords()
        {
            for (int i = 0; i < GeometricParams.Count; i++)                                         //  Перебираем все параметры в полученном списке параметров 
            {
                if (i == _List_tBox.Count)                                                          //      Если индекс параметра равен кол-ву текст-боксов,
                    AddTextFieldToParamPanel();                                                     //          добавляем новый тект-бокс на панель

                _List_tBox[i].Text = ((Point)GeometricParams[i].ParamValue).X.ToString() + " " + ((Point)GeometricParams[i].ParamValue).Y.ToString();//Заносим в текст бокс значение параметра
            }
        }

        //  Установка текстовых параметров фигуры в панели ввода из информации о параметрах фигуры из интерфейса
        public override void SetTextCoords(ShapeParams param)
        {
            if (_List_tBox == null)                                                                 //  Если списка текстовых полей для ввода параметров нет
            {
                _List_tBox = new List<TextBox>();                                                   //  Создаем его

                for (int i = 0; i < _Editor.ParamPanels.Children.Count; i++)                        //  Перебираем весь список панелей с текстовыми полями для задания параметров фигур
                {
                    if (((Panel)_Editor.ParamPanels.Children[i]).Name == "PolyLinePanel")            //      Если эта пенель для полигона, то
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

            for (int i = 0; i < param.GeometricParams.Count; i++)                                   //  Перебираем все параметры в полученном списке параметров 
            {
                if (i == _List_tBox.Count)                                                          //      Если индекс параметра равен кол-ву текст-боксов,
                    AddTextFieldToParamPanel();                                                     //          добавляем новый тект-бокс на панель

                if (param.GeometricParams[i].ParamValue is string)
                    _List_tBox[i].Text = (string)param.GeometricParams[i].ParamValue;
                else if (param.GeometricParams[i].ParamValue is Point)
                {
                    _List_tBox[i].Text = ((Point)param.GeometricParams[i].ParamValue).X.ToString() + "," + ((Point)param.GeometricParams[i].ParamValue).Y.ToString();
                }
                else
                    throw new System.ApplicationException("ShapePolyLine.SetTextCoords(): Unknown Type of ParamValue - the reason must be clarified by programmer!");
            }
        }

        //  Очистка содержимого текстовых полей для ввода координат фигуры
        public override void ClearTextCoords()
        {
            RemoveAllTextFieldFromParamPanel();                                                    //  Удаляем все лишние поля(свыше 3-х) с панели ввода текстовой инфы
            if (_List_tBox != null)                                                                 //  Если список с полями-боксами есть, то
            {
                for (int i = 2; i > -1; i--)                                                        //  Очищаем в оставшихся после удаления текст-боксах их содержимое
                {
                    _List_tBox[i].Text = "";
                }
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

            for (int i = 0; i < visualControler.ControlPoints.Count; i++)             //  Зададим для всех контролов значение всплывающих подказок
                ((Ellipse)visualControler.ControlPoints[i]).ToolTip = new_name;
        }

        #endregion

        //  Добавление новой вершины в фигуру
        public override void AddGeometryPoint(Point point)
        {
            paramNames.Add("Point" + (GeometricParams.Count + 1).ToString());                   //  Добавим название параметра в список с названиями параметров
            paramTypes.Add("Point");                                                            //  Добавим название типа параметра в список с названиями типов параметров

            namedParameter param = new namedParameter();
            param.ParamName = paramNames[GeometricParams.Count];
            param.ParamValue = point;

            GeometricParams.Add(param);
        }

        //  Добавление текстового поля ввода координат геометрической фигуры в случае отсутсвия нужного кол-ва(поли-фигуры)
        public override void AddTextFieldToParamPanel()
        {
            for (int i = 0; i < _Editor.ParamPanels.Children.Count; i++)                        //  Перебираем весь список панелей с текстовыми полями для задания параметров фигур
            {
                if (((Panel)_Editor.ParamPanels.Children[i]).Name == "PolyLinePanel")           //      Если эта пенель для полиЛинии, то
                {
                    TextBox tbox = _Editor.AddTextFieldToParamPanel((Canvas)_Editor.ParamPanels.Children[i]);

                    if (tbox != null)
                        _List_tBox.Add(tbox);

                    break;
                }
            }
        }

        //  Удаление текстового поля ввода координат по его индексу
        public override void RemoveTextFieldFromParamPanelAt(int tbox_idx)
        {
            for (int i = 0; i < _Editor.ParamPanels.Children.Count; i++)                        //  Перебираем весь список панелей с текстовыми полями для задания параметров фигур
            {
                if (((Panel)_Editor.ParamPanels.Children[i]).Name == "PolyLinePanel")
                {
                    int elem_idx = -1;
                    int t_box_count = 0;
                    int elem_count = 0;
                    foreach (object obj in ((Panel)_Editor.ParamPanels.Children[i]).Children)
                    {
                        if (obj is TextBox)
                        {
                            if (t_box_count == tbox_idx)
                                elem_idx = elem_count;
                            t_box_count++;
                        }
                        elem_count++;
                    }

                    if (elem_idx != -1)
                    {
                        foreach (object obj in ((Panel)_Editor.ParamPanels.Children[i]).Children)
                        {
                            if (obj is Button)
                            {
                                Canvas.SetTop((Button)obj, Canvas.GetTop((Button)obj) - 20);
                                if ("- Point".CompareTo(((Button)obj).Content.ToString()) == 0)
                                    if (((Panel)_Editor.ParamPanels.Children[i]).Children.Count < 13)
                                        ((Button)obj).Opacity = 0.4;
                                    else
                                        ((Button)obj).Opacity = 1;
                            }
                        }

                        ((Panel)_Editor.ParamPanels.Children[i]).Children.RemoveRange(elem_idx - 1, 2);

                        _List_tBox.RemoveAt(tbox_idx);
                    }

                    break;
                }
            }
        }

        //  Удаление последнего текстового поля ввода координат(поли-фигуры)
        public override void RemoveLastTextFieldFromParamPanel()
        {
            if (_List_tBox.Count < 4)
            {
                MessageBox.Show("A PolyLine must have at least three points(vertices) !", "Attention!");
                return;
            }
            else
            {
                RemoveTextFieldFromParamPanelAt(_List_tBox.Count - 1);
            }
        }


        //  Удаление текстового поля ввода координат по его индексу
        public void RemoveAllTextFieldFromParamPanel()
        {
            for (int i = 0; i < _Editor.ParamPanels.Children.Count; i++)                        //  Перебираем весь список панелей с текстовыми полями для задания параметров фигур
            {
                if (((Panel)_Editor.ParamPanels.Children[i]).Name == "PolyLinePanel")
                {
                    int tbox_count = 0;
                    int elem_count = 0;
                    foreach (object obj in ((Panel)_Editor.ParamPanels.Children[i]).Children)
                    {
                        if (obj is TextBox)
                            tbox_count++;

                        elem_count++;
                    }

                    int del_count = tbox_count - 3;
                    if (del_count > 0)
                    {
                        foreach (object obj in ((Panel)_Editor.ParamPanels.Children[i]).Children)
                        {
                            if (obj is Button)
                            {
                                Canvas.SetTop((Button)obj, Canvas.GetTop((Button)obj) - 20 * del_count);
                                if ("- Point".CompareTo(((Button)obj).Content.ToString()) == 0)
                                    if (((Panel)_Editor.ParamPanels.Children[i]).Children.Count < 13)
                                        ((Button)obj).Opacity = 0.4;
                                    else
                                        ((Button)obj).Opacity = 1;
                            }
                        }
                        ((Panel)_Editor.ParamPanels.Children[i]).Children.RemoveRange(8, 2 * del_count);
                    }
                    if (_List_tBox != null && _List_tBox.Count > 3)
                        _List_tBox.RemoveRange(3, _List_tBox.Count - 3);

                    break;
                }
            }
        }

        //--- Проверка самопересечений сторон поли-фигуры
        public override bool IsSelfIntersection(bool is_mess=true)
        {
            Point intersectionPoint = new Point();

            if (GeometricParams.Count > 2)
            {
                for (int i = 1; i < GeometricParams.Count - 1; i++)
                {
                    for (int j = i + 1; j < GeometricParams.Count; j++)
                    {
                        if (GeometricFunction.IntersectionTwoLineSegments((Point)GeometricParams[i - 1].ParamValue, (Point)GeometricParams[i].ParamValue,
                                                    (Point)GeometricParams[j - 1].ParamValue, (Point)GeometricParams[j].ParamValue, ref intersectionPoint))
                        {
                            if (is_mess) MessageBox.Show("The construction of the figure can not be completed because of the self-intersection of the sides!");
                            return (true);
                        }
                    }
                }
            }
            return (false);
        }
    }
}