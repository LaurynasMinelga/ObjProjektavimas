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
    public partial class Lobby : Form
    {
        static HttpClient client;
        private bool CancelButton_Clicked = false;
        public Lobby(HttpClient _client)
        {
            InitializeComponent();

            client = _client;
        }

        //click start
        private async void button1_Click(object sender, EventArgs e)
        {

            RadioButton radioBtn = this.Controls.OfType<RadioButton>()
                                       .Where(x => x.Checked).FirstOrDefault(); // get checked radio button

            string player_name = textBox1.Text; // player name
            string level_name = radioBtn.Text; // level name

            Player Player = new Player
            {
                Name = player_name
            };

            Console.WriteLine("Creating new player");
            Requests requests = new Requests();
            var url =  await requests.CreateAsync(Player, client, "api/Players"); // create new player
            Console.WriteLine($"Created at {url}");

            // If created successfully - proceed with session creation.
            if (url != null)
            {
                ICollection<Session> sessions = null;
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + "api/Sessions"); 
                sessions = await response.Content.ReadAsAsync<ICollection<Session>>();

                if (sessions.Count != 0) //if there are sessions active
                {
                    bool open_session_exists = true;
                    foreach (Session session in sessions)
                    {
                        if (session.PlayerTwoId == 0) // if an open session exists (someone awaits other players)
                        {
                            response = await client.GetAsync(url);
                            Player = await response.Content.ReadAsAsync<Player>();
                            Session new_session = new Session
                            {
                                Id = session.Id,
                                turn = 0,
                                PlayerOneId = session.PlayerOneId,
                                PlayerTwoId = Player.Id
                            };
                            response = await client.PutAsJsonAsync(client.BaseAddress.PathAndQuery + "api/Sessions/" + $"{session.Id}", new_session); // add second player

                            //initiate game
                            Initiate_Game(new_session.Id, level_name, Player.Id);
                            break;
                        }
                        open_session_exists = false;
                    }
                    if (!open_session_exists) // if open session does not exist
                    {
                        await CreateNewSession(url, level_name);
                    }
                }
                else //create new session
                {
                    await CreateNewSession(url, level_name);
                }
            }
            else // indicate user creation failure
            {
                textBox1.BackColor = Color.Red;
            }
        }
        // Initiate MainGame window and set level color
        private void Initiate_Game(long sessionID, string level_name, long PlayerID)
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
            this.Hide();
        }

        /// <summary>
        /// Create new game session and initiate MainGame window
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task CreateNewSession(Uri url, string level_name)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            Player Player = await response.Content.ReadAsAsync<Player>();
            Session session = new Session
            {
                turn = 0,
                PlayerOneId = Player.Id,
                PlayerTwoId = 0
            };
            Console.WriteLine("Creating new session");
            Requests requests = new Requests();
            url = await requests.CreateAsync(session, client, "api/Sessions"); // create new session
            Console.WriteLine($"Created at {url}");

            panel1.Visible = true; // enable matchmaking panel visibility

            while (session.PlayerTwoId == 0)
            {
                await Task.Delay(1000);
                response = await client.GetAsync(url);//client.BaseAddress.PathAndQuery + "api/Sessions/"+ $"{session.Id}");
                session = await response.Content.ReadAsAsync<Session>();
                Console.WriteLine("session id" + session.Id);
                //Task.WaitAll(Task.Delay(1000));

                if (CancelButton_Clicked)
                {
                    HttpStatusCode status = await requests.DeleteAsync(client, "api/Sessions/", session.Id);
                    panel1.Visible = false;
                    break; // break if user cancels the matchmaking
                }
            }

            if (session.PlayerTwoId != 0)
            {
                Initiate_Game(session.Id, level_name, session.PlayerOneId);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CancelButton_Clicked = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Lobby_Load(object sender, EventArgs e)
        {

        }
    }
}
