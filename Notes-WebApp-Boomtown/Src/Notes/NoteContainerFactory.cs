namespace Notes_WebApp_Boomtown.Src.Notes
{
    public class NoteContainerFactory
    {
        public static NoteContainer GetInstance(string dataSource)
        {
            NoteContainer container;
            switch (dataSource)
            {
                case "JSON":
                    container = new NoteContainerJson();
                    break;
                case "ORACLE":
                    //Would Implement ORACLE IMPLEMENTATION OF NoteContainer
                    //Set to NoteContainerJson now to keep compiler happy
                    container = new NoteContainerJson();
                    break;
                default:
                    throw new ArgumentException("DATA TYPE ARGUEMENT NOT ACCEPTED");

            }
            return container;
        }
    }
}
