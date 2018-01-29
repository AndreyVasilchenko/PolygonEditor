using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.ComponentModel.Design;
using CL_MartinezRuedaClipping;

namespace PolygonEditor
{
    public enum EDITOR_MODE { DrawNew, Edit, Selection, PolygonOperations, ModeUnknown };   //  Варианты возможных состояний окна редактора
    public enum SHAPE_TYPE { Point, Line, PolyLine, Polygon, Unknown };                     //  Варианты возможных фигур в редакторе

    public class EditorModel : ViewModelBase
    {
        public EditorModel()
        { }

        public PropertiesService prop_service;                      //  Сервис управления свойсвами редактора(как приложения)
        public CProperties save_property;                           //  Сохраняемые свойства приложения

        public Grid EditorGrid;                                     //  ???
        public Canvas MainPanel;                                    //  Основная панель-контейнер для остальных панелей
        public Canvas DrawPanel;                                    //  Панель на которой рисуем фигуры
        public Canvas ControlPanel;                                 //  Панель на которой рисуем точки-контролы и др.вспомогательные объекты для управления фигурами
        public Grid ParamPanels;                                    //  Контейнер с набором панелей для ввода параметров фигур

        public ShapesManager shapesManager;                         //  Менеджер фигур создаваемых в редакторе

        private Point _cursorPos;                                   //  Координаты курсора на холсте для рисования
        public Point CursorPos
        {
            get { return _cursorPos; }
            set
            {
                _cursorPos = value;
                OnPropertyChanged("CursorPos");
            }
        }

        private EDITOR_MODE _editorMode;                            //  Текущее состояние окна редактора
        public EDITOR_MODE EditorMode
        {
            get { return _editorMode; }
            set 
            {
                //if (value != _editorMode)
                //    IsSelectedShapeForEdit = false;

                _editorMode = value;
                OnPropertyChanged("EditorMode");
                if (MainPanel != null)
                {
                    if (_editorMode == EDITOR_MODE.DrawNew)
                        MainPanel.Cursor = Cursors.Cross;
                    else
                        MainPanel.Cursor = Cursors.Arrow;
                }
            }
        }

        private SHAPE_TYPE _shapeType;                              //  Выбранный тип для создания новой фигуры
        public SHAPE_TYPE ShapeType
        {
            get { return _shapeType; }
            set
            {
                if (value != SHAPE_TYPE.Unknown)
                {
                    _shapeType = value;
                    IsPressESC = true;                              //  Если был процесс рисования мышью, то отменим его
                    OnPropertyChanged("ShapeType");
                }
            }
        }

        private int _shapeStrokeWidth;                              //  Толщина линии рисования фигуры
        public int ShapeStrokeWidth
        {
            get { return _shapeStrokeWidth; }
            set
            {
                _shapeStrokeWidth = value;
                OnPropertyChanged("ShapeStrokeWidth");
                OnPropertyChanged("ShapeStrokeWidthForCombo");
            }
        }
        public int ShapeStrokeWidthForCombo                         //  Специальное свойство созданное для установки толoщины линии через селектор(Combo)
        {
            get { return _shapeStrokeWidth - 1; }
            set
            {
                _shapeStrokeWidth = value + 1;
                OnPropertyChanged("ShapeStrokeWidth");
                OnPropertyChanged("ShapeStrokeWidthForCombo");
            }
        }

        private Color _shapeStrokeColor;                            //  Цвет линии рисования фигуры
        public Color ShapeStrokeColor
        {
            get { return _shapeStrokeColor; }
            set
            {
                _shapeStrokeColor = value;
                OnPropertyChanged("ShapeStrokeColor");
            }
        }

        private Color _shapeFillColor;                              //  Цвет закраски тела фигуры
        public Color ShapeFillColor
        {
            get { return _shapeFillColor; }
            set
            {
                _shapeFillColor = value;
                OnPropertyChanged("ShapeFillColor");
            }
        }

        private int _shapeCount;                                     //  Счетчик общего количества фигур нарисованных на холсте редактора
        public int ShapeCount
        {
            get { return _shapeCount; }
            set
            {
                _shapeCount = value;
                OnPropertyChanged("ShapeCount");
            }
        }

        public int PointsCount;                                     //  Кол-во фигур-точек
        public int LinesCount;                                      //  Кол-во фигур-линий
        public int PolyLinesCount;                                  //  Кол-во фигур-полиЛиний
        
        private int _polygonsCount;                                 //  Кол-во фигур-полигонов
        public int PolygonsCount                                    //  Кол-во фигур-полигонов
        {
            get { return _polygonsCount; }
            set
            {
                _polygonsCount = value;
                OnPropertyChanged("PolygonsCount");
            }
        }

        private bool _isPressESC;                                   //  Флаг - в приложении нажата кнопка клавиатуры ESC
        public bool IsPressESC
        {
            get { return _isPressESC; }
            set
            {
                _isPressESC = value;
                OnPropertyChanged("IsPressESC");
            }
        }

        private bool _isPressEnd;                                   //  Флаг - в приложении нажата кнопка клавиатуры Enter
        public bool IsPressEnd
        {
            get { return _isPressEnd; }
            set
            {
                _isPressEnd = value;
                OnPropertyChanged("IsPressEnd");
            }
        }

        public bool IsPressedForBeginDraws;                         //  Флаг - нажата левая кнопка мыши с целью нарисовать новую фигуру

        public bool _isMouseDraws;                                  //  Флаг - в редакторе идет процесс рисования фигуры мышью
        public bool IsMouseDraws
        {
            get { return _isMouseDraws; }
            set
            {
                _isMouseDraws = value;
                OnPropertyChanged("IsMouseDraws");
            }
        }

        public bool _isPrompt;                                      //  Флаг - в редакторе идет процесс рисования фигуры мышью c подсказкой о том как закончить рисование полифигуры
        public bool IsPrompt
        {
            get { return _isPrompt; }
            set
            {
                _isPrompt = value;
                OnPropertyChanged("IsPrompt");
            }
        }

