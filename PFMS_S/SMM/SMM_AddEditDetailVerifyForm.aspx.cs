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
    public partial class SMM_AddEditDetailVerifyForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string outhtml()
        {
            string idStr = Request.QueryString["id"];
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
                if (idStr != "0")
                {
                    sb.Append("SELECT B.id,B.verify_info,B.verify_type,\n");
                    sb.Append("(SELECT value+';' FROM SteelMesh_VerifyForm_ValueForSelect WHERE smverifyinfolist_id=B.id FOR XML PATH('')) AS verify_type_value,\n");
                    sb.Append("B.check_type,B.standard_value,B.required,A.verify_content\n");
                    sb.Append("FROM SteelMesh_Detail_VerifyForm_Info_List A\n");
                    sb.Append("INNER JOIN SteelMesh_VerifyForm_Info_List B ON A.smverifyinfolist_id=B.id\n");
                    sb.Append("WHERE A.smlist_id='" + idStr + "'\n");
                    sb.Append("ORDER BY B.order_num ASC");
                }
                else
                {
                    sb.Append("SELECT id,verify_info,verify_type,\n");
                    sb.Append("(SELECT value+';' FROM SteelMesh_VerifyForm_ValueForSelect WHERE smverifyinfolist_id=SteelMesh_VerifyForm_Info_List.id FOR XML PATH('')) AS verify_type_value,\n");
                    sb.Append("check_type,standard_value,required,'' as verify_content\n");
                    sb.Append("FROM SteelMesh_VerifyForm_Info_List\n");
                    sb.Append("WHERE status='1'\n");
                    sb.Append("ORDER BY order_num ASC");
                }
                mycom.CommandText = sb.ToString();
                dr = mycom.ExecuteReader();
                int recordCount = 0;
                sb.Clear();
                sb.Append("<input type=\"hidden\" name=\"input_id\" id=\"input_id\" value=\"" + idStr + "\" />\n");
                sb.Append("<table style=\"width: 100%; margin: 0 auto; text-align:center; border: 1px solid gray;border-collapse: collapse;\">\n");
                while (dr.Read())
                {
                    recordCount++;
                    if (dr[1].ToString() == "钢网张力测试A点" || dr[1].ToString() == "钢网张力测试B点"
                                   || dr[1].ToString() == "钢网张力测试C点" || dr[1].ToString() == "钢网张力测试D点"
                                   || dr[1].ToString() == "钢网张力测试E点" || dr[1].ToString() == "钢网张力测试F点"
                                   || dr[1].ToString() == "钢网张力测试G点" || dr[1].ToString() == "钢网张力测试H点"
                                   || dr[1].ToString() == "钢网张力测试I点" || dr[1].ToString() == "钢网张力测试J点")
                    {
                        recordCount--;
                    }
                    else
                    {
                        sb.Append("<tr>\n");
                        sb.Append("<td rowspan=\"2\" style=\"text-align: center;width: 50px;border: 1px solid gray;\">\n");
                        if (dr[6].ToString() == "1")
                            sb.Append("<span style=\"color: red;\" title=\"必填项\">*</span>\n");
                        sb.Append(recordCount.ToString() + "\n");
                        sb.Append("<input type=\"hidden\" id=\"id_" + recordCount.ToString() + "\" name=\"id_" + recordCount.ToString() + "\" value=\"" + dr[0].ToString() + "\" />\n");
                        sb.Append("</td>\n");
                    }
                    switch(dr[2].ToString())
                    {
                        case "0":    //校验信息类型为CheckBox
                            sb.Append("<td style=\"text-align: left;height: 40px;\">\n");
                            //verify_info
                            if (dr[7].ToString().Contains("0&&") || dr[7].ToString() == "")
                            {
                                sb.Append("&nbsp;&nbsp;&nbsp;<input type=\"checkbox\" id=\"verify_info_" + recordCount.ToString() + "\" name=\"verify_info_" + recordCount.ToString() + "\" lay-skin=\"primary\" value=\"" + dr[1].ToString() + "\" lay-filter=\"select_verify_info\" /><span style=\"font-weight:bolder;\">" + dr[1].ToString() + "</span>\n");
                            }
                            else if(dr[7].ToString().Contains("1&&"))
                            {
                                sb.Append("&nbsp;&nbsp;&nbsp;<input type=\"checkbox\" id=\"verify_info_" + recordCount.ToString() + "\" name=\"verify_info_" + recordCount.ToString() + "\" lay-skin=\"primary\" value=\"" + dr[1].ToString() + "\" lay-filter=\"select_verify_info\" checked=\"checked\" /><span style=\"font-weight:bolder;\">" + dr[1].ToString() + "</span>\n");
                            }
                            //verify_type
                            sb.Append("<input type=\"hidden\" id=\"verify_type_" + recordCount.ToString() + "\" name=\"verify_type_" + recordCount.ToString() + "\" value=\"0\" />\n");
                            //verify_content
                            sb.Append("<input type=\"hidden\" id=\"verify_content_" + recordCount.ToString() + "\" name=\"verify_content_" + recordCount.ToString() + "\" value=\"" + ((dr[7].ToString() == "") ? "0" : dr[7].ToString().Substring(0, 1)) + "\" />\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            sb.Append("<tr>\n");
                            //备注内容,与verify_content拼接成一条内容(中间用&&隔开)存储在表单的verify_content栏位
                            sb.Append("<td style=\"border-bottom: 1px solid gray;text-align: left;height: 40px;\">\n");
                            sb.Append("<input name=\"remark_" + recordCount.ToString() + "\" id=\"remark_" + recordCount.ToString() + "\" type=\"text\" value=\"" + ((dr[7].ToString() == "") ? "" : dr[7].ToString().Substring(3)) + "\" placeholder=\"备注信息\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;border:none;border-bottom: 1px solid black;\" />\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            break;
                        case "1":    //校验信息类型为Input
                            if (dr[1].ToString() == "钢网张力测试A点" || dr[1].ToString() == "钢网张力测试B点"
                                   || dr[1].ToString() == "钢网张力测试C点" || dr[1].ToString() == "钢网张力测试D点"
                                   || dr[1].ToString() == "钢网张力测试E点" || dr[1].ToString() == "钢网张力测试F点"
                                   || dr[1].ToString() == "钢网张力测试G点" || dr[1].ToString() == "钢网张力测试H点"
                                   || dr[1].ToString() == "钢网张力测试I点" || dr[1].ToString() == "钢网张力测试J点")
                            {
                                //Response.Write("钢网张力测试在客户端完成，系统在提交验收表单时会先判断是否需要测试钢网张力，以及钢网张力测试结果");
                            }
                            else
                            {
                                sb.Append("<td style=\"text-align: left;height: 40px;\">\n");
                                //verify_info
                                sb.Append("<input type=\"hidden\" id=\"verify_info_" + recordCount.ToString() + "\" name=\"verify_info_" + recordCount.ToString() + "\" value=\"" + dr[1].ToString() + "\" />\n");
                                //verify_type
                                sb.Append("<input type=\"hidden\" id=\"verify_type_" + recordCount.ToString() + "\" name=\"verify_type_" + recordCount.ToString() + "\" value=\"1\" />\n");
                                sb.Append("&nbsp;&nbsp;&nbsp;<span style=\"font-weight:bolder;\">" + dr[1].ToString() + "</span>\n");
                                sb.Append("</td>\n");
                                sb.Append("</tr>\n");
                                sb.Append("<tr>\n");
                                sb.Append("<td style=\"border-bottom: 1px solid gray;text-align: left;height: 40px;\">\n");
                                //required
                                sb.Append("<input type=\"hidden\" id=\"required_" + recordCount.ToString() + "\" name=\"required_" + recordCount.ToString() + "\" value=\"" + dr[6].ToString() + "\" />\n");
                                //check_type
                                sb.Append("<input type=\"hidden\" id=\"check_type_" + recordCount.ToString() + "\" name=\"check_type_" + recordCount.ToString() + "\" value=\"" + dr[4].ToString() + "\" />\n");
                                //standard_value
                                sb.Append("<input type=\"hidden\" id=\"standard_value_" + recordCount.ToString() + "\" name=\"standard_value_" + recordCount.ToString() + "\" value=\"" + dr[5].ToString() + "\" />\n");
                                //verify_content
                                if (dr[6].ToString() == "0")
                                {
                                    sb.Append("<input name=\"verify_content_" + recordCount.ToString() + "\" id=\"verify_content_" + recordCount.ToString() + "\" type=\"text\" value=\"" + dr[7].ToString() + "\" placeholder=\"请填写确认内容\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;border:none;border-bottom: 1px solid black;\" />\n");
                                }
                                else
                                {
                                    sb.Append("<input name=\"verify_content_" + recordCount.ToString() + "\" id=\"verify_content_" + recordCount.ToString() + "\" type=\"text\" value=\"" + dr[7].ToString() + "\" placeholder=\"请填写确认内容\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;border:none;border-bottom: 1px solid black;\" lay-verify=\"verify_input\" required=\"required\" />\n");
                                }
                                sb.Append("</td>\n");
                                sb.Append("</tr>\n");
                            }
                            break;
                        case "2":    //校验信息类型为Select
                            sb.Append("<td style=\"text-align: left;height: 40px;\">\n");
                            //verify_info
                            sb.Append("<input type=\"hidden\" id=\"verify_info_" + recordCount.ToString() + "\" name=\"verify_info_" + recordCount.ToString() + "\" value=\"" + dr[1].ToString() + "\" />\n");
                            //verify_type
                            sb.Append("<input type=\"hidden\" id=\"verify_type_" + recordCount.ToString() + "\" name=\"verify_type_" + recordCount.ToString() + "\" value=\"2\" />\n");
                            sb.Append("&nbsp;&nbsp;&nbsp;<span style=\"font-weight:bolder;\">" + dr[1].ToString() + "</span>\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            sb.Append("<tr>\n");
                            sb.Append("<td style=\"border-bottom: 1px solid gray;text-align: left;height: 40px;\">\n");
                            //verify_content
                            sb.Append("<div style=\"width: 34%;display:inline-block;\">\n");
                            if(dr[6].ToString() == "0")
                                sb.Append("<select name=\"verify_content_" + recordCount.ToString() + "\" id=\"verify_content_" + recordCount.ToString() + "\" class=\"layui-select\">\n");
                            else
                                sb.Append("<select name=\"verify_content_" + recordCount.ToString() + "\" id=\"verify_content_" + recordCount.ToString() + "\" class=\"layui-select\" lay-verify=\"verify_select\" required=\"required\">\n");
                            if (dr[7].ToString().Contains("&&"))
                            {
                                sb.Append("<option value=\"" + dr[7].ToString().Substring(0, dr[7].ToString().IndexOf("&&")) + "\">" + dr[7].ToString().Substring(0, dr[7].ToString().IndexOf("&&")) + "</option>\n");
                            }
                            else
                            {
                                sb.Append("<option value=\"\">请选择...<option>\n");
                            }
                            string strTemp = dr[3].ToString();
                            while (strTemp.Contains(";"))
                            {
                                sb.Append("<option value=\"" + strTemp.Substring(0,strTemp.IndexOf(";")) + "\">" + strTemp.Substring(0, strTemp.IndexOf(";")) + "<option>\n");
                                strTemp = strTemp.Substring(strTemp.IndexOf(";") + 1);
                            }
                            sb.Append("</select>\n");
                            sb.Append("</div>\n");
                            //备注内容,与verify_content拼接成一条内容(中间用&&隔开)存储在表单的verify_content栏位
                            sb.Append("<div style=\"width: 64%;float:right;\">\n");
                            if (dr[7].ToString().Contains("&&"))
                                sb.Append("<input name=\"remark_" + recordCount.ToString() + "\" id=\"remark_" + recordCount.ToString() + "\" type=\"text\" value=\"" + dr[7].ToString().Substring(dr[7].ToString().IndexOf("&&") + 2) + "\" placeholder=\"备注信息\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;border:none;border-bottom: 1px solid black;\" />\n");
                            else
                                sb.Append("<input name=\"remark_" + recordCount.ToString() + "\" id=\"remark_" + recordCount.ToString() + "\" type=\"text\" value=\"\" placeholder=\"备注信息\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;border:none;border-bottom: 1px solid black;\" />\n");
                            sb.Append("</div>\n");
                            sb.Append("</td>\n");
                            sb.Append("</tr>\n");
                            break;
                    }
                }
                if (recordCount > 0)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td colspan=\"2\" style=\"text-align:center;border: 1px solid gray;height: 80px;\">\n");
                    sb.Append("<input type=\"hidden\" id=\"record_count\" name=\"record_count\" value=\"" + recordCount.ToString() + "\" />\n");
                    sb.Append("<button id=\"submit_action\" type=\"submit\" lay-submit=\"\" lay-filter=\"submit_action\" class=\"layui-btn layui-btn-lg\" style=\"margin-top:10px;\">提交</button>\n");
                    sb.Append("<button type=\"reset\" id=\"reset\" class=\"layui-btn layui-btn-lg\" style=\"margin-top:10px;\">重置</button>\n");
                    sb.Append("</td>\n");
                    sb.Append("</tr>\n");
                }
                else
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
            catch(Exception msg)
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