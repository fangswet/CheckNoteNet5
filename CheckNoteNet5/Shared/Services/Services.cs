using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckNoteNet5.Shared.Services
{
    public interface IAuthService
    {
        int GetUserId();
        void AssertAuthentication();
        Task<ServiceResult> Login(LoginInput credentials);
        Task<ServiceResult<UserModel>> Register(RegisterInput credentials);
        Task Logout();
        Task<ServiceResult<string>> Jwt(LoginInput credentials);
        Task<ServiceResult<UserModel>> GetUser();
    }

    public interface INoteService
    {
        Task<ServiceResult<NoteModel>> Get(int id);
        Task<ServiceResult<NoteModel>> Get(string title);
        Task<ServiceResult<NoteModel>> Add(NoteInput input);
        Task<ServiceResult> Remove(int id);
        Task<ServiceResult<NoteModel>> Update(int id, NoteInput input);
        Task<ServiceResult<List<NoteEntry>>> List(string title = null);
    }
}
