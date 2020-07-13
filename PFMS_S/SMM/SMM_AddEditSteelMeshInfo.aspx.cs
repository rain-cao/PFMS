using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace PFMS_S.SMM
{
    public partial class SMM_AddEditSteelMeshInfo : System.Web.UI.Page
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
                h2Show.InnerText = "新增钢网";
            }
            else
            {
                h2Show.InnerText = "编辑钢网信息";
            }
            SqlConnection mycon = null;
            SqlCommand mycom = null;
            SqlDataReader dr = null;
            DataTable dt = new DataTable();
            DataRow drow;
            dt.Columns.Add(new DataColumn("info_name", typeof(string)));
            dt.Columns.Add(new DataColumn("info_value", typeof(string)));
            dt.Columns.Add(new DataColumn("status", typeof(Int16)));
            dt.Columns.Add(new DataColumn("required", typeof(Int16)));
            mycon = DBConnect.ConnectSQLServer();
            try
            {
                mycon.Open();
                mycom = mycon.CreateCommand();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                if (idStr == "0")
                {
                    sb.Append("select info_name,'' as info_value,status,required from SteelMesh_Info_List");
                }
                else
                {
                    sb.Append("select B.info_name,\n");
                    sb.Append("case B.info_name\n");
                    sb.Append("when '钢网编号' then (select sn from SteelMesh_List where id='" + idStr + "')\n");
                    sb.Append("else A.info_value\n");
                    sb.Append("end as info_value,B.status,B.required\n");
                    sb.Append("from (select * from SteelMesh_Detail_Info_List where smlist_id='" + idStr + "') A\n");
                    sb.Append("right join SteelMesh_Info_List B on A.sminfolist_id=B.id");
                }
                mycom.CommandText = sb.ToString();
                sb.Clear();
                dr = mycom.ExecuteReader();
                while (dr.Read())
                {
                    drow = dt.NewRow();
                    drow[0] = dr[0].ToString();
                    drow[1] = dr[1].ToString();
                    drow[2] = dr[2].ToString();
                    drow[3] = dr[3].ToString();
                    dt.Rows.Add(drow);
                }
                dr.Close();
                dr = null;
                sb.Append("<table runat=\"server\" id=\"table\" style=\"width: 1000px; margin: 0 auto; text-align:center;\">\n");
                sb.Append("<tr>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px; width: 120px;\">钢网编号：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left; width: 213px;\">\n");
                sb.Append("        <input type=\"hidden\" name=\"input_id\" id=\"input_id\" value=\"" + idStr + "\" />\n");
                int exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if(drow2[0].ToString().Trim() == "钢网编号")
                    {
                        exitFlag = 1;
                        if(drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"sn\" id=\"sn\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;background-color: #E2E2E2;\" disabled=\"disabled\"/>\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"sn\" id=\"sn\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入钢网编号\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;\" lay-verify=\"sn\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"sn\" id=\"sn\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入钢网编号\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;\" />\n");
                        }
                        break;
                    }
                }
                if(exitFlag == 0)
                    sb.Append("        <input name=\"sn\" id=\"sn\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;background-color: #E2E2E2;\" disabled=\"disabled\"/>\n");
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px; width: 120px;\">钢网料号：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left; width: 213px;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "钢网料号")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"pn\" id=\"pn\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"pn\" id=\"pn\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入钢网料号\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;\" lay-verify=\"pn\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"pn\" id=\"pn\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入钢网料号\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;\" />\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input name=\"pn\" id=\"pn\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px; width: 120px;\">钢网名称：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left; width: 214px;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "钢网名称")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"name\" id=\"name\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"name\" id=\"name\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入钢网名称\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;\" lay-verify=\"name\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"name\" id=\"name\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入钢网名称\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;\" />\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input name=\"name\" id=\"name\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%; text-transform:uppercase;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("</tr>\n");
                sb.Append("<tr>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">钢网尺寸：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "钢网尺寸")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"size\" id=\"size\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"size\" id=\"size\" class=\"layui-select\" lay-verify=\"size\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"size\" id=\"size\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + drow2[1].ToString().Trim() + "</option>\n");
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            mycom.CommandText = "select size from SteelMeshSize";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"size\" id=\"size\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">钢网厚度：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "钢网厚度")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"thickness\" id=\"thickness\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"thickness\" id=\"thickness\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入钢网厚度\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" lay-verify=\"thickness\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"thickness\" id=\"thickness\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入钢网厚度\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" />\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input name=\"thickness\" id=\"thickness\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">钢网材料：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "钢网材料")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"material\" id=\"material\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"material\" id=\"material\" class=\"layui-select\" lay-verify=\"material\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"material\" id=\"material\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + drow2[1].ToString().Trim() + "</option>\n");
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            mycom.CommandText = "select Material from SteelMeshMaterial";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"material\" id=\"material\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("</tr>\n");
                sb.Append("<tr>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">钢网工艺：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "钢网工艺")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"technology\" id=\"technology\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"technology\" id=\"technology\" class=\"layui-select\" lay-verify=\"technology\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"technology\" id=\"technology\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + drow2[1].ToString().Trim() + "</option>\n");
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            mycom.CommandText = "select technology from SteelMesh_Technology";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"technology\" id=\"technology\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">阶梯钢网：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "阶梯钢网")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"ladder\" id=\"ladder\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"ladder\" id=\"ladder\" class=\"layui-select\" lay-verify=\"ladder\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"ladder\" id=\"ladder\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + drow2[1].ToString().Trim() + "</option>\n");
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            mycom.CommandText = "select stair from SteelMesh_Stair";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"ladder\" id=\"ladder\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">拼版数量：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "拼版数量")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"imposition\" id=\"imposition\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"imposition\" id=\"imposition\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入拼版数量\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" lay-verify=\"imposition\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"imposition\" id=\"imposition\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入拼版数量\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" />\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input name=\"imposition\" id=\"imposition\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("</tr>\n");
                sb.Append("<tr>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">表面处理方式：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "表面处理方式")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"treatment\" id=\"treatment\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"treatment\" id=\"treatment\" class=\"layui-select\" lay-verify=\"treatment\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"treatment\" id=\"treatment\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + drow2[1].ToString().Trim() + "</option>\n");
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            mycom.CommandText = "select st from SteelMesh_SurfaceTreatment";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"treatment\" id=\"treatment\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">印刷面：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "印刷面")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"printing\" id=\"printing\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"printing\" id=\"printing\" class=\"layui-select\" lay-verify=\"printing\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"printing\" id=\"printing\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() == "F")
                            {
                                sb.Append("            <option value=\"F\">正面</option>\n");
                            }
                            else if (drow2[1].ToString().Trim() == "B")
                            {
                                sb.Append("            <option value=\"B\">背面</option>\n");
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            sb.Append("            <option value=\"F\">正面</option>\n");
                            sb.Append("            <option value=\"B\">背面</option>\n");
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"printing\" id=\"printing\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">单板印刷次数：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "单板印刷次数")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"printcount\" id=\"printcount\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"printcount\" id=\"printcount\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入单板印刷次数\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" lay-verify=\"printcount\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"printcount\" id=\"printcount\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"请输入单板印刷次数\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" />\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input name=\"printcount\" id=\"printcount\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("</tr>\n");
                sb.Append("<tr>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">是否含铅：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "有铅无铅")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"leaded\" id=\"leaded\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"leaded\" id=\"leaded\" class=\"layui-select\" lay-verify=\"leaded\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"leaded\" id=\"leaded\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() == "Y")
                            {
                                sb.Append("            <option value=\"Y\">是</option>\n");
                            }
                            else if (drow2[1].ToString().Trim() == "N")
                            {
                                sb.Append("            <option value=\"N\">否</option>\n");
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            sb.Append("            <option value=\"Y\">是</option>\n");
                            sb.Append("            <option value=\"N\">否</option>\n");
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"leaded\" id=\"leaded\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">清洗类型：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "清洗类型")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"clean\" id=\"clean\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"clean\" id=\"clean\" class=\"layui-select\" lay-verify=\"clean\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"clean\" id=\"clean\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + drow2[1].ToString().Trim() + "</option>\n");
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            mycom.CommandText = "select clean_type from SteelMesh_CleanType";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"clean\" id=\"clean\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">供应商：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "供应商")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"manufacturer\" id=\"manufacturer\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"manufacturer\" id=\"manufacturer\" class=\"layui-select\" lay-verify=\"manufacturer\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"manufacturer\" id=\"manufacturer\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                mycom.CommandText = "select name from SteelMesh_Manufacturer_Management where id='" + drow2[1].ToString().Trim() + "'";
                                dr = mycom.ExecuteReader();
                                dr.Read();
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + dr[0].ToString() + "</option>\n");
                                dr.Close();
                                dr = null;
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            mycom.CommandText = "select id,name from SteelMesh_Manufacturer_Management where status='1'";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[1].ToString().Trim() + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"manufacturer\" id=\"manufacturer\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("</tr>\n");
                sb.Append("<tr>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">专供客户：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "客户")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"custormer\" id=\"custormer\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"custormer\" id=\"custormer\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"专用钢网应用产品所对应的客户\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" lay-verify=\"custormer\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"custormer\" id=\"custormer\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"专用钢网应用产品所对应的客户\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" />\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input name=\"custormer\" id=\"custormer\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">验收日期：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "验收日期")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"verifydate\" id=\"verifydate\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"verifydate\" id=\"verifydate\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"yyyy-MM-dd\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" lay-verify=\"verifydate\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"verifydate\" id=\"verifydate\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"yyyy-MM-dd\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" />\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input name=\"verifydate\" id=\"verifydate\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">审核规则：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\" title=\"请参考'参数设定--审核规则'中的ID进行选择\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "审核规则")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"audit\" id=\"audit\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"audit\" id=\"audit\" class=\"layui-select\" lay-verify=\"audit\" required=\"required\" title=\"请参考'参数设定--审核规则'中的ID进行选择\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"audit\" id=\"audit\" class=\"layui-select\" title=\"请参考'参数设定--审核规则'中的ID进行选择\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + drow2[1].ToString() + "</option>\n");
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            mycom.CommandText = "select id from SteelMesh_ExamineRule";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"audit\" id=\"audit\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("</tr>\n");
                sb.Append("<tr>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">清洗间隔时间：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "清洗间隔时间")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"intervalT\" id=\"intervalT\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"intervalT\" id=\"intervalT\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"使用中清洗间隔时间，单位分钟\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" lay-verify=\"intervalT\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"intervalT\" id=\"intervalT\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"使用中清洗间隔时间，单位分钟\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" />\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input name=\"intervalT\" id=\"intervalT\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">清洗间隔次数：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "清洗间隔次数")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"intervalC\" id=\"intervalC\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                                sb.Append("        <input name=\"intervalC\" id=\"intervalC\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"使用中清洗间隔次数，单位次\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" lay-verify=\"intervalC\" required=\"required\" />\n");
                            else
                                sb.Append("        <input name=\"intervalC\" id=\"intervalC\" type=\"text\" value=\"" + drow2[1].ToString().Trim() + "\" placeholder=\"使用中清洗间隔次数，单位次\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;\" />\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input name=\"intervalC\" id=\"intervalC\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">存放储位：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "存放储位")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"storage\" id=\"storage\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("        <select name=\"storage\" id=\"storage\" class=\"layui-select\" lay-verify=\"storage\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("        <select name=\"storage\" id=\"storage\" class=\"layui-select\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                mycom.CommandText = "SELECT A.storage_name,B.storage_name FROM SteelMesh_Detail_Storage A INNER JOIN SteelMesh_Storage_Management B ON A.storage_id=B.id WHERE A.id='" + drow2[1].ToString().Trim() + "'";
                                dr = mycom.ExecuteReader();
                                dr.Read();
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "(" + dr[1].ToString().Trim() + ")" + "</option>\n");
                                dr.Close();
                                dr = null;
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">请选择...</option>\n");
                            }
                            mycom.CommandText = "SELECT A.id,A.storage_name,B.storage_name FROM SteelMesh_Detail_Storage A INNER JOIN SteelMesh_Storage_Management B ON A.storage_id=B.id WHERE A.status='1' AND B.status='1'";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[1].ToString().Trim() + "(" + dr[2].ToString().Trim() + ")" + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"storage\" id=\"storage\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("    </td>\n");
                sb.Append("</tr>\n");
                sb.Append("<tr>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">工程人员：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                sb.Append("        <div style=\"display: inline-block; width: 100px;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "工程人员")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input name=\"deptE\" id=\"deptE\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 49%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            sb.Append("            <select name=\"deptE\" id=\"deptE\" class=\"layui-select\" style=\"width:50%\" lay-filter=\"deptE\">\n");
                            sb.Append("            <option value=\"\">部门...</option>\n");
                            mycom.CommandText = "select id,dept,dept_code from Department_Manage";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[1].ToString().Trim() + "(" + dr[2].ToString().Trim() + ")" + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("            </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("        <input name=\"deptE\" id=\"deptE\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 49%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("        </div>\n");
                sb.Append("        <div class=\"layui-form\" style=\"display: inline-block; width: 100px; float:right;\" lay-filter=\"engineer\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "工程人员")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("            <input name=\"engineer\" id=\"engineer\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 49%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("            <select name=\"engineer\" id=\"engineer\" class=\"layui-select\" style=\"width: 50%\" lay-verify=\"engineer\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("            <select name=\"engineer\" id=\"engineer\" class=\"layui-select\" style=\"width: 50%\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                mycom.CommandText = "select name,username from User_Manage where user_id='" + drow2[1].ToString().Trim() + "'";
                                dr = mycom.ExecuteReader();
                                dr.Read();
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "(" + dr[1].ToString().Trim() + ")" + "</option>\n");
                                dr.Close();
                                dr = null;
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">人员...</option>\n");
                            }
                            mycom.CommandText = "select user_id,name,username from User_Manage where user_state='1'";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[1].ToString().Trim() + "(" + dr[2].ToString().Trim() + ")" + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("            <input name=\"engineer\" id=\"engineer\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 49%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("        </div>\n");
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">入库人员：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                sb.Append("        <div style=\"display: inline-block; width: 100px;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "入库人员")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("            <input name=\"deptW\" id=\"deptW\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            sb.Append("            <select name=\"deptW\" id=\"deptW\" class=\"layui-select\" style=\"width:50%\" lay-filter=\"deptW\">\n");
                            sb.Append("            <option value=\"\">部门...</option>\n");
                            mycom.CommandText = "select id,dept,dept_code from Department_Manage";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[1].ToString().Trim() + "(" + dr[2].ToString().Trim() + ")" + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("            </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("            <input name=\"deptW\" id=\"deptW\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("        </div>\n");
                sb.Append("        <div class=\"layui-form\" style=\"display: inline-block; width: 100px; float:right;\" lay-filter=\"warehousing\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "入库人员")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("            <input name=\"warehousing\" id=\"warehousing\" type=\"text\" value=\"\" placeholder=\"未启用\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[3].ToString().Trim() == "1")
                            {
                                sb.Append("            <select name=\"warehousing\" id=\"warehousing\" class=\"layui-select\" style=\"width: 50%\" lay-verify=\"warehousing\" required=\"required\">\n");
                            }
                            else
                            {
                                sb.Append("            <select name=\"warehousing\" id=\"warehousing\" class=\"layui-select\" style=\"width: 50%\">\n");
                            }
                            if (drow2[1].ToString().Trim() != "")
                            {
                                mycom.CommandText = "select name,username from User_Manage where user_id='" + drow2[1].ToString().Trim() + "'";
                                dr = mycom.ExecuteReader();
                                dr.Read();
                                sb.Append("            <option value=\"" + drow2[1].ToString().Trim() + "\">" + dr[0].ToString().Trim() + "(" + dr[1].ToString().Trim() + ")" + "</option>\n");
                                dr.Close();
                                dr = null;
                            }
                            else
                            {
                                sb.Append("            <option value=\"\">人员...</option>\n");
                            }
                            mycom.CommandText = "select user_id,name,username from User_Manage where user_state='1'";
                            dr = mycom.ExecuteReader();
                            while (dr.Read())
                            {
                                sb.Append("            <option value=\"" + dr[0].ToString().Trim() + "\">" + dr[1].ToString().Trim() + "(" + dr[2].ToString().Trim() + ")" + "</option>\n");
                            }
                            dr.Close();
                            dr = null;
                            sb.Append("        </select>\n");
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                {
                    sb.Append("            <input name=\"warehousing\" id=\"warehousing\" type=\"text\" value=\"\" placeholder=\"信息不存在\" autocomplete=\"off\" class=\"layui-input\" style=\"width: 100%;background-color: #E2E2E2;\" disabled=\"disabled\" />\n");
                }
                sb.Append("        </div>\n");
                sb.Append("    </td>\n");
                sb.Append("    <td style=\"text-align: right; height: 50px;\">验收表单：</td>\n");
                sb.Append("    <td style=\"height: 50px; text-align: left;\">\n");
                exitFlag = 0;
                foreach (DataRow drow2 in dt.Rows)
                {
                    if (drow2[0].ToString().Trim() == "验收表单")
                    {
                        exitFlag = 1;
                        if (drow2[2].ToString().Trim() == "0")
                        {
                            sb.Append("        <input type=\"checkbox\" id=\"verifyform\" name=\"verifyform\" lay-filter=\"verifyform\" title=\"未启用\" disabled=\"disabled\" />\n");
                        }
                        else
                        {
                            if (drow2[1].ToString().Trim() == "1")
                            {
                                sb.Append("        <input type=\"checkbox\" id=\"verifyform\" name=\"verifyform\" lay-filter=\"verifyform\" title=\"必须\" checked=\"checked\" />\n");
                                sb.Append("        &nbsp;&nbsp;<a href=\"javascript:;\" style=\"color: blue; text-decoration:underline; cursor: pointer;\" onclick=\"\">点击填写表单</a>\n");
                            }
                            else
                            {
                                sb.Append("        <input type=\"checkbox\" id=\"verifyform\" name=\"verifyform\" lay-filter=\"verifyform\" title=\"必须\" />\n");
                                sb.Append("        &nbsp;&nbsp;<a id=\"form_entrance\" href=\"SMM_AddEditDetailVerifyForm.aspx?id=" + idStr + "\" target=\"_blank\" style=\"color: blue; text-decoration:underline; cursor: pointer; display: none;\">点击填写表单</a>\n");
                            }
                        }
                        break;
                    }
                }
                if (exitFlag == 0)
                    sb.Append("        <input type=\"checkbox\" id=\"verifyform\" name=\"verifyform\" lay-filter=\"verifyform\" title=\"信息不存在\" disabled=\"disabled\" />\n");
                sb.Append("    </td>\n");
                sb.Append("</tr>\n");
                sb.Append("<tr>\n");
                sb.Append("    <td colspan=\"6\" style=\"text-align:center;\">\n");
                sb.Append("        <hr />\n");
                sb.Append("        <a href=\"javascript:;\" class=\"layui-btn layui-btn-lg\" style=\"margin-top:10px;\" onclick=\"SaveForFutherEdit('" + idStr + "')\">暂存</a>");
                sb.Append("        <button id=\"submit_action\" type=\"submit\" lay-submit=\"\" lay-filter=\"submit_action\" class=\"layui-btn layui-btn-lg\" style=\"margin-top:10px;\">提交</button>\n");
                sb.Append("        <button type=\"reset\" id=\"reset\" class=\"layui-btn layui-btn-lg\" style=\"margin-top:10px;\">重置</button>\n");
                sb.Append("    </td>\n");
                sb.Append("</tr>\n");
                sb.Append("</table>");
                htmlStr = sb.ToString();
            }
            catch (Exception msg)
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
            dt.Clear();
            dt.Dispose();
            dt = null;
            return htmlStr;
        }
    }
}