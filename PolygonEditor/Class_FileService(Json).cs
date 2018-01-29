using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PolygonEditor
{
    public class JsonFileService : IJsonFileService
    {
        public T LoadInstanceFromFile<T>(string filename)
        {
            string json_str;

            try
            {
                json_str = File.ReadAllText(filename);

                return JsonConvert.DeserializeObject<T>(json_str);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }
        }

        public object LoadShapesFromFile(string filename)
        {
            string json_str;

            try
            {
                json_str = File.ReadAllText(filename);

                char[] first_char = json_str.ToCharArray(0, 1);

                if (first_char[0] == '[')
                {
                    List<ShapeParams> paramsList = JsonConvert.DeserializeObject<List<ShapeParams>>(json_str);
                    return paramsList;
                }
                else if (first_char[0] == '{')
                {
                    ShapeParams param = JsonConvert.DeserializeObject<ShapeParams>(json_str);
                    return param;
                }
                else
                {
                    throw new System.ApplicationException("JsonFileService.LoadFromFile(): An unknown instance has been uploaded!");
                }

            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }
        }

        public void SaveToFile(string filename, object save_instance)
        {
            try
            {
                string json_str = JsonConvert.SerializeObject(save_instance, Formatting.Indented);

                File.WriteAllText(filename, json_str);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }
        }

    }
}