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
    public class CourseService
    {
        private readonly CheckNoteContext dbContext;
        private readonly AuthService authService;
        private readonly IMapper mapper;

        public CourseService(CheckNoteContext dbContext, AuthService authService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.authService = authService;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<Course.Entry>> Get(int id)
        {
            var query = dbContext.Questions.Where(q => q.Id == id);
            var question = await mapper.ProjectTo<Course.Entry>(query).FirstOrDefaultAsync();

            return ServiceResult.NullCheck(question);
        }

        public async Task<ServiceResult<Course.Entry>> Add(Course course)
        {
            course.AuthorId = (int)authService.UserId; // caution implied that authorization is in place

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return await Get(course.Id);
        }
    }
}
