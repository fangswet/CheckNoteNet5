using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class QuestionService
    {
        private readonly CheckNoteContext dbContext;
        private readonly IMapper mapper;

        public QuestionService(CheckNoteContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        private Question.Model Convert(Question question)
        {
            switch (question.Type)
            {
                case Question.QuestionType.Binary:
                    return mapper.Map<Question.BinaryModel>(question);
                case Question.QuestionType.Input:
                case Question.QuestionType.Select:
                    return mapper.Map<Question.UnaryModel>(question);
            }

            return null;
        }

        public async Task<ServiceResult<Question.Model>> Add(Question.Input input)
        {
            var question = (Question)input;
            var model = Convert(question);

            if (model == null) return ServiceResult<Question.Model>.MakeError<ConflictError>();

            await dbContext.Questions.AddAsync(question);
            await dbContext.SaveChangesAsync();

            return ServiceResult.MakeOk(model);
        }

        public async Task<ServiceResult<Question.Model>> Get(int id)
        {
            var question = await dbContext.Questions.FindAsync(id);

            return ServiceResult.NullCheck(Convert(question));
        }
    }
}
