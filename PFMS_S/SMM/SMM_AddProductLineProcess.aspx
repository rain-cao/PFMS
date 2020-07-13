<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddProductLineProcess.aspx.cs" Inherits="PFMS_S.SMM.SMM_AddProductLineProcess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>流程添加</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        layui.use(['form','layer','form'], function () {
            var form = layui.form;
            var layer = layui.layer;
            var form = layui.form;

            form.verify({
                productline_id: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择线别!";
                    }
                },
                status_id: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择流程!";
                    }
                }
            });

            //勾选卡关提示规则时拼接存储字符串
            form.on('checkbox(role_1)', function (data) {
                GetDefineRole();
            });
            form.on('checkbox(role_2)', function (data) {
                GetDefineRole();
            });
            form.on('checkbox(role_3)', function (data) {
                GetDefineRole();
            });
            form.on('checkbox(role_4)', function (data) {
                GetDefineRole();
            });
            function GetDefineRole() {
                document.getElementById("role_define").value = "";
                if (document.getElementById("role_1").checked)
                    document.getElementById("role_define").value = document.getElementById("role_define").value + "1;";
                if (document.getElementById("role_2").checked)
                    document.getElementById("role_define").value = document.getElementById("role_define").value + "2;";
                if (document.getElementById("role_3").checked)
                    document.getElementById("role_define").value = document.getElementById("role_define").value + "3;";
                if (document.getElementById("role_4").checked)
                    document.getElementById("role_define").value = document.getElementById("role_define").value + "4;";
            }
            //选择强制卡关策略，转换为存储字符
            form.on('switch(force_select)', function (data) {
                if (data.elem.checked)
                    document.getElementById("role_force").value = "1";
                else
                    document.getElementById("role_force").value = "0";
            });
            //Form提交事件，将流程信息写入数据库
            form.on('submit(AddProcess)', function (data) {
                $.ajax({
                    url: 'SMM_AddProductLineProcessSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.msg('添加成功!');
                        } else {
                            layer.alert('添加失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('添加失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });
            form.render();
        })
    </script>
    <style>
        .layui-form-select dl {
            max-height: 180px;
        }
    </style>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <div class="layui-inline" style="width:100%;margin:0 auto;text-align:center;">
            <table style="margin:0 auto;text-align:center;border:none;border-collapse:collapse;width:90%">
                <tr><td colspan="2" style="text-align:center;"><h2>新增钢网使用线上流程</h2><br /></td></tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">线别：</td>
                    <td style="text-align:left;height:50px;">
                        <select runat="server" class="layui-select" name="productline_id" id="productline_id" size="5" lay-verify="productline_id" required="required">
                            <option value="">请选择...</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">流程：</td>
                    <td style="text-align:left;height:50px;">
                        <select runat="server" class="layui-select" name="status_id" id="status_id" size="4" lay-verify="status_id" required="required">
                            <option value="">请选择...</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">卡关提示规则：</td>
                    <td style="text-align:left;height:50px;">
                        <div class="layui-input-inline">
                            <input type="checkbox" id="role_1" lay-filter="role_1" value="1" lay-skin="primary" title="设备清洗时长" />
                            <input type="checkbox" id="role_2" lay-filter="role_2" value="2" lay-skin="primary" title="人工清洗时长" />
                        </div>
                        <div class="layui-input-inline">
                            <input type="checkbox" id="role_3" lay-filter="role_3" value="3" lay-skin="primary" title="钢网清洗间隔" />
                            <input type="checkbox" id="role_4" lay-filter="role_4" value="4" lay-skin="primary" title="钢网清洗次数" />
                        </div>
                        <input type="hidden" id="role_define" name="role_define" value="" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">强制卡关策略：</td>
                    <td style="text-align:left;height:50px;">
                        <input type="checkbox" lay-filter="force_select" lay-skin="switch" lay-text="强制|弹性" />
                        <input type="hidden" id="role_force" name="role_force" value="0" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center">
                        <hr />
                        <button type="submit" lay-submit="" lay-filter="AddProcess" class="layui-btn layui-btn-lg">添加</button>
                        <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
