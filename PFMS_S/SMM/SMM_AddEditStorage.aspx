<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddEditStorage.aspx.cs" Inherits="PFMS_S.SMM_AddEditStorage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>编辑储位</title>
    <link rel="icon" href="../Images/login_user.png" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <link rel="stylesheet" href="../CSS/StyleUserRegister.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        //Layui预加载
        layui.use(['form', 'layer','form'], function () {
            //操作对象
            var form = layui.form;
            var layer = layui.layer;
            var form = layui.form;
            //自定义验证信息
            form.verify({
                storage_name: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入储位名称!";
                    }
                }
            });

            //Form提交事件，将储位信息写入数据库
            form.on('submit(AddEditStorage)', function (data) {
                console.log(data.field);
                $.ajax({
                    url: 'SMM_AddEditStorageSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.alert('储位编辑成功!', { icon: 6 });
                            if (data.field['input_id'] == '0')
                                $('#reset').click();
                        } else {
                            layer.alert('储位编辑失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('储位编辑失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });
            form.render();
        });
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <h2 runat="server" id="h2Show">储位编辑</h2>
        <br />
        <table>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width: 30%;">储位名称：</td>
                <td style="height: 50px;width: 70%; text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input type="hidden" name="input_id" id="input_id" runat="server" value="" />
                        <input runat="server" name="storage_name" id="storage_name" type="text" placeholder="请输入储位名称" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" lay-verify="storage_name" required="required" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">位置：</td>
                <td style="height: 50px;text-align: left;">              
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input runat="server" name="address" id="address" type="text" placeholder="请输入储位位置" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <hr />
                    <button runat="server" id="btnAddEdit" type="submit" lay-submit="" lay-filter="AddEditStorage" class="layui-btn layui-btn-lg">添加</button>
                    <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
