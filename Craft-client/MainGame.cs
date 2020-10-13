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

        // Ship count for {cruiser, submarine, warship, aircarrier} by windows they occupy
        static int[] Ship_Count = new int[4] { 4, 6, 6, 4 };
        // Last ship placed
        static int Last_Row = 0; static int Last_Collumn = 0;

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
            panel2.Enabled = false;
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

                    //set tag as id of a button
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
            Button clickedButton = (Button)sender; //cast button object
            Point location = (Point)clickedButton.Tag; //Get button tag (we added coords to button tag)
            int row = location.X; //row
            int collumn = location.Y; //collumn

            //place ships

            Disable_Nearby_Buttons(row, collumn);

            // log where and what ship was placed
            
        }

        /// <summary>
        /// Disable nearby buttons - ships can not touch
        /// Cruiser - 1 window (max 4 ships)
        /// Submarine - 2 windows (max 3 ships)
        /// Warship - 3 windows (max 2 ships)
        /// Aircarrier - 4 windows (max 1 ship)
        /// </summary>
        private void Disable_Nearby_Buttons(int row, int collumn)
        {
            RadioButton radio_button = panel3.Controls.OfType<RadioButton>()
                                       .Where(x => x.Checked).FirstOrDefault(); // get checked radio button

            switch (radio_button.Text)
            {
                case "Cruiser":
                    Disable_Radio_Buttons(radio_button, 0);
                    Cruiser_Placed(row, collumn);
                    break;

                case "Submarine":
                    Disable_Radio_Buttons(radio_button, 1);
                    Submarine_Placed(row, collumn);
                    break;

                case "Warship":
                    Disable_Radio_Buttons(radio_button, 2);

                    break;

                case "Aircarrier":
                    Disable_Radio_Buttons(radio_button, 3);

                    break;
            }
        }

        /// <summary>
        /// Disable buttons around Submarine for player board - ships can not touch
        /// </summary>
        /// <param name="row"></param>
        /// <param name="collumn"></param>
        private void Submarine_Placed(int row, int collumn)
        {
            if (Ship_Count[1]%2 != 0 ) // if ship placement was NOT started
            {
                if (row != 9 && collumn != 9) { button_grid_player[row + 1, collumn + 1].Enabled = false; }
                if (row != 9 && collumn != 0) { button_grid_player[row + 1, collumn - 1].Enabled = false; }
                if (row != 0 && collumn != 9) { button_grid_player[row - 1, collumn + 1].Enabled = false; }
                if (row != 0 && collumn != 0) { button_grid_player[row - 1, collumn - 1].Enabled = false; }

                Last_Row = row; Last_Collumn = collumn;
            } else //ship placement was started - end the placement
            {
                if (row != 9 && collumn != 9) { button_grid_player[row + 1, collumn + 1].Enabled = false; }
                if (row != 9) { button_grid_player[row + 1, collumn].Enabled = false; }
                if (row != 9 && collumn != 0) { button_grid_player[row + 1, collumn - 1].Enabled = false; }
                if (row != 0 && collumn != 9) { button_grid_player[row - 1, collumn + 1].Enabled = false; }
                if (row != 0) { button_grid_player[row - 1, collumn].Enabled = false; }
                if (row != 0 && collumn != 0) { button_grid_player[row - 1, collumn - 1].Enabled = false; }
                if (collumn != 9 ) { button_grid_player[row, collumn + 1].Enabled = false; }
                if (collumn != 0 ) { button_grid_player[row, collumn - 1].Enabled = false; }
            }
        }

        /// <summary>
        /// Disable buttons around cruiser for player board - ships can not touch
        /// </summary>
        /// <param name="row"></param>
        /// <param name="collumn"></param>
        private void Cruiser_Placed(int row, int collumn)
        {
            if (row != 9 && collumn != 9) {button_grid_player[row + 1, collumn + 1].Enabled = false; }
            if (row != 9) { button_grid_player[row + 1, collumn].Enabled = false; }
            if (row != 9 && collumn != 0) {button_grid_player[row + 1, collumn - 1].Enabled = false; }
            if (row != 0 && collumn != 9) {button_grid_player[row - 1, collumn + 1].Enabled = false; }
            if (row != 0) {button_grid_player[row - 1, collumn].Enabled = false; }
            if (row != 0 && collumn != 0) { button_grid_player[row - 1, collumn - 1].Enabled = false; }
            if (collumn != 9) {button_grid_player[row, collumn + 1].Enabled = false; }
            if (collumn != 0) {button_grid_player[row, collumn - 1].Enabled = false; }
        }

        /// <summary>
        /// Disable radio buttons limiting number of ships available
        /// </summary>
        private void Disable_Radio_Buttons(RadioButton radio_button, int ship_number)
        {
            Ship_Count[ship_number]--;
            if (Ship_Count[ship_number] == 0)
            {
                radio_button.Enabled = false;
                radio_button.Checked = false;
            }
            label5.Text = Ship_Count[0].ToString();
            label6.Text = (Ship_Count[1] / 2).ToString();
            label7.Text = (Ship_Count[2] / 3).ToString();
            label8.Text = Ship_Count[3].ToString();
        }

    }
}
