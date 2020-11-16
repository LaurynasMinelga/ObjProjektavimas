using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Craft_server.Models;
using Craft_server.Builder;

namespace Craft_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Test builder
            SpecialAttackBuilder builder;
            SpecialAttack special = new SpecialAttack();

            //construct new special attacks
            builder = new AirCarrierBuilder();
            special.Construct(builder);
            Console.WriteLine("BUILDER IS BUILT: " + builder.Attack.Gun1+" rear gun: "+builder.Attack.Gun2);

            builder = new WarShipBuilder();
            special.Construct(builder);
            Console.WriteLine("BUILDER IS BUILT: " + builder.Attack.Gun1 + " rear gun: " + builder.Attack.Gun2);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
