using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Craft_client.objects;

namespace Craft_client
{
    public partial class MainGame : Form
    {
        static HttpClient client;
        static long SessionId; // Current session id
        static string level_name; // level string (Desert, Sea, Swamp, Space)
        static long PlayerId; //current player id
        static long EnemyGameBoardId; // Enemy gameboard id (for shooting straight)
        static long PlayerGameBoardId; // Player gameboard id (to Power up your ships)
        static long GamePanelId; // Game panel ID (it holds our level parameters)
        static int current_turn;

        static int GameState = 0; // Game state = 0 (deployment) / 1 (started, but waiting for enemy deployment) / 2 (waiting for enemy move) / 3 (player turn)

        static Ship[] ships = new Ship[10]; // Unsent ships log
        static int unsent_ships_count = 0; // count of ships already logged <- once 10 - initiate game

        //10x10 board of buttons a.k.a single game board
        Button[,] button_grid_player = new Button[10, 10];
        Button[,] button_grid_enemy = new Button[10, 10];

        ShipPlacementController PlaceShip = new ShipPlacementController(); // ship placement methods
        GameActionsController Game = new GameActionsController(); // game actions methods

        public MainGame(HttpClient _client, long _sessionId, string _level_name, long _playerID)
        {
            InitializeComponent();
            populateGameBoard();

            client = _client;
            SessionId = _sessionId;
            level_name = _level_name;
            PlayerId = _playerID;
        }

        private void MainGame_Load(object sender, EventArgs e)
        {
            panel2.Enabled = false; // Enemy panel disabled \
            button1.Enabled = false; // block button         until ships are placed
            label4.Visible = true; // Label visible         /
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        ///  Populate panel 1 and Panel 2 with buttons a.k.a gameboard
        /// </summary>
        private void populateGameBoard()
        {
            int buttonSize = panel1.Width / 10;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //initiate buttons and set their size
                    button_grid_enemy[i, j] = new Button();
                    button_grid_enemy[i, j].Height = buttonSize;
                    button_grid_enemy[i, j].Width = buttonSize;

                    button_grid_player[i, j] = new Button();
                    button_grid_player[i, j].Height = buttonSize;
                    button_grid_player[i, j].Width = buttonSize;

                    //physically add click events
                    button_grid_player[i, j].Click += Grid_Button_Click_Player;
                    button_grid_enemy[i, j].Click += Grid_Button_Click_Enemy;

                    //add buttons on the panel
                    panel1.Controls.Add(button_grid_player[i, j]);
                    panel2.Controls.Add(button_grid_enemy[i, j]);

                    //set button location on the panel
                    button_grid_player[i, j].Location = new Point(i * buttonSize, j * buttonSize);
                    button_grid_enemy[i, j].Location = new Point(i * buttonSize, j * buttonSize);

                    //set button.tag as id of a button with coordinates
                    button_grid_player[i, j].Tag = new Point(i, j);
                    button_grid_enemy[i, j].Tag = new Point(i, j);
                }
            }

        }

