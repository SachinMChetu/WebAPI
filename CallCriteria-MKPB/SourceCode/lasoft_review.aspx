<%@ Page Language="VB" AutoEventWireup="false" CodeFile="lasoft_review.aspx.vb" Inherits="lasoft_review" %>

<!doctype html>

<html lang="en">
<head runat="server">
<meta charset="utf-8">
<title>Call Criteria - Review</title>
<style type="text/css">
  @import url(/app/fonts/roboto/stylesheet.css?family=Roboto:100,100i,300,300i,400,400i,500,500i,700,700i,900,900i);
  @import url(/app/v<%=current_version %>/styles.css);
  @import url(/app/styles/_datepicker.css);
  @import url(/app/styles/react-selectize.css);  
</style>
<script src="/app/v<%=current_version %>/review.js"></script>
</head>

<body>   
  <div id="lasoft-mounting-point"></div>
  
  <%--<script src="https://lasoft-review-app.herokuapp.com/v<%=current_version %>/bundle.js"></script>--%>
  <script>
    (function () {
      var review_app_config = {
        mounting_element_id:'lasoft-mounting-point',
        record_id: '<%=request("ID")%>'
        <%if uSettings IsNot Nothing OR uSettings <> "" then  %>
            ,initialData:<%=uSettings %>
        <%end if %>   
      };
      lasoft.review_app.start(review_app_config);
    })();
  </script>
</body>
</html>