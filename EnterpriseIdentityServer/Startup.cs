using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseIdentityServer.Model;
using EnterpriseIdentityServer.Repository;
using EnterpriseIdentityServer.Repository.IRepository;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


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



            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryApiResources(new List<ApiResource>()
                     {
                         new ApiResource("api.sample", "Enterprise Identity API")
                     })                    
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


            services.AddDbContext<EnterpriseIdentityDbContext>(
                                 options => options.UseSqlServer(IdentityServerCon));
         


            //services.AddControllers();



            //services.AddOptions();
            //services.Configure<GlobalAppSettings>(ConfigurationRoot);
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddMvc()
                    .AddControllersAsServices();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
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
