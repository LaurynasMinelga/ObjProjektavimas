using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Craft_client.IteratorPattern;
using Craft_client.objects;

namespace Craft_client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:19282/");
            //client.BaseAddress = new Uri("https://craft-server.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //test iterator
            Collection collection = new Collection();
            collection[0] = new Coordinate
            {
                Row = 0,
                Collumn = 0,
            };
            collection[1] = new Coordinate
            {
                Row = 0,
                Collumn = 1,
            };
            collection[2] = new Coordinate
            {
                Row = 0,
                Collumn = 2,
            };

            Iterator iterator = collection.CreateIterator();
            Console.WriteLine("Iterating over collection:");

            for (Coordinate item = iterator.First();
                !iterator.IsDone; item = iterator.Next())
            {
                Console.WriteLine(item.Collumn);
            }


            //Program entry point - initialize lobby
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Lobby(client));
        }
    }
}
