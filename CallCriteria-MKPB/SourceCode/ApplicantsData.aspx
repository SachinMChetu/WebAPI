<%@ Page Title="Applicants Data" Language="VB" AutoEventWireup="false" CodeFile="ApplicantsData.aspx.vb" Inherits="ApplicantsData" %>

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
        <h2>QUESTIONS</h2>
        <hr />
        <asp:GridView ID="gvQ" AllowSorting="true" runat="server" AutoGenerateColumns="False"
            DataSourceID="dsQ" CellPadding="5" CssClass="gvReport">
            <Columns>
                
                <asp:BoundField DataField="ID" HeaderText="" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="Question" SortExpression="Question" HeaderText="Question" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsQ" runat="server"
            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="SELECT ID, Question, CASE WHEN Active=1 THEN 'Yes' ELSE 'No' END AS 'Active'  FROM ApplicantsQ">
        </asp:SqlDataSource>
        <input type="button" id="addQ" value="Add Question" />

        <br />
        <h2>APPLICANTS</h2>
        <hr />
        <asp:GridView ID="gvReport" AllowSorting="true" runat="server" AutoGenerateColumns="False"
            DataSourceID="dsReport" CellPadding="5" CssClass="gvReport">
            <Columns>

                <asp:BoundField DataField="DateCreated" SortExpression="DateCreated" HeaderText="Application Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="User Name" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="Birthday" SortExpression="Birthday" HeaderText="Birthday" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="Gender" SortExpression="Gender" HeaderText="Gender" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="QnA" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
                <asp:BoundField DataField="download" SortExpression="download" HeaderText="CV" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" />
                
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsReport" runat="server"
            ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="SELECT *, CONVERT(VARCHAR(10), ID) + '/' + CV AS download
                            FROM Applicants WHERE Active = 1 ORDER BY DateCreated DESC">
        </asp:SqlDataSource>

        <div class="popup saveQ">
            <table>
                <tr>
                    <td>Question</td>
                    <td>Active</td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField runat="server" ID="qid_edit" />
                        <asp:TextBox runat="server" ID="q_edit" TextMode="MultiLine" Columns="30" Rows="2" />
                    </td>
                    <td>
                        <input type="radio" id="yes" runat="server" name="active" value="1"/><label for="yes">Yes</label>
                        <input type="radio" id="no" runat="server" name="active" value="0" /><label for="no">No</label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:right;">
                        <asp:Button Text="Save" runat="server" ID="btnSave" /> &nbsp;
                        <input type="button" id="q_close" value="Close" />
                    </td>
                </tr>
            </table>
        </div>
        
        <div class="popup viewQ">
            <span></span>
            <div style="width:100%; height:340px; overflow-y:scroll;">
                <table>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <input type="button" id="v_close" value="Close" />
        </div>
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
        input[type=text], input[type=date], textarea {
            background-color: #FFF;
            border: 2px solid #CCC;
            border-radius: 3px;
            padding: 5px;
            font-family: 'Open Sans', Arial, Helvetica, sans-serif;
            font-size: 14px;
            font-weight: 600;
            color: #666;
        }
        textarea {
            resize: none;
        }
        input[type=text]:focus, input[type=date]:focus, textarea:focus {
            outline: 0;
            box-shadow: 0 0 2pt 1pt #666;
        }
        input[type=text]:disabled {
            background-color: #DDD;
        }
        input[type=button], input[type=submit] {
            background-color: #FFF;
            border: 2px solid #CCC;
            color: #666;
            border-radius: 6px;
            padding: 5px;
            font-size: 14px;
            font-weight: 600;
            min-width: 100px;
        }
        input[type="button"]:hover, input[type="submit"]:hover {
            background-color: #666;
            color: #FFF;
            box-shadow: 0 0 2pt 1pt #666;
        }
        a {
            text-decoration: none;
        }
        .main {
            width: 800px;
            margin: 0 auto;
        }
        .gvReport {
            width: 100%;
        }
        .clickable {
            color: #00F;
            cursor: pointer;
        }
        .popup {
            padding: 10px;
            background-color: #EFEFEF;
            display: none;
            position: absolute;
            z-index: 9999;
            font-size: 14px;
            box-shadow: 0px 10px 80px rgba(0,0,0,0.6);
        }
        .saveQ {
            width: 400px;
            height: 140px;
        }
        .viewQ {
            width: 600px;
            height: 400px;
            text-align:center;
        }
        .saveQ td {
            padding: 5px;
            text-align: center;
            vertical-align: top;
        }
        #addQ {
            margin-top: 5px;
        }
        #v_close {
            float: right;
            margin-top: 10px;
        }
    </style>
    
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>
    <script>
        $(document).ready(function () {
            $('.edit').click(function () {
                editQ($(this));
            });

            $('.answers').click(function () {
                showQnA($(this));
            });

            $('.popup').draggable();

            function editQ(obj) {
                $('[id$="qid_edit"]').val(obj.attr('qid'));
                $('[id$="q_edit"]').val(obj.parent().parent().find('td:nth-child(2)').text().trim());
                if (obj.parent().parent().find('td:nth-child(3)').text().trim() == 'Yes')
                    $('#yes').prop("checked", "checked");
                else
                    $('#no').prop("checked", "checked");

                $('.saveQ').css('left', $(window).width() / 2 - 200);
                $('.saveQ').css('top', $(window).height() / 2 - 70);
                $('.saveQ').show();
            }

            $('#q_close').click(function () {
                $('.saveQ').hide();
            });

            $('#addQ').click(function () {
                $('[id$="qid_edit"]').val("");
                $('[id$="q_edit"]').val("");
                $('#yes').prop("checked", "checked");
                $('#no').prop("checked", false);

                $('.saveQ').css('left', $(window).width() / 2 - 200);
                $('.saveQ').css('top', $(window).height() / 2 - 70);
                $('.saveQ').show();
            });

            function showQnA(obj) {
                var name = obj.parent().parent().find('td:nth-child(2)').text().trim() + ' ' + obj.parent().parent().find('td:nth-child(3)').text().trim()
                $('.viewQ span').text(name);
                $.ajax({
                    type: "POST",
                    url: "/CDService.svc/GetQnA",
                    data: '{"aid" : "' + obj.attr('aid') + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var jsonobj = eval(msg.d)
                        var items = ''
                        $('.viewQ tbody').empty();
                        $(jsonobj).each(function (i, el) {
                            items += '<tr><td style="padding:5px; text-align:left;">' + el.q + '</td></tr>';
                            items += '<tr><td style="padding:5px;"><textarea cols="66" rows="3" readonly>' + el.a + '</textarea></td></tr>';
                        });
                        $('.viewQ tbody').html(items);

                        $('.viewQ').css('left', $(window).width() / 2 - 300);
                        $('.viewQ').css('top', $(window).height() / 2 - 200);
                        $('.viewQ').show();
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }

            $('#v_close').click(function () {
                $('.viewQ').hide();
            });
        });
    </script>

</body>
</html>
