<%@ Page Language="VB" AutoEventWireup="false" CodeFile="lasoft_train.aspx.vb" Inherits="lasoft_review" %>

<!doctype html>

<html lang="en">
<head>
  <meta charset="utf-8">
  <title>Call Criteria - Training</title>
</head>

<body>
  <div id="lasoft-mounting-point"></div>
    <script src="/app/v<%=current_version %>/bundle.js"></script>
  <%--<script src="https://lasoft-review-app.herokuapp.com/v<%=current_version %>/bundle.js"></script>--%>
  <script>
      (function () {
          var train_app_config = {
              mounting_element_id: 'lasoft-mounting-point'
          };
          lasoft.train_app.start(train_app_config);
      })();
  </script>
</body>
</html>