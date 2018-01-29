using System;
using System.Windows;
using System.ComponentModel;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

namespace PolygonEditor
{
    public delegate void FuncDelegate(string controlName, Point movingEndLocation);
    
    public class VisualControler
    {
        public EditorModel _Editor;                                     //  Редактор в котором обрабатывается фигура
        public ShapeBase _Shape;                                        //  Фигура к которой прикреплен экземпляр контроллера
        public /*Path*/Shape _Path;                                              //  Общая геометрия используемая для визуализации фигуры
        public Canvas PaneControl;                                      //  Канва на которой размещаются точки контроля фигуры
        public Canvas PaneEditor;                                       //  Канва на которой размещаются точки контроля фигуры
        public ArrayList ControlPoints = new ArrayList();               //  Список визуальных контролов для фигуры
        public ArrayList MarkerPoints = new ArrayList();                //  Список визуальных маркеров "выбора" фигуры

        public bool BeginSelectPath;                                    //  Флаг начала процесса выбора фигуры
        public bool BeginUnselectPath;                                  //  Флаг начала процесса отмены выбора фигуры
        public bool IsSelectedPath;                                     //  Флаг - выбрана или нет фигура для каких либо действий

        private static double ControlPointSize = 13;                    //  Размер визуальных контролов
        private static Brush ControlPointColor = Brushes.Blue;          //  Цвет обводки контрола
        private static double MarkerPointSize = 8;                      //  Размер визуальных маркеров "выбора" фигуры
        private static Brush MarkerPointColor = Brushes.Red;            //  Цвет обводки визуальных маркеров "выбора" фигуры

        private bool IsPressedPath;                                     //  Флаг состояния - над фигурой нажата левая кнопка мыши
        private Point PressedPathLocation;                              //  Координаты точки панели редактора, где была нажата левая кнопка мыши

        private bool IsPressedControl;                                  //  Флаг состояния - на контоле нажата левая кнопка мыши
        private int PressedControlIndex;                                //  Индекс контрола на который произвелось нажатие левой кнопки мыши

        public bool IsMouseUnderControl;                                //  Флаг - курсор мыши над контролом

        public bool IsMouseDraged;                                      //  Флаг - фигура или контрол находятся в режиме перетаскивания

        public Line HelpLine;                                           //  Штриховая линия-помошник при построении фигур
        public bool InitNewPoint;                                       //  Флаг - начато построение новой точки для следующей линии
        public int DrawPointCount;                                      //  Количество "построенных" точек в режиме рисования мышью

        public Point intersectionPoint;                                 //  Координаты точка возможного самопересечения сложных фигур
        public Ellipse SplitPoint;                                      //  Точка пересечения хелп-линии с со сторонами фигуры

        public Point temp_point;                                        //  Точка для временного хранения координат при проверках возможных самопересечений сторон фигуры

        private FuncDelegate UpdateGeometries;                          //  Функция перерисовки фигуры при перетаскивании контрола


