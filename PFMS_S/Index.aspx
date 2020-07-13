<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="PFMS_S.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>用户登录</title>
    <link rel="icon" href="Images/iconTitle.ico" />
    <link rel="stylesheet" href="CSS/StyleIndex.css" />
    <link rel="stylesheet" href="layui/css/layui.css" />
    <script src="layui/layui.js"></script>    
    <script src="js/jquery-3.4.1.js"></script>
    <script src="js/jquery-3.4.1.min.js"></script>
    <script type="text/javascript">
        ////页面加载时设置侧边菜单栏高度
        window.onload = function () {
            resizeForm();
        };

        //页面大小改变时设置指定模块高度、宽度属性
        function resizeForm() {
            changecss('.div_leftStyle', 'height', ($(window).height()).toString() + "px");          //页面大小改变时，设置左侧内容页DIV高度适应文本显示区域高度
            changecss('.div_rightStyle', 'height', ($(window).height()).toString() + "px");         //页面大小改变时，设置右侧内容页DIV高度适应文本显示区域高度
        }

        //设置外联CSS属性函数
        function changecss(theClass, element, value) {
            var cssRules;
            if (document.all) {
                cssRules = 'rules';
            }
            else if (document.getElementById) {
                cssRules = 'cssRules';
            }
            for (var S = 0; S < document.styleSheets.length; S++) {
                for (var R = 0; R < document.styleSheets[S][cssRules].length; R++) {
                    if (document.styleSheets[S][cssRules][R].selectorText == theClass) {
                        document.styleSheets[S][cssRules][R].style[element] = value;
                        return;
                    }
                }
            }
        }

        //layui预加载
        layui.use(['form', 'layer'], function () {

            // 操作对象
            var form = layui.form;
            var layer = layui.layer;

            //自定义输入框验证
            form.verify({
                loginName: function(value) {
                    if ($.trim(value).length < 1) { return '用户登录账号不得为空或空格!'; }
                },
                loginPwd: function (value) {
                    if ($.trim(value).length < 1) { return '用户登录密码不得为空或空格!'; }
                }
            });

            //用户登录
            form.on('submit(login)', function (data) {
                var item = document.getElementById("remanberme");
                var index;
                $.ajax({
                    url: 'UserLogin.ashx',
                    data: {
                        'user': data.field['account'], 
                        'password': data.field['password'],
                        'checked': item.checked
                    },  
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    beforeSend: function () {
                        //弹出的lodinng层
                        index = layer.load(2, { shade: [0.5, "#333"] });
                    },
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            location.href = "Default.aspx";
                        } else {
                            layer.alert('登陆失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert("请检查数据库连接!", { icon: 2 });
                    },
                    complete: function () {
                        layer.close(index);
                    }
                });
                return false;
            });

            //弹出层，弹出用户注册页面
            $('#userReg').on('click', function () {
                layer.open({
                    type: 2,
                    title: '用户注册',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['600px', '700px'],
                    fixed: false, //不固定
                    maxmin: true,
                    content: 'UserRegister.aspx'
                });
            });

            //忘记密码，将密码发送至用户邮箱
            $('#getPwd').on('click', function () {
                var Item = document.getElementById('account');
                if ($.trim(Item.value) == "")
                    layer.msg("请输入用户登录账号(也不能为空格)!", {icon: 4});
                else {
                    $.ajax({
                        url: 'UserPasswordGet.ashx',
                        data: { "userAccount": $.trim(Item.value)},
                        dataType: 'text',
                        type: 'POST',
                        async: true,
                        cache: false,
                        success: function (data) {
                            if (data != "") {
                                layer.alert("密码已发送至邮箱" + data, { icon: 6 });
                            } else {
                                layer.alert('密码查找失败,Message：账户不存在或邮件发送失败!', { icon: 5 });
                            }
                        },
                        error: function () {
                            layer.alert("密码查找失败,请检查数据库连接!", { icon: 2 });
                        }
                    });
                }
            });
        });
    </script>
</head>
<body onresize="resizeForm();">
        <div class="div_leftStyle">
            <img src="Images/bg-sidebox.jpg" class="bg_img" />
        </div>
        <div class="div_rightStyle">
            <form class="layui-form">
                <table>
                    <tr>
                        <td colspan="2" style="height: 80px;text-align:center;"><h1>用户登录</h1></td>
                    </tr>
                    <tr>
                        <td style="text-align: right;height: 50px;font-size: large;width: 35%;">用户账号 <i class="layui-icon">&#xe66f;</i>：</td>
                        <td style="height: 50px;width: 65%;">
                            <input runat="server" name="account" id="account" type="text" placeholder="请输入用户名" autocomplete="off" class="layui-input" style="width: 100%;" lay-verify="loginName" required="required" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;height: 50px;font-size: large;">登录密码 <i class="layui-icon">&#xe672;</i>：</td>
                        <td style="height: 50px;">
                            <input runat="server" type="password" name="password" id="password" placeholder="密码" autocomplete="off" class="layui-input" style="width:100%;" lay-verify="loginPwd" required="required"  />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="height:30px; text-align:right;">
                           <input type="checkbox" id="remanberme" title="记住我" lay-skin="primary" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="height: 80px;">
                            <button type="submit" class="layui-btn layui-btn-lg" style="width:100%;text-align:center;" lay-submit="" lay-filter="login">登录</button>
                            <!--hr />
                            <p><a id="userReg" href="javascript:;" style="float:left;">立即注册</a><a id="getPwd" href="javascript:;" style="float:right;">忘记密码？</a></p-->
                        </td>
                    </tr>
                </table>
            </form>
        </div>
</body>
</html>
