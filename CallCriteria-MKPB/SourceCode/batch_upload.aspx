<%@ Page Language="VB" AutoEventWireup="false" CodeFile="batch_upload.aspx.vb" Inherits="batch_upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

</head>
<body>
    <form id="form1" runat="server">
        <%  Session.Timeout = 900 %>
        <div class="style1">
            <center style="text-align: center">
                <b>Call Criteria - Batch Upload</b><br />
                <br />
                <asp:HiddenField ID="hdnUploadedFile" runat="server" />
                <center>
                    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" BackColor="#EFF3FB" BorderColor="#B5C7DE"
                        BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em">
                        <NavigationStyle VerticalAlign="Bottom" HorizontalAlign="right"  Width="500px" Height="300px" />
                        <StepStyle HorizontalAlign="Center" VerticalAlign="Top" CssClass="steppadding" Font-Size="0.8em" ForeColor="#333333" />
                        <HeaderTemplate>
                            Step <%=Wizard1.ActiveStepIndex + 1%> of <%=Wizard1.WizardSteps.Count%>
                        </HeaderTemplate>
                        <HeaderStyle BackColor="AliceBlue" ForeColor="White" />
                        <WizardSteps>
                            <asp:WizardStep ID="WizardStep3" runat="server" Title="App/Scorecard/Call Date">
                                <br />
                                Appname:<br />
                                <br />
                                <asp:DropDownList ID="ddlAppname" runat="server" AutoPostBack="true"
                                    DataSourceID="dsCustomerName" DataTextField="appname"
                                    DataValueField="appname">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="dsCustomerName"
                                    ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    runat="server"
                                    SelectCommand="select appname from app_settings where active = 1 order by appname"></asp:SqlDataSource>
                                <br />
                                <br />
                                Project Phase:<br />
                                <br />
                                <asp:DropDownList ID="ddlScorecard" DataSourceID="dsScorecards" DataTextField="short_name" DataValueField="id" runat="server">
                                </asp:DropDownList>

                                <asp:SqlDataSource ID="dsScorecards"
                                    ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                    runat="server"
                                    SelectCommand="select short_name, id from scorecards where appname = @appname and active = 1 order by short_name">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlAppname" Name="appname" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <br />
                                <br />
                                Call Date:
                            <br />
                                <center>
                                    <asp:Calendar ID="calCallDate" runat="server"></asp:Calendar>
                                </center>
                            </asp:WizardStep>
                            <asp:WizardStep ID="WizardStep1" runat="server" Title="Select Data File">
                                <br />
                                Load Data File:<br />
                                <br />
                                <asp:FileUpload ID="fupExcelFile" runat="server" />
                            </asp:WizardStep>
                            <asp:WizardStep ID="WizardStep2" runat="server" Title="Choose Excel Sheet">
                                <br />
                                Choose Sheet to Load:<br />
                                <br />
                                <asp:DropDownList ID="ddlExcelSheets" runat="server">
                                </asp:DropDownList>
                            </asp:WizardStep>
                            <asp:WizardStep ID="WizardStep4" runat="server" Title="Map Columns">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                                Map Columns Between Spreadsheet and Table:<br />
                                <asp:Button ID="btnSaveMap" runat="server" Text="Save Map" Visible="false" />
                                <br />

                                <asp:GridView ID="gvColumns" runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="ColumnName" HeaderText="Excel Sheet Column" />
                                        <asp:BoundField DataField="ColumnSize" HeaderText="Size" />
                                        <asp:BoundField DataField="DataType" HeaderText="Data Type" />
                                        <asp:TemplateField HeaderText="Mapped Column">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlDestinationColumn" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                Found rows: <asp:Label ID="lblNumRows" runat="server" Text="0"></asp:Label>
                                <br />
                            </asp:WizardStep>
                            <asp:WizardStep ID="WizardStep5" runat="server" Title="Upload Data">
                                <br />
                                Final Check:<br />
                                <asp:GridView ID="gvColumnMap" runat="server">
                                </asp:GridView>
                                <br />
                                <asp:Label ID="lblFinalQuery" runat="server" Text=""></asp:Label>
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" />
                                <br />
                                <asp:Label ID="lblAlreadyAdded" runat="server" ForeColor="Red"></asp:Label>
                            </asp:WizardStep>
                        </WizardSteps>
                        <SideBarButtonStyle BackColor="#507CD1" Font-Names="Verdana" ForeColor="White" />
                        <NavigationButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
                            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
                        <SideBarStyle BackColor="#507CD1" Font-Size="0.9em" VerticalAlign="Top" CssClass="steppadding" />
                        <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                            Font-Bold="True" Font-Size="0.9em" ForeColor="White" HorizontalAlign="Center" />
                    </asp:Wizard>
                </center>
            </center>
        </div>
    </form>
</body>
</html>
