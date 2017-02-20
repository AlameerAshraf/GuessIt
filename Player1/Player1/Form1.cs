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
       
        TcpClient Player;
        NetworkStream Stream;
        BinaryReader Reader;
        BinaryWriter Writer;
        string PlayersName;
        string Receiver;
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
                    if (Pairs[0] != "ChatMessage")
                    {
                        for (int i = 0; i < Pairs.Length - 1; i++)
                        {
                            if (!comboBox1.Items.Contains(Pairs[i].Split('.')[1]))
                            {
                                comboBox1.Items.Add(Pairs[i].Split('.')[0]);
                                listView1.Items.Add(Pairs[i].Split('.')[0], Pairs[i].Split('.')[1]);//name,status
                            }
                        }
                    }
                    else if (Pairs[0] == "ChatMessage")
                    {
                        textBox2.Text += Pairs[1] + ": " + Pairs[2]; //name: msg
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

        private void button1_Click(object sender, EventArgs e) //create room button
        {
            Room rForm = new Room();
            rForm.ShowDialog();
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

    }
}