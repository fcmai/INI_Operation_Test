﻿
using System;
using System.Diagnostics;
using System.IO;
using Commons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
 
namespace TestProject
{
    /// <summary>
    /// UnitTest_IniFileUtil 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTest_IniFileUtil
    {
        public UnitTest_IniFileUtil()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
            string path = string.Format(@"{0}\ReadWrite.ini", AppDomain.CurrentDomain.BaseDirectory);
            File.Delete(path);
            Assert.IsFalse(File.Exists(path));
            INIFileUtil ini = new INIFileUtil(path);
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: 在此处添加测试逻辑
            //
            //读写测试
            string path = string.Format(@"{0}\ReadWrite.ini",  AppDomain.CurrentDomain.BaseDirectory);
            File.Delete(path);
            Assert.IsFalse(File.Exists(path));
            INIFileUtil ini = new INIFileUtil(path);
            byte[] vals = { 0x31, 0x32, 0x33, 0x31, 68 };
            byte[] tmp = new byte[255];
            //读写string 
            
            string item1 = "12-qq";
            string item2 = "34--56-msn";
            ini.IniWriteValue("string", "A1",item1 );
            string item11 = ini.IniReadValue("string", "A1");
            Assert.AreEqual(item1, item11);

            ini.IniWriteValue("string", "A1", item2);
            string item22 = ini.IniReadValue("string", "A1");
            Assert.AreEqual(item2, item22);


         
            //读写int
            int i = 1;
            ini.IniWriteValue("Int", "A1", i);
            Assert.AreEqual(i, ini.IniReadInt("Int", "A1",i-1));
            i = 2;
            ini.IniWriteValue("Int", "A1", i);
            Assert.AreEqual(i, ini.IniReadInt("Int", "A1", i +1));

            //读写bool
            bool b = false;
            ini.IniWriteValue("bool", "A1", b);
            Assert.AreEqual(b,ini.IniReadValuesBool("bool", "A1"));
            b = true;
            ini.IniWriteValue("bool", "A1", true);
            Assert.AreEqual(b, ini.IniReadValuesBool("bool", "A1"));

            //读写Byte【】
            /*
            byte [] bty= {0x30,0x31, 0x32,0x33,0x44,0x45};
            ini.IniWriteValueByte("Byte", "A1", bty);
            Debug.WriteLine("Byte " + ini.IniReadValue("Byte", "A1"));
            Byte[] btyR = ini.IniReadValueByte("Byte", "A1");
            Debug.WriteLine("Byte " + System.Text.Encoding.Default.GetString(bty));

            Debug.WriteLine("");
            ini.IniWriteValueByte("Byte", "A2", vals, 3);
            Debug.WriteLine("Byte " + ini.IniReadValue("Byte", "A2"));
            Debug.WriteLine("");
            Byte[] bty2 = ini.IniReadValueByte("Byte", "A2");
            Debug.WriteLine("Byte " + System.Text.Encoding.Default.GetString(bty2));
*/
            //读写Hex
            Debug.WriteLine("");
            byte[] valsHex = { 0x31, 0x33, 0x30, 0x31, 68 };
            byte[] readvals = new byte[sizeof(valsHex)];
            ini.IniWriteValueHex("Hex", "A-hex", vals);
            vals.Initialize();
            readvals = ini.IniReadHex("Hex", "A-hex");
            Debug.WriteLine("Hex" + ini.IniReadValue("Hex", "A-hex"));
            Debug.WriteLine("Hex" + System.Text.Encoding.Default.GetString(readvals) + "\r\n");
            vals[1] = 97;
            ini.IniWriteValueHex("Hex", "A-hex2", vals, 4);
            readvals = ini.IniReadHex("Hex", "A-hex2", 4);
            Debug.WriteLine("Hex" + ini.IniReadValue("Hex", "A-hex2"));
            Debug.WriteLine("Hex" + System.Text.Encoding.Default.GetString(readvals) + "\r\n");

        }
    }
}
