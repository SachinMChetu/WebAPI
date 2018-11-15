<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="calibration_status.aspx.vb" Inherits="calibration_status" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Calibration Status</h2>
        Data from last 60 Days<br />
        TL Approvals needed:
        <asp:Label ID="lblTL" ForeColor="Red" runat="server" Text="0"></asp:Label>
        <table class="detailsTable">
            <br />
            Filter by App: <asp:DropDownList ID="ddlAppname" DataSourceID="dsAPPs" AutoPostBack="true" AppendDataBoundItems="true" DataTextField="appname" DataValueField="appname" runat="server">
                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
            </asp:DropDownList>

            <asp:SqlDataSource ID="dsAPPs" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                SelectCommand="select distinct appname from vwCF with (nolock) where review_date > dateadd(d, -14, dbo.getMTDate()) order by appname"></asp:SqlDataSource>



            <asp:Repeater ID="rptdsScorecards" DataSourceID="dsScorecards" runat="server">

                <ItemTemplate>
                    <tr>
                        <th>
                            <h4>
                                <asp:Label ID="lblScorecard" runat="server" Text='<%#Eval("scorecard_name") %>'></asp:Label></h4>
                            <asp:HiddenField ID="hdnScorecard" Value='<%#Eval("scorecard") %>' runat="server" />
                            Reset all QA Retain date to:
                            <asp:TextBox ID="txtRetrain" CssClass="datepicker" runat="server"></asp:TextBox>
                            <asp:Button ID="btnRetrain" runat="server" Text="Go" OnClick="btnRetrain_Click" />
                            <a href="edit_scorecard.aspx?ID=<%#Eval("scorecard") %>" target="_blank">Edit Scorecard</a>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvCalibrations" OnRowDataBound="gvCalibrations_RowDataBound" OnRowCommand="gvCalibrations_RowCommand" CssClass="detailsTable" DataSourceID="dsCalibrations" AllowSorting="true" runat="server">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnBypass" CommandName="Reset" runat="server" Text="Bypass" Visible="false" />
                                            <asp:Button ID="btnTLApprove" CommandName="AllowTrain" runat="server" Text="TL Approve Training" Visible="false" />
                                            <asp:Button ID="btnRetrain" CommandName="RetainDate" runat="server" Text="Reset Retrain Date" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <asp:SqlDataSource ID="dsCalibrations" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                        SelectCommand="getSCCalibrations" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdnScorecard" Name="scorecard" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </ItemTemplate>

            </asp:Repeater>
        </table>


        <asp:SqlDataSource ID="dsScorecards" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
            SelectCommand="select distinct scorecard, scorecard_name + ' (' + appname + ')' as scorecard_name, appname from vwCF with (nolock) where review_date > dateadd(d, -14, dbo.getMTDate()) and 1=case when @appname <> '' and appname <> @appname then 0 else 1 end order by appname, scorecard_name + ' (' + appname + ')' ">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlAppname" Name="appname"  ConvertEmptyStringToNull="false"/>
            </SelectParameters>
        </asp:SqlDataSource>




        <asp:LoginView ID="LoginView1" runat="server">
            <RoleGroups>
                <asp:RoleGroup Roles="Admin">
                    <ContentTemplate>
                        <asp:GridView ID="gvCaliSettings" CssClass="detailsTable" DataSourceID="dsCaliSettings" runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="avg_cali" HeaderText="avg_cali" ReadOnly="True" SortExpression="avg_cali" />
                                <asp:BoundField DataField="Column1" HeaderText="Column1" ReadOnly="True" SortExpression="Column1" />
                                <asp:BoundField DataField="scorecard" HeaderText="scorecard" SortExpression="scorecard" />
                                <asp:BoundField DataField="scorecard_name" HeaderText="scorecard_name" SortExpression="scorecard_name" />
                                <asp:TemplateField HeaderText="cutoff percent avg" SortExpression="cutoff_percent_avg">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCutAvg" runat="server" Width="30" Text='<%# Bind("cutoff_percent_avg") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCutAvg" runat="server" Width="30" Text='<%# Bind("cutoff_percent_avg") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="avg_cali_85_percent" HeaderText="Current Cutoff Score" ReadOnly="True" SortExpression="avg_cali_85_percent" />
                                <asp:TemplateField HeaderText="cutoff percent hard" SortExpression="cutoff_percent">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCutPercent" Width="30" runat="server" Text='<%# Bind("cutoff_percent") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCutPercent" Width="30" runat="server" Text='<%# Bind("cutoff_percent") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="cutoff_count" SortExpression="cutoff_count">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCutCount" Width="30" runat="server" Text='<%# Bind("cutoff_count") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCutCount" Width="30" runat="server" Text='<%# Bind("cutoff_count") %>'></asp:TextBox>
                                        <asp:Button ID="btnUpdate" OnClick="btnUpdate_Click" runat="server" Text="Update" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>



                        <asp:SqlDataSource ID="dsCaliSettings" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                            SelectCommand="select convert(decimal(10,2),avg(total_score)) as avg_cali, isnull(convert(decimal(10,2), case when cutoff_percent is null or cutoff_percent = 0 then null else cutoff_percent end), avg(total_score) * isnull(convert(float,cutoff_percent_avg)/100,.85)) as avg_cali_85_percent,count(*), 
                            scorecard, scorecard_name, cutoff_percent, cutoff_percent_avg, cutoff_count 
                            from vwCF with(nolock) join scorecards with(nolock)  on scorecards.id = scorecard  where review_date &gt; dateadd(d, -14, dbo.getMTDate()) group by scorecard, scorecard_name, cutoff_percent, cutoff_count, cutoff_percent_avg"></asp:SqlDataSource>
                    </ContentTemplate>
                </asp:RoleGroup>
            </RoleGroups>
        </asp:LoginView>


        <script>
            $(window).on('load', function () {

                $(function () {
                    $(".datepicker").datepicker();
                });
            });

        </script>
    </section>
</asp:Content>

