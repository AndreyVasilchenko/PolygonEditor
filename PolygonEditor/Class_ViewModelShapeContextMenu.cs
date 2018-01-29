//using System;
using System.Windows;

namespace PolygonEditor
{
    /// <summary>
    /// Класс-модель определяющий поведение контекстного меню для фигуры
    /// </summary>
    public class ShapeContextMenuViewModel
    {
        private ShapeBase _shape;                                                           //  Обслуживаемая фигура
        private EditorModel _Editor;                                                        //  Редактор в котром происходит обслуживание

        public ShapeContextMenuViewModel(EditorModel editor, ShapeBase shape)
        {
            _shape = shape;
            
            _Editor = editor;
        }


        //--- команда для переименования фигуры
        private UserCommand shapeRename;
        public UserCommand ShapeRename
        {
            get { return shapeRename ?? (shapeRename = new UserCommand(obj => 
                    {
                        Window_SetName win = new Window_SetName(_shape.ShapeName, "RenameShape");  //  создаем окно для редактирования имени фигуры
                        win.Owner = App.Current.MainWindow;                                 //  это окно привязываем к окну приложения
                        if (win.ShowDialog() == true)                                       //  если есть желание измнить название, то
                        {
                            _shape.Rename(win.data_model.InstanceName);                     //      переименовываем фигуру
                        } 
                    }));
                }
        }

        //--- команда для удаления фигуры
        private UserCommand shapeDelete;
        public UserCommand ShapeDelete
        {
            get { return shapeDelete ?? (shapeDelete = new UserCommand(obj => 
                    {
                        if (_shape == null || _Editor == null)                              //  Если не определены фигура для удаления или редектор, в котором этоо будет происходить
                            return;                                                         //      выходим

                        if(_Editor.IsMouseDraws)
                        {
                            MessageBox.Show("Finish drawing the shape first!");
                            return;
                        }
                        if (MessageBox.Show("Are you sure you want to delete the " + _shape.ShapeName + "?", "Shape Deleting", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            _Editor.shapesManager.RemoveShape(_shape);                      //  Удаляем фигуру
                            _Editor.ClearParamPanels();                                     //  Очищаем панель ввода-просмотра параметров фигуры
                        }
                    }));
            }
        }

    }
}
