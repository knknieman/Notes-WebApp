using Microsoft.AspNetCore.Mvc;
using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Notes;

namespace Notes_WebApp_Boomtown.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {

        private readonly ILogger<NotesController> _logger;
        NoteHandler handler = NoteHandler.GetInstance();
        public NotesController(ILogger<NotesController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IEnumerable<NoteMetadata> GetAllNotes()
        {
            return handler.GetNotes();
        }

        [HttpGet("{id}")]
        public NoteMetadata GetNote(string id)
        {
            return handler.GetEntry(id);
        }

        [HttpPut]
        public int UpdateNote([FromBody] NoteMetadata note)
        {
            return handler.UpdateEntry(note);
        }

        [HttpPost]
        public int CreateNote([FromBody] NoteMetadata note)
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