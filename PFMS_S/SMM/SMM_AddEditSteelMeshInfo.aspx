<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_AddEditSteelMeshInfo.aspx.cs" Inherits="PFMS_S.SMM.SMM_AddEditSteelMeshInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>钢网信息编辑</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" href="../layui/css/layui.css" />
    <link rel="stylesheet" href="../CSS/StyleUserRegister.css" />
    <script src="../js/jquery-3.4.1.js"></script>
    <script src="../js/jquery-3.4.1.min.js"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        window.onload = function () {

        }
        //Layui预加载
        layui.use(['form', 'layer', 'laydate','form'], function () {
            //操作对象
            var form = layui.form;
            var layer = layui.layer;
            var laydate = layui.laydate;
            var form = layui.form;

            //鼠标移到Select选择项时tip弹框
            /*var $ = layui.$;
            $('.layui-form').on('mouseenter', '.layui-form-select dd', function () {
                var that = this, elem = $(that), title = elem.text();
                var index = layer.tips(title, that);
                elem.on('mouseleave', function () {
                    layer.close(index);
                });
            })*/

            //定义日期时间选择器
            laydate.render({
                elem: '#verifydate',
                theme: 'molv'
            });
            //自定义验证信息
            form.verify({
                sn: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网编号!";
                    }
                },
                pn: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网料号!";
                    }
                },
                name: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网名称!";
                    }
                },
                size: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网编号!";
                    }
                },
                thickness: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网厚度!";
                    }
                },
                material: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择钢网材料!";
                    }
                },
                technology: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择钢网工艺!";
                    }
                },
                ladder: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择钢网阶梯!";
                    }
                },
                imposition: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入拼版数量!";
                    }
                },
                treatment: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择钢网表面处理方式!";
                    }
                },
                printing: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择印刷面!";
                    }
                },
                printcount: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入单板印刷次数!";
                    }
                },
                leaded: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择是否含铅!";
                    }
                },
                clean: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择清洗类型!";
                    }
                },
                manufacturer: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择钢网供应商!";
                    }
                },
                custormer: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网适用产品供应的客户!";
                    }
                },
                verifydate: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择验收日期!";
                    }
                },
                audit: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择审核规则!";
                    }
                },
                intervalT: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网清洗间隔时间!";
                    }
                },
                intervalC: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请输入钢网清洗间隔次数!";
                    }
                },
                storage: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择存放储位!";
                    }
                },
                engineer: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择工程人员!";
                    }
                },
                engineer: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择工程人员!";
                    }
                },
                warehousing: function (value) {
                    if ($.trim(value).length < 1) {
                        return "请选择入库人员!";
                    }
                }
            });
            //根据筛选器监听对select选中事件
            form.on('select(deptE)', function (data) {
                MatchPersonnel(data.value, 'engineer');
            });
            form.on('select(deptW)', function (data) {
                MatchPersonnel(data.value, 'warehousing');
            });
            //Select标签联动，通过选择部门来查询该部门人员名单
            function MatchPersonnel(dptIDStr, selectID) {
                $.ajax({
                    url: 'SMM_GetPersonnelByDept.ashx',
                    data: { 'id': dptIDStr },
                    dataType: 'json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (data) {
                        if (data.flag == true) {
                            var item = document.getElementById(selectID);
                            item.options.length = 0;
                            item.appendChild(new Option("人员...", ""));
                            for (var i = 0, l = data.data.length; i < l; i++) {
                                item.appendChild(new Option(data.data[i].name + "(" + data.data[i].username + ")", data.data[i].user_id));
                            }
                            form.render('select', selectID);
                        }
                        else {
                            layer.alert('依部门查询人员失败,Message：' + data.msg, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('依部门查询人员失败,请确认数据库连接!', { icon: 5 });
                    }
                });
            }
            //通过点击是否填写验证表单开关来设置表单填写入口是否可显
            form.on('checkbox(verifyform)', function (data) {
                if (this.checked)
                    document.getElementById('form_entrance').style.display = "inline-block";
                else
                    document.getElementById('form_entrance').style.display = "none";
            });

            //Form提交事件，将注册信息写入数据库
            form.on('submit(submit_action)', function (data) {
                //1.当验收表单必填开关开启时，首先弹出提示框
                var confirmForm = 0;
                if (document.getElementById('verifyform').checked) {
                    layer.confirm('验收表单是否已填写？', function (index) {
                        confirmForm = 1;
                        layer.close(index);
                    });
                }
                else
                    confirmForm = 1;

                if (confirmForm == 1) {
                    //2.当验收表单已填时，再将钢网信息写入数据库进行入库操作
                    $.ajax({
                        url: 'SMM_AddEditSteelMeshInfoSubmit.ashx',
                        data: {
                            'submit': 'Submit',
                            'input_id': data.field.input_id,
                            'sn': data.field.sn,
                            'pn': data.field.pn,
                            'name': data.field.name,
                            'size': data.field.size,
                            'thickness': data.field.thickness,
                            'material': data.field.material,
                            'technology': data.field.technology,
                            'ladder': data.field.ladder,
                            'imposition': data.field.imposition,
                            'treatment': data.field.treatment,
                            'printing': data.field.printing,
                            'printcount': data.field.printcount,
                            'leaded': data.field.leaded,
                            'clean': data.field.clean,
                            'manufacturer': data.field.manufacturer,
                            'custormer': data.field.custormer,
                            'verifydate': data.field.verifydate,
                            'audit': data.field.audit,
                            'intervalT': data.field.intervalT,
                            'intervalC': data.field.intervalC,
                            'storage': data.field.storage,
                            'engineer': data.field.engineer,
                            'warehousing': data.field.warehousing,
                            'verifyform': document.getElementById('verifyform').checked ? '1' : '0'
                        },
                        dataType: 'text',
                        //contentType: 'application/json',
                        type: 'POST',
                        async: true,
                        cache: false,
                        success: function (returndata) {
                            if (returndata.substr(0, 3) == 'OK:') {
                                layer.msg('钢网信息提交成功!');
                                document.getElementById('form_entrance').href = "SMM_AddEditDetailVerifyForm.aspx?id=" + returndata.substr(3);
                            } else {
                                layer.alert('钢网信息提交失败,Message：' + returndata, { icon: 5 });
                            }
                        },
                        error: function () {
                            layer.alert('钢网信息提交失败,请确认数据库连接!', { icon: 5 });
                        }
                    });
                }
                return false;
            });

            form.render();
        });

        //暂存钢网信息
        function SaveForFutherEdit(steelMeshIDStr) {
            layui.use('layer', function () {
                $.ajax({
                    url: 'SMM_AddEditSteelMeshInfoSubmit.ashx',
                    data: {
                        'submit': 'NoSubmit',
                        'input_id': steelMeshIDStr,
                        'sn': document.getElementById('sn').value,
                        'pn': document.getElementById('pn').value,
                        'name': document.getElementById('name').value,
                        'size': document.getElementById('size').value,
                        'thickness': document.getElementById('thickness').value,
                        'material': document.getElementById('material').value,
                        'technology': document.getElementById('technology').value,
                        'ladder': document.getElementById('ladder').value,
                        'imposition': document.getElementById('imposition').value,
                        'treatment': document.getElementById('treatment').value,
                        'printing': document.getElementById('printing').value,
                        'printcount': document.getElementById('printcount').value,
                        'leaded': document.getElementById('leaded').value,
                        'clean': document.getElementById('clean').value,
                        'manufacturer': document.getElementById('manufacturer').value,
                        'custormer': document.getElementById('custormer').value,
                        'verifydate': document.getElementById('verifydate').value,
                        'audit': document.getElementById('audit').value,
                        'intervalT': document.getElementById('intervalT').value,
                        'intervalC': document.getElementById('intervalC').value,
                        'storage': document.getElementById('storage').value,
                        'engineer': document.getElementById('engineer').value,
                        'warehousing': document.getElementById('warehousing').value,
                        'verifyform': document.getElementById('verifyform').checked ? '1' : '0'
                    },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata.substr(0, 3) == 'OK:') {
                            layer.msg('钢网信息暂存成功!');
                            document.getElementById('form_entrance').href = "SMM_AddEditDetailVerifyForm.aspx?id=" + returndata.substr(3);
                        } else {
                            layer.alert('钢网信息暂存失败,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert('钢网信息暂存失败,请确认数据库连接!', { icon: 5 });
                    }
                });
            });
        }

    </script>
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <h2 runat="server" id="h2Show">钢网信息管理</h2>
        <br />
        <div style="margin: 0 auto;text-align: center;width: 100%;">
            <%=outhtml()%>
        </div>
    </form>
</body>
</html>
