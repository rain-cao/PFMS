<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_MenuManagement.aspx.cs" Inherits="PFMS_S.BackstageManagement.BM_MenuManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>目录菜单管理</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" type="text/css" href="../layui/css/layui.css" />
    <link rel="stylesheet" type="text/css" href="../CSS/StyleDefault.css" />
    <script src="../js/jquery-3.4.1.js" charset="utf-8"></script>
    <script src="../js/jquery-3.4.1.min.js" charset="utf-8"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        //页面加载时设置侧边菜单栏高度
        window.onload = function () {
            layui.use(['layer'], function () {
                var layer = layui.layer;
                var h = $(window).height() - 30;
                var w = $(window).width() - 30;
                layer.open({
                    type: 2,
                    title: '目录菜单管理',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: [w.toString() + 'px', h.toString() + 'px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'BM_EditMenuManagement.aspx?user_id=' + document.getElementById("userid").value,
                });
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input runat="server" id="userid" type="hidden" value="" />
    </form>
</body>
</html>
