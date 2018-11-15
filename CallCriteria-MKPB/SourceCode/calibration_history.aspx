<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="calibration_history.aspx.vb" Inherits="calibration_history" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container">
        
            <h1 class="section-title"><i class="fa fa-desktop"></i>
                Calibration History Report</h1>
        

        <div class="general-filter">
            <div class="yellow-container" style="height: 120px;">

                <table style="float: right;">
                    <tr>

                        <td>Select Domain(s):
                                    <asp:CheckBoxList runat="server" ID="cblDomainList" RepeatColumns="4">
                                        <asp:ListItem Selected="True"> EdSoup</asp:ListItem>
                                        <asp:ListItem Selected="True"> CC</asp:ListItem>
                                        <asp:ListItem Selected="True"> GVD</asp:ListItem>
                                        <asp:ListItem Selected="True"> MS</asp:ListItem>

                                    </asp:CheckBoxList>
                        </td>
                        <td>
                            <asp:Button ID="btnQAReport" CssClass="secondary-cta" runat="server" Text="Go" /></td>
                        <td>
                            <asp:Button ID="btnSupeExportxp" runat="server" CssClass="secondary-cta" Text="Export to Excel" /></td>
                    </tr>
                </table>

                <!-- close select-holder -->
                <!-- close date-holder -->

            </div>
            <!-- close yellow-container -->
        </div>

        <div class="table-outline">
            <asp:SqlDataSource ID="dsHistory" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" SelectCommand="getCalHistory" 
                SelectCommandType="StoredProcedure" runat="server">
                <SelectParameters>
                    <asp:Parameter Name="appnames" DefaultValue="EDSOUP,MS,CC,GVD" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:GridView ID="gvHistory" AllowSorting="true" DataSourceID="dsHistory" GridLines="None" runat="server">
              
            </asp:GridView>
        </div>
    </section>
</asp:Content>


