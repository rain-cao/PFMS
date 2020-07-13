using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace PFMS_S
{
    struct menuItem
    {
        public string menuID;
        public string menuName;
        public string menuURL;
        public string menuIco;
        public string menuPower;
    }
    /// <summary>
    /// GetMenuAndPower 的摘要说明
    /// </summary>
    public class GetMenuAndPower : IHttpHandler, IRequiresSessionState
    {
        menuItem menuitem = new menuItem();  //临时存放从数据表中取出的目录或菜单部分信息，会添加到menuList中，用于主目录与非主目录
        List<menuItem> menuList = new List<menuItem>();   //临时存放当次查询到的所有目录或菜单，会依据这个List里的目录或菜单信息拼接HTML,目前仅用于主目录
        StringBuilder nextMenuHTML = new StringBuilder();  //在查询主目录以下所有目录以及菜单时，存放拼接的HTML，目前仅用于非主目录
        int menuCount = 0;  //查询到的目录及菜单数量，目前仅用于非主目录

        SqlConnection mycon = null;
        SqlCommand mycom = null;
        SqlDataReader dr = null;

        SqlConnection myconT = null;
        SqlCommand mycomT = null;
        SqlDataReader drT = null;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            int resultFlag = 0;
            string msgStr = "";
            //首先判断登陆是否超时并返回超时信息Time Out
            if (context.Session.Count == 0)
            {
                msgStr = "Time Out";
            }
            else
            {
                string firstOrNext = context.Request.Form["FirstOrNext"];
                string menuID = context.Request.Form["menuID"];
                mycon = DBConnect.ConnectSQLServer();
                myconT = DBConnect.ConnectSQLServer();
                try
                {
                    mycon.Open();
                    mycom = mycon.CreateCommand();
                    StringBuilder sb = new StringBuilder();
                    sb.Clear();
                    switch (firstOrNext)
                    {
                        case "0":    //获取主目录
                            sb.Append("SELECT A.menu_id,A.menu_name,A.menu_url,A.icon,B.power_no\n");
                            sb.Append("FROM\n");
                            sb.Append("Menu_Manage A\n");
                            sb.Append("inner join\n");
                            sb.Append("Auth_Manage B\n");
                            sb.Append("ON A.menu_id=B.menu_id\n");
                            sb.Append("WHERE A.parent_id='0' AND A.menu_state='1' AND B.user_id='" + context.Session["user_id"] + "' AND B.power_no<>0\n");
                            sb.Append("ORDER BY A.order_num ASC");
                            mycom.CommandText = sb.ToString();
                            dr = mycom.ExecuteReader();
                            menuList.Clear();
                            while (dr.Read())
                            {
                                menuitem.menuID = dr[0].ToString();
                                menuitem.menuName = dr[1].ToString();
                                menuitem.menuURL = dr[2].ToString();
                                menuitem.menuIco = dr[3].ToString();
                                menuitem.menuPower = dr[4].ToString();
                                menuList.Add(menuitem);
                            }
                            resultFlag = 1;
                            break;
                        case "1":    //获取主目录下所有的次阶目录以及各次级目录下的菜单
                            nextMenuHTML.Clear();
                            menuCount = 0;
                            nextMenuHTML.Append("<div style=\"color:white;margin-left: 15px;line-height:30px;\">\n");
                            if ((msgStr = SearchNextMenu(menuID,1)) == "")
                            {
                                nextMenuHTML.Append("</div>");
                                resultFlag = 1;
                                if(menuCount > 0)
                                    msgStr = nextMenuHTML.ToString();
                            }
                            else
                            {
                                msgStr = "(" + msgStr + ")";
                            }
                            break;
                        default:
                            break;
                    }
                   
                    sb.Clear();
                    switch (firstOrNext)
                    {
                        case "0":    //主目录时，在此处拼接返回的HTML
                            foreach (menuItem m in menuList)
                            {
                                sb.Append("<span style=\"height: 80px; width: 150px; text-align:center; display: inline-block; \" onclick=\"InitNextMenu(this,'" + m.menuID + "');\"><p style=\"line-height:70px; \"><img src=\"Images/" + m.menuIco + "\" style=\"height: 70px; width: 80px; \" /></p><p style=\"line-height:8px; color: white; font-weight:700; margin-top:-5px; \">" + m.menuName + "</p></span>\n");
                            }
                            msgStr = sb.ToString();
                            break;
                        case "1":
                            //直接在上面代码和SearchNextMenu函数中拼接HTML，比较方便
                            break;
                        default:
                            break;
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
                    if (mycon.State != System.Data.ConnectionState.Closed)
                    {
                        mycon.Close();
                    }
                    mycon = null;
                }

                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (myconT.State != System.Data.ConnectionState.Closed)
                {
                    myconT.Close();
                }
                myconT = null;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { Result = resultFlag, Msg = msgStr, Data = "" }));    //返回JSON数据
        }

        private string SearchNextMenu(string menuid,int CNumber)
        {
            string returnStr = "";
            StringBuilder sb = new StringBuilder();
            List<menuItem> menuitemT = new List<menuItem>();
            //1.先获取menuid下阶所有目录
            sb.Clear();
            sb.Append("SELECT A.menu_id,A.menu_name,A.menu_url,A.icon,B.power_no\n");
            sb.Append("FROM\n");
            sb.Append("Menu_Manage A\n");
            sb.Append("inner join\n");
            sb.Append("Auth_Manage B\n");
            sb.Append("ON A.menu_id=B.menu_id\n");
            sb.Append("WHERE A.parent_id='" + menuid + "' AND A.menu_type='catalogue' AND A.menu_state='1' AND B.user_id='" + HttpContext.Current.Session["user_id"] + "' AND B.power_no<>0\n");
            sb.Append("ORDER BY A.order_num ASC");
            try
            {
                myconT.Open();
                mycomT = myconT.CreateCommand();
                mycomT.CommandText = sb.ToString();
                drT = mycomT.ExecuteReader();
                menuitemT.Clear();
                while (drT.Read())
                {
                    menuitem.menuID = drT[0].ToString();
                    menuitem.menuName = drT[1].ToString();
                    menuitem.menuURL = drT[2].ToString();
                    menuitem.menuIco = drT[3].ToString();
                    menuitem.menuPower = drT[4].ToString();
                    menuitemT.Add(menuitem);
                }
            }
            catch (Exception msg)
            {
                returnStr = msg.Message;
            }
            finally
            {
                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (myconT.State != System.Data.ConnectionState.Closed)
                {
                    myconT.Close();
                }
            }
            //2.拼接menuid下所有目录HTML并递归查询对应目录下的所有目录及菜单          
            int id = 0;
            foreach (menuItem m in menuitemT)
            {
                id++;
                if (m.menuIco != "")
                {
                    nextMenuHTML.Append("<div style=\"font-weight:bolder;font-size:15px;\" onclick=\"catalogueClick('div" + CNumber.ToString() + id.ToString() + "')\"><img src=\"Images/" + m.menuIco + "\" style=\"width: 15px; height: 15px;\" />" + m.menuName + "</div>\n");
                }
                else
                {
                    nextMenuHTML.Append("<div style=\"font-weight:bolder;font-size:15px;\"  onclick=\"catalogueClick('div" + CNumber.ToString() + id.ToString() + "')\">" + m.menuName + "<div>\n");
                }
                nextMenuHTML.Append("<div id=\"div" + CNumber.ToString() + id.ToString() + "\" style=\"margin-left: 15px;line-height:30px;display:none;\" >\n");
                string strT = SearchNextMenu(m.menuID, CNumber++);
                if (strT != "")
                {
                    returnStr = returnStr + "," + strT;
                }
                nextMenuHTML.Append("</div>\n");
                menuCount++;

            }
            //3.再获取menuid下阶所有菜单
            sb.Clear();
            sb.Append("SELECT A.menu_id,A.menu_name,A.menu_url,A.icon,B.power_no\n");
            sb.Append("FROM\n");
            sb.Append("Menu_Manage A\n");
            sb.Append("inner join\n");
            sb.Append("Auth_Manage B\n");
            sb.Append("ON A.menu_id=B.menu_id\n");
            sb.Append("WHERE A.parent_id='" + menuid + "' AND A.menu_type='menu' AND A.menu_state='1' AND B.user_id='" + HttpContext.Current.Session["user_id"] + "' AND B.power_no<>0\n");
            sb.Append("ORDER BY A.order_num ASC");
            try
            {
                myconT.Open();
                mycomT = myconT.CreateCommand();
                mycomT.CommandText = sb.ToString();
                drT = mycomT.ExecuteReader();
                menuitemT.Clear();
                while (drT.Read())
                {
                    menuitem.menuID = drT[0].ToString();
                    menuitem.menuName = drT[1].ToString();
                    menuitem.menuURL = drT[2].ToString();
                    menuitem.menuIco = drT[3].ToString();
                    menuitem.menuPower = drT[4].ToString();
                    menuitemT.Add(menuitem);
                }
            }
            catch (Exception msg)
            {
                returnStr = "{" + msg.Message + "}";
            }
            finally
            {
                if (drT != null)
                {
                    drT.Close();
                    drT = null;
                }
                if (myconT.State != System.Data.ConnectionState.Closed)
                {
                    myconT.Close();
                }
            }
            //4.拼接menuid下所有菜单HTML
            id = 0;
            foreach (menuItem m in menuitemT)
            {
                id++;
                if (m.menuIco != "")
                {
                    if (m.menuURL != "")
                    {
                        nextMenuHTML.Append("<div><img src=\"Images/" + m.menuIco + "\" style=\"width: 15px; height: 15px;\" /><a href=\"javascript:;\" onclick=\"aClick(this,'input" + CNumber.ToString() + id.ToString() + "')\">" + m.menuName + "</a><input id=\"input" + CNumber.ToString() + id.ToString() + "\" type=\"hidden\" value=\"" + m.menuURL + "?power=" + m.menuPower + "\" /></div>\n");
                    }
                    else
                    {
                        nextMenuHTML.Append("<div><img src=\"Images/" + m.menuIco + "\" style=\"width: 15px; height: 15px;\" /><a href=\"javascript:;\" onclick=\"aClick(this,'input" + CNumber.ToString() + id.ToString() + "')\">" + m.menuName + "</a><input id=\"input" + CNumber.ToString() + id.ToString() + "\" type=\"hidden\" value=\"\" /></div>\n");
                    }
                }
                else
                {
                    if (m.menuURL != "")
                    {
                        nextMenuHTML.Append("<div><a href=\"javascript:;\" onclick=\"aClick(this,'input" + CNumber.ToString() + id.ToString() + "')\">" + m.menuName + "</a><input id=\"input" + CNumber.ToString() + id.ToString() + "\" type=\"hidden\" value=\"" + m.menuURL + "?power=" + m.menuPower + "\" /></div>\n");
                    }
                    else
                    {
                        nextMenuHTML.Append("<div><a href=\"javascript:;\" onclick=\"aClick(this,'input" + CNumber.ToString() + id.ToString() + "')\">" + m.menuName + "</a><input id=\"input" + CNumber.ToString() + id.ToString() + "\" type=\"hidden\" value=\"\" /></div>\n");
                    }
                }
                menuCount++;
            }
            return returnStr;
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