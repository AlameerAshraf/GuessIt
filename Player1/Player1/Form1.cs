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
using System.Reflection;

//aaa
namespace Player1
{
    public partial class Form1 : Form
    {
        public delegate void delPassData(string Requester);
        public delegate void word();

        public TcpClient Player;
        public NetworkStream Stream;
        public BinaryReader Reader;
        public BinaryWriter Writer;
        Room newRoom;
        public string PlayersName;
        public string Receiver;
        public string RoomName;
        public string RoomOwner;
        public string Requester;
        public string gameword;
        public string anotherplyer;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            textBox3.Text = PlayersName;
            Player = new TcpClient();
            byte[] bt = new byte[] { 127, 0, 0, 1 };
            IPAddress localaddress = new IPAddress(bt);
            Player.Connect(localaddress, 9999);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;  
        }

        public void NamesLoader()
        {
            Reader = new BinaryReader(Stream);
            string PNames = null;
            string[] Pairs;
            while(true)
            {
                try
                {
                    PNames = Reader.ReadString();
                    Pairs = PNames.Split(',');
                    if (Pairs[0] != "ChatMessage" && Pairs[0] != "Rooms" && Pairs[0] != "WantToPlay" && Pairs[0] != "AcceptPlay")
                    {
                        for (int i = 0; i < Pairs.Length - 1; i++)
                        {
                            if (!comboBox1.Items.Contains(Pairs[i].Split('.')[0]))
                            {
                                comboBox1.Items.Add(Pairs[i].Split('.')[0]);
                                string n = Pairs[i].Split('.')[0];
                                string ns = Pairs[i].Split('.')[1];
                                var list = new ListViewItem(new string[] { n, ns });
                                listView1.Items.Add(list);//name,status
                            }
                        }
                    }
                    else if (Pairs[0] == "ChatMessage")
                    {
                        textBox2.Text += Pairs[1] + ": " + Pairs[2]; //name: msg
                    }
                    else if (Pairs[0] == "Rooms")
                    {
                        string[] allroom = new string[Pairs.Length];
                        listView2.Items.Clear();
                        for (int i = 1; i < Pairs.Length - 1; i++)
                        {
                            allroom = Pairs[i].Split('.');
                            string owner = allroom[1];
                            string n = allroom[0];
                            string stat = allroom[2];
                            string le = allroom[3];
                            string cat = allroom[4];

                            ListViewItem list2 = new ListViewItem(new string[] { owner, stat, cat, le, n, "" });
                            listView2.Items.Add(list2);
                        }
                    }
                    else if (Pairs[0] == "WantToPlay")
                    {
                        DialogResult Res = MessageBox.Show("Player :" + Pairs[1] + "Wants To Play With You In Your Room", "Join Request", MessageBoxButtons.YesNo);
                        if (Res == DialogResult.Yes)
                        {
                            Requester = Pairs[1];
                            delPassData del = new delPassData(newRoom.ActionToTakeFromUser);
                            del(Requester);
                        }
                        else
                        {
                            MessageBox.Show("You Cancelled The Request , Wait For another one!");
                        }
                    }
                    else if (Pairs[0] == "AcceptPlay")
                    {
                        gameword = Pairs[1];
                        anotherplyer = Pairs[2];
                        //The "invoke" call tells the form "Please execute this code in your thread rather than mine."
                        //Executes the specified delegate on the thread that owns the control's underlying window handle.
                        this.Invoke(
                           new MethodInvoker(delegate ()
                           {
                               Room RequestedRoom = new Room(this, 1);
                               RequestedRoom.Show();
                           }));

                    }
                }
                catch { }
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
           
        }



    

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Stream = Player.GetStream();
                Writer = new BinaryWriter(Stream);
                Writer.Write(textBox5.Text + "," + textBox6.Text);// 1 = name , 2 = password
                //Condition DataBase !
                PlayersName = textBox5.Text; 
                panel1.Visible = false;
                textBox3.Text = PlayersName;
                Thread Naming = new Thread(NamesLoader);
                Naming.Start();
            }
            catch
            {
                MessageBox.Show("Server Is Offline");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Player.Close();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string ChatMessage;
            Writer = new BinaryWriter(Stream);
            ChatMessage = "ChatMessage"+","+PlayersName+","+Receiver+","+textBox1.Text;

            textBox2.Text += "Me:"+textBox1.Text;
            textBox2.AppendText(Environment.NewLine);

            Writer.Write(ChatMessage); 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true; 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
            Receiver = comboBox1.SelectedItem.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //create new room 
            newRoom = new Room(this,0);
            newRoom.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(listView1.SelectedItems[0].ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //ShowOtherForm(this, new EventArgs());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //join button 
            Writer = new BinaryWriter(Stream);
            Writer.Write("JoinRoom"+","+PlayersName+","+RoomOwner+","+RoomName);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            ////watch button 
            //Writer = new BinaryWriter(Stream);
            //Writer.Write("WatchRoom" + "," + PlayersName + "," + "," + "owner" + "," + "RoomName");
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            RoomName = listView2.SelectedItems[0].SubItems[0].Text;
            RoomOwner = listView2.SelectedItems[0].SubItems[4].Text;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}