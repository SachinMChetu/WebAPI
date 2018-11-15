<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="Domain_Messages.aspx.vb" Inherits="Domain_Messages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    
        <h2>Domain Messages</h2>

    
    <div class="general-filter">
        <div class="yellow-container">

            <!-- close select-holder -->
            <!-- close date-holder -->

        </div>
        <!-- close yellow-container -->
    </div>
    <!-- close general-filter -->

    <div class="table-outline">

        <asp:GridView ID="gvMessages" GridLines="None" DataSourceID="dsMessages" DataKeyNames="id" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="secondary-cta" Text="Acknowledge"  />
                <asp:BoundField DataField="Appname" HeaderText="Applies To" />
                <asp:BoundField DataField="Message" HeaderText="Message" />
                 <asp:BoundField DataField="dateadded" HeaderText="Date Added" />
            </Columns>
            <EmptyDataTemplate>No messages</EmptyDataTemplate>

        </asp:GridView>

        <asp:SqlDataSource ID="dsMessages" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" SelectCommand="select * from DomainMessages 
                    where archived is null and id not in (select message_id from DomainMessageAck where ack_by = @username and ack_date is not null) ">
            <SelectParameters>
                <asp:Parameter Name="username" />
            </SelectParameters>

        </asp:SqlDataSource>

    </div>
</asp:Content>

