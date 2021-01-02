using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services.ServiceItems
{
    public class NoteServiceItem
    {
        public Note Entity { get; init; }
        public Note.Model Model { get; init; }
        private readonly CheckNoteContext dbContext;
        private readonly QuestionService questionService;

        public NoteServiceItem(Note note, Note.Model model, CheckNoteContext dbContext, QuestionService questionService)
        {
            Entity = note;
            Model = model;
            this.dbContext = dbContext;
            this.questionService = questionService;
        }

        public async Task<ServiceResult<Question.Model>> AddQuestion(Question.Input input)
        {
            var question = (Question)input;
            Entity.Questions.Add(question);
            await dbContext.SaveChangesAsync();

            return await questionService.Get(question.Id);
        }
    }

    // check identity somehow authenticate
    public class NoteServiceItemFactory
    {
        private readonly CheckNoteContext dbContext;
        private readonly IMapper mapper;
        private readonly QuestionService questionService;

        public NoteServiceItemFactory(CheckNoteContext dbContext, IMapper mapper, QuestionService questionService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.questionService = questionService;
        }

        public async Task<NoteServiceItem> Get(int id)
        {
            var query = dbContext.Notes.Where(n => n.Id == id);
            var note = await query.FirstOrDefaultAsync();
            if (note == null) return null;

            var model = await mapper.ProjectTo<Note.Model>(query).FirstOrDefaultAsync();

            return new NoteServiceItem(note, model, dbContext, questionService);
        }
    }
}
