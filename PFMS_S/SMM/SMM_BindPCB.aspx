<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_BindPCB.aspx.cs" Inherits="PFMS_S.SMM.SMM_BindPCB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>PCB料号管理</title>
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
            htmlStr = "<input id=\"input-condition\" type=\"text\" class=\"layui-input-inline\" style=\"width: 150px; height: 30px;text-transform: uppercase;\" lay-verify=\"SearchCondition\" autocomplete=\"off\" placeholder=\"请输入PCB料号查询\" />";
            htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn layui-btn-sm\" onclick=\"GetWaitBind()\">查询</button>";
            $("#div_Search").append(htmlStr);
            GetWaitBind();
        }
        //获取PCB料号清单
        function GetWaitBind() {
            var condition = document.getElementById("input-condition").value;
            var item = document.getElementById("inputPower").value;
            layui.use(['table','layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                table.render({
                    elem: '#WaitBindList',
                    url: 'SMM_GetBindPCBList.ashx?condition=' + condition,
                    title: '待分配钢网PCB清单',
                    page: true,
                    limits: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
                    limit: 10,
                    request: {
                        pageName: 'curr', //页码的参数名称，默认：page
                        limitName: 'nums' //每页数据量的参数名，默认：limit
                    },
                    cols: [[
                        { field: 'id', title: 'ID', width: 80, unresize: true, sort: true, align: 'center' },
                        { field: 'pcb_pn', title: 'PCB料号', align: 'center' },
                        { fixed: 'right', title: '操作', toolbar: '#Operate', width: 80, unresize: true, align: 'center', hide: (item == 2) ? false : true }
                    ]],
                    id: 'InfoListReload',
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
                table.on('tool(WaitBindList)', function (obj) {
                    var data = obj.data;
                    //console.log(obj)
                    if (obj.event === 'edit') {
                        layer.open({
                            type: 2,
                            title: '新增钢网PCB绑定关系',
                            skin: "layui-layer-lan",
                            shadeClose: true,
                            shade: 0.8,
                            area: ['500px', '250px'],
                            fixed: true, //固定
                            maxmin: false,
                            content: 'SMM_AddBindPCB.aspx?id=' + data.id,
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
        <a style="font-weight:900;cursor:pointer;" lay-event="edit" title="编辑"><img style="background-color: transparent;border:none;" src="../Images/edit.png" /></a>
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <input runat="server" type="hidden" id="inputPower" value="" />
    <div id="div_Table" style="width:99%;margin:20px auto;">
        <div id="div_Search" style="text-align:center;width:100%;"></div>
        <table id="WaitBindList" class="layui-hide" style="width:100%;" lay-filter="WaitBindList"></table>
    </div>
</body>
</html>
