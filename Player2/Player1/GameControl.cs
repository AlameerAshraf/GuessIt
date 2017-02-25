using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Player1
{
    public partial class GameControl : UserControl
    {
        Form1 MyClient;
        public char clicked_btn { set; get; }

        string Gameword;
        string[] arrayOfLabel;
        public GameControl(string word, Form1 IncomingForm)
        {
            InitializeComponent();
            Gameword = word.ToUpper();
            MyClient = IncomingForm;
        }

        private void GameControl_Load(object sender, EventArgs ex)
        {
            arrayOfLabel = new string[Gameword.Length];
            label1.Text = "";
            //=====================
            for (int x = 0; x < Gameword.Length; x++)
            {
                label1.Text += "_ ";
                arrayOfLabel[x] = "_ ";
            }
            //=====================
            int i = 20;
            for (char c = 'A'; c <= 'I'; c++)
            {
                Button b = new Button();
                b.BackColor = Color.LightSeaGreen;
                b.Name = c.ToString();
                b.Left = i + 30;
                i += 50;
                b.Top = 50;
                b.Width = 30;
                b.Text = c.ToString();
                b.Parent = this.panel1;
                b.Click += (s, e) =>
                {
                    b.Enabled = false;
                    b.BackColor = Color.Gray;
                    clicked_btn = c;
                    CheckChoosenLetter(char.Parse(b.Name));

                };
            }
            i = 20;
            for (char c = 'J'; c <= 'R'; c++)
            {
                Button b = new Button();
                b.BackColor = Color.LightSeaGreen;
                b.Name = c.ToString();
                b.Left = i + 30;
                i += 50;
                b.Top = 80;
                b.Width = 30;
                b.Text = c.ToString();
                b.Parent = this.panel1;
                b.Click += (s, e) =>
                {
                    b.Enabled = false;
                    b.BackColor = Color.Gray;
                    clicked_btn = c;
                    CheckChoosenLetter(char.Parse(b.Name));
                };
            }
            i = 20;
            for (char c = 'S'; c <= 'Z'; c++)
            {
                Button b = new Button();
                b.BackColor = Color.LightSeaGreen;
                b.Name = c.ToString();
                b.Left = i + 30;
                i += 50;
                b.Top = 110;
                b.Width = 30;
                b.Text = c.ToString();
                b.Parent = this.panel1;
                b.Click += (s, e) =>
                {
                    b.Enabled = false;
                    b.BackColor = Color.Gray;
                    clicked_btn = c;
                    CheckChoosenLetter(char.Parse(b.Name));


                };
            }
        }

        public void DisableChar(char c )
        {
            MessageBox.Show("gowa DisbChar fel Control");
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl.Name == c.ToString())
                {
                    MessageBox.Show("ctrl name " + ctrl.Name);
                    ctrl.Enabled = false;
                    ctrl.BackColor = Color.Gray;
                    //====
                    for (int i = 0; i < Gameword.Length; i++)
                    {
                        if (Gameword[i] == c)
                        {
                            arrayOfLabel[i] = c.ToString();
                        }

                    }
                    PrintLabel(arrayOfLabel);
                    //====
                    break;
                } 
            }
        }

        public void CheckChoosenLetter(char c)
        {
            int flag = 0;
            for (int i = 0; i < Gameword.Length; i++)
            {
                if (Gameword[i] == c)
                {
                    flag = 1;
                    arrayOfLabel[i] = c.ToString();
                }

            }
           
            if (flag == 1)
            {
                PrintLabel(arrayOfLabel);
                if(MyClient.Requester==null)
                {
                    MyClient.Writer.Write("ClickedChar," + c.ToString() + ",1," + MyClient.PlayersName + "," + MyClient.anotherplyer);
                }
                else
                {
                    MyClient.Writer.Write("ClickedChar," + c.ToString() + ",1," + MyClient.PlayersName + "," + MyClient.Requester);
                }
               
               
            }

            else
            {
                MessageBox.Show("wrong");
                if (MyClient.Requester == null)
                {
                    MyClient.Writer.Write("ClickedChar," + c.ToString() + ",0," + MyClient.PlayersName + "," + MyClient.anotherplyer);
                }
                else
                {
                    MyClient.Writer.Write("ClickedChar," + c.ToString() + ",0," + MyClient.PlayersName + "," + MyClient.Requester);
                }

                //MyClient.Writer.Write("ClickedChar," + c.ToString() + ",0," + MyClient.PlayersName + "," + MyClient.Requester);
                //MyClient.Writer.Write("ClickedChar," + c.ToString() + ",1," + MyClient.PlayersName + "," + MyClient.anotherplyer);
            }
        }

        public void PrintLabel(string[] arrayOfLabel)
        {
            label1.Text = "";
            for (int j = 0; j < arrayOfLabel.Length; j++)
            {
                label1.Text += arrayOfLabel[j];
            }
            if (arrayOfLabel.Contains("_ "))
            { }
            else
                MessageBox.Show("THE END");
        }
    }
}
