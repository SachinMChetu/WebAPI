<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="clear_notifications.aspx.vb" Inherits="clear_notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">


        <table>
            <tr>
                <td valign="top">
                    <h2>Clear Notifications</h2>
                    <table>
                        <tr>
                            <td>Scorecard:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSC" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" AutoPostBack="true" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>

                        <tr>

                            <td>Role:</td>
                            <td>
                                <asp:DropDownList ID="ddlROle" AutoPostBack="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    <asp:ListItem>Admin</asp:ListItem>
                                    <asp:ListItem>Agent</asp:ListItem>
                                    <asp:ListItem>Calibrator</asp:ListItem>
                                    <asp:ListItem>Client</asp:ListItem>
                                    <asp:ListItem>Inactive</asp:ListItem>
                                    <asp:ListItem>Manager</asp:ListItem>
                                    <asp:ListItem>QA</asp:ListItem>
                                    <asp:ListItem>QA Lead</asp:ListItem>
                                    <asp:ListItem>Supervisor</asp:ListItem>
                                    <asp:ListItem>Partner</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td colspan="2">Notification Created (not required)</td>
                        </tr>
                        <tr>
                            <td>From Date:</td>
                            <td>
                                <asp:TextBox ID="txtClearStart" AutoPostBack="true" CssClass="hasDatePicker" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>To Date:</td>
                            <td>
                                <asp:TextBox ID="txtClearEnd" AutoPostBack="true" CssClass="hasDatePicker" runat="server"></asp:TextBox></td>
                        </tr>
                    </table>

                    Number Rows Affected:
        <asp:Label ID="lblRows" runat="server" Text="0"></asp:Label>
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" />
                    <asp:SqlDataSource ID="dsScorecards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select short_name, id from scorecards where active = 1  order by short_name" runat="server"></asp:SqlDataSource>


                    <hr />

                    <h2>Clear Pending Calibrations</h2>

                    <table>
                        <tr>
                            <td>Scorecard:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSCC" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" AutoPostBack="true" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>


                    </table>

                    Number Rows Affected:
        <asp:Label ID="lblCRows" runat="server" Text="0"></asp:Label>
                    <asp:Button ID="btnPendingDelete" runat="server" Text="Delete" />
                    <asp:SqlDataSource ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select short_name, id from scorecards where active = 1  order by short_name" runat="server"></asp:SqlDataSource>


                    <hr />

                    <h2>Clear Pending Client Calibrations</h2>

                    <table>
                        <tr>
                            <td>Scorecard:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSCCC" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" AutoPostBack="true" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>

                        <tr>
                            <td>Client:</td>
                            <td>
                                <asp:DropDownList ID="ddlClient" DataSourceID="dsClientList" DataTextField="assigned_to" DataValueField="assigned_to" AutoPostBack="true" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="dsClientList" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    SelectCommand="select distinct assigned_to from cali_pending_client join vwForm on vwForm.f_id = form_id where date_completed is null and scorecard = @scorecard order by assigned_to" runat="server">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlSCCC" Name="scorecard" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </td>
                        </tr>

                        <tr>

                            <td>Role:</td>
                            <td>
                                <asp:DropDownList ID="ddlCPC" AutoPostBack="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    <asp:ListItem>Client</asp:ListItem>
                                    <asp:ListItem>Manager</asp:ListItem>
                                    <asp:ListItem>Supervisor</asp:ListItem>
                                    <asp:ListItem>Partner</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>

                    </table>

                    Number Rows Affected:
        <asp:Label ID="lblClientRows" runat="server" Text="0"></asp:Label>
                    <asp:Button ID="btnClientDelete" runat="server" Text="Delete" />


                    <hr />

                    <h2>Add QA Calibrations</h2>

                    <table>
                        <tr>
                            <td>Scorecard:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSCQAC" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>

                        <tr>
                            <td>QA:</td>
                            <td>
                                <asp:DropDownList ID="ddlQA" DataSourceID="dsQA" DataTextField="username" DataValueField="username" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="dsQA" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    SelectCommand="select username from userextrainfo where user_role = 'QA' order by username" runat="server"></asp:SqlDataSource>

                            </td>
                        </tr>
                        <tr>
                            <td>Quantity:</td>
                            <td>
                                <asp:TextBox ID="txtQAAdd" runat="server"></asp:TextBox></td>
                        </tr>
                    </table>

                    <asp:Button ID="btnAddQACal" runat="server" Text="Add" />
                    <asp:Label ID="lblAddQACal" runat="server" Text=""></asp:Label>



                    <hr />

                    <h2>Add Client Calibrations</h2>

                    <table>
                        <tr>
                            <td>Scorecard:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSCQACC" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" AutoPostBack="true" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>

                        <tr>
                            <td>QA/Calibrator/Admin/Lead:</td>
                            <td>
                                <asp:DropDownList ID="ddlQACA" DataSourceID="dsQACA" DataTextField="reviewer" DataValueField="reviewer" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="dsQACA" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    SelectCommand="select distinct reviewer from vwForm where scorecard =@scorecard order by reviewer" runat="server">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlSCQACC" Name="scorecard" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </td>
                        </tr>

                        <tr>
                            <td>Quantity:</td>
                            <td>
                                <asp:TextBox ID="txtAddCC" runat="server"></asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="GO" />
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:GridView ID="gvCallList" CssClass="detailsTable" DataSourceID="dsCallList" DataKeyNames="f_id" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Add">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkUserThis" Checked="true" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="dsCallList" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                                    SelectCommand="select top (@qty) review_date, F_ID from vwForm where reviewer = @reviewer and scorecard = @scorecard and f_id not in (select form_id from cali_pending_client)  order by f_id desc">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlQACA" Name="reviewer" />
                                        <asp:ControlParameter ControlID="txtAddCC" Name="qty" Type="Int16" />
                                        <asp:ControlParameter ControlID="ddlSCQACC" Name="scorecard" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>

                    </table>

                </td>

                <td valign="top">



                    <h2>Rescore Calls</h2>

                    <table>
                        <tr>
                            <td>Scorecard:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRescore" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" AutoPostBack="true" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>



                        <tr>
                            <td>Start Call Date:</td>
                            <td>
                                <asp:TextBox ID="txtRescoreStart" CssClass="datepicker" runat="server"></asp:TextBox>
                            </td>
                        </tr>


                        <tr>
                            <td>End Call Date:</td>
                            <td>
                                <asp:TextBox ID="txtRescoreEnd" CssClass="datepicker" runat="server"></asp:TextBox>
                                <asp:Button ID="btnRescore" runat="server" Text="GO" />
                                <asp:Label ID="lblRescore" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>



                    </table>



                    <h2>Rescore Calibrations</h2>

                    <table>
                        <tr>
                            <td>Scorecard:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRescoreCali" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" AutoPostBack="true" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>



                        <tr>
                            <td>Start Call Date:</td>
                            <td>
                                <asp:TextBox ID="txtRescoreStartCali" CssClass="datepicker" runat="server"></asp:TextBox>
                            </td>
                        </tr>


                        <tr>
                            <td>End Call Date:</td>
                            <td>
                                <asp:TextBox ID="txtRescoreEndCali" CssClass="datepicker" runat="server"></asp:TextBox>
                                <asp:Button ID="btnRescoreCali" runat="server" Text="GO" />
                                <asp:Label ID="lblRescoreCali" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>



                    </table>
                    <hr />
                 <h2>Mark reviewed calls bad</h2>

                    <table>
                        <tr>
                            <td>Scorecard:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMarkBadScorecard" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="ID" AutoPostBack="true" AppendDataBoundItems="true" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>


                        <tr>
                            <td>Reject Reason:</td>
                            <td>
                                <asp:TextBox ID="txtRejectReason" runat="server"></asp:TextBox><br />
                            </td>
                        </tr>

                        <tr>
                            <td>Start Review Date:</td>
                            <td>
                                <asp:TextBox ID="txtMarkBadReviewStart" CssClass="datepicker" runat="server"></asp:TextBox>
                            </td>
                        </tr>


                        <tr>
                            <td>End Review Date:</td>
                            <td>
                                <asp:TextBox ID="txtMarkBadReviewEnd" CssClass="datepicker" runat="server"></asp:TextBox>                                

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><b>--OR--</b></td>
                        </tr>
                        <tr>
                            <td>Start Call Date:</td>
                            <td>
                                <asp:TextBox ID="txtMarkBadCallDateStart" CssClass="datepicker" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                       <tr>
                            <td>End Call Date:</td>
                            <td>
                                <asp:TextBox ID="txtMarkBadCallDateEnd" CssClass="datepicker" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="btnMarkBad" runat="server" Text="GO" />
                                <asp:Label ID="lblMarkBad" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>
        </table>

        <asp:Button ID="btnAddCC" runat="server" Text="Add" />
        <asp:Label ID="lblAddCC" runat="server" Text=""></asp:Label>
        <script>
            $(window).on('load', function () {

                $(function () {
                    $(".datepicker").datepicker();
                });
            });

        </script>

    </section>
</asp:Content>

