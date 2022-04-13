using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Utilities;

namespace Notes_WebApp_Boomtown.Src.Notes
{
    public sealed class NoteHandler
    {
        private static NoteHandler? instance;

        private NoteContainer noteContainer;
        private NoteHandler()
        {
            string dataSourceType = Properties.GetProp(Properties.DATA_SOURCE_TYPE);
            noteContainer = NoteContainerFactory.GetInstance(dataSourceType);
        }

        /// <summary>
        /// Singleton instance of NoteHandler; the exposed class the controllers/tests call 
        /// </summary>
        /// <returns>NoteHandler instance</returns>
        public static NoteHandler GetInstance()
        {
            if(instance == null)
            {
                instance = new NoteHandler();
            }
            return instance;
        }

        /// <summary>
        /// Returns a the list of NoteMetadata Objects 
        /// </summary>
        /// <returns>copy of NoteMetadataDict</returns>
        public List<NoteModel> GetNotes()
        {
            //Return a copy of the list, prevents modification externally 
            return this.noteContainer.ToList();
        }

        /// <summary>
        /// Returns a complete NoteMetadata Object for a given ID
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <returns>Complete NoteMetadata Object </returns>
        public NoteModel GetEntry(string id)
        {
            return this.noteContainer.Get(id);
        }

        /// <summary>
        /// Performs update of NoteMetadata object
        /// </summary>
        /// <param name="note"></param>
        /// <returns>Http StatusCode</returns>
        public int UpdateEntry(NoteModel note)
        {
            int returnCode;
            try
            {
                this.noteContainer.Update(note);
                returnCode = StatusCodes.Status200OK;
            }
            catch(KeyNotFoundException)
            {
                returnCode = StatusCodes.Status404NotFound;
            }
            catch (IOException)
            {
                returnCode = StatusCodes.Status500InternalServerError;
            }
            return returnCode;
        }

        /// <summary>
        /// Creates new Entry from give NoteMetadata Object 
        /// </summary>
        /// <param name="note"></param>
        /// <returns>Http StatusCode</returns>
        public int CreateEntry(NoteModel note)
        {
            int returnCode;
            try
            {
                this.noteContainer.Create(note);
                returnCode = StatusCodes.Status200OK;
            }
            catch (IOException)
            {
                returnCode = StatusCodes.Status500InternalServerError;
            }
            return returnCode;
        }

        /// <summary>
        /// Performs complete Delete of Note Entry 
        /// </summary>
        /// <param name="noteID"></param>
        /// <returns>Http StatusCode</returns>
        public int DeleteEntry(string noteID)
        {
            int returnCode;
            try
            {
                this.noteContainer.Delete(noteID);
                returnCode = StatusCodes.Status200OK;
            }
            catch (KeyNotFoundException)
            {
                returnCode = StatusCodes.Status404NotFound;
            }
            catch (IOException)
            {
                returnCode = StatusCodes.Status500InternalServerError;
            }
            return returnCode;
        }

    }
}
