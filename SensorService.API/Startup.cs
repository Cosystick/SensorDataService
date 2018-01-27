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
            services.AddTransient<IAdministratorAuthorization, AdministratorAuthorization>();
            services.AddTransient<IAdministratorAuthorization<UserDTO>, AdministratorAuthorization<UserDTO>>();
            services.AddTransient<IAdministratorAuthorization<UserIdDTO>, AdministratorAuthorization<UserIdDTO>>();
            services.AddTransient<INoAuthorization, NoAuthorization>();
            services.AddTransient<INoAuthorization<LoginDTO>, NoAuthorization<LoginDTO>>();
            services.AddTransient<INoAuthorization<string>, NoAuthorization<string>>();
            services.AddTransient<INoAuthorization<DeviceDataDTO>, NoAuthorization<DeviceDataDTO>>();
            services.AddTransient<INoAuthorization<DeviceDTO>, NoAuthorization<DeviceDTO>>();
            services.AddTransient<INoAuthorization<UserDTO>, NoAuthorization<UserDTO>>();
            services.AddTransient<INoAuthorization<UserIdDTO>, NoAuthorization<UserIdDTO>>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IGetDevicesOperation, GetDevicesOperation>();
            services.AddTransient<IGetDeviceByIdOperation, GetDeviceByIdOperation>();
            services.AddTransient<ISendSensorDataOperation, SendSensorDataOperation>();
            services.AddTransient<IUpdateDeviceOperation, UpdateDeviceOperation>();
            services.AddTransient<IGenerateTokenOperation, GenerateTokenOperation>();
            services.AddTransient<IGetUsersOperation, GetUsersOperation>();
            services.AddTransient<IInsertUserOperation, InsertUserOperation>();
            services.AddTransient<IUpdateUserOperation, UpdateUserOperation>();
            services.AddTransient<IDeleteUserOperation, DeleteUserOperation>();
            services.AddTransient<IGetUserByIdOperation, GetUserByIdOperation>();
            services.AddTransient<IGetDevicesByUserOperation, GetDevicesByUserOperation>();
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
