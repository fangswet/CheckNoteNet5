using AutoMapper;
using CheckNoteNet5.Shared.Models.Auth;
using CheckNoteNet5.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class UserService
    {
        private readonly CheckNoteContext dbContext;
        private readonly IMapper mapper;

        public UserService(CheckNoteContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<User.Model>> Get(int id)
        {
            var query = dbContext.Users.Where(u => u.Id == id);
            var user = await mapper.ProjectTo<User.Model>(query).FirstOrDefaultAsync();

            return ServiceResult.NullCheck(user);
        }
    }
}
