using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Dtos;
using CheckNoteNet5.Shared.Models.Inputs;
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

        private QuestionModel Convert(Question question)
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

        public async Task<ServiceResult<QuestionModel>> Add(QuestionInput input)
        {
            var question = mapper.Map<Question>(input);
            var model = Convert(question);

            if (model == null) return ServiceResult<QuestionModel>.MakeError<ConflictError>();

            await dbContext.Questions.AddAsync(question);
            await dbContext.SaveChangesAsync();

            return ServiceResult.MakeOk(model);
        }

        public async Task<ServiceResult<QuestionModel>> Get(int id)
        {
            var question = await dbContext.Questions.FindAsync(id);

            return ServiceResult.NullCheck(Convert(question));
        }
    }
}
