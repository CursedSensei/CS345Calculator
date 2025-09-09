using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CS345Calculator
{
    public partial class Calculator : Form
    {
        private string displayedText = "";
        private bool decimalMode = false;

        public Calculator()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void CommonClick(object sender, EventArgs e)
        {
            string buttonText = ((Button)sender).Text;

            if (buttonText.StartsWith(" "))
            {
                if (
                    displayedText.Length == 0 ||
                    displayedText.EndsWith(" .") ||
                    (displayedText.Length == 1 && displayedText[0] == '.')
                )
                {
                    return;
                }
                else if (displayedText.EndsWith(" "))
                {
                    displayedText = displayedText.Remove(displayedText.Length - 3, 3);
                }

                decimalMode = false;
            }
            else if (buttonText == ".")
            {
                if (decimalMode) return;

                decimalMode = true;
            }

            displayedText += buttonText;
            displayBox.Text = displayedText;
        }

        private void ClearEntryClick(object sender, EventArgs e)
        {
            if (displayedText.Length == 0) return;

            char lastChar = displayedText[displayedText.Length - 1];

            switch (lastChar)
            {
                case '.':
                    decimalMode = false;
                    break;
                case ' ':
                    displayedText = displayedText.Remove(displayedText.Length - 3, 3);
                    displayBox.Text = displayedText;
                    return;
            }

            displayedText = displayedText.Remove(displayedText.Length - 1, 1);
            displayBox.Text = displayedText;
        }

        private void ClearClick(object sender, EventArgs e)
        {
            displayedText = "";
            displayBox.Text = "";
            decimalMode = false;
        }

        private void KeyboardHandler(object sender, KeyPressEventArgs e)
        {
            string key = e.KeyChar.ToString();

            if ("0987654321+-*/.".Contains(key))
            {
                if (!Char.IsNumber(e.KeyChar) && e.KeyChar != '.')
                {
                    if (key == "*")
                    {
                        key = "x";
                    }

                    key = ' ' + key + ' ';
                }

                Button objectHolder = new Button();
                objectHolder.Text = key;

                CommonClick(objectHolder, null);
            }
        }

        private void EqualClick(object sender, EventArgs e)
        {
            if (displayedText.EndsWith(" "))
            {
                displayedText = displayedText.Remove(displayedText.Length - 3, 3);
                displayBox.Text = displayedText;
            }

            if (displayedText.Length == 0) return;

            string[] equationParts = displayedText.Split(' ');
            List<object> resultParts = new List<object>(equationParts.Length / 2);
            char op = '\0';

            foreach (string i in equationParts)
            {
                if (op != '\0')
                {
                    if (op == 'x')
                    {
                        resultParts[resultParts.Count - 1] = (double)resultParts[resultParts.Count - 1] * Double.Parse(i);
                    }
                    else
                    {
                        resultParts[resultParts.Count - 1] = (double)resultParts[resultParts.Count - 1] / Double.Parse(i);
                    }

                    op = '\0';
                }
                else
                {
                    try
                    {
                        resultParts.Add(Double.Parse(i));
                    } catch (FormatException)
                    {
                        if (i[0] == 'x' || i[0] == '/')
                        {
                            op = i[0];
                        }
                        else
                        {
                            resultParts.Add(i[0]);
                        }
                    }
                }
            }

            double result = (double) resultParts[0];
            resultParts.RemoveAt(0);

            foreach (object i in resultParts)
            {
                if (op != '\0')
                {
                    if (op == '+')
                    {
                        result = result + (double)i;
                    }
                    else
                    {
                        result = result - (double)i;
                    }

                    op = '\0';
                }
                else
                {
                    op = (char) i;
                }
            }

            // RAAAAAAAAAAA!!!!!!!!!!!!!!!!!!!!!
            displayedText = result.ToString("0.###################################");
            displayBox.Text = displayedText;
        }
    }
}
