using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Commons
{

    /// <summary>
    /// INI文件操作辅助类
    /// </summary>
    public class INIFileUtil
    {
        public string path;

        /// <summary>
        /// 传入INI文件路径构造对象
        /// </summary>
        /// <param name="INIPath">INI文件路径</param>
        ///  if (!File.Exists(System.Windows.Forms.Application.StartupPath + "\tif.ini"))
        public INIFileUtil(string INIPath)
		{
			path = INIPath;
		}

        #region "声明变量"
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        /// <summary>
        /// 读取INI文件（INT型）
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="nDefault">没找到时返回的默认值</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>读取的INT值</returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section, string key, int nDefault, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
 
         
        /// <summary>
        /// 获取所有段落名
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="iLen"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileSectionNamesA(byte[] buffer, int iLen, string fileName);
        /// <summary>
        /// 获取指定小节所有项名和值的一个列表 
        /// </summary>
        /// <param name="section">节 段，欲获取的小节。注意这个字串不区分大小写</param>
        /// <param name="buffer">缓冲区 返回的是一个二进制的串，字符串之间是用"\0"分隔的</param>
        /// <param name="nSize">缓冲区的大小</param>
        /// <param name="filePath">初始化文件的名字。如没有指定完整路径名，windows就在Windows目录中查找文件</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileSection(string section, byte[] buffer, int nSize, string filePath);
        #endregion


        #region 16进制字符串和字节转换 EncodeByteArray(byte[] Value) DecodeByteArray(string Value)
        // *** Encode byte array ***
        private string EncodeByteArray(byte[] Value)
        {
            if (Value == null) return null;

            StringBuilder sb = new StringBuilder();
            foreach (byte b in Value)
            {
                string hex = Convert.ToString(b, 16);
                int l = hex.Length;
                if (l > 2)
                {
                    sb.Append(hex.Substring(l - 2, 2));
                }
                else
                {
                    if (l < 2) sb.Append("0");
                    sb.Append(hex);
                }
            }
            return sb.ToString();
        }

        // *** Decode byte array ***
        private byte[] DecodeByteArray(string Value)
        {
            if (Value == null) return null;

            int l = Value.Length;
            if (l < 2) return new byte[] { };

            l /= 2;
            byte[] Result = new byte[l];
            for (int i = 0; i < l; i++) Result[i] = Convert.ToByte(Value.Substring(i * 2, 2), 16);
            return Result;
        }
        #endregion 16进制字符串和字节转换

        /// <summary>
		/// 写INI文件
		/// </summary>
		/// <param name="section">分组节点</param>
		/// <param name="key">关键字</param>
		/// <param name="value">值</param>
		public long  IniWriteValue(string section,string key,string value)
		{
            return WritePrivateProfileString(section, key, value, this.path);
		}
        public long IniWriteValue(string section, string key, string value, string filePath)
        {
            return WritePrivateProfileString(section, key, value, filePath);
        }

        public long IniWriteValue(string section, string key, int value)
        {
            return WritePrivateProfileString(section, key, Convert.ToString(value),this.path);
        }

        public long IniWriteValue(string section, string key, double value)
        {
            return WritePrivateProfileString(section, key, value.ToString("f3"), this.path);//Convert.ToString(value,"f3")
        }

        public long IniWriteValue(string section, string key, bool value)
        {
            return WritePrivateProfileString(section, key, Convert.ToString(value), this.path);
        }

     
        public long IniWriteValueByte(string section, string key, Byte[] vals,int size )
        {
            string tmp = System.Text.Encoding.Default.GetString(vals);
            tmp = tmp.Substring(0, tmp.Length > size ? size : tmp.Length);
            return  WritePrivateProfileString(section, key, tmp , this.path);
        }
        
        public long IniWriteValueByte(string section, string key, Byte[] vals)
        {
            return WritePrivateProfileString(section, key, System.Text.Encoding.Default.GetString(vals), this.path);
        }

        public long IniWriteValueHex(string section, string key, Byte[] vals)
        {
            string tmp = EncodeByteArray(vals);       
            return WritePrivateProfileString(section, key, tmp, this.path);
        }

        public long IniWriteValueHex(string section, string key, Byte[] vals, int size)
        {
            string tmp = EncodeByteArray(vals);
            tmp = tmp.Substring(0, tmp.Length >2* size ?2* size : tmp.Length);
            return WritePrivateProfileString(section, key,tmp , this.path);
        }
		/// <summary>
		/// 读取INI文件
		/// </summary>
		/// <param name="Section">分组节点</param>
		/// <param name="Key">关键字</param>
		/// <returns></returns>
		public string IniReadValue(string section,string key)
		{
			StringBuilder temp = new StringBuilder(255);
			int i = GetPrivateProfileString(section,key,"",temp, 255, this.path);
			return temp.ToString();
		}

        //byte
		public byte[] IniReadValueByte(string section, string key)
		{
			byte[] temp = new byte[255];
			int i = GetPrivateProfileString(section, key, "", temp, 255, this.path);
			return temp;
		}
        //bool
        public bool IniReadValuesBool(string section, string key)
        {
            byte[] temp = new byte[255];
            int i = GetPrivateProfileString(section, key,  "", temp, 255, this.path);
            bool rtn = false;
        
            if (Boolean.TryParse(System.Text.Encoding.Default.GetString(temp),out rtn))
            {
              
            }
            else
            {
                Trace.WriteLine("Boolean.TryParse failure " + System.Text.Encoding.Default.GetString(temp));
                throw (new Exception("Boolean.TryParse failure " + System.Text.Encoding.Default.GetString(temp)));               
                //return false;
            }
            return rtn;
        }

        //bool
        public double IniReadValuesDouble(string section, string key)
        {
            byte[] temp = new byte[255]; //StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.path);
            double rtn = 0;
            try
            {
                rtn = Convert.ToDouble(System.Text.Encoding.Default.GetString(temp));
            }
            catch (Exception e)
            {
                Trace.WriteLine("Boolean.TryParse failure " + System.Text.Encoding.Default.GetString(temp));
                throw (new Exception("ToDouble failure " + System.Text.Encoding.Default.GetString(temp)));
                //return 0;
                
            }
            finally
            {
                
            }
            return rtn;
        }
        /// <summary>
        /// ini读操作（INT型）
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="nDefault">没找到时返回的默认值</param>
        /// <returns>读取的INT值</returns>
        public int IniReadInt(string section, string key, int nDefault)
        {
            return GetPrivateProfileInt(section, key, nDefault, this.path);
        }
        public int IniReadInt(string section, string key, int nDefault, string filePath)
        {
            return GetPrivateProfileInt(section, key, nDefault, filePath);
        }


        public byte[] IniReadHex(string section, string key )
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.path);
            return DecodeByteArray(temp.ToString());
            //return DecodeByteArray(GetPrivateProfileString(section, key, this.path));
        }
        public byte [] IniReadHex(string section, string key, int size)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.path);

