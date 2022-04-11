using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Utilities;

namespace Notes_WebApp_Boomtown.Src.Notes
{
    public sealed class NoteHandler
    {
        private static NoteHandler? instance;
        private static string? manifestPath;
        private Dictionary<string,NoteMetadata> notesMetadataDict; 

        private NoteHandler()
        {
            notesMetadataDict = new Dictionary<string, NoteMetadata>();
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
                instance.notesMetadataDict = instance.LoadNoteMetadataDict(manifestPath);
            }
            return instance;
        }

        /// <summary>
        /// Helper Function for Loading NoteMetadataDict
        /// </summary>
        /// <param name="indexPathOnDisk"></param>
        /// <returns></returns>
        private Dictionary<string,NoteMetadata> LoadNoteMetadataDict(string indexPathOnDisk)
        {
            Dictionary<string,NoteMetadata> returnDict;
            try
            {
                returnDict = FileHandler.LoadNoteJson(indexPathOnDisk);
            }catch(Exception ex)
            {
                returnDict = new Dictionary<string, NoteMetadata>();
            }
            return returnDict;
        }

        /// <summary>
        /// Saves NoteMetadataDict to File 
        /// NOTE: This is called anytime there is a modification to the MetadataDict Object
        /// </summary>
        /// <param name="dictToSave"></param>
        /// <returns>returns True if Sucessful</returns>
        private bool SaveMetadataDict(Dictionary<string, NoteMetadata> dictToSave)
        {
            string json = FileHandler.GetJsonFromObject(dictToSave);
            bool success = FileHandler.WriteToFile(manifestPath, json);

            return success;
        }

        /// <summary>
        /// Returns a copy of the noteMetadataList 
        /// NOTE: A copy to returned to prevent external actors from modifying the collection
        /// </summary>
        /// <returns>copy of MetadataList</returns>
        public Dictionary<string, NoteMetadata> GetNoteMetadataList()
        {
            //Return a copy of the list, prevents modification externally 
            return new Dictionary<string, NoteMetadata>(this.notesMetadataDict);
        }

        /// <summary>
        /// Returns a complete NoteMetadata Object for a given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Complete NoteMetadata Object </returns>
        public NoteMetadata GetEntryComplete(string id)
        {
            NoteMetadata completeNote;
            if (this.NoteExists(id)) {
                //Need to load file from disk here, and return metadata
                completeNote = this.notesMetadataDict[id];

            }
            else
            {
                throw new KeyNotFoundException("Unable to find Key: " + id);
            }
            return completeNote;
        }

        /// <summary>
        /// Performs update of passed NoteMetadata object
        /// Updates the Last Modified time
        /// Updates the notesMetadataList 
        /// Then Saves NotesMetadataList
        /// </summary>
        /// <param name="note"></param>
        /// <returns>StatusCode 200 for Success, 400 For Failure</returns>
        public int UpdateEntryComplete(NoteMetadata note)
        {
            int returnCode;
            if(this.NoteExists(note))
            {
                note.LastModified = DateTime.Now.ToString();
                this.notesMetadataDict[note.NoteID] = note;
                returnCode = this.SaveMetadataDict(this.notesMetadataDict) ? StatusCodes.Status200OK: StatusCodes.Status400BadRequest;
            }
            else
            {
                returnCode = StatusCodes.Status400BadRequest;
            }
            return returnCode;
        }

        /// <summary>
        /// Creates new Entry from give NoteMetadata Object 
        /// </summary>
        /// <param name="note"></param>
        /// <returns>StatusCode 200 for Success, 400 For Failure</returns>
        public int CreateEntryComplete(NoteMetadata note)
        {
            string noteID = this.GetNewID();
            note.NoteID = noteID;
            note.CreationDate = DateTime.Now.ToString();
            this.notesMetadataDict.Add(noteID,note);

            //Call UpdateEntryComplete to handle last bit of Entry Save
            return this.UpdateEntryComplete(note);
        }

        /// <summary>
        /// Performs complete Delete of Note Entry 
        /// </summary>
        /// <param name="noteID"></param>
        /// <returns>StatusCode 200 for Success, 400 For Failur</returns>
        public int DeleteEntry(string noteID)
        {
            int returnCode;
            if (NoteExists(noteID)){
                this.notesMetadataDict.Remove(noteID);
                returnCode = this.SaveMetadataDict(this.notesMetadataDict) ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
            }
            else
            {
                returnCode = StatusCodes.Status400BadRequest;
            }
            return returnCode;
        }

        /// <summary>
        /// Helper function for verifying note existence from note Object 
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        private bool NoteExists(NoteMetadata note)
        {
            return this.NoteExists(note.NoteID);
        }

        /// <summary>
        /// Helper function for verifying note existence from ID number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool NoteExists(string id)
        {
            return this.notesMetadataDict.ContainsKey(id);
        }

        /// <summary>
        /// Helper function for generating GUID.
        /// NOTE: This function should live in a common utility file
        /// </summary>
        /// <returns></returns>
        private string GetNewID()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
