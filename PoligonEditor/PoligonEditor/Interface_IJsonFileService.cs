using System;
using System.Collections.Generic;

namespace PolygonEditor
{
    public interface IJsonFileService
    {
        object LoadShapesFromFile(string filename);
        T LoadInstanceFromFile<T>(string filename);
        void SaveToFile(string filename, object save_instance);
    }
}