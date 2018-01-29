using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace PolygonEditor
{
    //public partial class MainWindow : Window
    //{
        /// <summary>
        /// Базовый абстрактный класс для создания различных геометрических фигур основанный на интерфейсе с данными о фигуре
        /// </summary>
        public abstract class ShapeBase : IShapeParams
        {
            //---  Свойства базового интерфейса IShapeParams ---
            public string ShapeName { get; set; }                       //  Название фигуры
            public string ShapeType { get; set; }                       //  Тип фигуры
            public int StrokeWeight { get; set; }                       //  Толщина обводки
            public string StrokeColor { get; set; }                     //  Цвет обводки
            public string FillColor { get; set; }                       //  Цвет "тела" фигуры(при его наличии)
            public List<namedParameter> GeometricParams { get; set; }   //  Список параметров по которым строим фигуру
          //--------------------------

            public EditorModel _Editor;                                 //  Экземпляр редактора в котором создана фигура
            public Canvas drawingPane;                                  //  Панель на которой будет производится отрисовка геометричекой фигуры
            public Canvas controlPane;                                  //  Панель на которой размещаются точки-контролы 
            
            public List<TextBox> _List_tBox;                            //  Список полей в которые произведен ввод координат фигуры
            public List<string> paramNames;                             //  Список с названиями параметров для построения фигуры
            public List<string> paramTypes;                             //  Список с названиями типов параметров для построения фигуры

            public SHAPE_TYPE shapeType;                                //  Тип фигуры
            public /*Path*/Shape shapeBase;                             //  Фигура-контейнер для отрисовки Фигуры(имеет возможность отрисовки любых геометрий входящих в ее состав)
            public VisualControler visualControler;                     //  Контроллер отвечающий за действия над фигурой

            private int controlPointsCount;                             //  Кол-во точек-контролов фигуры для работы с фигурой мышью
            public int ControlPointsCount {
                get { return controlPointsCount; }
                set { controlPointsCount = value; }
            }

            private bool selectedForEdit;                               //  Флаг - фигура выбрана для редактирования
            public bool SelectedForEdit
            {
                get { return selectedForEdit; }
                set { selectedForEdit = value; }
            }

            private bool selectedForGroup;                              //  Флаг - фигура выбрана для групповых операций
            public bool SelectedForGroup
            {
                get { return selectedForEdit; }
                set { selectedForGroup = value; }
            }

            public ShapeBase(EditorModel editor)
            {
                _Editor = editor;                                       //  Экземпляр редактора в котором создана фигура 
                drawingPane = _Editor.DrawPanel;                        //  "Привязываем" фигуру к панели на которую будет выводится(рисоваться) фигура
                controlPane = _Editor.ControlPanel;                     //  "Привязываем" фигуру к панели через которую будет осуществляться мышиный контроль за фигурой
                GeometricParams = new List<namedParameter>();           //  Создаем экземпляр списка именованных геометрических параметров
                paramNames = new List<string>();                        //  Cоздаем экземпляр списка с названиями параметров для построения фигуры
                paramTypes = new List<string>();                        //  Cоздаем экземпляр списка с названиями типов параметров для построения фигуры
                
                SelectedForEdit = false;                                //  Устанавливаем флаг - фигура не редактируется
                SelectedForGroup = false;                               //  Устанавливаем флаг - фигура в группу выбранных не добавлена
            }

            // Получение параметров интерфейса фигуры 
            public ShapeParams GetShapeParams()
            {
                ShapeParams param = new ShapeParams();

                param.ShapeName = ShapeName;
                param.ShapeType = ShapeType;
                param.StrokeWeight = StrokeWeight;
                param.StrokeColor = StrokeColor;
                param.FillColor = FillColor;
                param.GeometricParams = new List<namedParameter>();
                for (int j = 0; j < GeometricParams.Count; j++)
                {
                    param.GeometricParams.Add(new namedParameter() { ParamName = GeometricParams[j].ParamName, ParamValue = GeometricParams[j].ParamValue} );
                }
                return param;
            }

            #region Абстрактные методы требующие конкретной реализации для каждого типа фигуры

                // Получение координат и параметров фигуры из массива текстовых полей для ввода данных о фигуре
                public abstract void Parse(List<TextBox> list_tBox);

                // Построение визуального геометрического отображения фигуры
                public abstract Geometry CreateShapeGeometry();

                // Обновление визуального отображения фигуры при изменении свойств фигуры
                public abstract void UpdateShapeGeometryByProperty(string property_name);

                // Обновление визуального отображения фигуры при изменении положения на заданный вектор
                public abstract void UpdateShapeGeometryByVector(Vector locationChange);

                // Обновление визуального отображения фигуры при изменении положения контрола
                public abstract void UpdateShapeGeometryByControl(string controlName, Point endLocation);

                //  Обновление визуального отображения фигуры по кординатам заданным в текстовых полях ввода
                public abstract void UpdateShapeGeometryByTextCoords();

                // Удаление фигуры и ее контролов из панелей редактора 
                public abstract void RemoveShapeFromPanels();

                // Обновление текстовых полей ввода с координатами геометрической фигуры по координатом фактического расположения фигуры
                public abstract void UpdateTextCoords();

                //  Установка параметров фигуры по информации интерфейса 
                public abstract void SetTextCoords(ShapeParams param);

                // Очистка текстовых полей ввода координат геометрической фигуры
                public abstract void ClearTextCoords();

                // Переименовать фигуру
                public abstract void Rename(string new_name);

            #endregion

                //  Добавление новой вершины в фигуру(для поли-фигур)
                public virtual void AddGeometryPoint(Point point) { }

               // Добавление текстового поля ввода координат геометрической фигуры в случае отсутсвия нужного кол-ва(для поли-фигур)
                public virtual void AddTextFieldToParamPanel() {}

                //  Удаление текстового поля ввода координат по его индексу(для поли-фигур)
                public virtual void RemoveTextFieldFromParamPanelAt(int field_index) {}

                // Удаление последней текстового поля ввода координат(поли-фигуры)
                public virtual void RemoveLastTextFieldFromParamPanel() {}

                //--- Проверка самопересечений сторон поли-фигуры
                public virtual bool IsSelfIntersection(bool is_mess = true) { return (false); }


            #region Набор вспомогательных методов для парсинга данных из поля ввода координат или параметров

                // Парсер принятой из поля ввода строки в число типа double
                protected double doubleParser(string str)
                {
                    try
                    {
                        return Double.Parse(str);
                    }
                    catch (Exception ex)
                    {
                        throw new System.ApplicationException(ex.Message, ex);
                    }
                }

                // Парсер принятой из поля ввода строки в сруктуру Point
                protected Point pointParser(string str)
                {
                    try
                    {
                        return Point.Parse(str);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString());
                        throw new System.ApplicationException("Please enter two numeric values separated by a space('  ') or a comma(',')\nFor example: 10,30", ex);
                    }
                }

                // Парсер принятой из поля ввода строки в сруктуру Size
                protected Size sizeParser(string str)
                {
                    Size ret_val = new Size();

                    string[] sizeString = str.Split(new char[] { ' ', ','});

                    if (sizeString.Length == 0 || sizeString.Length != 2)
                    {
                        //MessageBox.Show("Error: a size should contain two double that seperated by a space or ','");
                        throw new System.ApplicationException("A size should contain two double that seperated by a space('  ') or a comma(',')\nFor example: 10,30");
                    }

                    try
                    {
                        ret_val.Width = Convert.ToDouble(sizeString[0], System.Globalization.CultureInfo.InvariantCulture);
                        ret_val.Height = Convert.ToDouble(sizeString[1], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString());
                        throw new System.ApplicationException("Please enter only two numeric values into the field", ex);
                    }

                    return ret_val;
                }
            #endregion
        }
//    }
}
