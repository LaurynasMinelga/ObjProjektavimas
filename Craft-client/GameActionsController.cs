using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft_client.objects;
using Craft_client.IteratorPattern;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Drawing;

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
            Requests requests = new Requests();

            GameBoard gameboard = new GameBoard();
            var url = await requests.CreateAsync(gameboard, client, "api/GameBoards"); // add new gameboard
            HttpResponseMessage response = await client.GetAsync(url);
            GameBoard gameboard_new = await response.Content.ReadAsAsync<GameBoard>(); //get gameboardID

            response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Players/" + PlayerId); //get player
            Player player = await response.Content.ReadAsAsync<Player>();
            Player new_player = new Player
            {
                Id = player.Id,
                Name = player.Name,
                GameBoardId = gameboard_new.Id,
            };
            response = await client.PutAsJsonAsync(client.BaseAddress.PathAndQuery + "api/Players/" + $"{player.Id}", new_player); // append gameboard id
            Console.WriteLine("Gameboard_new.id = " + gameboard_new.Id);

            foreach (Ship ship in ships)
            {
                Ship ship_tmp = new Ship
                {
                    Size = ship.Size,
                    type = ship.type
                };
                url = await requests.CreateAsync(ship_tmp, client, "api/Ships"); // add new ship

                response = await client.GetAsync(url);
                Ship Ship_local = await response.Content.ReadAsAsync<Ship>(); //get ship that we added (we need shipID)

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
                        Level = GamePanel.Levels.Swamp
                    };
                    break;
                case "Sea":
                    gamepanel = new GamePanel
                    {
                        SessionId = sessionID,
                        Level = GamePanel.Levels.Sea
                    };
                    break;
                case "Space":
                    gamepanel = new GamePanel
                    {
                        SessionId = sessionID,
                        Level = GamePanel.Levels.Space
                    };
                    break;
            }

            var url = await requests.CreateAsync(gamepanel, client, "api/GamePanels"); // add gamepanel on server
            HttpResponseMessage response = await client.GetAsync(url); //get gamepanel id
            GamePanel gamepanel_new = await response.Content.ReadAsAsync<GamePanel>(); 

            return gamepanel_new.Id;
        }

        public async Task<Button[,]> UpdateBoard(HttpClient client, long gamepanelId, Button[,] button_grid_player)
        {
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Shot/" + gamepanelId); // get gameboard
            var coordinates = await response.Content.ReadAsAsync<List<Coordinate>>();
            Collection coords = new Collection();

            for (int g = 0; g < coordinates.Count(); g++)
            {
                coords[g] = new Coordinate {
                    Row = coordinates.ToArray()[g].Row,
                    Collumn = coordinates.ToArray()[g].Collumn,
                    Occupation = coordinates.ToArray()[g].Occupation
                };
            }
            Iterator iterator = coords.CreateIterator();

            for (Coordinate c = iterator.First(); !iterator.IsDone; c = iterator.Next())
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Point location = (Point)button_grid_player[i, j].Tag;
                        if (location.X == c.Row && location.Y == c.Collumn &&
                            (c.Occupation == Occupation.Destroyed || c.Occupation == Occupation.Missed))
                        {
                            button_grid_player[i, j].Enabled = false;
                            Console.WriteLine("LOCATION >>> " + location.X + location.Y);
                        }

                    }
                }
            }
            /*
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < coordinates.Count(); k++)
                    {
                        Point location = (Point)button_grid_player[i, j].Tag;
                        if (location.X == coordinates.ToArray()[k].Row && location.Y == coordinates.ToArray()[k].Collumn && 
                            (coordinates.ToArray()[k].Occupation == Occupation.Destroyed || coordinates.ToArray()[k].Occupation == Occupation.Missed))
                        {
                            button_grid_player[i, j].Enabled = false;
                            Console.WriteLine("LOCATION >>> "+location.X+location.Y);
                        }
                    }
                }
            }*/

            return button_grid_player;
        }

        public async Task<int> AwaitYourTurn(HttpClient client, long sessionID, int current_turn)
        {
            int next_turn = current_turn+1;
            while (current_turn != next_turn)
            {
                await Task.Delay(1000);
                current_turn = await SyncTurns(client, sessionID);
            }

            return current_turn;
        }

        public async Task<int> SyncTurns(HttpClient client, long sessionID)
        {
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + sessionID); //get session turn
            Session session = await response.Content.ReadAsAsync<Session>();

            return session.turn;
        }
    }
}