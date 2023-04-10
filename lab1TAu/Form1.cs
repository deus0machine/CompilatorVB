using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace lab1TAu
{
    public partial class Form1 : Form
    {
        AnalyseWorker worker;
        List<Token> tokens;
        public Form1()
        {
            worker = new AnalyseWorker();
            InitializeComponent();
            //textBox1.Text = "for i := 10 to 100 do y := i + x ;";
            textBox2.Text = @"C:\Users\User\Desktop\scriptforlab.txt";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            worker.Analyse(textBox1);
            button3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            worker = new AnalyseWorker(textBox2.Text);
            worker.ReadFile(textBox3);
            MessageBox.Show("Opening successfully!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str;
            string type;
            Token token;
            tokens = new List<Token>();
            for (int i = 0; i < worker.buf.Count; i++)
            {
                str = (worker.buf[i].Split(' ')[0]);
                type = (worker.buf[i].Split(' ')[1]);
                if (type == "I")
                {
                    if (Token.IsSpecialWord(str))
                    {
                        token = new Token(Token.SpecialWords[str]);
                        tokens.Add(token);
                        continue;
                    }
                    else
                    {
                        token = new Token(Token.TokenType.IDENTIFIER);
                        token.Value = str;
                        tokens.Add(token);
                        continue;
                    }
                }
                else if (type == "D")
                {
                    token = new Token(Token.TokenType.LITERAL);
                    token.Value = str;
                    tokens.Add(token);
                    continue;
                }
                else if (type == "R")
                {
                    token = new Token(Token.SpecialSymbols[str[0]]);
                    tokens.Add(token);
                    continue;
                }
            }
            Token.PrintTokens(textBox1, tokens);
            button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AnalyseTokens AT = new AnalyseTokens(tokens);
            AT.Start();
            if (AT.Succes == true)
                MessageBox.Show("Analyse succesful!");
            else
                MessageBox.Show("Analyse failed...");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            worker = new AnalyseWorker(textBox2.Text);
            worker.ReadBox(textBox3, textBox1);
            MessageBox.Show("Reading successfully!");
        }
    }
}
