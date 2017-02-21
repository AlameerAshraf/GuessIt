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
            MyClient.ShowOtherForm += action;
        }

        private void action(object sender, EventArgs e)
        {
            
            MessageBox.Show("Hello");
        }

        private void Room_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        { // Onlin , Complete , Destroyed 
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
            MyClient.Writer.Write("SendRoom"+","+RoomName+","+RoomCategory+","+level.ToString()+","+RoomSt);
        }

    }
}
