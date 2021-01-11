using CheckNoteNet5.Shared.Models.Auth;
using CheckNoteNet5.Shared.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CheckNoteNet5.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;

        public AuthService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ServiceResult> Login(Login credentials) => (ServiceResult)await httpClient.PostAsJsonAsync("api/login", credentials);

        public async Task<ServiceResult<User.Model>> Register(Register credentials) 
            => await httpClient.PostAsJsonAsync("api/register", credentials);

        public async Task<ServiceResult<User.Model>> GetUser() => await httpClient.GetAsync("api/user");

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public int GetUserId()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<string>> Jwt(Login credentials)
        {
            throw new NotImplementedException();
        }
    }
}
