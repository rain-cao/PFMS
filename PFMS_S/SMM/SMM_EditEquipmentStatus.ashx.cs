using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_EditEquipmentStatus 的摘要说明
    /// </summary>
    public class SMM_EditEquipmentStatus : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msgStr = "";
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string idStr = context.Request.Form["id"];
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                //先更新设备绑定的线体的状态
                sb.Append("update SteelMesh_BindEquipAndPLine set status=\n");
                sb.Append("case status\n");
                sb.Append("when '1' then '0'\n");
                sb.Append("when '0' then '1'\n");
                sb.Append("end\n");
                sb.Append("where equipment_id='" + idStr + "'");
                mycom.CommandText = sb.ToString();
                int ret = mycom.ExecuteNonQuery();
                if (ret > 0)
                {
                    sb.Clear();
                    sb.Append("update SteelMesh_CleanEquipment set status=\n");
                    sb.Append("case status\n");
                    sb.Append("when '0' then '1'\n");
                    sb.Append("when '1' then '0'\n");
                    sb.Append("end\n");
                    sb.Append("where id='" + idStr + "'");
                    mycom.CommandText = sb.ToString();
                    ret = mycom.ExecuteNonQuery();
                    if (ret > 0)
                        msgStr = "OK";
                    else
                        msgStr = "操作影响行数0";
                }
                else
                    msgStr = "更新设备绑定线体的状态失败,操作影响行数0";
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