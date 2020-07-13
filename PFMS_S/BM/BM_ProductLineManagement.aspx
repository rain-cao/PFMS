<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_ProductLineManagement.aspx.cs" Inherits="PFMS_S.BackstageManagement.BM_ProductLineManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>产线管理</title>
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
                htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn\" onclick=\"GetProductLine()\">查询</button>";
            }
            else if (item == 2) {
                htmlStr = "<button id=\"btnAdd\" type=\"button\" class=\"layui-btn layui-btn-sm\" onclick=\"AddProductLine()\"><i class=\"layui-icon\">+</i></button>";
                htmlStr += "&nbsp;<input id=\"input-condition\" type=\"text\" class=\"layui-input-inline\" style=\"width: 150px; height: 30px;\" lay-verify=\"SearchCondition\" autocomplete=\"off\" placeholder=\"请输入查询条件\" />";
                htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn\" onclick=\"GetProductLine()\">查询</button>";
            }
            $("#div_Search").append(htmlStr);
            GetProductLine();
        }
        //获取已维护的部门清单
        function GetProductLine() {
            var condition = document.getElementById("input-condition").value;
            var item = document.getElementById("inputPower").value;
            layui.use(['table','layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                var ii = layer.load();
                table.render({
                    elem: '#PLList',
                    url: 'BM_GetProductLineList.ashx',
                    title: '产线清单',
                    method: 'GET',
                    where: { condition: condition },
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
                        { field: 'pl_name', title: '产线名称', width: 200, align: 'center' },
                        { field: 'code', title: '产线代码', width: 180, sort: true, align: 'center' },
                        { field: 'address', title: '产线位置', align: 'center' },
                        { field: 'workshop', title: '所属车间', width: 200, align: 'center' },
                        { field: 'owner', title: '负责人', width: 120, align: 'center' },
                        { field: 'remark', title: '备注信息', align: 'center' },
                        { fixed: 'right', title: '操作', toolbar: '#Operate', width: 80, fixed: 'right', unresize: true, align: 'center', hide: (item == 2) ? false : true }
                    ]],
                    //page: true,
                    done: function (res, curr, count) {
                        //如果是异步请求数据方式，res即为你接口返回的信息。
                        //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
                        //console.log(res);

                        //得到当前页码
                        //console.log(curr);

                        //得到数据总量
                        //console.log(count);
                    }
                });
                layer.close(ii);

                //监听行工具事件,用来删除记录
                table.on('tool(PLList)', function (obj) {
                    var data = obj.data;
                    //console.log(obj)
                    if (obj.event === 'del') {
                        layer.confirm('真的删除行么', function (index) {
                            $.ajax({
                                url: 'BM_DelProductLine.ashx',
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
                    /*else if (obj.event === 'edit') {
                        layer.prompt({
                            formType: 2
                            , value: data.email
                        }, function (value, index) {
                            obj.update({
                                email: value
                            });
                            layer.close(index);
                        });
                    }*/
                });
                form.render();
            });
        } 

        function AddProductLine() {
            layui.use("layer", function () {
                var layer = layui.layer;
                layer.open({
                    type: 2,
                    title: '线别添加',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['500px', '450px'],
                    fixed: true, //不固定
                    maxmin: false,
                    content: 'BM_AddProductLine.aspx',
                    end: function () {
                        GetProductLine();
                    }
                });
            });
        }
    </script>

    <script type="text/html" id="Operate">
        <a class="layui-btn layui-btn-danger layui-btn-xs" style="font-weight:900;" lay-event="del" title="删除">X</a>
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <input runat="server" type="hidden" id="inputPower" value="" />
    <div id="div_Search" style="width:100%;text-align:center;margin-top:20px;">
  
    </div>
    <div id="div_Table" style="width:99%;margin:0 auto;">
        <table id="PLList" class="layui-hide" style="width:100%;" lay-filter="PLList"></table>
    </div>
</body>
</html>
