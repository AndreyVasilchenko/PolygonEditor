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
using MongoDB.Bson;

namespace PolygonEditor
{
    /// <summary>
    /// Interaction logic for Window_LoadInstance.xaml
    /// </summary>
    public partial class Window_LoadInstance : Window
    {
        private EditorModel _Editor;
        public string collection_name;
        
        private BsonObjectId shape_id;
        private BsonObjectId group_id;
        private BsonObjectId operation_id;
        private string shape_name;
        private string group_name;
        private string operation_name;
        
        public BsonObjectId instance_id;
        public string instance_name;

        public Window_LoadInstance(EditorModel editor)
        {
            _Editor = editor;

            InitializeComponent();
        }

        private void WinLoadInsance_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_Editor.dataBase.isOpen || !_Editor.dataBase.CheckPing(1500))
            {
                MessageBox.Show("At the moment there is no access to the database!");
                this.DialogResult = false;
                this.Close();
                return;
            }

            BindGrid("Shapes");
            BindGrid("ShapeGroups");
            BindGrid("PolygonOperations");
        }

        public void BindGrid(string colection_name)
        {
            var context = _Editor.dataBase.GetNameList(colection_name);
            switch( colection_name )
            {
                case "Shapes":
                    ShapesGrid.ItemsSource = context;
                    break;
                case "ShapeGroups":
                    ShapeGroupsGrid.ItemsSource = context;
                    break;
                case "PolygonOperations":
                    PolygonOperationsGrid.ItemsSource = context;
                    break;
            }
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is nameInfo)
                {
                    nameInfo val = (nameInfo)e.AddedItems[0];
                    collection_name = ((TabItem)CollectionSelector.SelectedItem).Header.ToString();
                    switch( collection_name )
                    {
                        case "Shapes":
                            shape_id = val.Id; 
                            shape_name= val.Name;
                            break;
                        case "ShapeGroups":
                            group_id = val.Id; 
                            group_name= val.Name;
                            break;
                        case "PolygonOperations":
                            operation_id = val.Id; 
                            operation_name= val.Name;
                            break;
                    }
                }
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentInstance();
            if (instance_id == null)
            {
                MessageBox.Show("You did not select the object to download!");
            }
            else
            {
                this.DialogResult = true;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentInstance();
            if (instance_id == null)
            {
                this.DialogResult = false;
                MessageBox.Show("You did not select an object to delete!");
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to delete \""+instance_name+"\" ?", "Attention !!!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if(_Editor.dataBase.DeleteInstanceById(collection_name, instance_id))
                        BindGrid(collection_name);
                }
            }
        }

        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentInstance();
            if (instance_id == null)
            {
                this.DialogResult = false;
                MessageBox.Show("You did not select an object to delete!");
            }
            else
            {
                Window_SetName win = new Window_SetName(instance_name, "Rename \""+instance_name+"\" to...");   //  создаем окно для редактирования названия операции над полигонами
                win.Owner = App.Current.MainWindow;                                                             //  это окно привязываем к окну приложения
                if (win.ShowDialog() == true)                                                                   //  если операцию над полигонами сохраняем, то
                {
                    if(_Editor.dataBase.RenameInstanceById(collection_name, instance_id, win.data_model.InstanceName))
                        BindGrid(collection_name);
                }
            }
        }

        private void UpdateCurrentInstance()
        {
            instance_id = null;
            instance_name = "";

            collection_name = ((TabItem)CollectionSelector.SelectedItem).Header.ToString();
            switch (collection_name)
            {
                case "Shapes":
                    instance_id = shape_id;
                    instance_name = shape_name;
                    break;
                case "ShapeGroups":
                    instance_id = group_id;
                    instance_name = group_name;
                    break;
                case "PolygonOperations":
                    instance_id = operation_id;
                    instance_name = operation_name;
                    break;
            }
        }

        private void Grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateCurrentInstance();

            if (instance_id != null)
                this.DialogResult = true;
        }

    }
}