        private ShapeBase _drawShape;                               //  Фигура, которая находится в стадии рисования мышью
        public ShapeBase DrawShape
        {
            get { return _drawShape; }
            set
            {
                _drawShape = value;
                OnPropertyChanged("DrawShape");
            }
        }

        private bool _isSelectedShapeForEdit;                       //  Флаг наличия фигуры выбранной для редактирования
        public bool IsSelectedShapeForEdit
        {
            get { return _isSelectedShapeForEdit; }
            set
            {
                _isSelectedShapeForEdit = value;
                OnPropertyChanged("IsSelectedShapeForEdit");
            }
        }

        private ShapeBase _selectedShapeForEdit;                    //  Фигура выбранная для редактирования
        public ShapeBase SelectedShapeForEdit
        {
            get { return _selectedShapeForEdit; }
            set
            {
                _selectedShapeForEdit = value;
                OnPropertyChanged("SelectedShapeForEdit");
            }
        }

        private int _selectedShapeCount;                            //  Счетчик общего количества фигур выбранных для групповых действий над ними(сохранение, печать и т.п.)
        public int SelectedShapeCount
        {
            get { return _selectedShapeCount; }
            set
            {
                _selectedShapeCount = value;
                OnPropertyChanged("SelectedShapeCount");
            }
        }
        
        public List<ShapeBase> SelectedShapeList;                   //  Список фигур выбранных для групповых действий над ними(сохранение, печать и т.п.)


        private List<string> _polygonBinOptions;                    //  Список операций которые можно производить над полигонами(список создается из перечисления
        public List<string> PolygonBinOptions
        {
            get 
            {
                //return Enum.GetValues(typeof(BooleanOperations)).Cast<BooleanOperations>().Select(v => v.ToString()).ToList();
                return Enum.GetNames(typeof(BooleanOperations)).ToList();
            }
            set { _polygonBinOptions = value; }
        }

        private bool isLoadProcess;                                 //  Флаг - происходит процесс загрузки полигон-операции из файла
        public string _selectedPolygonBinOption;                    //  Наименование выбранной операции над полигонами
        public string SelectedPolygonBinOption
        {
            get { return _selectedPolygonBinOption; }
            set
            {
                _selectedPolygonBinOption = value;
                if( !isLoadProcess)
                    shapesManager.RemoveAllResults();                               
                ShowSubjects = true;
                ShowClippers = true;
                ShowResults = true;
                OnPropertyChanged("SelectedPolygonBinOption");
            }
        }

        private bool _setClippersOn;                                //  В режиме "операции с полигонами" включен режим выбора полигонов клипперов?
        public bool SetClippersOn
        {
            get { return _setClippersOn; }
            set
            {
                _setClippersOn = value;
                ShowSubjects = true;
                ShowClippers = true;
                OnPropertyChanged("SetClippersOn");
            }
        }

        private bool _showSubjects;                                 //  В режиме "операции с полигонами" показывать субъекты операций?
        public bool ShowSubjects
        {
            get { return _showSubjects; }
            set
            {
                _showSubjects = value;
                shapesManager.SetVisible(OPERATION_OBJECT_TYPE.Subject,value);
                OnPropertyChanged("ShowSubjects");
            }
        }

        private bool _showClippers;                                 //  В режиме "операции с полигонами" показывать полигоны-клипперы?
        public bool ShowClippers
        {
            get { return _showClippers; }
            set
            {
                _showClippers = value;
                shapesManager.SetVisible(OPERATION_OBJECT_TYPE.Clipper, value);
                OnPropertyChanged("ShowClippers");
            }
        }

        private bool _showResults;                                  //  В режиме "операции с полигонами" показывать полигоны-результаты операций?
        public bool ShowResults
        {
            get { return _showResults; }
            set
            {
                _showResults = value;
                shapesManager.SetVisible(OPERATION_OBJECT_TYPE.Result, value);
                OnPropertyChanged("ShowResults");
            }
        }

        public IFileServiceDialog fileServiceDialog;                //  Интерфейс для уточнения места сохранения и загрузки файлов
        public IJsonFileService jsonFileService;                    //  Интерфейс для сохранения и загрузки информации из файлов

        public My_MongoDB dataBase;                                 //  База данных для хранения фигур и разбиений полигонов
        public string databaseName= "PolygonEditor";                //  Название БазыДанных
        public string shapeColectionName = "Shapes";                //  Название коллекции отдельных фигур
        public string groupShapeColectionName = "ShapeGroups";      //  Название коллекции фигур обединенных в группы
        public string operationColectionName = "PolygonOperations"; //  Название коллекции разбиений полученых из полигонов

        //--- команда переключения редактора в режим выбора и манипуляций с фигурами
        private UserCommand setSelectionMode;
        public UserCommand SetSelectionMode
        { 
            get
            {
                return setSelectionMode ?? (setSelectionMode = new UserCommand(obj =>
                {
                    EditorMode = EDITOR_MODE.Selection;
                    IsEnabledParamPanels(false);                                        //  запрещаем доступ к панели корретировки параметров фигуры
                    IsPressESC = true;                                                  //  Если был режим рисования, отменим его
                }));
            }
        }

        //--- команда переключения редактора в режим рисования новой фигуры
        private UserCommand setDrawNewMode;
        public UserCommand SetDrawNewMode
        {
            get
            {
                return setDrawNewMode ?? (setDrawNewMode = new UserCommand(obj =>       //  Если команда ранее не была загружена, то загружаем следующее...
                {
                    EditorMode = EDITOR_MODE.DrawNew;                                   //  Изменяем свойство режима редактора

                    if (SelectedShapeList != null && SelectedShapeList.Count > 0)       //  Если имеется список выбранных фигур,  
                        shapesManager.UnselectAllShapes();                              //          отменяем "выбранность" у всех фигур списка

                    if (IsSelectedShapeForEdit)                                         //  Если обьявлена фигура для редактирования
                        shapesManager.UnselectShape(SelectedShapeForEdit);              //      отменяем этот выбор

                    IsEnabledParamPanels(true);                                         //  разрешаем доступ к панели корретировки параметров фигуры

                    IsPressESC = true;                                                  //  Если в этот момемент рисовалась фигура - сотрем ее
                }));
            }
        }

