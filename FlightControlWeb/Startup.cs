using FlightControlWeb.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the
        //container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers();
            //services.AddSingleton(typeof(IDatabase), typeof(Database));
            services.AddSingleton(typeof(IExternalFlights), typeof(ExternalFlights));
            services.AddSingleton(typeof(ISQLCommands), typeof(SQLCommands));
        }

        // This method gets called by the runtime. Use this method to
        //configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           //the last in chain of command "catch them all" reqest handler
            app.Run(async (handler) =>
            {
                string processName = Process.GetCurrentProcess().ProcessName;
                string dirName = Directory.GetCurrentDirectory();
                await handler.Response.WriteAsync($"Hello {processName},Directory: {dirName}");
            });
        }
    }
    
}
