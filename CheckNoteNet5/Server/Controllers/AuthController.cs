using CheckNoteNet5.Server.Services;
using CheckNoteNet5.Shared.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        //[HttpPost]
        //public async Task<ActionResult<User.Model>> Register(Register user) => await authService.Register(user);

        //[HttpPost]
        //public async Task<ActionResult> Login(Login user) => await authService.Login(user);

        //[HttpPost]
        //public async Task<ActionResult<string>> Jwt(Login user) => await authService.Jwt(user);
    }
}
