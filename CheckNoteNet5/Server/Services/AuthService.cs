using CheckNoteNet5.Shared.Models.Auth;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class AuthService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly UserService userService;
        private readonly JwtService jwtService;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, UserService userService, JwtService jwtService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.jwtService = jwtService;
        }

        public async Task<Result> Login(Login input)
        {
            var user = await userManager.FindByEmailAsync(input.Email);
            if (user == null) return Result.Error<NotFoundError>();

            var login = await signInManager.PasswordSignInAsync(user, input.Password, false, false);

            if (login.Succeeded) return Result.Ok();

            return Result.Error<UnauthorizedError>();
        }

        public async Task<Result<User.Model>> Register(Register input)
        {
            if ((await userManager.FindByEmailAsync(input.Email)) != null || (await userManager.FindByNameAsync(input.UserName)) != null)
                return Result<User.Model>.MakeError<ConflictError>("user already exists");

            var user = new User(input);
            await userManager.CreateAsync(user, input.Password);

            return await userService.Get(user.Id);
        }
        public async Task<Result> Logout()
        {
            await signInManager.SignOutAsync();

            return Result.Ok();
        }

        public async Task<Result<string>> Jwt(Login input)
        {
            var result = new Result<string>();

            var user = await userManager.FindByEmailAsync(input.Email);
            if (user == null) return result.Error<UnauthorizedError>();

            await signInManager.CheckPasswordSignInAsync(user, input.Password, false);

            return result.Ok(await jwtService.GenerateToken(user));
        }
    }
}
