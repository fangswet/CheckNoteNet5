using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Inputs;
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

        public async Task<ServiceResult> Login(LoginInput credentials) => (ServiceResult)await httpClient.PostAsJsonAsync("api/login", credentials);

        public async Task<ServiceResult<UserModel>> Register(RegisterInput credentials) 
            => await httpClient.PostAsJsonAsync("api/register", credentials);

        public async Task<ServiceResult<UserModel>> GetUser() => await httpClient.GetAsync("api/user");

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public int GetUserId()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<string>> Jwt(LoginInput credentials)
        {
            throw new NotImplementedException();
        }

        public void AssertAuthentication()
        {
            throw new NotImplementedException();
        }
    }
}
