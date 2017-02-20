using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace server
{
    public partial class KeyBoard : UserControl
    {
       public  char Button_letter;
        string game_word;//get from server
        char[] label_stirng;
        int wrong_score;
        Label Label1;
        Form1 MyForm;
        public KeyBoard(Form1 f)
        {
            InitializeComponent();
            MyForm = f;
            game_word = f.game_word;
            Label1 = new Label();
            Label1.Parent = this;
            game_word = f.game_word.ToUpper();
            int len = game_word.Length;
            label_stirng = new char[len];
            for (int x = 0; x < len; x++)
            {
                label_stirng[x] = '-';
                //Label1.Text = "sss";
                Label1.Text += label_stirng[x] + "  ";
            }
            int i = 20;
            for (char c = 'A'; c <= 'I'; c++)
            {
                Button b = new Button();
                b.BackColor = Color.LightSeaGreen;
                b.Name = c.ToString();
                b.Left = i + 30;
                i += 50;
                b.Top = 20;
                b.Width = 30;
                b.Text = c.ToString();
                b.Parent = this;
                b.Click += (s, e) =>
                {
                    b.Enabled = false;
                    b.BackColor = Color.Gray;
                    Button_letter = char.Parse(b.Name);
                    Draw_letter();
                    Sender();
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
                b.Top = 50;
                b.Width = 30;
                b.Text = c.ToString();
                b.Parent = this;
                b.Click += (s, e) =>
                {
                    b.Enabled = false;
                    b.BackColor = Color.Gray;
                    Button_letter =char.Parse(b.Name);
                    Draw_letter();
                    Sender();
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
                b.Top = 80;
                b.Width = 30;
                b.Text = c.ToString();
                b.Parent = this;
                b.Click += (s, e) =>
                {
                    b.Enabled = false;
                    b.BackColor = Color.Gray;
                    Button_letter = char.Parse(b.Name);
                    Draw_letter();
                    Sender();
                };
               
            }
        }
        public void Draw_letter()
        {
            //MessageBox.Show(Button_letter.ToString());
            for(int i=0;i<game_word.Length;i++)
            {
                if(Button_letter==game_word[i])
                {
                    label_stirng[i] = Button_letter;
                    Label1.Text = "";
                    for (int j = 0; j < game_word.Length; j++)
                    {
                        Label1.Text += label_stirng[j];
                    }
                }
            }
        }
        public void Select_Button(char dimmed_button)
        {
          
         foreach ( Control btn in this.Controls )
            {
                if (btn is Button)
                {
                    if (btn.Name == dimmed_button.ToString())
                    {
                        btn.Enabled = false;
                    }
                }
            }
        }
        public void Sender()
        {
            char let = Button_letter;
            this.MyForm.wr.Write("play"+","+let);
        }
        }
    }

