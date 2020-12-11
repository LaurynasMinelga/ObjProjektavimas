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
    class Initializer : Handler
    {
        public override async Task InitiateMatchmaking(string level_name, Lobby current, Uri url, HttpClient client)
        {
            MatchmakingController control = new MatchmakingController();
            //control.Initiate_Game(current, client, session.Id, level_name, session.PlayerOneId);
            if (successor != null)
            {
                successor.InitiateMatchmaking(level_name, current, url, client);
            }
        }
    }
}
