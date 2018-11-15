<%@ Page Language="C#" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="keyword_updates.aspx.cs" Inherits="keyword_updates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
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
                <a href="javascript:history.back()">Keyword Maitenance</a> > Update History

                <asp:GridView ID="gvKeywordChanges" CssClass="detailsTable" AllowSorting="true" DataSourceID="dsKeywordChanges" runat="server">                </asp:GridView>
                <asp:SqlDataSource ID="dsKeywordChanges" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    SelectCommand="SELECT * FROM Keyword_Changes WHERE Scorecard = @Scorecard ORDER BY ID DESC">
                    <SelectParameters>
                         <asp:QueryStringParameter Name="Scorecard" QueryStringField="Scorecard" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

