using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace lab1TAu
{
    class AnalyseWorker
    {
        string path;
        string str = "for i := 10 to 100 do y := i + x;";
        public List<string> buf = new List<string>();
        //string separators = "+=:;-*\n><";
        char[] _separators = { '=', '(', ')', '*', '/', '-', '+', ':', ';', '\n' , '<' , '>'};
        string[] lines = new string[1];

        public AnalyseWorker(string path)
        {
            this.path = path;
        }
        public AnalyseWorker()
        {
            
        }
        public void ReadFile(System.Windows.Forms.TextBox textbox)
        {
            StreamReader file = new StreamReader(path);
            int count = File.ReadAllLines(path).Length;
            lines = new string[count];
            int i = 0;
            textbox.Text = "";
            while (!file.EndOfStream)
            {
                lines[i] = file.ReadLine() + '\n';
                textbox.Text += lines[i];
                textbox.Text += Environment.NewLine;
                i++;
            }
            file.Close();
        }
        public void ReadBox(System.Windows.Forms.TextBox textbox, System.Windows.Forms.TextBox textbox2)
        {
            int count = textbox.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length;
            String[] s = textbox.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            bool end = false;
            lines = new string[count];
            int i = 0;
            textbox2.Text = "";
            while (i != count)
            {
                lines[i] = s[i] + '\n';
                textbox2.Text += lines[i];
                textbox2.Text += Environment.NewLine;
                i++;

            }
        }

        public void Analyse(System.Windows.Forms.TextBox textbox)
        {
            string bufEl = "";
            int numBuf = 0;
            bool I = false;
            bool D = false;
            textbox.Text = "";
            for (int j = 0; j < lines.Length; j++)
            {
                str = lines[j];
                for (int i = 0; i < str.Length; i++)
                {

                    if ((isLetter(str[i]) && D == false))
                    {
                        bufEl += str[i];
                        I = true;
                        continue;
                    }
                    else if (isLetter(str[i]) && D == true)
                    {
                        bufEl += str[i];
                        continue;
                    }
                    else if (isNumber(str[i]) && I != true)
                    {
                        bufEl += str[i];
                        D = true;
                        continue;
                    }
                    else if ((isNumber(str[i]) && I == true))
                    {
                        bufEl += str[i];
                        continue;
                    }
                    else if (isSeparator(str[i]))
                    {
                        if (bufEl.Length > 0)
                        {
                           if (I)
                                buf.Add(bufEl += " I ");
                           if (D)
                               buf.Add(bufEl += " D ");
                           bufEl = "";
                        }
                        if (str[i] == '\n')
                        {
                            bufEl = "# R ";
                            buf.Add(bufEl);
                            numBuf++;
                            bufEl = "";
                            I = false;
                            D = false;
                            continue;
                        }
                            numBuf++;
                            bufEl += str[i] + " R ";
                            buf.Add(bufEl);
                            bufEl = "";
                            numBuf++;
                        
                    }
                    else if (str[i] == ' ')
                    {
                        if (I)
                        {
                            buf.Add(bufEl += " I ");
                            bufEl = "";
                        }
                        if (D)
                        {
                            buf.Add(bufEl += " D ");
                            bufEl = "";
                        }
                        numBuf++;
                    }

                    I = false;
                    D = false;
                }
            }
            foreach(object o in buf)
            {
                textbox.Text += o.ToString();
                textbox.Text += Environment.NewLine;
            }
        }
        public bool isLetter(char c)
        {
            string z = "" + c;
            return Regex.IsMatch(z, "^[a-zA-Z]+$");
        }
        public bool isNumber(char c)
        {
            string z = "" + c;
            return Regex.IsMatch(z, "^[0-9]+$");
        }
        public bool isSeparator(char c)
        {
            return _separators.Contains(c);
        }   
    }
}
