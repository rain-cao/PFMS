<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageNavigation.aspx.cs" Inherits="PFMS_S.PageNavigation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>页面导航</title>
    <link rel="icon" href="Images/iconTitle.ico" />
    <link rel="stylesheet" href="layui/css/layui.css" />
    <script src="layui/layui.js"></script>
    <script src="js/jquery-3.4.1.js"></script>
    <script src="js/jquery-3.4.1.min.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            GetPageList();
        }
        function GetPageList() {
            layui.use(['table','layer'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var ii = layer.load();
                table.render({
                    elem: '#PageList',
                    url: 'GetPageList.ashx',
                    title: '页面导航',
                    page: false,
                    cols: [[
                        { field: 'menu_name', title: '目录/菜单名称', width: 150, align: 'center' },
                        { field: 'parent', title: '归属目录', width: 110,align: 'center' },
                        { field: 'url', title: '地址', align: 'center' },
                        { field: 'type', title: '类型', width: 110,align: 'center' },
                        { field: 'status', title: '状态', width: 110, align: 'center', templet: '#statusShow', },
                        { field: 'remark', title: '备注信息', align: 'center' },
                        { field: 'power', title: '用户权限', width: 130, align: 'center' },
                    ]],
                    done: function (res, curr, count) {
                    }
                });
                layer.close(ii);
            });
        } 
    </script>

    <script type="text/html" id="statusShow">
        <span {{d.status == 1 ? 'style="background-color:lime"' : 'style="background-color:lightgray"'}}>{{d.status == 1 ? '启用' : '禁用'}}</span>
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form id="form1" runat="server">
        <div id="div_Table" style="width:99%;margin:0 auto;">
            <table id="PageList" class="layui-hide" style="width:100%;" lay-filter="PageList"></table>
        </div>
    </form>
</body>
</html>
