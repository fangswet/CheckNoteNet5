using CheckNoteNet5.Server.Services;
using CheckNoteNet5.Server.Services.Extensions;
using CheckNoteNet5.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/{id:int}/[action]")]
    public class TagController : ControllerBase
    {
        private readonly TagService tagService;

        public TagController(TagService tagService)
        {
            this.tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NoteEntry>>> Notes(int id) => await tagService.GetNotes(id).MapToAction();
    }
}
