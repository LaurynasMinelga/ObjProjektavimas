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
        /// <summary>
        /// Set player as ready for game - ships are placed. Await another player.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sessionId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<int> InitializeGame(HttpClient client, long sessionId, long playerId)
        {
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + $"{sessionId}"+"/ready/"+$"{playerId}");
            Session session = await response.Content.ReadAsAsync<Session>();
            bool ready = false;

            while (!ready)
            {
                await Task.Delay(1000);
                response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + $"{sessionId}");
                session = await response.Content.ReadAsAsync<Session>();

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

        /// <summary>
        /// Register placed ships on server
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ships"></param>
        /// <param name="sessionId"></param>
        /// <param name="PlayerId"></param>
        /// <returns></returns>
        public async Task PrepareShipsOnServer(HttpClient client, Ship ships, long sessionId, long PlayerId)
        {
            
        }
    }
}