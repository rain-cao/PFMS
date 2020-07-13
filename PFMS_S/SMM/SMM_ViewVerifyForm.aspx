<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMM_ViewVerifyForm.aspx.cs" Inherits="PFMS_S.SMM.SMM_ViewVerifyForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钢网验收表单</title>
    <link rel="icon" href="../Images/iconTitle.ico"/>
    <link rel="stylesheet" href="../layui/css/layui.css" />
</head>
<body style="font-family:'Microsoft YaHei';">
    <form class="layui-form">
        <h2 runat="server" id="h2Show" style="text-align:center;">钢网验收表单</h2>
        <br />
        <div style="margin: 0 auto;text-align: center;width: 95%;">
            <%=outhtml()%>
        </div>
    </form>
</body>
</html>
