using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft_client.objects;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Craft_client
{
    class GameActionsController
    {
        public async Task<int> InitializeGame(HttpClient client, long sessionId, long playerId)
        {
            
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + $"{sessionId}");
            bool ready = false;

            while (ready)
            {
                await Task.Delay(1000);
                response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + $"{sessionId}");
                Session session = await response.Content.ReadAsAsync<Session>();
                Console.WriteLine("session id" + session.Id);

                if (session.PlayerOneReady == true && session.PlayerTwoReady == true) // both ready
                {
                    if (session.PlayerOneId == playerId)
                    {
                        ready = true;
                        return 3; //player goes first
                    } else
                    {
                        ready = true;
                        return 2; //enemy goes first
                    }
                }
            }
            return 1; // stuck 
        }
    }
}
