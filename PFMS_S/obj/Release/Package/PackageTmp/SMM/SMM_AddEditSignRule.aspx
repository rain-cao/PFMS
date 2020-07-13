<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddEditSignRule.aspx.cs" Inherits="PFMS_S.SMM.SMM_AddEditSignRule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>编辑钢网使用次数审核规范</title>
    <link rel="icon" href="../Images/login_user.png" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <link rel="stylesheet" href="../CSS/StyleUserRegister.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        //Layui预加载
        layui.use(['form', 'layer','form'], function () {
            //操作对象
            var form = layui.form;
            var layer = layui.layer;
            var form = layui.form;
            //自定义验证信息
            form.verify({
                total_use_count: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请填入最大可使用次数!";
                    }
                },
                status_rule: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请填写使用超限执行规则!";
                    }
                },
                alert_count: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请填入达预警使用次数!";
                    }
                }
            });
            //根据选择的部门筛选对应人员列表
            form.on('select(dptalertto)', function (data) {
                MatchPersonnel(data.value, 'personalertto');
            });
            form.on('select(dptalertcopy)', function (data) {
                MatchPersonnel(data.value, 'personalertcopy');
            });
            form.on('select(dpttransfiniteto)', function (data) {
                MatchPersonnel(data.value, 'pertransfiniteto');
            });
            form.on('select(dpttransfinitecopy)', function (data) {
                MatchPersonnel(data.value, 'pertransfinitecopy');
            });
            //根据选择的人员填写文本框
            form.on('select(personalertto)', function (data) {
                MatchInputContent(data.value, 'alert_mailto');
            });
            form.on('select(personalertcopy)', function (data) {
                MatchInputContent(data.value, 'alert_mailcopy');
            });
            form.on('select(pertransfiniteto)', function (data) {
                MatchInputContent(data.value, 'transfinite_mailto');
            });
            form.on('select(pertransfinitecopy)', function (data) {
                MatchInputContent(data.value, 'transfinite_mailcopy');
            });
            //根据部门查询人员
            function MatchPersonnel(dptIDStr, selectID) {
                $.ajax({
                    url: 'SMM_GetPersonnelByDept.ashx',
                    data: { 'id': dptIDStr },
                    dataType: 'json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (data) {
                        if (data.flag == true) {
                            var item = document.getElementById(selectID);
                            item.options.length = 0;
                            item.appendChild(new Option("人员...", ""));
                            for (var i = 0, l = data.data.length; i < l; i++) {
                                item.appendChild(new Option(data.data[i].username + "(" + data.data[i].name + ")", data.data[i].username + "(" + data.data[i].name + ")"));
                            }
                            form.render('select');
                        }
                        else {
                            layer.alert('依部门查询人员失败,Message：' + data.msg, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('依部门查询人员失败,请确认数据库连接!', { icon: 5 });
                    }
                });
            }
            //根据选择的人员填充Input输入框
            function MatchInputContent(val, inputID) {
                var valTmp = document.getElementById(inputID).value;
                if (val != "") {
                    document.getElementById(inputID).value = valTmp + val + ";";
                }
            }

            //Form提交事件，将审核规则信息写入数据库
            form.on('submit(AddEditSignRule)', function (data) {
                //console.log(data.field);
                $.ajax({
                    url: 'SMM_AddEditSignRuleSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.msg('钢网使用次数审核规则编辑成功!')
                            //layer.alert('详细储位编辑成功!', { icon: 6 });
                            //if (data.field['input_id'] == '0') {
                                //$('#reset').click();
                            //}
                        } else {
                            layer.alert('钢网使用次数审核规则编辑失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('钢网使用次数审核规则编辑失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });
            form.render();
        });
    </script>
    <style>
        .layui-form-select dl {
            max-height: 120px;
        }
    </style>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <h2 runat="server" id="h2Show">钢网使用次数审核规则编辑</h2>
        <br />
        <table style="width:97%;margin:0 auto;">
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width:150px;">最大可使用次数：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 100%; text-align: left;">    
                        <input type="hidden" name="input_id" id="input_id" runat="server" value="" />
                        <input runat="server" name="total_use_count" id="total_use_count" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" lay-verify="total_use_count" required="required" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">超限处理：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 100%; text-align: left;">         
                        <input runat="server" name="status_rule" id="status_rule" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" lay-verify="status_rule" required="required" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">达预警使用次数：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 100%; text-align: left;">         
                        <input runat="server" name="alert_count" id="alert_count" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:uppercase;" lay-verify="alert_count" required="required" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">预警发送人员：</td>
                <td style="height: 100px;text-align: left;">
                    <div class="layui-inline" style="width: 100%; text-align: left;">      
                        <div style="width:49%;float:left;">
                            <select runat="server" name="dptalertto" id="dptalertto" class="layui-select" lay-filter="dptalertto">
                                <option value="">请选择部门</option>
                            </select>
                        </div>
                        <div style="width:49%;float:right;">
                            <select runat="server" name="personalertto" id="personalertto" class="layui-select" lay-filter="personalertto">
                                <option value="">请选择人员</option>
                            </select>
                        </div>
                    </div>
                    <div class="layui-inline" style="width: 100%; text-align: left;">  
                        <input runat="server" name="alert_mailto" id="alert_mailto" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:lowercase;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">预警抄送人员：</td>
                <td style="height: 100px;text-align: left;">
                    <div class="layui-inline" style="width: 100%; text-align: left;">      
                        <div style="width:49%;float:left;">
                            <select runat="server" name="dptalertcopy" id="dptalertcopy" class="layui-select" lay-filter="dptalertcopy">
                                <option value="">请选择部门</option>
                            </select>
                        </div>
                        <div style="width:49%;float:right;">
                            <select runat="server" name="personalertcopy" id="personalertcopy" class="layui-select" lay-filter="personalertcopy">
                                <option value="">请选择人员</option>
                            </select>
                        </div>
                    </div>
                    <div class="layui-inline" style="width: 100%; text-align: left;">  
                        <input runat="server" name="alert_mailcopy" id="alert_mailcopy" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:lowercase;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">超限发送人员：</td>
                <td style="height: 100px;text-align: left;">
                    <div class="layui-inline" style="width: 100%; text-align: left;">      
                        <div style="width:49%;float:left;">
                            <select runat="server" name="dpttransfiniteto" id="dpttransfiniteto" class="layui-select" lay-filter="dpttransfiniteto">
                                <option value="">请选择部门</option>
                            </select>
                        </div>
                        <div style="width:49%;float:right;">
                            <select runat="server" name="pertransfiniteto" id="pertransfiniteto" class="layui-select" lay-filter="pertransfiniteto">
                                <option value="">请选择人员</option>
                            </select>
                        </div>
                    </div>
                    <div class="layui-inline" style="width: 100%; text-align: left;">  
                        <input runat="server" name="transfinite_mailto" id="transfinite_mailto" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:lowercase;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">超限抄送人员：</td>
                <td style="height: 100px;text-align: left;">
                    <div class="layui-inline" style="width: 100%; text-align: left;">      
                        <div style="width:49%;float:left;">
                            <select runat="server" name="dpttransfinitecopy" id="dpttransfinitecopy" class="layui-select" lay-filter="dpttransfinitecopy">
                                <option value="">请选择部门</option>
                            </select>
                        </div>
                        <div style="width:49%;float:right;">
                            <select runat="server" name="pertransfinitecopy" id="pertransfinitecopy" class="layui-select" lay-filter="pertransfinitecopy">
                                <option value="">请选择人员</option>
                            </select>
                        </div>
                    </div>
                    <div class="layui-inline" style="width: 100%; text-align: left;">  
                        <input runat="server" name="transfinite_mailcopy" id="transfinite_mailcopy" type="text" value="" placeholder="" autocomplete="off" class="layui-input" style="width: 100%;text-transform:lowercase;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <hr />
                    <button runat="server" id="btnAddEdit" type="submit" lay-submit="" lay-filter="AddEditSignRule" class="layui-btn layui-btn-lg">添加</button>
                    <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
