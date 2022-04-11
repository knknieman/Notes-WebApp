namespace Notes_WebApp_Boomtown.Src.Utilities
{
    public sealed class Properties
    {
        private static Properties instance = null;
        private Dictionary<string, string> propMap;

        public static readonly string NOTES_DIR = "NOTES_DIR";
        public static readonly List<string> PROP_LIST = new List<string> (){ NOTES_DIR };

        private Properties()
        {
            propMap = new Dictionary<string, string>();

        }

        public static Dictionary<string, string> GetPropMap()
        {
            if(instance == null)
            {
                instance = new Properties();
                Dictionary<string, string> properties = FileHandler.LoadJsonFile<Dictionary<string, string>>("NotesAppProperties.json");
                foreach (string prop in PROP_LIST)
                {
                    string propValue = properties[prop];
                    instance.propMap.Add(prop, propValue);
                }
            }


            //Need to get props to return
            return instance.propMap;
        }

        public static string GetProp(string propName)
        {
            //Returns Empty String, probably need to throw if we can't find value 
            return GetPropMap().GetValueOrDefault(propName, "");
        }
    }
}
