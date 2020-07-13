<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddEditDetailStorage.aspx.cs" Inherits="PFMS_S.SMM_AddEditDetailStorage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>编辑详细储位</title>
    <link rel="icon" href="../Images/login_user.png" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <link rel="stylesheet" href="../CSS/StyleUserRegister.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        //Layui预加载
        layui.use(['form', 'layer'], function () {
            //操作对象
            var form = layui.form;
            var layer = layui.layer;
            //自定义验证信息
            form.verify({
                storage_name: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入储位名称（代码）!";
                    }
                },
                storage_id: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择所属大储位!";
                    }
                }
            });

            //Form提交事件，将储位信息写入数据库
            form.on('submit(AddEditDetailStorage)', function (data) {
                console.log(data.field);
                $.ajax({
                    url: 'SMM_AddEditDetailStorageSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.alert('详细储位编辑成功!', { icon: 6 });
                            if (data.field['input_id'] == '0') {
                                //$('#reset').click();
                                document.getElementById("storage_name").value = "";
                            }
                        } else {
                            layer.alert('详细储位编辑失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('详细储位编辑失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });
            form.render();
        });
    </script>
    <style>
        .layui-form-select dl {
            max-height: 120px;
        }
    </style>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <h2 runat="server" id="h2Show">详细储位编辑</h2>
        <br />
        <table>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width:50%;">所属大储位：</td>
                <td style="height: 50px;text-align: left;width:50%;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input type="hidden" name="input_id" id="input_id" runat="server" value="" />
                        <select runat="server" name="storage_id" id="storage_id" lay-verify="storage_id" required="required" >
                            <option value="">请选择...</option>
                        </select>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">详细储位名称(代码)：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">         
                        <input runat="server" name="storage_name" id="storage_name" type="text" placeholder="请输入详细储位名称(代码)" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" lay-verify="storage_name" required="required" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <hr />
                    <button runat="server" id="btnAddEdit" type="submit" lay-submit="" lay-filter="AddEditDetailStorage" class="layui-btn layui-btn-lg">添加</button>
                    <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
