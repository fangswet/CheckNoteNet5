using AutoMapper;
using CheckNoteNet5.Server.Services;
using CheckNoteNet5.Server.Services.Filters;
using CheckNoteNet5.Shared.Models.Dtos;
using CheckNoteNet5.Shared.Services;
using GraphQL.Server;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddRoles<Role>()
                .AddSignInManager()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<CheckNoteContext>();

            services.AddAuthentication()
                .AddCookie(IdentityConstants.ApplicationScheme, options =>
                {
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = async context => context.Response.StatusCode = 401,
                        OnRedirectToAccessDenied = async context => context.Response.StatusCode = 403
                    };
                })
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
                var cookiePolicy = new AuthorizationPolicyBuilder(IdentityConstants.ApplicationScheme)
                    .RequireAuthenticatedUser().Build();

                var jwtPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build();

                options.AddPolicy(Policy.Cookie, cookiePolicy);
                options.AddPolicy(Policy.Jwt, jwtPolicy);
                options.DefaultPolicy = new AuthorizationPolicyBuilder().Combine(cookiePolicy).Combine(jwtPolicy).Build();
            });

            services.AddControllersWithViews(options => options.Filters.Add(new ServiceExceptionFilter()));
            services.AddRazorPages();

            // figure out adding maps
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<CacheService>();
            services.AddScoped<JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<UserService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<QuestionService>();
            services.AddScoped<CourseService>();
            services.AddScoped<TagService>();

            services.AddScoped<Schema.Schema>();
            services.AddGraphQL(options => options.EnableMetrics = false)
                    .AddSystemTextJson()
                    .AddGraphTypes(ServiceLifetime.Scoped)
                    .AddDataLoader()
                    .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CheckNoteContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager)
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

            app.UseGraphQL<Schema.Schema>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });

            try
            {
                dbContext.Database.ExecuteSqlRaw("CREATE UNIQUE INDEX UK_CourseNote ON CourseNotes (NoteId, CourseId);");
                dbContext.Database.ExecuteSqlRaw("CREATE UNIQUE INDEX UK_NoteTag ON NoteTags (NoteId, TagId);");
                dbContext.Database.ExecuteSqlRaw("CREATE UNIQUE INDEX UK_CourseLike ON CourseLikes (UserId, CourseId);");
                dbContext.Database.ExecuteSqlRaw("CREATE UNIQUE INDEX UK_NoteLike ON NoteLikes (UserId, NoteId);");
                dbContext.Database.ExecuteSqlRaw(@"
                    CREATE TRIGGER NoteModifiedAt
                        ON [dbo].[Notes]
                        FOR INSERT, UPDATE
                    AS
                    BEGIN
                        SET NOCOUNT ON;

	                    IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;

	                    DECLARE @Id INT

	                    SELECT @Id = INSERTED.Id
	                    FROM INSERTED

	                    UPDATE dbo.Notes
	                    SET ModifiedAt = GETDATE()
	                    WHERE Id = @Id
                    END
                ");
            }
            catch (SqlException e) when (e.Number == 1913) { }

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
