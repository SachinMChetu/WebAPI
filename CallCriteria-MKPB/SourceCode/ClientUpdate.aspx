<%@ Page Language="VB" MasterPageFile="~/CC_MASter.master" AutoEventWireup="false" CodeFile="ClientUpdate.aspx.vb" Inherits="ClientUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        body {
            background-color: #EEE;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 16px;
        }

        input[type="button"], input[type="submit"] {
            background-color: #FFF;
            border: 1px solid #000;
            color: #000;
            border-radius: 6px;
            padding: 10px;
            font-size: 16px;
            font-weight: 600;
            min-width: 100px;
        }

            input[type="button"]:hover, input[type="submit"]:hover {
                background-color: #000;
                color: #FFF;
                cursor: pointer;
            }

        input[type="text"], textarea, select {
            font-size: 16px;
            width: 99%;
            padding: 5px;
        }

        textarea {
            font-family: Arial, Helvetica, sans-serif;
            resize: none;
        }

        .jdiv div {
            padding: 10px;
        }

        /*.gvReport {
                width: 100%;
            }

                .gvReport tr:nth-child(even) {
                    background-color: #FFF;
                }

                .gvReport th, .gvReport td {
                    padding: 5px;
                }*/

        #popup, #addTo {
            background-color: #EFEFEF;
            display: none;
            position: absolute;
            z-index: 9999;
            width: 600px;
            height: 400px;
            font-size: 16px;
            box-shadow: 0px 10px 80px rgba(0,0,0,0.6);
        }

        #addTo {
            width: 240px;
            height: 300px;
        }

            #addTo div {
                padding: 0;
            }

        .gvUsers th {
            display: none;
        }

        .gvUsers, .gvUsers td {
            border: none;
        }

        #scrollable {
            max-height: 280px;
            overflow: auto;
        }

        #popup table {
            border-collapse: collapse;
            width: 95%;
            margin: 10px;
        }

        #popup td:first-child {
            font-weight: 600;
            width: 100px;
        }

        #popup td {
            padding: 10px;
        }

        #buttons {
            position: absolute;
            right: 20px;
            bottom: 20px;
        }

        #controls {
            position: absolute;
            right: 5px;
            bottom: 5px;
        }

        #cancel {
            margin-left: 20px;
        }

        .left, .fa-plus {
            float: left;
        }

        .fa-plus {
            padding-top: 8px;
            padding-left: 8px;
        }
    </style>


    <section class="main-container dash-modules general-button">

        <div class="jdiv">
            <div>
                <div>
                    <input type="button" id="create" value="CREATE" />
                </div>
            </div>

            <asp:GridView ID="gvReport" AllowSorting="true" runat="server" AutoGenerateColumns="False"
                DataKeyNames="ID" DataSourceID="dsReport" CellPadding="5" CssClass="detailsTable">
                <Columns>

                    <asp:TemplateField HeaderText="DATE ADDED" SortExpression="dateadded">
                        <ItemTemplate>
                            <asp:Label Text='<%#Eval("dateadded")%>' runat="server" />
                            <input type="hidden" id="hdnID" value='<%#Eval("id")%>' />
                            <input type="hidden" id="hdnMsg" value='<%#Eval("message_text")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="dateclosed" HeaderText="DATE CLOSED" SortExpression="dateclosed" />

                    <asp:BoundField DataField="from_login" HeaderText="FROM" SortExpression="from_login" />

                    <asp:BoundField DataField="to_login" HeaderText="TO" SortExpression="to_login" />

                    <asp:BoundField DataField="subject" HeaderText="SUBJECT" SortExpression="subject" />

                    <asp:BoundField DataField="message_text" HeaderText="MESSAGE" SortExpression="message_text" />

                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsReport" runat="server"
                ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="SELECT * FROM messaging ORDER BY dateadded DESC"></asp:SqlDataSource>

            <div id="popup">
                <table>
                    <tr>
                        <td>TO:</td>
                        <td>
                            <asp:TextBox ID="txtTos" runat="server" ReadOnly="true" Width="390" CssClass="left" />
                            <i class="fa fa-plus"></i>
                            <asp:HiddenField ID="hdnTos" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>SUBJECT:</td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>MESSAGE:</td>
                        <td>
                            <asp:TextBox ID="txtMsg" runat="server" TextMode="MultiLine" Rows="10" /></td>
                    </tr>
                </table>
                <div id="buttons">
                    <asp:Button Text="SAVE" ID="btnSave" runat="server" />
                    <input type="button" id="cancel" value="CANCEL" />
                </div>
            </div>

            <div id="addTo">
                <div id="scrollable">
                    <asp:GridView ID="gvUsers" AllowSorting="true" runat="server" AutoGenerateColumns="False"
                        DataSourceID="dsUsers" CellPadding="5" CssClass="gvUsers">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <input type="checkbox" />
                                    <span><%#Eval("username")%></span>
                                    <input type="hidden" id="hdnRole" value='<%#Eval("user_role")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsUsers" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                        SelectCommand="SELECT DISTINCT username, user_role FROM UserExtraInfo WHERE user_role = 'QA' OR user_role = 'QA Lead'AND active = 1 ORDER BY user_role DESC, username"></asp:SqlDataSource>
                </div>
                <div id="controls">
                    <input type="radio" name="selection" id="ALL" value="ALL" />
                    <label for="ALL">ALL</label>
                    <input type="radio" name="selection" id="QALead" value="QA Lead" />
                    <label for="QALead">QA Lead</label>
                    <input type="radio" name="selection" id="QA" value="QA" />
                    <label for="QA">QA</label>
                    &nbsp;&nbsp;&nbsp;
                <i class="fa fa-check"></i><i class="fa fa-times"></i>
                </div>
            </div>
        </div>

        <script>
            $(document).ready(function () {
                $('#create').click(function () {
                    $('#popup').css('left', $(window).width() / 2 - 300);
                    $('#popup').css('top', $(window).height() / 2 - 200);
                    $('#popup').show();
                    $('#addTo').hide();
                });
                $('#cancel').click(function () {
                    clearAddTo();
                    $('#popup').hide();
                    $('input[type=text], textarea').val('');
                });
                $('.fa-plus').click(function () {
                    $('#addTo').css('left', $(this).offset().left + 30);
                    $('#addTo').css('top', $(this).offset().top);
                    $('#addTo').show();
                });
                $('.fa-check').click(function () {
                    var Tos = '';
                    $('.gvUsers td').each(function () {
                        if ($(this).find('input[type=checkbox]').is(':checked')) {
                            Tos += $(this).find('span').text() + ',';
                        }
                    });
                    Tos = Tos.substr(0, Tos.length - 1);
                    $('[id*=txtTos]').val(Tos);
                    $('[id*=hdnTos]').val(Tos);
                    clearAddTo();
                });
                $('input[type=radio]').click(function () {
                    if ($(this).val() == 'ALL') {
                        $('.gvUsers td').each(function () {
                            $(this).find('input[type=checkbox]').prop('checked', true);
                        });
                    }
                    else if ($(this).val() == 'QA Lead') {
                        $('.gvUsers td').each(function () {
                            if ($(this).find('#hdnRole').val() == 'QA Lead')
                                $(this).find('input[type=checkbox]').prop('checked', true);
                            else
                                $(this).find('input[type=checkbox]').prop('checked', false);
                        });
                    }
                    else if ($(this).val() == 'QA') {
                        $('.gvUsers td').each(function () {
                            if ($(this).find('#hdnRole').val() == 'QA')
                                $(this).find('input[type=checkbox]').prop('checked', true);
                            else
                                $(this).find('input[type=checkbox]').prop('checked', false);
                        });
                    }
                });
                $('.fa-times').click(function () {
                    clearAddTo();
                });
            });

            function clearAddTo() {
                $('#addTo').hide();
                $('.gvUsers input[type=checkbox]').each(function () {
                    $(this).prop('checked', false);
                });
                $('input[type=radio]').each(function () {
                    $(this).prop('checked', false);
                });
            }

        </script>

    </section>
</asp:Content>