        public VisualControler(EditorModel editor, ShapeBase shape, int controlsCount)
        {
            _Editor= editor;                                            //  Редактор в котором работает контроллер
            _Shape= shape;                                              //  Фигура к которой прикреплен конторллер
            _Path = shape.shapeBase;                                    //  Контейнер для геометрий визуально изображающих фигуру
            PaneControl = editor.ControlPanel;                          //  Панель для изображения вспомогательных элементов для управления фигурой
            PaneEditor = editor.MainPanel;                              //  Пенель контейнер для всех панелей редактора
            
            BeginSelectPath = false;                                    //  установим флаг - процесс выбора фигуры не начат
            IsSelectedPath = false;                                     //  установим флаг - фигура не выбрана(маркеры неподсвечены)
            IsPressedControl = false;                                   //  уснановим флаг - на контролы нажатия не производились
            IsMouseUnderControl = false;                                //  установим флаг - курсор мыши пока не над контролом
            
            UpdateGeometries = shape.UpdateShapeGeometryByControl;      //  присвоим делегата по перерисовке геометрии при изменении расположения контрола
            
            _Shape.ControlPointsCount = controlsCount;                  //  зададим какое количество контролов есть у фигуры

            IsMouseDraged = false;

            for (int i = 0; i < controlsCount; i++)                     //  переберем все точки на которых должны располагаться контролы
            {
                // создаем визуальный маркер выбора фигуры
                Rectangle rec = new Rectangle();                                
                rec.Visibility = Visibility.Hidden;                                                 //  В момент создания маркер невидим
                rec.Stroke = MarkerPointColor;                                                      //  Цвет обводки синий
                rec.StrokeThickness = 1;                                                            //  Толщина обводки
                RenderOptions.SetEdgeMode(rec, EdgeMode.Aliased);                                   //  Размывания изображения - нет
                rec.Width = MarkerPointSize;                                                        //  Высота зоны захвата мышью 
                rec.Height = MarkerPointSize;                                                       //  Ширина зоны захвата мышью 
                Canvas.SetLeft(rec, ((Point)shape.GeometricParams[i].ParamValue).X - rec.Width / 2);//  Задаем место размещения зоны маркера на канве по X
                Canvas.SetTop(rec, ((Point)shape.GeometricParams[i].ParamValue).Y - rec.Height / 2);//  Задаем место размещения зоны маркера на канве по Y
                MarkerPoints.Add(rec);                                                              //  заносим маркер в список маркеров фигуры
                PaneControl.Children.Add(rec);                                                      //  добавляем маркер на панель контролов

                // создаем визуальную точку-контрол
                Ellipse el = new Ellipse();                                                         //  За прототип точки возьмем эллипс
                el.Stroke = Brushes.Transparent;                                                    //  В момент создания точка-контрол невидима - Цвет обводки прозрачный
                el.StrokeThickness = 1;                                                             //  Толщина обводки 
                //RenderOptions.SetEdgeMode(el, EdgeMode.Aliased);
                el.Fill = Brushes.Transparent;                                                      //  Заливка контрола прозрачная
                el.Width = ControlPointSize;                                                        //  Высота зоны захвата мышью 
                el.Height = ControlPointSize;                                                       //  Ширина зоны захвата мышью 
                el.ToolTip = _Shape.shapeBase.ToolTip;                                              //  Всплывающая подсказка с инфой о фигуре-хозяине

                ContextMenu context_menu = new ContextMenu();                                       //  На элементе установим для фигуры контекстное меню
                context_menu.Items.Add(new MenuItem() { Header = "RenameShape", Command = new ShapeContextMenuViewModel(_Editor,_Shape).ShapeRename });
                context_menu.Items.Add(new MenuItem() { Header = "DeleteShape", Command = new ShapeContextMenuViewModel(_Editor, _Shape).ShapeDelete });
                el.ContextMenu = context_menu;

                Canvas.SetLeft(el, ((Point)shape.GeometricParams[i].ParamValue).X - el.Width / 2);  //  Задаем место размещения на канве по X
                Canvas.SetTop(el, ((Point)shape.GeometricParams[i].ParamValue).Y - el.Height / 2);  //  Задаем место размещения на канве по Y
                ControlPoints.Add(el);                                                              //  заносим контрол в список контролов фигуры
                PaneControl.Children.Add(el);                                                       //  добавляем контрол на панель контролов
            
                // добавляем обработчики событий мыши для контрола
                el.MouseEnter += new MouseEventHandler(Control_MouseEnter);
                el.MouseLeave += new MouseEventHandler(Control_MouseLeave);
                el.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
                el.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            }


                // добавляем обработчики событий мыши для фигуры
                _Path.MouseEnter += new MouseEventHandler(path_MouseEnter);
                _Path.MouseLeave += new MouseEventHandler(path_MouseLeave);
                _Path.MouseLeftButtonDown += new MouseButtonEventHandler(path_MouseLeftButtonDown);
                _Path.MouseLeftButtonUp += new MouseButtonEventHandler(path_MouseLeftButtonUp);

                // добавляем обработчики событий мыши для панели редактора
                PaneEditor.MouseEnter += new MouseEventHandler(Pane_MouseEnter);
                PaneEditor.MouseMove += new MouseEventHandler(Pane_MouseMove);
                PaneEditor.MouseLeftButtonDown += new MouseButtonEventHandler(Pane_MouseLeftButtonDown);
                PaneEditor.MouseLeftButtonUp += new MouseButtonEventHandler(Pane_MouseLeftButtonUp);

                // добавляем обработчик событий по изменению свойств фигуры в редакторе
                editor.PropertyChanged += new PropertyChangedEventHandler(editor_PropertyChanged);

                temp_point = new Point();


                if (_Editor.IsMouseDraws)
                {
                    HelpLine = new Line();
                    HelpLine.X1 = HelpLine.X2 = ((Point)shape.GeometricParams[0].ParamValue).X;
                    HelpLine.Y1 = HelpLine.Y2 = ((Point)shape.GeometricParams[0].ParamValue).Y;
                    HelpLine.StrokeThickness = _Shape.shapeBase.StrokeThickness;
                    //RenderOptions.SetEdgeMode(HelpLine, EdgeMode.Aliased);
                    HelpLine.Stroke = _Shape.shapeBase.Stroke;
                    HelpLine.StrokeDashArray = new DoubleCollection();
                    HelpLine.StrokeDashArray.Add(5);
                    HelpLine.StrokeDashArray.Add(3);
                    PaneControl.Children.Add(HelpLine);
                    InitNewPoint = false;
                    DrawPointCount = 1;
                    editor.PropertyChanged += new PropertyChangedEventHandler(Pane_KeyDownESC);
                    if (_Shape.shapeType == SHAPE_TYPE.PolyLine || _Shape.shapeType == SHAPE_TYPE.Polygon)
                        editor.PropertyChanged += new PropertyChangedEventHandler(Pane_KeyDownEnd);

                    intersectionPoint = new Point();
                    // создаем визуальную точку возможного самопересечения
                    SplitPoint = new Ellipse();                                                 //  За прототип точки возьмем эллипс
                    SplitPoint.Stroke = Brushes.Red;                                                    //  В момент создания точка-контрол невидима - Цвет обводки прозрачный
                    SplitPoint.Fill = Brushes.Red;                                                      //  Заливка контрола прозрачная
                    SplitPoint.StrokeThickness = 8;                                                     //  Толщина обводки
                    SplitPoint.Width = 8;                                                               //  Толщина обводки
                    SplitPoint.Height = 8;                                                              //  Толщина обводки
                    SplitPoint.Visibility = Visibility.Collapsed;                                       //  Пока не видима
                    PaneControl.Children.Add(SplitPoint);
                }
        }

        
        //--- Обработчик события: курсор мыши вошел в область над фигурой
        void path_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_Editor.IsMouseDraws)                                                          // Если в данный момент не происходит рисования фигуры мышью
            {
                if (_Editor.EditorMode != EDITOR_MODE.DrawNew &&                                //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                    (e.LeftButton == MouseButtonState.Released || IsMouseDraged) )
                {
                    if (!IsMouseUnderControl)                                                   //      Если флаг "курсор над контролом" не установлен
                    {
                        IsMouseUnderControl = true;                                             //          устанавливаем его
                        foreach (object control in ControlPoints)                               //          и "подсвечиваем" все контролы фигуры
                            ((Ellipse)control).Stroke = ControlPointColor;

                        if (_Editor.EditorMode == EDITOR_MODE.Selection ||                      //      Если Редактор не находится в одном из след.режимeов
                            _Editor.EditorMode == EDITOR_MODE.PolygonOperations ||
                            (_Editor.EditorMode == EDITOR_MODE.Edit && IsSelectedPath) )
                        {
                            _Editor.ShapeType = _Shape.shapeType;                               //          устанавливаем в Редакторе тип фигуры над которой находится курсор
                            _Shape.ClearTextCoords();                                           //          стираем на панели координат поля с кординатами предыдущей фигуры
                            _Shape.UpdateTextCoords();                                          //          на пенели координат "рисуем" поля с координатами фигуры-владельца контролера
                        }
                    }
                }
            }
        }

