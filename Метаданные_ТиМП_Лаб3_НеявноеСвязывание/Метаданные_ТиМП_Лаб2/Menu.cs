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
using System.Reflection;
using System.Runtime.InteropServices;
using Dispatcher_lib;

namespace Метаданные_ТиМП_Лаб2
{
    public partial class Menu : Form
    {
        char[] getLetters(string line)
        {
            char[] letters = new char[line.Length];
            int index = 0;
            foreach (char c in line)
                letters[index++] = c;
            return letters;
        }
        string[] GetInformation(string line)        //get each word of stroke in massive
        {
            string[] info = new string[4];
            int index = 0;
            char[] letters = getLetters(line);  
            for(int i=0; i<letters.Length;i++)
            {
                if (letters[i] != ' ')
                    info[index] += letters[i];
                else
                {
                    if (Char.IsLetter(letters[i - 1]) && Char.IsLetter(letters[i + 1]))
                    {
                        info[index] += letters[i];
                        continue;
                    }
                    index++;
                    continue;
                }
            }
            return info;
        }

        int getSizeOfLines(string nameFile)
        {
            int size = 0;
            foreach (var line in File.ReadAllLines(nameFile))
                size++;
            return size;
        }

        string[][] initializeInfo(string nameFile, string[][] info)
        {
            int index = 0;
            foreach (var line in File.ReadAllLines(nameFile))
            {
                info[index] = GetInformation(line);
                index++;
            }
            return info;
        }
        public Menu(string nameFile)
        {
            int size = getSizeOfLines(nameFile);
            string[][] info = new string[size][];
            info = initializeInfo(nameFile, info);
            menuStrip1 = new MenuStrip();
            InitializeComponent();


            for (int i = 0; i < size; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item = new ToolStripMenuItem(info[i][1]);
                if (info[i][2] == "1" || info[i][2] =="2")
                    item.Enabled = false;
                if (info[i][2] == "2")
                    item.Visible = false;
                if (info[i][3]!="")
                {
                    if (Dispatcher_lib.Dispatcher.GetMethod(info[i][3])!=null) item.Click += new System.EventHandler(Dispatcher_lib.Dispatcher.GetMethod(info[i][3]));

                }
                if (i < size)
                {
                    if(i!=size-1)
                    {
                        while (info[i + 1][0] != "0")
                        {
                            i++;
                            ToolStripMenuItem downItem = new ToolStripMenuItem();

                            if (info[i][2] == "0" || info[i][2] == "1")
                                downItem = new ToolStripMenuItem(info[i][1]);
                            if (info[i][2] == "1")
                                downItem.Enabled = false;
                            
                            if(Dispatcher_lib.Dispatcher.GetMethod(info[i][3])!= null) downItem.Click += new System.EventHandler(Dispatcher_lib.Dispatcher.GetMethod(info[i][3]));
                          
                            //add function for menu
                            item.DropDownItems.Add(downItem);
                            
                            if (i == size - 1) break;

                        }
                    }
                }
                menuStrip1.Items.Add(item);
            }
        }

    }
}
