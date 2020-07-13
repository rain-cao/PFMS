<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_SignRule.aspx.cs" Inherits="PFMS_S.SteelMeshManagement.SMM_SignRule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网使用审核规则管理</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            //根据用户权限添加标签元素
            var item = document.getElementById("inputPower").value;
            var htmlStr = "";
            if (item == 2) {
                htmlStr = "<button id=\"btnAdd\" type=\"button\" title=\"新增\" class=\"layui-btn layui-btn-sm\" onclick=\"AddEditRule(0)\"><i class=\"layui-icon\">+</i></button>";
                $("#btnAddArea").append(htmlStr);
            }
            GetRule();
        }
        //获取已维护的钢网使用审核规则
        function GetRule() {
            var item = document.getElementById("inputPower").value;
            layui.use(['table', 'layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                table.render({
                    elem: '#RuleList',
                    url: 'SMM_GetSignRule.ashx?',
                    title: '钢网使用审核规则',
                    cols: [[
                        { field: 'id', title: 'ID', width: 80, unresize: true, sort: true, align: 'center' },
                        { field: 'total_use_count', title: '最大使用次数', width: 115, align: 'center' },
                        { field: 'status_rule', title: '超限规则', width: 115, align: 'center' },
                        { field: 'alert_count', title: '预警使用次数', width: 115, align: 'center' },
                        { field: 'alert_mailto', title: '预警发送人员', align: 'center' },
                        { field: 'alert_mailcopy', title: '预警抄送人员', align: 'center' },
                        { field: 'transfinite_mailto', title: '超限发送人员', align: 'center' },
                        { field: 'transfinite_mailcopy', title: '超限抄送人员', align: 'center' },
                        { fixed: 'right', title: '操作', toolbar: '#Operate', width: 160, unresize: true, align: 'center', hide: (item == 2) ? false : true }
                    ]],
                    page: false
                });

                //监听行工具事件,用来删除记录
                table.on('tool(RuleList)', function (obj) {
                    var data = obj.data;
                    if (obj.event === 'del') {
                        layer.confirm('真的删除行么', function (index) {
                            $.ajax({
                                url: 'SMM_DelSignRule.ashx',
                                data: { 'id': obj.data.id },
                                dataType: 'text',
                                //contentType: 'application/json',
                                type: 'POST',
                                async: true,
                                cache: false,
                                success: function (returndata) {
                                    if (returndata == 'OK') {
                                        obj.del();
                                    } else {
                                        layer.alert('删除失败!,Message：' + returndata, { icon: 5 });
                                    }
                                },
                                error: function () {
                                    layer.alert("请检查数据库连接!", { icon: 2 });
                                },
                                complete: function () {
                                    layer.close(index);
                                }
                            });
                        });
                    }
                    else if (obj.event === 'edit') {
                        AddEditRule(obj.data.id);
                    }
                });
                form.render();
            });
        }

        function AddEditRule(idStr) {
            layui.use("layer", function () {
                var layer = layui.layer;
                layer.open({
                    type: 2,
                    title: '钢网使用次数审核规则',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['500px', '730px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'SMM_AddEditSignRule.aspx?id=' + idStr.toString(),
                    end: function () {
                        GetRule();
                    }
                });
            });
        }
    </script>

    <script type="text/html" id="Operate">
        <a class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>
        <a class="layui-btn layui-btn-danger layui-btn-xs" lay-event="del" >删除</a>
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <input runat="server" type="hidden" id="inputPower" value="" />
    <div style="width:100%;text-align:center;margin:20px auto;">
        <h2 style="color:#666666;">钢网使用审核规则</h2>
    </div>
    <div id="div_Table" style="width:99%;margin:0 auto;">
        <div id="btnAddArea" style="text-align:right;width:100%;"></div>
        <table id="RuleList" class="layui-hide" style="width:100%;" lay-filter="RuleList"></table>
    </div>
</body>
</html>