        //--- команда переключения редактора в режим редактирования фигуры
        private UserCommand setEditMode;
        public UserCommand SetEditMode
        {
            get
            {
                return setEditMode ?? (setEditMode = new UserCommand(obj =>             //  Если команда ранее не была загружена, то загружаем следующее...
                {
                    EditorMode = EDITOR_MODE.Edit;                                      //  Изменяем свойство режима редактора
                    
                    if (SelectedShapeList != null)                                      //  Если имеется список выбранных фигур,
                    {
                        if (SelectedShapeList.Count > 1)                                //      Если в списке более одного элемента,
                            shapesManager.UnselectAllShapes();                          //          отменяем "выбранность" у всех фигур списка
                        else if (SelectedShapeList.Count == 1)                          //      Если в списке только один элемент,
                        {
                            IsSelectedShapeForEdit = true;                              //          обьявляем фигуру этого элемента "редактируемой...
                            SelectedShapeForEdit = SelectedShapeList[0];
                        }
                        else                                                            //      Если нет выбранных елементов для редактирования
                        {
                            IsEnabledParamPanels(false);                                //          запрещаем доступ к панели корретировки параметров фигуры
                        }
                    }
                    else                                                                //      Если нет выбранных елементов для редактирования
                    {
                        IsEnabledParamPanels(false);                                    //          запрещаем доступ к панели корретировки параметров фигуры
                    }

                    if (IsSelectedShapeForEdit)                                         //   Если обьявлена фигура для редактирования
                        shapesManager.ReselectShapeForEdit(SelectedShapeForEdit);       //      делаем перевыбор

                    IsPressESC = true;                                                  //   Если в этот момемент рисовалась фигура - сотрем ее
                }));
            }
        }

        //--- команда переключения редактора в режим выбора и манипуляций с фигурами
        private UserCommand setPolygonOperationsMode;
        public UserCommand SetPolygonOperationsMode
        { 
            get
            {
                return setPolygonOperationsMode ?? (setPolygonOperationsMode = new UserCommand(obj =>
                {
                    EditorMode = EDITOR_MODE.PolygonOperations;                         //  Изменяем свойство режима редактора

                    IsEnabledParamPanels(false);                                        //  запрещаем доступ к панели корретировки параметров фигуры
                    IsPressESC = true;                                                  //  если и был режим рисования, отменим его
                    shapesManager.UnselectAllShapes();                                  //  отменяем "выбранность" у всех фигур списка
                    shapesManager.SetAllPolygonAsSubject();                             //  устанавливаем все полигоны как Subject
                    shapesManager.RemoveAllShapesExceptPolygons();                      //  удалим все лишние фигуры, кроме полигонов, если таковые имеются
                    SetClippersOn = true;
                }));
            }
        }

        //--- команда отмены всех Клипперов
        private UserCommand undoClippers;
        public UserCommand UndoClippers
        {
            get
            {
                return undoClippers ?? (undoClippers = new UserCommand(obj =>
                {
                    shapesManager.RemoveAllResults();                               //  удаляем результаты предыдущих операций
                    shapesManager.SetAllPolygonAsSubject();                         //  устанавливаем все полигоны как Subject
                    ShowSubjects = true;
                    ShowClippers = true;
                    ShowResults = false;
                    SetClippersOn = true;
                }));
            }
        }

