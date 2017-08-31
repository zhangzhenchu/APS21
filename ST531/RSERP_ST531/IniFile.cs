using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;

using System.Runtime.InteropServices;

namespace IniFile

{
    public class Ini
    {
        // 声明INI文件的写操作函数 WritePrivateProfileString()

        //private static byte[] _key1 = UTF8Encoding.UTF8.GetBytes("#>TAZ}vK}NeKS:{/S!wHm<^x6Xq7|?l~");//32位
        //private static string keys_AEStxt = "f%Jy}+>n6Gn/{!lAjdijp+g5mAM\"]9oi!$8L8TX[8GtgAfw`][`tc`zbTGL_}c[*";//密钥,256位 
        //private static string keys_AES = "#JWNsOFgVJWOFg6@*;.OEiN]0\"wO&ap]`jq:H3|h(jUmY?Yg/+gBr4gLH0NJ'I-o3N9Aq";//密钥,256位 

        [System.Runtime.InteropServices.DllImport("kernel32")]

        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()

        [System.Runtime.InteropServices.DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


        private string sPath = null;
        public Ini(string path)
        {
            this.sPath = path;
        }

        public void Writue(string section, string key, string value)
        {

            // section=配置节，key=键名，value=键值，path=路径

            WritePrivateProfileString(section, key, value, sPath);

        }
        public string ReadValue(string section, string key)
        {

            // 每次从ini中读取多少字节

            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);

            // section=配置节，key=键名，temp=上面，path=路径

            GetPrivateProfileString(section, key, "", temp, 255, sPath);

            return temp.ToString();

        }



        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="pwd">原码</param>
        /// <returns></returns>
        public static string MakePassword(string pwd)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider oMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] returnByte = oMD5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd));

            StringBuilder result = new StringBuilder(32);
            for (int i = 0; i < returnByte.Length; i++)
            {
                result.Append(returnByte[i].ToString("X2"));
            }
            oMD5 = null;

            return result.ToString();
        }

        
        /// <summary>
        /// left截取字符串
        /// </summary>
        /// <param name="sSource">源字符串</param>
        /// <param name="iLength">截取长度</param>
        /// <returns></returns>
        public static string Left(string sSource, int iLength)
        {
            return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
        }
        

        
        /// <summary>
        /// Right截取字符串
        /// </summary>
        /// <param name="sSource">源字符串</param>
        /// <param name="iLength">截取长度</param>
        /// <returns></returns>
        public static string Right(string sSource, int iLength)
        {
            return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);
        }
        

      // 
        
        /// <summary>
        /// mid截取字符串
        /// </summary>
        /// <param name="sSource">源字符串</param>
        /// <param name="iStart">起始位置</param>
        /// <param name="iLength">截取长度</param>
        /// <returns></returns>
        public static string Mid(string sSource, int iStart, int iLength)
        {
            int iStartPoint = iStart > sSource.Length ? sSource.Length : iStart;
            return sSource.Substring(iStartPoint, iStartPoint + iLength > sSource.Length ? sSource.Length - iStartPoint : iLength);
        }


        //static void Main(string[] args)
        //{
        //    foreach (Process p in Process.GetProcesses())
        //       {
        //            Console.Write(p.ProcessName);
        //            Console.Write("----");
        //            Console.WriteLine(GetProcessiLoginEx.UserName()(p.Id));
        //       }

        //       Console.ReadKey();
        //}


      

    }
    public class Environment
    {
        [DllImport("shell32.dll")]
        static extern int SHGetFolderPath(IntPtr hwndOwner, SpecialFolderCSIDL nFolder, IntPtr hToken,
           uint dwFlags, [Out] StringBuilder pszPath);

        private const int MAX_PATH = 260;

        public enum SpecialFolderCSIDL : int
        {
            CSIDL_DESKTOP = 0x0000,    // <desktop>  
            CSIDL_INTERNET = 0x0001,    // Internet Explorer (icon on desktop)  
            CSIDL_PROGRAMS = 0x0002,    // Start Menu\Programs  
            CSIDL_CONTROLS = 0x0003,    // My Computer\Control Panel  
            CSIDL_PRINTERS = 0x0004,    // My Computer\Printers  
            CSIDL_PERSONAL = 0x0005,    // My Documents  
            CSIDL_FAVORITES = 0x0006,    // <user name>\Favorites  
            CSIDL_STARTUP = 0x0007,    // Start Menu\Programs\Startup  
            CSIDL_RECENT = 0x0008,    // <user name>\Recent  
            CSIDL_SENDTO = 0x0009,    // <user name>\SendTo  
            CSIDL_BITBUCKET = 0x000a,    // <desktop>\Recycle Bin  
            CSIDL_STARTMENU = 0x000b,    // <user name>\Start Menu  
            CSIDL_MYMUSIC = 0x000d, //  
            CSIDL_DESKTOPDIRECTORY = 0x0010,    // <user name>\Desktop  
            CSIDL_DRIVES = 0x0011,    // My Computer  
            CSIDL_NETWORK = 0x0012,    // Network Neighborhood  
            CSIDL_NETHOOD = 0x0013,    // <user name>\nethood  
            CSIDL_FONTS = 0x0014,    // windows\fonts  
            CSIDL_TEMPLATES = 0x0015,
            CSIDL_COMMON_STARTMENU = 0x0016,    // All Users\Start Menu  
            CSIDL_COMMON_PROGRAMS = 0x0017,    // All Users\Programs  
            CSIDL_COMMON_STARTUP = 0x0018,    // All Users\Startup  
            CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,    // All Users\Desktop  
            CSIDL_APPDATA = 0x001a,    // <user name>\Application Data  
            CSIDL_PRINTHOOD = 0x001b,    // <user name>\PrintHood  
            CSIDL_LOCAL_APPDATA = 0x001c,    // <user name>\Local Settings\Applicaiton Data (non roaming)  
            CSIDL_ALTSTARTUP = 0x001d,    // non localized startup  
            CSIDL_COMMON_ALTSTARTUP = 0x001e,    // non localized common startup  
            CSIDL_COMMON_FAVORITES = 0x001f,
            CSIDL_INTERNET_CACHE = 0x0020,
            CSIDL_COOKIES = 0x0021,
            CSIDL_HISTORY = 0x0022,
            CSIDL_COMMON_APPDATA = 0x0023,    // All Users\Application Data  
            CSIDL_WINDOWS = 0x0024,    // GetWindowsDirectory()  
            CSIDL_SYSTEM = 0x0025,    // GetSystemDirectory()  
            CSIDL_PROGRAM_FILES = 0x0026,    // C:\Program Files  
            CSIDL_MYPICTURES = 0x0027,    // C:\Program Files\My Pictures  
            CSIDL_PROFILE = 0x0028,    // USERPROFILE  
            CSIDL_SYSTEMX86 = 0x0029,    // x86 system directory on RISC  
            CSIDL_PROGRAM_FILESX86 = 0x002a,    // x86 C:\Program Files on RISC  
            CSIDL_PROGRAM_FILES_COMMON = 0x002b,    // C:\Program Files\Common  
            CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c,    // x86 Program Files\Common on RISC  
            CSIDL_COMMON_TEMPLATES = 0x002d,    // All Users\Templates  
            CSIDL_COMMON_DOCUMENTS = 0x002e,    // All Users\Documents  
            CSIDL_COMMON_ADMINTOOLS = 0x002f,    // All Users\Start Menu\Programs\Administrative Tools  
            CSIDL_ADMINTOOLS = 0x0030,    // <user name>\Start Menu\Programs\Administrative Tools  
            CSIDL_CONNECTIONS = 0x0031,    // Network and Dial-up Connections  
        };

        public static string GetAllUsersFolderPath(SpecialFolderCSIDL csidl)
        {
            StringBuilder path = new StringBuilder(MAX_PATH);
            SHGetFolderPath(IntPtr.Zero, csidl, IntPtr.Zero, 0, path);
            return path.ToString();
        }

        public static string GetAllUsersFolderPath(System.Environment.SpecialFolder specialFolder)
        {
            SpecialFolderCSIDL csidl;

            switch (specialFolder)
            {
                case System.Environment.SpecialFolder.ApplicationData:
                    csidl = SpecialFolderCSIDL.CSIDL_APPDATA;
                    break;
                case System.Environment.SpecialFolder.CommonApplicationData:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_APPDATA;
                    break;
                case System.Environment.SpecialFolder.CommonProgramFiles:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_PROGRAMS;
                    break;
                case System.Environment.SpecialFolder.Cookies:
                    csidl = SpecialFolderCSIDL.CSIDL_COOKIES;
                    break;
                case System.Environment.SpecialFolder.Desktop:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_DESKTOPDIRECTORY;
                    break;
                case System.Environment.SpecialFolder.DesktopDirectory:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_DESKTOPDIRECTORY;
                    break;
                case System.Environment.SpecialFolder.Favorites:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_FAVORITES;
                    break;
                case System.Environment.SpecialFolder.History:
                    csidl = SpecialFolderCSIDL.CSIDL_HISTORY;
                    break;
                case System.Environment.SpecialFolder.InternetCache:
                    csidl = SpecialFolderCSIDL.CSIDL_INTERNET_CACHE;
                    break;
                case System.Environment.SpecialFolder.LocalApplicationData:
                    csidl = SpecialFolderCSIDL.CSIDL_LOCAL_APPDATA;
                    break;
                case System.Environment.SpecialFolder.MyComputer:
                    csidl = SpecialFolderCSIDL.CSIDL_DRIVES;
                    break;
                case System.Environment.SpecialFolder.MyDocuments:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_DOCUMENTS;
                    break;
                case System.Environment.SpecialFolder.MyMusic:
                    csidl = SpecialFolderCSIDL.CSIDL_MYMUSIC;
                    break;
                case System.Environment.SpecialFolder.MyPictures:
                    csidl = SpecialFolderCSIDL.CSIDL_MYPICTURES;
                    break;
                case System.Environment.SpecialFolder.ProgramFiles:
                    csidl = SpecialFolderCSIDL.CSIDL_PROGRAM_FILES;
                    break;
                case System.Environment.SpecialFolder.Programs:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_PROGRAMS;
                    break;
                case System.Environment.SpecialFolder.Recent:
                    csidl = SpecialFolderCSIDL.CSIDL_RECENT;
                    break;
                case System.Environment.SpecialFolder.SendTo:
                    csidl = SpecialFolderCSIDL.CSIDL_SENDTO;
                    break;
                case System.Environment.SpecialFolder.StartMenu:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_STARTMENU;
                    break;
                case System.Environment.SpecialFolder.Startup:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_STARTUP;
                    break;
                case System.Environment.SpecialFolder.System:
                    csidl = SpecialFolderCSIDL.CSIDL_SYSTEM;
                    break;
                case System.Environment.SpecialFolder.Templates:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_TEMPLATES;
                    break;
                default:
                    csidl = SpecialFolderCSIDL.CSIDL_COMMON_DOCUMENTS;
                    break;
            }

            return GetAllUsersFolderPath(csidl);
        }
    }  
}

