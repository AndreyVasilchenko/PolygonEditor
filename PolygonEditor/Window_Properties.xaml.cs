using System;
using System.Security;
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
    /// <summary>
    /// Interaction logic for Window_Properties.xaml
    /// </summary>
    public partial class Window_Properties : Window
    {
        public Model_Properties data_model;

        public System.Security.SecureString Password
        {
            get
            {
                return _PasswordBox.SecurePassword;
            }
        }


        public Window_Properties(CProperties properties)
        {
            InitializeComponent();

            data_model = new Model_Properties(properties);
            this.DataContext = data_model;

            if (data_model.Properties.RemoteUsed)
                RadioButton_RemoteHost.IsChecked = true;
            else
                RadioButton_LocalHost.IsChecked = true;

            if (_PasswordBox.SecurePassword.IsReadOnly())
                MessageBox.Show("Password is read-only!");
            else if(_PasswordBox.SecurePassword == null )
                MessageBox.Show("Password is null!");
            else
            {
                if (properties.UserPassword != null && properties.UserPassword.Length > 0)
                {
                    _PasswordBox.Password = properties.UserPassword;
                    _PasswordTextBox.Text = _PasswordBox.Password;
                    PasswordHelper.SetIsUpdating(_PasswordBox, true);
                    PasswordHelper.SetPassword(_PasswordBox, _PasswordBox.Password);
                    PasswordHelper.SetIsUpdating(_PasswordBox, false);
                }
            }
            //this.UpdateLayout();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            data_model.Properties.RemoteUsed = (bool)RadioButton_RemoteHost.IsChecked;
            data_model.Properties.UserPassword = _PasswordBox.Password;
        }

        private void SelectPath_Click(object sender, RoutedEventArgs e)
        {
            try                                                                                 //  ПОПЫТКА...
            {
                FileServiceDialog fileServiceDialog = new FileServiceDialog();
                if (fileServiceDialog.ChooseFolder(data_model.FileLocation, "Choose Default files location...") == true)//  Если получили от пользователя "путь" к месту хранения файлов,
                {
                    data_model.FileLocation = fileServiceDialog.FilePath;                       //      запоминаем его
                }
            }
            catch (Exception ex)                                                                //  В случае "непредвиденных" обстоятельств на этапе ПОПЫТКИ,
            {
                MessageBox.Show("Exception: " + ex.Message, "SelectPath_Click()");              //      сообщаем о них
            }
        }
    }

    public class Model_Properties : ViewModelBase
    {
        public CProperties Properties;

        private bool _selfIntersectionCheck;
        public bool SelfIntersectionCheck
        {
            get { return _selfIntersectionCheck; }
            set
            {
                _selfIntersectionCheck = value;
                Properties.SelfIntersectionCheck = value;
                OnPropertyChanged("SelfIntersectionCheck");
            }
        }

        private int _subjectStrokeWidth;                            //  Толщина линии рисования фигуры
        public int SubjectStrokeWidth
        {
            get { return _subjectStrokeWidth; }
            set
            {
                _subjectStrokeWidth = value;
                Properties.opProperty[0].StrokeWeight = value;
                OnPropertyChanged("SubjectStrokeWidth");
            }
        }

        private Color _subjectStrokeColor;                            //  Цвет линии рисования фигуры
        public Color SubjectStrokeColor
        {
            get { return _subjectStrokeColor; }
            set
            {
                _subjectStrokeColor = value;
                Properties.opProperty[0].StrokeColor = value.ToString();
                OnPropertyChanged("SubjectStrokeColor");
            }
        }

        private Color _subjectFillColor;                              //  Цвет закраски тела фигуры
        public Color SubjectFillColor
        {
            get { return _subjectFillColor; }
            set
            {
                _subjectFillColor = value;
                Properties.opProperty[0].FillColor = value.ToString();
                OnPropertyChanged("SubjectFillColor");
            }
        }

        private int _clipperStrokeWidth;                            //  Толщина линии рисования фигуры
        public int ClipperStrokeWidth
        {
            get { return _clipperStrokeWidth; }
            set
            {
                _clipperStrokeWidth = value;
                Properties.opProperty[1].StrokeWeight = value;
                OnPropertyChanged("ClipperStrokeWidth");
            }
        }

        private Color _clipperStrokeColor;                            //  Цвет линии рисования фигуры
        public Color ClipperStrokeColor
        {
            get { return _clipperStrokeColor; }
            set
            {
                _clipperStrokeColor = value;
                Properties.opProperty[1].StrokeColor = value.ToString();
                OnPropertyChanged("ClipperStrokeColor");
            }
        }

        private Color _clipperFillColor;                              //  Цвет закраски тела фигуры
        public Color ClipperFillColor
        {
            get { return _clipperFillColor; }
            set
            {
                _clipperFillColor = value;
                Properties.opProperty[1].FillColor = value.ToString();
                OnPropertyChanged("ClipperFillColor");
            }
        }

        private int _resultStrokeWidth;                            //  Толщина линии рисования фигуры
        public int ResultStrokeWidth
        {
            get { return _resultStrokeWidth; }
            set
            {
                _resultStrokeWidth = value;
                Properties.opProperty[2].StrokeWeight = value;
                OnPropertyChanged("ResultStrokeWidth");
            }
        }

        private Color _resultStrokeColor;                            //  Цвет линии рисования фигуры
        public Color ResultStrokeColor
        {
            get { return _resultStrokeColor; }
            set
            {
                _resultStrokeColor = value;
                Properties.opProperty[2].StrokeColor = value.ToString();
                OnPropertyChanged("ResultStrokeColor");
            }
        }

        private Color _resultFillColor;                              //  Цвет закраски тела фигуры
        public Color ResultFillColor
        {
            get { return _resultFillColor; }
            set
            {
                _resultFillColor = value;
                Properties.opProperty[2].FillColor = value.ToString();
                OnPropertyChanged("ResultFillColor");
            }
        }

        private string _fileLocation;
        public string FileLocation
        {
            get { return _fileLocation; }
            set
            {
                _fileLocation = value;
                Properties.FileLocation = value;
                OnPropertyChanged("FileLocation");
            }
        }

        private string _userLogin;
        public string UserLogin
        {
            get { return _userLogin; }
            set
            {
                _userLogin = value;
                Properties.UserLogin = value;
                OnPropertyChanged("UserLogin");
            }
        }

        private string _userPassword;
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                _userPassword = value;
                Properties.UserPassword = value;
                OnPropertyChanged("UserPassword");
            }
        }

        private string _remoteHostAddress;
        public string RemoteHostAddress
        {
            get { return _remoteHostAddress; }
            set
            {
                _remoteHostAddress = value;
                Properties.RemoteHost = value;
                OnPropertyChanged("RemoteHostAddress");
            }
        }

        private string _localHostAddress;
        public string LocalHostAddress
        {
            get { return _localHostAddress; }
            set
            {
                _localHostAddress = value;
                Properties.LocalHost = value;
                OnPropertyChanged("LocalHostAddress");
            }
        }

        private string _remotePort;
        public string RemotePort
        {
            get { return _remotePort; }
            set
            {
                _remotePort = value;
                Properties.RemotePort = value;
                OnPropertyChanged("RemotePort");
            }
        }

        private string _localPort;
        public string LocalPort
        {
            get { return _localPort; }
            set
            {
                _localPort = value;
                Properties.LocalPort = value;
                OnPropertyChanged("LocalPort");
            }
        }

        private bool _remoteUsed;
        public bool RemoteUsed
        {
            get { return _remoteUsed; }
            set
            {
                _remoteUsed = value;
                Properties.RemoteUsed = value;
                OnPropertyChanged("RemoteUsed");
            }
        }


        public Model_Properties(CProperties properties)
        {
            Properties = properties;

            SelfIntersectionCheck = properties.SelfIntersectionCheck;

            SubjectStrokeWidth = properties.opProperty[0].StrokeWeight;
            SubjectStrokeColor = (Color)ColorConverter.ConvertFromString(properties.opProperty[0].StrokeColor);
            SubjectFillColor = (Color)ColorConverter.ConvertFromString(properties.opProperty[0].FillColor);
            
            ClipperStrokeWidth = properties.opProperty[1].StrokeWeight;
            ClipperStrokeColor = (Color)ColorConverter.ConvertFromString(properties.opProperty[1].StrokeColor);
            ClipperFillColor = (Color)ColorConverter.ConvertFromString(properties.opProperty[1].FillColor);
            
            ResultStrokeWidth = properties.opProperty[2].StrokeWeight;
            ResultStrokeColor = (Color)ColorConverter.ConvertFromString(properties.opProperty[2].StrokeColor);
            ResultFillColor = (Color)ColorConverter.ConvertFromString(properties.opProperty[2].FillColor);

            FileLocation = properties.FileLocation;
            
            UserLogin = properties.UserLogin;
            UserPassword = properties.UserPassword;
            
            RemoteHostAddress = properties.RemoteHost;
            RemotePort = properties.RemotePort;
            LocalHostAddress = properties.LocalHost;
            LocalPort = properties.LocalPort;
            RemoteUsed = properties.RemoteUsed;
        }
    }
}
