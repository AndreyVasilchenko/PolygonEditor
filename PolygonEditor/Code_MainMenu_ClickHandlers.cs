using System;
//using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
//using System.Text;
//using System.Drawing;
using System.Diagnostics;
//using System.Threading;
//using System.Reflection;
using System.Windows;
//using System.Runtime.InteropServices;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Media.Animation;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.IO;
//using System.Windows.Threading;

namespace PolygonEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();                           // закрытие окна приложения
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            new Window_About().ShowDialog();        // вывод на экран окна "О"
        }

    }
}