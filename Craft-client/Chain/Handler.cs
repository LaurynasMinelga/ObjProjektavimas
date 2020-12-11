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
    abstract class Handler
    {
        protected Handler successor;

        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }

        public abstract Task InitiateMatchmaking(string level_name,Lobby current, Uri url, HttpClient client);
    }
}
