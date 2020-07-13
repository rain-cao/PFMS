<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserRegister.aspx.cs" Inherits="PFMS_S.UserRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>用户注册</title>
    <link rel="icon" href="Images/login_user.png" />
    <link rel="stylesheet" href="layui/css/layui.css" />
    <link rel="stylesheet" href="CSS/StyleUserRegister.css" />
    <script src="js/jquery-3.4.1.js"></script>
    <script src="js/jquery-3.4.1.min.js"></script>
    <script src="layui/layui.js"></script>
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
                        return "请输入中文姓名!";
                    }
                    else {
                        item.style.display = "none";
                        item1.style.display = "block";
                        return;
                    }
                },
                username: function (value) {
                    var item = document.getElementById("uwr");
                    var item1 = document.getElementById("uri");
                    if ($.trim(value).length < 1) {
                        item.style.display = "block";
                        item1.style.display = "none";
                        return "请输入用户名!";
                    }
                    else {
                        item.style.display = "none";
                        item1.style.display = "block";
                        return;
                    }
                },
                opid: function (value) {
                    var item = document.getElementById("owr");
                    var item1 = document.getElementById("ori");
                    if ($.trim(value).length < 1) {
                        item.style.display = "block";
                        item1.style.display = "none";
                        return "请输入工号!";
                    }
                    else {
                        item.style.display = "none";
                        item1.style.display = "block";
                        return;
                    }
                },
                password: function (value) {
                    var item = document.getElementById("pwr");
                    var item1 = document.getElementById("pri");
                    if ($.trim(value).length < 1) {
                        item.style.display = "block";
                        item1.style.display = "none";
                        return "请输入密码!";
                    }
                    else {
                        item.style.display = "none";
                        item1.style.display = "block";
                        return;
                    }
                },
                email: function (value) {
                    var item = document.getElementById("ewr");
                    var item1 = document.getElementById("eri");
                    var reg = new RegExp("^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$"); //正则表达式
                    if ($.trim(value).length < 1) {
                        item.style.display = "block";
                        item1.style.display = "none";
                        return "邮箱地址不得为空!";
                    }
                    else if (!reg.test($.trim(value))){
                        item.style.display = "block";
                        item1.style.display = "none";
                        return "请输入正确的邮箱地址!";
                    }
                    else {
                        item.style.display = "none";
                        item1.style.display = "block";
                        return;
                    }
                }
            });

            //Form提交事件，将注册信息写入数据库
            form.on('submit(Register)', function (data) {
                var item = document.getElementById("rpwr");
                var item1 = document.getElementById("rpri");
                var item3 = document.getElementById("rpassword");
                var item4 = document.getElementById("password");
                if (item3.value.trim() != item4.value.trim()) {
                    layer.msg('两次输入的密码不一致!', { icon: 2 });
                    item.style.display = "block";
                    item1.style.display = "none";
                }
                else {
                    item.style.display = "none";
                    item1.style.display = "block";
                }
                console.log(data.field);
                $.ajax({
                    url: 'UserRegisterSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.alert('恭喜您注册成功!', { icon: 6 });
                            $('#reset').click();
                        } else {
                            layer.alert('注册失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('注册失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });
        });

        //点击reset按钮时将所有√和×影藏
        function resetClick() {
            var item = document.getElementById("ri");
            item.style.display = "none";
            item = document.getElementById("wr");
            item.style.display = "none";

            item = document.getElementById("uri");
            item.style.display = "none";
            item = document.getElementById("uwr");
            item.style.display = "none";

            item = document.getElementById("ori");
            item.style.display = "none";
            item = document.getElementById("owr");
            item.style.display = "none";

            item = document.getElementById("pri");
            item.style.display = "none";
            item = document.getElementById("pwr");
            item.style.display = "none";

            item = document.getElementById("rpri");
            item.style.display = "none";
            item = document.getElementById("rpwr");
            item.style.display = "none";

            item = document.getElementById("eri");
            item.style.display = "none";
            item = document.getElementById("ewr");
            item.style.display = "none";
        }
    </script>
    <style>
        .layui-form-select dl {
            max-height: 250px;
        }
    </style>
</head>
<body>
    <form class="layui-form">
        <h2>用户注册</h2>
        <br />
        <table>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width: 30%;">姓名：</td>
                <td style="height: 50px;width: 70%; text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input name="name" id="name" type="text" placeholder="中文姓名" autocomplete="off" class="layui-input" style="width: 100%;" lay-verify="name" required="required" />
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="ri" style="color: green;font-weight: bolder;display: none;">V</i>
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="wr" style="color: red; font-weight: bolder; display: none;">X</i>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">用户名：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input name="username" id="username" type="text" placeholder="用户名/用户账号" autocomplete="off" class="layui-input" style="width: 100%;" lay-verify="username" required="required" />
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="uri" style="color: green;font-weight: bolder;display: none;">V</i>
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="uwr" style="color: red; font-weight: bolder; display: none;">X</i>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">工号：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input name="opid" id="opid" type="text" placeholder="请输入工号" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" lay-verify="opid" required="required" />
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="ori" style="color: green;font-weight: bolder;display: none;">V</i>
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="owr" style="color: red; font-weight: bolder; display: none;">X</i>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">密码：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input name="password" id="password" type="text" placeholder="请输入登录密码" autocomplete="off" class="layui-input" style="width: 100%;" lay-verify="password" required="required" />
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="pri" style="color: green;font-weight: bolder;display: none;">V</i>
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="pwr" style="color: red; font-weight: bolder; display: none;">X</i>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">密码确认：</td>
                <td style="height: 50px;text-align: left;">              
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input name="rpassword" id="rpassword" type="text" placeholder="再次输入登录密码" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="rpri" style="color: green;font-weight: bolder;display: none;">V</i>
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="rpwr" style="color: red; font-weight: bolder; display: none;">X</i>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">所属部门：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <select runat="server" name="dept" id="dept" class="layui-select" >
                            <option value="">部门选择...</option>
                        </select>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">职务：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <select runat="server" name="post" id="post" class="layui-select">
                            <option value="">职务选择...</option>
                        </select>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">手机号码：</td>
                <td style="height: 50px;text-align: left;">              
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input name="telephone" id="telephone" type="text" placeholder="用户手机号码" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">邮箱地址：</td>
                <td style="height: 50px;text-align: left;">              
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input name="email" id="email" type="text" placeholder="someone@Example.com.cn" autocomplete="off" class="layui-input" style="width: 100%;" lay-verify="email" required="required" />
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="eri" style="color: green;font-weight: bolder;display: none;">V</i>
                    </div>
                    <div class="layui-inline">
                        <i class="layui-icon" id="ewr" style="color: red; font-weight: bolder; display: none;" >×</i>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">微信号：</td>
                <td style="height: 50px;text-align: left;">              
                    <div class="layui-inline" style="width: 80%; text-align: left;">
                        <input name="weixin_no" id="weixin_no" type="text" placeholder="用户微信号" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <hr />
                    <button type="submit" lay-submit="" lay-filter="Register" class="layui-btn layui-btn-lg">注册</button>
                    <button type="reset" id="reset" class="layui-btn layui-btn-lg" onclick="resetClick();">重置</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
