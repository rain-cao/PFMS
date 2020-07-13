<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_LendAndReturn.aspx.cs" Inherits="PFMS_S.SMM.SMM_LendAndReturn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网借出归还</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <link rel="stylesheet" href="../CSS/StyleUserRegister.css" />
    <script src="../layui/layui.js"></script>
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script>
        layui.use(['layer', 'form'], function () {
            var layer = layui.layer;
            var form = layui.form;

            form.on('select(Lend_return)', function (data) {
                if (data.value == 2)
                    document.getElementById("btnOprate").innerText = "借出";
                else if (data.value == 3)
                    document.getElementById("btnOprate").innerText = "归还";
                else
                    document.getElementById("btnOprate").innerText = "操作";
            });

            form.verify({
                Lend_return: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择借出或归还!";
                    }
                },
                sn: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网编号!";
                    }
                },
                opid: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入借用或归还人员工号!";
                    }
                }
            });

            form.on('submit(btnOprate)', function (data) {
                $.ajax({
                    url: 'SMM_LendAndReturnSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            if (data.field.Lend_return == '2') {
                                layer.msg(data.field.sn + '借出成功!');
                                document.getElementById("showResult").innerHTML = "<span style=\"color: green;\">" + data.field.sn + "借出成功!<span><br />" + document.getElementById("showResult").innerHTML;
                            }
                            else if (data.field.Lend_return == '3') {
                                layer.msg(data.field.sn + '归还成功!');
                                document.getElementById("showResult").innerHTML = "<span style=\"color: green;\">" + data.field.sn + "归还成功!<span><br />" + document.getElementById("showResult").innerHTML;
                            }
                        } else {
                            if (data.field.Lend_return == '2') {
                                layer.alert(data.field.sn + '借出失败,Message：' + returndata, { icon: 5 });
                                document.getElementById("showResult").innerHTML = "<span style=\"color: red;\">" + data.field.sn + "借出失败,Message:" + returndata + "<span><br />" + document.getElementById("showResult").innerHTML;
                            }
                            else if (data.field.Lend_return == '3') {
                                layer.alert(data.field.sn + '归还失败,Message：' + returndata, { icon: 5 });
                                document.getElementById("showResult").innerHTML = "<span style=\"color: red;\">" + data.field.sn + "归还失败,Message:" + returndata + "<span><br />" + document.getElementById("showResult").innerHTML;
                            }
                        }
                    },
                    error: function () {
                        if (data.field.Lend_return == '2') {
                            layer.alert(data.field.sn + '借出失败,请确认数据库连接!', { icon: 5 });
                            document.getElementById("showResult").innerHTML = "<span style=\"color: red;\">" + data.field.sn + "借出失败,Message:请确认数据库连接!<span><br />" + document.getElementById("showResult").innerHTML;
                        }
                        else if (data.field.Lend_return == '3') {
                            layer.alert(data.field.sn + '归还失败,请确认数据库连接!', { icon: 5 });
                            document.getElementById("showResult").innerHTML = "<span style=\"color: red;\">" + data.field.sn + "归还失败,Message:请确认数据库连接!<span><br />" + document.getElementById("showResult").innerHTML;
                        }
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
        <div style="width:100%;text-align:center">
            <h2 style="color:#666666;margin-top:10px;">钢网借出归还管理</h2>
        </div>
        <div style="width:650px;margin:0 auto;text-align:center;">
            <br />
            <table>
                <tr>
                    <td style="text-align: right;height: 50px;font-size: large;width:250px;">借出归还：</td>
                    <td style="height: 50px;text-align: left;width:400px;">
                        <div class="layui-inline" style="text-align: left;width:100%;">
                            <select class="layui-select" name="Lend_return" id="Lend_return" lay-filter="Lend_return" lay-verify="Lend_return" required="required" >
                                <option value="">请选择...</option>
                                <option value="2">借出</option>
                                <option value="3">归还</option>
                            </select>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;height: 50px;font-size: large;">钢网编号：</td>
                    <td style="height: 50px;text-align: left;">
                        <div class="layui-inline" style="text-align: left;width:100%;">         
                            <input name="sn" id="sn" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" lay-verify="sn" required="required" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;height: 50px;font-size: large;">PCB料号：</td>
                    <td style="height: 50px;text-align: left;">
                        <div class="layui-inline" style="text-align: left;width:100%;">         
                            <input name="pcb_pn" id="pcb_pn" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;height: 50px;font-size: large;">线上工单：</td>
                    <td style="height: 50px;text-align: left;">
                        <div class="layui-inline" style="text-align: left;width:100%;">         
                            <input name="wo" id="wo" type="text" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;height: 50px;font-size: large;">工单量：</td>
                    <td style="height: 50px;text-align: left;">
                        <div class="layui-inline" style="text-align: left;width:100%;">         
                            <input name="wo_count" id="wo_count" type="text" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;height: 50px;font-size: large;width:100px;">应用线体：</td>
                    <td style="height: 50px;text-align: left;width:300px;">
                        <div class="layui-inline" style="text-align: left;width:100%;">
                            <select runat="server" name="productLine" id="productLine" >
                                <option value="">请选择...</option>
                            </select>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;height: 50px;font-size: large;">借用/归还人员工号：</td>
                    <td style="height: 50px;text-align: left;">
                        <div class="layui-inline" style="text-align: left;width:100%;">         
                            <input name="opid" id="opid" type="text" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" lay-verify="opid" required="required" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: left;">
                        <div id="showResult" style="width:100%;text-align:left;margin:0 auto;max-height: 100px;overflow: auto;"></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;">
                        <hr />
                        <button runat="server" id="btnOprate" type="submit" lay-submit="" lay-filter="btnOprate" class="layui-btn layui-btn-lg">操作</button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
