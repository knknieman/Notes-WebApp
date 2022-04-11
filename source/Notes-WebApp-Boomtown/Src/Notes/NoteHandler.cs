

using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Utilities;

namespace Notes_WebApp_Boomtown.Src.Notes
{
    public sealed class NoteHandler
    {
        private static NoteHandler instance = null;
        private static string manifestPath;
        private Dictionary<string,NoteMetadata> notesMetadataList; 

        private NoteHandler()
        {
            notesMetadataList = new Dictionary<string, NoteMetadata>();
            manifestPath = "";
        }

        /// <summary>
        /// Singleton instance of NoteHandler; the exposed class the controllers/tests call 
        /// </summary>
        /// <returns>NoteHandler instance</returns>
        public static NoteHandler getInstance()
        {
            if(instance == null)
            {
                instance = new NoteHandler();

                //Instaniating Properties, getting Note Metadata List 
                manifestPath = Properties.GetProp(Properties.NOTES_DIR);
                instance.notesMetadataList = instance.LoadNoteMetadataList(manifestPath);
            }
            return instance;
        }

        private Dictionary<string,NoteMetadata> LoadNoteMetadataList(string indexPathOnDisk)
        {
            Dictionary<string,NoteMetadata> returnList;
            try
            {
                returnList = FileHandler.LoadNoteJson(indexPathOnDisk);
            }catch(Exception ex)
            {
                //Log Error, then return new List
                returnList = new Dictionary<string, NoteMetadata>();
            }
            return returnList;
        }

        private bool SaveMetadataList(Dictionary<string, NoteMetadata> listToSave)
        {
            string json = FileHandler.GetJsonFromObject(listToSave);
            bool success = FileHandler.WriteToFile(manifestPath, json);

            return success;
        }

        public Dictionary<string, NoteMetadata> GetNoteMetadataList()
        {
            //Return a copy of the list, prevents modification externally 
            return new Dictionary<string, NoteMetadata>(this.notesMetadataList);
        }


        public NoteMetadata GetEntryComplete(string id)
        {
            NoteMetadata completeNote;
            if (this.NoteExists(id)) {
                //Need to load file from disk here, and return metadata
                completeNote = this.notesMetadataList[id];

            }
            else
            {
                completeNote = new NoteMetadata();
            }
            return completeNote;
        }

        public int UpdateEntryComplete(NoteMetadata note)
        {
            int returnCode;
            if(this.NoteExists(note))
            {
                note.LastModified = DateTime.Now.ToString();
                this.notesMetadataList[note.NoteID] = note;
                returnCode = this.SaveMetadataList(this.notesMetadataList) ? StatusCodes.Status200OK: StatusCodes.Status400BadRequest;
            }
            else
            {
                returnCode = StatusCodes.Status400BadRequest;
            }
            return returnCode;
        }
        
        public int CreateEntryComplete(NoteMetadata note)
        {
            string noteID = this.GetNewID();
            note.NoteID = noteID;
            note.CreationDate = DateTime.Now.ToString();
            this.notesMetadataList.Add(noteID,note);
            return this.UpdateEntryComplete(note);
        }

        public int DeleteEntry(string noteID)
        {
            int returnCode;
            if (NoteExists(noteID)){
                this.notesMetadataList.Remove(noteID);
                returnCode = this.SaveMetadataList(this.notesMetadataList) ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
            }
            else
            {
                returnCode = StatusCodes.Status400BadRequest;
            }
            return returnCode;
        }

        private Boolean NoteExists(NoteMetadata note)
        {
            return this.notesMetadataList.ContainsKey(note.NoteID);
        }

        private Boolean NoteExists(string id)
        {
            return this.notesMetadataList.ContainsKey(id);
        }

        private string GetNewID()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
