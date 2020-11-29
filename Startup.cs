using System.Net;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Youpay.API.Data;
using Youpay.API.Helpers;
using Youpay.API.Repository;
using Youpay.API.Repository.Impl;
using Youpay.API.Services;
using Youpay.API.Services.Impl;
using Youpay.API.Utils;
using Youpay.API.Utils.Impl;

namespace Youpay.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContextPool<DataContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContextPool<DataContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            ConfigureServices(services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddAutoMapper(typeof(AuthServices ).Assembly);
            services.AddAutoMapper(typeof(BankingDetailsServices ).Assembly);
            services.AddScoped<ICustomAuthorization, CustomAuthorization>();
            services.AddScoped<ITokenUtil, TokenUtil>();
            services.AddScoped<IUserUtil, UserUtil>();
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<ITransactionServices, TransactionServices>();
            services.AddScoped<IBankingDetailsServices, BankingDetailsServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IMailingServices, MailGunMailingService>();
            services.AddScoped<IBankingDetailsRepository, BankingDetailsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBankingDetailsRepository, BankingDetailsRepository>();
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            
            
            services.AddControllers();
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Youpay.API", Version = "v1" });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Youpay.API v1"));
            }
             else
            {
                //Sets Global Exception handler
                app.UseExceptionHandler( builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if(error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            //app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
