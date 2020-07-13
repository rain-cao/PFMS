using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using Newtonsoft.Json;

namespace PFMS_S.BM
{
    /// <summary>
    /// UploadFile 的摘要说明
    /// </summary>
    public class UploadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string serverPath = context.Request.Form["Floder"].ToString();
            serverPath = context.Server.MapPath("~") + serverPath;
            string account = context.User.Identity.Name;
            HttpFileCollection postedFiles = context.Request.Files;
            int code = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("filePath", typeof(string)));
            dt.Columns.Add(new DataColumn("fileName", typeof(string)));
            dt.Columns.Add(new DataColumn("uploadFlag", typeof(Boolean)));
            dt.Columns.Add(new DataColumn("failMsg", typeof(string)));

            for (int i = 0; i < postedFiles.Count; i++)
            {
                drow = dt.NewRow();
                drow[0] = serverPath;

                var postedFile = postedFiles[i];
                string file;
                file = postedFile.FileName;

                //创建账号对应的文件夹目录
                if (!Directory.Exists(serverPath))
                    Directory.CreateDirectory(serverPath);
                string fileDirectory = serverPath;

                string ext = Path.GetExtension(file);
                file = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                drow[1] = file;

                fileDirectory = serverPath + "\\" + file;

                try
                {
                    postedFile.SaveAs(fileDirectory);
                    drow[2] = true;
                    drow[3] = "";
                }
                catch(Exception msg)
                {
                    drow[2] = false;
                    drow[3] = msg.Message;
                    code = 1;
                }

                dt.Rows.Add(drow);
            }
            ds.Tables.Add(dt);
            context.Response.AddHeader("Vary", "Accept");
            try
            {
                if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                    context.Response.ContentType = "application/json";
                else
                    context.Response.ContentType = "text/plain";
            }
            catch
            {
                context.Response.ContentType = "text/plain";
            }

            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { code = code, data = ds.Tables[0] }));    //返回JSON数据
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