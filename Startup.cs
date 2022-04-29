using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ServerApp.Data;
using ServerApp.Models;

namespace ServerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        readonly string MyAllowOrigins="_myAllowOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SocialContext>(options => options.UseSqlServer(Configuration["dbConnection"]));
            services.AddIdentity<User,Role>().AddEntityFrameworkStores<SocialContext>();
            services.Configure<IdentityOptions>(options =>{
                options.Password.RequireDigit =false;
                options.Password.RequireLowercase =false;
                options.Password.RequireUppercase =false;
                options.Password.RequireNonAlphanumeric =false;
                options.Password.RequiredLength =3;

                //kullanıcı 5 defa şifresini yanlış girerse 5dk giriş yapamaz.
                options.Lockout.MaxFailedAccessAttempts=5;
                options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);
                //yeni hesap ise kilitlenmez
                options.Lockout.AllowedForNewUsers=true;
                // options.User.AllowedUserNameCharacters="asdasdasasdfadgd";

            });
            services.AddControllers();
            services.AddCors(options =>{
                options.AddPolicy(
                    name:MyAllowOrigins,
                builder=>{
                    builder
                    .WithOrigins("http://localhost:4200","https://localhost:4200")
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                    // .WithMethods("GET","POST","DELETE","PUT");
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServerApp", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerApp v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(MyAllowOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
