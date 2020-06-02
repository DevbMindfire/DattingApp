using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DattingApp.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DattingApp.API.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DattingApp.API.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DattingApp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace DattingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services){
            services.AddDbContext<DataContext>(Options=> {
                Options.UseLazyLoadingProxies();
                Options.UseSqlite(Configuration.GetConnectionString("DefaultConnectionString"));
            }
            );

            ConfigureServices(services);
        }
        public void ConfigureProductionServices(IServiceCollection services){
            services.AddDbContext<DataContext>(Options=>{
                Options.UseLazyLoadingProxies();
                Options.UseSqlServer(Configuration.GetConnectionString("SqlServerString"));
            });

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            IdentityBuilder builder = services.AddIdentityCore<User>(opt => {
                 opt.Password.RequireDigit = false;
                 opt.Password.RequiredLength = 4;
                 opt.Password.RequireNonAlphanumeric = false;
                 opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role) , builder.Services);

            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options=>{

                options.TokenValidationParameters=new TokenValidationParameters(){
                    
                    ValidateIssuerSigningKey=true,
                    ValidateIssuer=false,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateAudience=false

                };
            });

            services.AddControllers(options => {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddNewtonsoftJson(options =>{
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddCors();
            services.Configure<CloudinarySetting>(Configuration.GetSection("CloudinarySettings"));

            //Adding Dependency injection in Auth Repository
            services.AddScoped<IDattingRepository,DattingRepository>(); 
            services.AddAutoMapper(typeof(DattingRepository).Assembly);

            services.AddScoped<LogUserActivity>();
            //Adding Authorisation
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else{
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if(error != null){

                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
            //app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index","Fallback");
            });
        }
    }
}
