<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_AddMainMenu.aspx.cs" Inherits="PFMS_S.BM.BM_AddMainMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>新增主目录</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" type="text/css" href="../layui/css/layui.css" />
    <link rel="stylesheet" type="text/css" href="../CSS/StyleDefault.css" />
    <script src="../js/jquery-3.4.1.js" charset="utf-8"></script>
    <script src="../js/jquery-3.4.1.min.js" charset="utf-8"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        layui.use(['form','layer'], function () {
            var form = layui.form;
            var layer = layui.layer;

            form.verify({
                menu_name: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入目录或菜单在页面上显示的名称";
                    }
                },
                menu_type: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择用户对该目录/菜单的初始权限";
                    }
                }
            });

            form.on('submit(AddMainMenu)', function (data) {
                console.log(data.field);
                $.ajax({
                    url: 'BM_AddNextMenuSubmit.ashx',
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
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <div class="layui-inline" style="width:100%;margin:0 auto;text-align:center;">
            <table style="margin:0 auto;text-align:center;border:none;border-collapse:collapse;width:95%">
                <tr><td colspan="2" style="text-align:center"><h2>新增主目录</h2></td></tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">显示名称：</td>
                    <td style="text-align:left;height:50px;">
                        <input type="text" name="menu_name" class="layui-input" value="" autocomplete="off" lay-verify="menu_name" required="required" />
                        <input type="hidden" name="path" class="layui-input" value="" autocomplete="off" />
                        <input type="hidden" name="menu_type" class="layui-input" value="catalogue" autocomplete="off" />
                        <input type="hidden" name="menu_url" class="layui-input" value="" autocomplete="off" />
                        <input type="hidden" name="thisMenuID" value="0" />
                    </td>
                </tr>  
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">图标：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="icon" class="layui-input" value="" autocomplete="off" /></td>
                </tr> 
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">备注信息：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="remark" class="layui-input" value="" autocomplete="off" /></td>
                </tr> 
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">用户初始权限：</td>
                    <td style="text-align:left;height:50px;">
                        <select name="init_power" class="layui-select" lay-verify="menu_type" required="required">
                            <option value="0">不可显</option>
                            <option value="1">只读</option>
                            <option value="2">读写</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center">
                        <hr />
                        <button type="submit" lay-submit="" lay-filter="AddMainMenu" class="layui-btn layui-btn-lg">添加</button>
                        <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
