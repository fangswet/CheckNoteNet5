using AutoMapper;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Dtos;
using CheckNoteNet5.Shared.Models.Inputs;
using CheckNoteNet5.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class CourseService : ServerService
    {
        private readonly CheckNoteContext dbContext;
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public CourseService(CheckNoteContext dbContext, IAuthService authService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.authService = authService;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<CourseEntry>> Get(int id)
        {
            var query = dbContext.Courses.Where(q => q.Id == id);
            var question = await mapper.ProjectTo<CourseEntry>(query).FirstOrDefaultAsync();

            return ServiceResult.NullCheck(question);
        }

        public async Task<ServiceResult<CourseEntry>> Add(Course course)
        {
            course.AuthorId = authService.GetUserId();

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return await Get(course.Id);
        }

        public async Task<ServiceResult> AddNote(int courseId, int noteId)
        {
            var userId = authService.GetUserId();
            var course = await dbContext.Courses.FindAsync(courseId);

            if (course == null) return NotFound();
            if (course.AuthorId != userId) return Unauthorized();
            if (course.Notes.Any(n => n.Id == noteId)) return Conflict();

            var note = await dbContext.Notes.FindAsync(noteId);

            if (note == null) return NotFound();

            course.Notes.Add(note);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        public async Task<ServiceResult> RemoveNote(int courseId, int noteId)
        {
            var userId = authService.GetUserId();
            var course = await dbContext.Courses.FindAsync(courseId);

            if (course == null) return NotFound();
            if (course.AuthorId != userId) return Unauthorized();

            course.Notes.Remove(null); // check what happens on null
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        //public async Task<ServiceResult<int>> Test(int id, ICollection<AnswerAttempt> answers)
        //{
        //    var course = await dbContext.Courses.FindAsync(id);
        //    if (course == null) return NotFound<int>();

        //    // course testing mechanism
        //    // result is the percentage of max score
        //}
    }
}
