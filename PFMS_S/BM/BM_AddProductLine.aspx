<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_AddProductLine.aspx.cs" Inherits="PFMS_S.BM_AddProductLine" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>添加产线</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        layui.use(['form','layer'], function () {
            var form = layui.form;
            var layer = layui.layer;

            form.verify({
                pl_name: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入产线名称!";
                    }
                },
            });

            //Form提交事件，将产线信息写入数据库
            form.on('submit(AddProductLine)', function (data) {
                console.log(data.field);
                $.ajax({
                    url: 'BM_AddProductLineSubmit.ashx',
                    data: data.field,
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.alert('添加成功!', { icon: 6 });
                        } else {
                            layer.alert('添加失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('添加失败,请确认数据库连接!', { icon: 5 });
                    }
                });
                return false;
            });
            form.render();
        })
    </script>
    <style>
        .layui-form-select dl {
            max-height: 120px;
        }
    </style>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <div class="layui-inline" style="width:100%;margin:0 auto;text-align:center;">
            <table style="margin:0 auto;text-align:center;border:none;border-collapse:collapse;width:92%">
                <tr><td colspan="2" style="text-align:center"><h2>新增线别</h2></td></tr>
                <tr>
                    <td style="text-align:right;height:50px;width:100px;">产线名称：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="pl_name" class="layui-input" value="" style="text-transform:uppercase;" placeholder="请输入产线名称" autocomplete="off" lay-verify="pl_name" required="required" /></td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;">产线代码：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="code" class="layui-input" value="" style="text-transform:uppercase;" placeholder="请输入产线代码" autocomplete="off" /></td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;">所在区域：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="address" class="layui-input" value="" placeholder="产线所处位置" autocomplete="off" /></td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;">所属车间：</td>
                    <td style="text-align:left;height:50px;">
                        <select runat="server" name="workshop_id" id="workshop_id" class="layui-select" >
                            <option value="">请选择...</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;">负责人员：</td>
                    <td style="text-align:left;height:50px;">
                        <select runat="server" name="ownerid" id="ownerid" class="layui-select" >
                            <option value="">请选择...</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;height:50px;">备注说明：</td>
                    <td style="text-align:left;height:50px;"><input type="text" name="remark" class="layui-input" value="" placeholder="备注" autocomplete="off" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center">
                        <hr />
                        <button type="submit" lay-submit="" lay-filter="AddProductLine" class="layui-btn layui-btn-lg">添加</button>
                        <button type="reset" id="reset" class="layui-btn layui-btn-lg">重置</button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
