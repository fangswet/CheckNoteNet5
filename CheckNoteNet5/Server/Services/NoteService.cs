﻿using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Auth;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class NoteService : INoteService
    {
        private readonly CheckNoteContext dbContext;
        private readonly AuthService authService;
        private readonly IMapper mapper;
        private readonly QuestionService questionService;

        public NoteService(CheckNoteContext dbContext, AuthService authService, IMapper mapper, QuestionService questionService)
        {
            this.dbContext = dbContext;
            this.authService = authService;
            this.mapper = mapper;
            this.questionService = questionService;
        }

        public async Task<ServiceResult<Note.Model>> Add(Note.Input input)
        {
            var unauthorized = ServiceResult<Note.Model>.MakeError<UnauthorizedError>();
            var userId = authService.UserId;
            if (userId == null) return unauthorized;

            Note note = input;

            if (note.ParentId != null)
            {
                var parent = await dbContext.Notes.FindAsync(note.ParentId);
                if (parent == null && parent.AuthorId != userId) return unauthorized;
            }

            note.AuthorId = (int)userId;

            await dbContext.Notes.AddAsync(note);
            await dbContext.SaveChangesAsync();

            return await Get(note.Id);
        }

        public async Task<ServiceResult<Note.Model>> Get(int id)
        {
            var query = dbContext.Notes.Where(n => n.Id == id);
            var note = await mapper.ProjectTo<Note.Model>(query).FirstOrDefaultAsync();

            return ServiceResult.NullCheck(note);
        }

        public async Task<ServiceResult> Remove(int id)
        {
            // perhaps replace lazy loading with this (check if projectto still works)
            var note = await dbContext.Notes.Where(n => n.Id == id).Include(n => n.Author).FirstOrDefaultAsync();
            if (authService.UserId != note.Author.Id) return ServiceResult.MakeError<UnauthorizedError>();

            dbContext.Notes.Remove(note);
            await dbContext.SaveChangesAsync();

            return ServiceResult.MakeOk();
        }

        public async Task<ServiceResult<Note.Model>> Update(int id, Note.Input input)
        {
            var note = await dbContext.Notes.Where(n => n.Id == id).FirstOrDefaultAsync();
            var result = new ServiceResult<Note.Model>();

            if (note == null) return result.Error<NotFoundError>();

            if (note?.Author?.Id != authService.UserId) return result.Error<UnauthorizedError>(); // check if ? fails condition or makes null == UserId (wrong)

            note.Title = input.Title;
            note.Content.Text = input.Text;
            if (input.Description != null) note.Description = input.Description;

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
