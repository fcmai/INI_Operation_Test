
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
        public UnitTest_IniFileUtil(/*   */  ) 
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
        public void TestMethod_iniDataWriteandRead()
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
         
            byte [] btyW= {0x30,0x31, 0x32,0x33,0x44,0x45};
            ini.IniWriteValueByte("Byte", "A1", btyW);          ;
            Byte[] btyR = ini.IniReadValueByte("Byte", "A1");
            Assert.AreEqual(btyW.Length,btyR.Length);
            for (int k = 0; k < btyR.Length; k++)
            {
                Assert.AreEqual(btyR[k], btyW[k]);
            }
            for (int k = 0; k < btyW.Length; k++)
            {
                Assert.AreEqual(btyR[k], btyW[k]);
            }

            btyW[0] = 0x30; btyW[1] = 0x31; btyW[2] = 0x32; btyW[3] = 0x33;
            ini.IniWriteValueByte("Byte", "A2", btyW, 3);
            Assert.AreEqual("012", ini.IniReadValue("Byte", "A2"));
            ini.IniWriteValueByte("Byte", "A2", btyW, 3);
            Assert.AreNotEqual("013", ini.IniReadValue("Byte", "A2"));

            btyW[0] = 0x30; btyW[1] = 0x31; btyW[2] = 0x32; btyW[3] = 0x33;
            ini.IniWriteValueByte("Byte", "A2", btyW, 4);
            Byte[] bty2 = ini.IniReadValueByte("Byte", "A2");
            Assert.AreEqual("0123", System.Text.Encoding.Default.GetString(bty2));

            //读写Hex//
            Debug.WriteLine("");
            byte[] valsHex = { 0x31, 0x33, 0x30, 0x31, 68 };
            byte[] readvals = new byte[5];
            ini.IniWriteValueHex("Hex", "A-hex", valsHex);
            readvals = ini.IniReadHex("Hex", "A-hex");
           // Assert.AreEqual(readvals, valsHex);
            for (int k = 0; k < valsHex.Length; k++)
            {
                Assert.AreEqual(valsHex[k], readvals[k]);
            }

            int numtoRead = 4;
            valsHex[0] = 96; valsHex[1] = 97; valsHex[2] = 98; valsHex[1] = 99;
            ini.IniWriteValueHex("Hex", "A-hex2", valsHex, numtoRead);
            readvals = ini.IniReadHex("Hex", "A-hex2", numtoRead);
            if (numtoRead<=valsHex.Length)
            {
                for (int j = 0; j < numtoRead; j++)
                {
                    Assert.AreEqual(valsHex[j], readvals[j]);
                
                 }
            }
            else
            {
                for (int k = 0; k < readvals.Length; k++)
                {
                    Assert.AreEqual(valsHex[k], readvals[k]);
                }
                 
            }
            

        }
    }
}
