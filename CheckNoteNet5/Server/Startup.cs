using AutoMapper;
using CheckNoteNet5.Server.Services;
using CheckNoteNet5.Shared.Models.Auth;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CheckNoteNet5.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CheckNoteContext>();
            services.AddHttpContextAccessor();

            services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddSignInManager()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<CheckNoteContext>();

            services.AddAuthentication(AuthScheme.Cookie)
                .AddCookie() // how the fuck do I configure this? for example the login path? shit is starting to get ridiculous
                // outsource configuration
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // development

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        RequireAudience = false,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAssertion(_ => true).Build();
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<JwtService>();
            services.AddScoped<AuthService>();
            services.AddScoped<UserService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<QuestionService>();
            services.AddScoped<CourseService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });

            // move this

            if (!roleManager.RoleExistsAsync(Role.Admin).Result)
            {
                roleManager.CreateAsync(new Role { Name = Role.Admin }).Wait();
            }

            var admin = userManager.FindByNameAsync("admin").Result;

            if (admin == null)
            {
                var newAdmin = new User { UserName = "admin", Email = "admin@admin.com" };

                userManager.CreateAsync(newAdmin, "admin").Wait();
                userManager.AddToRoleAsync(newAdmin, Role.Admin).Wait();
            }
        }
    }
}
