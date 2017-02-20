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
        string RoomName;
        string RoomCategory;
        int level;

        //public string RoomName1
        //{
        //    get
        //    {
        //        return RoomName;
        //    }

        //    set
        //    {
        //        RoomName = value;
        //    }
        //}

        //public string RoomCategory1
        //{
        //    get
        //    {
        //        return RoomCategory;
        //    }

        //    set
        //    {
        //        RoomCategory = value;
        //    }
        //}

        //public int Level
        //{
        //    get
        //    {
        //        return level;
        //    }

        //    set
        //    {
        //        level = value;
        //    }
        //}

        public Room()
        {
            InitializeComponent();
        }

        private void Room_Load(object sender, EventArgs e)
        {
            RoomName = textBox1.Text;
            RoomCategory = comboBox1.SelectedText;
            if (radioButton1.Checked)
                level = 1;
            else if (radioButton2.Checked)
                level = 2;
            else
                level = 3;
        }
    }
}
