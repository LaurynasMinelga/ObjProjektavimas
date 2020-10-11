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

            }
            else // indicate user creation failure
            {
                textBox1.BackColor = Color.Red;
            }
        }


    }
}
