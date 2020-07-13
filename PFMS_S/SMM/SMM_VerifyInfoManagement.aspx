<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_VerifyInfoManagement.aspx.cs" Inherits="PFMS_S.SMM.SMM_VerifyInfoManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网验收信息维护</title>
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
                htmlStr += "<button id=\"btnAdd\" type=\"button\" title=\"新增\" class=\"layui-btn layui-btn-sm\" onclick=\"AddSVerifyForm(0)\"><i class=\"layui-icon\">+</i></button>";
                $("#btnAddArea").append(htmlStr);
            }
            
            GetInformation();
        }
        //获取钢网信息清单
        function GetInformation() {
            var item = document.getElementById("inputPower").value;
            layui.use(['table','layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                var data_tr;
                table.render({
                    elem: '#InfoList',
                    url: 'SMM_GetVerifyInfoList.ashx',
                    title: '钢网验收表单',
                    cols: [[   
                        { field: 'status', title: '选择', width: 80, templet: '#selectItem', unresize: true, align: 'center'},          
                        { field: 'id', title: 'ID', width: 80, unresize: true, align: 'center' },
                        { field: 'order_num', title: '排序', width: 120, toolbar: '#orderOprate', unresize: true, align: 'center', hide: (item == 2) ? false : true },
                        { field: 'verify_info', title: '验收信息名称', align: 'center' },
                        { field: 'verify_type', title: '验收信息类型', align: 'center' },
                        { field: 'select_value', title: '选项值', align: 'center' },
                        { field: 'check_type', title: '校对类型', align: 'center' },
                        { field: 'standard_value', title: '校对标准', align: 'center' },
                        { field: 'remark', title: '备注', align: 'center' },
                        { field: 'required', title: '是否必填', width: 110, templet: '#switchStatus', unresize: true, align: 'center' },
                        { field: 'oprate', title: '操作', width: 120, toolbar: '#oprate', unresize: true, align: 'center', hide: (item == 2) ? false : true }
                    ]],
                    id: 'InfoListReload',
                    page: false,
                    done: function (res, curr, count) {
                        this.where = {};
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
                                switchArray[i].prop("title", "无权限");
                            }
                        }
                        //用户无权限时设置选择项是否启用复选框不可用
                        var ItemArray = document.getElementsByName("status"); 
                        for (var i = 0; i < ItemArray.length; i++) {
                            if (item !== '2') {
                                ItemArray[i].disabled = true;
                                ItemArray[i].style.cursor = "not-allowed";
                                ItemArray[i].prop("title", "无权限");
                            }
                        }
                        //用户无权限时设置全选复选框不可用
                        if (item !== '2') {
                            var selectAll = document.getElementById("selectAll");
                            selectAll.disabled = true;
                            selectAll.style.cursor = 'not-allowed';
                            selectAll.prop("title", "无权限");
                        }
                    }
                });

                //监听行工具条事件,用来删除或编辑记录以及行内操作
                table.on('tool(InfoList)', function (obj) {
                    ////console.log(obj)
                    data_tr = $(this);
                    var data = obj.data;
                    if (obj.event === 'del') {
                        if (data.verify_info == "钢网张力测试A点" || data.verify_info == "钢网张力测试B点"
                            || data.verify_info == "钢网张力测试C点" || data.verify_info == "钢网张力测试D点"
                            || data.verify_info == "钢网张力测试E点" || data.verify_info == "钢网张力测试F点"
                            || data.verify_info == "钢网张力测试G点" || data.verify_info == "钢网张力测试H点"
                            || data.verify_info == "钢网张力测试I点" || data.verify_info == "钢网张力测试J点") {
                            layer.alert("\"" + data.verify_info + "\"为验收表单保留字段，不可删除!");
                        }
                        else{
                            layer.confirm('真的删除验收信息(' + obj.data.verify_info + ')么？', function (index) {
                                $.ajax({
                                    url: 'SMM_DelVerifyInfo.ashx',
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
                        AddSVerifyForm(obj.data.id);
                    }
                    else if (obj.event === "up") {
                        MoveTableRowUpDown(obj.data.id,'up',obj.data.order_num);
                    }
                    else if (obj.event === "down") {
                        MoveTableRowUpDown(obj.data.id,'down', obj.data.order_num);
                    }
                });

                //控制table行上移或下移，并更新数据库排序
                function MoveTableRowUpDown(idStr, updown, ordernum) {
                    var tbData = table.cache.InfoListReload; //是一个Array
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
                        url: 'SMM_OrderVerifyInfo.ashx',
                        data: { 'id': idStr, 'updown': updown, 'ordernum': ordernum },
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
                        url: 'SMM_EditVerifyInfoRequired.ashx',
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
                        url: 'SMM_EditVerifyInfoStatus.ashx',
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
                        url: 'SMM_EditAllVerifyInfoStatus.ashx',
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

        function AddSVerifyForm(idStr) {
            layui.use("layer", function () {
                var layer = layui.layer;
                layer.open({
                    type: 2,
                    title: '钢网验收表单编辑',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['550px', '470px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'SMM_AddEditVerifyForm.aspx?id=' + idStr,
                    end: function () {
                        GetInformation();
                    }
                });
            });
        }
    </script>

    <script type="text/html" id="switchStatus">
        <input type="checkbox" name="required" value="{{d.id}}" lay-skin="switch" lay-text="必填|非必填" lay-filter="switchRequired" {{ d.required == 1 ? 'checked' : '' }} />
    </script>

    <script type="text/html" id="selectItem">
        <input type="checkbox" name="status" value="{{d.id}}" lay-skin="primary" lay-filter="switchStatus" {{ d.status == 1 ? 'checked' : '' }} />
    </script>

    <script type="text/html" id="orderOprate">
        <a class="layui-btn layui-btn-xs" lay-event="up" title="上移"><img src="../Images/up.png" style="width: 15px;height: 15px;" /></a>
        <a class="layui-btn layui-btn-xs" lay-event="down" title="下移"><img src="../Images/down.png" style="width: 15px;height: 15px;" /></a>
    </script>

    <script type="text/html" id="oprate">
        <a class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>
        <a class="layui-btn layui-btn-danger layui-btn-xs" lay-event="del">删除</a>
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <input runat="server" type="hidden" id="inputPower" value="" />
        <div style="width:100%;text-align:center;margin:20px auto;">
            <h2 style="color:#666666;">钢网验收表单管理</h2>
        </div>
        <div id="div_Table" style="width:99%;margin:0 auto;">
            <div style="text-align:right; width:100%;">
                <div id="btnAddArea"></div>
            </div>
            <table id="InfoList" class="layui-hide" style="width:100%;" lay-filter="InfoList"></table>
            <div style="text-align:left;margin-left: 32px;"><input type="checkbox" id="selectAll" name="selectAll" lay-skin="primary" lay-filter="selectAll"/>全选</div>
            <div style="text-align:right; width:100%;">
                <div><a href="SMM_AddEditDetailVerifyForm.aspx?id=0" target="_blank" style="color: blue; text-decoration:underline; cursor: pointer;">预览表单</a>&nbsp;</div>
            </div>
        </div>
        <br />
    </form>
</body>
</html>
