using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTLoginEx;
using System.Data.OleDb;
using System.Data;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace RSERP_APS21
{
   public class OLEDBHelper
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        //////////////////////////////////////////////////////////////以上是无边框移动操作,调用API
     

        public static  UTLoginEx.LoginEx iLoginEx = new LoginEx();
        //创建一个连接对象
        private static OleDbConnection con = null;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        public static OleDbConnection GetCon()
        {
            if (con == null || con.ConnectionString == "")
            {
                con = new OleDbConnection(iLoginEx.ConnString());  //获取连接字符串
            }
            return con;
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        public static void OpenCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void CloseCon()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        /// <summary>
        /// 执行动作查询：添加、修改、删除
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, CommandType type)           //SQL查询语句     //命令类型(默认值)      //命令参数列表
        {
            int n = 0;
            OleDbConnection con = GetCon();
            OpenCon();
            OleDbCommand com = new OleDbCommand(sql,con);
            type=CommandType.Text;
            com.CommandType = type;//指定命令类型
            n = com.ExecuteNonQuery();
            CloseCon();
            return n;
        }

        /// <summary>
        /// 执行一般查询：返回首行首列（一个值）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, CommandType type)
        {
            object o = null;
            OleDbConnection con = GetCon();
            OpenCon();
            OleDbCommand com = new OleDbCommand(sql, con);
            type=CommandType.Text;
            com.CommandType = type;
          //  com.Parameters.AddRange(paras);
            o = com.ExecuteScalar();
            CloseCon();
            return o;
        }

        /// 执行一般查询：返回多行多列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(string sql, CommandType type)
        {
            OleDbConnection con = GetCon();
            OpenCon();
            OleDbCommand com = new OleDbCommand(sql, con);
            type = CommandType.Text;
            com.CommandType = type;
            OleDbDataReader dr = com.ExecuteReader();
            return dr;
        }
       /// <summary>
       /// 数据表DataTable
       /// </summary>
       /// <param name="sql"></param>
       /// <param name="type"></param>
       /// <returns></returns>
        public static DataTable GetDataTalbe(string sql,CommandType type )//,params OleDbParameter [] para
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter sda = new OleDbDataAdapter(sql,GetCon());
            type =CommandType.Text;
            sda.SelectCommand.CommandType = type;
           // sda.SelectCommand.Parameters.AddRange(para);
            sda.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 对用户指定的字符串进行MD5加密
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string GetMD5(string pwd)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(pwd);
            bytes = md5.ComputeHash(bytes);
            pwd = BitConverter.ToString(bytes);
            return pwd;
        }
       
       /// <summary>
        /// 返回总记录数
       /// </summary>
       /// <returns></returns>
        public static int GetCount(string sql)
        {
            int n = Convert.ToInt32(ExecuteScalar(sql, CommandType.Text));
            return n;
        }
    }
}
