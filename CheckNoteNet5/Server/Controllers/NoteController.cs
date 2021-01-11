using CheckNoteNet5.Server.Services.Extensions;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Inputs;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService noteService;

        public NoteController(INoteService noteService)
        {
            this.noteService = noteService;
        }

        [Route("{id}")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<NoteModel>> Get(int id) => await noteService.Get(id).MapToAction();

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<NoteEntry>>> List(string title) => await noteService.List(title).MapToAction();

        [HttpPost]
        public async Task<ActionResult<NoteModel>> Add(NoteInput input) => await noteService.Add(input).MapToAction();

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> Remove(int id) => await noteService.Remove(id).MapToAction();
        
        [Route("{id}")]
        [HttpPatch]
        public async Task<ActionResult<NoteModel>> Update(int id, NoteInput input) => await noteService.Update(id, input).MapToAction();
    }
}
