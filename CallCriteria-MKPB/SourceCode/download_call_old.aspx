<%@ Page Language="VB" AutoEventWireup="false" CodeFile="download_call_old.aspx.vb" Inherits="download_call" EnableEventValidation="false" %>

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
        
        <asp:HiddenField ID="hdnThisID" runat="server" />
        <asp:HiddenField ID="hdnCallLength" runat="server" />
        <asp:HiddenField ID="hdnThisAgent" runat="server" />
        <br />
        <br />


        <table>

            <asp:Literal ID="litPersonal" runat="server"></asp:Literal>


        </table>

        <asp:Repeater ID="rptSections" DataSourceID="dsSections" runat="server">
            <HeaderTemplate>

                <table cellpadding="0" cellspacing="0">
            </HeaderTemplate>
            <ItemTemplate>

                <tr class="section-header" unselectable="on" data-spaceheight="220">
                    <td style="-webkit-user-select: none; color: #242f3a; background-color: #dedfe0;"></td>
                    <td style="-webkit-user-select: none; color: #242f3a; background-color: #dedfe0;" colspan="6" class="section-name"><b><%#Eval("section")%></b>
                        
                    </td>
                </tr>
                <asp:HiddenField ID="hdnSectionID" Value='<%#Eval("ID")%>' runat="server" />
                <asp:Repeater ID="Repeater2" runat="server" DataSourceID="dsQuestions" OnItemDataBound="gvQuestions_RowDataBound">
                    <ItemTemplate>
                        <tr class='<%#Eval("bad-response")%>' runat="server" id="trHeader">
                            <td class="emptyBox">
                                <div class="rowShadow rowShadow1"></div>
                                <div class="rowShadow rowShadow2"></div>
                            </td>
                            <td class="question"><%#Eval("q_short_name")%>:</td>
                            <td style="width: 80%">
                                <%#Replace(Eval("comment").ToString, Chr(10), "<br>")%>
                                <asp:HiddenField ID="hdnQID" runat="server" Value='<%#Eval("question_id")%>' />

                                <asp:Repeater ID="rptComment" runat="server" DataSourceID="dsComments">
                                    <ItemTemplate>
                                        <%#Replace(Eval("comment").ToString, Chr(13), "<br>")%>&nbsp;
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:SqlDataSource ID="dsComments" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    SelectCommand="select distinct isnull(comment, other_answer_text) as comment from form_q_responses 
                                        left join answer_comments on answer_comments.id = form_q_responses.answer_id where form_id = @form_id 
                                        and form_q_responses.question_id = @QID">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnThisID" Name="form_ID" />
                                        <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <%#Eval("template_text")%>



                            </td>
                            <td class="response"><%#Eval("answer_text")%></td>
                            <td style="text-align: right"><%#Eval("answer_points")%></td>
                            <td style="text-align: right"><%#Eval("click_text")%></td>
                            <td style="text-align: right"><a href='<%#Eval("view_link")%>'><%#Eval("view_link")%></a></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class='<%#Eval("bad-response")%>' runat="server" id="trHeader">
                            <td class="emptyBox">
                                <div class="rowShadow rowShadow1"></div>
                                <div class="rowShadow rowShadow2"></div>
                            </td>
                            <td class="question"><%#Eval("q_short_name")%>:</td>
                            <td style="width: 80%">
                                <%#Replace(Eval("comment").ToString, Chr(10), "<br>")%>
                                <asp:HiddenField ID="hdnQID" runat="server" Value='<%#Eval("question_id")%>' />
                                <asp:Repeater ID="rptComment" runat="server" DataSourceID="dsComments">
                                    <ItemTemplate>
                                        <%#Replace(Eval("comment").ToString, Chr(13), "<br>")%>&nbsp;
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:SqlDataSource ID="dsComments" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    SelectCommand="select distinct isnull(comment, other_answer_text) as comment from form_q_responses 
                                        left join answer_comments on answer_comments.id = form_q_responses.answer_id where form_id = @form_id 
                                        and form_q_responses.question_id = @QID">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnThisID" Name="form_ID" />
                                        <asp:ControlParameter ControlID="hdnQID" Name="QID" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <%#Eval("template_text")%>
                            </td>
                            <td class="response"><%#Eval("answer_text")%></td>
                            <td style="text-align: right"><%#Eval("answer_points")%></td>
                            <td style="text-align: right"><%#Eval("click_text")%></td>
                            <td style="text-align: right"><a href='<%#Eval("view_link")%>'><%#Eval("view_link")%></a></td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>


                <asp:SqlDataSource ID="dsQuestions" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand=" select  isnull(comment,fs.other_answer_text) as comment, [dbo].getTemplateText(@form_id , q.ID, 1) as template_text,
                                *, case when right_answer = 1  then 'Right' else 'Wrong' end as real_result, 
                                case when right_answer = 0 then 'bad-response' else '' end as [bad-response] from dbo.form_q_scores fs
                                  left join answer_comments on answer_comments.id = fs.answer_comment
                                  join [Questions] q on fs.question_ID = q.ID
                                  join question_answers qa on qa.ID = question_answered  
                                  join sections s on s.id = q.section 
                                    where form_ID = @form_id  and s.id = @section and template not in ('Preferences')
                                    order by section_order, q_order"
                    runat="server">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnSectionID" Name="Section" />
                        <asp:ControlParameter ControlID="hdnThisID" Name="form_ID" />
                        <asp:SessionParameter SessionField="appname" Name="appname" />
                        <asp:ControlParameter ControlID="hdnThisAgent" Name="agent" DefaultValue="Rosvia" />
                    </SelectParameters>
                </asp:SqlDataSource>

            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:Repeater>


        <asp:SqlDataSource ID="dsSections" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="getSections2" SelectCommandType="StoredProcedure" runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnThisID" Name="form_ID" />
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
                <asp:Panel runat="server" ID="pnlNonWeb">
                    <tr>
                        <td style="background-color: #dedfe0; height: 30px"></td>
                        <td style="background-color: #dedfe0; height: 30px"><strong>View Call:</strong></td>
                        <td style="background-color: #dedfe0; height: 30px; text-align: left;"><a href="http://app.callcriteria.com/review_record.aspx?ID=<%#Eval("F_ID")%>>">
                            <img src="http://app.callcriteria.com/img/CC_play_button.png" /></a></td>


                        <td style="background-color: #dedfe0; height: 30px"></td>
                        <td style="background-color: #dedfe0; height: 30px"></td>
                        <td style="background-color: #dedfe0; height: 30px"></td>
                    </tr>

                    <tr>
                        <td style="background-color: #dedfe0; height: 30px"></td>
                        <td style="background-color: #dedfe0; height: 30px"><strong>Download Audio File:</strong></td>
                        <td style="background-color: #dedfe0; height: 30px; text-align: left;"><a href="http://app.callcriteria.com/download_audio.aspx?ID=<%#Eval("F_ID")%>">
                            <img src="http://app.callcriteria.com/img/CC_download_button.png" /></a> </td>


                        <td style="background-color: #dedfe0; height: 30px"></td>
                        <td style="background-color: #dedfe0; height: 30px"></td>
                        <td style="background-color: #dedfe0; height: 30px"></td>
                    </tr>
                </asp:Panel>
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

        </table>

            </div>
        </div>



    </form>
</body>
</html>
