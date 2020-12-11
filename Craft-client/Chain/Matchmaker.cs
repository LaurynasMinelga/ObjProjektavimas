using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Craft_client.objects;
using Craft_client;

namespace Craft_client.Chain
{
    class Matchmaker: Handler
    {
        public override async Task InitiateMatchmaking(string level_name, Lobby current,Uri url, HttpClient client)
        {
            ICollection<Session> sessions = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Sessions");
            sessions = await response.Content.ReadAsAsync<ICollection<Session>>();

            bool open_session_exists = true;
            if (sessions.Count != 0)
            {
                foreach (Session session in sessions)
                {
                    if (session.PlayerTwoId == 0) // if an open session exists (someone awaits other players)
                    {
                        response = await client.GetAsync(url);
                        Player Player = await response.Content.ReadAsAsync<Player>();
                        Session new_session = new Session
                        {
                            Id = session.Id,
                            turn = 0,
                            PlayerOneId = session.PlayerOneId,
                            PlayerTwoId = Player.Id,
                            PlayerOneReady = false,
                            PlayerTwoReady = false
                        };
                        response = await client.PutAsJsonAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + $"{session.Id}", new_session); // add second player

                        //initiate game
                        MatchmakingController control = new MatchmakingController();
                        control.Initiate_Game(current,client, new_session.Id, level_name, Player.Id);
                        break;
                    }
                    open_session_exists = false;
                }
            } else
            {
                open_session_exists = false;
            }

            if (successor != null && !open_session_exists)
            { 
               await successor.InitiateMatchmaking(level_name, current, url, client);
            }
        }
    }
}
