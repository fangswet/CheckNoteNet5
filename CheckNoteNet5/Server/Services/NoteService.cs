using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Auth;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class NoteService : INoteService
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

        // currently only the properties included in Input type are validated
        // meaning if you allow for the input of the Note entity in a controller and pass it to this method
        // it will fuck up the application
        // consider extending the entities with validation logic to relieve services from the responsibility
        public async Task<ServiceResult<Note.Model>> Add(Note note)
        {
            var user = await userManager.GetUserAsync(httpContext.User);

            // dont check for no user here 
            if (user == null) return ServiceResult<Note.Model>.MakeError<BadRequestError>();

            if (note.ParentId != null)
            {
                var parent = await dbContext.Notes.FindAsync(note.ParentId);

                if (parent == null && parent.AuthorId != user.Id)
                {
                    return ServiceResult<Note.Model>.MakeError<UnauthorizedError>();
                }
            }

            note.AuthorId = user.Id;

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
    }
}
