using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;

namespace PFMS_S.SMM
{
    /// <summary>
    /// SMM_GetVerifyInfoList 的摘要说明
    /// </summary>
    public class SMM_GetVerifyInfoList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataAdapter da = null;
            DataSet ds = new DataSet();
            int code = 0;
            string msgStr = "";
            int count = 0;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                StringBuilder sb = new StringBuilder();
                mycon.Open();
                mycom = mycon.CreateCommand();
                sb.Clear();
                sb.Append("select status,id,order_num,verify_info,\n");
                sb.Append("case verify_type\n");
                sb.Append("when '0' then '复选框'\n");
                sb.Append("when '1' then '文本输入框'\n");
                sb.Append("when '2' then '下拉列表框'\n");
                sb.Append("end as verify_type,(select value+';' from SteelMesh_VerifyForm_ValueForSelect where smverifyinfolist_id=SteelMesh_VerifyForm_Info_List.id for xml path('')) as select_value,\n");
                sb.Append("case check_type\n");
                sb.Append("when '0' then '无需校对'\n");
                sb.Append("when '1' then '数值校对'\n");
                sb.Append("when '2' then '文本校对'\n");
                sb.Append("else '无需校对'\n");
                sb.Append("end as check_type,standard_value,remark,required,null as oprate\n");
                sb.Append("from SteelMesh_VerifyForm_Info_List\n");
                sb.Append("order by order_num asc");
                mycom.CommandText = sb.ToString();
                sb.Clear();
                da = new SqlDataAdapter(mycom.CommandText, mycon);
                da.Fill(ds);
            }
            catch (Exception msg)
            {
                code = 1;
                msgStr = msg.Message;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
                if (mycon.State != ConnectionState.Closed)
                    mycon.Close();
                mycon = null;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { code = code, msg = msgStr, count = ds.Tables[0].Rows.Count, data = ds.Tables[0] }));    //返回JSON数据
            ds.Dispose();
            ds = null;
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