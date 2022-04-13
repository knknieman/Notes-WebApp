using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Utilities;

namespace Notes_WebApp_Boomtown.Src.Notes
{
    public class NoteContainerJson : NoteContainer
    {
        private string indexFile;
        private Dictionary<string, NoteModel> notesMetadataDict;
        private Object accessLock;

        public NoteContainerJson()
        {
            notesMetadataDict = new Dictionary<string, NoteModel>();
            indexFile = Properties.GetProp(Properties.NOTES_INDEX_FILE);
            accessLock = new Object();
            this.Load();
        }

        public override void Load()
        {
            lock (accessLock)
            {
                try
                {
                    this.notesMetadataDict = FileHandler.LoadJsonFile<Dictionary<string, NoteModel>>(this.indexFile);
                }
                catch (IOException ex)
                {
                    //Return new Dictionary on instance of fileNotFoundException
                    this.notesMetadataDict = new Dictionary<string, NoteModel>();
                }
            }
        }

        public override void Save()
        {
            lock (accessLock)
            {
                string json = FileHandler.GetJsonFromObject(this.notesMetadataDict);
                FileHandler.WriteToFile(indexFile, json);
            }
        }

        public override NoteModel Get(string id)
        {
            if (this.NoteExists(id))
            {
                return this.notesMetadataDict[id];
            }
            else
            {
                throw new KeyNotFoundException("Unable to find Key: " + id);
            }
        }

        public override void Create(NoteModel note)
        {
            string noteID = this.GetNewID();
            note.NoteID = noteID;
            note.CreationDate = DateTime.Now.ToString();
            this.notesMetadataDict.Add(noteID, note);

            //Call UpdateEntry to handle last bit of Entry Save
            this.Update(note);
        }

        public override void Update(NoteModel note)
        {
            if (this.NoteExists(note))
            {
                note.LastModified = DateTime.Now.ToString();
                this.notesMetadataDict[note.NoteID] = note;
                this.Save();
            }
            else
            {
                throw new KeyNotFoundException("Unable to find ID " + note.NoteID);
            }

        }

        public override void Delete(string id)
        {
            if (NoteExists(id))
            {
                this.notesMetadataDict.Remove(id);
                this.Save();
            }
            else
            {
                throw new KeyNotFoundException("Unable to find ID " + id);
            }
        }

        public override List<NoteModel> ToList()
        {
            return new List<NoteModel> (this.notesMetadataDict.Values);
        }

        /// <summary>
        /// Helper function for verifying note existence from note Object 
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        private bool NoteExists(NoteModel note)
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
        /// Returns GUID
        /// </summary>
        /// <returns></returns>
        private string GetNewID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
