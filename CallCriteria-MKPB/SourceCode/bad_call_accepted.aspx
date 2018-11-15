<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="bad_call_accepted.aspx.vb" Inherits="bad_call_accepted" EnableEventValidation="false" %>

<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="head">
	<style>
	    .dash-modules .detailsTable td {
	        font-size: xx-small;
	        border-right: solid 1px #f3f4f4;
	        vertical-align: middle;
	    }

	    tr.paging-table, table {
	        width: auto !important;
	    }

	    .header-table td input[type="text"] {
	        height: 32px;
	    }

	    #ContentPlaceHolder1_ddlSCORECARD {
	        height: 36px;
	    }

	    #ContentPlaceHolder1_ddlAppname {
	        height: 36px;
	    }

	    .header-table td {
	        padding-right: 7px;
	    }

	    .grid-pager a, .grid-pager span {
	        font-size: 12px;
	        padding-left: 4px;
	        padding-right: 4px;
	    }

	    .grid-pager span {
	        background-color: #41637c;
	        color: white;
	        font-size: 14px;
	        font-weight: bold;
	    }

	    .ui-datepicker {
	        padding: 0px;
	        width: initial;
	    }
	</style>
	<script type="text/javascript">
        $(function () {
            $("[id$=txtCALL_DATE_FROM],[id$=txtCALL_DATE_TO],[id$=txtBAD_CALL_ACCEPTED]").datepicker({
                showOn: "focus"
                , autoSize: true
                , showAnim: "fadeIn"
                , changeMonth: true
                , changeYear: true
                , showButtonPanel: true
                , dateFormat: "mm/dd/yy"
                , maxDate: "d"
                , closeText: "Clear"
                , onClose: function (dateText, inst) {
                    if ($(window.event.srcElement).hasClass("ui-datepicker-close")) {
                        document.getElementById(this.id).value = "";
                    }
                }
            });
        });
	</script>
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1">

	<section class="main-container dash-modules general-button">

		<h2>Bad Call Accepted Report</h2>

		<table style="width: 100%;" class="header-table">
			<tr class="headingsearch">
				<td><asp:Label runat="server" Font-Size="Small" Font-Bold="true" Text="SCORE CARD" /></td>
                <td><asp:Label runat="server" Font-Size="Small" Font-Bold="true" Text="APPNAME" /></td>
				<td><asp:Label runat="server" Font-Size="Small" Font-Bold="true" Text="FROM CALL DATE" /></td>
				<td><asp:Label runat="server" Font-Size="Small" Font-Bold="true" Text="TO CALL DATE" /></td>
				<td><asp:Label runat="server" Font-Size="Small" Font-Bold="true" Text="DATE ACCEPTED" /></td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>
					<asp:DropDownList runat="server" ID="ddlSCORECARD" DataSourceID="dsSCORECARD" DataTextField="short_name" DataValueField="scorecard" AppendDataBoundItems="true">
						<items><asp:ListItem Text="--ALL--" Value="" /></items>
					</asp:DropDownList>
				</td>
                <td>
					<asp:DropDownList runat="server" ID="ddlAppname" DataSourceID="dsAppname" DataTextField="appname" DataValueField="appname" AppendDataBoundItems="true">
						<items><asp:ListItem Text="--ALL--" Value="" /></items>
					</asp:DropDownList>
				</td>
				<td><asp:TextBox runat="server" ID="txtCALL_DATE_FROM" placeholder="MM/DD/YYYY" /></td>
				<td><asp:TextBox runat="server" ID="txtCALL_DATE_TO" placeholder="MM/DD/YYYY" /></td>
				<td><asp:TextBox runat="server" ID="txtBAD_CALL_ACCEPTED" placeholder="MM/DD/YYYY" /></td>
				<td><asp:Button runat="server" ID="btnSEARCH" Text="SEARCH" OnClick="btnSEARCH_Click" Font-Bold="true" BackColor="#41637c" BorderColor="#999999" ForeColor="White" /></td>
			</tr>
            <tr>
                <td colspan="2"></td>
                <td colspan="3"><i>With no date selected, range is last 30 days</i></td>
            </tr>
			<tr>
				<td><asp:Button runat="server" ID="btnEXPORT" Text="EXPORT TO EXCEL" Font-Bold="true" BackColor="#41637c" BorderColor="#999999" ForeColor="White"/></td>
				<td colspan="3"><asp:Label runat="server" ID="lblCOUNT" Text="" /></td>
				<td><asp:Button runat="server" ID="btnSHOWALL" Text="SHOW ALL" OnClick="btnSHOWALL_Click" Visible="false" Font-Bold="true" BackColor="#41637c" BorderColor="#999999" ForeColor="White" /></td>
			</tr>
		</table>
         
		<asp:GridView runat="server"
			ID="gvBadCallAccepted"
			CssClass="detailsTable"
			Style="font-size: xx-small;"
			CellPadding="5"
			AllowSorting="true"
			OnSorting="gvBadCallAccepted_Sorting"
			AutoGenerateColumns="true"
			DataKeyNames="ID"
			OnRowDataBound="gvBadCallAccepted_RowDataBound"
			AllowPaging="true"
			PageSize="100"
			OnPageIndexChanging="gvBadCallAccepted_OnPageIndexChanging">
			<PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
			<PagerStyle CssClass="grid-pager" />
		</asp:GridView>

	</section>

	<asp:SqlDataSource runat="server" ID="dsSCORECARD" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
		SelectCommand="SELECT DISTINCT xcc_report_new.scorecard, scorecards.short_name FROM xcc_report_new JOIN scorecards ON scorecards.id = xcc_report_new.scorecard 
        and scorecard in (select user_scorecard from userapps where username =  @username) WHERE xcc_report_new.bad_call_accepted IS NOT NULL AND xcc_report_new.bad_call_accepted > DATEADD(d, -30, getdate()) ORDER BY scorecards.short_name">
		<SelectParameters>
			<asp:Parameter Name="username" DefaultValue="<%= HttpContext.Current.User.Identity.Name %>" />
		</SelectParameters>
	</asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsAppname" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
		SelectCommand="SELECT DISTINCT appname FROM scorecards where id in (select user_scorecard from userapps where username =  @username) ORDER BY appname">
		<SelectParameters>
			<asp:Parameter Name="username" DefaultValue="<%= HttpContext.Current.User.Identity.Name %>" />
		</SelectParameters>
	</asp:SqlDataSource>

</asp:Content>
