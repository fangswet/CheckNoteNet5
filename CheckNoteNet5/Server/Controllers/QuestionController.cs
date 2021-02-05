using CheckNoteNet5.Server.Services;
using CheckNoteNet5.Server.Services.Extensions;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService questionService;

        public QuestionController(QuestionService questionService)
        {
            this.questionService = questionService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<QuestionModel>> Get(int id) => await questionService.Get(id).MapToAction();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Remove(int id) => await questionService.Remove(id).MapToAction();

        [HttpPost("{id:int}")]
        public async Task<ActionResult<bool>> Answer(int id, AnswerAttempt answer) 
            => await questionService.Answer(id, answer).MapToAction();
    }
}
