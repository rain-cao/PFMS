using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_AddEditVerifyFormSubmit 的摘要说明
    /// </summary>
    public class SMM_AddEditVerifyFormSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string msgStr = "";
            string idStr = context.Request.Form["input_id"].ToString();
            string verify_infoStr = context.Request.Form["verify_info"].ToString().Trim();
            string verify_typeStr = context.Request.Form["verify_type"].ToString();
            string select_valueStr = context.Request.Form["select_value"].ToString().Trim();
            if (select_valueStr != "" && select_valueStr.Substring(select_valueStr.Length - 1) != ";")
                select_valueStr += ";";
            string check_typeStr = context.Request.Form["check_type"].ToString();
            string standard_valueStr = context.Request.Form["standard_value"].ToString().Trim();
            if (standard_valueStr != "" && check_typeStr == "1" && standard_valueStr.Substring(standard_valueStr.Length - 1) != ";")
                standard_valueStr += ";";
            if (check_typeStr != "0")
            {
                if (standard_valueStr == "")
                    msgStr = "请输入校对标准";
                else
                {
                    if (check_typeStr == "1" && standard_valueStr.IndexOf(";") < 0)
                        msgStr = "数值校对时，最大值与最小值之间需以英文;隔开";
                }
            }
            string statusStr = "1";
            string requiredStr = context.Request.Form["required"].ToString();
            string remarkStr = context.Request.Form["remark"].ToString().Trim();
            string create_byStr = context.Session["user_id"].ToString();
            string update_byStr = create_byStr;
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string update_timeStr = create_timeStr;
            if (msgStr == "")
            {
                SqlConnection mycon = null;
                SqlCommand mycom = null;
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                mycon = DBConnect.ConnectSQLServer();
                try
                {
                    mycon.Open();
                    mycom = mycon.CreateCommand();
                    if (idStr == "0")
                    {
                        sb.Append("insert into SteelMesh_VerifyForm_Info_List(verify_info,verify_type,check_type,standard_value,status,required,order_num,remark,create_by,create_time,update_by,update_time)\n");
                        sb.Append("values(N'" + verify_infoStr + "','" + verify_typeStr + "','" + check_typeStr + "',N'" + standard_valueStr + "','" + statusStr + "','" + requiredStr + "',\n");
                        sb.Append("case (select count(*) from SteelMesh_VerifyForm_Info_List)\n");
                        sb.Append("when '0' then '1'\n");
                        sb.Append("else ((select max(order_num) from SteelMesh_VerifyForm_Info_List) + 1)\n");
                        sb.Append("end,N'" + remarkStr + "','" + create_byStr + "','" + create_timeStr + "','" + update_byStr + "','" + update_timeStr + "')\n");
                        sb.Append("select @@IDENTITY");
                    }
                    else
                    {
                        sb.Append("declare @update_id int\n");
                        sb.Append("set @update_id=" + idStr + "\n");
                        sb.Append("update SteelMesh_VerifyForm_Info_List set verify_info=N'" + verify_infoStr + "',verify_type='" + verify_typeStr + "',check_type='" + check_typeStr + "',standard_value=N'" + standard_valueStr + "',remark=N'" + remarkStr + "',update_by='" + update_byStr + "',update_time='" + update_timeStr + "' where id='" + idStr + "'\n");
                        sb.Append("select @update_id");
                    }
                    mycom.CommandText = sb.ToString();
                    sb.Clear();
                    string updateidStr = mycom.ExecuteScalar().ToString();
                    if (updateidStr != "" && updateidStr != null)
                    {
                        if (idStr != "0" && verify_typeStr == "2")
                        {
                            mycom.CommandText = "delete from SteelMesh_VerifyForm_ValueForSelect where smverifyinfolist_id='" + idStr + "'";
                            mycom.ExecuteNonQuery();
                        }
                        if (verify_typeStr == "2")
                        {
                            while (select_valueStr.Contains(";"))
                            {
                                mycom.CommandText = "insert into SteelMesh_VerifyForm_ValueForSelect(smverifyinfolist_id,value) values('" + updateidStr + "',N'" + select_valueStr.Substring(0, select_valueStr.IndexOf(";")) + "')";
                                mycom.ExecuteNonQuery();
                                select_valueStr = select_valueStr.Substring(select_valueStr.IndexOf(";") + 1);
                            }
                        }
                        msgStr = "OK";
                    }
                    else
                    {
                        msgStr = "新增或编辑验收表单信息时插入数据库失败";
                    }
                }
                catch (Exception msg)
                {
                    msgStr = msg.Message;
                }
                finally
                {
                    if (mycon.State != System.Data.ConnectionState.Closed)
                        mycon.Close();
                    mycon = null;
                }
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