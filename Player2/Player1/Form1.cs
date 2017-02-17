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
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Players;


namespace Player1
{
    public partial class Form1 : Form
    {
        TcpClient Player;
        NetworkStream Stream;
        BinaryReader Reader;
        BinaryWriter Writer;
        string PlayersName = "Sara Ramadan";
        public Form1()
        {
            InitializeComponent();
            textBox3.Text = PlayersName;

            Player = new TcpClient();
            byte[] bt = new byte[] { 127, 0, 0, 1 };
            IPAddress localaddress = new IPAddress(bt);
            Player.Connect(localaddress, 9999);
            try
            {
                Stream = Player.GetStream();
                Writer = new BinaryWriter(Stream);
                Writer.Write(PlayersName);
            }
            catch
            {
                MessageBox.Show("Connection Error");
            }
            finally
            {
                this.Refresh();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Reader = new BinaryReader(Stream);
            BinaryFormatter bin = new BinaryFormatter();

            var IncomingPlyer = (List<Player>)bin.Deserialize(Stream);
            foreach(var i in IncomingPlyer)
            {
                MessageBox.Show(i.ToString()); 
            }

        }
    }
}