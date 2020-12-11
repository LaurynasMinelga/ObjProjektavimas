using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Craft_client
{
    class MatchmakingController
    {
        // Initiate MainGame window and set level color
        public void Initiate_Game(Lobby current,HttpClient client, long sessionID, string level_name, long PlayerID)
        {
            MainGame MainGame = new MainGame(client, sessionID, level_name, PlayerID);
            switch (level_name)
            {
                case "Desert":
                    MainGame.BackgroundImage = Properties.Resources.Desert;
                    break;
                case "Swamp":
                    MainGame.BackgroundImage = Properties.Resources.swamp;
                    break;
                case "Sea":
                    MainGame.BackgroundImage = Properties.Resources.sea;
                    break;
                case "Space":
                    MainGame.BackgroundImage = Properties.Resources.space;
                    break;
            }
            MainGame.Show();
            current.Hide();
        }
    }
}
