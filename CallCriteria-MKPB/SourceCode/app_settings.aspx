<%@ Page Title="App Settings" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="app_settings.aspx.vb" Inherits="app_settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="main-container dash-modules general-button">

        <h2>App Settings Management</h2>

        <!-- close general-filter -->




        Select App:
            <asp:DropDownList ID="ddlApps" AutoPostBack="true" AppendDataBoundItems="true" DataSourceID="dsApps" DataTextField="appname" DataValueField="appname" runat="server">
                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
            </asp:DropDownList>
        <asp:SqlDataSource ID="dsApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            SelectCommand="select appname from app_settings  order by active desc,appname" runat="server"></asp:SqlDataSource>

        Add new:
            <asp:TextBox ID="txtNewApp" runat="server"></asp:TextBox><asp:Button ID="btnAdd" runat="server" Text="Add" />

        <br />



        <asp:FormView ID="fvSettings" DataSourceID="dsSettings" runat="server" DefaultMode="Edit"
            DataKeyNames="id">
            <EditItemTemplate>
                <table>
                    <tr>
                        <td>Client Name :
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("FullName")%>' />
                        </td>
                        <td>Contact Name :
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("contact_name")%>' />
                        </td>
                        <td>Contact Phone :
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox12" runat="server" Text='<%# Bind("contact_phone")%>' />
                        </td>

                        <td>App Active :
                        </td>
                        <td>
                            <asp:CheckBox ID="DropDownList3" Checked='<%# Bind("active") %>' runat="server"></asp:CheckBox>
                        </td>


                    </tr>
                    <tr>

                        <td>Vertical for App :
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList2" runat="server" SelectedValue='<%# Bind("vertical")%>'>
                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                <asp:ListItem>Auto</asp:ListItem>
                                <asp:ListItem>Education</asp:ListItem>
                                <asp:ListItem>Insurance</asp:ListItem>
                                <asp:ListItem>Mortgage</asp:ListItem>
                                <asp:ListItem>Real Estate</asp:ListItem>
                                <asp:ListItem>Employment</asp:ListItem>
                                <asp:ListItem>Telecom</asp:ListItem>
                                <asp:ListItem>Financial</asp:ListItem>
                                <asp:ListItem>Home Improvement</asp:ListItem>
                            </asp:DropDownList>
                        </td>


                        <td>NA Scoring :
                        </td>
                        <td>

                            <asp:DropDownList ID="ddlNA" Width="200" SelectedValue='<%# Bind("NA_affect")%>' runat="server">
                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                <asp:ListItem Text="No Score Reduction" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Reduce Total Score" Value="1"></asp:ListItem>

                            </asp:DropDownList>
                        </td>

                        <td>App Difficulty :
                        </td>
                        <td>

                            <asp:DropDownList ID="ddlDifficulty" Width="200" SelectedValue='<%# Bind("app_difficulty")%>' runat="server">
                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                <asp:ListItem>Low</asp:ListItem>
                                <asp:ListItem>Medium</asp:ListItem>
                                <asp:ListItem>High</asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td>Truncate Scores (> 100 = 100, < 0 = 0) :
                        </td>
                        <td>
                            <asp:CheckBox ID="chktruncate" Checked='<%# Bind("truncate_scores")%>' runat="server"></asp:CheckBox>
                        </td>


                    </tr>
                    <tr>
                        <td>Billing Rate :
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillRate" runat="server" Text='<%# Bind("bill_rate") %>' />
                        </td>

                        <td>Base Score (start at score) :
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("default_score")%>' />
                        </td>

                        <td>Top Score (if all right, top score) :
                        </td>
                        <td>
                            <asp:TextBox ID="txttopscore" runat="server" Text='<%# Bind("top_score")%>' />
                        </td>
                        <td>Agent Summary Only (no Call Details) :
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAgentSummary" Checked='<%# Bind("agent_summary_only")%>' runat="server"></asp:CheckBox>
                        </td>
                    </tr>


                    <tr>

                        <td>First Notification Goes to :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFirstN" runat="server" SelectedValue='<%#Bind("first_noti") %>'>
                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                <asp:ListItem>Agent</asp:ListItem>
                                <asp:ListItem>Supervisor</asp:ListItem>
                                <asp:ListItem>Manager</asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td>Configuration Profile:</td>
                        <td>
                            <asp:DropDownList ID="ddlNotProfile" AutoPostBack="true" AppendDataBoundItems="true" DataSourceID="dsNotProfile"
                                DataTextField="profile_name" DataValueField="id" SelectedValue='<%#Bind("setting_profile") %>' runat="server">
                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dsNotProfile" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="select id, profile_name from notification_profiles  order by profile_name" runat="server"></asp:SqlDataSource>
                        </td>
                        <td>Show Section Score :
                        </td>
                        <td>
                            <asp:CheckBox ID="chkShowSection" Checked='<%# Bind("show_section_score")%>' runat="server"></asp:CheckBox></td>

                        <td>Stream Only:
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckBox1" Checked='<%# Bind("stream_only")%>' runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>

                        <td>Rejection Reason Profile:</td>
                        <td>
                            <asp:DropDownList ID="ddlRejection" AutoPostBack="true" AppendDataBoundItems="true" DataSourceID="dsRejection"
                                DataTextField="profile_name" DataValueField="id" SelectedValue='<%#Bind("rejection_profile") %>' runat="server">
                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dsRejection" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="select id, profile_name from rejection_profiles  order by profile_name" runat="server"></asp:SqlDataSource>
                        </td>

                        <td>App Dashboard:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDash" SelectedValue='<%#Bind("dashboard") %>' runat="server">
                                <asp:ListItem>old</asp:ListItem>
                                <asp:ListItem>new</asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td>Minumum Minutes
                        </td>
                        <td>
                            <asp:TextBox ID="minimum_minutesTextBox" runat="server" Text='<%# Bind("minimum_minutes") %>' />
                        </td>
                        <td>Transcript Rate
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox6" Width="50" runat="server" Text='<%# Bind("transcript_rate") %>' />
                        </td>



                    </tr>
                    <tr>
                        <td>Budget ($) :
                        </td>
                        <td>
                            <asp:TextBox ID="budgetTextBox" runat="server" Text='<%# Bind("budget") %>' />
                        </td>
                    </tr>

                    <tr>
                        <td>Recording URL :
                        </td>
                        <td>
                            <asp:TextBox ID="recording_urlTextBox" runat="server" Text='<%# Bind("recording_url") %>' />
                        </td>


                        <td>Recording User :
                        </td>

                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("recording_user") %>' />
                        </td>
                        <td>Recording Password :
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("record_password") %>' />
                        </td>
                        <td>Allow file upload to review page:
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckBox2" Checked='<%# Bind("allowDocumentUpload")%>' runat="server"></asp:CheckBox>
                        </td>

                    </tr>
                    <tr>
                        <td>Recording Date Format :
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("record_format")%>' />
                        </td>


                        <td>Recording Directories :
                        </td>
                        <td colspan="3">
                            <asp:TextBox Width="300" ID="TextBox1" TextMode="MultiLine" Rows="2" runat="server" Text='<%# Bind("recording_dirs") %>' />
                        </td>


                    </tr>

                </table>

                <table>
                    <tr>
                        <td>Client Logo:<br />
                            <asp:FileUpload ID="fupLogo" runat="server" />
                        </td>
                        <td>
                            <asp:Image ID="imgClientLogo" Height="100" runat="server" ImageUrl='<%#Eval("client_logo") %>' />

                        </td>

                        <td>Client Small Logo (100px X 100px):<br />
                            <asp:FileUpload ID="fupLogoSmall" runat="server" />
                        </td>
                        <td>
                            <asp:Image ID="imgClientLogoSmall" Width="50" runat="server" ImageUrl='<%#Eval("client_logo_small") %>' />

                        </td>
                    </tr>
                </table>

                <br />
                <table>
                    <tr>
                        <td><b>Email Server Settings for App:</b>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>SMTP Host:</td><td><asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("smtp_host") %>' /></td>
                        <td>SMTP Port:</td><td><asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("smtp_port") %>' /></td>
                        <td>SMTP Username:</td><td><asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("smtp_username") %>' /></td>
                    </tr>
                    <tr>
                        <td>SMTP Password:</td><td><asp:TextBox ID="TextBox13" runat="server" Text='<%# Bind("smtp_password") %>' /></td>
                        <td>Email Address (Ex: hello@callcriteria.com) : </td><td><asp:TextBox ID="TextBox14" runat="server" Text='<%# Bind("smtp_email") %>' /></td>
                        <td>Friendly Name (Ex: Call Criteria) :</td><td><asp:TextBox ID="TextBox15" runat="server" Text='<%# Bind("smtp_friendly_name") %>' /></td>
                    </tr>
                </table>

                <asp:Button CssClass="secondary-cta" ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                    Text="Update" />




                <table>
                    <tr>
                        <td style="vertical-align: top;">
                            <h4>Billing Rates</h4>
                            <asp:GridView ID="gvBillRate" CssClass="detailsTable" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="dsBillRate" AutoGenerateEditButton="True">
                                <Columns>
                                    <asp:BoundField DataField="start_minutes" HeaderText="Start Minutes" SortExpression="start_minutes" />
                                    <asp:BoundField DataField="end_minutes" HeaderText="End Minutes" SortExpression="end_minutes" />
                                    <asp:BoundField DataField="rate" HeaderText="rate" SortExpression="rate" />
                                    <asp:TemplateField HeaderText="Billing Type" SortExpression="bill_type">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%#Bind("bill_type") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlBillType" SelectedValue='<%#Bind("bill_type") %>' runat="server">
                                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                                <asp:ListItem>per Minute</asp:ListItem>
                                                <asp:ListItem>per Item</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Specific Scorecard" SortExpression="scorecard_only">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlScorecards" AppendDataBoundItems="true" Enabled="false" DataSourceID="dsScorecards" SelectedValue='<%#Bind("scorecard_only") %>' DataTextField="short_name" DataValueField="id" runat="server">
                                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                                <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlScorecards" AppendDataBoundItems="true" DataSourceID="dsScorecards" SelectedValue='<%#Bind("scorecard_only") %>' DataTextField="short_name" DataValueField="id" runat="server">
                                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                                <asp:ListItem Text="(Select)" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:CommandField ShowDeleteButton="true" />
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnNewBillRate" runat="server" Text="Add Rate" OnClick="btnNewBillRate_Click" />

                            Scorecard:
                
                            <asp:SqlDataSource ID="dsScorecards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="select scorecards.id, short_name from app_settings join scorecards on scorecards.appname = app_settings.appname where scorecards.active = 1 and app_settings.active = 1 order by short_name" runat="server"></asp:SqlDataSource>


                            <asp:SqlDataSource ID="dsBillRate" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                                DeleteCommand="DELETE FROM [billing_rates] WHERE [id] = @id"
                                InsertCommand="INSERT INTO [billing_rates] ([appname], [start_minutes], [end_minutes], [rate]) 
                    VALUES (@appname, @start_minutes, @end_minutes, @rate)"
                                ProviderName="System.Data.SqlClient"
                                SelectCommand="SELECT * FROM [billing_rates] WHERE ([appname] = @appname)  order by start_minutes"
                                UpdateCommand="UPDATE [billing_rates] SET [start_minutes] = @start_minutes, [end_minutes] = @end_minutes, [rate] = @rate, 
                                    bill_type=@bill_type, scorecard_only=@scorecard_only WHERE [id] = @id">
                                <DeleteParameters>
                                    <asp:Parameter Name="id" Type="Int32" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:ControlParameter ControlID="ddlApps" Name="appname" PropertyName="SelectedValue" Type="String" />
                                    <asp:Parameter Name="start_minutes" Type="Int32" />
                                    <asp:Parameter Name="end_minutes" Type="Int32" />
                                    <asp:Parameter Name="rate" Type="Double" />
                                </InsertParameters>
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlApps" Name="appname" PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="appname" Type="String" />
                                    <asp:Parameter Name="start_minutes" Type="Int32" />
                                    <asp:Parameter Name="end_minutes" Type="Int32" />
                                    <asp:Parameter Name="rate" Type="Double" />
                                    <asp:Parameter Name="id" Type="Int32" />
                                </UpdateParameters>
                            </asp:SqlDataSource>

                        </td>
                        <td style="vertical-align: top">

                            <h4>API Keys</h4>
                            <asp:GridView ID="gvAPI" CssClass="detailsTable" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="dsAPI">
                                <Columns>
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                                    <asp:BoundField DataField="api_key" HeaderText="api_key" SortExpression="api_key" ReadOnly="true" />
                                    <%--<asp:BoundField DataField="active" HeaderText="active" SortExpression="active" />--%>
                                    <asp:CheckBoxField DataField="active" HeaderText="active" SortExpression="active" />
                                    <asp:BoundField DataField="date_added" HeaderText="date_added" SortExpression="date_added" ReadOnly="true" DataFormatString="{0:MM/dd/yyyy}" />
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnNewAPI" OnClick="btnNewAPI_Click" runat="server" Text="New API Key" />
                            <asp:SqlDataSource ID="dsAPI" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                                DeleteCommand="DELETE FROM [API_KEYS] WHERE [id] = @id"
                                InsertCommand="INSERT INTO [API_KEYS] ([appname], [active] ) VALUES (@appname, 1)"
                                SelectCommand="SELECT * FROM [API_KEYS] WHERE ([appname] = @appname)"
                                UpdateCommand="UPDATE [API_KEYS] SET  [active] = @active WHERE [id] = @id">
                                <DeleteParameters>
                                    <asp:Parameter Name="id" Type="Int32" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="api_key" Type="Object" />
                                    <asp:ControlParameter ControlID="ddlApps" Name="appname" PropertyName="SelectedValue" Type="String" />
                                    <asp:Parameter Name="active" Type="Int32" />
                                    <asp:Parameter Name="date_added" Type="DateTime" />
                                </InsertParameters>
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlApps" Name="appname" PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="api_key" Type="Object" />
                                    <asp:Parameter Name="appname" Type="String" />
                                    <asp:Parameter Name="active" Type="Int32" />
                                    <asp:Parameter Name="date_added" Type="DateTime" />
                                    <asp:Parameter Name="id" Type="Int32" />
                                </UpdateParameters>
                            </asp:SqlDataSource>



                        </td>
                        <td style="vertical-align: top">
                            <h4>Export</h4>
                            <asp:GridView ID="gvAppFields" runat="server" AutoGenerateColumns="False" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" CssClass="detailsTable" DataKeyNames="id" DataSourceID="dsAppFields">
                                <Columns>
                                    <asp:TemplateField HeaderText="field" SortExpression="field">
                                        <EditItemTemplate>

                                            <asp:DropDownList ID="ddlFields" runat="server" DataSourceID="dsFields" DataTextField="column_name" DataValueField="column_name"
                                                AppendDataBoundItems="true" SelectedValue='<%# Bind("field") %>'>
                                                <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="dsFields" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                                SelectCommand="select distinct column_name from available_columns 
                                                where column_name not in (select field from app_specific_exports WHERE appname = @appname and field is not null ) 
                                                and isnull(column_required,0) = 0 order by column_name"
                                                runat="server">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddlApps" Name="appname" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("field") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="dsAppFields" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                DeleteCommand="DELETE FROM [app_specific_exports] WHERE [id] = @id"
                                InsertCommand="INSERT INTO [app_specific_exports] ([appname], [field], [sp]) VALUES (@appname, @field, @sp)" ProviderName="System.Data.SqlClient"
                                SelectCommand="SELECT * FROM [app_specific_exports] WHERE ([appname] = @appname)"
                                UpdateCommand="UPDATE [app_specific_exports] SET [field] = @field WHERE [id] = @id">
                                <DeleteParameters>
                                    <asp:Parameter Name="id" Type="Int32" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:ControlParameter ControlID="ddlApps" Name="appname" PropertyName="SelectedValue" Type="String" />
                                    <asp:Parameter Name="field" Type="String" />
                                    <asp:Parameter Name="sp" Type="String" />
                                </InsertParameters>
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlApps" Name="appname" PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="appname" Type="String" />
                                    <asp:Parameter Name="field" Type="String" />
                                    <asp:Parameter Name="sp" Type="String" />
                                    <asp:Parameter Name="id" Type="Int32" />
                                </UpdateParameters>
                            </asp:SqlDataSource>
                            <asp:Button ID="btnAddExport" runat="server" Text="Add Export" OnClick="btnAddExport_Click" /></td>
                    </tr>
                </table>



                <h4>Notes</h4>
                <asp:GridView ID="gvAppNotes" runat="server" AutoGenerateDeleteButton="true" AutoGenerateEditButton="true" CssClass="detailsTable" DataKeyNames="ID" DataSourceID="dsAppNotes">
                </asp:GridView>
                <asp:SqlDataSource ID="dsAppNotes" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" DeleteCommand="delete from app_Notes  where id = @id" SelectCommand="select * from app_Notes where appname = @appname" UpdateCommand="update app_Notes set note=@note where id = @id">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlApps" Name="appname" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:TextBox ID="txtNewNote" runat="server" TextMode="MultiLine"></asp:TextBox>
                <asp:Button ID="btnAddNote" runat="server" OnClick="btnAddNote_Click" Text="Add Note" />





            </EditItemTemplate>
            <EmptyDataTemplate>Select an app name to continue</EmptyDataTemplate>
        </asp:FormView>


        <asp:SqlDataSource ID="dsSettings" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
            DeleteCommand="DELETE FROM [app_settings] WHERE [id] = @id" InsertCommand="INSERT INTO [app_settings] ([recording_url], [recording_user], [record_password], [appname]) VALUES (@recording_url, @recording_user, @record_password, @appname)"
            SelectCommand="SELECT * FROM [app_settings] WHERE ([appname] = @appname)" UpdateCommand="UPDATE [app_settings] SET  bill_rate=@bill_rate, 
                FullName=@FullName, vertical=@vertical, default_score=@default_score,
                recording_dirs=@recording_dirs, active=@active, [recording_url] = @recording_url, [recording_user] = @recording_user, 
                [record_password] = @record_password, NA_affect=@NA_affect, truncate_scores=@truncate_scores, agent_summary_only=@agent_summary_only,
                top_score=@top_score, show_section_score=@show_section_score, minimum_minutes=@minimum_minutes,transcript_rate=@transcript_rate,
                record_format=@record_format,first_noti=@first_noti, stream_only=@stream_only, app_difficulty=@app_difficulty, contact_name=@contact_name,
                contact_phone=@contact_phone,setting_profile=@setting_profile, rejection_profile=@rejection_profile,allowDocumentUpload=@allowDocumentUpload,smtp_host=@smtp_host,
                smtp_port=@smtp_port,smtp_username=@smtp_username,smtp_password=@smtp_password,smtp_email=@smtp_email,smtp_friendly_name=@smtp_friendly_name,
                dashboard=@dashboard, budget=@budget WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="recording_url" Type="String" />
                <asp:Parameter Name="recording_user" Type="String" />
                <asp:Parameter Name="record_password" Type="String" />
                <asp:ControlParameter Name="appname" ControlID="ddlApps" />
            </InsertParameters>
            <SelectParameters>
                <asp:ControlParameter Name="appname" ControlID="ddlApps" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="recording_url" Type="String" />
                <asp:Parameter Name="app_difficulty" Type="String" />
                <asp:Parameter Name="recording_user" Type="String" />
                <asp:Parameter Name="record_password" Type="String" />
                <asp:Parameter Name="appname" Type="String" />
                <asp:Parameter Name="recording_dirs" Type="String" />
                <asp:Parameter Name="top_score" Type="String" />
                <asp:Parameter Name="listen_template" Type="String" />
                <asp:Parameter Name="active" Type="Boolean" />
                <asp:Parameter Name="isCalibrated" Type="Boolean" />
                <asp:Parameter Name="truncate_scores" Type="Boolean" />
                <asp:Parameter Name="show_section_score" Type="Boolean" />
                <asp:Parameter Name="auto_submit" Type="Boolean" />
                <asp:Parameter Name="email_on_fail" Type="Boolean" />
                <asp:Parameter Name="NA_affect" Type="String" />
                <asp:Parameter Name="calibration_percent" Type="String" />
                <asp:Parameter Name="first_noti" Type="String" />
                <asp:Parameter Name="contact_name" Type="String" />
                <asp:Parameter Name="contact_phone" Type="String" />
                <asp:Parameter Name="contact_email" Type="String" />
                <asp:Parameter Name="record_format" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
                <asp:Parameter Name="use_scorecard" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>





    </section>

</asp:Content>
