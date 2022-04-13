namespace Notes_WebApp_Boomtown.Src.Utilities
{
    public static class Properties
    {
        private static Dictionary<string, string> propMap = null;

        public static readonly string NOTES_INDEX_FILE = "NOTES_INDEX_FILE";
        public static readonly string DATA_SOURCE_TYPE = "DATA_SOURCE_TYPE";
        public static readonly List<string> PROP_LIST = new List<string> (){ NOTES_INDEX_FILE, DATA_SOURCE_TYPE };


        /// <summary>
        /// Returns Internal Property Map
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetPropMap()
        {
            //Loading Props into internal map to reduce reads to file system
            if(propMap == null)
            {
                Dictionary<string, string> properties = GetPropMapFromFile("NotesAppProperties.json");
                propMap = new Dictionary<string, string>(properties);
            }
            return propMap;
        }

        /// <summary>
        /// Returns a Dictionary with Props from a given file 
        /// </summary>
        /// <param name="file"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns></returns>
        public static Dictionary<string, string> GetPropMapFromFile(string file)
        {
            return FileHandler.LoadJsonFile<Dictionary<string, string>>(file);
        }

        /// <summary>
        /// Gets Property from Internal Map
        /// </summary>
        /// <param name="propName"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <returns></returns>
        public static string GetProp(string propName)
        {
            return GetPropMap()[propName];
        }
    }
}
