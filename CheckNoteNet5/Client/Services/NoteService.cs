using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckNoteNet5.Client.Services
{
    public class NoteService : ClientService, INoteService
    {
        private readonly HttpClient httpClient;
        public NoteService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ServiceResult<Note.Model>> Get(int id)
        {
            var response = await httpClient.GetAsync($"api/note/{id}");

            return await Parse<Note.Model>(response);
        }

        public async Task<ServiceResult<Note.Model>> Add(Note note)
        {
            var response = await httpClient.PostAsJsonAsync("api/note", note);

            return await Parse<Note.Model>(response);
        }
    }
}
