using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;
using System.Windows.Shapes;

namespace PolygonEditor
{
    public partial class MainWindow : Window
    {
        private void Button_DeleteSelectedShapes_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.SelectedShapeCount > 0)
            {
                if (System.Windows.MessageBox.Show("Are you sure you want to delete the selected shapes?", "Canvas Clearing", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    for (int i = Editor.SelectedShapeList.Count - 1; i > -1; i--)
                        Editor.shapesManager.RemoveShape(Editor.SelectedShapeList[i]);
                    Editor.ClearParamPanels();
                }
            }
        }

        private void Button_CanvasClearing_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.ShapeCount > 0)
            {
                if (System.Windows.MessageBox.Show("Are you sure you want to delete all the shapes?", "Canvas Clearing", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Editor.shapesManager.RemoveAllShapes();
                    Editor.ClearAllPanels();
                }
            }
        }

    }
}