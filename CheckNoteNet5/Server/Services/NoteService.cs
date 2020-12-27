using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class NoteService
    {
        private readonly CheckNoteContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly HttpContext httpContext;

        public NoteService(CheckNoteContext dbContext, UserManager<User> userManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.mapper = mapper;
            httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<Result<Note.Model>> Add(Note note)
        {
            //note.Author = await userManager.GetUserAsync(httpContext.User);
            note.AuthorId = 1;
            await dbContext.Notes.AddAsync(note);
            await dbContext.SaveChangesAsync();

            return await Get(note.Id);
        }

        public async Task<Result<Note.Model>> Get(int id)
        {
            var result = new Result<Note.Model>();
            var query = dbContext.Notes.Where(n => n.Id == id);
            var note = await mapper.ProjectTo<Note.Model>(query).FirstOrDefaultAsync();

            return note != null ? result.Ok(note) : result.Error<NotFoundError>(); // check if returning an implicit null form the controller is 404
        }
    }
}
