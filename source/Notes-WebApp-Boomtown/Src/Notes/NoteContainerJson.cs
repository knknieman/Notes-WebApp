using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Utilities;

namespace Notes_WebApp_Boomtown.Src.Notes
{
    public class NoteContainerJson : NoteContainer
    {
        private string indexFile;
        private Dictionary<string, NoteMetadata> notesMetadataDict;
        private Object accessLock;

        public NoteContainerJson()
        {
            notesMetadataDict = new Dictionary<string, NoteMetadata>();
            accessLock = new Object();
            indexFile = Properties.GetProp(Properties.NOTES_DIR);
            this.Load();
        }

        public override void Load()
        {
            lock (accessLock)
            {
                try
                {
                    this.notesMetadataDict = FileHandler.LoadJsonFile<Dictionary<string, NoteMetadata>>(this.indexFile);
                }
                catch (IOException ex)
                {
                    //Return new Dictionary on instance of fileNotFoundException
                    this.notesMetadataDict = new Dictionary<string, NoteMetadata>();
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

        public override NoteMetadata Get(string id)
        {
            if (this.NoteExists(id))
            {
                //Need to load file from disk here, and return metadata
                return this.notesMetadataDict[id];
            }
            else
            {
                throw new KeyNotFoundException("Unable to find Key: " + id);
            }
        }

        public override void Create(NoteMetadata note)
        {
            string noteID = this.GetNewID();
            note.NoteID = noteID;
            note.CreationDate = DateTime.Now.ToString();
            this.notesMetadataDict.Add(noteID, note);

            //Call UpdateEntry to handle last bit of Entry Save
            this.Update(note);
        }

        public override void Update(NoteMetadata note)
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

        public override List<NoteMetadata> ToList()
        {
            return new List<NoteMetadata> (this.notesMetadataDict.Values);
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


        private string GetNewID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
