<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_PCBManagement.aspx.cs" Inherits="PFMS_S.BM.BM_PCBManagement" %>

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
            htmlStr += "&nbsp;<button id=\"btnSearch\" type=\"button\" class=\"layui-btn layui-btn-sm\" onclick=\"GetPCB()\">查询</button>";
            $("#div_Search").append(htmlStr);
            if (item == 2) {
                document.getElementById("div_Add").style.display = "block";
            }
            GetPCB();
        }
        //获取PCB料号清单
        function GetPCB() {
            var condition = document.getElementById("input-condition").value;
            var item = document.getElementById("inputPower").value;
            layui.use(['table','layer','form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
                table.render({
                    elem: '#PCBList',
                    url: 'BM_GetPCBList.ashx?condition=' + condition,
                    title: 'PCB清单',
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
                table.on('tool(PCBList)', function (obj) {
                    var data = obj.data;
                    //console.log(obj)
                    if (obj.event === 'del') {
                        layer.confirm('真的删除行么', function (index) {
                            $.ajax({
                                url: 'BM_DelPCB.ashx',
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
                });
                form.render();
            });
        } 

        function AddPCB() {
            layui.use(['layer', 'table'], function () {
                var layer = layui.layer;
                var table = layui.table;
                layer.open({
                    type: 2,
                    title: '添加PCB',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['500px', '200px'],
                    fixed: true, //不固定
                    maxmin: false,
                    content: 'BM_AddPCB.aspx',
                    end: function () {
                        //GetPCB();
                        table.reload('InfoListReload', {});
                    }
                });
            });
        }

        layui.use(['layer', 'upload', 'table'], function () {
            var layer = layui.layer;
            var upload = layui.upload;
            var table = layui.table;
            upload.render({
                elem: '#batchImport'
                , url: '../UploadFile.ashx'
                , data: {Floder:'TempFile'}
                , multiple: false
                , auto: true
                , accept: 'file'
                , acceptMime:'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                , exts: 'xls|xlsx'
                , drag: true
                , choose: function (obj) {
                    //var files = obj.pushFile();
                    //obj.preview(function (index, file, result) {
                        //console.log(index); //得到文件索引
                        //console.log(file); //得到文件对象
                        //console.log(result); //得到文件base64编码，比如图片

                        //obj.resetFile(index, file, '123.jpg'); //重命名文件名，layui 2.3.0 开始新增

                        //这里还可以做一些 append 文件列表 DOM 的操作

                        //obj.upload(index, file); //对上传失败的单个文件重新上传，一般在某个事件中使用
                        //delete files[index]; //删除列表中对应的文件，一般在某个事件中使用
                    //});
                }
                , before: function (obj) { //obj参数包含的信息，跟 choose回调完全一致，可参见上文。
                    layer.load(); //上传loading
                }
                /*, progress: function (n, elem) {
                    var percent = n + '%' //获取进度百分比
                    element.progress('demo', percent); //可配合 layui 进度条元素使用

                    //以下系 layui 2.5.6 新增
                    console.log(elem); //得到当前触发的元素 DOM 对象。可通过该元素定义的属性值匹配到对应的进度条。
                }*/
                , done: function (res, index, upload) {
                    if (res.data[0].uploadFlag) {
                        $.ajax({
                            url: 'BM_BathImportPCB.ashx',
                            data: { 'filePath': res.data[0].filePath, 'fileName': res.data[0].fileName },
                            dataType: 'text',
                            //contentType: 'application/json',
                            type: 'POST',
                            async: true,
                            cache: false,
                            success: function (returndata) {
                                if (returndata == 'OK') {
                                    layer.msg("数据导入成功!");
                                } else {
                                    layer.alert('数据导入失败!,Message：<br />' + returndata, { icon: 5 });
                                }
                                //GetPCB();
                                table.reload('InfoListReload', {});
                            },
                            error: function () {
                                layer.alert("数据导入失败，请检查数据库连接!", { icon: 2 });
                            },
                            complete: function () {
                                layer.closeAll('loading'); //关闭loading
                            }
                        });
                    }
                    else {
                        layer.closeAll('loading'); //关闭loading
                        layer.alert("文件上传失败，Message: " + res.data[0].failMsg, { icon: 5 });
                    }
                }
                , error: function (index, upload) {
                    layer.closeAll('loading'); //关闭loading
                    layer.alert("文件上传失败，请检查文件上传接口!", { icon: 2 });
                }
            });
        });
    </script>

    <script type="text/html" id="Operate">
        <a class="layui-btn layui-btn-danger layui-btn-xs" style="font-weight:900;" lay-event="del" title="删除">X</a>
    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <input runat="server" type="hidden" id="inputPower" value="" />
    <div id="div_Table" style="width:99%;margin:20px auto;">
        <div id="div_Search" style="text-align:center;width:100%;"></div>
        <div id="div_Add" style="text-align:right;width:100%;display:none;">
            <button id="btnAdd" type="button" title="新增" class="layui-btn layui-btn-sm" onclick="AddPCB()"><i class="layui-icon">+</i></button>
            <button type="button" class="layui-btn layui-btn-sm" id="batchImport" title="文件格式xls|xlsx,支持拖拽">批量导入</button>&nbsp;<a style="cursor: pointer; font-size: 12px; text-decoration:underline;color:blue;" href="batchImportPCB.xlsx">批量导入格式</a>
        </div>
        <table id="PCBList" class="layui-hide" style="width:100%;" lay-filter="PCBList"></table>
    </div>
</body>
</html>
