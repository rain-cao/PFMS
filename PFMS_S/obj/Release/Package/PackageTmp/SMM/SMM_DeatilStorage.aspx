<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_DeatilStorage.aspx.cs" Inherits="PFMS_S.SteelMeshManagement.SMM_DeatilStorage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>储位管理</title>
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
            if (item == 1) {
                htmlStr = "<input id=\"input-condition\" type=\"text\" class=\"layui-input-inline\" style=\"width: 150px; height: 30px;\" lay-verify=\"SearchCondition\" autocomplete=\"off\" placeholder=\"请输入查询条件\" />";
                htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn\" onclick=\"GetDetailStorage()\">查询</button>";
            }
            else if (item == 2) {
                htmlStr = "<button id=\"btnAdd\" type=\"button\" class=\"layui-btn layui-btn-sm\" onclick=\"AddEditDetailStorage(0)\"><i class=\"layui-icon\">+</i></button>";
                htmlStr += "&nbsp;<input id=\"input-condition\" type=\"text\" class=\"layui-input-inline\" style=\"width: 150px; height: 30px;\" lay-verify=\"SearchCondition\" autocomplete=\"off\" placeholder=\"请输入查询条件\" />";
                htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn\" onclick=\"GetDetailStorage()\">查询</button>";
            }
            $("#div_Search").append(htmlStr);
            GetDetailStorage();
        }
        //获取已维护的储位清单
        function GetDetailStorage() {
            var condition = document.getElementById("input-condition").value;
            var item = document.getElementById("inputPower").value;
            layui.use(['table','layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                table.render({
                    elem: '#DSList',
                    url: 'SMM_GetDetailStorageList.ashx',
                    method: 'GET',
                    where: { condition: condition},
                    title: '详细储位清单',
                    page: true,
                    limits: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
                    limit: 10,
                    request: {
                        pageName: 'curr', //页码的参数名称，默认：page
                        limitName: 'nums' //每页数据量的参数名，默认：limit
                    },
                    id: 'InfoListReload',
                    cols: [[
                        { field: 'id', title: 'ID', width: 80, unresize: true, sort: true, align: 'center' },
                        { field: 'storage', title: '归属大储位', align: 'center' },
                        { field: 'storage_name', title: '详细储位(编码)', align: 'center' },
                        { field: 'status', title: '状态', width: 100, templet: '#switchStatus', unresize: true, align: 'center' },
                        { fixed: 'right', title: '操作', toolbar: '#Operate', width: 120, fixed: 'right', unresize: true, align: 'center', hide: (item == 2) ? false : true }
                    ]],
                    done: function (res, curr, count) {
                        var switchArray = document.getElementsByName("status");
                        for (var i = 0; i < switchArray.length; i++) {
                            if (item !== '2') {
                                switchArray[i].disabled = true;
                                switchArray[i].style.cursor = "not-allowed";
                            }
                        }
                    }
                });

                //监听行工具事件,用来删除记录
                table.on('tool(DSList)', function (obj) {
                    var data = obj.data;
                    //console.log(obj)
                    if (obj.event === 'del') {
                        layer.confirm('真的删除行么', function (index) {
                            $.ajax({
                                url: 'SMM_DelDetailStorage.ashx',
                                data: { 'id': obj.data.id },
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
                        AddEditDetailStorage(obj.data.id);
                    }
                });
                form.render();
            });
        } 

        //切换switch开关更改储位状态
        layui.use('form', function () {
            var form = layui.form;
            form.on('switch(switchStatus)', function () {
                var obj = this;
                $.ajax({
                    url: 'SMM_EditDetailStorageStatus.ashx',
                    data: { 'id': this.value },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata != 'OK') {
                            layer.alert('详细储位状态编辑失败!,Message：' + returndata, { icon: 5 });
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

        //弹出框新增或编辑储位信息
        function AddEditDetailStorage(id) {
            layui.use(['layer','table'], function () {
                var layer = layui.layer;
                var table = layui.table;
                layer.open({
                    type: 2,
                    title: (id == 0) ? '新增详细储位' : '详细储位编辑',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['600px', '300px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'SMM_AddEditDetailStorage.aspx?id=' + id.toString(),
                    end: function () {
                        //GetDetailStorage();
                        table.reload('InfoListReload', {});
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
        <input type="checkbox" name="status" value="{{d.id}}" lay-skin="switch" lay-text="启用|禁用" lay-filter="switchStatus" {{ d.status == 1 ? 'checked' : '' }} />
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <input runat="server" type="hidden" id="inputPower" value="" />
    <div id="div_Search" style="width:100%;text-align:center;margin-top:20px;">
  
    </div>
    <div id="div_Table" style="width:99%;margin:0 auto;">
        <table id="DSList" class="layui-hide" style="width:100%;" lay-filter="DSList"></table>
    </div>
</body>
</html>
