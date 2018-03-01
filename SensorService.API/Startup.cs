using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SensorService.Api.Services;
using SensorService.API.Authorizations;
using SensorService.API.Middleware;
using SensorService.API.Models;
using SensorService.API.Operations;
using SensorService.API.Queries;
using SensorService.API.Services;
using SensorService.Shared.Dtos;
using Swashbuckle.AspNetCore.Swagger;

namespace SensorService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SensorContext>();
            services.AddSingleton<IMapperService, MapperService>();
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddScoped<IAdministratorAuthorization, AdministratorAuthorization>();
            services.AddScoped<IAdministratorAuthorization<UserDto>, AdministratorAuthorization<UserDto>>();
            services.AddScoped<IAdministratorAuthorization<UserIdDto>, AdministratorAuthorization<UserIdDto>>();
            services.AddScoped<INoAuthorization, NoAuthorization>();
            services.AddScoped<INoAuthorization<LoginDto>, NoAuthorization<LoginDto>>();
            services.AddScoped<INoAuthorization<RefreshTokenDto>, NoAuthorization<RefreshTokenDto>>();
            services.AddScoped<INoAuthorization<string>, NoAuthorization<string>>();
            services.AddScoped<INoAuthorization<UpdateDeviceDataDto>, NoAuthorization<UpdateDeviceDataDto>>();
            services.AddScoped<INoAuthorization<DeviceDto>, NoAuthorization<DeviceDto>>();
            services.AddScoped<INoAuthorization<UserDto>, NoAuthorization<UserDto>>();
            services.AddScoped<INoAuthorization<UserIdDto>, NoAuthorization<UserIdDto>>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IGetDevicesOperation, GetDevicesOperation>();
            services.AddScoped<IGetDeviceByIdOperation, GetDeviceByIdOperation>();
            services.AddScoped<ISendSensorDataOperation, SendSensorDataOperation>();
            services.AddScoped<IUpdateDeviceOperation, UpdateDeviceOperation>();
            services.AddScoped<IGenerateTokenOperation, GenerateTokenOperation>();
            services.AddScoped<IRefreshTokenOperation, RefreshTokenOperation>();
            services.AddScoped<IGetUsersOperation, GetUsersOperation>();
            services.AddScoped<IInsertUserOperation, InsertUserOperation>();
            services.AddScoped<IUpdateUserOperation, UpdateUserOperation>();
            services.AddScoped<IDeleteUserOperation, DeleteUserOperation>();
            services.AddScoped<IGetUserByIdOperation, GetUserByIdOperation>();
            services.AddScoped<IGetDevicesByUserOperation, GetDevicesByUserOperation>();
            services.AddScoped<ILoginUserOperation, LoginUserOperation>();
            services.AddScoped<IDeviceQueries, DeviceQueries>();
            services.AddScoped<IUserQueries, UserQueries>();
            services.AddScoped<ITokenQueries, TokenQueries>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMvc();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build());
            });
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddCookie(cfg => cfg.SlidingExpiration = true)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Sensor Data", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseAccountService();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.ApplicationServices.GetRequiredService<IMapperService>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
