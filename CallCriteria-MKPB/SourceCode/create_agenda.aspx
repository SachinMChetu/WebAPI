<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="create_agenda.aspx.vb" Inherits="create_agenda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="sc_files/scorecard.css" rel="stylesheet" />
    <script src="sc_files/scorecard.js"></script>
    <link href="sc_files/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>Create Agenda Item</h2>

        <table class="detailsTable">
           
            <tr>
                <th>Which scorecard is this about?</th>
                <td>
                    <asp:DropDownList ID="ddlApp" DataSourceID="dsApps" AutoPostBack="true" DataValueField="id" DataTextField="appname" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                    </asp:DropDownList>

                    <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select distinct userapps.appname + ' (' + isnull(short_name,'') + ')'  as appname, scorecards.id from userapps join scorecards on scorecards.appname = userapps.appname where username = @username and active = 1 order by  userapps.appname + ' (' + isnull(short_name,'') + ')' " runat="server">
                        <SelectParameters>
                            <asp:Parameter Name="username" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>

             <tr>
                <th>What is the link to the page this is about?</th>
                <td>
                    <asp:TextBox ID="txtLink" Width="500" runat="server"></asp:TextBox></td>
            </tr>

            <tr>

                <th>Which section?</th>
                <td>
                    <asp:DropDownList ID="ddlSections" DataSourceID="dsSections" AutoPostBack="true" AppendDataBoundItems="true" Enabled="false" DataValueField="id" DataTextField="section" runat="server">
                        <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                        <asp:ListItem Text="Scorecard General" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  ControlToValidate="ddlSections" InitialValue="" runat="server" ForeColor="Red" ErrorMessage="Section Required"></asp:RequiredFieldValidator>
                    <asp:SqlDataSource ID="dsSections" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                        SelectCommand="select section, id from sections where scorecard_id = @scorecard  order by section" runat="server">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlApp" Name="scorecard" PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>

            <tr>
                <th>Which question/instruction/FAQ?<br />(Select one)</th>
                <td>

                    <table>
                        <tr>
                            <td>Question:</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlQ" DataTextField="q_short_name" DataValueField="q_short_name" DataSourceID="dsQ" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Instruction:</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlI" DataTextField="q_text" DataValueField="q_text" DataSourceID="dsI" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>FAQ:</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlF"  DataTextField="q_text" DataValueField="q_text" DataSourceID="dsI" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Other:</td>
                            <td align="left">
                                <asp:TextBox ID="txtOther" runat="server"></asp:TextBox></td>
                        </tr>

                    </table>

                    <asp:SqlDataSource ID="dsQ" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                        SelectCommand="select * from questions where section =@section and active=1">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlSections" Name="section" PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="dsI" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                        SelectCommand="select [dbo].[udf_StripHTML](question_text) as q_text from q_faqs where question_id in (select id from questions where section =@section and active=1) ">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlSections" Name="section" PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="dsF" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server" 
                        SelectCommand="select [dbo].[udf_StripHTML](question_text) as q_text from q_instructions where question_id in (select id from questions where section =@section and active=1)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlSections" Name="section"  PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <th>Statement for discussion:</th>
                <td>
                    <asp:TextBox ID="txtDiscuss" TextMode="MultiLine" Rows="3" Columns="80" runat="server"></asp:TextBox></td>
            </tr>

        </table>
        <asp:Button ID="btnAddItem" runat="server" Text="Add" />

        <asp:HiddenField ID="hdnAgendaID" runat="server" />

        <asp:GridView ID="gvAgendaItems" CssClass="detailsTable" AutoGenerateColumns="false" DataKeyNames="ID" DataSourceID="dsAgendaItems" runat="server" AutoGenerateDeleteButton="true">
            <Columns>
                <asp:BoundField  DataField="date_created" SortExpression="date_created" HeaderText="Added" />
                <asp:BoundField  DataField="section_name" SortExpression="section_name" HeaderText="Section" />
                <asp:BoundField  DataField="header" SortExpression="header" HeaderText="Q/I/F" />
                <asp:BoundField  DataField="question" SortExpression="question" HeaderText="Discussion Point" />
<asp:BoundField  DataField="page_link" SortExpression="page_link" HeaderText="Page Link" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsAgendaItems" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" 
         DeleteCommand="delete from agenda_topics where id=@ID "
            SelectCommand="select *, sections.section as section_name from agenda_topics left join sections on agenda_topics.section = sections.ID where agenda_id= @ID" runat="server">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnAgendaID" Name="ID" />
            </SelectParameters>
        </asp:SqlDataSource>

    </section>
</asp:Content>

