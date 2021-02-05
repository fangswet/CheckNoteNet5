using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Dtos;
using CheckNoteNet5.Shared.Models.Inputs;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly UserService userService;
        private readonly JwtService jwtService;
        private readonly HttpContext httpContext;
        private int? userId;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, UserService userService, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.jwtService = jwtService;
            httpContext = httpContextAccessor.HttpContext;
        }

        public int GetUserId()
        {
            if (userId == null)
            {
                if (httpContext.User.Identity.IsAuthenticated &&
                    int.TryParse(userManager.GetUserId(httpContext.User), out int id))
                {
                    userId = id;
                    return id;
                }
                throw new UnauthorizedException();
            }
            return (int)userId;
        }

        public void AssertAuthentication() { GetUserId(); }

        public async Task<ServiceResult<UserModel>> GetUser()
        {
            var result = new ServiceResult<UserModel>();

            try
            {
                var user = await userService.Get(GetUserId());
                return result.Ok(user.Unwrap());
            }
            catch
            {
                return result.Error<UnauthorizedError>();
            }
        }

        public async Task<ServiceResult> Login(LoginInput credentials)
        {
            var user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null) return ServiceResult.MakeError<NotFoundError>();

            var login = await signInManager.PasswordSignInAsync(user, credentials.Password, false, false);

            if (login.Succeeded) return ServiceResult.MakeOk();

            return ServiceResult.MakeError<UnauthorizedError>();
        }

        public async Task<ServiceResult<UserModel>> Register(RegisterInput credentials)
        {
            if ((await userManager.FindByEmailAsync(credentials.Email)) != null || (await userManager.FindByNameAsync(credentials.UserName)) != null)
                return ServiceResult<UserModel>.MakeError<UserExistsError>();

            var user = new User(credentials);
            await userManager.CreateAsync(user, credentials.Password);

            return await userService.Get(user.Id);
        }

        public async Task Logout() => await signInManager.SignOutAsync();

        public async Task<ServiceResult<string>> Jwt(LoginInput credentials)
        {
            var unauthorized = ServiceResult<string>.MakeError<UnauthorizedError>();

            var user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null) return unauthorized;

            var signIn = await signInManager.CheckPasswordSignInAsync(user, credentials.Password, false);
            if (!signIn.Succeeded) return unauthorized;

            return ServiceResult.MakeOk(await jwtService.GenerateToken(user));
        }
    }
}
