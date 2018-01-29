using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace PolygonEditor
{
    public class ShapesManager
    {
        public List<ShapeBase> shapeList;                                       //  Список обслуживаемых фигур
        EditorModel _Editor;                                                    //  Редактор в котором происходят манипуляции с фигурами

        public ShapesManager(EditorModel editor)
        {
            _Editor = editor;                                                   //  Модель управления приложением - Редактор 
            shapeList = new List<ShapeBase>();                                  //  Создаем список обслуживаемых фигур

            _Editor.ShapeCount = 0;                                             //  Общее количество фигур равно нулю
            _Editor.PointsCount = 0;                                            //  Кол-во фигур-точек равно нулю
            _Editor.LinesCount = 0;                                             //  Кол-во фигур-линий равно нулю
            _Editor.PolyLinesCount = 0;                                         //  Кол-во фигур-полиЛиний равно нулю
            _Editor.PolygonsCount = 0;                                          //  Кол-во фигур-полигонов равно нулю

            _Editor.SelectedShapeCount = 0;                                     //  Кол-во "выбранных фигур" равноо нулю
        }

        //--- Создание новой фигуры по списку параметров, заданных в текстовых полях ввода
        public ShapeBase CreateNewShape(object params_source)
        {
            ShapeBase shape;                                                    //  Фигура, которую будем пытаться создать
            shape = null; 

            switch (_Editor.ShapeType)                                          //  Выбираем конструктор для создания фигуры в зависимости от типа фигуры заданного в Редакторе
            {
                case SHAPE_TYPE.Point:
                    shape = new ShapePoint(params_source, _Editor);                  //      Создаем фигуру-точку
                    break;
                case SHAPE_TYPE.Line:
                    shape = new ShapeLine(params_source, _Editor);                   //      Создаем фигуру-линию
                    break;
                case SHAPE_TYPE.PolyLine:
                    shape = new ShapePolyLine(params_source, _Editor);               //      Создаем фигуру-полиЛинию
                    break;
                case SHAPE_TYPE.Polygon:
                    shape = new ShapePolygon(params_source,  _Editor);                //      Создаем фигуру-полигон
                    break;
                default:
                    throw new System.ApplicationException("ShapesManager.CreateNewShape(List<TextBox> l_tb): Incorrect Geometry type!");
            }
            
            if (shape != null)                                                  //  Если фигура создана
            {
                if (_Editor.IsMouseDraws)                                       //      Если режим рисования мышью не закончен, то
                    _Editor.DrawShape = shape;                                  //          запоминаем в редакторе фигуру, которую "рисуем" мышью
                _Editor.ShapeCount++;                                           //      Увеличиваем число фигур в Редакторе на одну
                shapeList.Add(shape);                                           //      Добавляем созданную фигуру в список "обслуживаемых" МенеджеромФигур
            }
            else
            {
                throw new System.ApplicationException("ShapesManager.CreateNewShape(List<TextBox> l_tb):\n The shape is not created - the reason must be clarified by programmer!");
            }
            return shape;
        }

        //--- Создание новой фигуры с помощью мыши по кординатам начальной точки построения фигуры
        public void CreateNewShapeWithMouse(Point startPoint)
        {
            //_Editor.ClearParamPanels();
            List<TextBox> l_tb = new List<TextBox>();                                           //  Создаем и заполняем список полей содержащих координаты создаваемой фигуры
            if (l_tb == null)                                                                   //  Если создать список не удалось, то
                throw new System.ApplicationException("ShapesManager.CreateNewShapeWithMouse(): Could not create a list of text fields with shape parameters!");
            
            for (int i = 0; i < _Editor.ParamPanels.Children.Count; i++)                        //  Перебираем весь список панелей с текстовыми полями для задания параметров фигур
            {
                if (((Panel)_Editor.ParamPanels.Children[i]).Visibility == Visibility.Visible)  //      Если панель видима для пользователя, то
                {
                    foreach (object obj in ((Panel)_Editor.ParamPanels.Children[i]).Children)   //          Перебираем все элементы на этой панели
                        if (obj is TextBox)                                                     //              Если элемент является текстовым полем, то
                            l_tb.Add((TextBox)obj);                                             //                  добавляем его в список полей соодержщих параметры фигуры
                    break;
                }
            }
            
            if (l_tb.Count < 1)                                                                 //  Если наполнить список не удалось, то
                throw new System.ApplicationException("ShapesManager.CreateNewShapeWithMouse(): Parameter list does not contain elements!");
            else                                                                                //  Иначе
            {
                l_tb[0].Text = startPoint.X.ToString() + " " + startPoint.Y.ToString();         //      В первое поле созданного списка заносим кординаты начала построения фигуры
                switch (_Editor.ShapeType)                                                      //      Узнаем фигуру какого типа строим в Редакторе
                {
                    case SHAPE_TYPE.Point:
                        _Editor.IsMouseDraws = false;                                           //          Фигура-точка дальнейшего рисования не требует - достаточно первого нажатиямыши
                        break;
                    case SHAPE_TYPE.Line:
                    case SHAPE_TYPE.PolyLine:
                    case SHAPE_TYPE.Polygon:
                        _Editor.IsMouseDraws = true;                                            //          Требуется дальнейшее продолжение рисования мышью
                        l_tb[1].Text = startPoint.X.ToString() + " " + startPoint.Y.ToString(); //          во второе поле созданного списка также заносим кординаты начала построения фигуры
                        break;
                    default:                                                                    //          Если тип фигуры для построения МенеджерФигур "не знает", то
                        throw new System.ApplicationException("ShapesManager.CreateNewShapeWithMouse(): Incorrect Geometry type!");
                }

                try                                                                             //      пытаемся
                {
                    CreateNewShape(l_tb);                                                       //          построить фигуру по начальным кординатам мыши с дальнейшим продолжением
                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show(ex.Message, "Error!");
                }
                catch (Exception ex)
                {
                    throw new System.ApplicationException("ShapesManager.CreateNewShapeWithMouse(): Could not draw the shape with the mouse!\n\n" + ex.Message);
                }
            }
        }

        //--- Создание новой фигуры по параметрам загруженным из файла или БД
        public ShapeBase CreateNewShapeByLoadParams(ShapeParams param)
        {
            ShapeBase shape= null;                                                          //  Фигура, которую будем пытаться создать
            
            switch (param.ShapeType)                                                        //      Узнаем фигуру какого типа строим в Редакторе
            {
                case "Point":
                    _Editor.ShapeType = SHAPE_TYPE.Point;
                    break;
                case "Line":
                    _Editor.ShapeType = SHAPE_TYPE.Line;
                    break;
                case "PolyLine":
                    _Editor.ShapeType = SHAPE_TYPE.PolyLine;
                    break;
                case "Polygon":
                    _Editor.ShapeType = SHAPE_TYPE.Polygon;
                    break;
                default:                                                                    //          Если тип фигуры для построения МенеджерФигур "не знает", то
                    throw new System.ApplicationException("ShapesManager.CreateNewShapeByLoadParams(): Unknown Geometry Type!");
            }

            try                                                                             //      пытаемся
            {
                shape= CreateNewShape(param);                                               //          построить фигуру по начальным кординатам мыши с дальнейшим продолжением
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("ShapesManager.CreateNewShapeByLoadParams(): Could not draw the shape by loaded Parameters!\n\n" + ex.Message);
            }
            
            return shape;
        }

        //--- Создание новой фигуры по заданным параметрам
        public void CreateNewShapeByParams(string shapeType, string[,] param_array, int strokeWeight = 1, string shapeName = "Shape", string strokeColor = "#FF000000")
        {
            ShapeParams param = new ShapeParams();
            param.GeometricParams = new List<namedParameter>();
            for (int i = 0; i < param_array.GetLength(0); i++)
            {
                namedParameter p = new namedParameter();
                p.ParamName = param_array[i, 0];
                p.ParamValue = param_array[i, 1];
                param.GeometricParams.Add(p);
            }
            param.ShapeType = shapeType;
            param.ShapeName = shapeName;
            param.StrokeColor = strokeColor;
            param.StrokeWeight = strokeWeight;
            
            CreateNewShapeByLoadParams( param );        //          и создаем фигуру по параметрам элемента
        }

        //--- Удаление фигуры из Редактора из МенеджераФигур
        public void RemoveShape(ShapeBase shape)
        {
            if (_Editor.SelectedShapeForEdit != null && _Editor.SelectedShapeForEdit.GetHashCode() == shape.GetHashCode())
            {
                _Editor.IsSelectedShapeForEdit = false;                             // Теперь редактируемой фигуры нет
                _Editor.SelectedShapeForEdit = null;
            }
            if (_Editor.SelectedShapeList != null && _Editor.SelectedShapeList.Contains(shape))
            {
                _Editor.SelectedShapeList.Remove(shape);
                _Editor.SelectedShapeCount--;
            }

            ((ShapeBase)shape).RemoveShapeFromPanels();                             //  Удалем фигуру из всех панелей рисования и обслуживания
            shapeList.Remove(shape);                                                //  Удаляем фигуру из списока обслуживамых фигур
            _Editor.ShapeCount--;                                                   //  Общее кол-во фигур уменьшилось на одну
            
            switch (shape.shapeType)                                                // В зависимости от типа фигуры уменьшаем счетчик этих фигур на единицу
            {
                case SHAPE_TYPE.Point:
                    if (_Editor.PointsCount > 0) _Editor.PointsCount--;
                    break;
                case SHAPE_TYPE.Line:
                    if (_Editor.LinesCount > 0) _Editor.LinesCount--;
                    break;
                case SHAPE_TYPE.PolyLine:
                    if (_Editor.PolyLinesCount > 0) _Editor.PolyLinesCount--;
                    break;
                case SHAPE_TYPE.Polygon:
                    if (_Editor.PolygonsCount > 0) 
                        _Editor.PolygonsCount--;
                    if (_Editor.EditorMode == EDITOR_MODE.PolygonOperations)
                        if (_Editor.PolygonsCount < 2)
                            _Editor.EditorMode = EDITOR_MODE.Selection;
                    break;
            }

            if (_Editor.ShapeCount < 1)
                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)
                    _Editor.EditorMode = EDITOR_MODE.ModeUnknown;
        }

        //--- Удаление всех фигур из Редактора и МенеджераФигур
        public void RemoveAllShapes()
        {
            if (shapeList.Count > 0)                                                //  Если список фигур в редакторе не пуст
            {
                foreach (object shape in shapeList)                                 //      Удаляем все фигуры с панелей Редактора служащих для рисования и обслуживания
                {
                    if (shape is ShapeBase)                                         //          контрольная проверка на соответсвие типу
                    {
                        ((ShapeBase)shape).RemoveShapeFromPanels();                 //              удаление фигуры из всех панелей рисования и обслуживания
                    }
                }
                shapeList.Clear();                                                  //      и очищаем весь список обслуживамых фигур

                _Editor.ShapeCount = 0;                                             //      Общее количество фигур равно нулю
                _Editor.PointsCount = 0;                                            //      Кол-во фигур-точек равно нулю
                _Editor.LinesCount = 0;                                             //      Кол-во фигур-линий равно нулю
                _Editor.PolyLinesCount = 0;                                         //      Кол-во фигур-полиЛиний равно нулю
                _Editor.PolygonsCount = 0;                                          //      Кол-во фигур-полигонов равно нулю

                if (_Editor.SelectedShapeList != null )
                {
                    _Editor.SelectedShapeCount = 0;                                 //      Выбранных фигур не стало
                    _Editor.SelectedShapeList.Clear();
                }
                
                _Editor.IsSelectedShapeForEdit = false;                             //      Редактируемой фигуры нет
                _Editor.SelectedShapeForEdit = null;
            }

            if (_Editor.EditorMode != EDITOR_MODE.DrawNew)
                _Editor.EditorMode = EDITOR_MODE.ModeUnknown;
        }

        //--- Удаление всех фигур, кроме полигонов, из Редактора и МенеджераФигур
        public void RemoveAllShapesExceptPolygons()
        {
            if (shapeList.Count > 0)                                                 //  Если список фигур в редакторе не пуст
            {
                for (int i = shapeList.Count - 1; i > -1; i-- )                      //      Удаляем все фигуры с панелей Редактора служащих для рисования и обслуживания
                {
                    if (shapeList[i] is ShapeBase && !(shapeList[i] is ShapePolygon))//          контрольная проверка на соответсвие типу
                    {
                        this.RemoveShape((ShapeBase)shapeList[i]);                   //              удаление фигуры из всего
                    }
                }
            }
            if (_Editor.ShapeCount < 1)
                if (_Editor.EditorMode != EDITOR_MODE.DrawNew)
                    _Editor.EditorMode = EDITOR_MODE.ModeUnknown;
        }

        //--- Смена выбранной фигуры в режиме редактирования
        public void ReselectShapeForEdit(ShapeBase new_select_shape)
        {
            UnselectAllShapes();                                                                //  Убираем пометки выбора у всех выбранных до этого фигур
            SelectShape(new_select_shape);                                                      //  и выбираем заданную в параметре
        }

        //--- Выбор фигуры для дальнейших действий
        public void SelectShape(ShapeBase new_select_shape)
        {
            new_select_shape.ClearTextCoords();                                                                     //  Очистим панель ручноговвода параметров для новой выбираемой фигуры

            _Editor.IsSelectedShapeForEdit = true;                                                                  //  в редакторе есть выделенная фигура
            _Editor.SelectedShapeForEdit = new_select_shape;                                                        //  устанавливаем эту выделенную фигуру
            _Editor.ShapeType = new_select_shape.shapeType;                                                         //  устанавливаем тип этой фигуры
            _Editor.SelectedShapeForEdit.SelectedForEdit = true;
            _Editor.SelectedShapeForEdit.visualControler.IsSelectedPath = true;
            for (int i = 0; i < _Editor.SelectedShapeForEdit.visualControler.ControlPoints.Count; i++)              //  подсвечиваем маркеры выделенности у этой фигуры
            {
                ((Rectangle)_Editor.SelectedShapeForEdit.visualControler.MarkerPoints[i]).Visibility = Visibility.Visible;
            }
            
            if (_Editor.SelectedShapeList == null)                                                                  //  Если списка выделенных фигур еще нет
            {
                _Editor.SelectedShapeList = new List<ShapeBase>();                                                  //      создаем список выделенных фигур
            }

            if (!_Editor.SelectedShapeList.Contains(new_select_shape))                                              //      на всякий случай проверяем, что фигуры нет в списке(исключам дублирование)
            {
                _Editor.SelectedShapeList.Add(new_select_shape);                                                    //          и добавляем ее в список
                _Editor.SelectedShapeCount++;
            }

            _Editor.SelectedShapeForEdit.UpdateTextCoords();                                                        //  обновим панель ручного ввода параметров
            
            _Editor.ShapeStrokeWidth = new_select_shape.StrokeWeight;                                               //  зададим редактору параметры редактруемой фигуры
            _Editor.ShapeStrokeColor = (Color)ColorConverter.ConvertFromString(new_select_shape.StrokeColor);
            if (new_select_shape.FillColor != null )
                _Editor.ShapeFillColor = (Color)ColorConverter.ConvertFromString(new_select_shape.FillColor);

            if(_Editor.EditorMode == EDITOR_MODE.Edit)
                _Editor.IsEnabledParamPanels(true);                                                                  //  Разрешаем доступ к панели ручного редактирования параметров
        }

        //--- Выбор всех фигур на холсте редактора для дальнейших действий
        public void SelectAllShapes()
        {
            if (_Editor.shapesManager.shapeList != null && _Editor.shapesManager.shapeList.Count > 0)               //  Если имеются на холсте фигуры
            {
                foreach (ShapeBase shape in _Editor.shapesManager.shapeList)                                        //      перебираем весь список фигур
                {
                    SelectShape(shape);                                                                             //          и выберем их
                }
            }
        }

        //--- Отмена "выбранности" у заданной "выбранной" фигуры
        public void UnselectShape(ShapeBase select_shape)
        {
            int find_idx = _Editor.SelectedShapeList.FindIndex(obj => obj.GetHashCode() == select_shape.GetHashCode());

            if (find_idx >= 0 )
            {
                for (int i = 0; i < select_shape.visualControler.ControlPoints.Count; i++)                          // Отменяем "видимость" у маркеров фигуры
                {
                    ((Rectangle)select_shape.visualControler.MarkerPoints[i]).Visibility = Visibility.Hidden;
                }
                select_shape.visualControler.IsSelectedPath = false;                                                // "обнуляем" флаг выбранности у фигуры
                if ((select_shape.shapeType== SHAPE_TYPE.PolyLine ||  select_shape.shapeType== SHAPE_TYPE.Polygon) && select_shape._List_tBox.Count > 3)
                    select_shape._List_tBox.RemoveRange(3, select_shape._List_tBox.Count - 3);
                _Editor.SelectedShapeList.RemoveAt(find_idx);                                                       //  удаляем фигуру из списка выбраннных
                _Editor.SelectedShapeCount--;                                                                       //  счетчик выбранных фигур уменьшаем на единицу
                _Editor.IsSelectedShapeForEdit = false;                                                             //  флаг наличия редактируемой фигуры - "обнуляем"
                _Editor.SelectedShapeForEdit = null;                                                                //  таковой больше нет
                _Editor.ClearParamPanels();                                                                         //  "чистим" все текстовые поля ввода параметров

                if (_Editor.EditorMode == EDITOR_MODE.Edit)
                    _Editor.IsEnabledParamPanels(false);                                                            //  запрещаем доступ к панели ручного редактирования параметров
            }
        }

        //--- Отмена "выбранности" у всех "выбранных" фигур
        public void UnselectAllShapes()
        {
            if (_Editor.SelectedShapeList != null && _Editor.SelectedShapeList.Count > 0)                           //  Если имеются выбранные фигуры
            {
                for (int j = 0; j < _Editor.SelectedShapeList.Count; j++)                                           //      перебираем весь список выбранных фигур
                {
                    for (int i = 0; i < _Editor.SelectedShapeList[j].visualControler.ControlPoints.Count; i++)      //          у каждой фигуры из списка выбранных, отменяем видимость маркеров "выбранности"
                    {
                        ((Rectangle)_Editor.SelectedShapeList[j].visualControler.MarkerPoints[i]).Visibility = Visibility.Hidden;
                    }
                    _Editor.SelectedShapeList[j].visualControler.IsSelectedPath = false;                            //          и "сбрасываем" флаг "выбранности"
                    if ((_Editor.SelectedShapeList[j].shapeType == SHAPE_TYPE.PolyLine || _Editor.SelectedShapeList[j].shapeType == SHAPE_TYPE.Polygon) 
                        && _Editor.SelectedShapeList[j]._List_tBox.Count > 3)
                        
                        _Editor.SelectedShapeList[j]._List_tBox.RemoveRange(3, _Editor.SelectedShapeList[j]._List_tBox.Count - 3);
                }
                _Editor.SelectedShapeList.Clear();                                                                  //      очищаем список выбранных фигур от содержимого
                _Editor.SelectedShapeCount = 0;                                                                     //      выбранных фигур - нет
                _Editor.SelectedShapeForEdit= null;                                                                 //      фигуры для редактирования нет
                _Editor.IsSelectedShapeForEdit = false;                                                             //      флаг наличия такой фигуры - "обнуляем"
                _Editor.ClearParamPanels();                                                                         //      "чистим" все текстовые поля ввода параметров
            }
        }

        //--- устанавливаем все полигоны как Subject
        public void SetAllPolygonAsSubject()
        {
            if (_Editor.PolygonsCount > 1)
            {
                for (int i = 0; i < _Editor.ShapeCount; i++)
                {
                    if (shapeList[i] is ShapePolygon)
                    {
                        if (((ShapePolygon)shapeList[i]).opObjType != OPERATION_OBJECT_TYPE.Result)
                        {
                            ((ShapePolygon)shapeList[i]).opObjType = OPERATION_OBJECT_TYPE.Subject;
                            shapeList[i].StrokeWeight = _Editor.save_property.opProperty[(int)OPERATION_OBJECT_TYPE.Subject].StrokeWeight;
                            shapeList[i].StrokeColor = _Editor.save_property.opProperty[(int)OPERATION_OBJECT_TYPE.Subject].StrokeColor;
                            shapeList[i].FillColor = _Editor.save_property.opProperty[(int)OPERATION_OBJECT_TYPE.Subject].FillColor;
                            ((ShapePolygon)shapeList[i]).UpdateShapeGeometry();
                        }
                    }
                }

                RemoveAllResults();
            }
        }

        //--- устанавливаем все полигоны как Subject
        public void UnsetPolygonType()
        {
            if (_Editor.PolygonsCount > 1)
            {
                for (int i = 0; i < _Editor.ShapeCount; i++)
                {
                    if (shapeList[i] is ShapePolygon)
                    {
                        ((ShapePolygon)shapeList[i]).opObjType = null;
                    }
                }
            }
        }

        //--- устанавливаем или убираем у полигона свойства Clipper(а)
        public bool ResetAsClipper( ShapeBase shape )
        {
            if (_Editor.PolygonsCount > 1)
            {
                if (shape is ShapePolygon)
                {
                    RemoveAllResults();
                    _Editor.ShowSubjects = true;
                    _Editor.ShowClippers = true;

                    if (((ShapePolygon)shape).opObjType == OPERATION_OBJECT_TYPE.Subject)
                    {
                        ((ShapePolygon)shape).opObjType = OPERATION_OBJECT_TYPE.Clipper;
                        shape.StrokeWeight = _Editor.save_property.opProperty[(int)OPERATION_OBJECT_TYPE.Clipper].StrokeWeight;
                        shape.StrokeColor = _Editor.save_property.opProperty[(int)OPERATION_OBJECT_TYPE.Clipper].StrokeColor;
                        shape.FillColor = _Editor.save_property.opProperty[(int)OPERATION_OBJECT_TYPE.Clipper].FillColor;
                        ((ShapePolygon)shape).UpdateShapeGeometry();
                    }
                    else if (((ShapePolygon)shape).opObjType == OPERATION_OBJECT_TYPE.Clipper)
                    {
                        ((ShapePolygon)shape).opObjType = OPERATION_OBJECT_TYPE.Subject;
                        shape.StrokeWeight = _Editor.save_property.opProperty[(int)OPERATION_OBJECT_TYPE.Subject].StrokeWeight;
                        shape.StrokeColor = _Editor.save_property.opProperty[(int)OPERATION_OBJECT_TYPE.Subject].StrokeColor;
                        shape.FillColor = _Editor.save_property.opProperty[(int)OPERATION_OBJECT_TYPE.Subject].FillColor;
                        ((ShapePolygon)shape).UpdateShapeGeometry();
                    }
                }
                else
                {
                    MessageBox.Show("Unsuccessful attempt to set Clipper parameters for a figure that is not a Polygon!", "ShapeManager.ResetAsClipper()");
                    return (false);
                }
            }
            else
            {
                MessageBox.Show("There is not enough polygons for a boolean operation over polygons!", "ShapeManager.ResetAsClipper()");
                return (false);
            }
            return (true);
        }

        //--- Удаление всех полигонов, которые являются результатами операций над полигонами
        public void RemoveAllResults()
        {
            if (_Editor.PolygonsCount > 0)
            {
                for (int i = _Editor.ShapeCount-1; i > -1; i--)
                {
                    if (shapeList[i] is ShapePolygon)
                    {
                        if (((ShapePolygon)shapeList[i]).opObjType == OPERATION_OBJECT_TYPE.Result)
                        {
                            RemoveShape(shapeList[i]);
                        }
                    }
                }
            }
        }

        //--- Установка у всех полигонов определенного типа, заданного параметром свойства визульной видимости
        public void SetVisible(OPERATION_OBJECT_TYPE type, bool value)
        {
            if (_Editor.PolygonsCount > 0)
            {
                for (int i = 0; i < _Editor.ShapeCount; i++)
                {
                    if (shapeList[i] is ShapePolygon)
                    {
                        if (((ShapePolygon)shapeList[i]).opObjType == type)
                        {
                            Visibility vis= (value) ? Visibility.Visible : Visibility.Hidden;

                            ((ShapePolygon)shapeList[i]).shapeBase.Visibility = vis;
                            shapeList[i].visualControler.SetControlsVisibility( vis );
                        }
                    }
                }
            }
/*            else
            {
                MessageBox.Show("There is not enough polygons for a boolean operation over polygons!", "ShapeManager.SetAllPolygonAsSubject()");
                return (false);
            }
            return (true);
*/        }
    }
}
