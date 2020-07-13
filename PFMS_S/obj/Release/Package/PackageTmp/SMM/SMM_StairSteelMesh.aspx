<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_StairSteelMesh.aspx.cs" Inherits="PFMS_S.SMM_StairSteelMesh" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网阶梯管理</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            //根据用户权限添加标签元素
            var item = document.getElementById("inputPower").value;
            var htmlStr = "";
            if (item == 2) {
                htmlStr = "<button id=\"btnAdd\" type=\"button\" title=\"新增\" class=\"layui-btn layui-btn-sm\" onclick=\"AddStair()\"><i class=\"layui-icon\">+</i></button>";
                $("#btnAddArea").append(htmlStr);
            }
            GetStair();
        }
        //获取已维护的钢网尺寸清单
        function GetStair() {
            var condition = "";
            var item = document.getElementById("inputPower").value;
            layui.use(['table', 'layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                table.render({
                    elem: '#StairList',
                    url: 'SMM_GetStairList.ashx?condition=' + condition,
                    title: '钢网阶梯清单',
                    cols: [[
                        { field: 'id', title: 'ID', width: 80, unresize: true, sort: true, align: 'center' },
                        { field: 'stair', title: '钢网阶梯', align: 'center' },
                        { fixed: 'right', title: '操作', toolbar: '#Operate', width: 80, unresize: true, align: 'center', hide: (item == 2) ? false : true }
                    ]],
                    page: false,
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

                //监听行工具事件,用来删除记录
                table.on('tool(StairList)', function (obj) {
                    var data = obj.data;
                    //console.log(obj)
                    if (obj.event === 'del') {
                        layer.confirm('真的删除行么', function (index) {
                            $.ajax({
                                url: 'SMM_DelStair.ashx',
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

        function AddStair() {
            layui.use("layer", function () {
                var layer = layui.layer;
                layer.open({
                    type: 2,
                    title: '钢网阶梯添加',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['500px', '200px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'SMM_AddStair.aspx',
                    end: function () {
                        GetStair();
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
    <div style="width:100%;text-align:center;margin:20px auto;">
        <h2 style="color:#666666;">钢网阶梯清单</h2>
    </div>
    <div id="div_Table" style="width:99%;margin:0 auto;">
        <div id="btnAddArea" style="text-align:right;width:100%;"></div>
        <table id="StairList" class="layui-hide" style="width:100%;" lay-filter="StairList"></table>
    </div>
</body>
</html>
