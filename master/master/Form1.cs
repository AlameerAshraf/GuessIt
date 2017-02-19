using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

    

/* /*
      async void connection()
        {
            Player p1 = new Player(await Started.AcceptSocketAsync(), listView1);
            players.Add(p1);
            connection();
        }
 /* */

// rec online !
// message 
// owneer   


namespace master
{
    public partial class Form1 : Form
    {
        TcpListener Started;
        List<Player> players;
        
        bool Flag = true;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; 
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public void BroadcastingPlayers() // O(n)
        {
            foreach ( Player P in players )
            {
                P.DataSender(players);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            byte[] Ip = new byte[] { 127, 0, 0, 1 };
            IPAddress Iaddress = new IPAddress(Ip);
            Started = new TcpListener(Iaddress, 9999);
            Started.Start();
            players = new List<Player>(); 
            Thread TH2 = new Thread(MainFunction);
            TH2.Start();
        }
        public void MainFunction()
        {
            while (Flag)
            {
                Player Pl = new Player(Started.AcceptSocket(), listView1,RoomlistView);
                players.Add(Pl);
                BroadcastingPlayers();
            }  
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            Flag = false; 
            Started.Stop(); 
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //foreach (Player pl in players)
            //{
            //    MessageBox.Show(pl.Row[1]);
            //    MessageBox.Show(pl.PlyersStutes); 
            //}
        }
    }

    [Serializable()]
    public class Player : ISerializable
    {
        Thread Th1;
        Socket Dummy;
        NetworkStream Stream;
        BinaryReader WhoReads;
        BinaryWriter WhoWrites;
        public string PlayersName;
        public int PlyersId;
        public string PlyersStutes; 
        //string PlayersPassword;
        public string[] Row;
        //List<Player> InternalPlyersList; 
        ListView PanelControl;
        ListView RoomListView;
        int check = 0;
        List<CreatedRoom> RoomList;

        public Player(Socket ObjFromSocket , ListView Li ,ListView RList)
        {
            Dummy = ObjFromSocket;
            PanelControl = Li;
            RoomListView = RList;
            Th1 = new Thread(Run);     
            Th1.Start(); 
        }

        public void DataSender( List<Player> PlayersList)
        {
            Stream = new NetworkStream(Dummy);
            WhoWrites = new BinaryWriter(Stream);
            BinaryFormatter List = new BinaryFormatter();
            List.Serialize(Stream, PlayersList); 
        }


        public void ConnectClient (string MessageFromAnotherClient)
        {
            Stream = new NetworkStream(Dummy);
            WhoWrites = new BinaryWriter(Stream);
            WhoWrites.Write(MessageFromAnotherClient);
        }

        public void ReceiveMessageToSendItToAnotherClient()
        {
            Stream = new NetworkStream(Dummy);
            WhoReads = new BinaryReader(Stream);

        }

        public bool SocketActive (Socket s)
        {
            bool ac1 = s.Poll(1000, SelectMode.SelectRead);
            bool ac2 = (s.Available == 0);
            if (ac1 && ac2)
                return false;
            else
                return true;
        }
        public void Run()
        { 
            Stream = new NetworkStream(Dummy);
            WhoReads = new BinaryReader(Stream);
            do
            {
                try
                {
                    if(check==0)
                    {
                        PlayersName = WhoReads.ReadString();
                        PlyersId = 1;
                        PlyersStutes = "Online";
                        Th1.Name = PlayersName;
                        Row = new string[] { PlyersId.ToString(), PlayersName, PlyersStutes };
                        var li = new ListViewItem(Row);
                        PanelControl.Items.Add(li);

                        check = 1;
                    }
                    else
                    {
                        //recieving room object
                        string s = WhoReads.ReadString();
                        var room = Newtonsoft.Json.JsonConvert.DeserializeObject<CreatedRoom>(s);
                        string[] Row = new string[] { room.Name_Player,PlayersName, "ON", room.Category_Player, room.difficultly_Player.ToString() };  
                        var li = new ListViewItem(Row);
                        RoomListView.Items.Add(li);

                        //broadcast new room to all players 
                        WhoWrites = new BinaryWriter(Stream);
                        CreatedRoom ss = new CreatedRoom { Category_Player = room.Category_Player, difficultly_Player = room.difficultly_Player, Name_Player = room.Name_Player };
                        WhoWrites.Write(JsonConvert.SerializeObject(ss));       
                    }
                    
                    
                }
                catch 
                {
                   // MessageBox.Show("EndOfStream Exception Handling");
                   // We have to pass it one time to get the things Rigth
                }
            }
            while (SocketActive(Dummy));
            PlyersStutes = "Offline";
            foreach (ListViewItem item in PanelControl.Items)
            {
                if (item.SubItems[1].Text== PlayersName)
                {
                  item.SubItems[2].Text = PlyersStutes;
                }
            }

            Dummy.Close(); 

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PlyerName", PlayersName);
            info.AddValue("PlyerId", PlyersId);
            info.AddValue("PlyerStutes", PlyersStutes); 
        }
        public Player(SerializationInfo info, StreamingContext ctxt)
        {
            PlayersName = (string)info.GetValue("PlyerName", typeof(string));
            PlyersId = (int)info.GetValue("PlyerId", typeof(int));
            PlyersStutes = (string)info.GetValue("PlyerStutes", typeof(string));
        }
    }
    public class CreatedRoom
    {
        public string Category_Player { get; set; }
        public int difficultly_Player { get; set; }
        public string Name_Player { get; set; }
    }

}
