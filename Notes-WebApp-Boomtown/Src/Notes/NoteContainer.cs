using Notes_WebApp_Boomtown.Models;

namespace Notes_WebApp_Boomtown.Src.Notes
{
    public abstract class NoteContainer
    {
        /// <summary>
        /// Performs Save
        /// </summary>
        /// <exception cref="IOException"></exception>
        public abstract void Save();

        /// <summary>
        /// Performs Load
        /// </summary>
        /// <exception cref="IOException"></exception>
        public abstract void Load();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract NoteMetadata Get(string id);

        /// <summary>
        /// Performs Create 
        /// </summary>
        /// <exception cref="IOException"></exception>
        public abstract void Create(NoteMetadata note);

        /// <summary>
        /// Performs Update 
        /// </summary>
        /// <exception cref="IOException"></exception>
        public abstract void Update(NoteMetadata note);

        /// <summary>
        /// Performs Save
        /// </summary>
        /// <exception cref="IOException"></exception>
        public abstract void Delete(string id);

        /// <summary>
        /// Returns All NotesMetadata as a list 
        /// </summary>
        /// <returns></returns>
        public abstract List<NoteMetadata> ToList();
    }
}
