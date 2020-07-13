using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace PFMS_S.SMM
{
    public partial class SMM_AddEditSignRule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idStr = Request.QueryString["id"];
            //根据传入的id设置主旨内容
            if (idStr == "0")
            {
                h2Show.InnerText = "新增详细储位";
                btnAddEdit.InnerText = "添加";
            }
            else
            {
                h2Show.InnerText = "编辑详细储位";
                btnAddEdit.InnerText = "编辑";
            }
            input_id.Value = idStr;
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                //查询部门清单到下拉列表框
                mycon.Open();
                mycom = mycon.CreateCommand();
                mycom.CommandText = "select id,dept,dept_code from Department_Manage";
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    dptalertto.Items.Add(new ListItem(dr[1].ToString()+"(" + dr[2].ToString() +")", dr[0].ToString()));
                    dptalertcopy.Items.Add(new ListItem(dr[1].ToString() + "(" + dr[2].ToString() + ")", dr[0].ToString()));
                    dpttransfiniteto.Items.Add(new ListItem(dr[1].ToString() + "(" + dr[2].ToString() + ")", dr[0].ToString()));
                    dpttransfinitecopy.Items.Add(new ListItem(dr[1].ToString() + "(" + dr[2].ToString() + ")", dr[0].ToString()));
                }
                dr.Close();
                dr = null;
                //根据id查询相关审核规则信息
                if(idStr != "0")
                {
                    string alert_mailtoStr = "", alert_mailcopyStr = "", transfinite_mailtoStr = "", transfinite_mailcopyStr = "";
                    mycom.CommandText = "select total_use_count,status_rule,alert_count,alert_mailto,alert_mailcopy,transfinite_mailto,transfinite_mailcopy from SteelMesh_ExamineRule where id='" + idStr + "'";
                    dr = mycom.ExecuteReader();
                    dr.Read();
                    total_use_count.Value = dr[0].ToString();
                    status_rule.Value = dr[1].ToString();
                    alert_count.Value = dr[2].ToString();
                    alert_mailtoStr = dr[3].ToString();
                    alert_mailcopyStr = dr[4].ToString();
                    transfinite_mailtoStr = dr[5].ToString();
                    transfinite_mailcopyStr = dr[6].ToString();
                    //预警发送人员
                    dr.Close();
                    dr = null;
                    string[] arrayMail = new string[] { };
                    Array.Clear(arrayMail, 0, arrayMail.Length);
                    arrayMail = alert_mailtoStr.Split(';');
                    alert_mailtoStr = "";
                    foreach (string item in arrayMail)
                    {
                        if (item != "" && item != null)
                        {
                            mycom.CommandText = "select username,name from User_Manage where user_id='" + item + "'";
                            dr = mycom.ExecuteReader();
                            dr.Read();
                            alert_mailtoStr = alert_mailtoStr + dr[0].ToString() + "(" + dr[1].ToString() + ");";
                        }
                    }
                    //预警抄送人员
                    dr.Close();
                    dr = null;
                    Array.Clear(arrayMail, 0, arrayMail.Length);
                    arrayMail = alert_mailcopyStr.Split(';');
                    alert_mailcopyStr = "";
                    foreach (string item in arrayMail)
                    {
                        if (item != "" && item != null)
                        {
                            mycom.CommandText = "select username,name from User_Manage where user_id='" + item + "'";
                            dr = mycom.ExecuteReader();
                            dr.Read();
                            alert_mailcopyStr = alert_mailcopyStr + dr[0].ToString() + "(" + dr[1].ToString() + ");";
                        }
                    }
                    //超限发送人员
                    dr.Close();
                    dr = null;
                    Array.Clear(arrayMail, 0, arrayMail.Length);
                    arrayMail = transfinite_mailtoStr.Split(';');
                    transfinite_mailtoStr = "";
                    foreach (string item in arrayMail)
                    {
                        if (item != "" && item != null)
                        {
                            mycom.CommandText = "select username,name from User_Manage where user_id='" + item + "'";
                            dr = mycom.ExecuteReader();
                            dr.Read();
                            transfinite_mailtoStr = transfinite_mailtoStr + dr[0].ToString() + "(" + dr[1].ToString() + ");";
                        }
                    }
                    //超限抄送人员
                    dr.Close();
                    dr = null;
                    Array.Clear(arrayMail, 0, arrayMail.Length);
                    arrayMail = transfinite_mailcopyStr.Split(';');
                    transfinite_mailcopyStr = "";
                    foreach (string item in arrayMail)
                    {
                        if (item != "" && item != null)
                        {
                            mycom.CommandText = "select username,name from User_Manage where user_id='" + item + "'";
                            dr = mycom.ExecuteReader();
                            dr.Read();
                            transfinite_mailcopyStr = transfinite_mailcopyStr + dr[0].ToString() + "(" + dr[1].ToString() + ");";
                        }
                    }
                    alert_mailto.Value = alert_mailtoStr;
                    alert_mailcopy.Value = alert_mailcopyStr;
                    transfinite_mailto.Value = transfinite_mailtoStr;
                    transfinite_mailcopy.Value = transfinite_mailcopyStr;
                }
            }
            catch (Exception msg)
            {
                System.Console.WriteLine(msg.Message);
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
        }
    }
}