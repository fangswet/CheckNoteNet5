using CheckNoteNet5.Server.Services;
using CheckNoteNet5.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly CheckNoteContext dbContext;
        private readonly NoteService noteService;

        public NoteController(CheckNoteContext dbContext, NoteService noteService)
        {
            this.dbContext = dbContext;
            this.noteService = noteService;
        }

        [Route("{id}")]
        public async Task<ActionResult<Note.Model>> Get(int id) => await noteService.Get(id);

        [HttpPost]
        public async Task<ActionResult> Add(Note.Input note) => await noteService.Add(note);
    }
}
