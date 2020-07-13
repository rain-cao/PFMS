using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S.SMM
{
    public partial class SMM_ViewVerifyForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string outhtml()
        {
            string idStr = Request.QueryString["smlist_id"];
            string htmlStr = "";
            //根据传入的id设置主旨内容
            if (idStr == "0")
            {
                h2Show.InnerText = "新增钢网验收表单";
            }
            else
            {
                h2Show.InnerText = "编辑钢网验收表单";
            }
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                sb.Append("SELECT B.verify_info,B.verify_type,A.verify_content\n");
                sb.Append("FROM SteelMesh_Detail_VerifyForm_Info_List A\n");
                sb.Append("INNER JOIN SteelMesh_VerifyForm_Info_List B ON A.smverifyinfolist_id=B.id\n");
                sb.Append("WHERE A.smlist_id='" + idStr + "'\n");
                sb.Append("ORDER BY B.order_num ASC");
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                int recordCount = 0;
                sb.Clear();
                sb.Append("<input type=\"hidden\" name=\"input_id\" id=\"input_id\" value=\"" + idStr + "\" />\n");
                sb.Append("<table style=\"width: 100%; margin: 0 auto; text-align:center; border: 1px solid gray;border-collapse: collapse;\">\n");
                while (dr.Read())
                {
                    recordCount++;
                    sb.Append("<tr>\n");
                    sb.Append("<td rowspan=\"2\" style=\"text-align: center;width: 50px;border: 1px solid gray;\">\n");
                    sb.Append(recordCount.ToString() + "\n");
                    sb.Append("</td>\n");
                    switch (dr[1].ToString())
                    {
                        case "0":    //校验信息类型为CheckBox
                            sb.Append("<td style=\"text-align: left;height: 40px;\">\n");
                            //verify_info
                            if (dr[2].ToString().Contains("0&&") || dr[2].ToString() == "")
                            {
                                sb.Append("&nbsp;&nbsp;&nbsp;<input type=\"checkbox\" lay-skin=\"primary\" disabled=\"disabled\" /><span style=\"font-weight:bolder;\">" + dr[0].ToString() + "</span>\n");
                            }
                            else if (dr[2].ToString().Contains("1&&"))
                            {
                                sb.Append("&nbsp;&nbsp;&nbsp;<input type=\"checkbox\" lay-skin=\"primary\" disabled=\"disabled\" checked=\"checked\" /><span style=\"font-weight:bolder;\">" + dr[0].ToString() + "</span>\n");
                            }
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            sb.Append("<tr>\n");
                            //备注内容,与verify_content拼接成一条内容(中间用&&隔开)存储在表单的verify_content栏位
                            sb.Append("<td style=\"border-bottom: 1px solid gray;text-align: left;height: 40px;\">\n");
                            sb.Append("<input type=\"text\" value=\"" + ((dr[2].ToString() == "") ? "" : dr[2].ToString().Substring(3)) + "\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;border:none;border-bottom: 1px solid black;\" />\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            break;
                        case "1":    //校验信息类型为Input
                            sb.Append("<td style=\"text-align: left;height: 40px;\">\n");
                            //verify_info
                            sb.Append("&nbsp;&nbsp;&nbsp;<span style=\"font-weight:bolder;\">" + dr[0].ToString() + "</span>\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            sb.Append("<tr>\n");
                            sb.Append("<td style=\"border-bottom: 1px solid gray;text-align: left;height: 40px;\">\n");
                            //verify_content
                            sb.Append("&nbsp;&nbsp;&nbsp;<span style=\"font-weight:bolder;\">" + dr[2].ToString() + "</span>\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            break;
                        case "2":    //校验信息类型为Select
                            sb.Append("<td style=\"text-align: left;height: 40px;\">\n");
                            //verify_info
                            sb.Append("&nbsp;&nbsp;&nbsp;<span style=\"font-weight:bolder;\">" + dr[0].ToString() + "</span>\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            sb.Append("<tr>\n");
                            sb.Append("<td style=\"border-bottom: 1px solid gray;text-align: left;height: 40px;\">\n");
                            //verify_content
                            sb.Append("<div style=\"width: 34%;display:inline-block;\">\n");
                            sb.Append("<select class=\"layui-select\" disabled=\"disabled\">\n");
                            if (dr[2].ToString().Contains("&&"))
                            {
                                sb.Append("<option value=\"" + dr[2].ToString().Substring(0, dr[2].ToString().IndexOf("&&")) + "\">" + dr[2].ToString().Substring(0, dr[2].ToString().IndexOf("&&")) + "</option>\n");
                            }
                            else
                            {
                                sb.Append("<option value=\"\">未维护<option>\n");
                            }
                            sb.Append("</select>\n");
                            sb.Append("</div>\n");
                            //备注内容,与verify_content拼接成一条内容(中间用&&隔开)存储在表单的verify_content栏位
                            sb.Append("<div style=\"width: 64%;float:right;\">\n");
                            if (dr[2].ToString().Contains("&&"))
                                sb.Append("&nbsp;&nbsp;&nbsp;<span style=\"font-weight:bolder;\">" + dr[2].ToString().Substring(dr[2].ToString().IndexOf("&&") + 2) + "</span>\n");
                            sb.Append("</div>\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            break;
                    }
                }
                if (recordCount <= 0)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td style=\"text-align:center;height:23px;border: 1px solid gray;\">\n");
                    sb.Append("查无数据\n");
                    sb.Append("</td>\n");
                    sb.Append("</tr>\n");
                }
                sb.Append("</table>");
                htmlStr = sb.ToString();
            }
            catch (Exception msg)
            {
                htmlStr = "Error: " + msg.Message;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
                if (mycon.State != System.Data.ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            return htmlStr;
        }
    }
}