using CheckNoteNet5.Shared.Models;
using System.Threading.Tasks;

namespace CheckNoteNet5.Shared.Services
{
    public interface INoteService
    {
        Task<ServiceResult<Note.Model>> Get(int id);
        //Task<IResult<Note.Model>> Add(Note note);
    }
}
