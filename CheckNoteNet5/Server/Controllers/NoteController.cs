using CheckNoteNet5.Server.Services.Extensions;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NoteController
    {
        private readonly INoteService noteService;

        public NoteController(INoteService noteService)
        {
            this.noteService = noteService;
        }

        [Route("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Note.Model>> Get(int id) => await noteService.Get(id).MapToAction();

        [HttpPost]
        public async Task<ActionResult<Note.Model>> Add(Note.Input note) => await noteService.Add(note).MapToAction();
    }
}
