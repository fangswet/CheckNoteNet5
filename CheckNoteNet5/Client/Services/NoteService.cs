using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Services;
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

        // shit would make more sense if the whole Result was being returned from the API
        // also somehow standardize route strings
        // also check if two db queries without caching
        
        // handle errors
        public async Task<ServiceResult<Note.Model>> Get(int id)
        {
            var result = new ServiceResult<Note.Model>();
            var response = await httpClient.GetAsync($"api/note/{id}");

            if (response.IsSuccessStatusCode)
            {
                var note = await response.Content.ReadFromJsonAsync<Note.Model>();
                return result.Ok(note);
            }

            var error = await response.Content.ReadFromJsonAsync<Error>();
            // wrong because I actually need to access the error and the value is empty when casted to ServiceResult
            // this may call for a unified ServiceResult type where the Errors are always visible
            // or at the least the IsOk flag should be set manually so I can now return ServiceResult(error) and access it that way
            return new ServiceError<Note.Model>(error); //result.Error(error);
        }
    }
}
