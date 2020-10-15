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
        static long SessionId;
        static string level_name;

        //10x10 board of buttons a.k.a single game board
        Button[,] button_grid_player = new Button[10, 10];
        Button[,] button_grid_enemy = new Button[10, 10];

        ShipPlacementController PlaceShip = new ShipPlacementController(); // ship placement methods

        public MainGame(HttpClient _client, long _sessionId, string _level_name)
        {
            InitializeComponent();
            populateGameBoard();

            client = _client;
            SessionId = _sessionId;
            level_name = _level_name;
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
        private void Grid_Button_Click_Enemy(object sender, EventArgs e)
        {
            //get the row and collumn
            Button clickedButton = (Button)sender;
            Point location = (Point)clickedButton.Tag;

            int row = location.X; //row
            int collumn = location.Y; //collumn

            //shoot

            //get information about the coordinate we shot at
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

            //place ships

            Disable_Nearby_Buttons(row, collumn, clicked_Button);

            // log where and what ship was placed
            
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
            }
            
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
        /// After ship placement, start the battle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
