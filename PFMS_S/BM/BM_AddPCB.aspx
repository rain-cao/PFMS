<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_AddPCB.aspx.cs" Inherits="PFMS_S.BM.BM_AddPCB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>PCB添加</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        layui.use(['form','layer'], function () {
            var form = layui.form;
            var layer = layui.layer;

            form.verify({
                pcb_pn: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入PCB料号!";
                    }
                }
            });

            //Form提交事件，将PCB料号写入数据库
            form.on('submit(AddPCB)', function (data) {
                console.log(data.field);
                $.ajax({
                    url: 'BM_AddPCBSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.msg('添加成功!');
                            $('#reset').click();
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
                <tr><td colspan="2" style="text-align:center"><h2>新增PCB料号</h2></td></tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">PCB料号：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="pcb_pn" class="layui-input" value="" style="text-transform:uppercase;" placeholder="" autocomplete="off" lay-verify="pcb_pn" required="required" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center">
                        <hr />
                        <button type="submit" lay-submit="" lay-filter="AddPCB" class="layui-btn layui-btn-lg">添加</button>
                        <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
