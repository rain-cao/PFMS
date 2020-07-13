<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_UserManagement.aspx.cs" Inherits="PFMS_S.BackstageManagement.BM_UserManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>人员管理</title>
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
            //if (item == 1) {
                htmlStr = "<input id=\"input-condition\" type=\"text\" class=\"layui-input-inline\" style=\"width: 150px; height: 30px;\" lay-verify=\"SearchCondition\" autocomplete=\"off\" placeholder=\"请输入查询条件\" />";
                htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn\" onclick=\"GetUsers()\">查询</button>";
            //}
            /*else if (item == 2) {
                htmlStr = "<button id=\"btnAdd\" type=\"button\" class=\"layui-btn layui-btn-sm\" onclick=\"AddEditUser(0)\"><i class=\"layui-icon\">+</i></button>";
                htmlStr += "<input id=\"input-condition\" type=\"text\" class=\"layui-input-inline\" style=\"width: 150px; height: 30px;\" lay-verify=\"SearchCondition\" autocomplete=\"off\" placeholder=\"请输入查询条件\" />";
                htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn\" onclick=\"GetUsers()\">查询</button>";
            }*/
            $("#div_Search").append(htmlStr);
            GetUsers();
        }
        //获取已维护的部门清单
        function GetUsers() {
            var condition = document.getElementById("input-condition").value;
            var item = document.getElementById("inputPower").value;
            layui.use(['table','layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                //var ii = layer.load(0, { shade: false });
                var ii = layer.load();
                table.render({
                    elem: '#userList',
                    url: 'BM_GetUserList.ashx',
                    title: '用户清单',
                    method: 'GET',
                    where: { condition: condition, flag: '0' }, //flag为0代表人员用户管理时获取人员相关信息列表
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
                        { field: 'name', title: '用户姓名', width: 110,align: 'center' },
                        { field: 'username', title: '用户账号', width: 130,align: 'center' },
                        { field: 'dept', title: '归属部门', width: 110,align: 'center' },
                        { field: 'post', title: '所处职务', width: 110,align: 'center' },
                        { field: 'telephone', title: '手机号', align: 'center' },
                        { field: 'email', title: '邮箱', align: 'center' },
                        { field: 'weixin_no', title: '微信号', align: 'center' },
                        { field: 'user_state', title: '状态', width: 100, templet: '#switchStatus', unresize: true, align: 'center' },
                        { fixed: 'right', title: '操作', toolbar: '#Operate', width: 120, fixed: 'right', unresize: true, align: 'center', hide: (item == 2) ? false : true }
                    ]],
                    //page: true,
                    done: function (res, curr, count) {
                        var switchArray = document.getElementsByName("user_state");
                        for (var i = 0; i < switchArray.length; i++) {
                            if (item !== '2') {
                                switchArray[i].disabled = true;
                                switchArray[i].style.cursor = "not-allowed";
                            }
                        }
                    }
                });
                layer.close(ii);

                //监听行工具事件,用来删除记录
                table.on('tool(userList)', function (obj) {
                    var data = obj.data;
                    //console.log(obj)
                    if (obj.event === 'del') {
                        layer.confirm('真的删除行么', function (index) {
                            $.ajax({
                                url: 'BM_DelUser.ashx',
                                data: { 'id': obj.data.user_id },
                                dataType: 'text',
                                //contentType: 'application/json',
                                type: 'POST',
                                async: true,
                                cache: false,
                                success: function (returndata) {
                                    if (returndata == 'OK') {
                                        obj.del();
                                        table.reload('InfoListReload', {});
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
                        AddEditUser(obj.data.user_id);
                    }
                });
                form.render();
            });
        } 

        //切换switch开关更改用户状态
        layui.use('form', function () {
            var form = layui.form;
            form.on('switch(switchStatus)', function () {
                var obj = this;
                $.ajax({
                    url: 'BM_EditUserStatus.ashx',
                    data: { 'id': this.value },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata != 'OK') {
                            layer.alert('用户状态编辑失败!,Message：' + returndata, { icon: 5 });
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

        //弹出框新增或编辑用户信息
        function AddEditUser(userid) {
            layui.use("layer", function () {
                var layer = layui.layer;
                layer.open({
                    type: 2,
                    title: (userid == 0) ? '新增用户' : '用户编辑',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['600px', '700px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'BM_AddEditUser.aspx?user_id=' + userid.toString(),
                    end: function () {
                        GetUsers();
                    }
                });
            });
        }
    </script>

    <script type="text/html" id="Operate">
        <a class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>
        <a class="layui-btn layui-btn-danger layui-btn-xs" style="font-weight:900;" lay-event="del">删除</a>
    </script>

    <script type="text/html" id="switchStatus">
        <input type="checkbox" name="user_state" value="{{d.user_id}}" lay-skin="switch" lay-text="启用|禁用" lay-filter="switchStatus" {{ d.user_state == 1 ? 'checked' : '' }} />
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <input runat="server" type="hidden" id="inputPower" value="" />
    <div id="div_Search" style="width:100%;text-align:center;margin-top:20px;">
  
    </div>
    <div id="div_Table" style="width:99%;margin:0 auto;">
        <table id="userList" class="layui-hide" style="width:100%;" lay-filter="userList"></table>
    </div>
</body>
</html>
