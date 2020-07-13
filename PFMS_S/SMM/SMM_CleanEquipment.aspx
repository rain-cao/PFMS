<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_CleanEquipment.aspx.cs" Inherits="PFMS_S.SMM.SMM_CleanEquipment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>清洗设备管理</title>
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
                htmlStr = "<button id=\"btnAdd\" type=\"button\" title=\"新增\" class=\"layui-btn layui-btn-sm\" onclick=\"AddEditEquipment(0)\"><i class=\"layui-icon\">+</i></button>";
                $("#btnAddArea").append(htmlStr);
            }
            GetEquipment();
        }
        //获取清洗设备清单
        function GetEquipment() {
            var item = document.getElementById("inputPower").value;
            layui.use(['table','layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                table.render({
                    elem: '#EquipList',
                    url: 'SMM_GetEquipmentList.ashx',
                    title: '清洗设备清单',
                    //method: 'GET',
                    //where: {},
                    page: false,
                    //limits: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
                    //limit: 10,
                    //request: {
                    //    pageName: 'curr', //页码的参数名称，默认：page
                    //    limitName: 'nums' //每页数据量的参数名，默认：limit
                    //},
                    cols: [[
                        { field: 'id', title: 'ID', width: 80, unresize: true, sort: true, align: 'center' },
                        { field: 'Equipment_name', title: '设备名称', align: 'center' },
                        { field: 'ip_address', title: '设备IP地址', align: 'center' },
                        { field: 'clean_length', title: '清洗时长', align: 'center' },
                        { field: 'productline_id', title: '绑定线体', align: 'center' },
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
                table.on('tool(EquipList)', function (obj) {
                    var data = obj.data;
                    //console.log(obj)
                    if (obj.event === 'del') {
                        if (data.Equipment_name == "人工清洗") {
                            layer.alert("\"人工清洗\"为系统保留字段，不可删除!");
                        }
                        else {
                            layer.confirm('真的删除行么', function (index) {
                                $.ajax({
                                    url: 'SMM_DelEquipment.ashx',
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
                    }
                    else if (obj.event === 'edit') {
                        AddEditEquipment(obj.data.id);
                    }
                });
                form.render();
            });
        } 

        //切换switch开关更改设备使用状态
        layui.use('form', function () {
            var form = layui.form;
            form.on('switch(switchStatus)', function () {
                var obj = this;
                $.ajax({
                    url: 'SMM_EditEquipmentStatus.ashx',
                    data: { 'id': this.value },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata != 'OK') {
                            layer.alert('设备状态编辑失败!,Message：' + returndata, { icon: 5 });
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

        //弹出框新增或编辑清洗设备
        function AddEditEquipment(id) {
            layui.use("layer", function () {
                var layer = layui.layer;
                layer.open({
                    type: 2,
                    title: (id == 0) ? '新增设备' : '设备编辑',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['690px', '640px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'SMM_AddEditEquipment.aspx?id=' + id.toString(),
                    end: function () {
                        GetEquipment();
                    }
                });
            });
        }
    </script>

    <script type="text/html" id="Operate">
        <a class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>
        <a class="layui-btn layui-btn-danger layui-btn-xs" lay-event="del">删除</a>
    </script>

    <script type="text/html" id="switchStatus">
        <input type="checkbox" name="status" value="{{d.id}}" lay-skin="switch" lay-text="启用|禁用" lay-filter="switchStatus" {{ d.status == 1 ? 'checked' : '' }} />
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
    <input runat="server" type="hidden" id="inputPower" value="" />
    <div style="width:100%;text-align:center;margin:20px auto;">
        <h2 style="color:#666666;">清洗设备管理</h2>
    </div>
    <div id="div_Table" style="width:99%;margin:0 auto;">
        <div id="btnAddArea" style="text-align:right;width:100%;"></div>
        <table id="EquipList" class="layui-hide" style="width:100%;" lay-filter="EquipList"></table>
    </div>
</body>
</html>
