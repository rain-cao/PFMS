<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_CreateSteelMeshSN.aspx.cs" Inherits="PFMS_S.SMM.SMM_CreateSteelMeshSN" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网编号</title>
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
                sn: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网编号!";
                    }
                }
            });

            //Form提交事件，将厂区信息写入数据库
            form.on('submit(AddSN)', function (data) {
                //console.log(data.field);
                $.ajax({
                    url: 'SMM_AddSteelMeshSN.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            InitSteelMeshInfo(data.field["sn"]);
                        } else {
                            layer.alert('录入钢网编号失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('录入钢网编号失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });

            function InitSteelMeshInfo(SN) {
                $.ajax({
                    url: 'SMM_AddEditSteelMeshInfoSubmit.ashx',
                    data: {
                        'submit': 'NoSubmit',
                        'input_id': '0',
                        'sn': SN,
                        'pn': '',
                        'name': '',
                        'size': '',
                        'thickness': '',
                        'material': '',
                        'technology': '',
                        'ladder': '',
                        'imposition': '',
                        'treatment': '',
                        'printing': '',
                        'printcount': '',
                        'leaded': '',
                        'clean': '',
                        'manufacturer': '',
                        'custormer': '',
                        'verifydate': '',
                        'audit': '',
                        'intervalT': '',
                        'intervalC': '',
                        'storage': '',
                        'engineer': '',
                        'warehousing': '',
                        'verifyform': '0'
                    },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata.substr(0, 3) == 'OK:') {
                            var textarea = document.getElementById("textarea");
                            textarea.value = textarea.value + SN + ";";
                            $('#sn').prop('value', '');
                            layer.msg('钢网编号成功并已初始化钢网信息!');
                        } else {
                            layer.alert('钢网信息初始化失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('钢网信息初始化失败,请确认数据库连接!', { icon: 5 });
                    }
                });
            }

            /*$('#sn').bind('keydown', function (event) {
                if (event.keyCode == "13") {
                    $('#btn').click();
                }
            });*/
            form.render();
        })
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <div class="layui-inline" style="width:100%;margin:0 auto;text-align:center;">
            <table style="margin:10% auto;text-align:center;border:none;border-collapse:collapse;width:50%;">
                <tr><td colspan="2" style="text-align:center"><h2 style="color:#666666;">钢网编号</h2><br /></td></tr>
                <tr>
                    <td style="text-align:center;height:50px;">
                        编号：<input type="text" name="sn" id="sn" style="text-transform:uppercase;width: 200px;display:inline-block;" class="layui-input" value="" placeholder="" autocomplete="off" lay-verify="sn" required="required" />&nbsp;
                        <button type="submit" id="btn" lay-submit="" lay-filter="AddSN" class="layui-btn layui-btn-primary" style="margin-top: -5px;">录入系统</button>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center;">
                        <textarea id="textarea" placeholder="" class="layui-textarea" readonly="readonly" style="color: gray;background-color: lightgrey;"></textarea>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
