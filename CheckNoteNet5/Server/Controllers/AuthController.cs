using CheckNoteNet5.Server.Services.Extensions;
using CheckNoteNet5.Shared.Models.Auth;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<User.Model>> Register(Register user) => await authService.Register(user).MapToAction();

        [HttpPost]
        public async Task<ActionResult> Login(Login user) => await authService.Login(user).MapToAction();

        [HttpPost]
        public async Task<ActionResult<string>> Jwt(Login user) => await authService.Jwt(user).MapToAction();

        [Authorize]
        public async Task Logout() => await authService.Logout();

        [Authorize]
        [HttpGet]
        public async new Task<ActionResult<User.Model>> User() => await authService.GetUser().MapToAction();
    }
}
