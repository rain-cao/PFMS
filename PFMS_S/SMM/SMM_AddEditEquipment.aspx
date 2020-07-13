<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddEditEquipment.aspx.cs" Inherits="PFMS_S.SMM.SMM_AddEditEquipment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>编辑清洗设备</title>
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
            //自定义验证信息
            form.verify({
                Equipment_name: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请填写清洗设备名称!";
                    }
                },
                clean_length: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请填写清洗时长!";
                    }
                }
            });

            //Form提交事件，将设备信息写入数据库
            form.on('submit(AddEditEquipment)', function (data) {
                //console.log(data.field);
                var BindPLine = transfer.getData('bindPLine');
                //var bind_PLineT = JSON.stringify(BindPLine);
                var bind_PLine = "";
                for (var i in BindPLine) {
                    bind_PLine = bind_PLine + BindPLine[i].value + ";";
                }
                $.ajax({
                    url: 'SMM_AddEditEquipmentSubmit.ashx',
                    data: {
                        'id': document.getElementById("input_id").value,
                        'Equipment_name': document.getElementById("Equipment_name").value,
                        'ip_address': document.getElementById("ip_address").value,
                        'clean_length': document.getElementById("clean_length").value,
                        'bind_PLine': bind_PLine
                    },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.msg('编辑清洗设备成功!');
                        } else {
                            layer.alert('编辑清洗设备失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('编辑清洗设备失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });

            //穿梭框
            var idStr = document.getElementById("input_id").value;
            $.ajax({
                url: 'SMM_IniProductLineBindEquipment.ashx',
                data: { 'id': idStr },
                dataType: 'json',
                type: 'POST',
                async: true,
                cache: false,
                success: function (data) {
                    if (data.code == 0) {
                        var dataL = data.data;
                        var dataR = new Array();
                        for (var i in data.value) {
                            dataR.push(data.value[i].productline_id);
                        }

                        transfer.render({
                            elem: '#PLine'
                            , title: ['线体清单', '已绑线体']  //自定义标题
                            , height: 300
                            , data: dataL
                            , value: dataR
                            , id: 'bindPLine'
                            , onchange: function (obj, index) {
                                //var jsonObj = JSON.stringify(obj);
                            }
                        });
                    }
                    else if(data.code == 1){
                        layer.alert('获取线体列表失败或查询绑定清洗设备线体失败,Message: ' + data.msg, { icon: 5 });
                    }
                },
                error: function () {
                    layer.alert('获取线体列表失败或查询绑定清洗设备线体失败,请检查数据库连接!', { icon: 5 });
                }
            });
            form.render();
        });
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <h2 runat="server" id="h2Show">编辑清洗设备</h2>
        <br />
        <table style="width: 95%">
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width:150px;">设备名称：</td>
                <td style="height: 50px;text-align: left;">
                    <div style="width:100%;" class="layui-inline" text-align: left;">
                        <input type="hidden" name="input_id" id="input_id" runat="server" value="" />
                        <input runat="server" name="Equipment_name" id="Equipment_name" type="text" value="" placeholder="非设备请填写'人工清洗'" autocomplete="off" class="layui-input" style="width: 98%;" lay-verify="Equipment_name" required="required" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width:100px;">设备IP地址：</td>
                <td style="height: 50px;text-align: left;">
                    <div style="width:100%;" class="layui-inline" text-align: left;">
                        <input runat="server" name="ip_address" id="ip_address" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 98%;"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width:100px;">清洗时长：</td>
                <td style="height: 50px;text-align: left;">
                    <div style="width:100%;" class="layui-inline" text-align: left;">
                        <div class="layui-inline" style="float:left;">
                            <input runat="server" name="clean_length" id="clean_length" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100px;" lay-verify="clean_length" required="required"/>
                         </div>
                        <div class="layui-inline" style="margin-top:20px;">&nbsp;<span>min</span></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width:100px;">线体绑定：</td>
                <td style="height: 50px;text-align: left;">
                    <div style="width:100%;" class="layui-inline" text-align: left;">
                        <div id="PLine" class="demo-transfer"></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <hr />
                    <button runat="server" id="btnAddEdit" type="submit" lay-submit="" lay-filter="AddEditEquipment" class="layui-btn layui-btn-lg">添加</button>
                    <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
