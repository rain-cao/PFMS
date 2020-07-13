<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_Acceptance.aspx.cs" Inherits="PFMS_S.SMM.SMM_Acceptance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网验收管理</title>
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
                htmlStr = "<input id=\"input-condition\" type=\"text\" class=\"layui-input-inline\" style=\"width: 150px; height: 30px; text-transform:uppercase;\" lay-verify=\"SearchCondition\" autocomplete=\"off\" placeholder=\"钢网编号\" />";
                htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn\" onclick=\"GetAccpList()\">查询</button>";
            }
            else if (item == 2) {
                htmlStr = "<button id=\"btnAdd\" type=\"button\" class=\"layui-btn layui-btn-sm\" onclick=\"AddEditAccptance(0)\"><i class=\"layui-icon\">+</i></button>";
                htmlStr += "&nbsp;<input id=\"input-condition\" type=\"text\" class=\"layui-input-inline\" style=\"width: 150px; height: 30px; text-transform:uppercase;\" lay-verify=\"SearchCondition\" autocomplete=\"off\" placeholder=\"钢网编号\" />";
                htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn\" onclick=\"GetAccpList()\">查询</button>";
            }
            $("#div_Search").append(htmlStr);
            GetAccpList();
        }
        //获取已维护的储位清单
        function GetAccpList() {
            var condition = document.getElementById("input-condition").value;
            var item = document.getElementById("inputPower").value;
            layui.use(['layer','form'], function () {
                var layer = layui.layer;
                var form = layui.form;

                var ii = layer.load();
                $.ajax({
                    url: 'SMM_GetAccpList.ashx',
                    data: { 'condition': condition, 'power': item},
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata.substring(0, 3) != 'OK:') {
                            layer.alert('获取待验收钢网失败!,Message：' + returndata, { icon: 5 });
                        }
                        else {
                            returndata = returndata.substring(3);
                            $("#div_Table").empty();
                            $("#div_Table").append(returndata);
                            //document.getElementById("div_Table").innerHTML = returndata;
                        }
                    },
                    error: function () {
                        layer.alert("请检查数据库连接!", { icon: 2 });
                    }
                });
                layer.close(ii);
                form.render();
            });
        } 

        function ViewDeatil(obj, objID, ItemID) {
            layui.use(['layer'], function () {
                layer = layui.layer;
                $.ajax({
                    url: 'SMM_GetDetailAcceptance.ashx',
                    data: { 'id': ItemID },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata.substring(0, 3) != 'OK:') {
                            layer.alert('获取详细钢网信息失败!,Message：' + returndata, { icon: 5 });
                        }
                        else {
                            returndata = returndata.substring(3);
                            layer.open({
                                type: 1,
                                title: '详细钢网信息',
                                skin: 'layui-layer-rim', //加上边框
                                shadeClose: true,
                                shade: 0.1,
                                area: ['350px', '500'], //宽高
                                content: returndata
                            });
                        }
                    },
                    error: function () {
                        layer.alert("请检查数据库连接!", { icon: 2 });
                    }
                });
            });
        }

        function AddEditAccptance(ItemID) {
            layui.use(['layer'], function () {
                layer = layui.layer;
                layer.open({
                    type: 2,
                    title: (ItemID == 0) ? '新增钢网信息' : '编辑钢网信息',
                    skin: "layui-layer-lan",
                    shadeClose: false,
                    shade: 0.8,
                    area: ['1020px', '600px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'SMM_AddEditSteelMeshInfo.aspx?id=' + ItemID.toString(),
                    end: function () {
                        GetAccpList() ;
                    }
                });
            });
        }

        function DeleteThis(objContainerID, objID, ItemsID) {
            layui.use(['layer'], function () {
                layer = layui.layer;
                layer.confirm('真的删除行么', function (index) {
                    $.ajax({
                        url: 'SMM_DelAcceptance.ashx',
                        data: { 'id': ItemsID },
                        dataType: 'text',
                        //contentType: 'application/json',
                        type: 'POST',
                        async: true,
                        cache: false,
                        success: function (returndata) {
                            if (returndata != 'OK') {
                                layer.alert("删除待验收钢网失败!,Message：" + returndata, { icon: 5 });
                            }
                            else {
                                $("#" + objID).remove();
                                $("#" + objContainerID).remove();
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
            });
        }
    </script>

    <script type="text/html" id="Operate">
        <a class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>
        <a class="layui-btn layui-btn-danger layui-btn-xs" style="font-weight:900;" lay-event="del">删除</a>
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <input runat="server" type="hidden" id="inputPower" value="" />
    <div id="div_Search" style="width:100%;text-align:center;margin-top:20px;">
    </div>
    <div id="div_Table" style="width:99%;margin:0 auto;">
    </div>
</body>
</html>