        //--- команда разбиения полигона
        private UserCommand runOperation;
        public UserCommand RunOperation
        {
            get
            {
                return runOperation ?? (runOperation = new UserCommand(obj =>     //  Если команда ранее не была загружена, то загружаем следующее...
                {
                    try                                                                         //  ПОПЫТКА...
                    {
                        shapesManager.RemoveAllResults();
                        ShowSubjects = true;
                        ShowClippers = true;
                        ShowResults = true;
                        
                        if (PolygonsCount > 1)
                        {
                            if (SelectedPolygonBinOption == "None")
                                return;

                            BooleanOperations op_type = BooleanOperations.None;
                            GeoMultiPolygon subject = null;
                            GeoMultiPolygon clipper = null;
                        
                            for (int i = 0; i < shapesManager.shapeList.Count; i++)
                            {
                                if (shapesManager.shapeList[i].shapeType == SHAPE_TYPE.Polygon)
                                {
                                    if (((ShapePolygon)shapesManager.shapeList[i]).opObjType == OPERATION_OBJECT_TYPE.Subject)
                                    {
                                        if (subject == null)
                                            subject = PoligonsConvertor.GetGeoMultiPolygon(shapesManager.shapeList[i].GeometricParams);
                                        else
                                            PoligonsConvertor.AddToMultiPolygon(subject, shapesManager.shapeList[i].GeometricParams);
                                    }
                                    else if (((ShapePolygon)shapesManager.shapeList[i]).opObjType == OPERATION_OBJECT_TYPE.Clipper)
                                    {
                                        if(clipper == null)
                                            clipper = PoligonsConvertor.GetGeoMultiPolygon(shapesManager.shapeList[i].GeometricParams);
                                        else
                                            PoligonsConvertor.AddToMultiPolygon(clipper, shapesManager.shapeList[i].GeometricParams);
                                    }
                                }
                            }

                            if (subject == null)
                            {
                                MessageBox.Show("There is no Subject to perform the operation!");
                                return;
                            }

                            if (clipper == null || subject == null)
                            {
                                MessageBox.Show("There is no Clipper to perform the operation!");
                                return;
                            }

                            Point[][][] result_multiPolygon = null;

                            switch (SelectedPolygonBinOption)
                            {
                                case "Intersection":
                                    op_type = BooleanOperations.Intersection;
                                    result_multiPolygon = MartinezRuedaClipping.intersection(subject, clipper);
                                    break;
                                case "XOR":
                                    op_type = BooleanOperations.XOR;
                                    result_multiPolygon = MartinezRuedaClipping.xor(subject, clipper);
                                    break;
                                case "Intersect_and_XOR":
                                    op_type = BooleanOperations.Intersect_and_XOR;
                                    Point[][][] m_polygon1 = MartinezRuedaClipping.xor(subject, clipper);
                                    Point[][][] m_polygon2 = MartinezRuedaClipping.intersection(subject, clipper);
                                    if (m_polygon2 == null)
                                    {
                                        if (m_polygon1 != null)
                                            result_multiPolygon = m_polygon1;
                                    }
                                    else
                                    {
                                        if (m_polygon1 == null)
                                            result_multiPolygon = m_polygon2;
                                        else
                                            result_multiPolygon = MartinezRuedaClipping.ConcatArrays(m_polygon1, m_polygon2);
                                    }
                                    break;
                                case "Union":
                                    op_type = BooleanOperations.Union;
                                    result_multiPolygon = MartinezRuedaClipping.union(subject, clipper);
                                    break;
                                case "Difference":
                                    op_type = BooleanOperations.Difference;
                                    result_multiPolygon = MartinezRuedaClipping.diff(subject, clipper);
                                    break;
                            }

                            PolygonOperation op = new PolygonOperation()
                            {
                                SubjectGeometry = PoligonsConvertor.GetGeoMultiPolygon_RFC7946((Point[][][])subject.coordinates),
                                ClipperGeometry = PoligonsConvertor.GetGeoMultiPolygon_RFC7946((Point[][][])clipper.coordinates),
                                ResultGeometry = (result_multiPolygon != null && result_multiPolygon.GetLength(0) > 0) ? PoligonsConvertor.GetGeoMultiPolygon_RFC7946(result_multiPolygon) : null,
                                OperationType = op_type
                            };

                            if ( op.ResultGeometry != null)
                            {
                                shapesManager.RemoveAllResults();
                            
                                List<ShapeParams> shapeParam_list = PoligonsConvertor.GetShapeParams((double[][][][])op.ResultGeometry.coordinates, prop_service.properties.opProperty[(int)OPERATION_OBJECT_TYPE.Result]);

                                foreach (var item in shapeParam_list)                                               //      перебераем все элементы в списке 
                                {
                                    ShapeBase shape= shapesManager.CreateNewShapeByLoadParams((ShapeParams)item);   //          и создаем фигуру по параметрам элемента
                                    ((ShapePolygon)shape).opObjType = OPERATION_OBJECT_TYPE.Result;
                                    shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                                }
                            }
                            else
                            {
                                MessageBox.Show("Logical operation of the Subject with the Clipper has no result!");
                            }
                        }
                    }
                    catch (Exception ex)                                                                //  В случае "непредвиденных" обстоятельств на этапе ПОПЫТКИ,
                    {
                        MessageBox.Show("Exception: " + ex.Message, "UserCommand: RunOperation"); //      сообщаем о них
                    }
                }));
            }
        }

        //--- команда - стереть все результаты операции с полигонами
        private UserCommand deleteResults;
        public UserCommand DeleteResults
        {
            get
            {
                return deleteResults ?? (deleteResults = new UserCommand(obj =>
                {
                    shapesManager.RemoveAllResults();
                }));
            }
        }

        //--- команда для создания оповещения о нажатии в окне приложения кнопки ESC
        private UserCommand pressESC;
        public UserCommand PressESC
        {
            get { return pressESC ?? (pressESC = new UserCommand(obj => { IsPressESC = true; })); }
        }

        //--- команда для создания оповещения о нажатии в окне приложения кнопки Enter
        private UserCommand pressEnd;
        public UserCommand PressEnd
        {
            get { return pressEnd ?? (pressEnd = new UserCommand(obj => { IsPressEnd = true; })); }
        }

