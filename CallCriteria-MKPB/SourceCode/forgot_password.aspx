<%@ Page Language="VB" AutoEventWireup="false" CodeFile="forgot_password.aspx.vb" Inherits="forgot_password" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Basic Page Needs
    ================================================== -->
    <meta charset="utf-8" />
    <title>Call Criteria</title>
    <meta name="description" content="" />
    <meta name="author" content="" />
    <!--<meta name="viewport" content="width=device-width; initial-scale=1; maximum-scale=1">-->

    <!--[if lt IE 9]>
    <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->

    <!-- CSS ================================================== -->

    <link href='http://fonts.googleapis.com/css?family=Open+Sans:100,400,300,600,700,800' rel='stylesheet' type='text/css' />

    <link rel="stylesheet" href="css/style.css" type="text/css" />
    <link rel="stylesheet" href="css/fonts.css" type="text/css" />
    <link rel="stylesheet" href="css/font-awesome.css" type="text/css" />

    <script src="http://code.jquery.com/jquery-1.10.2.min.js"></script>

    <!-- Pick-a-Date resources -->
    <link rel="stylesheet" href="css/pickadate/default.css" type="text/css" />
    <link rel="stylesheet" href="css/pickadate/default.date.css" type="text/css" />
    <link rel="stylesheet" href="css/pickadate/default.time.css" type="text/css" />
    <script src="js/pickadate/compressed/picker.js"></script>
    <script src="js/pickadate/compressed/picker.date.js"></script>

    <!-- Custom scroll resources -->
    <link rel="stylesheet" href="css/custom-scrollbar/jquery.mCustomScrollbar.css" type="text/css" />
    <script src="js/custom-scrollbar/jquery.mousewheel.min.js"></script>
    <script src="js/custom-scrollbar/jquery.mCustomScrollbar.min.js"></script>

    <!-- Flot Charts resources -->
    <script type="text/javascript" src="js/flot/excanvas.min.js"></script>
    <script type="text/javascript" src="js/flot/jquery.flot.js"></script>
    <script type="text/javascript" src="js/flot/jquery.flot.pie.js"></script>

    <!-- Main script -->
    <script type="text/javascript" src="js/main.js"></script>

</head>
<body class="login-body">
    <form id="form1" runat="server">
        <div class="login-panel">
            <hgroup>
                <h1>
                    <img src="/images/CallCriteriaLogo.png" height="75" /></h1>
                <h2>Enter your user name below.  Your password will be reset and sent to your email address</h2>
            </hgroup>

            <div class="login-fields">
                <div class="top-field">
                    <i class="fa fa-user"></i>
                    <asp:TextBox ID="UserName" placeholder="Username or Email..." runat="server"></asp:TextBox>
                </div>

            </div>
            <!-- close login-fields -->

            <div class="login-actions">
                <asp:Button runat="server" ID="btnGetPassword" CssClass="main-cta add-category-btn" Text="Send Password"></asp:Button>
            </div>
            <!-- close login-actions -->

        </div>

    </form>
</body>
</html>
