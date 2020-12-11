using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Craft_client.objects;

namespace Craft_client.Chain
{
    class Delayer : Handler
    {
        public override async Task InitiateMatchmaking(string level_name, Lobby current,Uri url, HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            Player Player = await response.Content.ReadAsAsync<Player>();
            Session session = new Session
            {
                turn = 0,
                PlayerOneId = Player.Id,
                PlayerTwoId = 0,
                PlayerOneReady = false,
                PlayerTwoReady = false
            };
            Console.WriteLine("Creating new session");
            Requests requests = new Requests();
            url = await requests.CreateAsync(session, client, "api/Sessions"); // create new session
            Console.WriteLine($"Created at {url}");

            current.panel1.Visible = true; // enable matchmaking panel visibility

            while (session.PlayerTwoId == 0)
            {
                await Task.Delay(1000);
                response = await client.GetAsync(url);//client.BaseAddress.PathAndQuery + "api/Sessions/"+ $"{session.Id}");
                session = await response.Content.ReadAsAsync<Session>();
                Console.WriteLine("session id" + session.Id);
                //Task.WaitAll(Task.Delay(1000));

                if (current.CancelButton_Clicked)
                {
                    HttpStatusCode status = await requests.DeleteAsync(client, "api/Sessions/", session.Id);
                    current.panel1.Visible = false;
                    break; // break if user cancels the matchmaking
                }
            }

            if (session.PlayerTwoId != 0)
            {
                MatchmakingController control = new MatchmakingController();
                control.Initiate_Game(current, client, session.Id, level_name, session.PlayerOneId);
            } else if (successor != null && session.PlayerTwoId != 0)
            {
                await successor.InitiateMatchmaking(level_name, current,url, client);
            }
        }
    }
}
