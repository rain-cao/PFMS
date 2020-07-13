<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_CustomInfo.aspx.cs" Inherits="PFMS_S.SteelMeshManagement.SMM_CustomInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网信息选择</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <script src="../layui/layui.js"></script>
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            GetInformation();
        }
        //获取钢网信息清单
        function GetInformation() {
            var item = document.getElementById("inputPower").value;
            layui.use(['table','layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                table.render({
                    elem: '#InfoList',
                    url: 'SMM_GetInfoList.ashx',
                    title: '钢网信息清单',
                    cols: [[ 
                        { field: 'status', title: '选择', width: 80, fixed: 'left', templet: '#selectItem', unresize: true, align: 'center', sort: true },
                        { field: 'id', title: 'ID', width: 80, unresize: true, sort: true, align: 'center' },
                        { field: 'info_name', title: '选项名称', align: 'center' },
                        { field: 'remark', title: '备注', align: 'center' },
                        { field: 'required', title: '是否必填', width: 110, templet: '#switchStatus', unresize: true, align: 'center', sort: true, fixed: 'right' }
                    ]],
                    page: false,
                    done: function (res, curr, count) {
                        //根据查询的选择项启动状态，状态是否全部为1来设置全选复选框是否勾选
                        var allSelect = 1;
                        for (var i = 0; i < res.count; i++) {
                            if (res.data[i]['status'] == '0') {
                                allSelect = 0;
                                break;
                            }
                        }
                        if (allSelect == 1) {
                            $("input[name='selectAll']").prop("checked", true);
                            form.render('checkbox');
                        }
                        //用户无权限时设置选择项是否必填开关不可用
                        var switchArray = document.getElementsByName("required");
                        for (var i = 0; i < switchArray.length; i++) {
                            if (item !== '2') {
                                switchArray[i].disabled = true;
                                switchArray[i].style.cursor = "not-allowed";
                            }
                        }
                        //用户无权限时设置选择项是否启用复选框不可用
                        var ItemArray = document.getElementsByName("status"); 
                        for (var i = 0; i < ItemArray.length; i++) {
                            if (item !== '2') {
                                ItemArray[i].disabled = true;
                                ItemArray[i].style.cursor = "not-allowed";
                            }
                        }
                        //用户无权限时设置全选复选框不可用
                        if (item !== '2') {
                            var selectAll = document.getElementById("selectAll");
                            selectAll.disabled = true;
                            selectAll.style.cursor = 'not-allowed';
                        }
                    }
                });
                form.render();
            });
        } 

        layui.use(['form','layer'], function () {
            var form = layui.form;
            var layer = layui.layer;
            var item = document.getElementById("inputPower").value;
            //切换switch开关来设置选项是否必填
            form.on('switch(switchRequired)', function () {
                if (item === '2') {
                    var obj = this;
                    var index;
                    $.ajax({
                        url: 'SMM_EditInfoRequired.ashx',
                        data: { 'id': this.value },
                        dataType: 'text',
                        //contentType: 'application/json',
                        type: 'POST',
                        async: true,
                        cache: false,
                        /*beforeSend: function () {
                            //弹出的lodinng层
                            index = layer.load(2, { shade: [0.5, "#333"] });
                        },*/
                        success: function (returndata) {
                            if (returndata != 'OK') {
                                layer.alert('编辑是否为必填项失败!,Message：' + returndata, { icon: 5 });
                                obj.checked = obj.checked ? false : true;
                                form.render('checkbox');
                            }
                        },
                        error: function () {
                            layer.alert("请检查数据库连接!", { icon: 2 });
                            obj.checked = obj.checked ? false : true;
                            form.render('checkbox');
                        }/*,
                        complete: function () {
                            layer.close(index);
                        }*/
                    });
                }
            });
            //点击复选框开关来设置选项是否启用
            form.on('checkbox(switchStatus)', function () {
                if (item === '2') {
                    var obj = this;
                    var index;
                    $.ajax({
                        url: 'SMM_EditInfoStatus.ashx',
                        data: { 'id': this.value },
                        dataType: 'text',
                        //contentType: 'application/json',
                        type: 'POST',
                        async: true,
                        cache: false,
                        /*beforeSend: function () {
                            //弹出的lodinng层
                            index = layer.load(2, { shade: [0.5, "#333"] });
                        },*/
                        success: function (returndata) {
                            if (returndata != 'OK') {
                                layer.alert('编辑是否启用选择项失败!,Message：' + returndata, { icon: 5 });
                                obj.checked = obj.checked ? false : true;
                                form.render('checkbox');
                            }
                            else {
                                //遍历所有选择项复选框，判断是否全部选中，来修改全选复选框的勾选状态
                                var ItemArray = document.getElementsByName("status");
                                var allSelect = 1;
                                for (var i = 0; i < ItemArray.length; i++) {
                                    if (ItemArray[i].value == obj.value)
                                        ItemArray[i].checked = obj.checked;
                                    if (ItemArray[i].checked == false)
                                        allSelect = 0;
                                }
                                if (allSelect == 1) {
                                    $("input[name='selectAll']").prop("checked", true);
                                }
                                else {
                                    $("input[name='selectAll']").prop("checked", false);
                                }
                                form.render('checkbox');
                            }
                        },
                        error: function () {
                            layer.alert("请检查数据库连接!", { icon: 2 });
                            obj.checked = obj.checked ? false : true;
                            form.render('checkbox');
                        }/*,
                        complete: function () {
                            layer.close(index);
                        }*/
                    });
                }
            });
            //点击全选复选框开关来设置所有选项是否启用
            form.on('checkbox(selectAll)', function () {
                if (item === '2') {
                    var obj = this;
                    var index;
                    $.ajax({
                        url: 'SMM_EditAllInfoStatus.ashx',
                        data: { 'status': this.checked ? '1' : '0' },
                        dataType: 'text',
                        //contentType: 'application/json',
                        type: 'POST',
                        async: true,
                        cache: false,
                        /*beforeSend: function () {
                            //弹出的lodinng层
                            index = layer.load(2, { shade: [0.5, "#333"] });
                        },*/
                        success: function (returndata) {
                            if (returndata != 'OK') {
                                layer.alert('编辑是否启用所有选择项失败!,Message：' + returndata, { icon: 5 });
                                obj.checked = obj.checked ? false : true;
                                form.render('checkbox');
                            }
                            else {
                                //点击全选复选框该表其它复选框的状态
                                var ItemArray = document.getElementsByName("status");
                                for (var i = 0; i < ItemArray.length; i++) {
                                    ItemArray[i].checked = obj.checked;
                                }
                                form.render('checkbox');
                            }
                        },
                        error: function () {
                            layer.alert("请检查数据库连接!", { icon: 2 });
                            obj.checked = obj.checked ? false : true;
                            form.render('checkbox');
                        }/*,
                        complete: function () {
                            layer.close(index);
                        }*/
                    });
                }
            });
        });
    </script>

    <script type="text/html" id="switchStatus">
        <input type="checkbox" name="required" value="{{d.id}}" lay-skin="switch" lay-text="必填|非必填" lay-filter="switchRequired" {{ d.required == 1 ? 'checked' : '' }} />
    </script>

    <script type="text/html" id="selectItem">
        <input type="checkbox" name="status" value="{{d.id}}" lay-skin="primary" lay-filter="switchStatus" {{ d.status == 1 ? 'checked' : '' }} />
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <input runat="server" type="hidden" id="inputPower" value="" />
        <div style="width:100%;text-align:center;margin:20px auto;">
            <h2 style="color:#666666;">钢网信息选择</h2>
        </div>
        <div id="div_Table" style="width:99%;margin:0 auto;">
            <table id="InfoList" class="layui-hide" style="width:100%;" lay-filter="InfoList"></table>
            <div style="text-align:left;margin-left: 32px;"><input type="checkbox" id="selectAll" name="selectAll" lay-skin="primary" lay-filter="selectAll" />全选</div>
        </div>
        <br />
    </form>
</body>
</html>
