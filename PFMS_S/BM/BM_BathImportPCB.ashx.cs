using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using System.IO;
using System.Text;

namespace PFMS_S.BM
{
    /// <summary>
    /// BM_BathImportPCB 的摘要说明
    /// </summary>
    public class BM_BathImportPCB : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string filePath = context.Request.Form["filePath"].ToString();
            string fileName = context.Request.Form["fileName"].ToString();
            string file = filePath + "\\" + fileName;

            SqlConnection mycon = null;
            SqlCommand mycom = null;
            StringBuilder sb = new StringBuilder();

            string pcb_pnStr = "";
            string msgStr = "";

            FileStream fs = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;

            using (fs = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                if (Path.GetExtension(file).ToUpper() == ".XLS")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else if (Path.GetExtension(file).ToUpper() == ".XLSX")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else
                {
                    msgStr = "文件格式错误，必须为xls或xlsx格式!";
                }

                if (msgStr == "")
                {
                    mycon = DBConnect.ConnectSQLServer();
                    try
                    {
                        if (workbook != null)
                        {
                            sheet = workbook.GetSheetAt(0);
                            if (sheet != null)
                            {
                                mycon.Open();
                                mycom = mycon.CreateCommand();
                                int ret = 0;
                                int totalCount = sheet.LastRowNum;
                                int successCount = 0;
                                for (int i = (sheet.FirstRowNum + 1); i <= totalCount; i++)
                                {
                                    row = sheet.GetRow(i);
                                    pcb_pnStr = row.GetCell(0).ToString().Trim().ToUpper();
                                    if (pcb_pnStr != "")
                                    {
                                        sb.Clear();
                                        sb.Append("if not exists (select id from PCB_PN where pcb_pn='" + pcb_pnStr + "')\n");
                                        sb.Append("begin\n");
                                        sb.Append("insert into PCB_PN(pcb_pn) values('" + pcb_pnStr + "')\n");
                                        sb.Append("end");
                                        mycom.CommandText = sb.ToString();
                                        ret = mycom.ExecuteNonQuery();
                                        if (ret > 0)
                                        {
                                            successCount++;
                                        }
                                        else
                                        {
                                            msgStr = msgStr + pcb_pnStr + "插入异常,已存在或插入失败!<br />";
                                        }
                                    }
                                }
                                if (msgStr == "")
                                    msgStr = "OK";
                                else
                                {
                                    msgStr = msgStr + "成功插入数据" + successCount.ToString() + "条";
                                }
                            }
                            else
                            {
                                msgStr = "NPOI读取Sheet失败!";
                            }
                        }
                        else
                        {
                            msgStr = "NPOI初始化失败!";
                        }
                    }
                    catch (Exception msg)
                    {
                        msgStr = msg.Message;
                        msgStr += "-->待导入表格中可能存在空行!";
                    }
                    finally
                    {
                        if (mycon.State != System.Data.ConnectionState.Closed)
                            mycon.Close();
                        mycon = null;
                    }
                }
            }

            sheet = null;
            if (workbook != null)
            {
                workbook.Close();
            }
            workbook = null;

            if (File.Exists(file))
                File.Delete(file);

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