using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Diagnostics;

namespace PolygonEditor
{
    public partial class MainWindow : Window
    {
        public EditorModel Editor;              //  Экземляр модели редактора
        
        public MainWindow()
        {
            InitializeComponent();

            Editor = new EditorModel();
            this.DataContext = Editor;

            // Загрузим обработчики событий для главного окна приложения
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //--- Инициализируем исходные данные и холсты на которых редактор будет рисовать фигуры и контролы 
            Editor.Init(_EditorGrid, EditorPane, DrawingPane, ControlPane, ParametersPanel);

            //--- Сообщение в лог о запуске приложения
            Log("Application Loaded");                                          
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Editor.ClearAllPanels();                                //  Очищаем все холсты для показа "прощания"
            Editor.dataBase.CloseDB();                              //  Производим закрытие БД
            
                                    //-- В канве визуализации приглашающего логотипа изменим отображение текста с мульти-заполнения на единичное
                                    LogoBrush.Viewport = new Rect(0, 0, 1, 1);
                                    LogoBrush.TileMode = TileMode.None;

                                    //--- Созадаем анимацию при выходе из приложения
                                    DoubleAnimation animation_1 = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(2)));
                                    Storyboard.SetTarget(animation_1, RectBrush);
                                    Storyboard.SetTargetProperty(animation_1, new PropertyPath("Opacity"));

