﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddSize.aspx.cs" Inherits="PFMS_S.SMM_AddSize" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网尺寸添加</title>
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
                size: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网尺寸!";
                    }
                }
            });

            //Form提交事件，将厂区信息写入数据库
            form.on('submit(AddSize)', function (data) {
                console.log(data.field);
                $.ajax({
                    url: 'SMM_AddSizeSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.alert('添加成功!', { icon: 6 });
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
                <tr><td colspan="2" style="text-align:center"><h2>新增钢网尺寸</h2></td></tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">钢网尺寸：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="size" class="layui-input" value="" placeholder="请输入钢网尺寸" autocomplete="off" lay-verify="size" required="required" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center">
                        <hr />
                        <button type="submit" lay-submit="" lay-filter="AddSize" class="layui-btn layui-btn-lg">添加</button>
                        <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
