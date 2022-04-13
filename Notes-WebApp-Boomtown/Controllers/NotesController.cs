using Microsoft.AspNetCore.Mvc;
using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Notes;

namespace Notes_WebApp_Boomtown.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {

        NoteHandler handler = NoteHandler.GetInstance();

        [HttpGet]
        public IEnumerable<NoteModel> GetAllNotes()
        {
            return handler.GetNotes();
        }

        [HttpGet("{id}")]
        public NoteModel GetNote(string id)
        {
            return handler.GetEntry(id);
        }

        [HttpPut]
        public int UpdateNote([FromBody] NoteModel note)
        {
            return handler.UpdateEntry(note);
        }

        [HttpPost]
        public int CreateNote([FromBody] NoteModel note)
        {
            return handler.CreateEntry(note);
        }

        [HttpDelete("{id}")]
        public int DeleteNote(string id)
        {
            return handler.DeleteEntry(id);
        }
    }
}