using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Craft_server.Models;
using Craft_server.Models.Contexts;
using Craft_server.Logger;

namespace Craft_server
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
            services.AddDbContext<ItemContext>(opt => opt.UseInMemoryDatabase("ItemList"));
            services.AddDbContext<PlayerContext>(opt => opt.UseInMemoryDatabase("PlayerList"));
            services.AddDbContext<ShipContext>(opt => opt.UseInMemoryDatabase("ShipList"));
            services.AddDbContext<SessionContext>(opt => opt.UseInMemoryDatabase("SessionList"));
            services.AddDbContext<GamePanelContext>(opt => opt.UseInMemoryDatabase("GamePanelList"));
            services.AddDbContext<GameBoardContext>(opt => opt.UseInMemoryDatabase("GameBoardList"));
            services.AddDbContext<CoordinateContext>(opt => opt.UseInMemoryDatabase("CoordinateList"));

            //logging of requests
            services.AddDbContext<LoggerContext>(opt => opt.UseInMemoryDatabase("LoggerList"));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
