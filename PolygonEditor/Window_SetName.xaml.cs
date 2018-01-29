using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PolygonEditor
{
    public partial class Window_SetName : Window
    {
        public Model_SetName data_model;

        public Window_SetName(string set_name, string win_title)
        {
            InitializeComponent();

            data_model = new Model_SetName();
            this.DataContext = data_model;
            data_model.InstanceName = set_name;
            this.Title = win_title;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }

    public class Model_SetName : ViewModelBase
    {
        private string _instanceName;                             
        public string InstanceName
        {
            get { return _instanceName; }
            set
            {
                _instanceName = value;
                OnPropertyChanged("InstanceName");
            }
        }

        public Model_SetName(string name= "")
        {
            InstanceName = name;
        }
    }
}
