using Newtonsoft.Json;
using Notes_WebApp_Boomtown.Models;

namespace Notes_WebApp_Boomtown.Src.Utilities
{
    public class FileHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">Path to File on Disk</param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns></returns>
        public static dynamic LoadJsonFile(string filePath)
        {

            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                dynamic jsonDataList = JsonConvert.DeserializeObject<List<dynamic>>(json);
                return jsonDataList;
            }
        }

        public static Dictionary<string, NoteMetadata> LoadNoteJson(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                Dictionary<string, NoteMetadata> jsonDataList = JsonConvert.DeserializeObject<Dictionary<string, NoteMetadata>>(json);
                return jsonDataList;
            }
        }


        public static string GetJsonFromObject(dynamic item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public static string GetFileContent(string filePath)
        {
            string fileText = System.IO.File.ReadAllText(filePath);
            return fileText;
        }

        public static Boolean WriteToFile (string filePath, string content)
        {
            Boolean success;
            try
            {
                File.WriteAllText(filePath, content);
                success = true;
            }catch(Exception ex)
            {
                success = false;
            }
            return true;
        }
    }
}
