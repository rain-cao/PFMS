using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Web.SessionState;

namespace PFMS_S
{
    /// <summary>
    /// SMM_AddEditManufacturerSubmit 的摘要说明
    /// </summary>
    public class SMM_AddEditManufacturerSubmit : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string returnStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string idStr = context.Request.Form["input_id"];
            string nameStr = context.Request.Form["name"];
            nameStr = nameStr.Trim();
            string codeStr = context.Request.Form["code"];
            codeStr = codeStr.ToUpper().Trim();
            string responsibleStr = context.Request.Form["responsible"];
            responsibleStr = responsibleStr.Trim();
            string telephoneStr = context.Request.Form["telephone"];
            telephoneStr = telephoneStr.Trim();
            string emailStr = context.Request.Form["email"];
            emailStr = emailStr.ToLower().Trim();
            string addressStr = context.Request.Form["address"];
            addressStr = addressStr.Trim();
            string statusStr = "1";
            string remarkStr = context.Request.Form["remark"];
            remarkStr = remarkStr.Trim();
            string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string update_time = create_timeStr;
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                if (idStr == "0")
                {
                    sb.Append("if not exists(select id from SteelMesh_Manufacturer_Management where name='" + nameStr + "')\n");
                    sb.Append("begin\n");
                    sb.Append("insert into SteelMesh_Manufacturer_Management(name,code,responsible,telephone,email,address,status,remark,create_by,create_time,update_by,update_time)\n");
                    sb.Append("values(N'" + nameStr + "',N'" + codeStr + "',N'" + responsibleStr + "','" + telephoneStr + "','" + emailStr + "',N'" + addressStr + "','" + statusStr + "',N'" + remarkStr + "','" + context.Session["user_id"] + "','" + create_timeStr + "','" + context.Session["user_id"] + "','" + update_time + "')\n");
                    sb.Append("end\n");
                    mycom.CommandText = sb.ToString();
                    sb.Clear();
                    int ret = mycom.ExecuteNonQuery();
                    if (ret < 1)
                    {
                        returnStr = "厂商已存在,请确认!";
                    }
                    else
                    {
                        returnStr = "OK";
                    }
                }
                else
                {
                    mycom.CommandText = "update SteelMesh_Manufacturer_Management set name=N'" + nameStr + "',code=N'" + codeStr + "',responsible=N'" + responsibleStr + "',telephone='" + telephoneStr + "',email='" + emailStr + "',address='" + addressStr + "',remark='" + remarkStr + "',update_by='" + context.Session["user_id"] + "',update_time='" + update_time + "' where id='" + idStr + "'";
                    int ret = mycom.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        returnStr = "OK";
                    }
                    else
                    {
                        returnStr = "厂商编辑更新失败!";
                    }
                }
            }
            catch (Exception msg)
            {
                returnStr = msg.Message;
            }
            finally
            {
                if (mycon.State != System.Data.ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            context.Response.Write(returnStr);
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