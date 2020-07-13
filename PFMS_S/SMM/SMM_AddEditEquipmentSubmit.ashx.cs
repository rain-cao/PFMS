using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_AddEditEquipmentSubmit 的摘要说明
    /// </summary>
    public class SMM_AddEditEquipmentSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string idStr = context.Request.Form["id"].ToString();
            string Equipment_nameStr = context.Request.Form["Equipment_name"].ToString().Trim().ToUpper();
            string ip_addressStr = context.Request.Form["ip_address"].ToString().Trim();
            string clean_lengthStr = context.Request.Form["clean_length"].ToString().Trim();
            string bind_PLineStr = context.Request.Form["bind_PLine"].ToString();
            string statusStr = "1";
            string msgStr = "";
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
                //当更新时，先获取清洗设备的状态
                if (idStr != "0")
                {
                    mycom.CommandText = "select status from SteelMesh_CleanEquipment where id='" + idStr + "'";
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    statusStr = dr[0].ToString();
                    dr.Close();
                    dr = null;
                }
                //首先插入或更新清洗设备
                if(idStr == "0")
                {
                    sb.Append("insert into SteelMesh_CleanEquipment(Equipment_name,ip_address,clean_length,status)\n");
                    sb.Append("values(N'" + Equipment_nameStr + "','" + ip_addressStr + "','" + clean_lengthStr + "','" + statusStr + "')\n");
                    sb.Append("select @@IDENTITY");
                }
                else
                {
                    sb.Append("declare @update_id int\n");
                    sb.Append("set @update_id=" + idStr + "\n");
                    sb.Append("update SteelMesh_CleanEquipment set Equipment_name=N'" + Equipment_nameStr + "',ip_address='" + ip_addressStr + "',clean_length='" + clean_lengthStr + "',status='" + statusStr + "'\n");
                    sb.Append("select @update_id");
                }
                mycom.CommandText = sb.ToString();
                sb.Clear();
                string updateidStr = mycom.ExecuteScalar().ToString();
                if (updateidStr != "" && updateidStr != null)
                {
                    //然后更新设备绑定线体的关系
                    //1.删除原有关系
                    mycom.CommandText = "delete from SteelMesh_BindEquipAndPLine where equipment_id='" + idStr + "'";
                    mycom.ExecuteNonQuery();
                    //2.插入新的关系
                    bool flag = true;
                    int ret;
                    while (bind_PLineStr.Contains(";"))
                    {
                        mycom.CommandText = "insert into SteelMesh_BindEquipAndPLine(equipment_id,productline_id,status) values('" + updateidStr + "','" + bind_PLineStr.Substring(0, bind_PLineStr.IndexOf(";")) + "','" + statusStr + "')";
                        ret = mycom.ExecuteNonQuery();
                        if (ret <= 0)
                            flag = false;
                        bind_PLineStr = bind_PLineStr.Substring(bind_PLineStr.IndexOf(";") + 1);
                    }
                    if (flag)
                        msgStr = "OK";
                    else
                        msgStr = "插入设备绑定的线体失败";
                }
                else
                {
                    msgStr = "插入或编辑清洗设备失败";
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