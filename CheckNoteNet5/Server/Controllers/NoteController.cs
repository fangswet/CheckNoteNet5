using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : CheckNoteController
    {
        private readonly INoteService noteService;

        public NoteController(INoteService noteService)
        {
            this.noteService = noteService;
        }

        [AllowAnonymous]
        [Route("{id}")]
        public async Task<ActionResult<Note.Model>> Get(int id) => await ServiceAction(noteService.Get(id));

        [HttpPost]
        public async Task<ActionResult<Note.Model>> Add(Note.Input note) => await ServiceAction(noteService.Add(note));
    }
}
