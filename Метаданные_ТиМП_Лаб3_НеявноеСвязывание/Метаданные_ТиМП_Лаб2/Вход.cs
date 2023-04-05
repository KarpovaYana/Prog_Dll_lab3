using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Метаданные_ТиМП_Лаб2
{
    
    public partial class Вход : Form
    {
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr hwnd, IntPtr proccess);
        [DllImport("user32.dll")] static extern IntPtr GetKeyboardLayout(uint thread);

        bool checkCapsLock = false;
        bool checkEngRus = false;
        public Вход()
        {
            InitializeComponent();
            IntPtr foregroundWindow = GetForegroundWindow();
            uint foregroundProcess = GetWindowThreadProcessId(foregroundWindow, IntPtr.Zero);
            int keyboardLayout = GetKeyboardLayout(foregroundProcess).ToInt32() & 0xFFFF;

            if (keyboardLayout == 1033)
            {
                labelEngRus.Text = "Язык ввода Английский";
                checkEngRus = true;
            }
            else labelEngRus.Text = "Язык ввода Русский";

            if (Console.CapsLock)
            {
                checkCapsLock = true;
                labelCapsLock.Text = "Клавиша CapsLock нажата";
            }
            else labelCapsLock.Text = "Клавиша CapsLock не нажата";
        }

        private void checkKey(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.CapsLock)
                checkCapsLock=!checkCapsLock;
            if (checkCapsLock)
                labelCapsLock.Text = "Клавиша CapsLock нажата";
            else labelCapsLock.Text = "Клавиша CapsLock не нажата";

            if (e.Alt && e.Shift)
                checkEngRus = !checkEngRus;
            if(checkEngRus)
                labelEngRus.Text = "Язык ввода Английский";
            else labelEngRus.Text = "Язык ввода Русский";
        }

        private string GetPassword(string InfLine)
        {
            bool flagFirstSpace = false;
            
            foreach (var letter in InfLine)
            {
                if (flagFirstSpace)
                {
                    if (letter != ' ') continue;
                    else
                    {
                        string nameInfLine = InfLine.Substring(InfLine.IndexOf(' ')+1);
                        InfLine = InfLine.Replace(nameInfLine, "");
                        InfLine = InfLine.Remove(InfLine.Length - 1, 1);
                        break;
                    }
                }
                if (letter == ' ')
                {
                    InfLine = InfLine.Remove(0, 1);
                    flagFirstSpace = true;
                    continue;
                }
                InfLine = InfLine.Remove(0, 1);
            }
            return InfLine;
        }

        private string GetFileName(string InfLine)
        {
            for(int i=0; i<2; i++)
            {
                InfLine = InfLine.Substring(InfLine.IndexOf(' ') + 1);
            }
            return InfLine;
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (textBoxLogin.Text == "" || textBoxPassword.Text == "") return;
            string InfLine="";
            foreach(var line in File.ReadAllLines("Users.txt"))
            {
                if (line.Contains(textBoxLogin.Text+" "))
                {
                    InfLine = line;
                    break;
                }
            }
            if(InfLine=="")
            {
                MessageBox.Show("Пользователя с таким именем нет", "Ошибка");
                return;
            }
            string password = GetPassword(InfLine);
            string nameFile = GetFileName(InfLine);
            if (password == textBoxPassword.Text)
            {
                Menu a = new Menu(nameFile);
                a.ShowDialog();
            }
            else { MessageBox.Show("Неправильный пароль", "Ошибка"); }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            textBoxLogin.Text = "";
            textBoxPassword.Text = "";
        }

        private void keyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show("keys is pressed");
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.CapsLock)
            {
                checkCapsLock = !checkCapsLock;
                if (checkCapsLock)
                    labelCapsLock.Text = "Клавиша CapsLock нажата";
                else labelCapsLock.Text = "Клавиша CapsLock не нажата";
            }
            InputLanguage currentLanguage = InputLanguage.CurrentInputLanguage;
            labelEngRus.Text = currentLanguage.Culture.EnglishName;
        }

        private void Enter_Load(object sender, EventArgs e)
        {
            this.KeyDown += keyDown;
            this.KeyPress += keyPress;
        }
    }
}
