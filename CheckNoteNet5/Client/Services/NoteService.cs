using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
using Microsoft.Extensions.Caching.Memory;
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

        public async Task<ServiceResult<Note.Model>> Get(int id) => await FromCache("note", async factory =>
        {
            var response = await httpClient.GetAsync($"api/note/{id}");

            return await Parse<Note.Model>(response);
        });

        public async Task<ServiceResult<Note.Model>> Add(Note note)
        {
            var response = await httpClient.PostAsJsonAsync("api/note", note);

            return await Parse<Note.Model>(response);
        }
    }
}
