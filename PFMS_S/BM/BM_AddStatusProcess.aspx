<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_AddStatusProcess.aspx.cs" Inherits="PFMS_S.BM.BM_AddStatusProcess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>职务添加</title>
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
                status_id: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入流程状态id号!";
                    }
                    else if ($.trim(value) == "0" || $.trim(value) == "-1"
                        || $.trim(value) == "1" || $.trim(value) == "2"
                        || $.trim(value) == "3" || $.trim(value) == "4"
                        || $.trim(value) == "5" || $.trim(value) == "6"
                        || $.trim(value) == "7" || $.trim(value) == "8"
                        || $.trim(value) == "9" || $.trim(value) == "10") {
                        return "流程状态id号-1~10为保留项, 不可用!";
                    }
                },
                status_name: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入流程状态名称!";
                    }
                }
            });

            //Form提交事件，将厂区信息写入数据库
            form.on('submit(AddSP)', function (data) {
                console.log(data.field);
                $.ajax({
                    url: 'BM_AddStatusProcessSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.msg('添加成功!');
                            $("#reset").click();
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
                <tr><td colspan="2" style="text-align:center"><h2>新增流程状态</h2></td></tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">流程状态ID号：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="status_id" class="layui-input" value="" style="text-transform:uppercase;" placeholder="请勿填写-1~10" autocomplete="off" lay-verify="status_id" required="required" /></td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">流程状态名称：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="status_name" class="layui-input" value="" style="text-transform:uppercase;" autocomplete="off" lay-verify="status_name" required="required" /></td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">流程状态描述：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="status_desc" class="layui-input" value="" autocomplete="off" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center">
                        <hr />
                        <button type="submit" lay-submit="" lay-filter="AddSP" class="layui-btn layui-btn-lg">添加</button>
                        <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
