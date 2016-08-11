using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Commons;

namespace Del
{
    public partial class Form_INI_Operation : Form
    {
        public Form_INI_Operation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            INIFileUtil ini=new INIFileUtil(string.Format(@"{0}\del.ini", Application.StartupPath) );
            ini.IniWriteValue("AAA","A1","1");
            ini.IniWriteValue("AAA", "A2", "2");
            ini.IniWriteValue("AAA", "A3", "3");
            Debug.WriteLine(ini.IniReadValue("AAA","A1"));
            Debug.WriteLine(ini.IniReadValue("AAA","A2"));
            Debug.WriteLine(ini.IniReadValue("AAA","A3"));
            

            ini.IniWriteValue("BBB", "B1", "11");
            ini.IniWriteValue("BBB", "B2", "22");
            ini.IniWriteValue("BBB", "B3", "33");
            Debug.WriteLine(ini.IniReadValue("BBB", "B1"));
            Debug.WriteLine(ini.IniReadValue("BBB", "B2"));
            Debug.WriteLine(ini.IniReadValue("BBB", "B3"));

            ini.ClearKey("AAA", "A3");
            
            ini.ClearSection("BBB");

            ini.ClearKey("AAA", "A1", AppDomain.CurrentDomain.BaseDirectory + @"\aaa.ini");
            ini.ClearSection("BBB", AppDomain.CurrentDomain.BaseDirectory + @"\aaa.ini");
            ini.ClearKey("AAA","A1");

            iniStatic.IniWriteValue("XXX", "A1", "21", AppDomain.CurrentDomain.BaseDirectory + @"\aaa.ini");
            iniStatic.ClearKey("CCC", "A1",string.Format(@"{0}\aaa.ini", Application.StartupPath)) ;
            iniStatic.ClearSection("BBB", @"\aaa.ini");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //读写byte测试
            INIFileUtil ini = new INIFileUtil(string.Format(@"{0}\byte.ini", Application.StartupPath));
            byte[] val = {0x31, 0x33, 0x30, 0x31, 68};
            byte [] tmp=new byte[255];
            ini.IniWriteValue("AAA", "A1", 1);
            ini.IniWriteValue("AAA", "A2", false);
            ini.IniWriteValue("AAA", "A3", true);
            ini.IniWriteValue("AAA", "A5", 1.001);
            tmp=ini.IniReadValueByte("AAA", "A0");
            ini.IniWriteValueByte("AAA", "AA4", val, 3);
            ini.IniWriteValueByte("AAA", "AA6", val, 4);
            ini.IniWriteValueByte("AAA", "AA7", val, 5);
            ini.IniWriteValueByte("AAA", "AA6", val, 9);



        }

        private void button4_Click(object sender, EventArgs e)
        {
               byte[] vals = {0x31, 0x33, 0x30, 0x31, 68};
               byte[] readvals =new byte[100];
            INIFileUtil ini = new INIFileUtil(string.Format(@"{0}\hex.ini", Application.StartupPath));
            ini.IniWriteValueHex("AAA", "A-hex", vals, 5);
            readvals = ini.IniReadHex("AAA", "A-hex", 5);
            vals[1] = 97;
            ini.IniWriteValueHex("AAA", "A-hex2", vals, 4);
            readvals = ini.IniReadHex("AAA", "A-hex2", 4);
            int i = 0;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();
            INIFileUtil ini = new INIFileUtil(string.Format(@"{0}\ReadKeyValues.ini", Application.StartupPath));
            byte[] val = {0x31, 0x33, 0x30, 0x31, 68};
            byte [] tmp=new byte[255];
            ini.IniWriteValue("AAA", "A1", 1);
            ini.IniWriteValue("AAA", "A2", false);
            ini.IniWriteValue("AAA", "A3", true);
            ini.IniWriteValue("AAA", "A5", 1.001);
            lst = ini.ReadKeyValues("AAA");
            string all = string.Empty;
            all = string.Format("[{0}]\r\n", "AAA");
            foreach ( string str in lst)
            {
                all += str + "\r\n";
            }
            MessageBox.Show(all);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();
            INIFileUtil ini = new INIFileUtil(string.Format(@"{0}\ReadSections.ini", Application.StartupPath));
            byte[] val = { 0x31, 0x33, 0x30, 0x31, 68 };
            byte[] tmp = new byte[255];
            ini.IniWriteValue("AAA", "A1", 1);
            ini.IniWriteValue("BBB", "A2", false);
            ini.IniWriteValue("CCC", "A3", true);
            ini.IniWriteValue("DDD", "A5", 1.001);

            lst = ini.ReadSections();
            string all = string.Empty;
            all = string.Format("{0}所有section如下\r\n", "ReadSections.ini");
            foreach (string str in lst)
            {
                all += str + "\r\n";
            }
            MessageBox.Show(all);
        }

