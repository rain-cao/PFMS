<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_UserPowerManagement.aspx.cs" Inherits="PFMS_S.BM.BM_UserPowerManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>目录菜单权限管理</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../layui/layui.js"></script>
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            var htmlStr = "";
            htmlStr = "<input id=\"input-condition\" type=\"text\" class=\"layui-input-inline\" style=\"width: 150px; height: 30px;\" lay-verify=\"SearchCondition\" autocomplete=\"off\" placeholder=\"请输入查询条件\" />";
            htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn\" onclick=\"GetUsers()\">查询</button>";

            $("#div_Search").append(htmlStr);
            GetUsers();
        }
        //获取用户列表以及钢网借用权限
        function GetUsers() {
            var condition = document.getElementById("input-condition").value;
            var item = document.getElementById("inputPower").value;
            layui.use(['table', 'layer', 'form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                //var ii = layer.load(0, { shade: false });
                var ii = layer.load();
                table.render({
                    elem: '#userList',
                    url: '../BM/BM_GetUserList.ashx',
                    title: '用户清单',
                    method: 'GET',
                    where: { condition: condition, flag: '2' }, //flag为2代表目录菜单权限管理时获取人员相关信息列表
                    page: true,
                    limits: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
                    limit: 10,
                    request: {
                        pageName: 'curr', //页码的参数名称，默认：page
                        limitName: 'nums' //每页数据量的参数名，默认：limit
                    },
                    id: 'InfoListReload',
                    cols: [[
                        { field: 'user_id', title: 'ID', width: 80, unresize: true, sort: true, align: 'center' },
                        { field: 'name', title: '用户姓名', align: 'center' },
                        { field: 'username', title: '用户账号', align: 'center' },
                        { field: 'dept', title: '归属部门', align: 'center' },
                        { field: 'post', title: '所处职务', align: 'center' },
                        { fixed: 'right', title: '操作', toolbar: '#Operate', width: 60, fixed: 'right', unresize: true, align: 'center' }
                    ]],
                    //page: true,
                    done: function (res, curr, count) {
                        
                    }
                });
                layer.close(ii);

                //监听行工具事件
                table.on('tool(userList)', function (obj) {
                    var data = obj.data;
                    var h = $(window).height() - 30;
                    var w = $(window).width() - 30;
                    if (obj.event === 'edit') {
                        layer.open({
                            type: 2,
                            title: '目录菜单权限明细',
                            skin: "layui-layer-lan",
                            shadeClose: true,
                            shade: 0.8,
                            area: [w.toString() + 'px', h.toString() + 'px'],
                            fixed: true, //固定
                            maxmin: false,
                            content: 'BM_EditMenuPowerForUser.aspx?user_id=' + data.user_id,
                        });
                    }
                });
                form.render();
            });
        }
    </script>

    <script type="text/html" id="Operate">
        <a class="" lay-event="edit" title="权限明细" style="cursor:pointer;"><img src="../Images/view.png" style="width:24px;height:24px;margin-top:-5px;" /></a>
    </script>

</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <input runat="server" type="hidden" id="inputPower" value="" />
        <div id="div_Search" style="width:100%;text-align:center;margin-top:20px;">
  
        </div>
        <div id="div_Table" style="width:99%;margin:0 auto;">
            <table id="userList" class="layui-hide" style="width:100%;" lay-filter="userList"></table>
        </div>
    </form>
</body>
</html>
