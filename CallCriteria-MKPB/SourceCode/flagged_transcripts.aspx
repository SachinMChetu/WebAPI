<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" EnableEventValidation="false" CodeFile="flagged_transcripts.aspx.vb" Inherits="flagged_transcripts" %>

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
            margin: 0px auto;
        }

            .dash-modules .detailsTable th {
                border: none;
            }

            .dash-modules .detailsTable td {
                border: none;
                padding: 5px 10px;
            }

                .dash-modules .detailsTable td a.listen-btn {
                    position: relative;
                    display: inline-block;
                    width: 26px;
                    height: 26px;
                    border-radius: 100px;
                    background-color: #24353e;
                    margin: -2px -2px;
                    cursor: pointer;
                    overflow: hidden;
                    font-size: 0px;
                    text-align: center;
                }

                    .dash-modules .detailsTable td a.listen-btn:after {
                        display: inline-block;
                        content: '\f04b';
                        font-family: FontAwesome;
                        position: relative;
                        font-size: 10px;
                        color: #f9f9f9;
                        line-height: 26px;
                        margin-left: 1px;
                        margin-top: 1px;
                    }

                    .dash-modules .detailsTable td a.listen-btn:hover {
                        background-color: #334d5a;
                        width: 30px;
                        height: 30px;
                        margin: -4px -4px;
                        text-decoration: none;
                    }

                        .dash-modules .detailsTable td a.listen-btn:hover:after {
                            line-height: 30px;
                        }

                .dash-modules .detailsTable td a.hollow-link {
                    position: relative;
                    display: inline-block;
                    width: 26px;
                    height: 26px;
                    border-radius: 100px;
                    border: solid 2px transparent;
                    margin: -5px -5px;
                    cursor: pointer;
                    overflow: hidden;
                    font-size: 18px;
                    line-height: 26px;
                    color: #24353e;
                    text-align: center;
                }

                    .dash-modules .detailsTable td a.hollow-link:hover {
                        border-color: #24353e;
                    }

                    .dash-modules .detailsTable td a.hollow-link i {
                        position: relative;
                        padding: 0px;
                        top: -1px;
                    }

        .iframe-popup-wrap {
            display: none;
            position: fixed;
            width: 100%;
            height: 100%;
            background-color: rgba(240,240,240,0.95);
            cursor: pointer;
            z-index: 1100;
            top: 0px;
        }

            .iframe-popup-wrap.show {
                display: block;
            }

            .iframe-popup-wrap .iframe-popup {
                display: block;
                position: relative;
                width: 90%;
                width: calc(100% - 100px);
                max-width: 1000px;
                height: calc(100% - 100px);
                margin: 50px auto;
                cursor: auto;
            }

                .iframe-popup-wrap .iframe-popup iframe {
                    border: none;
                    width: 100%;
                    height: 100%;
                    max-width: 1200px;
                }
    </style>

    <script>
        function showPopupIframe(url) {
            $('.iframe-popup-wrap').addClass('show');
            $('.iframe-popup-wrap iframe').attr('src', url);
        }
        $(function () {
            $('.iframe-popup-wrap').not('.iframe-popup').click(function () {
                $('.iframe-popup-wrap').removeClass('show');
                $('.iframe-popup-wrap iframe').attr('src', '');
            });
        });
    </script>

    <div class="iframe-popup-wrap">
        <div class="iframe-popup">
            <iframe src=""></iframe>
        </div>
    </div>

    <section class="main-container dash-modules general-button">
        <h2 class="header-title">Flagged Transcripts</h2>

        <table class="control-table">
            <tr>
                <td class="input-label">Select scorecard:</td>
                <td>
                    <span class="select-wrap">
                        <asp:DropDownList ID="ddlScorecard" DataSourceID="dsSC"
                            DataTextField="scorecard_name" AppendDataBoundItems="true"
                            DataValueField="ID" runat="server">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                        </asp:DropDownList></span>
                    <asp:SqlDataSource ID="dsSC" SelectCommand="select short_name as scorecard_name, ID, appname 
                            from scorecards  where appname in (select appname from userapps where username = @username) 
                            and active =1 and transcribe = 1 order by appname, short_name"
                        ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server">
                        <SelectParameters>
                            <asp:Parameter Name="username" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                </td>
            </tr>

            <tr>
                <td class="input-label">Flagged:</td>
                <td>
                    <span class="select-wrap">
                        <asp:DropDownList ID="ddlFlagged" runat="server">
                            <asp:ListItem Text="Both" Value="2"></asp:ListItem>
                            <asp:ListItem Value="1">Flagged</asp:ListItem>
                            <asp:ListItem Value="0">Not Flagged</asp:ListItem>

                        </asp:DropDownList></span>
                </td>
            </tr>


            <tr>
                <td colspan="2">
                    <asp:TextBox ID="start_date" runat="server" PlaceHolder="Start Date" CssClass="short-input" />
                    -
               		<asp:TextBox ID="end_date" runat="server" PlaceHolder="End Date" CssClass="short-input" />
                </td>
            </tr>

            <tr>

                <td class="button-cell" colspan="2">
                    <asp:Button ID="btnGO" runat="server" Text="Go" CssClass="darkBtn" />
                    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="darkBtn" />
                </td>
            </tr>

        </table>



        <asp:GridView CssClass="detailsTable freezable" EnableViewState="false" AutoGenerateColumns="false"
            Font-Size="Smaller" Caption="" AllowSorting="true" ID="gvPMComp" DataSourceID="dsPMComp" runat="server">
            <Columns>
                <asp:BoundField DataField="Call_date" HeaderText="Call Date" SortExpression="call_date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="TRANSCRIPT_ANALYZED" HtmlEncode="false" HeaderText="Date Analyzed" SortExpression="TRANSCRIPT_ANALYZED" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="agent" HeaderText="Agent" SortExpression="agent" />
                <asp:BoundField DataField="phone" HeaderText="Phone" SortExpression="phone" />
                <asp:HyperLinkField DataNavigateUrlFields="f_id" DataTextField="f_id" DataNavigateUrlFormatString="review_record.aspx?ID={0}" Target="_blank" />
                <asp:BoundField DataField="total_score" HeaderText="Score" SortExpression="total_score" />
                <asp:BoundField DataField="missed_list" HeaderText="Missed List" SortExpression="missed_list" />
                <asp:BoundField DataField="transcript_count" HeaderText="Flagged Count" SortExpression="transcript_count" />
                
                <asp:BoundField DataField="TRANSCRIPT_FLAGGED_REASON" HeaderText="Flagged Reason" HtmlEncode="false" SortExpression="TRANSCRIPT_FLAGGED_REASON" />
                <asp:BoundField DataField="call_status" HeaderText="Status" SortExpression="call_status" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbMark" CommandArgument='<%#Eval("ID") %>' runat="server" CssClass="hollow-link mark-btn" ToolTip="Mark for Review"><i class="fa fa-flag" aria-hidden="true"></i></asp:LinkButton>
                        <asp:HiddenField ID="lblFlag" runat="server" Value='<%#Eval("ID") %>'></asp:HiddenField>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="audio_link" Text="Listen" DataNavigateUrlFormatString="{0}" Target="_blank">
                    <ControlStyle CssClass="listen-btn"></ControlStyle>
                </asp:HyperLinkField>
                <%-- <asp:HyperLinkField DataNavigateUrlFields="id" Text="View Transcript" DataNavigateUrlFormatString="view_transcript.aspx?ID={0}" Target="_blank"><ControlStyle CssClass="view-transcript-btn"></ControlStyle></asp:HyperLinkField> --%>
                <asp:TemplateField>
                    <ItemTemplate>
                        <!-- <a href="javascript:showPopupIframe('view_transcript.aspx?ID=<%#eval("ID") %>&noheader=true');" title="View Transcript" class="hollow-link view-transcript-btn"><i class="fa fa-eye" aria-hidden="true"></i></a> -->
                        <a href="view_transcript.aspx?ID=<%#eval("ID") %>" target="_blank" title="View Transcript" class="hollow-link view-transcript-btn"><i class="fa fa-eye" aria-hidden="true"></i></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="flagged_by_date" HeaderText="Flagged Date" SortExpression="flagged_by_date" />
                <asp:BoundField DataField="flagged_by" HeaderText="Flagged By" SortExpression="flagged_by" />
                <%--  <asp:BoundField DataField="" HeaderText="" SortExpression="" />
                <asp:BoundField DataField="" HeaderText="" SortExpression="" />
                <asp:BoundField DataField="" HeaderText="" SortExpression="" />
                <asp:BoundField DataField="" HeaderText="" SortExpression="" />
                <asp:BoundField DataField="" HeaderText="" SortExpression="" />--%>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsPMComp" ConnectionString="<%$ ConnectionStrings:estomesManual %>" runat="server"
            SelectCommand="select agent,TRANSCRIPT_FLAGGED_REASON, flagged_by_date, flagged_by,
                        call_date, TRANSCRIPT_ANALYZED, transcript_count, total_score,missed_list,phone,call_duration,audio_link,xcc_report_new.id, max_reviews,
                        form_score3.id as f_id, 
                        case when max_reviews = 99 then 'Pending Transcription' 
                        when max_reviews = 0 then 'Ready' 
                        when max_reviews = 1 and bad_call = 1  then 'Bad Call'              
                        when max_reviews in ( 1,2) and form_score3.id is not null then 'Worked'          
                        when max_reviews in ( 1,2) and form_score3.id is null then 'Transcribed Only'
                        else '' end as call_status from xcc_report_new with (nolock) 
                        left join form_score3  with (nolock) on form_score3.review_id = xcc_report_new.id 
                        where call_date between @date1 and @date2 and scorecard = @scorecard and transcript_analyzed is not null">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlScorecard" Name="scorecard" />
                <asp:ControlParameter ControlID="ddlFlagged" Name="flagged" ConvertEmptyStringToNull="false" />
                <asp:ControlParameter ControlID="start_date" Name="date1" />
                <asp:ControlParameter ControlID="end_date" Name="date2" />
            </SelectParameters>

        </asp:SqlDataSource>


        <script type="text/javascript">
            $(document).ready(function () {


                $('#ContentPlaceHolder1_start_date').datepicker({
                    dateFormat: "mm/dd/yy"
                });
                $('#ContentPlaceHolder1_end_date').datepicker({
                    dateFormat: "mm/dd/yy"
                });

                $('.freezable').each(function () {
                    $(this).tableHeadFixer({ 'left': 2 });

                })


            });

        </script>


    </section>
</asp:Content>

