<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddEditManufacturer.aspx.cs" Inherits="PFMS_S.SMM_AddEditManufacturer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>编辑厂商</title>
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
                name: function (value) {
                    var item = document.getElementById("wr");
                    var item1 = document.getElementById("ri");
                    if ($.trim(value).length < 1) {
                        item.style.display = "block";
                        item1.style.display = "none";
                        return "请输入厂商名称!";
                    }
                    else {
                        item.style.display = "none";
                        item1.style.display = "block";
                        return;
                    }
                }
            });

            //Form提交事件，将注册信息写入数据库
            form.on('submit(AddEditManufacturer)', function (data) {
                console.log(data.field);
                $.ajax({
                    url: 'SMM_AddEditManufacturerSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.alert('厂商编辑成功!', { icon: 6 });
                            if (data.field['input_id'] == '0')
                                $('#reset').click();
                        } else {
                            layer.alert('厂商编辑失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('厂商编辑失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });
            form.render();
        });

        //点击reset按钮时将所有√和×影藏
        function resetClick() {
            var item = document.getElementById("ri");
            item.style.display = "none";
            item = document.getElementById("wr");
            item.style.display = "none";
        }
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <h2 runat="server" id="h2Show">厂商编辑</h2>
        <br />
        <table>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width: 30%;">厂商名称：</td>
                <td style="height: 50px;width: 70%; text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input type="hidden" name="input_id" id="input_id" runat="server" value="" />
                        <input runat="server" name="name" id="name" type="text" placeholder="请输入厂商名称" autocomplete="off" class="layui-input" style="width: 100%;" lay-verify="name" required="required" />
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="ri" style="color: green;font-weight: bolder;display: none;">V</i>
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="wr" style="color: red; font-weight: bolder; display: none;" >X</i>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">厂商代码：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input runat="server" name="code" id="code" type="text" placeholder="请输入厂商代码" autocomplete="off" class="layui-input" style="width: 100%;text-transform: uppercase;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">厂商负责人：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input runat="server" name="responsible" id="responsible" type="text" placeholder="请输入厂商负责人" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">联系方式：</td>
                <td style="height: 50px;text-align: left;">              
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input runat="server" name="telephone" id="telephone" type="text" placeholder="厂商负责人的联系电话" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">邮箱地址：</td>
                <td style="height: 50px;text-align: left;">              
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input runat="server" name="email" id="email" type="text" placeholder="厂商负责人的邮箱" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">地址：</td>
                <td style="height: 50px;text-align: left;">              
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input runat="server" name="address" id="address" type="text" placeholder="厂商的地址" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">备注：</td>
                <td style="height: 50px;text-align: left;">              
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input runat="server" name="remark" id="remark" type="text" placeholder="备注信息" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <hr />
                    <button runat="server" id="btnAddEdit" type="submit" lay-submit="" lay-filter="AddEditManufacturer" class="layui-btn layui-btn-lg">添加</button>
                    <button type="reset" id="reset" class="layui-btn layui-btn-lg" onclick="resetClick();">重置</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
