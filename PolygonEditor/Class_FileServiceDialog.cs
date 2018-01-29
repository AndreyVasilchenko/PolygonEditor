using System.Windows;
using System.Windows.Forms;

namespace PolygonEditor
{
    /// <summary>
    /// Класс содержащий сервисные фунции для определения пользователем места хранения и названия файлов при их загруке или сохранении
    /// </summary>
    public class FileServiceDialog : IFileServiceDialog
    {
        public string FilePath { get; set; }                                                //  Строка с адресом хранения файла

        //--- Определение места загузки
        public bool LoadFromFile(string directory = "", string ext_filter = "")                                                          
        {
            bool ret = false;

            OpenFileDialog openFileDialog = new OpenFileDialog() { InitialDirectory = directory, Filter = ext_filter };//  Создаем экземпляр диалогового окна
            if (openFileDialog.ShowDialog() == DialogResult.OK)                             //  Если пользватель сообщил, где хранятся файл и как он называется,
            {
                FilePath = openFileDialog.FileName;                                         //      запоминаем это
                return true;                                                                //      и выходим с результатом - все ОК
            }

            openFileDialog.Dispose();                                                       //     освобождаем ресурсы окна диалога
            return ret;                                                                     //  выходим с отрицательным результатом
        }

        //--- Определение места сохранения
        public bool SaveToFile(string directory = "", string file_name = "", string ext_filter = "")
        {
            bool ret = false;

            SaveFileDialog saveFileDialog = new SaveFileDialog() { InitialDirectory = directory, FileName = file_name, Filter = ext_filter };  //  Создаем экземпляр диалогового окна
            if (saveFileDialog.ShowDialog() == DialogResult.OK)                             //  Если пользватель сообщил, где будем сохранять файл и как он будет называться,
            {
                FilePath = saveFileDialog.FileName;                                         //      запоминаем это
                ret = true;                                                                 //      и выходим с результатом - все ОК
            }

            saveFileDialog.Dispose();                                                       //     освобождаем ресурсы окна диалога
            return ret;                                                                     //  выходим с отрицательным результатом
        }

        //---  Определение папки для загрузки или записи
        public bool ChooseFolder(string def_path = "", string discription="")
        {
            bool ret = false;

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog() { SelectedPath = def_path };   //  Создаем экземпляр диалогового окна
            if (discription != "")
                folderBrowserDialog.Description = discription;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)                        //  Если пользватель сообщил, где должен храниться файл,
            {
                FilePath = folderBrowserDialog.SelectedPath;                                //     запоминаем это
                ret= true;                                                                  //     и выходим с результатом - все ОК
            }
            
            folderBrowserDialog.Dispose();                                                  //     освобождаем ресурсы окна диалога
            return ret;
        }

    }
}