//             string tmp = System.Text.Encoding.Default.GetString(vals);
//             tmp = tmp.Substring(0, tmp.Length > 2*size ? 2*size : tmp.Length);
//             Debug.WriteLine(tmp);
            return DecodeByteArray(temp.ToString().Substring(0,2*size));
            //return DecodeByteArray(GetPrivateProfileString(section, key, this.path));
        }



        #region 删除/清空ini中全部内容、section、key
        /// <summary>
		/// 删除ini文件下所有段落
		/// </summary>
		public void ClearAllSection()
		{
			IniWriteValue(null,null,null);
		}

        public void ClearAllSection(string filePath)
        {
            IniWriteValue(null, null, null,  filePath);
        }

		/// <summary>
		/// 删除ini文件下指定段落下的所有键
		/// </summary>
		/// <param name="section"></param>
		public void ClearSection(string section)
		{
			IniWriteValue(section,null,null);
		}
        public void ClearSection(string section, string filePath)
        {
            IniWriteValue(section, null, null, filePath);
        }
        /// <summary>
        /// 删除ini文件下指定Section下的键
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        public void ClearKey(string section,string key)
        {
            IniWriteValue(section, key, null);
        }
        public void ClearKey(string section, string key, string filePath)
        {
            IniWriteValue(section, key, null,filePath);
        }
        #endregion 删除/清空ini中全部内容、section、key

        /// <summary>
        /// 获取ini文件中所有的段名(节名)，保存在列表中
        /// </summary>
        /// <param name="filePath">ini文件的绝对路径</param>
        /// <returns></returns>
        public  List<string> ReadSections(/*string filePath*/)
        {
            byte[] buffer = new byte[65535];
            int rel = GetPrivateProfileSectionNamesA(buffer, buffer.GetUpperBound(0), this.path);
            int iCnt, iPos;
            List<string> arrayList = new List<string>();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                        {
                            arrayList.Add(tmp);
                        }
                    }
                }
            }
            return arrayList;
        }

        /// <summary>
        /// 获取指定段section下的所有键值对 返回集合的每一个元素形如"key=value"
        /// </summary>
        /// <param name="section">指定的段落</param>
        /// <param name="filePath">ini文件的绝对路径</param>
        /// <returns></returns>
        public  List<string> ReadKeyValues(string section/*, string filePath*/)
        {
            byte[] buffer = new byte[32767];
            List<string> list = new List<string>();
            int length = GetPrivateProfileSection(section, buffer, buffer.GetUpperBound(0), this.path);
            string temp;
            int postion = 0;
            for (int i = 0; i < length; i++)
            {
                if (buffer[i] == 0x00) //以'\0'来作为分隔
                {
                    temp = System.Text.ASCIIEncoding.Default.GetString(buffer, postion, i - postion).Trim();
                    postion = i + 1;
                    if (temp.Length > 0)
                    {
                        list.Add(temp);
                    }
                }
            }
            return list;
        }
    }

    #region iniStatic

    public static class iniStatic
    {
        
       #region "声明变量"
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
       [DllImport("kernel32")] private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
       
 
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")] private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        /// <summary>
        /// 读取INI文件（INT型）
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="nDefault">没找到时返回的默认值</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>读取的INT值</returns>
        [DllImport("kernel32")] private static extern int GetPrivateProfileInt(string section, string key, int nDefault, string filePath);

        [DllImport("kernel32")] private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
        #endregion

#region 写ini
        public static long IniWriteValue(string section, string key, string value, string filePath)
        {
           return WritePrivateProfileString(section, key, value, filePath);
        }

        public static long IniWriteValue(string section, string key, int value, /*params*/ string filePath)
        {
            return WritePrivateProfileString(section, key, Convert.ToString(value), filePath);
        }

        public static long IniWriteValue(string section, string key, double value, string filePath)
        {
            return WritePrivateProfileString(section, key, value.ToString("f3"), filePath);//Convert.ToString(value,"f3")
        }

        public static long IniWriteValue(string section, string key, bool value, string filePath)
        {
            return WritePrivateProfileString(section, key, Convert.ToString(value), filePath);
        }



        public static long IniWriteValue(string section, string key, Byte[] vals, int size, string filePath)
        {
            return WritePrivateProfileString(section, key,  System.Text.Encoding.Default.GetString (vals), filePath);
        }
 
#endregion 写ini

#region 读ini
        public static string IniReadValue(string section, string key, string filePath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255,   filePath);
            return temp.ToString();
        }

        public static byte[] IniReadValues(string section, string key, string filePath)
        {
            byte[] temp = new byte[255];
            int i = GetPrivateProfileString(section, key, "", temp, 255, filePath);
            return temp;

        }
        

        public static int IniReadInt(string section, string key, int nDefault, string filePath)
        {
            return GetPrivateProfileInt(section, key, nDefault, filePath);
        }

#endregion 读ini

        #region 删除/清空ini中全部内容、section、key

        public static void ClearAllSection(string filePath)
        {
            IniWriteValue(null, null, null, filePath);
        }


        public static void ClearSection(string section, string filePath)
        {
            IniWriteValue(section, null, null, filePath);
        }

        public static void ClearKey(string section, string key, string filePath)
        {
            IniWriteValue(section, key, null, filePath);
        }
        #endregion 删除/清空ini中全部内容、section、key
    }
    #endregion iniStatic
}
