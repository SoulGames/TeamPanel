using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace TeamPanel
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
            string serverName = Configuration.GetValue<string>("host");
            uint port = Configuration.GetValue<uint>("port");
            string userId = Configuration.GetValue<string>("user");
            string password = Configuration.GetValue<string>("password");
            string databaseName = Configuration.GetValue<string>("name");

            MySqlConnectionStringBuilder ConnectionBuilder = new MySqlConnectionStringBuilder();
            ConnectionBuilder.Server = serverName;
            ConnectionBuilder.Port = port;
            ConnectionBuilder.UserID = userId;
            ConnectionBuilder.Password = password;
            ConnectionBuilder.Database = databaseName;
            ConnectionBuilder.AllowUserVariables = true;
            ConnectionBuilder.AllowZeroDateTime = true;

            string cs = ConnectionBuilder.ConnectionString;

            Library.Managers.Entries.Load(new MySqlConnection(ConnectionBuilder.ConnectionString));

            MySqlConnection connection = new MySqlConnection(ConnectionBuilder.ConnectionString);

            services.AddTransient<MySqlConnection>(e => new MySqlConnection(cs));
            services.AddDistributedMemoryCache();

            services.AddControllersWithViews()
                .AddSessionStateTempDataProvider();
            services.AddRazorPages()
                .AddSessionStateTempDataProvider();

            services.AddMvc();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            { app.UseDeveloperExceptionPage(); }
            else
            { app.UseExceptionHandler("/Error"); }

            app.UseStaticFiles();
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }


        public static MySqlConnection GetSQLConnection(string serverName, string userId, string password, string databaseName)
        {
            MySqlConnectionStringBuilder ConnectionBuilder = new MySqlConnectionStringBuilder();
            ConnectionBuilder.Server = serverName;
            ConnectionBuilder.UserID = userId;
            ConnectionBuilder.Password = password;
            ConnectionBuilder.Database = databaseName;

            MySqlConnection connection = new MySqlConnection(ConnectionBuilder.ConnectionString);
            return connection;
        }
    }
}
