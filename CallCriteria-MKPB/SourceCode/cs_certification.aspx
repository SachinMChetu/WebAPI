<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="cs_certification.aspx.vb" Inherits="cs_certification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">

        <h2>CallSouce Certification Tracker</h2>

        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>

         <asp:GridView ID="gvCSDetails" CssClass="detailsTable" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:HyperLinkField DataTextField="id" DataNavigateUrlFields="id" HeaderText="Review Call" DataNavigateUrlFormatString="/training/{0}" Target="_blank" />
                <asp:BoundField DataField="reviewer" HeaderText="Reviewer" />
                <asp:BoundField DataField="review_date" HeaderText="Review Date" />
                <asp:BoundField DataField="train_review_time" HeaderText="Review Time" />
                <asp:BoundField DataField="trainee_score" HeaderText="Score" />


            </Columns>
        </asp:GridView>

        <asp:GridView ID="gvdsCSStats" CssClass="detailsTable" AutoGenerateColumns="false" AllowSorting="true" DataKeyNames="QA" DataSourceID="dsCSStats" runat="server">
            <Columns>
                <asp:BoundField DataField="Campaign" HeaderText="Campaign" SortExpression="Campaign" />
                <asp:ButtonField DataTextField="ALL COMPLETED CALLS" HeaderText="ALL COMPLETED CALLS" SortExpression="ALL COMPLETED CALLS" CommandName="all" />
                <asp:BoundField DataField="QA" HeaderText="QA" SortExpression="QA" />
                <asp:ButtonField DataTextField="% ALL MISSED Q" HeaderText="% ALL MISSED Q" SortExpression="% ALL MISSED Q" CommandName="questions" />
                <asp:ButtonField DataTextField="% ALL MISSED CMNT" HeaderText="% ALL MISSED CMNT" SortExpression="% ALL MISSED CMNT" CommandName="comments" />
                <asp:BoundField DataField="AVG REVIEW TIME" HeaderText="AVG REVIEW TIME" SortExpression="AVG REVIEW TIME" />
                <asp:BoundField DataField="ALL AVG SCORE" HeaderText="ALL AVG SCORE" SortExpression="ALL AVG SCORE" />
                <asp:BoundField DataField="TOP 50 COMPLETED CALLS" HeaderText="TOP 50 COMPLETED CALLS" SortExpression="TOP 50 COMPLETED CALLS" />
                <asp:BoundField DataField="% TOP 50 MISSED Q" HeaderText="% TOP 50 MISSED Q" SortExpression="% TOP 50 MISSED Q" />
                <asp:BoundField DataField="% TOP 50 MISSED CMNT" HeaderText="% TOP 50 MISSED CMNT" SortExpression="% TOP 50 MISSED CMNT" />
                <asp:BoundField DataField="TOP 50 REVIEW TIME" HeaderText="TOP 50 REVIEW TIME" SortExpression="TOP 50 REVIEW TIME" />
                <asp:BoundField DataField="TOP 50 AVG SCORE" HeaderText="TOP 50 AVG SCORE" SortExpression="TOP 50 AVG SCORE" />
                <asp:BoundField DataField="Scorecard" HeaderText="Scorecard" SortExpression="Scorecard" />
                <asp:ButtonField ButtonType="Button" CommandName="certify" Text="Mark Certified" />
                <asp:ButtonField ButtonType="Button" CommandName="recertify" Text="Reset/Delete Training" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsCSStats" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="exec getCSTrainingStats" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
        <br /><br />
       
        


    </section>
</asp:Content>

