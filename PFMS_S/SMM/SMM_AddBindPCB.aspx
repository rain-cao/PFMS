<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddBindPCB.aspx.cs" Inherits="PFMS_S.SMM.SMM_AddBindPCB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>绑定钢网与PCB关系</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script>
        layui.use(['layer', 'form'], function () {
            var layer = layui.layer;
            var form = layui.form;

            form.verify({
                smlist_id: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择钢网编号!";
                    }
                }
            });

            //Form提交事件，将钢网绑定PCB料号关系写入数据库
            form.on('submit(AddBindPCB)', function (data) {
                console.log(data.field);
                $.ajax({
                    url: 'SMM_AddBindPCBSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                            //window.parent.location.reload();   //刷新父界面
                            parent.layer.close(index);    //关闭弹出层
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
        });
    </script>
    <style>
        .layui-form-select dl {
            max-height: 115px;
        }
    </style>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <div class="layui-inline" style="width:100%;margin:0 auto;text-align:center;">
            <table style="margin:0 auto;text-align:center;border:none;border-collapse:collapse;width:80%">
                <tr><td colspan="2" style="text-align:center"><h2>选择钢网</h2></td></tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">钢网编号：</td>
                    <td style="text-align:left;height:50px;">
                        <input type="hidden" name="input_id" id="input_id" runat="server" value="" />
                        <select runat="server" class="layui-select" name="smlist_id" id="smlist_id" lay-verify="smlist_id" required="required" lay-search="">
                           <option value=""></option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center">
                        <hr />
                        <br />
                        <button type="submit" lay-submit="" lay-filter="AddBindPCB" class="layui-btn layui-btn-lg">添加</button>
                        <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
