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
using master;
using Newtonsoft.Json;


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
            Control.CheckForIllegalCrossThreadCalls = false; 
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
            Thread Naming = new Thread(NamesLoader);
            Naming.Start();

            Thread AllRooms = new Thread(RoomLoader);
            AllRooms.Start();
        }
        public void NamesLoader()
        {
            while (true)
            {
                try
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    var IncomingPlyer = (List<Player>)bin.Deserialize(Stream);
                    foreach (var i in IncomingPlyer)
                    {
                        if (i.PlyersStutes == "Online" && !comboBox1.Items.Contains(i.PlayersName))
                        {
                            comboBox1.Items.Add(i.PlayersName);
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Loading Players Will Take Seconds");
                }
                catch { } //Io Exceptopn . Server Closed and client expect for incoming data !
            }
        }

        public void RoomLoader()
        {
            while (true)
            {
                try
                {
                    //recieving room object
                    string s = Reader.ReadString();
                    var room = Newtonsoft.Json.JsonConvert.DeserializeObject<CreatedRoom>(s);
                    string []Row = new string[] { room.Name_Player,PlayersName, "?",room.Category_Player,room.difficultly_Player.ToString() };
                    MessageBox.Show(Row[0]+Row[1]+ Row[2]+ Row[3]);
                    var li = new ListViewItem(Row);
                    RoomlistView.Items.Add(li);
                }
                catch (NullReferenceException)
                {
                    //No rooms created yet
                }
                catch { }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Reader = new BinaryReader(Stream);
         

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Player.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rooms new_room = new Rooms();
            new_room.ShowDialog();
            ////new writer & with json object    
            Writer = new BinaryWriter(Stream);
            CreatedRoom ss = new CreatedRoom { Category_Player = new_room.Category_Player , difficultly_Player = new_room.difficultly_Player , Name_Player = new_room.Name_Player };
            Writer.Write(JsonConvert.SerializeObject(ss));
        }

        private void RoomlistView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    public class CreatedRoom
    {
        public string Category_Player { get; set; }
        public int difficultly_Player { get; set; }
        public string Name_Player { get; set; }
    }
}