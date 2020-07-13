using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Web.SessionState;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_AddEditDeatilVerifyFormSubmit 的摘要说明
    /// </summary>
    public class SMM_AddEditDeatilVerifyFormSubmit : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");context.Session["user_id"]
            string msgStr = "";
            int verifyCount = Convert.ToInt32(context.Request.Form["verifyCount"].ToString());
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            string smlist_idStr = "", smverifyinfolist_idStr = "", verify_contentStr = "", contentTmpStr = "", remarkStr = "";
            int verifyType = -1;
            smlist_idStr = context.Request.Form["input_id"].ToString();
            if (smlist_idStr != "0")
            {
                string create_byStr = context.Session["user_id"].ToString();
                string create_timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string update_byStr = create_byStr;
                string update_timeStr = create_timeStr;
                mycon = DBConnect.ConnectSQLServer();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                try
                {
                    mycon.Open();
                    mycom = mycon.CreateCommand();
                    for (int i = 1; i <= verifyCount; i++)
                    {
                        smverifyinfolist_idStr = context.Request.Form["id_" + i.ToString()].ToString();
                        contentTmpStr = context.Request.Form["verify_content_" + i.ToString()].ToString();
                        remarkStr = context.Request.Form["remark_" + i.ToString()].ToString();
                        verifyType = Convert.ToInt16(context.Request.Form["verify_type_" + i.ToString()].ToString());
                        verify_contentStr = "";
                        switch (verifyType)
                        {
                            case 0:
                                verify_contentStr = contentTmpStr + "&&" + remarkStr;
                                break;
                            case 1:
                                verify_contentStr = contentTmpStr;
                                break;
                            case 2:
                                verify_contentStr = contentTmpStr + "&&" + remarkStr;
                                break;
                        }
                        sb.Clear();
                        sb.Append("if not exists(select id from SteelMesh_Detail_VerifyForm_Info_List where smlist_id='" + smlist_idStr + "' and smverifyinfolist_id='" + smverifyinfolist_idStr + "')\n");
                        sb.Append("begin\n");
                        sb.Append("insert into SteelMesh_Detail_VerifyForm_Info_List(smlist_id,smverifyinfolist_id,verify_content,create_by,create_time,update_by,update_time)\n");
                        sb.Append("values('" + smlist_idStr + "','" + smverifyinfolist_idStr + "',N'" + verify_contentStr + "','" + create_byStr + "','" + create_timeStr + "','" + update_byStr + "','" + update_timeStr + "')\n");
                        sb.Append("end\n");
                        sb.Append("else\n");
                        sb.Append("begin\n");
                        sb.Append("update SteelMesh_Detail_VerifyForm_Info_List set verify_content=N'" + verify_contentStr + "',update_by='" + update_byStr + "',update_time='" + update_timeStr + "' where smlist_id='" + smlist_idStr + "' and smverifyinfolist_id='" + smverifyinfolist_idStr + "'\n");
                        sb.Append("end");
                        mycom.CommandText = sb.ToString();
                        int ret = mycom.ExecuteNonQuery();
                        if (ret > 0)
                        {
                            msgStr = "OK";
                        }
                        else
                        {
                            msgStr = "SQL语句：" + sb.ToString() + "  执行失败!";
                            break;
                        }
                        sb.Clear();
                    }
                    if (verifyCount < 1)
                    {
                        msgStr = "验收表单为空";
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
            else
            {
                msgStr = "未指定对应钢网编号，无法编辑提交验收表单!";
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