﻿using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// important to note that service methods are only concerned with presenting a usable api 
// and do not explicitly check for scenarios outside of usual (predetermined) usage
// e.g the NoteService.Add method will not provide back any helpful information when trying to attach a nonexistent Tag id
namespace CheckNoteNet5.Server.Services
{
    public class NoteService : INoteService
    {
        private readonly CheckNoteContext dbContext;
        private readonly IAuthService authService;
        private readonly IMapper mapper;
        private readonly QuestionService questionService;
        private readonly CacheService cacheService;

        public NoteService(CheckNoteContext dbContext, IAuthService authService, IMapper mapper, QuestionService questionService, CacheService cacheService)
        {
            this.dbContext = dbContext;
            this.authService = authService;
            this.mapper = mapper;
            this.questionService = questionService;
            this.cacheService = cacheService;
        }

        public async Task<ServiceResult<Note.Model>> Add(Note.Input input)
        {
            var note = input.Convert();

            if (note.ParentId != null)
            {
                var parent = await dbContext.Notes.FindAsync(note.ParentId);
                if (parent == null && parent.AuthorId != authService.GetUserId())
                    return ServiceResult<Note.Model>.MakeError<UnauthorizedError>();
            }

            note.Tags = await ConvertTags(note.Tags);
            note.AuthorId = authService.GetUserId();

            await dbContext.Notes.AddAsync(note);
            await dbContext.SaveChangesAsync();

            return await Get(note.Id);
        }

        private async Task<Tag[]> ConvertTags(ICollection<Tag> tags)
        {
            var conversion = tags.Select(async t =>
            {
                if (t.Id != default)
                {
                    var tag = await dbContext.Tags.FindAsync(t.Id);
                    if (tag != null) return tag;
                }
                return t;
            });

            return await Task.WhenAll(conversion);
        }

        private async Task<ServiceResult<Note.Model>> GetRaw(int id)
        {
            var query = dbContext.Notes.Where(n => n.Id == id);
            var note = await mapper.ProjectTo<Note.Model>(query).FirstOrDefaultAsync();

            return ServiceResult.NullCheck(note);
        }

        public async Task<ServiceResult<Note.Model>> Get(int id) => await cacheService.Get($"note/{id}", _ => GetRaw(id));

        public async Task<ServiceResult> Remove(int id)
        {
            // perhaps replace lazy loading with this (check if projectto still works)
            var note = await dbContext.Notes.Where(n => n.Id == id).Include(n => n.Author).FirstOrDefaultAsync();
            if (authService.GetUserId() != note.Author.Id) return ServiceResult.MakeError<UnauthorizedError>();

            dbContext.Notes.Remove(note);
            await dbContext.SaveChangesAsync();

            return ServiceResult.MakeOk();
        }

        public async Task<ServiceResult<Note.Model>> Update(int id, Note.Input input)
        {
            var note = await dbContext.Notes.Where(n => n.Id == id).FirstOrDefaultAsync();
            var result = new ServiceResult<Note.Model>();

            if (note == null) return result.Error<NotFoundError>();

            if (note?.Author?.Id != authService.GetUserId()) return result.Error<UnauthorizedError>(); // check if ? fails condition or makes null == UserId (wrong)

            note.Title = input.Title;
            note.Content.Text = input.Text;
            note.Description = input.Description;

            await dbContext.SaveChangesAsync();
            return await Get(id);
        }

        public async Task<ServiceResult<Question.Model>> AddQuestion(int id, Question.Input input)
        {
            var note = await dbContext.Notes.FindAsync(id);
            var question = (Question)input;

            note.Questions.Add(question);
            await dbContext.SaveChangesAsync();

            return await questionService.Get(question.Id);
        }

        // add limits
        public async Task<ServiceResult<List<Note.Entry>>> List(string title = null)
        {
            var query = title == null 
                ? dbContext.Notes.AsQueryable()
                : dbContext.Notes.Where(n => n.Title.Contains(title));

            var notes = await mapper.ProjectTo<Note.Entry>(query).ToListAsync();

            return ServiceResult.MakeOk(notes);
        }
    }
}
