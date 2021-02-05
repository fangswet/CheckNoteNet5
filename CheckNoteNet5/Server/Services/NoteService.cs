using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Dtos;
using CheckNoteNet5.Shared.Models.Inputs;
using CheckNoteNet5.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class NoteService : ServerService, INoteService
    {
        private readonly CheckNoteContext dbContext;
        private readonly IAuthService authService;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly QuestionService questionService;
        private static readonly Regex whitespace = new Regex(@"\s+");

        public NoteService(CheckNoteContext dbContext, IAuthService authService, IMapper mapper, IMemoryCache cache, QuestionService questionService)
        {
            this.dbContext = dbContext;
            this.authService = authService;
            this.mapper = mapper;
            this.cache = cache;
            this.questionService = questionService;
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

        public async Task<ServiceResult<NoteModel>> Add(NoteInput input)
        {
            authService.AssertAuthentication();

            var tags = new List<Tag>();
            var note = mapper.Map<Note>(input);

            note.Title = whitespace.Replace(note.Title.Trim(), " ");
            if (note.Title.Length < 10) return BadRequest<NoteModel>(); // format this somehow to convention

            if (note.ParentId != null)
            {
                var parent = await dbContext.Notes.FindAsync(note.ParentId);

                if (parent == null) return BadRequest<NoteModel>();

                if (parent.AuthorId != authService.GetUserId())
                    return Unauthorized<NoteModel>();

                note.RootId = parent.IsRoot ? note.ParentId : parent.RootId;
                tags.AddRange(parent.Tags); // parents tags can be excluded here
            }
            else note.IsRoot = true;

            tags.AddRange(await ConvertTags(note.Tags));
            note.Tags = tags;
            note.AuthorId = authService.GetUserId();

            await dbContext.Notes.AddAsync(note);
            await dbContext.SaveChangesAsync();

            note.Url = whitespace.Replace(note.Title, "-").ToLowerInvariant() + "-" + note.Id;
            await dbContext.SaveChangesAsync();

            return await Get(note.Id);
        }

        public async Task<ServiceResult<NoteModel>> Get(int id)
            => await cache.GetOrCreateAsync($"note/{id}",
                async _ =>
                {
                    var query = dbContext.Notes.Where(n => n.Id == id);
                    var note = await mapper.ProjectTo<NoteModel>(query).FirstOrDefaultAsync();
                    var result = NullCheck(note);
                    cache.Set($"note/{note.Url}", result);

                    return result;
                });

        public async Task<ServiceResult<NoteModel>> Get(string title) 
            => await cache.GetOrCreateAsync($"note/{title}", 
                async _ =>
                {
                    var query = dbContext.Notes.Where(n => n.Url == title);
                    var note = await mapper.ProjectTo<NoteModel>(query).FirstOrDefaultAsync();

                    return NullCheck(note);
                });

        public async Task<ServiceResult> Remove(int id)
        {
            var note = await dbContext.Notes.FindAsync(id);

            if (note == null) return NotFound();

            if (authService.GetUserId() != note.Author.Id) return Unauthorized();

            dbContext.Notes.Remove(note);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        public async Task<ServiceResult<NoteModel>> Update(int id, NoteInput input)
        {
            var note = await dbContext.Notes.Where(n => n.Id == id).FirstOrDefaultAsync();

            if (note == null) return NotFound<NoteModel>();

            if (note.AuthorId != authService.GetUserId()) return Unauthorized<NoteModel>(); // check if ? fails condition or makes null == UserId (wrong)

            note.Title = input.Title;
            note.Content.Text = input.Text;
            note.Description = input.Description;

            await dbContext.SaveChangesAsync();
            return await Get(id);
        }

        public async Task<ServiceResult<QuestionModel>> AddQuestion(int id, QuestionInput input)
        {
            var note = await dbContext.Notes.FindAsync(id);
            
            if (note == null) return NotFound<QuestionModel>();

            if (note.AuthorId != authService.GetUserId())
                return Unauthorized<QuestionModel>();

            var question = mapper.Map<Question>(input);
            question.RootId = note.IsRoot ? id : note.RootId;

            note.Questions.Add(question);
            await dbContext.SaveChangesAsync();

            return await questionService.Get(question.Id);
        }

        // add limits
        public async Task<ServiceResult<List<NoteEntry>>> List(string title = null)
        {
            var query = title == null 
                ? dbContext.Notes.AsQueryable()
                : dbContext.Notes.Where(n => n.Title.Contains(title));

            var notes = await mapper.ProjectTo<NoteEntry>(query).ToListAsync();

            return Ok(notes);
        }
    }
}