                                    DoubleAnimation animation_2 = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(2)));
                                    Storyboard.SetTarget(animation_2, geoText);
                                    Storyboard.SetTargetProperty(animation_2, new PropertyPath("Opacity"));

                                    Storyboard storyboard = new Storyboard();
                                    storyboard.Children.Add(animation_1);
                                    storyboard.Children.Add(animation_2); 
            
                                    storyboard.Begin();

                                    //-- Чтобы была возможность досмотреть анимацию до конца(по времени анимации), 
                                    //-- сделаем вызов невидимого диалогового окна с временем его "жизни" равным времени анимации
                                    Window_Pause wp = new Window_Pause(2000);
                                    wp.ShowDialog();
              
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Log("Application Closed");
        }


        public string log_path = "log.txt";      //  Название файла с лог-записями о событиях при работе приложения

        public void Log(string eventName)
        {
            using (StreamWriter logger = new StreamWriter(log_path, true))
            {
                logger.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " - " + eventName);
            }
        }



        private void AddField(object sender, RoutedEventArgs e)
        {
            if (sender is Button &&  ("+ Point".CompareTo( ((Button)sender).Content.ToString() ) == 0) )
            {
                Canvas SenderParent = (Canvas)LogicalTreeHelper.GetParent((Button)sender);  //      Ищем панель в которой находится кнопка "-Point"

                if (SenderParent != null)                                                   //          Если нашли,
                {
                    if (SenderParent.Children.Count > 46)                                   //              Если полей слишком много ( 20*2(tbox+label)+6(other) )
                    {
                        MessageBox.Show("There may be a maximum of 20 Points!");
                        return;                                                             //                  выходим
                    }

                    if (Editor.EditorMode == EDITOR_MODE.DrawNew)                           //              Если редактор находится в режиме создания новой фигуры, то
                    {
                        Editor.AddTextFieldToParamPanel(SenderParent);                      //                  Добавляем поле ввода координат
                    }
                    else                                                                    //              Иначе(режим редактрование)
                    {
                        //Editor.AddTextFieldToParamPanel(SenderParent);                      //                  Добавляем поле ввода координат
                        Editor.SelectedShapeForEdit.AddTextFieldToParamPanel();             //                  Добавляем поле ввода координат к редактируемой фигуре
                    }
                }
                else
                    MessageBox.Show("MainWindow.AddField(): Isn't find ParentCanvas for button" + ((Button)sender).Name, "For Programmer...");
            }
            else
                MessageBox.Show("MainWindow.AddField(): Is invoked by " + sender.ToString(), "For Programmer...");
        }

        private void DeleleField(object sender, RoutedEventArgs e)
        {
            if (sender is Button && ("- Point".CompareTo(((Button)sender).Content.ToString()) == 0) )
            {
                Canvas SenderParent = (Canvas)LogicalTreeHelper.GetParent((Button)sender);  //      Ищем панель в которой находится кнопка "-Point"

                if (SenderParent != null)                                                   //          Если нашли,
                {
                    if (SenderParent.Children.Count < 13)                                   //              Если удалять уже больше нельзя,
                    {
                        return;                                                             //                  просто выходим
                    }

                    if (Editor.EditorMode == EDITOR_MODE.DrawNew)                           //              Если редактор находится в режиме создания новой фигуры, то
                    {
                        Editor.RemoveLastTextFieldFromParamPanel(SenderParent);             //                  Удаляем последнее поле ввода координат с панели
                    }
                    else                                                                    //              Иначе(режим редактрование)
                    {
//                        Editor.RemoveLastTextFieldFromParamPanel(SenderParent);             //                  Удаляем последнее поле ввода координат с панели
                        Editor.SelectedShapeForEdit.RemoveLastTextFieldFromParamPanel();    //                  удаляем последнее поле ввода координат у редактируемой фигуры
                    }
                }
                else
                    MessageBox.Show("MainWindow.DeleleField(): Isn't find ParentCanvas for button" + ((Button)sender).Name, "For Programmer...");
            }
            else
                MessageBox.Show("MainWindow.DeleleField(): Is invoked by " + sender.ToString(), "For Programmer...");
        }

        private void ActionForFields(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Panel SenderParent = (Panel)((Button)sender).Parent;                        //  Идентифицируем панель на которой была нажата кнопка вызвавшая эту функцию
                
                if (SenderParent != null)                                                   //  Если это удалось сделать, то
                {
                    List<TextBox> l_tb = new List<TextBox>();                               //      Создаем и заполняем список полей содержащих координаты создаваемой фигуры
                    foreach (object obj in SenderParent.Children)
                        if (obj is TextBox)
                            l_tb.Add((TextBox)obj);
                    try                                                                     //    ПОПЫТКА...
                    {
                        if (Editor.EditorMode == EDITOR_MODE.DrawNew)                       //      Если редактор находится в режиме создания новой фигуры, то
                        {
                            Editor.shapesManager.CreateNewShape(l_tb);                      //          пытаемся создать фигуру по введеным координатам
                            Editor.ClearParamPanels();                                      //          очистим поля ввода
                        }
                        else                                                                //      Иначе(режим редактрование)
                        {
                            Editor.SelectedShapeForEdit._List_tBox = l_tb;                  //          передаем редактируемой фигуре список текст-боксов
                            Editor.SelectedShapeForEdit.UpdateShapeGeometryByTextCoords();  //          пытаемся изменить геометрию фигуры на основании введенных координат
                        }
                    }
                    catch (ApplicationException ex)
                    {
                        MessageBox.Show(ex.Message, "Error!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "MainWindow.ActionForFields(): An Error not previously described! ");
                    }
                }
                else
                    MessageBox.Show("MainWindow.ActionForFields(): Isn't find ParentPanel for button" + ((Button)sender).Name, "For Programmer...");
            }
            else
                MessageBox.Show("MainWindow.ActionForFields(): Is invoked by " + sender.ToString(), "For Programmer...");
        }

        private void MenuItem_Unselect_Click(object sender, RoutedEventArgs e)
        {
            Editor.shapesManager.UnselectAllShapes();
        }

        private void CommandBinding_Executed_SelectAll(object sender, ExecutedRoutedEventArgs e)
        {
            Editor.shapesManager.SelectAllShapes();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Editor != null && Editor.ShapeCount > 0);
        }

        private void CommandBinding_Executed_Help(object sender, ExecutedRoutedEventArgs e)
        {
            Process[] procs = Process.GetProcessesByName("hh");
            if (procs != null && procs.Count() > 0) //"процесс существует"
            {
                ;//SetForegroundWindow((int)(procs[0].MainWindowHandle));//нормально развернутое
            }
            else
            { //"процесс не существует"
                try
                {
                    Process HelpProcess = new Process();
                    HelpProcess.StartInfo.ErrorDialog = true;
                    HelpProcess.StartInfo.FileName = "PolygonEditorHelp.chm";
                    HelpProcess.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Log(ex.Message);
                }
            }
        }

        private void CommandBinding_Executed_Print(object sender, ExecutedRoutedEventArgs e)
        {
            try                                                                                 //  ПОПЫТКА...
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)                                           // Получить от пользователя "куда печатать" и если получили,
                {
                    printDialog.PrintVisual(DrawingPane, "Распечатываем содежимое DrawPanel");  //      печатаем все что на панели рисования
                }
            }
            catch (Exception ex)                                                                //  В случае "непредвиденных" обстоятельств на этапе ПОПЫТКИ,
            {
                MessageBox.Show("Exception: " + ex.Message, "PrintError!");                      //      сообщаем о них
            }
        }

        private void CommandBinding_Executed_Properties(object sender, ExecutedRoutedEventArgs e)
        {
            Window_Properties win = new Window_Properties(Editor.prop_service.GetPropertiesClone());    //  Создаем диалоговое окне редактирования свойств
            win.Owner = App.Current.MainWindow;                                                         //  это окно привязываем к окну приложения
            if (win.ShowDialog() == true)                                                               //  Если диалог закончился подтверждением изменений
            {
                Editor.prop_service.UpdateProperties(win.data_model.Properties);                        //      Обновим свойсва редактора
            }
        }

        private void CommandBinding_CanExecute_SelectAll(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Editor != null && Editor.EditorMode != EDITOR_MODE.Edit);
        }

        private void RButton_modePolygonOperations_Unchecked(object sender, RoutedEventArgs e)
        {
            Editor.shapesManager.SetVisible(OPERATION_OBJECT_TYPE.Subject, true);
            Editor.shapesManager.SetVisible(OPERATION_OBJECT_TYPE.Clipper, true);
            Editor.shapesManager.SetVisible(OPERATION_OBJECT_TYPE.Result, true);
            Editor.shapesManager.UnsetPolygonType();
        }

    }
}