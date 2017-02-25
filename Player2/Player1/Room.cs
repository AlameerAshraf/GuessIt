using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

namespace Player1
{
    public partial class Room : Form
    {
        Form1 MyClient; 
        string RoomName;
        string RoomCategory;
        int level;
        int RoomTypeflag;
        string Gameword;
        string[] arrayOfLabel;
        GameControl GC;

        public Room(Form1 IncomingForm, int Flag)
        {
            InitializeComponent();
            MyClient = IncomingForm;
            Control.CheckForIllegalCrossThreadCalls = false;
            RoomTypeflag = Flag;
        }

        //===========================
        public void TurnEnable(int t)
        {
            MessageBox.Show("enable of req_room");
            if (t==1)
                this.panel2.Enabled = true;
            else
                this.panel2.Enabled = false;
        }

        public void TurnEnable2(int t)
        {
            MessageBox.Show("enable of new_room");
            if (t == 1)
                this.panel2.Enabled = true;
            else
                this.panel2.Enabled = false;
        }
        //============================
        public void DimmChar(char c)
        {
            MessageBox.Show("gowa DimmedChar");
            GC.DisableChar(c);
        }
        //============================
        public void ActionToTakeFromUser(string s)
        {
            string Wordtosent = GettingTheRandomGameWord();
            owner.Text = MyClient.PlayersName;
            plyerone.Text = s;
            label4.Text = Wordtosent;
            //=============
            try
            {
                this.Invoke(
                           new MethodInvoker(delegate ()
                           {
                               GameControl GC = new GameControl(Wordtosent, MyClient);
                               GC.Parent = this.panel2;
                               GC.Top = 80;
                               GC.Enabled = true;
                           }));
            }
            catch(Exception e) { MessageBox.Show("Control ERROr "+e); }     
            //=============

            // MessageBox.Show(Wordtosent);
            MyClient.Writer = new System.IO.BinaryWriter(MyClient.Stream);
            MyClient.Writer.Write("AcceptPlay"+","+RoomName+","+s+","+MyClient.PlayersName+","+Wordtosent);
        }



        public string GettingTheRandomGameWord()
        {
            string GameLevel = null;
            string Worditself = null;
            string connections = @"Data Source=.;Initial Catalog=project;Integrated Security=True";
            SqlConnection con = new SqlConnection(connections);

            SqlCommand cmd = new SqlCommand("get_rand", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = RoomCategory;
            if (level == 1)
            {
                GameLevel = "easy";
            }
            else if (level == 2)
            {
                GameLevel = "medium";
            }
            else if (level == 3)
            {
                GameLevel = "hard";
            }
            cmd.Parameters.Add("@level", SqlDbType.VarChar).Value = GameLevel;

            con.Open();

            SqlParameter Go = cmd.Parameters.Add("@word", SqlDbType.VarChar);
            Go.Direction = ParameterDirection.ReturnValue;
            SqlDataReader Word = cmd.ExecuteReader();
            try
            {
                while (Word.Read())
                {
                    Worditself = Word.GetString(0);
                }
            }
            catch
            {
                Worditself = "Alameer";
            }
            
            return Worditself;
        }
        private void Room_Load(object sender, EventArgs e)
        {
            if (RoomTypeflag == 1)
            {
                StartingGame();
            }
            else { }
        }
        public void StartingGame()
        {
            panel1.Visible = false;
            owner.Text = MyClient.PlayersName;
            plyerone.Text = MyClient.anotherplyer;
            label4.Text = MyClient.gameword;
            //==========
            GC = new GameControl(MyClient.gameword, MyClient);
            GC.Parent = this.panel2;
            GC.Top = 80;
            TurnEnable(0); //disables player2's room
            //==========
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