        //--- Обработчик события: курсор мыши покинул область над фигурой
        void path_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_Editor.IsMouseDraws)                                                          // Если в данный момент не происходит рисования фигуры мышью
            {
                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)                                  //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                {
                    if(!IsMouseDraged)                                                          //      Если нет перетаскивания мышью фигуры или контрола
                    {
                        IsMouseUnderControl = false;                                            //          установим - курсор мыши не над контролом
                        foreach (object control in ControlPoints)                               //          убираем "подсветку" контролов
                            ((Ellipse)control).Stroke = Brushes.Transparent;
                    }
                }
                if (_Editor.EditorMode == EDITOR_MODE.Selection ||                      //  Если Редактор находится в режимe Выбор или ОперацииНадПолигонами,
                    _Editor.EditorMode == EDITOR_MODE.PolygonOperations)
                {
                    _Editor.ShapeType = _Shape.shapeType;                               //      устанавливаем в Редакторе тип фигуры над которой находится курсор
                    _Shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                }
            }
        }

        //--- Обработчик события: нажата вниз левая кнопка мыши, когда курсор мыши над фигурой
        void path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_Editor.IsMouseDraws)                                                          // Если в данный момент не происходит рисования фигуры мышью
            {
                if (_Editor.EditorMode == EDITOR_MODE.PolygonOperations && _Editor.SetClippersOn)//  Если Редактор находится в режимe ПолигонОперации и включена установка Клипперов
                {
                    if (((ShapePolygon)_Shape).opObjType != OPERATION_OBJECT_TYPE.Result)
                        _Editor.shapesManager.ResetAsClipper(_Shape);                           //      Зададим или уберем у фигуры параметры Клиппера
                }
                else if (_Editor.EditorMode != EDITOR_MODE.DrawNew)                             //  Если Редактор находится в любом из оставшихся режимов, кроме рисования новой фигуры
                {
                    if (!IsSelectedPath)                                                        //      Если фигура "не выбрана", то 
                    {
                        BeginSelectPath = true;                                                 //          начинаем процесс выбора
                    }
                    else                                                                        //      Иначе(если фигура уже "выбрана"), то 
                    {
                        BeginUnselectPath = true;                                               //          начинаем процесс отмены выбора
                    }
                }
            }

            IsPressedPath = true;                                                               //  Фиксируем нажатие на фигуру
            PressedPathLocation = e.GetPosition(PaneEditor);                                    //  и запоминаем координаты этого нажатия
        }

        //--- Обработчик события: отжата вверх левая кнопка мыши, когда курсор мыши над фигурой
        void path_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseDraged = false;

            if (!_Editor.IsMouseDraws)                                                          // Если в данный момент не происходит рисования фигуры мышью
            {
                IsPressedPath = false;                                                          //  Нажатия на фигуру - нет

                if (_Editor.EditorMode == EDITOR_MODE.Edit ||                                   //  Если Редактор находится в режимe "Редактирование" или
                    _Editor.EditorMode == EDITOR_MODE.Selection)                                //  режиме "Выбор"
                {
                    if (!IsSelectedPath)                                                        //      Если фигура "не выбрана"
                    {
                        if (BeginSelectPath)                                                    //          Если начат процесс выбора
                        {
                            IsSelectedPath = true;                                              //              Фигура - "выбрана"
                            BeginSelectPath = false;                                            //              Завершаем процесс выбора
                            if (_Editor.EditorMode == EDITOR_MODE.Edit)                         //              Если у редактора режим "Редактирование"
                            {
                                _Editor.shapesManager.ReselectShapeForEdit(_Shape);             //                  меняем "редактируемую" фигуру на текущую
                            }
                            if (_Editor.EditorMode == EDITOR_MODE.Selection)                    //              Если у редактора режим "Выбор"
                            {
                                _Editor.shapesManager.SelectShape(_Shape);                      //                  помечаем фигуру как "выбранную"
                            }
                        }
                    }
                    else                                                                        //      Иначе(если фигура "выбрана")
                    {
                        if (BeginUnselectPath)                                                  //          Если начат процесс отмены выбора
                        {
                            IsSelectedPath = false;                                             //              Фигура - "не выбрана"
                            BeginUnselectPath = false;                                          //              процесс отмены выбора завершаем
                            _Editor.shapesManager.UnselectShape(_Shape);                        //              отменяем пометку выбранности у фигуры
                        }
                    }
                }    

                if (_Editor.EditorMode == EDITOR_MODE.Selection ||                      //  Если Редактор находится в режимe Выбор или ОперацииНадПолигонами,
                    _Editor.EditorMode == EDITOR_MODE.PolygonOperations )                 
                {
                    _Editor.ShapeType = _Shape.shapeType;                               //      устанавливаем в Редакторе тип фигуры над которой находится курсор
                    _Shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                    _Shape.UpdateTextCoords();                                          //      на пенели координат "рисуем" поля с координатами фигуры-владельца контролера
                }
            }
        }



        //--- Обработчик события: курсор мыши зашел в область над контролом
        void Control_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_Editor.IsMouseDraws)                                                          // Если в данный момент не происходит рисования фигуры мышью
            {
                if (_Editor.EditorMode != EDITOR_MODE.DrawNew &&                                //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                    (e.LeftButton == MouseButtonState.Released /*|| IsMouseDraged*/))           //      и левая кнопка мыши отпущена
                {
                    IsMouseUnderControl = true;                                                 //      Запоминаем что курсор над контролом
                    if (_Editor.EditorMode == EDITOR_MODE.Edit && IsSelectedPath/*_Shape.SelectedForEdit*/)       //      Если Редактор находится в режимe Редактирование и данная фигура выбрана,
                    {
                        foreach (object control in ControlPoints)                               //          "посвечиваем" контрол над которым курсор
                            if (control.GetHashCode() == sender.GetHashCode())
                                ((Ellipse)control).Stroke = ControlPointColor;
                    }
                    else {
                        foreach (object control in ControlPoints)                               //          "подсвечиваем" все контролы фигуры
                            ((Ellipse)control).Stroke = ControlPointColor;
                    }
                    
                    if (_Editor.EditorMode == EDITOR_MODE.Selection ||                          //      Если Редактор не находится в одном из след.режимeов
                             _Editor.EditorMode == EDITOR_MODE.PolygonOperations ||
                             (_Editor.EditorMode == EDITOR_MODE.Edit && IsSelectedPath))
                    {

                        _Editor.ShapeType = _Shape.shapeType;                                   //      "выбираем" в редакторе тип данной фигуры
                        _Shape.ClearTextCoords();                                               //      очищаем панель ручного ввода параметров
                        _Shape.UpdateTextCoords();                                              //      заполняем эту панель праметрами текущей фигуры
                    }
                }
            }
        }

        //--- Обработчик события: курсор мыши покинул область над контролом
        void Control_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_Editor.IsMouseDraws)                                                          // Если в данный момент не происходит рисования фигуры мышью
            {
                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)                                  //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                {
                    if (!IsMouseDraged)
                    {
                        IsMouseUnderControl = false;                                            //          курсор мыши не над контролом
                        foreach (object control in ControlPoints)                               //          убираем подсветку контролов
                            ((Ellipse)control).Stroke = Brushes.Transparent;
                    }
                }
                if (_Editor.EditorMode == EDITOR_MODE.Selection ||                      //  Если Редактор находится в режимe Выбор или ОперацииНадПолигонами,
                    _Editor.EditorMode == EDITOR_MODE.PolygonOperations)
                {
                    _Editor.ShapeType = _Shape.shapeType;                               //      устанавливаем в Редакторе тип фигуры над которой находится курсор
                    _Shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                }
            }
        }

        //--- Обработчик события: нажатие левой кнопки мыши, когда курсор мыши над контролом
        void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_Editor.EditorMode == EDITOR_MODE.PolygonOperations)                            //  Если Редактор находится в режимe ПолигонОперации 
            {
                if (_Editor.SetClippersOn)                                                      //      Если включена установка Клипперов
                    if( ((ShapePolygon)_Shape).opObjType != OPERATION_OBJECT_TYPE.Result)
                        _Editor.shapesManager.ResetAsClipper(_Shape);                           //          зададим или уберем у фигуры параметры Клиппера
                return;
            }

            if (!_Editor.IsMouseDraws)                                                          // Если в данный момент не происходит рисования фигуры мышью
            {
                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)                                  //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                {
                    if (!IsSelectedPath)                                                        //      Если фигура "не выбрана", то 
                    {
                        BeginSelectPath = true;                                                 //          начинаем процесс выбора
                    }
                    else                                                                        //      Иначе(если фигура уже "выбрана"), то 
                    {
                        BeginUnselectPath = true;                                               //          начинаем процесс отмены выбора
                    }
                }

                int i = 0;
                for (; i < ControlPoints.Count; i++)                                            //  Ищем контрол на котором произведено нажатие в списке контролов фигуры
                    if (ControlPoints[i].GetHashCode() == sender.GetHashCode())
                    {
                        PressedControlIndex = i;                                                //      и запоминаем его индекс в списке
                        break;
                    }

                if (_Editor.EditorMode == EDITOR_MODE.Edit)                                     //  Если включен режим "Редактирование"
                {
                    if( IsSelectedPath )                                                        //      Если фигура выбрана для редактирования
                        IsPressedControl = true;                                                //          Фиксируем нажатие на контрол
                }
                else if (_Editor.EditorMode == EDITOR_MODE.Selection)                           //  Если режим "Выбор"
                {
                    IsPressedPath = true;                                                       //      Фиксируем нажатие на фигуру
                    PressedPathLocation.X = Canvas.GetLeft((Ellipse)ControlPoints[i]) + ((Ellipse)ControlPoints[i]).Width / 2; // запоминаем координаты центра контрола на который было нажатие
                    PressedPathLocation.Y = Canvas.GetTop((Ellipse)ControlPoints[i]) + ((Ellipse)ControlPoints[i]).Height / 2;
                }
            }
        }

        //--- Обработчик события: отжата вверх левая кнопка мыши, когда курсор мыши над контролом
        void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_Editor.EditorMode == EDITOR_MODE.PolygonOperations)                            //  Если Редактор находится в режимe ПолигонОперации
            {
                return;                                                                         //      выходим
            }

            IsMouseDraged = false;

            if (!_Editor.IsMouseDraws)                                                          // Если в данный момент не происходит рисования фигуры мышью
            {
                IsPressedControl = false;                                                       //  Нажатия на контрол - нет
                IsPressedPath = false;                                                          //  Нажатие на фигуру - нет

                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)                                  //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                {
                    if (!IsSelectedPath)                                                        //      Если фигура "не выбрана"
                    {
                        if (BeginSelectPath)                                                    //          Если начат процесс выбора
                        {
                            IsSelectedPath = true;                                              //              Фигура - "выбрана"
                            BeginSelectPath = false;                                            //              Завершаем процесс выбора
                            if (_Editor.EditorMode == EDITOR_MODE.Edit)                         //              Если у редактора режим "Редактирование"
                            {
                                _Editor.shapesManager.ReselectShapeForEdit(_Shape);
                            }
                            if (_Editor.EditorMode == EDITOR_MODE.Selection)
                            {
                                _Editor.shapesManager.SelectShape(_Shape);
                            }
                        }
                    }
                    else //IsSelectedPath                                                       //      Иначе(если фигура "выбрана")
                    {
                        if (BeginUnselectPath)                                                  //          Если начат процесс отмены выбора
                        {
                            IsSelectedPath = false;                                             //              Фигура - "не выбрана"
                            BeginUnselectPath = false;                                          //              процесс отмены выбора завершаем
                            _Editor.shapesManager.UnselectShape(_Shape);
                        }
                    }
                    if (_Editor.EditorMode == EDITOR_MODE.Selection)
                    {
                        _Editor.ShapeType = _Shape.shapeType;
                        _Shape.ClearTextCoords();
                        _Shape.UpdateTextCoords();
                    }
                }
            }
        }

        //--- Обработчик события: передвежение курсора мыши по полю контрола
        void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Editor.EditorMode == EDITOR_MODE.PolygonOperations)                            //  Если Редактор находится в режимe ПолигонОперации
            {
                return;                                                                         //      выходим
            }

            if (!_Editor.IsMouseDraws)                                                          // Если в данный момент не происходит рисования фигуры мышью
            {
                BeginSelectPath = false;                                                        //  Анулируем ранее начатые процессы "выбора" или "отмены выбора" фигуры
                BeginUnselectPath = false;

                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)                                  //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                {
                    if (IsPressedControl && IsSelectedPath)                                     //      Если над контролом нажата левая кнопка мыши 
                    {
                        if( _Editor.EditorMode == EDITOR_MODE.Edit )                            //          Если включен режим "Редактора"
                        {
                            IsMouseDraged = true;                                               //              фиксируем процесс перемещения контрола мышью 

                            Point movingEndLocation = e.GetPosition(PaneEditor);                //              узнаем координаты курсора на панели Редактора

                            if (_Editor.prop_service.properties.SelfIntersectionCheck &&        //              Если необходимо контролировать самопересечения и фигура
                                 (_Shape.shapeType == SHAPE_TYPE.Polygon || _Shape.shapeType == SHAPE_TYPE.PolyLine))// полигон или полиЛиния
                            {
                                temp_point = (Point)_Shape.GeometricParams[PressedControlIndex].ParamValue;//       запоминаем координаты точки, где располагался перемещаемый контрол

                                namedParameter param = new namedParameter();                    //                  устанавливаем у фигуры координаты по новому местоположению контрола
                                param.ParamName = _Shape.paramNames[PressedControlIndex];
                                param.ParamValue = movingEndLocation;

                                _Shape.GeometricParams.RemoveRange(PressedControlIndex,1);
                                _Shape.GeometricParams.Insert(PressedControlIndex, param);

                                if (_Shape.IsSelfIntersection(false))                           //                  Если есть самопересечение при новом положении контрола, то 
                                {                                                               //                      восстанавливаем "старое" положение
                                    param.ParamValue = temp_point;
                                    _Shape.GeometricParams.RemoveRange(PressedControlIndex, 1);
                                    _Shape.GeometricParams.Insert(PressedControlIndex, param);
                                    movingEndLocation = temp_point;
                                    //return;
                                }
                            }

                            UpdateControlsCoords(movingEndLocation);                             //          перерисовываем "передвинутый" контрол
                            UpdateGeometries(((Ellipse)ControlPoints[PressedControlIndex]).Name, movingEndLocation);// перерисовываем "передвинутую" вершину фигуры
                        }
                    }
                }
            }
        }



        //--- Обработчик события: левая кнопки мыши нажата над панелью редактора
        public void Pane_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_Editor.IsMouseDraws)                                                          // Если в данный момент Происходит рисование фигуры мышью
                if ( _Editor.DrawShape.GetHashCode() == _Shape.GetHashCode() )
                {
                    InitNewPoint = true;
                }
        }

        //--- Обработчик события: левая кнопки мыши отпущена над панелью редактора
        public void Pane_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseDraged = false;

            if (_Editor.IsMouseDraws)                                                          // Если в данный момент Происходит рисование фигуры мышью
            {
                if ( _Editor.DrawShape.GetHashCode() == _Shape.GetHashCode()  && InitNewPoint )
                {
                    InitNewPoint = false;

                    if (_Editor.prop_service.properties.SelfIntersectionCheck )
                        if (_Shape.shapeType == SHAPE_TYPE.PolyLine || _Shape.shapeType == SHAPE_TYPE.Polygon)
                        {
                            _Shape.AddGeometryPoint(new Point(HelpLine.X2, HelpLine.Y2));
                            if (IsSelfIntersection())
                            {
                                _Shape.GeometricParams.RemoveAt(_Shape.GeometricParams.Count - 1);
                                return;
                            }
                            _Shape.GeometricParams.RemoveAt(_Shape.GeometricParams.Count - 1);
                        }

                    Point endLocation = e.GetPosition(PaneEditor);

                    namedParameter param = new namedParameter();
                    param.ParamName = _Shape.GeometricParams[DrawPointCount].ParamName;
                    param.ParamValue = endLocation;
                    _Shape.GeometricParams.RemoveAt(DrawPointCount);
                    _Shape.GeometricParams.Insert(DrawPointCount, param);

                    DrawPointCount++;

                    switch (_Editor.ShapeType)
                    {
                        case SHAPE_TYPE.Line:
                            if (DrawPointCount == 2)
                            {
                                _Editor.IsMouseDraws = false;
                                
                                PaneControl.Children.Remove(HelpLine);
                                PaneControl.Children.Remove(SplitPoint);

                                _Shape.UpdateTextCoords();
                                _Shape.UpdateShapeGeometryByTextCoords();
                                _Shape.ClearTextCoords();
                            }
                            break;
                        
                        case SHAPE_TYPE.PolyLine:
                        case SHAPE_TYPE.Polygon:
                                _Shape.AddGeometryPoint(new Point(HelpLine.X2, HelpLine.Y2));
                                HelpLine.X1 = HelpLine.X2;
                                HelpLine.Y1 = HelpLine.Y2;
                                _Shape.UpdateTextCoords();
                                _Shape.UpdateShapeGeometryByTextCoords();
                            break;
                    }
                }
            }
            else
            {
                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)                                  //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                {
                    if (IsSelectedPath)
                    {
                        if (IsPressedControl)
                        {
                            IsPressedControl = false;
                            ((Ellipse)ControlPoints[PressedControlIndex]).Cursor = System.Windows.Input.Cursors.Arrow;

                            Point movingEndLocation = e.GetPosition(PaneEditor);

                            if (_Editor.prop_service.properties.SelfIntersectionCheck &&
                                 (_Shape.shapeType == SHAPE_TYPE.Polygon || _Shape.shapeType == SHAPE_TYPE.PolyLine))
                            {
                                temp_point = (Point)_Shape.GeometricParams[PressedControlIndex].ParamValue;

                                namedParameter param = new namedParameter();
                                param.ParamName = _Shape.paramNames[PressedControlIndex];
                                param.ParamValue = movingEndLocation;

                                _Shape.GeometricParams.RemoveRange(PressedControlIndex, 1);
                                _Shape.GeometricParams.Insert(PressedControlIndex, param);

                                if (_Shape.IsSelfIntersection(false))
                                {
                                    param.ParamValue = temp_point;
                                    _Shape.GeometricParams.RemoveRange(PressedControlIndex, 1);
                                    _Shape.GeometricParams.Insert(PressedControlIndex, param);
                                    movingEndLocation = temp_point;
                                    //return;
                                }
                            }

                            UpdateControlsCoords(movingEndLocation);
                            UpdateGeometries(((Ellipse)ControlPoints[PressedControlIndex]).Name, movingEndLocation);
                        }
                        if (IsPressedPath)
                        {
                            IsPressedPath = false;

                            Point current_point = e.GetPosition(PaneEditor);
                            /*Vector locationChange = new Vector();
                            locationChange.X = current_point.X - PressedPathLocation.X;
                            locationChange.Y = current_point.Y - PressedPathLocation.Y;

                            _Shape.UpdateShapeGeometryByVector(locationChange);*/
                            _Shape.UpdateShapeGeometryByVector(current_point - PressedPathLocation);
                        }
                    }
                }
            }
        }

        //--- Обработчик события: передвежение курсора мыши над панелью редактора
        public void Pane_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Editor.IsMouseDraws)                                                          // Если в данный момент Происходит рисование фигуры мышью
            {
                if (_Editor.DrawShape.GetHashCode() == _Shape.GetHashCode())
                {
                    Point movingEndLocation = e.GetPosition(PaneEditor);

                    HelpLine.X2 = movingEndLocation.X;
                    HelpLine.Y2 = movingEndLocation.Y;

                    _Shape._List_tBox[DrawPointCount].Text = movingEndLocation.X.ToString() + " " + movingEndLocation.Y.ToString();

                    InitNewPoint = false;
                    
                    if(_Editor.prop_service.properties.SelfIntersectionCheck)
                        CheckIntersectWithHelpLine();
                }
            }
            else
            {
                BeginSelectPath = false;
                BeginUnselectPath = false;

                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)                        //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                {
                    if (IsSelectedPath)
                    {
                        if (IsPressedControl)
                        {
                            IsMouseDraged = true;

                            Point movingEndLocation = e.GetPosition(PaneEditor);

                            if (_Editor.prop_service.properties.SelfIntersectionCheck && 
                                 (_Shape.shapeType == SHAPE_TYPE.Polygon || _Shape.shapeType == SHAPE_TYPE.PolyLine))
                            {
                                temp_point = (Point)_Shape.GeometricParams[PressedControlIndex].ParamValue;

                                namedParameter param = new namedParameter();
                                param.ParamName = _Shape.paramNames[PressedControlIndex];
                                param.ParamValue = movingEndLocation;

                                _Shape.GeometricParams.RemoveRange(PressedControlIndex,1); 
                                _Shape.GeometricParams.Insert(PressedControlIndex, param);

                                if (_Shape.IsSelfIntersection(false))
                                {
                                    param.ParamValue = temp_point;
                                    _Shape.GeometricParams.RemoveRange(PressedControlIndex,1);
                                    _Shape.GeometricParams.Insert(PressedControlIndex, param);
                                    movingEndLocation = temp_point;
                                    //return;
                                }
                            }

                            UpdateControlsCoords(movingEndLocation);
                            UpdateGeometries(((Ellipse)ControlPoints[PressedControlIndex]).Name, movingEndLocation);
                        }
                        if (IsPressedPath)
                        {
                            IsMouseDraged = true;

                            Point current_point = e.GetPosition(PaneEditor);

                            _Shape.UpdateShapeGeometryByVector(current_point - PressedPathLocation);
                            
                            PressedPathLocation = current_point;
                        }
                    }
                }
            }
        }

        //--- Обработчик события: вход курсора мыши на панель редактора
        public void Pane_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((IsPressedControl || IsPressedPath ) && e.LeftButton == MouseButtonState.Released)
            {
                IsMouseDraged = false;
                IsPressedControl = false;
                IsPressedPath = false;
                IsMouseUnderControl = false;
            }

            if (_Editor.IsMouseDraws)
            {
            }
            else
            {
                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)                                  //  Если Редактор находится в любом из режимов, кроме рисования новой фигуры
                {
                    if (IsPressedControl)                                                       //      Если был контрол в режиме перетаскивания
                    {
                        if (e.LeftButton == MouseButtonState.Released)                          //          Если левая кнопка мыши в отпушена
                        {
                            IsPressedControl = false;                                           //          перетаскивание отменяем
                            ((Ellipse)ControlPoints[PressedControlIndex]).Cursor = System.Windows.Input.Cursors.Arrow; // изображение курсора над контролом меняем на обычный указатель
                        }
                    }
                }
            }
        }




        //--- Обработчик событий изменения в редакторе свойств рисования фигуры
        public void editor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_Editor.EditorMode == EDITOR_MODE.DrawNew && _Editor.IsMouseDraws)
            {
                if (_Editor.DrawShape != null && _Editor.DrawShape.GetHashCode() == _Shape.GetHashCode())
                {
                    _Shape.UpdateShapeGeometryByProperty(e.PropertyName);
                    if (HelpLine != null)
                    {
                        switch (e.PropertyName)
                        {
                            case "ShapeStrokeWidth":
                                HelpLine.StrokeThickness = _Shape.shapeBase.StrokeThickness;
                                break;
                            case "ShapeStrokeColor":
                                HelpLine.Stroke = _Shape.shapeBase.Stroke;
                                break;
                        }
                    }
                }
            }
            if( _Editor.EditorMode == EDITOR_MODE.Edit && IsSelectedPath )
            { 
                _Shape.UpdateShapeGeometryByProperty(e.PropertyName);
            }
        }

        //--- Обработчик событий произошло нажатие на клавиатуре клавиши "Esc"
        public void Pane_KeyDownESC(object sender, PropertyChangedEventArgs e)
        {
            IsMouseDraged = false;

            if (_Editor.IsMouseDraws)                                                          // Если в данный момент Происходит рисование фигуры мышью
                if (_Editor.DrawShape.GetHashCode() == _Shape.GetHashCode())
                {
                    if (e.PropertyName == "IsPressESC" && _Editor.IsPressESC)
                    {
                        _Editor.IsPressESC = false;
                        _Editor.IsMouseDraws = false;
                        _Editor.IsPrompt = false;
                        PaneControl.Children.Remove(HelpLine); 
                        PaneControl.Children.Remove(SplitPoint);
                        _Shape.ClearTextCoords();
                        _Editor.shapesManager.RemoveShape(_Shape);
                    }
                }
        }

        //--- Обработчик событий произошло нажатие на клавиатуре клавиши "End"
        public void Pane_KeyDownEnd(object sender, PropertyChangedEventArgs e)
        {
            IsMouseDraged = false;

            if (_Editor.IsMouseDraws)                                                          // Если в данный момент Происходит рисование фигуры мышью
            {
                if (e.PropertyName == "IsPressEnd" && _Editor.IsPressEnd)
                {
                    if (_Editor.DrawShape.GetHashCode() == _Shape.GetHashCode())
                    {
                        if (_Shape.shapeType == SHAPE_TYPE.PolyLine || _Shape.shapeType == SHAPE_TYPE.Polygon)
                        {
                            _Editor.IsPressEnd = false;

                            if (_Editor.prop_service.properties.SelfIntersectionCheck)
                            {
                                if (_Shape.shapeType == SHAPE_TYPE.Polygon)
                                {
                                    _Shape.AddGeometryPoint(new Point(((Point)_Shape.GeometricParams[0].ParamValue).X, ((Point)_Shape.GeometricParams[0].ParamValue).Y));
                                    if (IsSelfIntersection())
                                    {
                                        _Shape.GeometricParams.RemoveAt(_Shape.GeometricParams.Count - 1);
                                        return;
                                    }
                                    _Shape.GeometricParams.RemoveAt(_Shape.GeometricParams.Count - 1);
                                }
                            }


                            if (_Shape.GeometricParams.Count < 4)
                            {
                                if (MessageBox.Show("A poly-shape must have at least three points(vertices) !\n   Delete the drawing ?", "Attention!", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                {
                                    return;
                                }
                                _Editor.shapesManager.RemoveShape(_Shape);
                            }
                            else
                            {
                                _Shape.RemoveTextFieldFromParamPanelAt(_Shape._List_tBox.Count - 1);
                                if (_Shape.shapeType == SHAPE_TYPE.Polygon)
                                    ((ShapePolygon)_Shape).IsCreated = true;
                                _Shape.UpdateShapeGeometryByTextCoords();
                                AddControls();
                            }

                            _Editor.IsPrompt = false;
                            PaneControl.Children.Remove(HelpLine);
                            PaneControl.Children.Remove(SplitPoint);
                            _Editor.IsMouseDraws = false;
                            _Shape.ClearTextCoords();
                        }
                    }
                }
            }
        }



        //--- Перерисовка контрола и соответствующего ему маркера выбора фигуры
        private void UpdateControlsCoords(Point coords)
        {
            Canvas.SetLeft(((Ellipse)ControlPoints[PressedControlIndex]), coords.X - ((Ellipse)ControlPoints[PressedControlIndex]).Width / 2);
            Canvas.SetTop(((Ellipse)ControlPoints[PressedControlIndex]), coords.Y - ((Ellipse)ControlPoints[PressedControlIndex]).Height / 2);
            Canvas.SetLeft(((Rectangle)MarkerPoints[PressedControlIndex]), coords.X - ((Rectangle)MarkerPoints[PressedControlIndex]).Width / 2);
            Canvas.SetTop(((Rectangle)MarkerPoints[PressedControlIndex]), coords.Y - ((Rectangle)MarkerPoints[PressedControlIndex]).Height / 2);
        }

        //--- Перерисовка всех контролов и маркеров выбора фигуры
        public void UpdateAllControlsCoords()
        {
            for (int i = 0; i < ControlPoints.Count; i++)
            {
                Canvas.SetLeft(((Ellipse)ControlPoints[i]), ((Point)_Shape.GeometricParams[i].ParamValue).X - ((Ellipse)ControlPoints[i]).Width / 2);
                Canvas.SetTop(((Ellipse)ControlPoints[i]), ((Point)_Shape.GeometricParams[i].ParamValue).Y - ((Ellipse)ControlPoints[i]).Height / 2);
                Canvas.SetLeft(((Rectangle)MarkerPoints[i]), ((Point)_Shape.GeometricParams[i].ParamValue).X - ((Rectangle)MarkerPoints[i]).Width / 2);
                Canvas.SetTop(((Rectangle)MarkerPoints[i]), ((Point)_Shape.GeometricParams[i].ParamValue).Y - ((Rectangle)MarkerPoints[i]).Height / 2);
            }
        }

        //--- Добавление к фигуре недостающих контролов
        public void AddControls()
        {
            int i = ControlPoints.Count;
            for (; i < _Shape.GeometricParams.Count; i++)                                           //  переберем все точки на которых должны располагаться контролы
            {
                // создаем визуальный маркер выбора фигуры
                Rectangle rec = new Rectangle();
                rec.Visibility = _Shape.SelectedForEdit ? Visibility.Visible : Visibility.Hidden;  //  "Видимость" маркера
                rec.Stroke = MarkerPointColor;                                                      //  Цвет обводки синий
                rec.StrokeThickness = 1;                                                            //  Толщина обводки
                RenderOptions.SetEdgeMode(rec, EdgeMode.Aliased);
                rec.Width = MarkerPointSize;                                                        //  Высота зоны захвата мышью 
                rec.Height = MarkerPointSize;                                                       //  Ширина зоны захвата мышью 
                Canvas.SetLeft(rec, ((Point)_Shape.GeometricParams[i].ParamValue).X - rec.Width / 2);//  Задаем место размещения зоны маркера на канве по X
                Canvas.SetTop(rec, ((Point)_Shape.GeometricParams[i].ParamValue).Y - rec.Height / 2);//  Задаем место размещения зоны маркера на канве по Y
                MarkerPoints.Add(rec);                                                              //  заносим маркер в список маркеров фигуры
                PaneControl.Children.Add(rec);                                                      //  добавляем маркер на панель контролов

                // создаем визуальную точку-контрол
                Ellipse el = new Ellipse();
                el.Stroke = Brushes.Transparent;                                                    //  В момент создания точка-контрол невидима - Цвет обводки прозрачный
                el.StrokeThickness = 1;                                                             //  Толщина обводки 
                //RenderOptions.SetEdgeMode(el, EdgeMode.Aliased);
                el.Fill = Brushes.Transparent;                                                      //  Заливка контрола прозрачная
                el.Width = ControlPointSize;                                                        //  Высота зоны захвата мышью 
                el.Height = ControlPointSize;                                                       //  Ширина зоны захвата мышью 
                el.ToolTip = _Shape.shapeBase.ToolTip;                                              //  Всплывающая подсказка с инфой о фигуре-хозяине
                ContextMenu context_menu = new ContextMenu();                                       //  На элементе установим для фигуры контекстное меню
                context_menu.Items.Add(new MenuItem() { Header = "RenameShape", Command = new ShapeContextMenuViewModel(_Editor, _Shape).ShapeRename });
                context_menu.Items.Add(new MenuItem() { Header = "DeleteShape", Command = new ShapeContextMenuViewModel(_Editor, _Shape).ShapeDelete });
                el.ContextMenu = context_menu;
                Canvas.SetLeft(el, ((Point)_Shape.GeometricParams[i].ParamValue).X - el.Width / 2); //  Задаем место размещения зоны захвата на канве по X
                Canvas.SetTop(el, ((Point)_Shape.GeometricParams[i].ParamValue).Y - el.Height / 2); //  Задаем место размещения зоны захвата на канве по Y
                el.Name = _Shape.shapeBase.Name + "_Point" + (i + 1).ToString();                    //  Зададим название контролу
                ControlPoints.Add(el);                                                              //  заносим контрол в список контролов фигуры
                PaneControl.Children.Add(el);                                                       //  добавляем контрол на панель контролов

                // добавляем обработчики событий мыши для контрола
                el.MouseEnter += new MouseEventHandler(Control_MouseEnter);
                el.MouseLeave += new MouseEventHandler(Control_MouseLeave);
                el.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
                el.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            }
            _Shape.ControlPointsCount = ControlPoints.Count;
        }

        //--- Удаление у фигуры лишних контролов
        public void RemoveControls()
        {
            if (_Shape.GeometricParams.Count < ControlPoints.Count)
            {
                //int i = _Shape.GeometricParams.Count;
                //for (; i < ControlPoints.Count; i++)                                //  переберем все контролы с индексом превышающим кол-во вершин фигуры
                int i = ControlPoints.Count - 1;
                for (; i >= _Shape.GeometricParams.Count; i--)                      //  переберем все контролы с индексом превышающим кол-во вершин фигуры
                {
                    PaneControl.Children.Remove((Rectangle)MarkerPoints[i]);        //      удаляем визуальный маркер выбора фигуры из панели контролов
                    PaneControl.Children.Remove((Ellipse)ControlPoints[i]);         //      удаляем контрол из панели контролов
                }
                MarkerPoints.RemoveRange(_Shape.GeometricParams.Count, ControlPoints.Count - _Shape.GeometricParams.Count);
                ControlPoints.RemoveRange(_Shape.GeometricParams.Count, ControlPoints.Count - _Shape.GeometricParams.Count);
                _Shape.ControlPointsCount = ControlPoints.Count;
            }
        }

        //--- Удаление у фигуры лишних контролов
        public void SetControlsVisibility( Visibility value )
        {
            for (int i= 0; i < ControlPoints.Count; i++)               //  переберем все контролы с индексом превышающим кол-во вершин фигуры
            {
                //((Rectangle)MarkerPoints[i]).Visibility= value;        //      устанавливаем "видимость" визуального маркера выбора фигуры
                ((Ellipse)ControlPoints[i]).Visibility = value;        //      устанавливаем "видимость" контрола
            }
        }

        //--- Проверка пересечений сторон поли-фигуры с хелп-линией
        public void CheckIntersectWithHelpLine()
        {
            int intersect_count = 0;
            double dist;
            double dist_prev= 100000000;

            Point h_point1 = new Point(HelpLine.X1, HelpLine.Y1);
            Point h_point2 = new Point(HelpLine.X2, HelpLine.Y2);
            Point curr_intersect = new Point();
            for (int i = 1; i < _Shape.GeometricParams.Count; i++)
            {
                if (GeometricFunction.IntersectionTwoLineSegments((Point)_Shape.GeometricParams[i - 1].ParamValue, (Point)_Shape.GeometricParams[i].ParamValue, 
                                                                    h_point1, h_point2, ref curr_intersect))
                {
                    intersect_count++;
                    dist = (h_point1 - curr_intersect).Length;
                    if( dist < dist_prev)
                    {
                        dist_prev = dist;
                        intersectionPoint = curr_intersect;
                    }
                }
            }

            if (intersect_count > 0)
            {
                SplitPoint.Visibility = Visibility.Visible;
                Canvas.SetLeft(SplitPoint, intersectionPoint.X - SplitPoint.Width / 2);
                Canvas.SetTop(SplitPoint, intersectionPoint.Y - SplitPoint.Height / 2);
                return;
            }
            else
            {
                SplitPoint.Visibility = Visibility.Collapsed;
            }
        }

        //--- Проверка самопересечений только существующих сторон поли-фигуры(без хелп-линии)
        public bool IsSelfIntersection()
        {
            if (_Shape.GeometricParams.Count > 2)
            {
                for (int i = 1; i < _Shape.GeometricParams.Count-1; i++)
                {
                    for (int j = i + 1; j < _Shape.GeometricParams.Count; j++)
                    {
                        if (GeometricFunction.IntersectionTwoLineSegments((Point)_Shape.GeometricParams[i - 1].ParamValue, (Point)_Shape.GeometricParams[i].ParamValue,
                                                    (Point)_Shape.GeometricParams[j - 1].ParamValue, (Point)_Shape.GeometricParams[j].ParamValue, ref intersectionPoint))
                        {
                            MessageBox.Show("The construction of the figure can not be completed because of the self-intersection of the sides!");
                            return (true);
                        }
                    }
                }
            }
            return (false);
        }

    }
}