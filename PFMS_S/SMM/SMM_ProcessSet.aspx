<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_ProcessSet.aspx.cs" Inherits="PFMS_S.SMM.SMM_ProcessSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>流程设定</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../layui/layui.js"></script>
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
<script type="text/javascript">
    window.onload = function () {
        //根据用户权限添加标签元素
        var item = document.getElementById("inputPower").value;
        var htmlStr = "";
        if (item == 2) {
            htmlStr = "<button id=\"btnAdd\" type=\"button\" title=\"新增\" class=\"layui-btn layui-btn-sm\" onclick=\"AddProductLineProcess()\"><i class=\"layui-icon\">+新增流程</i></button>";
            $("#btnAddArea").append(htmlStr);
        }
    }

    function AddProductLineProcess() {
        layui.use('layer', function () {
            var layer = layui.layer;
            layer.open({
                type: 2,
                title: '线上流程添加',
                skin: "layui-layer-lan",
                shadeClose: true,
                shade: 0.8,
                area: ['400px', '380px'],
                fixed: true, //固定
                maxmin: false,
                content: 'SMM_AddProductLineProcess.aspx',
                end: function () {
                    document.location.reload();
                }
            });
        });
    }

    function ShowdetailProcess(PLid) {
        var item = document.getElementById("inputPower").value;
        layui.use('layer', function () {
            var layer = layui.layer;
            layer.open({
                type: 2,
                title: '流程明细',
                skin: "layui-layer-lan",
                shadeClose: true,
                shade: 0.8,
                area: ['800px', '500px'],
                fixed: true, //固定
                maxmin: false,
                content: 'SMM_DetailProductLineProcess.aspx?power=' + item + '&PLid=' + PLid,
                end: function () {
                    document.location.reload();
                }
            });
        });
    }

    layui.use(['layer','form'], function () {
        var form = layer.form;
        form.render();
    })
</script>
<style type="text/css">
    a:hover{color:#FE6229;font-weight:bolder;}
</style>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <input runat="server" type="hidden" id="inputPower" value="" />
        <div style="width:100%;text-align:center;margin:20px auto;">
            <h2 runat="server" style="color:#666666;"></h2>
        </div>
        <div style="width:98%;margin:0 auto;">
            <div id="btnAddArea" style="text-align:right;width:100%;"></div>
            <hr />
            <div style="text-align:left;width:100%;">
                <%=outhtml() %>
            </div>
        </div>
    </form>
</body>
</html>