        // команда сохранения фигуры, набора фигур или разбиения полигонов в файл
        private UserCommand saveShapesToFile;
        public UserCommand SaveShapesToFile
        {
            get
            {
                return saveShapesToFile ?? (saveShapesToFile = new UserCommand(mode =>
                {
                    try
                    {
                        if (EditorMode == EDITOR_MODE.PolygonOperations)
                        {
                            PolygonOperation po = GetCurrentPolygonsOperationForSave();

                            if (po == null)
                                return;

                            string directory = prop_service.properties.FileLocation + @"\";
                            string file_name = "";
                            string file_ext = "Fragmentation(*.frag)|*.frag";

                            if (fileServiceDialog.SaveToFile(directory, file_name, file_ext) == true)
                            {
                                po.OperationName = Path.GetFileNameWithoutExtension(fileServiceDialog.FilePath);

                                jsonFileService.SaveToFile(fileServiceDialog.FilePath, po);
                            }
                        }
                        else
                        {
                            object save_instance;

                            ShapeParams param;
                            List<ShapeParams> paramList;

                            string directory = prop_service.properties.FileLocation + @"\";
                            string file_name = "";
                            string file_ext = "Shapes(*.shapes)|*.shapes";

                            if (mode != null && mode.ToString() == "All")
                            {
                                if (shapesManager.shapeList.Count > 0)
                                {

                                    if (shapesManager.shapeList.Count == 1)
                                    {
                                        param = shapesManager.shapeList[0].GetShapeParams();
                                        file_name = param.ShapeName;
                                        file_ext = "Shape(*.shape)|*.shape";
                                        save_instance = param;
                                    }
                                    else
                                    {
                                        paramList = new List<ShapeParams>();
                                        for (int i = 0; i < shapesManager.shapeList.Count; i++)
                                        {
                                            paramList.Add(shapesManager.shapeList[i].GetShapeParams());
                                        }
                                        save_instance = paramList;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Message: There are no Shapes to save!", "UserCommand: SaveShapesToFile");
                                    return;
                                }
                            }
                            else
                            {
                                if (SelectedShapeList != null && SelectedShapeList.Count > 0)
                                {
                                    if (SelectedShapeList.Count == 1)
                                    {
                                        param = SelectedShapeList[0].GetShapeParams();
                                        file_name = param.ShapeName;
                                        file_ext = "Shape(*.shape)|*.shape";
                                        save_instance = param;
                                    }
                                    else
                                    {
                                        paramList = new List<ShapeParams>();
                                        for (int i = 0; i < SelectedShapeList.Count; i++)
                                        {
                                            paramList.Add(SelectedShapeList[i].GetShapeParams());
                                        }
                                        save_instance = paramList;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Message: No selected Shapes to save!", "UserCommand: SaveShapesToFile");
                                    return;
                                }
                            }

                            if (fileServiceDialog.SaveToFile(directory, file_name, file_ext) == true)
                            {
                                jsonFileService.SaveToFile(fileServiceDialog.FilePath, save_instance);
                            }
                        }
                    }
                    catch (Exception ex)                                                                //  В случае "непредвиденных" обстоятельств на этапе ПОПЫТКИ,
                    {
                        MessageBox.Show("Exception: " + ex.Message, "UserCommand: SaveShapesToFile");   //      сообщаем о них
                    }
                }));
            }
        }

        // команда сохранения фигуры или набора фигур в файл
        private UserCommand saveShapesToDataBase;
        public UserCommand SaveShapesToDataBase
        {
            get
            {
                return saveShapesToDataBase ?? (saveShapesToDataBase = new UserCommand(mode =>
                {
                    try
                    {
                        if (EditorMode == EDITOR_MODE.PolygonOperations)
                        {
                            PolygonOperation po = GetCurrentPolygonsOperationForSave();

                            if (po == null)
                                return;

                            Window_SetName win = new Window_SetName(po.OperationName, "Save PoligonOperation As...");  //  создаем окно для редактирования названия операции над полигонами
                            win.Owner = App.Current.MainWindow;                                 //  это окно привязываем к окну приложения
                            if (win.ShowDialog() == true)                                       //  если операцию над полигонами сохраняем, то
                            {
                                dataBase.Save(operationColectionName, win.data_model.InstanceName, po);
                            }
                        }
                        else
                        {
                            object save_instance;
                            ShapeParams param;
                            List<ShapeParams> paramList;

                            string record_name="";
                            string win_title = "Save Shape As...";
                            string collection_name = groupShapeColectionName;

                            if (mode != null && mode.ToString() == "All")
                            {
                                if (shapesManager.shapeList.Count > 0)
                                {

                                    if (shapesManager.shapeList.Count == 1)
                                    {
                                        param = shapesManager.shapeList[0].GetShapeParams();
                                        record_name = param.ShapeName;
                                        collection_name = shapeColectionName;
                                        save_instance = param;
                                    }
                                    else
                                    {
                                        paramList = new List<ShapeParams>();
                                        for (int i = 0; i < shapesManager.shapeList.Count; i++)
                                        {
                                            paramList.Add(shapesManager.shapeList[i].GetShapeParams());
                                        }
                                        save_instance = paramList;
                                        win_title = "Save ShapeCollection As...";
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Message: There are no Shapes to save!", "UserCommand: SaveShapesToDataBase");
                                    return;
                                }
                            }
                            else
                            {
                                if (SelectedShapeList != null && SelectedShapeList.Count > 0)
                                {
                                    if (SelectedShapeList.Count == 1)
                                    {
                                        param = SelectedShapeList[0].GetShapeParams();
                                        record_name = param.ShapeName;
                                        collection_name = shapeColectionName;
                                        save_instance = param;
                                    }
                                    else
                                    {
                                        paramList = new List<ShapeParams>();
                                        for (int i = 0; i < SelectedShapeList.Count; i++)
                                        {
                                            paramList.Add(SelectedShapeList[i].GetShapeParams());
                                        }
                                        save_instance = paramList;
                                        win_title = "Save ShapeCollection As...";
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Message: No selected Shapes to save!", "UserCommand: SaveShapesToDataBase");
                                    return;
                                }
                            }

                            Window_SetName win = new Window_SetName(record_name, win_title);            //  создаем окно для редактирования названия фигуры или коллекции фигур
                            win.Owner = App.Current.MainWindow;                                         //  это окно привязываем к окну приложения
                            if (win.ShowDialog() == true)                                               //  если операцию над полигонами сохраняем, то
                            {
                                dataBase.Save(collection_name, win.data_model.InstanceName, save_instance);
                            }
                        }
                    }
                    catch (Exception ex)                                                                //  В случае "непредвиденных" обстоятельств на этапе ПОПЫТКИ,
                    {
                        MessageBox.Show("Exception: " + ex.Message, "UserCommand: SaveShapesToDataBase");   //      сообщаем о них
                    }
                }));
            }
        }

        // команда загрузки из файла фигуры или набора фигур
        private UserCommand loadInstanceFromFile;
        public UserCommand LoadInstanceFromFile
        {
            get
            {
                return loadInstanceFromFile ?? (loadInstanceFromFile = new UserCommand(obj =>               //  Если команда ранее не была загружена, то загружаем следующее...
                {
                    try                                                                                     //  ПОПЫТКА...
                    {
                        if (fileServiceDialog.LoadFromFile(prop_service.properties.FileLocation + @"\",
                            "Shapes(*.shape,*.shapes)|*.shape;*.shapes|PolygonOperation(*.frag)|*.frag") == true)// Получить от пользователя название и "путь" к загружаемому файлу
                        {
                            string ext = Path.GetExtension(fileServiceDialog.FilePath);

                            object instance;
                            
                            if (ext == ".frag")
                                instance = jsonFileService.LoadInstanceFromFile<PolygonOperation>(fileServiceDialog.FilePath);
                            else
                                instance = jsonFileService.LoadShapesFromFile(fileServiceDialog.FilePath);  //  Че-нибудь загружаем из выбранного файла
                            
                            if (instance is ShapeParams)                                                    //  Если загруженное - набор параметров для фигуры,
                            {
                                if (/*((ShapeParams)instance).ShapeType != "Polygon" &&*/ EditorMode == EDITOR_MODE.PolygonOperations)
                                    EditorMode = EDITOR_MODE.Selection;
                                ShapeBase shape = shapesManager.CreateNewShapeByLoadParams((ShapeParams)instance);            //      создаем фигуру по этим параметрам
                                shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                            }
                            else if (instance is List<ShapeParams>)                                         //  Если загруженное - список из наборов параметров для фигур,
                            {
                                foreach (var item in (List<ShapeParams>)instance)                           //      перебераем все элементы в списке 
                                {
                                    if (/*((ShapeParams)item).ShapeType != "Polygon" &&*/ EditorMode == EDITOR_MODE.PolygonOperations)
                                        EditorMode = EDITOR_MODE.Selection;
                                    ShapeBase shape = shapesManager.CreateNewShapeByLoadParams((ShapeParams)item);            //          и создаем фигуру по параметрам элемента
                                    shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                                }
                            }
                            else if (instance is PolygonOperation)                                          //  Если загруженное - операция над полигонами
                            {
                                ShowPolygonOperation((PolygonOperation)instance);
                            }
                            else                                                                            //  Если "загруженное" не соответствует требуемой информации....
                            {
                                MessageBox.Show("Message: An unknown instance has been loaded!", "UserCommand: LoadShapesFromFile");
                            }
                        }
                    }
                    catch (Exception ex)                                                                //  В случае "непредвиденных" обстоятельств на этапе ПОПЫТКИ,
                    {
                        MessageBox.Show("Exception: " + ex.Message, "UserCommand: LoadShapesFromFile"); //      сообщаем о них
                    }
                }));
            }
        }

        // команда загрузки из базы данных фигуры или набора фигур
        private UserCommand loadInstanceFromDataBase;
        public UserCommand LoadInstanceFromDataBase
        { 
            get
            {
                return loadInstanceFromDataBase ?? (loadInstanceFromDataBase = new UserCommand(obj =>               //  Если команда ранее не была загружена, то загружаем следующее...
                {
                    try                                                                                 //  ПОПЫТКА...
                    {
                        Window_LoadInstance win = new Window_LoadInstance(this);                        //  Создаем диалоговое окне редактирования свойств
                        win.Owner = App.Current.MainWindow;                                             //  это окно привязываем к окну приложения
                        if (win.ShowDialog() == true)                                                   //  Если диалог закончился подтверждением изменений
                        {
                            var instance = dataBase.GetInstanceByID(win.collection_name, win.instance_id);//  Че-нибудь загружаем из БД

                            if (instance is ShapeParams)                                                //  Если загруженное - набор параметров для фигуры,
                            {
                                if (/*((ShapeParams)instance).ShapeType != "Polygon" &&*/ EditorMode == EDITOR_MODE.PolygonOperations)
                                    EditorMode = EDITOR_MODE.Selection;
                                ShapeBase shape= shapesManager.CreateNewShapeByLoadParams((ShapeParams)instance);        //      создаем фигуру по этим параметрам
                                shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                            }
                            else if (instance is List<ShapeParams>)                                     //  Если список из наборов параметров для фигур,
                            {
                                foreach (var item in (List<ShapeParams>)instance)                       //      перебераем все элементы в списке 
                                {
                                    if (/*((ShapeParams)item).ShapeType != "Polygon" &&*/ EditorMode == EDITOR_MODE.PolygonOperations)
                                        EditorMode = EDITOR_MODE.Selection;
                                    ShapeBase shape = shapesManager.CreateNewShapeByLoadParams((ShapeParams)item);        //          и создаем фигуру по параметрам элемента
                                    shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                                }
                            }
                            else if (instance is PolygonOperation)                                      //  Если какая-нибудь операция над полигонами,
                            {
                                ShowPolygonOperation((PolygonOperation)instance);
                            }
                            else                                                                        //  Если "загруженное" не соответствует требуемой информации....
                            {
                                MessageBox.Show("Message: An unknown instance has been loaded!", "UserCommand: LoadShapesFromDataBase");
                            }
                        }
                    }
                    catch (Exception ex)                                                                //  В случае "непредвиденных" обстоятельств на этапе ПОПЫТКИ,
                    {
                        MessageBox.Show("Exception: " + ex.Message, "UserCommand: LoadShapesFromDataBase"); //      сообщаем о них
                    }
                }));
            }
        }




        //--- Создание начальных установок редактора
        public void Init(Grid _editorGrig, Canvas _mainPanel, Canvas _drawingPane, Canvas _controlPane, Grid _paramPane)
        {
            fileServiceDialog= new FileServiceDialog();                 //  Интерфейс для выбора места сохранения и загрузки файлов
            jsonFileService= new JsonFileService();                     //  Интерфейс для сохранения и загрузки информации из файлов

            prop_service = new PropertiesService();                     //  Создаем сервис свойств приложения
            prop_service.InitFromSetupFile();                           //  Пытаемся загрузить свойства редактора
            save_property= prop_service.properties;                     //  свойства

            dataBase = new My_MongoDB();                                //  Создаем экземпляр обработчика БД
            dataBase.OpenDB( save_property, databaseName );             //  Открываем БД
            
            EditorGrid = _editorGrig;
            MainPanel = _mainPanel;
            DrawPanel = _drawingPane;                                   //  Панель для рисования
            ControlPanel = _controlPane;                                //  Перель для контрольных элементов
            ParamPanels = _paramPane;                                   //  Контейнер с набором панелей для ввода параметров фигур

            shapesManager = new ShapesManager(this);                    //  Создаем менеджер для управления фигурами
            
            // задаем начальные праметры для построения фигуры в редакторе
            ShapeType = SHAPE_TYPE.Point;                               
            ShapeStrokeWidth = save_property.opProperty[0].StrokeWeight;
            ShapeStrokeColor = (Color)ColorConverter.ConvertFromString(save_property.opProperty[0].StrokeColor);
            ShapeFillColor = (Color)ColorConverter.ConvertFromString(save_property.opProperty[0].FillColor);

            IsSelectedShapeForEdit = false;                             //  Фигура для редактирования не выбрана
            SelectedShapeCount = 0;                                     //  да и вообще - ничего еще не выбрано )
            EditorMode = EDITOR_MODE.ModeUnknown;                       //  Задаем редактору режим выбора фигур
            IsMouseDraws = false;                                       //  Мышью пока никто не рисует
            IsPressedForBeginDraws = false;                             //  левую кнопку мыши еще ни кто для начала рисования не нажимал
            IsPressESC = false;                                         //  ESC никто не давил
            IsPressEnd = false;                                         //  End никто не давил
            IsPrompt = false;                                           //  Подсказка пока не нужна

            // задаем начальные праметры для режима "Операции с полигонами"
            SelectedPolygonBinOption = "Intersection";
            SetClippersOn = false;
            ShowSubjects = true;
            ShowClippers = true;
            ShowResults = true;
            isLoadProcess = false;
        }

        //--- Очистка всех панелей Редактора предназначенных для рисования и обслуживания фигур
        public void ClearAllPanels()
        {
            //-- Очистим панель используемую для хранения геометрических фигур
            if (DrawPanel.Children.Count > 0)
                DrawPanel.Children.RemoveRange(0, DrawPanel.Children.Count);
            //-- Очистим панель используемую для хранения вспомогательных элементов
            if (ControlPanel.Children.Count > 0)
                ControlPanel.Children.RemoveRange(0, ControlPanel.Children.Count);
            //--- Очистка панелей Редактора предназначенных для ручного ввода параметров
            ClearParamPanels();
       }

        //--- Очистка панелей Редактора предназначенных для ручного ввода параметров
        public void ClearParamPanels()
        {
            foreach (object panel in ParamPanels.Children)
            {
                if (panel is Panel)
                {
                    if (((Panel)panel).Name == "PolyLinePanel" || ((Panel)panel).Name == "PolygonPanel")
                    {
                        int t_box_count = 0;
                        int elem_idx = 0;
                        int[] idx = new int[0];
                        foreach (object element in ((Panel)panel).Children)               //  Считаем текс-боксы на панели и если есть "лишние" запоминаем их индексы
                        {
                            if (element is TextBox)
                            {
                                t_box_count++;
                                if (t_box_count > 3)
                                {
                                    Array.Resize<int>(ref idx, idx.Length + 1);
                                    idx[idx.Length - 1] = elem_idx;
                                }
                            }
                            elem_idx++;
                        }
                        if (t_box_count > 3)
                        {
                            for (int i = idx.Length - 1; i > -1; i--)
                            {
                                ((Panel)panel).Children.RemoveRange(idx[i] - 1, 2);
                            }
                            foreach (object element in ((Panel)panel).Children)
                            {
                                if (element is Button)
                                {
                                    switch (((Button)element).Content.ToString())
                                    {
                                        case "- Point":
                                            Canvas.SetTop((Button)element, 110);
                                            //((Button)element).Opacity = 0.4;
                                            //((Button)element).IsEnabled = false;
                                            break;
                                        case "+ Point":
                                            Canvas.SetTop((Button)element, 110);
                                            break;
                                        case "InsertShape":
                                            Canvas.SetTop((Button)element, 140);
                                            break;
                                        case "UpdateCoords":
                                            Canvas.SetTop((Button)element, 140);
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    foreach (object child in ((Panel)panel).Children)
                        if (child is TextBox)
                            ((TextBox)child).Text = "";
                }
            }
        }

        //---  Разрешение/запрет доступа к панелей Редактора предназначенных для ручного ввода параметров
        public void IsEnabledParamPanels(bool value)
        {
            foreach (object panel in ParamPanels.Children)
            {
                if (panel is Panel)
                {
                    ((Panel)panel).IsEnabled = value;
                }
            }
        }

        //--- Добавление поля ввода геометрических координат в панель параметров(полифигуры)
        public TextBox AddTextFieldToParamPanel(Canvas panel)
        {
            int label_count = -2;
            foreach (object obj in panel.Children)
            {
                if (obj is Label)
                    label_count++;

                if (obj is Button)
                {
                    Canvas.SetTop((Button)obj, Canvas.GetTop((Button)obj) + 20);
                    if (((Button)obj).Content.ToString() == "- Point")
                    {
                        ((Button)obj).Opacity = 1;
                    }
                }
            }
            Label label = new Label();
            label.FontSize = 12;
            label.FontWeight = FontWeights.Bold;
            label.Content = "Point_" + (label_count + 1).ToString() + ":";
            Canvas.SetTop(label, 45 + label_count * 20);
            panel.Children.Insert(panel.Children.Count - 4, label);

            TextBox tbox = new TextBox();
            tbox.Name = (panel.Name == "PolyLinePanel" ? "Pl" : "Pg") + "Point_" + (label_count + 1).ToString();
            tbox.Width = 60;
            tbox.AcceptsTab = false;
            Canvas.SetLeft(tbox, 56);
            Canvas.SetTop(tbox, 45 + label_count * 20);
            panel.Children.Insert(panel.Children.Count - 4, tbox);
            return tbox;
        }

        //--- Удаление последнего поля для ввода геометрических координат из панели параметров(полифигуры)
        public void RemoveLastTextFieldFromParamPanel(Canvas panel)
        {
            int tbox_count = 0;
            int elem_count = 0;
            foreach (object obj in panel.Children)
            {
                if (obj is TextBox)
                {
                    tbox_count++;
                }
                elem_count++;
            }
            if (elem_count > 13 && tbox_count > 3)
            {
                foreach (object obj in panel.Children)
                {
                    if (obj is Button)
                    {
                        Canvas.SetTop((Button)obj, Canvas.GetTop((Button)obj) - 20);
                        if ("- Point".CompareTo(((Button)obj).Content.ToString()) == 0)
                        {
                            if (panel.Children.Count < 13)
                                ((Button)obj).Opacity = 0.4;
                            else
                                ((Button)obj).Opacity = 1;
                        }
                    }
                }
                panel.Children.RemoveRange(elem_count - 5, 2);
            }
        }

        //--- Получение экземпляра текущей оперрации над полигонами
        public PolygonOperation GetCurrentPolygonsOperationForSave()
        {
            if (EditorMode == EDITOR_MODE.PolygonOperations)
            {
                GeoMultiPolygon subject = null;
                GeoMultiPolygon clipper = null;
                GeoMultiPolygon result = null;
                BooleanOperations op_type = BooleanOperations.None;

                for (int i = 0; i < shapesManager.shapeList.Count; i++)
                {
                    if (shapesManager.shapeList[i].shapeType == SHAPE_TYPE.Polygon)
                    {
                        if (((ShapePolygon)shapesManager.shapeList[i]).opObjType == OPERATION_OBJECT_TYPE.Subject)
                        {
                            if (subject == null)
                                subject = PoligonsConvertor.GetGeoMultiPolygon(shapesManager.shapeList[i].GeometricParams);
                            else
                                PoligonsConvertor.AddToMultiPolygon(subject, shapesManager.shapeList[i].GeometricParams);
                        }
                        else if (((ShapePolygon)shapesManager.shapeList[i]).opObjType == OPERATION_OBJECT_TYPE.Clipper)
                        {
                            if (clipper == null)
                                clipper = PoligonsConvertor.GetGeoMultiPolygon(shapesManager.shapeList[i].GeometricParams);
                            else
                                PoligonsConvertor.AddToMultiPolygon(clipper, shapesManager.shapeList[i].GeometricParams);
                        }
                        else if (((ShapePolygon)shapesManager.shapeList[i]).opObjType == OPERATION_OBJECT_TYPE.Result)
                        {
                            if (result == null)
                                result = PoligonsConvertor.GetGeoMultiPolygon(shapesManager.shapeList[i].GeometricParams);
                            else
                                PoligonsConvertor.AddToMultiPolygon(result, shapesManager.shapeList[i].GeometricParams);
                        }
                    }
                }

                switch (SelectedPolygonBinOption)
                {
                    case "Intersection":
                        op_type = BooleanOperations.Intersection;
                        break;
                    case "XOR":
                        op_type = BooleanOperations.XOR;
                        break;
                    case "Intersect_and_XOR":
                        op_type = BooleanOperations.Intersect_and_XOR;
                        break;
                    case "Union":
                        op_type = BooleanOperations.Union;
                        break;
                    case "Difference":
                        op_type = BooleanOperations.Difference;
                        break;
                }

                if (subject == null)
                {
                    MessageBox.Show("To save an operation there must be at least one Subject!");
                    return null;
                }
                if (clipper == null)
                {
                    MessageBox.Show("To save an operation there must be at least one Clipper!");
                    return null;
                }
                if (result == null)
                {
                    MessageBox.Show("To save an operation there must be at least one Result!");
                    return null;
                }

                PolygonOperation po = new PolygonOperation()
                {
                    SubjectGeometry = PoligonsConvertor.GetGeoMultiPolygon_RFC7946((Point[][][])subject.coordinates),
                    ClipperGeometry = PoligonsConvertor.GetGeoMultiPolygon_RFC7946((Point[][][])clipper.coordinates),
                    ResultGeometry = PoligonsConvertor.GetGeoMultiPolygon_RFC7946((Point[][][])result.coordinates),
                    OperationType = op_type
                };
                
                return po;
            }
            return null;
        }

        //--- Показ экземпляра оперрации над полигонами
        public void ShowPolygonOperation( PolygonOperation operation )
        {
            if (operation != null)
            {
                shapesManager.RemoveAllShapes();

                SelectedPolygonBinOption = Enum.GetName(typeof(BooleanOperations), operation.OperationType);

                if (operation.SubjectGeometry != null)
                {
                    List<ShapeParams> shapeParam_list = PoligonsConvertor.GetShapeParams((double[][][][])operation.SubjectGeometry.coordinates,
                        prop_service.properties.opProperty[(int)OPERATION_OBJECT_TYPE.Subject]);

                    foreach (var item in shapeParam_list)                                   //      перебераем все элементы в списке 
                    {
                        ShapeBase shape = shapesManager.CreateNewShapeByLoadParams((ShapeParams)item);   //          и создаем фигуру по параметрам элемента
                        ((ShapePolygon)shape).opObjType = OPERATION_OBJECT_TYPE.Subject;
                        shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                    }
                }

                if (operation.ClipperGeometry != null)
                {
                    List<ShapeParams> shapeParam_list = PoligonsConvertor.GetShapeParams((double[][][][])operation.ClipperGeometry.coordinates,
                        prop_service.properties.opProperty[(int)OPERATION_OBJECT_TYPE.Clipper]);

                    foreach (var item in shapeParam_list)                                   //      перебераем все элементы в списке 
                    {
                        ShapeBase shape = shapesManager.CreateNewShapeByLoadParams((ShapeParams)item);   //          и создаем фигуру по параметрам элемента
                        ((ShapePolygon)shape).opObjType = OPERATION_OBJECT_TYPE.Clipper;
                        shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                    }
                }

                if (operation.ResultGeometry != null)
                {
                    List<ShapeParams> shapeParam_list = PoligonsConvertor.GetShapeParams((double[][][][])operation.ResultGeometry.coordinates,
                        prop_service.properties.opProperty[(int)OPERATION_OBJECT_TYPE.Result]);

                    foreach (var item in shapeParam_list)                                   //      перебераем все элементы в списке 
                    {
                        ShapeBase shape = shapesManager.CreateNewShapeByLoadParams((ShapeParams)item);   //          и создаем фигуру по параметрам элемента
                        ((ShapePolygon)shape).opObjType = OPERATION_OBJECT_TYPE.Result;
                        shape.ClearTextCoords();                                           //      стираем на панели координат поля с кординатами предыдущей фигуры
                    }
                }

                isLoadProcess = true;
                EditorMode = EDITOR_MODE.PolygonOperations;
                isLoadProcess = false;
            }
        }
    }
}
