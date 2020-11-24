using System;
using System.Text;
using System.Threading.Tasks;
using IdentityAndJWT.CSUI.Data;
using IdentityAndJWT.CSUI.Model;
using IdentityAndJWT.CSUI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAndJWT.CSUI
{
    class Program
    {
        static void Main(string[] args)
        {

            IServiceCollection services = new ServiceCollection();

            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Get Service and call method
            var service = serviceProvider.GetService<IAuthService>();


            User user = new User()
            {
                Email = "erdinckara10@gmail.com",
                UserName = "erdinckara10@gmail.com",
                DateOfBirth = new DateTime(1990, 03, 24),
                Bio = "Hello from my bio"
            };

            string password = "MyPassword_123";

            var resultOfRegister = service.Register(user, password).Result;

            if (resultOfRegister.Succeeded)
            {
                Console.WriteLine("Register succeeded...");
                Task.Delay(3000);

                var resultOfLogin = service.Login(user, password).Result;

                if (resultOfLogin.Succeeded)
                {
                    Console.WriteLine("Login succeeded...");
                    Task.Delay(3000);

                    var token = service.GetToken(user);

                    Console.WriteLine($"JWT Token: {token}");
                }
            }

            Console.Read();
        }
    }


    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddScoped<IAuthService, AuthService>();
            services.AddDbContext<MyContext>();

            // services.AddScoped<UserManager<User>>();
            // services.AddScoped<SignInManager<User>>();
            // services.AddScoped<RoleManager<Role>>();

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<MyContext>();

            services.AddIdentityCore<User>()
    .AddRoles<Role>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, Role>>()
    .AddEntityFrameworkStores<MyContext>()
    .AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(options =>
                                              {

                                                  options.Password.RequireDigit = true;
                                                  options.Password.RequireLowercase = true;
                                                  options.Password.RequireUppercase = true;
                                                  options.Password.RequireNonAlphanumeric = true;
                                                  options.Password.RequiredLength = 8;

                                                  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                                                  options.Lockout.MaxFailedAccessAttempts = 5;
                                                  options.Lockout.AllowedForNewUsers = true;

                                                  options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                                                  options.User.RequireUniqueEmail = true;
                                              });

            services.AddAuthentication(x =>
                       {
                           x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                           x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                       })
                       .AddJwtBearer(x =>
                       {
                           x.RequireHttpsMetadata = false;
                           x.SaveToken = true;
                           x.TokenValidationParameters = new TokenValidationParameters
                           {
                               ValidateIssuerSigningKey = true,
                               IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Secret").Value)),
                               ValidateIssuer = false,
                               ValidateAudience = false
                           };
                       });

        }
    }
}
