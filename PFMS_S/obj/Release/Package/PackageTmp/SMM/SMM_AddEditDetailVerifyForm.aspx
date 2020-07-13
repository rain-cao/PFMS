<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddEditDetailVerifyForm.aspx.cs" Inherits="PFMS_S.SMM.SMM_AddEditDetailVerifyForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网验收表单管理</title>
    <link rel="icon" href="../Images/iconTitle.ico"/>
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
<script>
    layui.use(['form', 'layer'], function () {
        var form = layui.form;
        var layer = layui.layer;
        //输入校验
        form.verify({
            verify_select: function (value) {
                if ($.trim(value).length < 1) {
                    return "选择项不得为空";
                }
            },
            verify_input: function (value) {
                if ($.trim(value).length < 1) {
                    return "请输入确认信息";
                }
            }
        });
        //CheckBox点击时改变勾选状态值
        form.on('checkbox(select_verify_info)', function (data) {
            var thisName = data.elem.name;
            var inputID = 'verify_content_' + thisName.substr(thisName.lastIndexOf("_") + 1);
            if (data.elem.checked) {
                document.getElementById(inputID).value = '1';
            }
            else {
                document.getElementById(inputID).value = '0';
            }
        });
        //提交按钮事件执行过程
        form.on('submit(submit_action)', function (data) {
            //1.首先判断验收表单信息是否有需要校对的(仅针对Input)
            var elemCount = parseInt(data.field['record_count']);
            var passFlag = 1;
            for (var i = 1; i <= elemCount; i++) {
                if (document.getElementById('verify_type_' + i.toString()).value == "1") {
                    if (document.getElementById('required_' + i.toString()).value == "1") {
                        var chkType = "0";
                        if ((chkType = document.getElementById('check_type_' + i.toString()).value) != "0") {
                            var standardValue = document.getElementById('standard_value_' + i.toString());
                            var verifyContent = document.getElementById('verify_content_' + i.toString());
                            if (chkType == "1") {
                                //数值校对
                                var maxV = 0, minV = 0;
                                if (patch(';', standardValue.value) != 2) {
                                    verifyContent.style.backgroundColor = "red";
                                    layer.alert("标准值范围未维护或维护格式错误，提交失败!");
                                    passFlag = 0;
                                    break;
                                }
                                else {
                                    var valueArry = standardValue.value.split(';');
                                    var tmp1 = Number(valueArry[0]);
                                    var tmp2 = Number(valueArry[1]);
                                    if (tmp1 >= tmp2) {
                                        maxV = tmp1;
                                        minV = tmp2;
                                    }
                                    else {
                                        maxV = tmp2;
                                        minV = tmp1;
                                    }
                                    if (Number(verifyContent.value) >= minV && Number(verifyContent.value) <= maxV) {
                                        verifyContent.style.backgroundColor = "green";
                                    }
                                    else {
                                        verifyContent.style.backgroundColor = "red";
                                        layer.alert("实际值(" + verifyContent.value + ")在标准值(" + standardValue.value + ")范围之外，提交失败!");
                                        passFlag = 0;
                                        break;
                                    }
                                }
                            }
                            else if (chkType == "2") {
                                //文本校对
                                if ($.trim(verifyContent.value) != $.trim(standardValue.value)) {
                                    verifyContent.style.backgroundColor = "red";
                                    layer.alert("维护信息(" + verifyContent.value + ") != 标准内容(" + standardValue.value + ")，提交失败!");
                                    passFlag = 0;
                                    break;
                                }
                                else {
                                    verifyContent.style.backgroundColor = "green";
                                }
                            }
                        }
                    }
                }
            }
            //2.查询是否需要验证钢网张力测试，并Check钢网张力是否在标准范围内
            if (passFlag == 1) {
                $.ajax({
                    url: 'SMM_ConfirmSteelMeshTension.ashx',
                    data: { 'id': data.field['input_id'] },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata != 'OK') {
                            layer.alert('确认钢网张力测试异常,Message：' + returndata + '，提交失败', { icon: 5 });
                            passFlag = 0;
                        }
                    },
                    error: function () {
                        layer.alert('确认钢网张力测试状态失败,请确认数据库连接!', { icon: 5 });
                        passFlag = 0;
                    }
                });
            }
            //3.提交验收表单
            if (passFlag == 1) {
                $.ajax({
                    url: 'SMM_AddEditDeatilVerifyFormSubmit.ashx',
                    data: {
                        'input_id': data.field['input_id'],
                        'verifyCount': data.field['record_count']
                    },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata != 'OK') {
                            layer.alert('提交失败,Message：' + returndata, { icon: 5 });
                        }
                        else {
                            layer.msg('提交成功!');
                        }
                    },
                    error: function () {
                        layer.alert('维护验收表单失败,请确认数据库连接!', { icon: 5 });
                    }
                });
            }
            return false;
        });
        //正则表达式，判断一个字符在另一个字符串中出现的次数
        function patch(re, s) {
            re = eval("/" + re + "/ig")
            return s.match(re).length;
        }

        form.render();
    });
</script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <h2 runat="server" id="h2Show" style="text-align:center;">钢网验收表单管理</h2>
        <br />
        <div style="margin: 0 auto;text-align: center;width: 95%;">
            <%=outhtml()%>
        </div>
    </form>
</body>
</html>
