<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_ClientPagePower.aspx.cs" Inherits="PFMS_S.SMM.SMM_ClientPagePower" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>客户端权限管理</title>
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

        function GetUsers() {
            var condition = document.getElementById("input-condition").value;
            var item = document.getElementById("inputPower").value;
            layui.use(['table', 'layer','form'], function () {
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
                    where: { condition: condition, flag: '3' }, //flag为3代表客户端页面管理时获取的用户清单
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
                        { field: 'client_page', title: '客户端权限配置', align: 'center' },
                        { fixed: 'right', title: '操作', toolbar: '#Operate', width: 120, fixed: 'right', unresize: true, align: 'center', hide: (item == 2) ? false : true }
                    ]],
                    //page: true,
                    done: function (res, curr, count) {
                    }
                });
                layer.close(ii);

                table.on('tool(userList)', function (obj) {
                    var data = obj.data;
                    //console.log(obj)
                    if (obj.event === 'edit') {
                        layer.open({
                            type: 2,
                            title: '客户端页面权限管理',
                            skin: "layui-layer-lan",
                            shadeClose: true,
                            shade: 0.8,
                            area: ['620px', '380px'],
                            fixed: true, //固定
                            maxmin: false,
                            content: 'SMM_AddEditClientPagePower.aspx?id=' + data.user_id,
                            end: function () {
                                table.reload('InfoListReload', {});
                            }
                        });
                    }
                });
                form.render();
            });
        }
    </script>

    <script type="text/html" id="Operate">
        <a class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>
    </script>

    <style type="text/css">
        .layui-table-cell {
            font-size:14px;
            padding:0 5px;
            height:auto;
            overflow:visible;
            text-overflow:inherit;
            white-space:normal;
            word-break: break-all;
        }
    </style>
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
