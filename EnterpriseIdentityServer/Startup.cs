using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EnterpriseIdentityServer.Model;
using EnterpriseIdentityServer.Repository;
using EnterpriseIdentityServer.Repository.IRepository;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace EnterpriseIdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
       
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public IConfigurationRoot ConfigurationRoot { get; }
        //public Startup(Microsoft.Extensions.Hosting.IHostingEnvironment env)
        //{
        //    var dom = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json",optional:true,reloadOnChange:true)
        //        //.AddJsonFile("appcredentials.json")
        //        //.AddJsonFile("appui.json")
        //        .AddInMemoryCollection(
        //                new Dictionary<string, string> {
        //                    {"Timezone", "+1"}
        //                })
        //        .Build();


        //    // Save the configuration DOM
        //    ConfigurationRoot = dom;

        //    // Next tasks:
        //    //   - Load the config data into a POCO class
        //    //   - Share the POCO class with the rest of the app
        //}

        public void ConfigureServices(IServiceCollection services)
        {

            var IdentityServerCon = Configuration["ConnectionStrings:IdentityServerConnection"];


            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddMvcCore().AddJsonFormatters(); //Adding mvc and able to work with json.
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddIdentityServer()
                
                    .AddInMemoryClients(new List<Client>())
                    .AddInMemoryIdentityResources(new List<IdentityResource>())
                    .AddInMemoryApiResources(new List<ApiResource>())
                    //.AddTestUsers(new List<TestUser>())
                    .AddDeveloperSigningCredential()
                    .AddInMemoryApiResources(new List<ApiResource>()
                     {
                         new ApiResource("api.sample", "Enterprise Identity API")
                     })
                    .AddConfigurationStore(options =>
                            options.ConfigureDbContext = builder =>
                                builder.UseSqlServer(IdentityServerCon, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                    .AddInMemoryClients(new List<Client>()
                    {
                        new Client
                        {
                            ClientId = "Authentication",
                            ClientSecrets =
                            {
                                new Secret("clientsecret".Sha256())
                            },
                            AllowedGrantTypes = { "authentication" },
                            AllowedScopes =
                            {
                                "api.sample"
                            },
                            AllowOfflineAccess = true
                        }
                    });


            services.AddDbContext<EnterpriseIdentityDbContext>(options => options.UseSqlServer(IdentityServerCon));   
            services.AddControllers();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddMvc().AddControllersAsServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new  OpenApiInfo{ Title = "Enterprise Identity", Version = "v1" });
            });
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
