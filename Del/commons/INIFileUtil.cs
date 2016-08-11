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
    /// INI�ļ�����������
    /// </summary>
    public class INIFileUtil
    {
        public string path;

        /// <summary>
        /// ����INI�ļ�·���������
        /// </summary>
        /// <param name="INIPath">INI�ļ�·��</param>
        ///  if (!File.Exists(System.Windows.Forms.Application.StartupPath + "\tif.ini"))
        public INIFileUtil(string INIPath)
		{
			path = INIPath;
		}

        #region "��������"
        /// <summary>
        /// д��INI�ļ�
        /// </summary>
        /// <param name="section">�ڵ�����[��[TypeName]]</param>
        /// <param name="key">��</param>
        /// <param name="val">ֵ</param>
        /// <param name="filepath">�ļ�·��</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>
        /// ��ȡINI�ļ�
        /// </summary>
        /// <param name="section">�ڵ�����</param>
        /// <param name="key">��</param>
        /// <param name="def">ֵ</param>
        /// <param name="retval">stringbulider����</param>
        /// <param name="size">�ֽڴ�С</param>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        /// <summary>
        /// ��ȡINI�ļ���INT�ͣ�
        /// </summary>
        /// <param name="section">�ڵ�����</param>
        /// <param name="key">��</param>
        /// <param name="nDefault">û�ҵ�ʱ���ص�Ĭ��ֵ</param>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns>��ȡ��INTֵ</returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section, string key, int nDefault, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
 
         
        /// <summary>
        /// ��ȡ���ж�����
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="iLen"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileSectionNamesA(byte[] buffer, int iLen, string fileName);
        /// <summary>
        /// ��ȡָ��С������������ֵ��һ���б� 
        /// </summary>
        /// <param name="section">�� �Σ�����ȡ��С�ڡ�ע������ִ������ִ�Сд</param>
        /// <param name="buffer">������ ���ص���һ�������ƵĴ����ַ���֮������"\0"�ָ���</param>
        /// <param name="nSize">�������Ĵ�С</param>
        /// <param name="filePath">��ʼ���ļ������֡���û��ָ������·������windows����WindowsĿ¼�в����ļ�</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileSection(string section, byte[] buffer, int nSize, string filePath);
        #endregion


        #region 16�����ַ������ֽ�ת�� EncodeByteArray(byte[] Value) DecodeByteArray(string Value)
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
        #endregion 16�����ַ������ֽ�ת��

        /// <summary>
		/// дINI�ļ�
		/// </summary>
		/// <param name="section">����ڵ�</param>
		/// <param name="key">�ؼ���</param>
		/// <param name="value">ֵ</param>
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
		/// ��ȡINI�ļ�
		/// </summary>
		/// <param name="Section">����ڵ�</param>
		/// <param name="Key">�ؼ���</param>
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
        /// ini��������INT�ͣ�
        /// </summary>
        /// <param name="section">�ڵ�����</param>
        /// <param name="key">��</param>
        /// <param name="nDefault">û�ҵ�ʱ���ص�Ĭ��ֵ</param>
        /// <returns>��ȡ��INTֵ</returns>
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



        #region ɾ��/���ini��ȫ�����ݡ�section��key
        /// <summary>
		/// ɾ��ini�ļ������ж���
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
		/// ɾ��ini�ļ���ָ�������µ����м�
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
        /// ɾ��ini�ļ���ָ��Section�µļ�
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
        #endregion ɾ��/���ini��ȫ�����ݡ�section��key

        /// <summary>
        /// ��ȡini�ļ������еĶ���(����)���������б���
        /// </summary>
        /// <param name="filePath">ini�ļ��ľ���·��</param>
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
        /// ��ȡָ����section�µ����м�ֵ�� ���ؼ��ϵ�ÿһ��Ԫ������"key=value"
        /// </summary>
        /// <param name="section">ָ���Ķ���</param>
        /// <param name="filePath">ini�ļ��ľ���·��</param>
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
                if (buffer[i] == 0x00) //��'\0'����Ϊ�ָ�
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
        
       #region "��������"
        /// <summary>
        /// д��INI�ļ�
        /// </summary>
        /// <param name="section">�ڵ�����[��[TypeName]]</param>
        /// <param name="key">��</param>
        /// <param name="val">ֵ</param>
        /// <param name="filepath">�ļ�·��</param>
        /// <returns></returns>
       [DllImport("kernel32")] private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
       
 
        /// <summary>
        /// ��ȡINI�ļ�
        /// </summary>
        /// <param name="section">�ڵ�����</param>
        /// <param name="key">��</param>
        /// <param name="def">ֵ</param>
        /// <param name="retval">stringbulider����</param>
        /// <param name="size">�ֽڴ�С</param>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns></returns>
        [DllImport("kernel32")] private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        /// <summary>
        /// ��ȡINI�ļ���INT�ͣ�
        /// </summary>
        /// <param name="section">�ڵ�����</param>
        /// <param name="key">��</param>
        /// <param name="nDefault">û�ҵ�ʱ���ص�Ĭ��ֵ</param>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns>��ȡ��INTֵ</returns>
        [DllImport("kernel32")] private static extern int GetPrivateProfileInt(string section, string key, int nDefault, string filePath);

        [DllImport("kernel32")] private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
        #endregion

#region дini
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
 
#endregion дini

#region ��ini
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

#endregion ��ini

        #region ɾ��/���ini��ȫ�����ݡ�section��key

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
        #endregion ɾ��/���ini��ȫ�����ݡ�section��key
    }
    #endregion iniStatic
}
