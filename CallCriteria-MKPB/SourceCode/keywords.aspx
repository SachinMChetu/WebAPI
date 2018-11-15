<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="keywords.aspx.vb" Inherits="keywords" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        .header-title {
            text-align: center;
            font-weight: 300;
            text-transform: uppercase;
            letter-spacing: 0.1em;
            font-size: 32px;
        }

        .darkBtn, .general-button input.darkBtn[type="submit"], .general-button input.darkBtn[type="button"] {
            display: inline-block;
            background-color: #8b949a;
            background-color: #41637c;
            background-color: #334759;
            color: #ffffff;
            font-size: 12px;
            line-height: 12px;
            padding: 12px 16px;
            margin: 8px 8px;
            text-transform: uppercase;
            letter-spacing: 0.15em;
            text-align: center;
            border: none;
            font-family: 'Open Sans', Arial, Helvetica, sans-serif;
            cursor: pointer;
            border-radius: 3px;
        }

            .darkBtn:hover, .general-button input.darkBtn[type="submit"]:hover, .general-button input.darkBtn[type="button"]:hover {
                background-color: #37434e;
            }

        .button-cell {
            text-align: center;
        }

        .centered-cell {
            text-align: center;
        }

        .control-table {
            padding: 10px;
            margin: 10px auto;
        }

            .control-table .select-wrap {
                display: inline-block;
                height: 36px;
                cursor: pointer;
            }

                .control-table .select-wrap:after {
                    font-family: FontAwesome;
                    content: '\f107';
                    position: absolute;
                    display: inline-block;
                    width: 30px;
                    margin-left: -30px;
                    text-align: center;
                    font-size: 20px;
                    color: #41637C;
                    line-height: 36px;
                    pointer-events: none;
                }

            .control-table select {
                font-size: 16px;
                width: 250px;
                padding: 8px 30px 8px 12px;
                -webkit-appearance: none;
                border-radius: 0px;
                border: solid 1px #cccccc;
                cursor: pointer;
                background-color: #ffffff;
            }

            .control-table .select-wrap:hover select {
                border: solid 1px #999999;
            }

            .control-table input[type="text"] {
                font-size: 16px;
                width: 224px;
                padding: 8px 12px;
                border-radius: 0px;
                border: solid 1px #cccccc;
                background-color: #ffffff;
            }

                .control-table input[type="text"].short-input {
                    width: 164px;
                }

                .control-table input[type="text"]:hover, .control-table input[type="text"]:focus {
                    border: solid 1px #999999;
                }

            .control-table .select-wrap, .control-table input[type="text"] {
                margin: 5px 10px;
            }

            .control-table .input-label {
                text-transform: uppercase;
                font-size: 13px;
                letter-spacing: 0.15em;
            }

        .dash-modules .detailsTable {
            border-color: transparent;
            margin: 10px auto;
        }

            .dash-modules .detailsTable th {
                border: none;
            }

            .dash-modules .detailsTable td {
                border: none;
                padding: 10px 15px;
            }

        .example-text {
            text-align: center;
            font-size: 0.8em;
            padding: 2px 0px 10px 0px;
        }

        .spacer-row {
            display: block;
            width: 100%;
            height: 1px;
            background-color: #cccccc;
            margin: 10px 0px;
        }

        .middle-space {
            width: 20px;
        }
    </style>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <section class="main-container dash-modules general-button">
                <h2 class="header-title">Keyword Maintenance</h2>

                <table class="control-table">
                    <tr>
                        <td colspan="5" class="centered-cell">
                            <asp:Label ID="lblScorecard" runat="server" Text="Scorecard:"></asp:Label>

                            <span class="select-wrap">
                                <asp:DropDownList ID="ddlScorecard" AppendDataBoundItems="true" DataSourceID="dsNICards" AutoPostBack="true" DataTextField="short_name" DataValueField="ID" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                </asp:DropDownList></span>
                            <asp:SqlDataSource ID="dsNICards" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                                SelectCommand="Select scorecards.id, short_name from userapps join scorecards on user_scorecard = scorecards.id 
											   where username = @username and active = 1 order by short_name"
                                runat="server">
                                <SelectParameters>
                                    <asp:Parameter Name="username" DefaultValue="stacemoss" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:HyperLink ID="hlChangelog" runat="server"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="spacer-row"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>New keyword:</td>
                        <td>
                            <asp:TextBox ID="txtNew" runat="server"></asp:TextBox></td>
                        <td class="middle-space"></td>
                        <td>Accepted Variants:</td>
                        <td>
                            <asp:TextBox ID="txtAccepted" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Type:</td>
                        <td>
                            <span class="select-wrap">
                                <asp:DropDownList ID="ddlNewType" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    <asp:ListItem>Banned</asp:ListItem>
                                    <asp:ListItem>Required</asp:ListItem>
                                </asp:DropDownList></span>
                        </td>
                        <td class="middle-space"></td>
                        <td>Problem:</td>
                        <td>
                            <span class="select-wrap">
                                <asp:DropDownList ID="ddlProblem" runat="server">
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    <asp:ListItem>Consumer was Incentivized</asp:ListItem>
                                    <asp:ListItem>Financial Cost</asp:ListItem>
                                    <asp:ListItem>Guarantees</asp:ListItem>
                                    <asp:ListItem>Length of program</asp:ListItem>
                                    <asp:ListItem>Post Degree Employment</asp:ListItem>
                                </asp:DropDownList></span>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="4" class="example-text">* <strong>qualify</strong> is banned, but <strong>may qualify</strong> would not be
                            <br />
                            use <strong>may qualify|can qualify</strong> -- separate by |
                        </td>
                        <td colspan="1" class="button-cell">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="darkBtn" />
                        </td>
                    </tr>

                </table>


                <asp:GridView ID="gvKeywords" CssClass="detailsTable" AllowSorting="true" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="dsKeywords" runat="server">
                    <Columns>
                        <asp:CommandField ShowEditButton="true" />
                        <asp:BoundField HeaderText="Keyword(s)" ControlStyle-Width="350" DataField="Utterance" SortExpression="Utterance" />
                        <asp:BoundField HeaderText="Minimum Count" ControlStyle-Width="350" DataField="Utterance_count" SortExpression="Utterance_count" />
                        <asp:TemplateField HeaderText="Problem" SortExpression="Problem">
                            <EditItemTemplate>

                                <asp:DropDownList ID="ddlProblem" runat="server" SelectedValue='<%# Bind("Problem") %>'>
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    <asp:ListItem>Consumer was Incentivized</asp:ListItem>
                                    <asp:ListItem>Financial Cost</asp:ListItem>
                                    <asp:ListItem>Guarantees</asp:ListItem>
                                    <asp:ListItem>Length of program</asp:ListItem>
                                    <asp:ListItem>Post Degree Employment</asp:ListItem>
                                </asp:DropDownList>

                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Problem") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="utterance_type" SortExpression="utterance_type">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlUType" runat="server" SelectedValue='<%# Bind("utterance_type") %>'>
                                    <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                                    <asp:ListItem>Banned</asp:ListItem>
                                    <asp:ListItem>Required</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("utterance_type") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Accepted Variant" ControlStyle-Width="350" DataField="accepted_variant" SortExpression="accepted_variant" />
                        <asp:CheckBoxField DataField="mandatory_review" HeaderText="Mandatory Review" SortExpression="mandatory_review" />
                        <asp:CommandField ShowDeleteButton="true" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="dsKeywords" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    UpdateCommand="IF NOT EXISTS (SELECT * FROM Utterance_Flags WHERE ID = @id AND mandatory_review = @mandatory_review AND Utterance_count = @Utterance_count AND utterance_type = @utterance_type AND accepted_variant = @accepted_variant AND Problem = @Problem AND Utterance = @Utterance)
	                    INSERT INTO Keyword_Changes (Scorecard, Update_By, Update_Type, Keyword_From, Min_Count_From, Problem_From, Utterance_Type_From, Accepted_Variant_From, Mandatory_Review_From, 
	                    Keyword_To, Min_Count_To, Problem_To, Utterance_Type_To, Accepted_Variant_To, Mandatory_Review_To)
	                    SELECT  U.Scorecard, @Username, 'Change', U.Utterance, U.Utterance_Count, U.Problem, U.Utterance_Type, U.Accepted_Variant, U.Mandatory_Review, 
	                    @Utterance, @Utterance_count, @Problem, @utterance_type, @accepted_variant, @mandatory_review
	                    FROM Utterance_Flags AS U WHERE U.ID = @id;
                        update utterance_flags set mandatory_review=@mandatory_review,Utterance_count=@Utterance_count, utterance_type=@utterance_type,accepted_variant=@accepted_variant, Problem=@Problem, Utterance=@Utterance where id =@id  "
                    DeleteCommand="INSERT INTO Keyword_Changes (Scorecard, Update_By, Update_Type, Keyword_From, Min_Count_From, Problem_From, Utterance_Type_From, Accepted_Variant_From, Mandatory_Review_From)
                        SELECT  U.Scorecard, @Username, 'Remove', U.Utterance, U.Utterance_Count, U.Problem, U.Utterance_Type, U.Accepted_Variant, U.Mandatory_Review 
                        FROM Utterance_Flags AS U WHERE U.ID = @id;
                        delete from utterance_flags where id =@id "
                    SelectCommand="select * from utterance_flags where scorecard = @scorecard">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="Username" Type="String" DefaultValue="Anonymous" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Username" Type="String" DefaultValue="Anonymous" />
                    </UpdateParameters>
                </asp:SqlDataSource>

            </section>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

