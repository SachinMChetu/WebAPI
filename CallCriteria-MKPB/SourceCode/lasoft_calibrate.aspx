<%@ Page Language="VB" AutoEventWireup="false" CodeFile="lasoft_calibrate.aspx.vb" Inherits="lasoft_review" %>

<!doctype html>

<html lang="en">
<head runat="server">
  <meta charset="utf-8">
  <title>Call Criteria - Listen</title>
</head>

<body>
  <div id="lasoft-mounting-point"></div>
    <script src="/app/v<%=current_version %>/bundle.js"></script>
  <%--<script src="https://lasoft-review-app.herokuapp.com/v<%=current_version %>/bundle.js"></script>--%>
  <script>
      (function () {
          var calibrate_app_config = {
              mounting_element_id: 'lasoft-mounting-point'
          };
          lasoft.calibrate_app.start(calibrate_app_config);
      })();
  </script>
</body>
</html>