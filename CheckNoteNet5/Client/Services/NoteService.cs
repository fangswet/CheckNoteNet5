using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CheckNoteNet5.Client.Services
{
    public class NoteService : ClientService, INoteService
    {
        private readonly HttpClient httpClient;

        public NoteService(HttpClient httpClient, IMemoryCache memoryCache) : base(memoryCache)
        {
            this.httpClient = httpClient;
        }

        public async Task<ServiceResult<Note.Model>> Get(int id) 
            => await FromCache("note", async factory => await httpClient.GetAsync($"api/note/{id}"));

        public async Task<ServiceResult<Note.Model>> Add(Note.Input note) => await httpClient.PostAsJsonAsync("api/note", note);

        public async Task<ServiceResult> Remove(int id) => await httpClient.DeleteAsync($"api/note/{id}");

        public async Task<ServiceResult<Note.Model>> Update(int id, Note.Input input)
            => await httpClient.PutAsJsonAsync($"api/note/{id}", input);

        public async Task<ServiceResult<List<Note.Entry>>> List(string title = null) 
            => await httpClient.GetAsync(title == null ? "api/note" : $"api/note?title={title}");
    }
}
