using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

namespace PolygonEditor
{
    public partial class MainWindow : Window
    {
        //--- Реакция Редактора на событие - курсор мыши зашел в пространство над панелью для рисования
        private void EditorPane_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Editor.ControlPanel != null)
                Editor.ControlPanel.Focus();                                                      //  Желания рисовать новую фигуру - нет
        }

        //--- Реакция Редактора на событие - курсор мыши покинул пространство над панелью для рисования
        private void EditorPane_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Editor.CursorPos = new Point(0, 0);                                                          //  Теперь местоположение курсора - неизвестно, для "красоты" - обнуляем
        }

        //--- Реакция Редактора на событие - произошло движение курсора мыши над панелью для рисования
        private void EditorPane_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Editor.CursorPos = e.GetPosition(Editor.MainPanel);                                         //  Сообщаем Редакору новые координаты расположения курсора мыши

            Editor.IsPressedForBeginDraws = false;                                                      //  Желания рисовать новую фигуру - нет
        }

        //--- Реакция Редактора на событие - над панелью для рисования отпущена ранее нажатая левая кнопка мыши
        private void EditorPane_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Editor.EditorMode == EDITOR_MODE.DrawNew)                                               //  Если редактор в режиме создания новых фигур
                if (Editor.IsPressedForBeginDraws)                                                      //      Если у пользователя есть желание начать рисовать мышью, то
                {
                    Editor.IsPressedForBeginDraws = false;                                              //          обнуляем флаг желания рисовать и 
                    try                                                                                 //          пытаемся 
                    {
                        Editor.shapesManager.CreateNewShapeWithMouse(e.GetPosition(Editor.MainPanel));  //              создать новую фигуру с помошью мыши
                    }
                    catch (ApplicationException ex)
                    {
                        MessageBox.Show(ex.Message, "Error!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "An Error not previously described!");
                    }
                }
        }

        //--- Реакция Редактора на событие - над панелью для рисования нажата левая кнопка мыши
        private void EditorPane_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if( Editor.EditorMode == EDITOR_MODE.DrawNew )                                              //  Если редактор в режиме создания новых фигур
                if (!Editor.IsMouseDraws)                                                               //      Если рисование мышью еще не началось, то
                    Editor.IsPressedForBeginDraws = true;                                               //          Фиксируем желание начать рисование мышью
        }

    }
}