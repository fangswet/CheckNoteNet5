using AutoMapper;
using CheckNoteNet5.Shared.Models;
using Microsoft.EntityFrameworkCore;
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

        public Question.Model Convert(Question question)
        {
            switch (question.Type)
            {
                case Question.QuestionType.Binary:
                    return mapper.Map<Question.BinaryModel>(question);
                case Question.QuestionType.Input:
                case Question.QuestionType.Select:
                    return mapper.Map<Question.UnaryModel>(question);
            }

            // custom exception
            throw new System.Exception(); 
        }

        public async Task<Question.Model> Add(Question question)
        {
            await dbContext.Questions.AddAsync(question);
            await dbContext.SaveChangesAsync();

            return Convert(question);
        }

        public async Task<Question.Model> Get(int id)
        {
            var question = await dbContext.Questions.FirstOrDefaultAsync(q => q.Id == id);

            return question != null ? Convert(question) : null;
        }
    }
}
