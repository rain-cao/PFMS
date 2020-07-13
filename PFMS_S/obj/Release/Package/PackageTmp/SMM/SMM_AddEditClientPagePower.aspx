<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddEditClientPagePower.aspx.cs" Inherits="PFMS_S.SMM.SMM_AddEditClientPagePower" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>编辑客户端页面权限</title>
    <link rel="icon" href="../Images/login_user.png" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <link rel="stylesheet" href="../CSS/StyleUserRegister.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        //Layui预加载
        layui.use(['form', 'layer', 'transfer'], function () {
            //操作对象
            var form = layui.form;
            var layer = layui.layer;
            var transfer = layui.transfer;

            form.on('submit(EditPage)', function (data) {
                //console.log(data.field);
                var BindPage = transfer.getData('bindPage');
                //var bind_PLineT = JSON.stringify(BindPLine);
                var bind_Page = "";
                for (var i in BindPage) {
                    bind_Page = bind_Page + BindPage[i].value + ";";
                }
                $.ajax({
                    url: 'SMM_AddEditClientPagePowerSubmit.ashx',
                    data: {
                        'user_id': document.getElementById("user_id").value,
                        'clintpage_id': bind_Page
                    },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.msg('客户端页面访问权限设置成功!');
                        } else {
                            layer.alert('客户端页面访问权限设置失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('客户端页面访问权限设置失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });

            //穿梭框
            var idStr = document.getElementById("user_id").value;
            $.ajax({
                url: 'SMM_GetClientPageInfoForUser.ashx',
                data: { 'user_id': idStr },
                dataType: 'json',
                type: 'POST',
                async: true,
                cache: false,
                success: function (data) {
                    if (data.code == 0) {
                        var dataL = data.data;
                        var dataR = new Array();
                        for (var i in data.value) {
                            dataR.push(data.value[i].clintpage_id);
                        }

                        transfer.render({
                            elem: '#PageContent'
                            , title: ['客户端页面', '可操作页面']  //自定义标题
                            , height: 200
                            , data: dataL
                            , value: dataR
                            , id: 'bindPage'
                            , onchange: function (obj, index) {
                                //var jsonObj = JSON.stringify(obj);
                            }
                        });
                    }
                    else if(data.code == 1){
                        layer.alert('获取客户端页面信息以及用户对页面的访问权限失败,Message: ' + data.msg, { icon: 5 });
                    }
                },
                error: function () {
                    layer.alert('获取客户端页面信息以及用户对页面的访问权限失败,请检查数据库连接!', { icon: 5 });
                }
            });
            form.render();
        });
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <h2 runat="server" id="h2Show">用户对客户端页面权限管理</h2>
        <br />
        <table style="width: 98%">
            <tr>
                <td style="text-align: center;">
                    <div style="width:100%;" class="layui-inline" text-align: left;">
                        <input runat="server" type="hidden" id="user_id" name="user_id" value="" />
                        <div id="PageContent" class="demo-transfer"></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align:center;">
                    <hr />
                    <button runat="server" id="btnAddEdit" type="submit" lay-submit="" lay-filter="EditPage" class="layui-btn layui-btn-lg">编辑</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
