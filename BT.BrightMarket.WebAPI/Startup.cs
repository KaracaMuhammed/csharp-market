using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using BT.BrightMarket.Infrastructure.Extensions;
using BT.BrightMarket.Application.Extensions;
using BT.BrightMarket.WebAPI.Extensions;
using BT.BrightMarket.WebAPI.Hubs;

namespace BT.BrightMarket.WebAPI
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.RegisterInfrastructure();
            services.RegisterApplication();
            services.AddSignalR();
            services.AddSingleton<ConnectedClientsService>();
            services.AddControllers().AddJsonOptions(options =>
            {
                //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Enable when cycle object depth larger than 32
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BT.BrightMarket.WebAPI", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer" // must be lowercase
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

            });

            // Add BrightMarketContext with SQL Server configuration
            //services.AddDbContext<BrightMarketContext>(options =>
            //{
            //    var connectionString = Configuration.GetConnectionString("BrightMarketDatabase");
            //    // Inject IWebHostEnvironment to check if it's development environment
            //    var environment = services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();
            //    if (!environment.IsDevelopment())
            //    {
            //        var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
            //        connectionString = string.Format(connectionString, password);
            //    }
            //    options.UseSqlServer(connectionString);
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BT.BrightMarket.WebAPI v1")); //duplicated
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BT.BrightMarket.WebAPI v1"));


            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });

            app.UseHttpsRedirection();

            app.UseErrorHandlingMiddleware();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorizationMiddleware();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapHub<NotificationHub>("/notificationhub");
                endpoints.MapControllers();//endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
