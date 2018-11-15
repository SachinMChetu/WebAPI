<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="All_Missed.aspx.vb" Inherits="All_Missed" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container">
        <h1 class="section-title"><i class="fa fa-desktop"></i>Missed Points Report</h1>
        <%--<div class="general-filter">
            <div class="yellow-container">

                 <asp:Button ID="btnExcel" runat="server" Text="Download"  CssClass="secondary-cta" />
                &nbsp;&nbsp;
                <asp:Button ID="btnAgentReport" CssClass="secondary-cta" runat="server" Text="APPLY" />

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentEnd" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
                </div>

                <div class="field-holder-right">
                    <i class="fa fa-calendar-o"></i>
                    <asp:TextBox ID="txtAgentStart" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
                </div>

               
                <!-- close date-holder -->

                <%--<div class="field-holder-right">
                    <i class="fa fa-user"></i>

                    <asp:DropDownList ID="ddlAgentGrp" runat="server" DataSourceID="dsAgentGrp" DataTextField="AGENT_GROUP"
                        AutoPostBack="true" AppendDataBoundItems="true"
                        DataValueField="AGENT_GROUP">
                        <asp:ListItem Text="All" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsAgentGrp" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="SELECT distinct lower([AGENT_GROUP]) as AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname order by lower([AGENT_GROUP]) ">
                        <SelectParameters>
                            <asp:QueryStringParameter QueryStringField="appname" Name="appname" DefaultValue="edsoup" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </div>
                <!-- close select-holder -->--%>


        <!-- close date-holder -->


        </div>
            <!-- close yellow-container -->

        </div>



         <table width="100%">
             <tr>
                 <td>
                     <img src="/images/photo.JPG" height="0" /></td>
                 <td align="right">


                     <div style="float: right;">


                         <div class="field-holder">
                             <i class="fa fa-tag"></i>

                             <asp:DropDownList ID="ddlGroup" runat="server" DataSourceID="dsAgentGrp" DataTextField="AGENT_GROUP"
                                 AutoPostBack="true" AppendDataBoundItems="true"
                                 DataValueField="AGENT_GROUP">
                                 <asp:ListItem Text="All Groups" Value=""></asp:ListItem>
                             </asp:DropDownList>
                             <asp:SqlDataSource ID="dsAgentGrp" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                 SelectCommand="SELECT distinct AGENT_GROUP FROM [XCC_REPORT_NEW] where appname=@appname and call_date between @start_date and @end_date and AGENT_GROUP is not null and AGENT_GROUP <> '' order by AGENT_GROUP ">
                                 <SelectParameters>
                                     <asp:QueryStringParameter QueryStringField="appname" Name="appname" DefaultValue="edsoup" />
                                     <asp:ControlParameter Name="start_date" ControlID="txtStartDate" />
                                     <asp:ControlParameter Name="end_date" ControlID="txtEndDate" />
                                 </SelectParameters>
                             </asp:SqlDataSource>

                         </div>




                         <div class="field-holder">
                             <i class="fa fa-user"></i>
                             <asp:DropDownList ID="ddlAgent" Visible="true" runat="server" AppendDataBoundItems="true"
                                 DataTextField="AGent" DataValueField="AGent">
                                 <%-- DataSourceID="dsAgents"--%>
                                 <asp:ListItem Text="All Agents" Value=""></asp:ListItem>
                             </asp:DropDownList>

                         </div>


                         <div class="field-holder">
                             <i class="fa fa-cogs"></i>
                             <asp:DropDownList ID="ddlCampaign" Visible="true" runat="server" AppendDataBoundItems="true"
                                 DataTextField="campaign" DataValueField="campaign" DataSourceID="dsCampaign">
                                 <%-- DataSourceID="dsAgents"--%>
                                 <asp:ListItem Text="All Campaigns" Value=""></asp:ListItem>
                             </asp:DropDownList>
                             <asp:SqlDataSource ID="dsCampaign" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                 SelectCommand="SELECT distinct campaign FROM [XCC_REPORT_NEW] where appname=@appname and call_date between @start_date and @end_date order by campaign ">
                                 <SelectParameters>
                                     <asp:ControlParameter ControlID="txtStartDate" Name="start_date" />
                                     <asp:ControlParameter ControlID="txtEndDate" Name="end_date" />
                                     <asp:QueryStringParameter QueryStringField="appname" Name="appname" DefaultValue="edsoup" />
                                 </SelectParameters>
                             </asp:SqlDataSource>
                         </div>

                         <div class="field-holder">
                             <i class="fa fa-calendar-o"></i>
                             <asp:TextBox ID="txtStartDate" CssClass="datepicker start-date" placeholder="Start..." runat="server"></asp:TextBox>
                         </div>
                         <!-- close date-holder -->


                         <div class="field-holder">
                             <i class="fa fa-calendar-o"></i>
                             <asp:TextBox ID="txtEndDate" CssClass="datepicker end-date" placeholder="End..." runat="server"></asp:TextBox>
                         </div>
                         <asp:Button ID="btnApplyFilter" CssClass="secondary-cta" runat="server" Text="APPLY" />
                         &nbsp;<asp:Button ID="btnExcel" runat="server" Text="Download" CssClass="secondary-cta" />
                         <div class="field-holder">
                         </div>
                         <!-- close date-holder -->


                         <!-- close yellow-container -->

                     </div>

                 </td>
                 <td>
                     <%--<a href="view_scorecard.aspx" class="third-priority-buttom"><i class="fa fa-gear"></i>View Scorecard</a>--%>

                 </td>
             </tr>
         </table>


        <!-- close general-filter -->

        <p>&nbsp;</p>




        <asp:Panel ID="pnlExcel" runat="server">
            <div class="table-outline users-list">
                <%--<table> 
                    <thead>
                        <tr>
                            <th class="first">Short Names
                            </th>
                            <th>Total Reviews
                            </th>
                            <th>Number Missed
                            </th>
                            <th class="last">Percent Missed
                            </th>
                        </tr>
                    </thead>
                    <tbody>--%>
                <%-- <asp:Repeater ID="repeatMissedPoint" runat="server" DataSourceID="dsMissedPoint">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href="missed_details.aspx?QID=<%# Eval("question_ID")%>&start_date=<%# eval("start_date")%>&end_date=<%# eval("end_date")%>&short_name=<%# eval("q_short_name") %>">
                                            <%#Eval("q_short_name") %></a>
                                    </td>
                                    <td>
                                        <%#Eval("total_questions")%>
                                    </td>
                                    <td>
                                        <%#Eval("number_missed")%>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Font-Size="X-Small" Text='<%#Eval("Percent_Qs") %>'></asp:Label>%
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>--%>


                <asp:GridView runat="server" AllowSorting="true" ID="repeatMissedPoint" DataSourceID="dsMissedPoint" AutoGenerateColumns="false" GridLines="None">
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFields="question_ID,start_date,end_date,q_short_name" DataTextField="q_short_name"
                            DataNavigateUrlFormatString="missed_details.aspx?QID={0}&start_date={1}&end_date={2}&short_name={3}" SortExpression="q_short_name" HeaderText="Short Names" />
                        <asp:BoundField DataField="total_questions" HeaderText="Total Reviews" SortExpression="total_questions" />
                        <asp:BoundField DataField="number_missed" HeaderText="Number Missed" SortExpression="number_missed" />
                        <asp:BoundField DataField="Percent_Qs" HeaderText="Percent Missed" SortExpression="Percent_Qs" />
                    </Columns>
                </asp:GridView>

                <%--                    </tbody>
                </table>--%>
            </div>
        </asp:Panel>
        <asp:SqlDataSource runat="server" ID="dsMissedPoint" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="total_missed" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtStartDate" DefaultValue="1/1/1900" Name="start_date" />
                <asp:ControlParameter ControlID="txtEndDate" DefaultValue="1/1/2015" Name="end_date" />
                <asp:Parameter Name="appname" DefaultValue="edsoup" />
                <asp:Parameter DefaultValue="" Name="agent_group_filter" />
            </SelectParameters>
        </asp:SqlDataSource>
        <div class="clear">
        </div>
    </section>
</asp:Content>
