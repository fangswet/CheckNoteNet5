using AutoMapper;
using CheckNoteNet5.Server.Services;
using CheckNoteNet5.Shared.Models;
using CheckNoteNet5.Shared.Models.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;
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

        public record Test1(int Id, string Text, bool Sex);
        public record Test2(int Id, string Text);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CheckNoteContext>();
            services.AddHttpContextAccessor();

            services.AddIdentity<User, Role>(config =>
            {
                // development
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<CheckNoteContext>()
                .AddRoles<Role>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(config =>
            {
                config.RequireAuthenticatedSignIn = false;
            })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false; // development
                    config.SaveToken = true;

                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        RequireAudience = false,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

            services.ConfigureApplicationCookie(options => options.LoginPath = "/");

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped(services =>
                new HttpClient
                {
                    BaseAddress = new Uri(services.GetRequiredService<NavigationManager>().BaseUri)
                }
            );

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<JwtService>();
            services.AddScoped<AuthService>();
            services.AddScoped<QuestionService>();
            services.AddScoped<NoteService>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/Index");
            });

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
