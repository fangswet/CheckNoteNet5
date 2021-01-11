using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Inputs;
using CheckNoteNet5.Shared.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CheckNoteNet5.Client.Services
{
    public class NoteService : INoteService
    {
        private readonly HttpClient httpClient;

        public NoteService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ServiceResult<NoteModel>> Get(int id) => await httpClient.GetAsync($"api/note/{id}");

        public async Task<ServiceResult<NoteModel>> Add(NoteInput note) => await httpClient.PostAsJsonAsync("api/note", note);

        public async Task<ServiceResult> Remove(int id) => await httpClient.DeleteAsync($"api/note/{id}");

        public async Task<ServiceResult<NoteModel>> Update(int id, NoteInput input)
            => await httpClient.PutAsJsonAsync($"api/note/{id}", input);

        public async Task<ServiceResult<List<NoteEntry>>> List(string title = null)
            => await httpClient.GetAsync(title == null ? "api/note" : $"api/note?title={title}");
    }
}
