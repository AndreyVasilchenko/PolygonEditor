namespace PolygonEditor
{
    public interface IFileServiceDialog
    {
        string FilePath { get; set; }                                                           //  путь к выбранному файлу

        bool LoadFromFile(string directory = "", string ext_filter = "");                       //  открытие файла
        bool SaveToFile(string directory = "", string file_name = "", string ext_filter = "");  //  сохранение файла
        bool ChooseFolder(string def_path = "", string discription = "");                       //  определение папки для загрузки или записи
    }
}
