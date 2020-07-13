using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_GetAccpList 的摘要说明
    /// </summary>
    public class SMM_GetAccpList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            string condition = context.Request.Form["condition"];
            string powerStr = context.Request.Form["power"];
            try
            {
                mycon.Open();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append("SELECT\n");
                sb.Append("id,sn from SteelMesh_List\n");
                sb.Append("WHERE now_status='-1'");
                mycom = mycon.CreateCommand();
                mycom.CommandText = sb.ToString();
                sb.Clear();
                dr = mycom.ExecuteReader();
                int recordCount = 0;
                msgStr = "<table id=\"AccpList\" class=\"layui-table\" style=\"width:100 %;\">";
                msgStr += "<thead><tr>";
                msgStr += "<th style=\"text-align:center;height:23px;width:80px;\">ID</th>";
                msgStr += "<th style=\"text-align:center;height:23px;\">钢网编号</th>";
                msgStr += "<th style=\"text-align:center;height:23px;width:120px;\">当前状态</th>";
                msgStr += "<th style=\"text-align:center;height:23px;width:200px;\">操作</th>";
                msgStr += "</tr></thead>";
                msgStr += "<tbody>";
                while (dr.Read())
                {
                    recordCount++;
                    msgStr += "<tr id=\"trContainer" + recordCount.ToString() + "\">";
                    msgStr += "<td style=\"text-align:center;height:23px;\">" + dr[0].ToString() + "</td>";
                    msgStr += "<td style=\"text-align:center;height:23px;\">" + dr[1].ToString() + "</td>";
                    msgStr += "<td style=\"text-align:center;height:23px;\"><span style=\"background-color:yellow;\">待验证</span></td>";
                    msgStr += "<td style=\"text-align:center;height:23px;\"><a class=\"layui-btn layui-btn-primary layui-btn-xs\" onclick=\"ViewDeatil(this,'tr" + recordCount.ToString() + "'," + dr[0].ToString() + ")\">查看</a>";
                    if (powerStr == "2")
                        msgStr += "<a class=\"layui-btn layui-btn-xs\" onclick=\"AddEditAccptance(" + dr[0].ToString() + ")\">编辑</a><a class=\"layui-btn layui-btn-danger layui-btn-xs\" onclick=\"DeleteThis('trContainer" + recordCount.ToString() + "','tr" + recordCount.ToString() + "'," + dr[0].ToString() + ")\">删除</a>";
                    msgStr += "</td>";
                    msgStr += "</tr>";
                    msgStr += "<tr id=\"tr" + recordCount.ToString() + "\" style=\"display:none\"></tr>";
                }
                if (recordCount == 0)
                    msgStr += "<tr><td colspan=\"4\" style=\"text-align:center;height:23px;\">无数据</td></tr>";
                msgStr += "</tbody>";
                msgStr += "</table>";
                msgStr = "OK:" + msgStr;
            }
            catch (Exception msg)
            {
                msgStr = msg.Message;
            }
            finally
            { 
                if(dr!=null)
                {
                    dr.Close();
                    dr = null;
                }
                if (mycon.State != System.Data.ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            context.Response.Write(msgStr);
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