        /// <summary>
        /// Click events for each button on enemy panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Button_Click_Enemy(object sender, EventArgs e)
        {
            //get the row and collumn
            Button clickedButton = (Button)sender;
            Point location = (Point)clickedButton.Tag;

            int row = location.X; //row
            int collumn = location.Y; //collumn

            //shoot
            clickedButton.BackColor = Color.Black;
            panel2.Enabled = false;
            panel1.Enabled = false;
            label4.Text = "Opponents turn!";
            Coordinate coordinate_tmp = new Coordinate
            {
                Row = row,
                Collumn = collumn
            };
            Requests requests = new Requests();
            var hit = await client.PostAsJsonAsync(client.BaseAddress.PathAndQuery + "api/Shot/" + EnemyGameBoardId, coordinate_tmp); // Register a new shot
            Console.WriteLine("\n\nshot content: "+hit.StatusCode);
            if (hit.StatusCode == HttpStatusCode.OK)
            {
                clickedButton.BackColor = Color.Red;
            }

            //get information about the coordinate we shot at
            button_grid_enemy = await Game.UpdateBoard(client, EnemyGameBoardId, button_grid_enemy);

            //Register turn
            current_turn++;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + $"{SessionId}");
            Session session_tmp = await response.Content.ReadAsAsync<Session>();
            session_tmp.turn = session_tmp.turn + 1;
            response = await client.PutAsJsonAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + $"{SessionId}", session_tmp);

            //await another player
            current_turn = await Game.AwaitYourTurn(client, SessionId, current_turn);
            // ready
            panel2.Enabled = true;
            panel1.Enabled = true;
            label4.Text = "Your turn!";
        }

        /// <summary>
        /// click events for each button on player panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Button_Click_Player(object sender, EventArgs e)
        {
            //get the row and collumn
            Button clicked_Button = (Button)sender; //cast button object
            Point location = (Point)clicked_Button.Tag; //Get button tag (we added coords to button tag)
            int row = location.X; //row
            int collumn = location.Y; //collumn

            //place ships while game state = 0 (deployment)
            if (GameState == 0)
            {
                Disable_Nearby_Buttons(row, collumn, clicked_Button); // log where and what ship was placed
            } else if (GameState == 3) // if player turn
            {
                //enable special skills
            }
            
        }

        /// <summary>
        /// Disable nearby buttons - ships can not touch
        /// Cruiser - 1 window (max 4 ships)
        /// Submarine - 2 windows (max 3 ships)
        /// Warship - 3 windows (max 2 ships)
        /// Aircarrier - 4 windows (max 1 ship)
        /// </summary>
        private void Disable_Nearby_Buttons(int row, int collumn, Button clicked_Button)
        {
            RadioButton radio_button = panel3.Controls.OfType<RadioButton>()
                                       .Where(x => x.Checked).FirstOrDefault(); // get checked radio button

            if (radio_button != null)
            {
                switch (radio_button.Text)
                    {
                        case "Cruiser":
                            Disable_Radio_Buttons(radio_button, 0);
                            button_grid_player = PlaceShip.Cruiser_Placed(row, collumn, button_grid_player);
                            clicked_Button.BackColor = Color.Aqua;
                        break;

                        case "Submarine":
                            Disable_Radio_Buttons(radio_button, 1);
                            button_grid_player = PlaceShip.Submarine_Placed(row, collumn, button_grid_player);
                            clicked_Button.BackColor = Color.Azure;
                            break;

                        case "Warship":
                            Disable_Radio_Buttons(radio_button, 2);
                            button_grid_player = PlaceShip.Warship_Placed(row, collumn, button_grid_player);
                            clicked_Button.BackColor = Color.Bisque;
                            break;

                        case "Aircarrier":
                            Disable_Radio_Buttons(radio_button, 3);
                            button_grid_player = PlaceShip.Aircarrier_Placed(row, collumn, button_grid_player);
                            clicked_Button.BackColor = Color.BlueViolet;
                            break;
                    }
                PlaceShip.Last_step_count++; //placement was made, coordinates of placement will be added to Last_row and Last_collumn
                Add_Ship_To_Log(row, collumn, radio_button.Text);
                Check_If_Ships_Created();
            }
            
        }
        /// <summary>
        /// Check if all chips were created 
        /// </summary>
        private void Check_If_Ships_Created()
        {
            if (unsent_ships_count == 10)
            {
                button1.Enabled = true; // enable start button
                label4.Text = "All ready! Press start:"; // Label visible
            }
        }

        /// <summary>
        /// Assign ship object for sending to server
        /// </summary>
        /// <param name="row"></param>
        /// <param name="collumn"></param>
        /// <param name="text"></param>
        private void Add_Ship_To_Log(int row, int collumn, string ship_type)
        {

            if (ships[unsent_ships_count]==null)// check if object exists
            {
                Initialize_Ship_Object();
            }
            switch (ship_type)
            {
                case "Cruiser":
                    ships[unsent_ships_count] = SetShip(row, collumn, ship_type);
                    unsent_ships_count++; //ship created
                    break;

                case "Submarine":
                    if (ships[unsent_ships_count].Size == 0)
                    {
                        ships[unsent_ships_count] = SetShip(row, collumn, ship_type);
                    }
                    else
                    {
                        ships[unsent_ships_count] = SetShip(row, collumn, ship_type);
                        unsent_ships_count++; //ship created
                    }
                    break;

                case "Warship":
                    if (ships[unsent_ships_count].Size == 0 || ships[unsent_ships_count].Size == 1)
                    {
                        ships[unsent_ships_count] = SetShip(row, collumn, ship_type);
                    }
                    else
                    {
                        ships[unsent_ships_count] = SetShip(row, collumn, ship_type);
                        unsent_ships_count++; //ship created
                    }
                    break;

                case "Aircarrier":
                    if (ships[unsent_ships_count].Size == 0 || ships[unsent_ships_count].Size == 1 || ships[unsent_ships_count].Size == 2)
                    {
                        ships[unsent_ships_count] = SetShip(row, collumn, ship_type);
                    }
                    else
                    {
                        ships[unsent_ships_count] = SetShip(row, collumn, ship_type);
                        unsent_ships_count++; //ship created
                    }
                    break;
            }
        }

        private Ship SetShip(int row, int collumn, string ship_type)
        {
            ships[unsent_ships_count].type = ship_type;
            ships[unsent_ships_count].Row[ships[unsent_ships_count].Size] = row;
            ships[unsent_ships_count].Collumn[ships[unsent_ships_count].Size] = collumn;
            ships[unsent_ships_count].Size++;
            
            return ships[unsent_ships_count];
        }

        private void Initialize_Ship_Object()
        {
                ships[unsent_ships_count] = new Ship
                {
                    Row = new int[] { 0, 0, 0, 0 },
                    Collumn = new int[] { 0, 0, 0, 0 },
                    type = null,
                    Size = 0
                };
        }

        /// <summary>
        /// Disable radio buttons limiting number of ships available
        /// </summary>
        private void Disable_Radio_Buttons(RadioButton radio_button, int ship_number)
        {
            PlaceShip.Ship_Count[ship_number]--;
            if (PlaceShip.Ship_Count[ship_number] == 0)
            {
                radio_button.Enabled = false;
                radio_button.Checked = false;
            }
            label5.Text = PlaceShip.Ship_Count[0].ToString();
            label6.Text = (PlaceShip.Ship_Count[1] / 2).ToString();
            label7.Text = (PlaceShip.Ship_Count[2] / 3).ToString();
            label8.Text = (PlaceShip.Ship_Count[3] / 4).ToString();
        }

        /// <summary>
        /// After ship placement, start the battle - send ships log to server and wait for other player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {
            GameState = 1;

            button1.Text = "Loading";
            button1.Enabled = false; // enable start button
            label4.Text = "Waiting for other player"; // Label visible

            //Create GamePanel
            GamePanelId = await Game.CreateGamePanel(client, level_name, SessionId);

            //send ships to server
            await Game.PrepareShipsOnServer(client, ships, SessionId, PlayerId);

            //check if another player is ready
            GameState = await Game.InitializeGame(client, SessionId, PlayerId);

            //get enemy and player game board ids
            EnemyGameBoardId = await Game.GetGameBoardId(client, SessionId, PlayerId, "Enemy");
            PlayerGameBoardId = await Game.GetGameBoardId(client, SessionId, PlayerId, "Player");

            Console.WriteLine("GAMEBOARD ID: "+EnemyGameBoardId+ " - player: "+PlayerGameBoardId);

            if (GameState == 2) // enemy turn
            {
                panel1.Enabled = false;
                panel2.Enabled = false;
                button1.Visible = false;
                label4.Text = "Opponents turn!";
                current_turn = await Game.SyncTurns(client, SessionId);

                //await another player
                current_turn = await Game.AwaitYourTurn(client, SessionId, current_turn);
                // ready
                panel2.Enabled = true;
                panel1.Enabled = true;
                label4.Text = "Your turn!";

            } else if (GameState == 3) // player turn
            {
                panel2.Enabled = true;
                panel1.Enabled = true;
                button1.Visible = false;
                label4.Text = "Your turn!";
                current_turn = await Game.SyncTurns(client, SessionId);
            }
        }
    }
}
