using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Player1
{
    public partial class Room : Form
    {
        Form1 MyClient; 
        string RoomName;
        string RoomCategory;
        int level;

        public Room(Form1 IncomingForm)
        {
            InitializeComponent();
            MyClient = IncomingForm;
            MyClient.ShowOtherForm += ActionToTakeFromUser;
        }

        private void ActionToTakeFromUser(object sender, EventArgs e)
        {
            label1.Text = this.MyClient.Requester;
            MessageBox.Show(this.MyClient.Requester);
        }

        private void Room_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
            {
                RoomName = textBox1.Text;
                RoomCategory = comboBox1.SelectedItem.ToString();
                if (radioButton1.Checked)
                    level = 1;
                else if (radioButton2.Checked)
                    level = 2;
                else
                    level = 3;
            }
            else
            {
                MessageBox.Show("Insert Data To Start Game !");
            }


            string RoomSt = "Online";
            MyClient.Writer = new System.IO.BinaryWriter(MyClient.Stream);
            MyClient.Writer.Write("SendRoom" + "," + RoomName + "," + RoomCategory + "," + level.ToString() + "," + RoomSt);
            panel1.Visible = false;
        }
    }
}
