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
        public async Task PrepareShipsOnServer(HttpClient client, Ship[] ships, long sessionId, long PlayerId)
        {
            foreach (Ship ship in ships)
            {
                Requests requests = new Requests();
                Ship ship_tmp = new Ship
                {
                    Size = ship.Size,
                    type = ship.type
                };
                var url = await requests.CreateAsync(ship_tmp, client, "api/Ships"); // add new ship

                HttpResponseMessage response = await client.GetAsync(url);
                Ship Ship_local = await response.Content.ReadAsAsync<Ship>(); //get ship that we added (we need shipID)

                GameBoard gameboard = new GameBoard();
                url = await requests.CreateAsync(gameboard, client, "api/GameBoards"); // add new gameboard
                response = await client.GetAsync(url);
                GameBoard gameboard_new = await response.Content.ReadAsAsync<GameBoard>(); //get gameboardID

                response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Players/"+PlayerId); //get player
                Player player = await response.Content.ReadAsAsync<Player>();
                Player new_player = new Player
                {
                    Id = player.Id,
                    Name = player.Name,
                    GameBoardId = gameboard_new.Id,
                };
                response = await client.PutAsJsonAsync(client.BaseAddress.PathAndQuery + "api/Player/" + $"{player.Id}", new_player); // append gameboard id

                for (int i = 0; i < ship.Size; i++)
                {
                    Coordinate coord = new Coordinate
                    {
                        ShipId = Ship_local.Id,
                        Row = ship.Row[i],
                        Collumn = ship.Collumn[i],
                        Occupation = Occupation.Ship,
                        GameBoardId = gameboard_new.Id,
                    };
                    url = await requests.CreateAsync(coord, client, "api/Coordinates"); // add new ship piece
                }
            }
        }

        /// <summary>
        /// Get enemy game board id to identify which coordinates belong to the enemy
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sessionID"></param>
        /// <param name="PlayerId"></param>
        /// <param name="which"></param>
        /// <returns></returns>
        public async Task<long> GetGameBoardId(HttpClient client,long sessionID, long PlayerId, string which)
        {
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + sessionID);
            Session session = await response.Content.ReadAsAsync<Session>();

            if (which == "Enemy")
            {
                if (session.PlayerOneId == PlayerId)
                {
                    response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Players/" + session.PlayerTwoId);

                }
                else
                {
                    response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Players/" + session.PlayerOneId);
                }
            } else
            {
                if (session.PlayerOneId == PlayerId)
                {
                    response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Players/" + session.PlayerOneId);

                }
                else
                {
                    response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Players/" + session.PlayerTwoId);
                }
            }

            Player player = await response.Content.ReadAsAsync<Player>();

            long gameboardId = player.GameBoardId;

            return gameboardId;
        }

        public async Task<long> CreateGamePanel(HttpClient client, string level_name, long sessionID){
            
            Requests requests = new Requests();
            GamePanel gamepanel = new GamePanel
            {
                SessionId = sessionID,
                Level = GamePanel.Levels.Desert
            };
            switch (level_name)
            {
                case "Desert":
                    gamepanel = new GamePanel
                    {
                        SessionId = sessionID,
                        Level = GamePanel.Levels.Desert
                    };
                    break;
                case "Swamp":
                    gamepanel = new GamePanel
                    {
                        SessionId = sessionID,
                        Level = GamePanel.Levels.Desert
                    };
                    break;
                case "Sea":
                    gamepanel = new GamePanel
                    {
                        SessionId = sessionID,
                        Level = GamePanel.Levels.Desert
                    };
                    break;
                case "Space":
                    gamepanel = new GamePanel
                    {
                        SessionId = sessionID,
                        Level = GamePanel.Levels.Desert
                    };
                    break;
            }

            var url = await requests.CreateAsync(gamepanel, client, "api/GamePanels"); // add gamepanel on server
            HttpResponseMessage response = await client.GetAsync(url); //get gamepanel id
            GamePanel gamepanel_new = await response.Content.ReadAsAsync<GamePanel>(); 

            return gamepanel_new.Id;
        }


    }
}