using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.SessionState;

namespace PFMS_S.BM
{
    /// <summary>
    /// BM_EditMenuPowerForUserSubmit 的摘要说明
    /// </summary>
    public class BM_EditMenuPowerForUserSubmit : IHttpHandler, IRequiresSessionState
    {
        StringBuilder sb = new StringBuilder();
        SqlConnection mycon = null;
        SqlCommand mycom = null;
        SqlDataReader dr = null;
        int flag = 1;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string msgStr = "";
            string user_idStr = context.Request.Form["userID"].ToString();
            string menu_idStr = context.Request.Form["menuID"].ToString();
            string type = context.Request.Form["type"].ToString();
            string power_noStr = context.Request.Form["power"].ToString();
            string power_descStr = "";
            if (power_noStr == "0")
                power_descStr = "不可显";
            else if(power_noStr == "1")
                power_descStr = "只读权限";
            else if (power_noStr == "2")
                power_descStr = "读写权限";
            int group = Convert.ToInt32(context.Request.Form["group"].ToString());
            string update_byStr = context.Session["user_id"].ToString();
            string update_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                int ret = 0;
                if (type == "menu")
                {
                    List<string> useridList = new List<string>();
                    if (group == 1)
                    {
                        mycom.CommandText = "select user_id from User_Manage where dept=(select dept from User_Manage where user_id='" + user_idStr + "')";
                        dr = mycom.ExecuteReader();
                        while (dr.Read())
                        {
                            useridList.Add(dr[0].ToString());
                        }
                        dr.Close();
                        dr = null;
                    }
                    else
                        useridList.Add(user_idStr);
                    int totalCount = 0, opCount = 0;
                    foreach (string userID in useridList)
                    {
                        totalCount++;
                        sb.Clear();
                        sb.Append("if not exists(select auth_id from Auth_Manage where  user_id='" + userID + "' and menu_id='" + menu_idStr + "')\n");
                        sb.Append("begin\n");
                        sb.Append("insert into Auth_Manage(user_id,menu_id,power_no,power_desc,create_by,create_time,update_by,update_time)\n");
                        sb.Append("values('" + userID + "','" + menu_idStr + "','" + power_noStr + "',N'" + power_descStr + "','" + update_byStr + "','" + update_timeStr + "','" + update_byStr + "','" + update_timeStr + "')\n");
                        sb.Append("end\n");
                        sb.Append("else\n");
                        sb.Append("begin\n");
                        sb.Append("update Auth_Manage set power_no='" + power_noStr + "',power_desc=N'" + power_descStr + "',update_by='" + update_byStr + "',update_time='" + update_timeStr + "' where user_id='" + userID + "' and menu_id='" + menu_idStr + "'\n");
                        sb.Append("end");
                        mycom.CommandText = sb.ToString();
                        ret = mycom.ExecuteNonQuery();
                        if (ret > 0)
                            opCount++;
                    }
                    if (totalCount != opCount)
                        msgStr = "操作影响用户" + opCount.ToString() + "个,应该影响用户" + totalCount.ToString() + "个";
                    else
                        msgStr = "OK";
                }
                else if (type == "catalogue")
                {
                    if (power_noStr == "0")
                    {
                        SetPowerZero(user_idStr, menu_idStr, power_descStr, group);
                        if (flag == 1)
                            msgStr = "OK";
                        else
                            msgStr = "该目录下可能存在部分子目录或菜单权限设置失败或某些用户权限设置失败!";
                    }
                    else if (power_noStr == "1")
                    {
                        SetPowerOne(user_idStr, menu_idStr, power_descStr, group, 1);
                        if (flag == 1)
                            msgStr = "OK";
                        else
                            msgStr = "该目录下可能存在部分子目录或菜单权限设置失败或某些用户权限设置失败!";
                    }
                }
                else
                {
                    msgStr = "要操作的对象既不是目录也不是菜单!";
                }
            }
            catch (Exception msg)
            {
                msgStr = msg.Message;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
                if (mycon.State != ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            context.Response.Write(msgStr);
        }
        //
        public void SetPowerZero(string userid, string menuid, string powerDes, int group)
        {
            List<string> idList = new List<string>();
            //首先设置用户对当前目录以及当前目录下子目录和菜单的访问权限
            mycom.CommandText = "select menu_id from Menu_Manage where parent_id='" + menuid + "' and menu_state='1'";
            idList.Clear();
            idList.Add(menuid);
            dr = mycom.ExecuteReader();
            while (dr.Read())
            {
                idList.Add(dr[0].ToString());
            }
            dr.Close();
            dr = null;
            //获取用户
            List<string> useridList = new List<string>();
            if (group == 1)
            {
                mycom.CommandText = "select user_id from User_Manage where dept=(select dept from User_Manage where user_id='" + userid + "')";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    useridList.Add(dr[0].ToString());
                }
                dr.Close();
                dr = null;
            }
            else
                useridList.Add(userid);
            int ret = 0;
            foreach (string userID in useridList)
            {
                for (int i = 0; i < idList.Count; i++)
                {
                    sb.Clear();
                    sb.Append("if not exists(select auth_id from Auth_Manage where  user_id='" + userID + "' and menu_id='" + idList[i].ToString() + "')\n");
                    sb.Append("begin\n");
                    sb.Append("insert into Auth_Manage(user_id,menu_id,power_no,power_desc,create_by,create_time,update_by,update_time)\n");
                    sb.Append("values('" + userID + "','" + idList[i].ToString() + "','0',N'" + powerDes + "','" + HttpContext.Current.Session["user_id"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + HttpContext.Current.Session["user_id"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')\n");
                    sb.Append("end\n");
                    sb.Append("else\n");
                    sb.Append("begin\n");
                    sb.Append("update Auth_Manage set power_no='0',power_desc=N'" + powerDes + "',update_by='" + HttpContext.Current.Session["user_id"].ToString() + "',update_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_id='" + userID + "' and menu_id='" + idList[i].ToString() + "'\n");
                    sb.Append("end");
                    mycom.CommandText = sb.ToString();
                    ret = mycom.ExecuteNonQuery();
                    if (ret < 1)
                        flag = 0;
                }
            }
            //查询当前目录下所有子目录
            mycom.CommandText = "select menu_id from Menu_Manage where parent_id='" + menuid + "' and menu_type='catalogue' and menu_state='1'";
            idList.Clear();
            dr = mycom.ExecuteReader();
            while (dr.Read())
            {
                idList.Add(dr[0].ToString());
            }
            dr.Close();
            dr = null;
            //子目录递归执行当前函数
            for (int i = 0; i < idList.Count; i++)
            {
                SetPowerZero(userid, idList[i].ToString(), powerDes, group);
            }
        }

        public void SetPowerOne(string userid, string menuid, string powerDes, int group, int firstCall)
        {
            //获取用户
            List<string> useridList = new List<string>();
            if (group == 1)
            {
                mycom.CommandText = "select user_id from User_Manage where dept=(select dept from User_Manage where user_id='" + userid + "')";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    useridList.Add(dr[0].ToString());
                }
                dr.Close();
                dr = null;
            }
            else
                useridList.Add(userid);
            //首先设置用户对当前目录的访问权限
            int ret;
            if (firstCall == 1)
            {
                foreach (string userID in useridList)
                {
                    sb.Clear();
                    sb.Append("if not exists(select auth_id from Auth_Manage where  user_id='" + userID + "' and menu_id='" + menuid + "')\n");
                    sb.Append("begin\n");
                    sb.Append("insert into Auth_Manage(user_id,menu_id,power_no,power_desc,create_by,create_time,update_by,update_time)\n");
                    sb.Append("values('" + userID + "','" + menuid + "','1',N'" + powerDes + "','" + HttpContext.Current.Session["user_id"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + HttpContext.Current.Session["user_id"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')\n");
                    sb.Append("end\n");
                    sb.Append("else\n");
                    sb.Append("begin\n");
                    sb.Append("update Auth_Manage set power_no='1',power_desc=N'" + powerDes + "',update_by='" + HttpContext.Current.Session["user_id"].ToString() + "',update_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_id='" + userID + "' and menu_id='" + menuid + "'\n");
                    sb.Append("end");
                    mycom.CommandText = sb.ToString();
                    ret = mycom.ExecuteNonQuery();
                    if (ret < 1)
                        flag = 0;
                }
            }
            //查询当前目录下所有菜单
            List<string> idList = new List<string>();
            idList.Clear();
            mycom.CommandText = "select menu_id from Menu_Manage where parent_id='" + menuid + "' and menu_type='menu'  and menu_state='1'";
            dr = mycom.ExecuteReader();
            while (dr.Read())
            {
                idList.Add(dr[0].ToString());
            }
            dr.Close();
            dr = null;
            //初始化用对当前目录下所有菜单的访问权限
            foreach (string userID in useridList)
            {
                for (int i = 0; i < idList.Count; i++)
                {
                    sb.Clear();
                    sb.Append("if not exists(select auth_id from Auth_Manage where  user_id='" + userID + "' and menu_id='" + idList[i].ToString() + "')\n");
                    sb.Append("begin\n");
                    sb.Append("insert into Auth_Manage(user_id,menu_id,power_no,power_desc,create_by,create_time,update_by,update_time)\n");
                    sb.Append("values('" + userID + "','" + idList[i].ToString() + "',(select init_power from Menu_Manage where menu_id='" + idList[i].ToString() + "'),'','" + HttpContext.Current.Session["user_id"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + HttpContext.Current.Session["user_id"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')\n");
                    sb.Append("end\n");
                    sb.Append("else\n");
                    sb.Append("begin\n");
                    sb.Append("update Auth_Manage set power_no=(select init_power from Menu_Manage where menu_id='" + idList[i].ToString() + "'),power_desc='',update_by='" + HttpContext.Current.Session["user_id"].ToString() + "',update_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_id='" + userID + "' and menu_id='" + idList[i].ToString() + "'\n");
                    sb.Append("end");
                    mycom.CommandText = sb.ToString();
                    ret = mycom.ExecuteNonQuery();
                    if (ret < 1)
                        flag = 0;
                }
            }
            //查询当前目录下所有子目录
            mycom.CommandText = "select menu_id from Menu_Manage where parent_id='" + menuid + "' and menu_type='catalogue'";
            idList.Clear();
            dr = mycom.ExecuteReader();
            while (dr.Read())
            {
                idList.Add(dr[0].ToString());
            }
            dr.Close();
            dr = null;
            //子目录递归执行当前函数
            for (int i = 0; i < idList.Count; i++)
            {
                SetPowerOne(userid, idList[i].ToString(), powerDes, group, 0);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}