        private void button6_Click(object sender, EventArgs e)
        {
           //读写测试
            INIFileUtil ini = new INIFileUtil(string.Format(@"{0}\ReadWrite.ini", Application.StartupPath));
            byte[] vals = { 0x31, 0x32, 0x33, 0x31, 68 };
            byte[] tmp = new byte[255];
            //读写string 
            ini.IniWriteValue("string", "A1","12-qq");
            Debug.WriteLine("string " + ini.IniReadValue("string", "A1"));
            ini.IniWriteValue("string", "A1", "34--56-msn");
            Debug.WriteLine("string " + ini.IniReadValue("string", "A1"));
            Debug.WriteLine(""); 

            //读写int
            ini.IniWriteValue("Int", "A1", 1);
            Debug.WriteLine("Int "+ini.IniReadValue("Int", "A1"));
            ini.IniWriteValue("Int", "A1", 2);
            Debug.WriteLine("Int "+ini.IniReadValue("Int", "A1"));

            //读写bool
            Debug.WriteLine("");
            ini.IniWriteValue("bool", "A2", false);
            Debug.WriteLine("bool "+ini.IniReadValuesBool("bool", "A2").ToString());
            ini.IniWriteValue("bool", "A3", true);
            Debug.WriteLine("bool "+ini.IniReadValuesBool("bool", "A3").ToString());

            //读写Byte【】
            Debug.WriteLine("");
            ini.IniWriteValueByte("Byte", "A1", vals);
            Debug.WriteLine("Byte " + ini.IniReadValue("Byte", "A1"));
            Byte[] bty = ini.IniReadValueByte("Byte", "A1");
            Debug.WriteLine("Byte " + System.Text.Encoding.Default.GetString(bty));

            Debug.WriteLine("");
            ini.IniWriteValueByte("Byte", "A2", vals,3);
            Debug.WriteLine("Byte " + ini.IniReadValue("Byte", "A2")); 
            Debug.WriteLine("");
            Byte[] bty2 = ini.IniReadValueByte("Byte", "A2");
            Debug.WriteLine("Byte " + System.Text.Encoding.Default.GetString(bty2));

            //读写Hex
            Debug.WriteLine("");
            //byte[] vals = { 0x31, 0x33, 0x30, 0x31, 68 };
            byte[] readvals = new byte[100];
            ini.IniWriteValueHex("Hex", "A-hex", vals);
            vals.Initialize();
            readvals = ini.IniReadHex("Hex", "A-hex");
            Debug.WriteLine("Hex" + ini.IniReadValue("Hex", "A-hex"));
            Debug.WriteLine("Hex"+System.Text.Encoding.Default.GetString(readvals)+"\r\n");
            vals[1] = 97;
            ini.IniWriteValueHex("Hex", "A-hex2", vals, 4);
            readvals = ini.IniReadHex("Hex", "A-hex2", 4);
            Debug.WriteLine("Hex" + ini.IniReadValue("Hex", "A-hex2"));
            Debug.WriteLine("Hex" + System.Text.Encoding.Default.GetString(readvals) + "\r\n");
             
        }
    }
}
