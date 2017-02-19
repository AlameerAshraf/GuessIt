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
    public partial class Rooms : Form
    {
        string Name1, Category;
        int difficultly;
        public string Name_Player
        {
            get
            {
                return Name1;
            }

            set
            {
                Name1 = value;
            }
        }

        public string Category_Player
        {
            get
            {
                return Category;
            }

            set
            {
                Category = value;
            }
        }

        public int difficultly_Player
        {
            get
            {
                return difficultly;
            }

            set
            {
                difficultly = value;
            }
        }

        public Rooms()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Name1 = textBox1.Text;
            Category = comboBox1.Text;
            if (radioButton1.Checked == true)
                difficultly = 1;
            else if (radioButton2.Checked == true)
                difficultly = 2;
            else
                difficultly = 3;
            this.Close();
        }

        private void Rooms_Load(object sender, EventArgs e)
        {

        }
    }
}
