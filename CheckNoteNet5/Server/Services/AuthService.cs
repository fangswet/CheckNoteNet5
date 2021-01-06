﻿using CheckNoteNet5.Shared.Models.Auth;
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
        public int UserId
        {
            get
            {
                if (userId == null)
                {
                    if (httpContext.User.Identity.IsAuthenticated && int.TryParse(userManager.GetUserId(httpContext.User), out int id))
                    {
                        userId = id;
                        return id;
                    }
                    throw new UnauthorizedException();
                }
                return (int)userId;
            }
        }

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, UserService userService, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.jwtService = jwtService;
            httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ServiceResult> Login(Login credentials)
        {
            var user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null) return ServiceResult.MakeError<NotFoundError>();

            var login = await signInManager.PasswordSignInAsync(user, credentials.Password, false, false);

            if (login.Succeeded) return ServiceResult.MakeOk();

            return ServiceResult.MakeError<UnauthorizedError>();
        }

        public async Task<ServiceResult<User.Model>> Register(Register credentials)
        {
            if ((await userManager.FindByEmailAsync(credentials.Email)) != null || (await userManager.FindByNameAsync(credentials.UserName)) != null)
                return ServiceResult<User.Model>.MakeError<ConflictError>("user already exists");

            var user = new User(credentials);
            await userManager.CreateAsync(user, credentials.Password);

            return await userService.Get(user.Id);
        }

        public async Task<ServiceResult> Logout()
        {
            await signInManager.SignOutAsync();

            return ServiceResult.MakeOk();
        }

        public async Task<ServiceResult<string>> Jwt(Login input)
        {
            var unauthorized = ServiceResult<string>.MakeError<UnauthorizedError>();

            var user = await userManager.FindByEmailAsync(input.Email);
            if (user == null) return unauthorized;

            var signIn = await signInManager.CheckPasswordSignInAsync(user, input.Password, false);
            if (!signIn.Succeeded) return unauthorized;

            return ServiceResult.MakeOk(await jwtService.GenerateToken(user));
        }
    }
}
