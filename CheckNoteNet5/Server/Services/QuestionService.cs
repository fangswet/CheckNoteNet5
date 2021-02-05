using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Dtos;
using CheckNoteNet5.Shared.Models.Inputs;
using CheckNoteNet5.Shared.Services;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class QuestionService : ServerService
    {
        private readonly CheckNoteContext dbContext;
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public QuestionService(CheckNoteContext dbContext, IAuthService authService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.authService = authService;
            this.mapper = mapper;
        }

        private QuestionModel MapToModel(Question question)
        {
            switch (question.Type)
            {
                case QuestionType.Binary:
                    return mapper.Map<BinaryQuestionModel>(question);
                case QuestionType.Input:
                case QuestionType.Select:
                    return mapper.Map<UnaryQuestionModel>(question);
            }

            return null;
        }

        public async Task<ServiceResult> Remove(int id)
        {
            var question = await dbContext.Questions.FindAsync(id);

            if (question == null)
                return NotFound();

            if (question.Note.AuthorId != authService.GetUserId())
                return Unauthorized();

            dbContext.Questions.Remove(question);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        public async Task<ServiceResult<QuestionModel>> Get(int id)
        {
            var question = await dbContext.Questions.FindAsync(id);

            return NullCheck(MapToModel(question));
        }

        public async Task<ServiceResult<bool>> Answer(int id, AnswerAttempt answer)
        {
            var question = await dbContext.Questions.FindAsync(id);
            var correct = false;

            if (question == null)
                return NotFound<bool>();

            else if (question.Type == QuestionType.Binary && answer.Correct != null)
                correct = question.Answer((bool)answer.Correct);

            else if (question.Type == QuestionType.Select && answer.Answers != null)
                correct = question.Answer(answer.Answers);

            else if (question.Type == QuestionType.Input && answer.TextAnswers != null)
                correct = question.Answer(answer.TextAnswers);

            return Ok(correct);
        }
    }
}
