using Newtonsoft.Json;

namespace Notes_WebApp_Boomtown.Src.Utilities
{
    public class FileHandler
    {
        /// <summary>
        /// Uses generics to load Json Files into given objects
        /// </summary>
        /// <param name="filePath">Path to File on Disk</param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns></returns>
        public static T? LoadJsonFile<T>(string filePath)
        {

            string json = GetFileContent(filePath);
            dynamic jsonDataList = JsonConvert.DeserializeObject<T>(json);
            return jsonDataList;
            
        }

        /// <summary>
        /// Will Serialize an Object into a Json String
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetJsonFromObject(dynamic item)
        {
            return JsonConvert.SerializeObject(item);
        }

        /// <summary>
        /// Helper Function to Get Contents From Files
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns>File Content</returns>
        public static string GetFileContent(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }

        /// <summary>
        /// Helper Function for saving to File
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void WriteToFile (string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
    }
}
