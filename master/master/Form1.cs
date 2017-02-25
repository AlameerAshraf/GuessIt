using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using Newtonsoft.Json;



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
        int numOfPlayers = 0;
        TcpListener Started;
        public static List<Player> players;
        bool Flag = true;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            label2.Text = numOfPlayers.ToString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

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
            button1.Text = "Running";
            button1.BackColor = System.Drawing.Color.Green;
        }
        public void MainFunction()
        {
            while (Flag)
            {
                Player Pl = new Player(Started.AcceptSocket(), listView1 , listView2);
                players.Add(Pl);
                numOfPlayers++;
                label2.Text = numOfPlayers.ToString();
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
        }
    }

    //[Serializable()]
    public class Player  //: ISerializable
    {
        Thread Th1;
        Socket Dummy;
        NetworkStream Stream;
        BinaryReader WhoReads;
        BinaryWriter WhoWrites;
        public string[] PlyersData; // 1 = Name , 2 = Password , 3 = ID , 4 = state
        public string[] RoomData;  //1 = name , 2 = level , 3 = cat , 4 = this.plyerdata[0] , 5 = anotherplyer , 6 = roomstate 
        public string PlyersStutes; 
        public string[] Row;
        public string[] Holder;
        string Container;
        public int Falg = 0; 
        List<Room> RoomsOfThePlayer;
        ListView PanelControl;
        ListView PanelControlR;  
             
        
        
        public Player(Socket ObjFromSocket , ListView Li , ListView Li2 )
        {
            Dummy = ObjFromSocket;
            PanelControl = Li;
            PanelControlR = Li2;
            RoomsOfThePlayer = new List<Room>();
            PlyersData = new string[3];
            Th1 = new Thread(Run);     
            Th1.Start(); 
        }
        public void BroadcastingPlayers(List<Player> Plyers) // O(n)
        {
            foreach (Player P in Plyers)
            {
                P.DataSender(Plyers); 
            }
        }

        public void Thing ()
        {
            while (SocketActive(Dummy))
            {
                Stream = new NetworkStream(Dummy);
                WhoReads = new BinaryReader(Stream);

                Container = WhoReads.ReadString();
                Holder = Container.Split(',');

                if (Holder[0] == "ChatMessage") //"ChatMessage"+","+PlayersName+","+Receiver+","+textBox1.Text
                {
                    ReceiveMessageToSendToAnotherClient(Holder);
                }
                else if (Holder[0] == "SendRoom")
                { //"SendRoom"+","+RoomName+","+RoomCategory+","+level.ToString()+ room satte 
                    Room NewRoom = new Room();
                    NewRoom.Owner = PlyersData[0];
                    NewRoom.RoomName = Holder[1];
                    NewRoom.RoomCat = Holder[2];
                    NewRoom.RoomLevel = Holder[3];
                    NewRoom.Player = null;
                    NewRoom.RoomStutes = Holder[4];

                    var lir = new ListViewItem(new string[] { Holder[1] , Holder[4] , PlyersData[0]});
                    PanelControlR.Items.Add(lir);

                    RoomsOfThePlayer.Add(NewRoom);
                    RoomSender(Form1.players);
                }
                else if (Holder[0] == "ChangeRoomState")
                {
                    Room NewRoom = new Room();
                    NewRoom.RoomName = Holder[1];
                    NewRoom.RoomLevel = Holder[2];
                    NewRoom.RoomCat = Holder[3];
                    NewRoom.Owner = PlyersData[0];
                    NewRoom.Player = null;
                    NewRoom.RoomStutes = Holder[6];

                    // Changing Procedure ! 
                }

                else if (Holder[0] =="JoinRoom")//"JoinRoom"+","+PlayersName+","+RoomOwner+","+RoomName
                {
                    Joinning(Holder[3], Holder[2], Holder[1]);
                }
                else if (Holder[0] == "AcceptPlay")//"AcceptPlay" + "," + RoomName + "," + ","+ requester +","+ MyClient.PlayersName + "," + gameword
                {
                    Accepting(Holder[1], Holder[3], Holder[2], Holder[4]);
                  
                }
                else if (Holder[0] == "GamePlay")//ownwer , player,score,playerstat
                {
                    ReceiveMessageToSendToAnotherClient(new string[] {  });
                }
                else if (Holder[0] == "ClickedChar")//"ClickedChar," + c.ToString() + ",1," + MyClient.PlayersName + "," + MyClient.Requester
                {
                  
                    //MessageBox.Show("at server : "+Holder[1]);
                    ReceiveMessageToSendToAnotherClient(new string[] {"OpponentClickedChar",Holder[1],Holder[4],Holder[2]});
                }
            }
        }


        public void Accepting(string RoomName, string OwnerRoom,string Requester, string gameword)
        {
            foreach (Player P in Form1.players)
            {
                if (P.PlyersData[0] == OwnerRoom)
                {
                    for (var i = 0; i < P.RoomsOfThePlayer.Count; i++)
                    {
                        if (P.RoomsOfThePlayer[i].RoomName == RoomName)
                        {
                            P.ReceiveMessageToSendToAnotherClient(new string[] { "AcceptPlay", gameword,Requester, OwnerRoom });
                        }
                    }
                }
            }
        }
        public void Joinning (string RoomName , string OwnerRoom , string Requester)
        {
            foreach (Player P in Form1.players)
            {
                if (P.PlyersData[0] == OwnerRoom)
                {
                    P.ReceivePlayRequestToSendToAnotherClient(OwnerRoom, Requester);
                }
            }
        }

        public void DataSender( List<Player> PlayersList)
        {
            string PDataSentToClients = null;
            Stream = new NetworkStream(Dummy);
            WhoWrites = new BinaryWriter(Stream);
            
            foreach (var element in PlayersList)
            { 
                PDataSentToClients += element.PlyersData[0] + "." + element.PlyersData[1] + ",";
            }
            PDataSentToClients.Substring(0, PDataSentToClients.Length);
            WhoWrites.Write(PDataSentToClients);
        }

        public void RoomSender (List<Player> PlayersList)
        {
            string RDataSentToClients = "Rooms" + ",";
            Stream = new NetworkStream(Dummy);
            WhoWrites = new BinaryWriter(Stream);
            foreach (var element in PlayersList)
            {
                for (var i = 0; i < element.RoomsOfThePlayer.Count; i++)
                {
                    RDataSentToClients += element.RoomsOfThePlayer[i].Owner + "." + element.RoomsOfThePlayer[i].RoomName + "." + element.RoomsOfThePlayer[i].RoomStutes + "." + element.RoomsOfThePlayer[i].RoomLevel + "." + RoomsOfThePlayer[i].RoomCat + ",";
                }
            }
          
            WhoWrites.Write(RDataSentToClients);
        }


        public void ConnectClient (string MessageFromAnotherClient)
        {
            //MessageBox.Show( this.PlyersData[0]);
            Stream = new NetworkStream(Dummy);
            WhoWrites = new BinaryWriter(Stream);
            WhoWrites.Write(MessageFromAnotherClient);
        }

        public void ReceiveMessageToSendToAnotherClient(string [] MessageInfromation)
        {
            MessageBox.Show("sending char to "+MessageInfromation[2]);
            string Receiver = MessageInfromation[2];
            foreach(Player P in Form1.players)
            {
                if (P.PlyersData[0] == Receiver)
                {
                    MessageBox.Show("found receiver");
                    P.ConnectClient(MessageInfromation[0]+","+ MessageInfromation[1]+","+MessageInfromation[3]);
                }
            }
        }

        public void ReceiveGamePlayToSendToAnotherClient(string[] MessageInfromation)
        {
            string Receiver = MessageInfromation[2];
            foreach (Player P in Form1.players)
            {
                if (P.PlyersData[0] == Receiver)
                {
                    for (var i = 0; i < P.RoomsOfThePlayer.Count; i++)
                    {
                        if (P.RoomsOfThePlayer[i].Player == MessageInfromation[3])
                        {
                            P.ConnectClient(MessageInfromation[0] + "," + MessageInfromation[1] + "," + MessageInfromation[3]);
                        }
                    }
                    
                }
            }
        }
        public void ReceivePlayRequestToSendToAnotherClient(string Owner , string Requester)
        {
            foreach (Player P in Form1.players)
            {
                if (P.PlyersData[0] == Owner)
                {
                    P.ConnectClient("WantToPlay"+","+Requester);
                }
            }
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
                    Container = WhoReads.ReadString();
                    Holder = Container.Split(',');
                    PlyersData[0] = Holder[0]; //name 
                    PlyersData[1] = Holder[1]; //pass
                    PlyersData[2] = "Online"; // state
                    Th1.Name = Holder[1];

                    Row = new string[] { PlyersData[0], PlyersData[2], "--BUTTON--" };
                    var lip = new ListViewItem(Row);
                    PanelControl.Items.Add(lip);

                    BroadcastingPlayers(Form1.players);
                    Thing();
                }
                catch {}
            }
            while (SocketActive(Dummy));
            PlyersData[2] = "Offline"; 
            foreach (ListViewItem item in PanelControl.Items)
            {
                if (item.SubItems[0].Text == PlyersData[0])
                {
                    item.SubItems[1].Text = PlyersData[2];
                }
            }
            Dummy.Close(); 
        }
    }

    class Room
    {
        public string RoomName;
        public string RoomLevel;
        public string RoomCat;
        public string RoomStutes;
        public string Owner;
        public string Player;
    }
}
