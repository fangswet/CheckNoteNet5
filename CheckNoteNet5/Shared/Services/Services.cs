using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckNoteNet5.Shared.Services
{
    public interface IAuthService
    {
        Task<ServiceResult> Login(Login credentials);
        Task<ServiceResult<User.Model>> Register(Register credentials);
        Task<ServiceResult> Logout();
    }

    public interface INoteService
    {
        Task<ServiceResult<Note.Model>> Get(int id);
        Task<ServiceResult<Note.Model>> Add(Note.Input input);
        Task<ServiceResult> Remove(int id);
        Task<ServiceResult<Note.Model>> Update(int id, Note.Input input);
        Task<ServiceResult<List<Note.Entry>>> List(string title = null);
    }
}
