<%@ Page Language="VB" AutoEventWireup="false" CodeFile="download_call.aspx.vb" Inherits="download_call" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Basic Page Needs    ================================================== -->
    <meta charset="utf-8" />
    <title>Call Criteria</title>
    <!--<meta name="viewport" content="width=device-width; initial-scale=1; maximum-scale=1">-->

    <!--[if lt IE 9]>
    <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->

    <!-- CSS ================================================== -->

    <link href='http://fonts.googleapis.com/css?family=Open+Sans:100,400,300,600,700,800' rel='stylesheet' type='text/css' />


    <link rel="stylesheet" href="/css/fonts.css" type="text/css" />
    <link rel="stylesheet" href="/css/font-awesome.css" type="text/css" />

    <script src="http://code.jquery.com/jquery-1.10.2.min.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <%--<script src="http://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.min.js"></script>

    <link rel="stylesheet" type="text/css" href="/css/imgareaselect-default.css" />
    <%--<script type="text/javascript" src="scripts/jquery.min.js"></script>--%>
    <script type="text/javascript" src="/scripts/jquery.imgareaselect.pack.js"></script>

    <!-- Main script -->
    <script type="text/javascript" src="/js/main.js?1009"></script>


    <script type="text/javascript">
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
            var WinPrint = window.open('', '', 'letf=0,top=0,width=400,height=400,toolbar=0,scrollbars=0,status=0');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();

        }

        function cancelBubble(e) {
            var evt = e ? e : window.event;
            if (evt.stopPropagation) evt.stopPropagation();
            if (evt.cancelBubble != null) evt.cancelBubble = true;
        }

    </script>

</head>
<body style="padding: 0px 0px 0px 0px; font-family: 'Open Sans', Arial, Helvetica, sans-serif;">

    <form id="form1" runat="server">
        <asp:Panel ID="pnlLogo" runat="server">
        </asp:Panel>

        <img src= "http://app.callcriteria.com/img/CallCriteria-side.png" height="100" />
        <asp:HiddenField ID="hdnThisID" runat="server" />
        <asp:HiddenField ID="hdnCallLength" runat="server" />
        <asp:HiddenField ID="hdnThisAgent" runat="server" />
        <br />
        <br />


        <table>

            <asp:Literal ID="litPersonal" runat="server"></asp:Literal>


        </table>
        <br />
        <br />

        <asp:GridView ID="gvAllQs" GridLines="None" DataSourceID="dsAllQs" runat="server"></asp:GridView>
        <asp:SqlDataSource ID="dsAllQs" SelectCommand="getFullCallTable" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="ID" Name="f_id" />
                <asp:Parameter Name="username" />
            </SelectParameters>
        </asp:SqlDataSource>


        <asp:FormView ID="fvFORMData" RenderOuterTable="false" DataSourceID="dsFormData" runat="server">
            <ItemTemplate>
                <asp:HiddenField ID="hdnXCCID" runat="server" />
                <asp:HiddenField ID="theXID" runat="server" Value='<%#Eval("review_id")%>' />

                <tr>
                    <td style="background-color: #dedfe0;"></td>
                    <td style="background-color: #dedfe0;"><strong>Total</strong></td>
                    <td style="background-color: #dedfe0;"></td>
                    <td style="background-color: #dedfe0;"></td>
                    <td style="background-color: #dedfe0;"><strong><%#Eval("total_score")%></strong></td>
                    <td style="background-color: #dedfe0;"></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td>&nbsp;</td>
                </tr>
                <asp:Panel runat="server" ID="pnlNonWeb">
                  <%--  <tr>
                        <td style="background-color: #dedfe0; height: 30px"></td>
                        <td style="background-color: #dedfe0; height: 30px"><strong>View Call:</strong></td>
                        <td style="background-color: #dedfe0; height: 30px; text-align: left;">
                            <a href='http://app.callcriteria.com/review/<%#Eval("F_ID")%>>'>View</a></td>


                    </tr>

                    <tr>
                        <td style="background-color: #dedfe0; height: 30px"></td>
                        <td style="background-color: #dedfe0; height: 30px"><strong>Download Audio File:</strong></td>
                        <td style="background-color: #dedfe0; height: 30px; text-align: left;">
                            <a href='http://app.callcriteria.com/download_audio.aspx?ID=<%#Eval("F_ID")%>'>Download Audio</a> </td>

                    </tr>--%>
                </asp:Panel>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td>&nbsp;</td>
                </tr>
                <asp:Repeater ID="rptComments" OnItemDataBound="rptComments_ItemDataBound" DataSourceID="dsComments" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <td style="background-color: #dedfe0;"></td>
                            <td colspan="3" class="section-name" style="background-color: #dedfe0;"><strong>Comments:</strong>
                            </td>
                            <td class="error-indicator" style="background-color: #dedfe0;">
                                <div></div>
                            </td>
                        </tr>

                    </HeaderTemplate>
                    <ItemTemplate>

                        <tr>
                            <td class="emptyBox" style="background-color: #f1f6f8">
                                <div class="rowShadow rowShadow1"></div>
                                <div class="rowShadow rowShadow2"></div>
                            </td>
                            <td style="background-color: #f1f6f8" class="question"><%#Eval("comment_header")%></td>
                            <td><%#Replace(Eval("comment").ToString, Chr(10), "<br>")%>
                            </td>
                            <td style="background-color: #f1f6f8" class="response"><%#Eval("comment_who")%><br />
                                <%#Eval("comment_date")%></td>
                            <td style="background-color: #f1f6f8" class="td-play">
                                <div></div>
                                </button>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:SqlDataSource ID="dsComments" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="select ROW_NUMBER() OVER(ORDER BY comment_date) + 100 AS data_section,*, substring(comment_pos,1,CHARINDEX(':', comment_pos) - 1) * 60 + substring(comment_pos,CHARINDEX(':', comment_pos) + 1, 1000) as click_pos 
                            from system_comments where comment_id = @form_id order by id desc">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnThisID" Name="form_id" />
                    </SelectParameters>
                </asp:SqlDataSource>





                </div>
            </ItemTemplate>
        </asp:FormView>
        <asp:SqlDataSource ID="dsFormData" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="SELECT * from vwForm join app_settings on app_settings.appname = vwForm.appname where f_id = @ID"
            runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnThisID" Name="ID" />
            </SelectParameters>
        </asp:SqlDataSource>



    </form>
</body>
</html>
