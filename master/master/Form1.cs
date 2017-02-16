using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;



/* /*
      async void connection()
        {
            Player p1 = new Player(await Started.AcceptSocketAsync(), listView1);
            players.Add(p1);
            connection();
        }
 /* */

namespace master
{
    public partial class Form1 : Form
    {
        TcpListener Started;
        List<Player> players;
        bool Flag = true;
        string[] arr; 
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; 
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
            Thread TH2 = new Thread(Oops);
            TH2.Start();
        }
        public void Oops()
        {
            while (Flag)
            {
                Player Pl = new Player(Started.AcceptSocket(), listView1);
            }  
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            Flag = false; 
            Started.Stop(); 
        }
    }

    public class Player
    {
        Thread Th1;
        Socket Dummy;
        NetworkStream Stream;
        BinaryReader WhoReads;
        BinaryWriter WhoWrites;
        string PlayersName;
        string PlayersPassword;
        string[] Row;
        ListView PanelControl;
        
        public Player(Socket ObjFromSocket , ListView Li  )
        {
            Dummy = ObjFromSocket;
            PanelControl = Li; 
            Th1 = new Thread(Run);     
            Th1.Start(); 
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
                    PlayersName = WhoReads.ReadString();
                    Th1.Name = PlayersName;
                    MessageBox.Show(PlayersName);
                    Row = new string[] { "1", PlayersName, "Online" };
                    var li = new ListViewItem(Row);
                    PanelControl.Items.Add(li);
                }
                catch
                {
                    MessageBox.Show("oops");
                }
            }
            while (SocketActive(Dummy));

            foreach (ListViewItem item in PanelControl.Items)
            {
                if (item.SubItems[1].Text== PlayersName)
                {
                  item.SubItems[2].Text = "Offline";
                }
            }

            Dummy.Close(); 

        }
    }
}
