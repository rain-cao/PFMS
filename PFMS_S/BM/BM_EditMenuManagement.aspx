<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BM_EditMenuManagement.aspx.cs" Inherits="PFMS_S.BM.BM_EditMenuManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>目录菜单管理</title>
    <link rel="icon" href="../Images/iconTitle.ico" />
    <link rel="stylesheet" type="text/css" href="../layui/css/layui.css" />
    <link rel="stylesheet" type="text/css" href="../CSS/StyleDefault.css" />
    <script src="../js/jquery-3.4.1.js" charset="utf-8"></script>
    <script src="../js/jquery-3.4.1.min.js" charset="utf-8"></script>
    <script src="../layui/layui.js"></script>
    <script type="text/javascript">
        //页面加载时设置侧边菜单栏高度
        window.onload = function () {
            resizeForm();
            InitMainMenu();
        }

        //查询用户权限下第一阶菜单列表,并显示
        function InitMainMenu() {
            layui.use(['layer', 'form'], function () {
                var layer = layui.layer;
                var form = layui.form;
                var idx = layer.load();
                $.ajax({
                    type: 'POST',
                    url: 'BM_GetMenuAndPower.ashx',
                    dataType: 'json',
                    //contentType: 'application/json',    //默认为"application/x-www-form-urlencoded"
                    data: { "FirstOrNext": "0", "menuID": "0", "user_id": document.getElementById("userid").value, "menuCtl": "1" },
                    async: true,
                    cache: false,
                    success: function (data) {
                        if (data.Result == 1) {
                            if (data.Msg != "")
                                $("#div_main_menu_list").append(data.Msg);
                            else
                                layer.alert("未查询到任何有效的主目录!", { icon: 5 });
                        }
                        else {
                            layer.alert(data.Msg, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert("查询主目录失败失败!", { icon: 5 });
                    }
                });
                layer.close(idx);
                form.render();
            });
        }

        //查询指定第一阶菜单内用户权限下所有阶菜单列表，并显示
        function InitNextMenu(obj, menuID) {
            //改变所选中的主目录的背景色，并重置其它主目录的背景
            /*$("span").each(function () {
                if (this === obj) {
                    $(this).siblings().removeClass("pad");
                    $(this).addClass("pad");
                }
                //$(this).click(function () {
                    //$(this).siblings().removeClass("pad");
                    //$(this).addClass("pad");
                //});
            });*/
            //查询用户权限下主目录以下所有目录及菜单并显示
            layui.use(['layer'], function () {
                var layer = layui.layer;
                var idx = layer.load();
                $.ajax({
                    type: 'POST',
                    url: 'BM_GetMenuAndPower.ashx',
                    dataType: 'json',
                    //contentType: 'application/json',    //默认为"application/x-www-form-urlencoded"
                    data: { "FirstOrNext": "1", "menuID": menuID, "user_id": document.getElementById("userid").value, "menuCtl": "1" },
                    async: true,
                    cache: false,
                    success: function (data) {
                        if (data.Result == 1) {
                            $("#div_left_menu_list").empty();
                            if (data.Msg != "") {
                                $("#div_left_menu_list").append(data.Msg);
                            }
                            else
                                layer.alert("未查询到任何有效的次级目录或菜单!", { icon: 5 });
                        }
                        else {
                            layer.alert(data.Msg, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert("查询左侧目录与菜单失败!", { icon: 5 });
                    }
                });
                layer.close(idx);
            });
        }

        //点击菜单时，被点击的菜单文字呈橘色，其它菜单文字为白色
        function aClick(obj, idStr) {
            $("a").each(function () {
                if (this == obj)
                    $(this).css({ "color": "#FE6229" });
                else
                    $(this).css({ "color": "white" });
            });
            var item = document.getElementById(idStr);
            if (item.value != "") {
                var item1 = document.getElementById("iframe_main");
                item1.src = item.value;
            }
        }

        //页面大小改变时设置指定模块高度、宽度属性
        function resizeForm() {
            /*changecss('.leftStyle', 'height', ($(window).height() - 80).toString() + "px");*/          //页面大小改变时，设置左侧菜单栏DIV高度适应文本显示区域高度
            /*document.getElementById("div_left_menu_list").style.height = ($(window).height() - 80).toString() + "px";*/   //页面大小改变时，设置左侧菜单栏DIV高度适应文本显示区域高度
            changecss('.rightStyle', 'height', ($(window).height() - 80).toString() + "px");         //页面大小改变时，设置右侧内容页DIV高度适应文本显示区域高度
            changecss('.iframe_main', 'height', ($(window).height() - 80).toString() + "px");        //页面大小改变时，设置内容页框架iframe高度适应文本显示区域高度
            var obj = document.getElementById("div_left_menu");
            if (obj.style.display == "none") {
                changecss('.rightStyle', 'width', ($(window).width()).toString() + "px");             //页面大小改变时，当左侧菜单栏不显示时，设置右侧内容页宽度
            }
            else {
                changecss('.rightStyle', 'width', ($(window).width() - 180).toString() + "px");       //页面大小改变时，当左侧菜单栏显示时，设置右侧内容页宽度
            }
            changecss('.div_main_menu', 'width', ($(window).width() - 180).toString() + "px");
        }

        //设置外联CSS属性函数
        function changecss(theClass, element, value) {
            var cssRules;
            if (document.all) {
                cssRules = 'rules';
            }
            else if (document.getElementById) {
                cssRules = 'cssRules';
            }
            for (var S = 0; S < document.styleSheets.length; S++) {
                for (var R = 0; R < document.styleSheets[S][cssRules].length; R++) {
                    if (document.styleSheets[S][cssRules][R].selectorText == theClass) {
                        document.styleSheets[S][cssRules][R].style[element] = value;
                        return;
                    }
                }
            }
        }

        //点击控制左侧菜单栏显示或影藏
        function Show_Hide_Menu() {
            var obj = document.getElementById("div_left_menu");
            if (obj.style.display == "none") {
                obj.style.display = "block";
                changecss('.rightStyle', 'margin-left', "180px");    //当左侧菜单栏显示时，设置右侧内容页向右偏移180个像素点
                changecss('.rightStyle', 'width', ($(window).width() - 180).toString() + "px");    //当左侧菜单栏显示时，设置内容页的宽度
            }
            else {
                obj.style.display = "none";
                changecss('.rightStyle', 'margin-left', "0px");    //当左侧菜单栏不显示时，设置右侧内容页向右偏移0个像素点
                changecss('.rightStyle', 'width', ($(window).width()).toString() + "px");     //当左侧菜单栏不显示时，设置内容页的宽度
            }
        }

        //点击控制对应目录下的子目录及菜单影藏或显示
        function catalogueClick(idObj) {
            var item = document.getElementById(idObj);
            if (item.style.display == "none")
                item.style.display = "block";
            else
                item.style.display = "none";
        }

        function addNextMenu(menuID) {
            layui.use(['layer'], function () {
                var layer = layui.layer;
                layer.open({
                    type: 2,
                    title: '新增次阶目录或菜单',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['500px', '500px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'BM_AddNextMenu.aspx?thisMenu=' + menuID,
                    end: function () {
                        location.reload(true);
                    }
                });
            });
        }

        function addMainMenu() {
            layui.use(['layer'], function () {
                var layer = layui.layer;
                layer.open({
                    type: 2,
                    title: '新增主目录',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['500px', '350px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'BM_AddMainMenu.aspx',
                    end: function () {
                        location.reload(true);
                    }
                });
            });
        }

        function updateMenu(menuID) {
            layui.use(['layer'], function () {
                var layer = layui.layer;
                layer.open({
                    type: 2,
                    title: '更新目录或菜单',
                    skin: "layui-layer-lan",
                    shadeClose: true,
                    shade: 0.8,
                    area: ['500px', '500px'],
                    fixed: true, //固定
                    maxmin: false,
                    content: 'BM_UpdateMenu.aspx?thisMenu=' + menuID,
                    end: function () {
                        location.reload(true);
                    }
                });
            });
        }

        function moveMenu(menuID, updown) {
            layui.use(['layer'], function () {
                var layer = layui.layer;
                $.ajax({
                    url: 'BM_MoveMenu.ashx',
                    data: { 'menuID': menuID, 'updown': updown },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.msg("目录顺序更新成功!");
                            location.reload(true);
                        } else {
                            layer.alert('删除失败!,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert("请检查数据库连接!", { icon: 2 });
                    },
                    complete: function () {
                    }
                });
            });
        }

        function setMenuStatus(menuID, statusNum) {
            layui.use(['layer'], function () {
                var layer = layui.layer;
                $.ajax({
                    url: 'BM_SetMenuStatus.ashx',
                    data: { 'menuID': menuID, 'statusNum': statusNum },
                    dataType: 'text',
                    //contentType: 'application/json',
                    type: 'POST',
                    async: true,
                    cache: false,
                    success: function (returndata) {
                        if (returndata == 'OK') {
                            layer.msg("状态更新成功!");
                            location.reload(true);
                        } else {
                            layer.alert('删除失败!,Message：' + returndata, { icon: 5 });
                        }
                    },
                    error: function () {
                        layer.alert("请检查数据库连接!", { icon: 2 });
                    },
                    complete: function () {
                    }
                });
            });
        }
    </script>

    <style>
          .dropdown-content
          {
            display:none;
            position:absolute;
            background-color: #2F4050;
            min-width:150px;
            box-shadow:0px 8px 16px 0px rgba(0,0,0,0.2);
          }
  
          .dropdown-content a
          {
            color:white;
            padding:12px 16px;
            text-decoration:none;
            display:block;
            font-weight:bold;
            text-align:center;
          }
  
          .dropdown-content a:hover {
              background-color: #C15A34;
          }
  
          .dropdown:hover .dropdown-content
          {
            display:block;
          }
  
  </style>
</head>
<body onresize="resizeForm()" style="overflow:auto;background-color: #F3F3F4;">
    <form id="form1" runat="server">
        <div class="topStyle">
            <input runat="server" id="userid" type="hidden" value="" />
            <div class="div_hide_menu" onclick="Show_Hide_Menu()">
                <img class="img_hide_menu" src="../Images/hide_menu.png" />
            </div>
            <div class="div_title">
                <h4 style="font-weight: 900; color: white;">智能化事务管控系统</h4>
            </div>
            <div class="div_main_menu">
                <div id="div_main_menu_list" style="float:left;height:80px;">                    
                </div>               
            </div>
        </div>
        <div id="div_left_menu" class="leftStyle">
            <!--div style="width:180px;height:40px;line-height:40px;">
                <div style="text-align:left;margin-left:15px;">
                    <img src="../Images/user.png" style="width:15px;height:15px;" /><label id="userShow" runat="server" style="color:white;" ></label>
                </div>
            </div-->
            <div id="div_left_menu_list" style="width:180px;height:auto;text-align:left;">
            </div>
        </div>
        <div class="rightStyle">
            <iframe src="" id="iframe_main" class="iframe_main"></iframe>
        </div>
    </form>
</body>
</html>
