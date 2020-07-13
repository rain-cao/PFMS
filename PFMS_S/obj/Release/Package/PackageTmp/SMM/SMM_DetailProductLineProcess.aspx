<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_DetailProductLineProcess.aspx.cs" Inherits="PFMS_S.SMM.SMM_DetailProductLineProcess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>线上流程明细</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            GetProcess();
        }
        //获取流程明细
        function GetProcess() {
            var condition = document.getElementById("PLid").value;
            var item = document.getElementById("inputPower").value;
            layui.use(['table', 'layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                var data_tr;
                table.render({
                    elem: '#ProcessList',
                    url: 'SMM_GetDetalProductLineProcess.ashx?condition=' + condition,
                    title: '流程明细',
                    cols: [[
                        { field: 'id', title: 'ID', width: 80, unresize: true, align: 'center' },
                        { field: 'order_number', title: '排序', width: 120, toolbar: '#orderOprate', unresize: true, align: 'center', hide: (item == 2) ? false : true },
                        { field: 'status_id', title: '流程状态', width: 120, align: 'center' },
                        { field: 'role_define', title: '卡关提示内容', align: 'center' },
                        { field: 'role_force', title: '是否强制卡关', width: 130, align: 'center' },
                        { fixed: 'right', title: '操作', toolbar: '#Operate', width: 80, unresize: true, align: 'center', hide: (item == 2) ? false : true }
                    ]],
                    id: 'InfoListReload',
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

                //监听行工具条事件,用来删除或编辑记录以及行内操作
                table.on('tool(ProcessList)', function (obj) {
                    ////console.log(obj)
                    data_tr = $(this);
                    if (obj.event === 'del') {
                        layer.confirm('真的要删除流程(' + obj.data.status_id + ')么？', function (index) {
                            $.ajax({
                                url: 'SMM_DelDetailProductLineProcess.ashx',
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
                    else if (obj.event === "up") {
                        MoveTableRowUpDown(obj.data.id, 'up', obj.data.order_number);
                    }
                    else if (obj.event === "down") {
                        MoveTableRowUpDown(obj.data.id, 'down', obj.data.order_number);
                    }
                });

                //控制table行上移或下移，并更新数据库排序
                function MoveTableRowUpDown(idStr, updown, ordernum) {
                    //var tbData = table.cache.InfoListReload; //是一个Array
                    if (data_tr == null) {
                        layer.msg("请选择元素");
                        return;
                    }
                    var tr = $(data_tr).parent().parent().parent();
                    if (updown == "up") {
                        if ($(tr).prev().html() == null) {
                            layer.msg("已经是最顶部了");
                            return;
                        }
                        /*else {
                            // 未上移前，记录本行和上一行的数据
                            var tem = tbData[tr.index()];
                            var tem2 = tbData[tr.prev().index()];
                            // 将本身插入到目标tr之前
                            $(tr).insertBefore($(tr).prev());
                            // 上移之后，数据交换
                            tbData[tr.index()] = tem;
                            tbData[tr.next().index()] = tem2;
                        }*/
                    }
                    else if (updown == "down") {
                        if ($(tr).next().html() == null) {
                            layer.msg("已经是最底部了");
                            return;
                        }
                        /*else {
                            // 记录本行和下一行的数据
                            var tem = tbData[tr.index()];
                            var tem2 = tbData[tr.next().index()];
                            // 将本身插入到目标tr的后面
                            $(tr).insertAfter($(tr).next());
                            // 交换数据
                            tbData[tr.index()] = tem;
                            tbData[tr.prev().index()] = tem2;
                        }*/
                    }
                    $.ajax({
                        url: 'SMM_OrderProductLineProcess.ashx',
                        data: { 'id': idStr, 'updown': updown, 'ordernum': ordernum, 'PLid': condition },
                        dataType: 'text',
                        //contentType: 'application/json',
                        type: 'POST',
                        async: true,
                        cache: false,
                        success: function (returndata) {
                            if (returndata == 'OK') {
                                table.reload('InfoListReload', {});
                            } else {
                                layer.alert('更新排序失败!,Message：' + returndata, { icon: 5 });
                            }
                        },
                        error: function () {
                            layer.alert("请检查数据库连接!", { icon: 2 });
                        }
                    });
                }
                form.render();
            });
        }
    </script>

    <script type="text/html" id="orderOprate">
        <a class="layui-btn layui-btn-xs" lay-event="up" title="上移"><img src="../Images/up.png" style="width: 15px;height: 15px;" /></a>
        <a class="layui-btn layui-btn-xs" lay-event="down" title="下移"><img src="../Images/down.png" style="width: 15px;height: 15px;" /></a>
    </script>

    <script type="text/html" id="Operate">
        <a class="layui-btn layui-btn-danger layui-btn-xs" style="font-weight:900;" lay-event="del" title="删除">X</a>
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
    <input runat="server" type="hidden" id="PLid" value="" />
    <div style="width:100%;text-align:center;margin:20px auto;">
        <h2 runat="server" id="h2" style="color:#666666;"></h2>
    </div>
    <div id="div_Table" style="width:99%;margin:0 auto;">
        <table id="ProcessList" class="layui-hide" style="width:100%;" lay-filter="ProcessList"></table>
    </div>
</body>
</html>
