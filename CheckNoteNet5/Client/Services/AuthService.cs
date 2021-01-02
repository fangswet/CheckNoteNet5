using CheckNoteNet5.Shared.Models.Auth;
using CheckNoteNet5.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ServiceResult> Login(Login credentials)
        {
            var response = await httpClient.PostAsJsonAsync("api/login", credentials);

            return await ServiceResult<object>.Parse(response);
        }

        public async Task<ServiceResult<User.Model>> Register(Register credentials) 
            => await httpClient.PostAsJsonAsync("api/register", credentials);

        public Task<ServiceResult> Logout()
        {
            throw new NotImplementedException();
        }
    }
}
