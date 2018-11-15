<%@ Page Language="VB" AutoEventWireup="false" CodeFile="badcalls.aspx.vb" Inherits="badcalls" %>

<html lang="en">
<head>
    <meta charset="utf-8">
    <style type="text/css">
        @import url(/app/fonts/roboto/stylesheet.css?family=Roboto:100,100i,300,300i,400,400i,500,500i,700,700i,900,900i);
        @import url(/app/v<%=current_version %>/styles.css);
        @import url(/app/styles/_datepicker.css);
        @import url(/app/styles/react-selectize.css);  
    </style>
    <title>Call Criteria - Bad Calls Wizard</title>
</head>

<body>
    <div id="lasoft-mounting-point"></div>
     <script src="/app/v<%=current_version %>/bundle.js"></script>
    <%--<script src="https://lasoft-review-app.herokuapp.com/v<%=current_version%>/bundle.js"></script>--%>
    <%--<script src="/bundle/bundle.js?<%=current_version%>"></script>--%>
    <script>
        (function () {
            var badcalls_app_config = {
                mounting_element_id: 'lasoft-mounting-point'
            };
            lasoft.badcalls_app.start(badcalls_app_config);
        })();
    </script>
</body>
</html>
