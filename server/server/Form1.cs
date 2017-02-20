using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
namespace server
{
    public partial class Form1 : Form
    {
        TcpListener server;
        IPAddress ip;
        public Socket connection;
        public NetworkStream nstream;
        public BinaryReader dr;
        public BinaryWriter wr;
       public string game_word = "HELLO";
        string[] label_stirng;
        int flag;//to choose which player have the role
        public Form1()
        {
            InitializeComponent();
            byte[] bt = new byte[] { 127, 0, 0, 1 };
            ip = new IPAddress(bt);
            server = new TcpListener(ip, 6666);
            server.Start();
            connection = server.AcceptSocket();
            nstream = new NetworkStream(connection);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //send the word of game to other client(player)  
            wr = new BinaryWriter(nstream);
            wr.Write("word" + "," + game_word);
            // check of position of this command to be ensure that public (game_word) we get it 
            KeyBoard KB = new KeyBoard(this);
            //KB.Button_letter;
            //KB.Select_Button('M');
            KB.Parent = this.panel1;
        }             
        }
    }

