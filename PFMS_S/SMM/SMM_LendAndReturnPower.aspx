<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_LendAndReturnPower.aspx.cs" Inherits="PFMS_S.SMM.SMM_LendAndReturnPower" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网借出归还权限管理</title>
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
                    where: { condition: condition, flag: '1' }, //flag为1代表钢网借出权限管理时获取人员相关信息列表
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
                        { field: 'power_status', title: '权限状态', width: 100, templet: '#switchStatus', unresize: true, align: 'center' },
                    ]],
                    //page: true,
                    done: function (res, curr, count) {
                        var switchArray = document.getElementsByName("power_status");
                        for (var i = 0; i < switchArray.length; i++) {
                            if (item !== '2') {
                                switchArray[i].disabled = true;
                                switchArray[i].style.cursor = "not-allowed";
                            }
                        }
                    }
                });
                layer.close(ii);
                form.render();
            });
        }

        //切换switch开关更改用户借用钢网的权限
        layui.use(['layer','form'], function () {
            var form = layui.form;
            var layer = layui.layer;
            form.on('switch(switchStatus)', function () {
                var obj = this;
                $.ajax({
                    url: 'SMM_EditUserLendPowerStatus.ashx',
                    data: { 'user_id': this.value },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata != 'OK') {
                            layer.alert('用户借用权限状态编辑失败!,Message：' + returndata, { icon: 5 });
                            obj.checked = obj.checked ? false : true;
                            form.render('checkbox');
                        }
                    },
                    error: function () {
                        layer.alert("请检查数据库连接!", { icon: 2 });
                        obj.checked = obj.checked ? false : true;
                        form.render('checkbox');
                    }
                });
            });
        });

        $('#input-condition').bind('keydown', function (event) {
            if (event.keyCode == "13") {
                $('#btnSearch').click();
            }
        });
    </script>

    <script type="text/html" id="switchStatus">
        <input type="checkbox" name="power_status" value="{{d.user_id}}" lay-skin="switch" lay-text="授权|无权限" lay-filter="switchStatus" {{ d.power_status == 1 ? 'checked' : '' }} />
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
