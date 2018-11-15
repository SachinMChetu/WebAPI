<%@ Page Title="Application Form" Language="VB" AutoEventWireup="false" CodeFile="ApplicationForm.aspx.vb" Inherits="ApplicationForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <br />
        <div>
            <img src="http://app.callcriteria.com/img/cc_words_logo.png" alt="Call Criteria" width="322" height="50" />
        </div>

        <br />
        <h2>PERSONAL INFORMATION</h2>
        <hr />
        <table class="form">
            <tr>
                <td>FIRST NAME</td>
                <td><input type="text" id="firstname" runat="server" value="" /></td>
                <td class="divider"></td>
                <td>LAST NAME</td>
                <td><input type="text" id="lastname" runat="server" value="" /></td>
            </tr>
            <tr>
                <td>USERNAME</td>
                <td><input type="text" id="username" runat="server" value="" disabled="disabled" /></td>
                <td></td>
                <td>PASSWORD</td>
                <td><input type="password" id="password" runat="server" value="" required="required" /></td>
            </tr>
            <tr>
                <td>BIRTHDAY</td>
                <td><input type="text" id="bday" runat="server" value="" /></td>
                <td></td>
                <td>AGE</td>
                <td><input type="text" id="age" value="" disabled="disabled" /></td>
            </tr>
            <tr>
                <td>GENDER</td>
                <td>
                    <input type="radio" id="male" runat="server" name="gender" value="MALE" required="required" /><label for="male">MALE</label>
                    <input type="radio" id="female" runat="server" name="gender" value="FEMALE" /><label for="female">FEMALE</label>
                </td>
                <td></td>
                <td>EMAIL</td>
                <td><input type="text" id="emailadd" runat="server" value="" /></td>
            </tr>
        </table>
        <br />
        <table class="form">
            <tr>
                <td>RESUME UPLOAD <asp:FileUpload ID="file" runat="server" /> <span class="warning">Upload .doc .docx .pdf only.</span></td>
            </tr>
        </table>
        <br />
        <h2>QUESTIONS</h2>
        <hr />
        <table class="form">
            <asp:Repeater runat="server" ID="questions" DataSourceID="dsQuestions">
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("Question")%></td>
                    </tr>
                    <tr>
                        <td class="answer">
                            <asp:TextBox ID="Answer" qid='<%#Eval("ID")%>' TextMode="MultiLine" Columns="80" Rows="3" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="dsQuestions" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="SELECT ID, Question FROM ApplicantsQ WHERE Active = 1">
            </asp:SqlDataSource>
        </table>
        <br />
        <div class="right">
            <asp:Button Text="Submit" ID="submit" runat="server" />
        </div>
        <br />
        <br />
    </div>
    </form>

    <style>
        body {
            background-color: #EEE;
            background-image: url(../img/crossword-gray1.png);
            font-family: 'Open Sans', Arial, Helvetica, sans-serif;
            font-size: 14px;
            font-weight: 600;
            color: #666;
        }
        input[type=text], input[type=date], input[type=password], input[type=email], textarea {
            background-color: #FFF;
            border: 2px solid #CCC;
            border-radius: 3px;
            padding: 5px;
            font-family: 'Open Sans', Arial, Helvetica, sans-serif;
            font-size: 14px;
            font-weight: 600;
            color: #666;
        }
        input[type=text]:focus, input[type=date]:focus, input[type=password]:focus, input[type=email]:focus, textarea:focus {
            outline: 0;
            box-shadow: 0 0 2pt 1pt #666;
        }
        input[type=text]:disabled {
            background-color: #DDD;
        }
        input[type=submit] {
            background-color: #FFF;
            border: 2px solid #CCC;
            color: #666;
            border-radius: 6px;
            padding: 5px;
            font-size: 14px;
            font-weight: 600;
            min-width: 100px;
        }
        input[type="submit"]:hover {
            background-color: #666;
            color: #FFF;
            box-shadow: 0 0 2pt 1pt #666;
        }
        .main {
            width: 700px;
            margin: 0 auto;
        }
        .form {
            width: 100%;
        }
        .form td {
            padding: 5px;
        }
        .divider {
            width: 40px;
        }
        .form .answer {
            padding-left: 20px;
        }
        .right {
            float: right;
            padding-right: 10px;
        }
        .warning {
            color: #800000;
        }
    </style>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/ui-lightness/jquery-ui.css" type="text/css" />
    
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#bday').datepicker({
                dateFormat: "mm/dd/yy",
                maxDate: "-18Y",
                yearRange: "1900:",
                changeMonth: true,
                changeYear: true
            });

            $('input[type=text]').prop("required", "required");
            $('textarea').prop("required", "required");
            $('#bday').prop("required", "required");
            $('#file').prop("required", "required");
            $('#emailadd').prop("type", "email");

            $('#firstname, #lastname').keyup(function () {
                getUserName();
            });

            function getUserName() {
                var first = $('#firstname').val().replace(/ /g, '');
                var last = $('#lastname').val().replace(/ /g, '');
                $('#username').val(first + last);
            }

            $('#bday').change(function () {
                if ($('#bday').val().length == 10) {
                    var from = new Date($('#bday').val());
                    var to = new Date();
                    var diff = new Date(to - from);
                    $('#age').val(Math.floor((diff / 1000 / 60 / 60 / 24 / 365.25) + 0.00315576));
                }
                else {
                    $('#age').val("Invalid birthday!");
                    $('#bday').val("");
                }
            });

            $('#file').change(function () {
                var ext = $(this).val().split('.').pop().toLowerCase();
                if ($.inArray(ext, ['doc', 'docx', 'pdf']) == -1) {
                    var el = $("#file");
                    el.replaceWith(el = el.clone(true));
                }
            });
        });
    </script>

</body>
</html>
