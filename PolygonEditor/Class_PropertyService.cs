using System;
using System.Security;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonEditor
{
    public class CProperties
    {
        public string FileLocation { get; set; }
        public string LocalDataBaseLocation { get; set; }
        public string LocalHost { get; set; }
        public string LocalPort { get; set; }
        public bool RemoteUsed { get; set; }
        public string RemoteHost { get; set; }
        public string RemotePort { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        
        public bool SelfIntersectionCheck { get; set; }
        public OperationShapeProperty[] opProperty { get; set; }

        public CProperties()
        {
            opProperty = new OperationShapeProperty[3];
            for (int i = 0; i < 3; i++)
            {
                opProperty[i] = new OperationShapeProperty();
            }
        }
    }

    public class PropertiesService
    {
        public CProperties properties;
        private string setupFileName= "PolygonEditor.set";

        public PropertiesService()
        {
            properties = new CProperties();

            // Задаем свойства "по умолчанию"
            
            properties.SelfIntersectionCheck = true;

            properties.opProperty[(int)OPERATION_OBJECT_TYPE.Subject].StrokeWeight = 2;
            properties.opProperty[(int)OPERATION_OBJECT_TYPE.Subject].StrokeColor = "#FF000000";
            properties.opProperty[(int)OPERATION_OBJECT_TYPE.Subject].FillColor = "#3C000000";
            properties.opProperty[(int)OPERATION_OBJECT_TYPE.Clipper].StrokeWeight = 2;
            properties.opProperty[(int)OPERATION_OBJECT_TYPE.Clipper].StrokeColor = "#FF00BFFF";
            properties.opProperty[(int)OPERATION_OBJECT_TYPE.Clipper].FillColor = "#5100BFFF";
            properties.opProperty[(int)OPERATION_OBJECT_TYPE.Result].StrokeWeight = 2;
            properties.opProperty[(int)OPERATION_OBJECT_TYPE.Result].StrokeColor = "#FF3CB371";
            properties.opProperty[(int)OPERATION_OBJECT_TYPE.Result].FillColor = "#4700FF00";
            
            properties.FileLocation = Application.StartupPath + @"\File";

            properties.UserLogin = "testUser";
            properties.UserPassword = "testUser";

            properties.RemoteUsed = true;
            properties.RemoteHost = "109.248.33.124";
            properties.RemotePort = "37067";
            properties.LocalHost = "127.0.0.1";// "mongodb://localhost"
            properties.LocalPort = "37067";
            properties.LocalDataBaseLocation = Application.StartupPath + @"\MongoDB\data";
        }

        public void InitFromSetupFile()
        {
            string setupFullPath = Application.StartupPath + @"\" + setupFileName;

            JsonFileService jfs = new JsonFileService();

            if (File.Exists(setupFullPath))                                                       //  Если файл установок существует, то
            {
                try
                {
                    CProperties instance = jfs.LoadInstanceFromFile<CProperties>(setupFullPath);  //      загружаем его содержимое

                    if (instance != null)
                    {
                        if( ((CProperties)instance).SelfIntersectionCheck.GetType().Equals(typeof(bool)) )
                        {
                            properties.SelfIntersectionCheck = ((CProperties)instance).SelfIntersectionCheck;
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            if (((CProperties)instance).opProperty[i].StrokeWeight.GetType().Equals(typeof(int)))
                                properties.opProperty[i].StrokeWeight = ((CProperties)instance).opProperty[i].StrokeWeight;
                            if (((CProperties)instance).opProperty[i].StrokeColor.GetType().Equals(typeof(string)))
                                properties.opProperty[i].StrokeColor = ((CProperties)instance).opProperty[i].StrokeColor;
                            if (((CProperties)instance).opProperty[i].FillColor.GetType().Equals(typeof(string)))
                                properties.opProperty[i].FillColor = ((CProperties)instance).opProperty[i].FillColor;
                        }

                        if (((CProperties)instance).FileLocation != null)
                        {
                            if (Directory.Exists(((CProperties)instance).FileLocation))
                            {
                                properties.FileLocation = ((CProperties)instance).FileLocation;
                            }
                            else
                            {
                                SavePropities();
                                //MessageBox.Show("The path specified in the files location property is missing, the default path will be used!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Could not read property of the shape-files location path, default path will be used!");
                        }
                        if (!Directory.Exists(properties.FileLocation))                          //      Если нет дефолтной директории сохранения файлов
                        {
                            Directory.CreateDirectory(properties.FileLocation);                  //          создаем ее
                        }

                        if (((CProperties)instance).UserLogin.GetType().Equals(typeof(string)))
                        {
                            properties.UserLogin = ((CProperties)instance).UserLogin;
                        }

                        if (((CProperties)instance).UserPassword.GetType().Equals(typeof(string)))
                        {
                            properties.UserPassword = ((CProperties)instance).UserPassword;
                        }

                        if (((CProperties)instance).RemoteUsed.GetType().Equals(typeof(bool)))
                        {
                            properties.RemoteUsed = ((CProperties)instance).RemoteUsed;
                        }

                        if (((CProperties)instance).RemoteHost.GetType().Equals(typeof(string)))
                        {
                            properties.RemoteHost = ((CProperties)instance).RemoteHost;
                        }

                        if (((CProperties)instance).RemotePort.GetType().Equals(typeof(string)))
                        {
                            properties.RemotePort = ((CProperties)instance).RemotePort;
                        }

                        if (((CProperties)instance).LocalHost.GetType().Equals(typeof(string)))
                        {
                            properties.LocalHost = ((CProperties)instance).LocalHost;
                        }

                        if (((CProperties)instance).LocalPort.GetType().Equals(typeof(string)))
                        {
                            properties.LocalPort = ((CProperties)instance).LocalPort;
                        }

     /********** Прописать этот блок если получится сконфигурировать портативный сервер MongoDB ***************
                        if (((CProperties)instance).LocalDataBaseLocation != null)
                        {
                            if (Directory.Exists(((CProperties)instance).LocalDataBaseLocation))
                            {
                                properties.LocalDataBaseLocation = ((CProperties)instance).LocalDataBaseLocation;
                            }
                            else
                            {
                                MessageBox.Show("The path specified in the files location property is missing, the default path will be used!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Could not read property of the shape-files location path, default path will be used!");
                        }
                        /*if (!Directory.Exists(properties.LocalDataBaseLocation))                      //      Если нет дефолтной директории для БД
                        {
                            Directory.CreateDirectory(properties.LocalDataBaseLocation);              //          создаем ее
      ****************                  }************************************************************************/
                    }
                    else
                    {
                        //MessageBox.Show("Could not read Properties from the Setup File, Default Properties will be used!!");
                        SavePropities();
                    }
                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show("PropertiesService.InitFromSetupFile()" + ex.Message, "ApplicationException!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("PropertiesService.InitFromSetupFile()" + ex.Message, "Exeption!");
                }
            }
        }


        public void SavePropities()
        {
            JsonFileService jfs = new JsonFileService();

            jfs.SaveToFile(Application.StartupPath + "\\" + setupFileName, properties);         //      записываем существующие установки
        }

        public CProperties GetPropertiesClone()
        {
            CProperties prop_clone = new CProperties();

            prop_clone.SelfIntersectionCheck = properties.SelfIntersectionCheck;

            for (int i = 0; i < 3; i++)
            {
                prop_clone.opProperty[i].StrokeWeight = properties.opProperty[i].StrokeWeight-1;
                prop_clone.opProperty[i].StrokeColor = properties.opProperty[i].StrokeColor;
                prop_clone.opProperty[i].FillColor = properties.opProperty[i].FillColor;
            }

            prop_clone.FileLocation = properties.FileLocation;

            prop_clone.UserLogin = properties.UserLogin;
            prop_clone.UserPassword = properties.UserPassword;

            prop_clone.RemoteUsed = properties.RemoteUsed;
            prop_clone.RemoteHost = properties.RemoteHost;
            prop_clone.RemotePort = properties.RemotePort;
            prop_clone.LocalHost = properties.LocalHost;
            prop_clone.LocalPort = properties.LocalPort;
            prop_clone.LocalDataBaseLocation = properties.LocalDataBaseLocation;

            return prop_clone;
        }

        public void UpdateProperties(CProperties new_prop)
        {
            properties.SelfIntersectionCheck = new_prop.SelfIntersectionCheck;
            
            for (int i = 0; i < 3; i++)
            {
                properties.opProperty[i].StrokeWeight = new_prop.opProperty[i].StrokeWeight+1;
                properties.opProperty[i].StrokeColor = new_prop.opProperty[i].StrokeColor;
                properties.opProperty[i].FillColor = new_prop.opProperty[i].FillColor;
            }

            properties.FileLocation = new_prop.FileLocation;
            
            properties.UserLogin = new_prop.UserLogin;
            properties.UserPassword = new_prop.UserPassword;

            properties.RemoteUsed = new_prop.RemoteUsed;
            properties.RemoteHost = new_prop.RemoteHost;
            properties.RemotePort = new_prop.RemotePort;
            properties.LocalHost = new_prop.LocalHost;
            properties.LocalPort = new_prop.LocalPort;
            properties.LocalDataBaseLocation = new_prop.LocalDataBaseLocation;

            SavePropities();
        }

    }
}
