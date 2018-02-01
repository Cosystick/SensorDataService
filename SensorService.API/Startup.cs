using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SensorService.API.Authorizations;
using SensorService.API.DTOs;
using SensorService.API.Models;
using SensorService.API.Operations;

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
            services.AddScoped<IAdministratorAuthorization, AdministratorAuthorization>();
            services.AddScoped<IAdministratorAuthorization<UserDTO>, AdministratorAuthorization<UserDTO>>();
            services.AddScoped<IAdministratorAuthorization<UserIdDTO>, AdministratorAuthorization<UserIdDTO>>();
            services.AddScoped<INoAuthorization, NoAuthorization>();
            services.AddScoped<INoAuthorization<LoginDTO>, NoAuthorization<LoginDTO>>();
            services.AddScoped<INoAuthorization<string>, NoAuthorization<string>>();
            services.AddScoped<INoAuthorization<DeviceDataDTO>, NoAuthorization<DeviceDataDTO>>();
            services.AddScoped<INoAuthorization<DeviceDTO>, NoAuthorization<DeviceDTO>>();
            services.AddScoped<INoAuthorization<UserDTO>, NoAuthorization<UserDTO>>();
            services.AddScoped<INoAuthorization<UserIdDTO>, NoAuthorization<UserIdDTO>>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IGetDevicesOperation, GetDevicesOperation>();
            services.AddScoped<IGetDeviceByIdOperation, GetDeviceByIdOperation>();
            services.AddScoped<ISendSensorDataOperation, SendSensorDataOperation>();
            services.AddScoped<IUpdateDeviceOperation, UpdateDeviceOperation>();
            services.AddScoped<IGenerateTokenOperation, GenerateTokenOperation>();
            services.AddScoped<IGetUsersOperation, GetUsersOperation>();
            services.AddScoped<IInsertUserOperation, InsertUserOperation>();
            services.AddScoped<IUpdateUserOperation, UpdateUserOperation>();
            services.AddScoped<IDeleteUserOperation, DeleteUserOperation>();
            services.AddScoped<IGetUserByIdOperation, GetUserByIdOperation>();
            services.AddScoped<IGetDevicesByUserOperation, GetDevicesByUserOperation>();
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
                //.AddCookie(cfg => cfg.SlidingExpiration = true)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                        
                    };
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
            
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
