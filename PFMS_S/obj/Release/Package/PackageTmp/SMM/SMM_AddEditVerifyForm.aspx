<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddEditVerifyForm.aspx.cs" Inherits="PFMS_S.SMM.SMM_AddEditVerifyForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>编辑验收表单</title>
    <link rel="icon" href="../Images/login_user.png" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
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
                verify_info: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入确认项描述信息!";
                    }
                },
                verify_type: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择确认项类型!";
                    }
                },
                required: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择是否为必须确认项!";
                    }
                }
            });

            //确认项类型选择事件
            form.on('select(verify_type)', function (data) {
                if (data.value == '2') {
                    document.getElementById('trSelectValue').style.display = "";
                }
                else {
                    document.getElementById('trSelectValue').style.display = 'none';
                }
            });

            //Form提交事件，将验收表单信息写入数据库
            form.on('submit(AddEditVerifyForm)', function (data) {
                //console.log(data.field);
                $.ajax({
                    url: 'SMM_AddEditVerifyFormSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.msg('编辑成功!');
                            if (data.field['input_id'] == '0') {
                                $('#reset').click();
                            }
                        } else {
                            layer.alert('编辑失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('编辑失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });
            form.render();
        });

        //监测select_value输入框内容，当出现中文；时将其改为因为;
        function MonitorInut(obj) {
            var str = obj.value.toString();
            var str1 = str.substr(str.length - 1, 1);
            if (str1 == "；") 
                obj.value = str.substr(0, str.length - 1) + ";";
        }
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <table style="margin:0 auto;text-align:center;border:none;border-collapse:collapse;width:95%">
            <tr><td colspan="2" style="text-align:center;height:50px;"><h2 runat="server" id="h2Show">验收表单编辑</h2></td></tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;width:30%;">确认项描述：</td>
                <td style="height: 50px;text-align: left;width:70%;">
                    <div class="layui-inline" style="width: 95%; text-align: left;">
                        <input type="hidden" name="input_id" id="input_id" runat="server" value="" />
                        <input runat="server" name="verify_info" id="verify_info" type="text" value="" autocomplete="off" class="layui-input" style="width: 100%;" lay-verify="verify_info" required="required" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">确认项类型：</td>
                <td style="height: 50px; text-align: left;">
                    <div class="layui-inline" style="width: 95%; text-align: left;">
                        <select runat="server" name="verify_type" id="verify_type" class="layui-select" lay-filter="verify_type" lay-verify="verify_type" required="required" >
                            <option value="">请选择...</option>
                            <option value="0">复选框</option>
                            <option value="1">文本输入框</option>
                            <option value="2">下拉列表框</option>
                        </select>
                    </div>
                </td>
            </tr>
            <tr runat="server" id="trSelectValue" style="display:none;">
                <td style="text-align: right;height: 50px;font-size: large;"></td>
                <td style="height: 50px; text-align: left;">
                    <div class="layui-inline" style="width: 95%; text-align: center;">
                        <input runat="server" name="select_value" id="select_value" value="" type="text" autocomplete="off" class="layui-input" style="width: 100%;" placeholder="请输入选择项,中间用英文;隔开" oninput="MonitorInut(this)" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">内容校对：</td>
                <td style="height: 50px; text-align: left;">
                    <div class="layui-inline" style="width: 95%; text-align: left;">
                        <div class="layui-inline" style="width: 35%;">
                            <select runat="server" name="check_type" id="check_type" class="layui-select" lay-verify="check_type" required="required" >
                                <option value="">请选择...</option>
                                <option value="0">无需校对</option>
                                <option value="1">数值校对</option>
                                <option value="2">文本校对</option>
                            </select>
                        </div>
                        <div class="layui-inline" style="width: 65%;float:right;">
                            <input runat="server" name="standard_value" id="standard_value" value="" type="text" autocomplete="off" class="layui-input" style="width: 100%;" title="仅针对文本输入框有效，其它确认类型请选无需校对" placeholder="标准值,数值最大最小用英文;隔开" oninput="MonitorInut(this)" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">是否必须确认：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 95%; text-align: left;">
                        <select runat="server" name="required" id="required" class="layui-select" lay-verify="required" required="required" >
                            <option value="">请选择...</option>
                            <option value="0">否</option>
                            <option value="1">是</option>
                        </select>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;height: 50px;font-size: large;">备注信息：</td>
                <td style="height: 50px;text-align: left;">
                    <div class="layui-inline" style="width: 95%; text-align: left;">
                        <input runat="server" name="remark" id="remark" type="text" value="" autocomplete="off" class="layui-input" style="width: 100%;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <hr />
                    <button runat="server" id="btnAddEdit" type="submit" lay-submit="" lay-filter="AddEditVerifyForm" class="layui-btn layui-btn-lg">添加</button>
                    <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
