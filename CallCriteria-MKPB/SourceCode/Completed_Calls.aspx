<%@ Page Title="" Language="C#" MasterPageFile="~/CC_Master.master" AutoEventWireup="true" CodeFile="Completed_Calls.aspx.cs" Inherits="Completed_Calls" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .dash-modules .detailsTable td {
            font-size: xx-small;
            border-right: solid 1px #f3f4f4;
            vertical-align: middle;

        }
         .selected {
        background-color: #ddffe7!important;
    }
         tr.paging-table,table {
             width:auto!important;
         }
         .headingsearch td{
           
         }
         .header-table td input[type="text"]{
                  height: 32px;
         }
         .header-table td
         {
             padding-right: 3%;
         }
        .auto-style1 {
            width: 298px;
        }
        .auto-style4 {
            width: 168px;
        }
        .auto-style6 {
            width: 203px;
        }
        /*#ContentPlaceHolder1_txtscorecard{
            height:37px;
            width:150px;
        }
        #ContentPlaceHolder1_txtqa{
             height:37px;
             width: 136px;
        }*/
        .auto-style7 {
            width: 152px;
        }
        .auto-style8 {
            width: 20px;
        }
        /*#ContentPlaceHolder1_txtefficnum
        {
             height:37px;
             width: 136px;
        }
        #ContentPlaceHolder1_txtmissedpoints
        {
            height:36px;
        }*/
        .auto-style9 {
            height: 75px;
        }
        .auto-style10 {
            width: 20px;
            height: 75px;
        }
        .auto-style11 {
            height: 75px;
        }
        .auto-style12 {
            width: 298px;
            height: 75px;
        }
        .auto-style13 {
            width: 162px;
        }
        .auto-style14 {
            width: 136px;
        }
        .auto-style15 {
            width: 96px;
            height: 75px;
        }
        .auto-style16 {
            width: 96px;
        }
        #ContentPlaceHolder1_UpdatePanel1 input[type="text"]
        {
             height:36px;
             width: 132px;
        }
         #ContentPlaceHolder1_UpdatePanel1 select
        {
             height:37px;
             width: 136px;
        }
         #ContentPlaceHolder1_txtcommentsspotcheck 
         {
             width: 160px!important;
         }
         .jdstabl
          {
    background-color: #F8F8F8;
    border-right: 1px solid #888;
    border-bottom: 1px solid #888;
    padding: 5px;
          }
    </style>
   
<%--    ////////////////////--%>

<%--    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
<link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css"
    rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $("[id*=spotcheckbtn]").live("click", function () {
        $('#ContentPlaceHolder1_hdnfid').val($(this).parents('tr').attr('id'));
        $("#modal_dialog").dialog({
            title: "ADD SPOT CHECK COMMENTS",
            buttons: {
                Close: function () {
                    $(this).dialog('close');
                }
            },
            modal: true
        });
        return false;
    });

</script>--%>

    <%--    ////////////////////--%>


    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>
              <section class="main-container dash-modules general-button">
        <h2>Completed Calls</h2>
                  <asp:HiddenField ID="hdnIdusrname" runat="server" />
                  <table style="width: 100%;" class="header-table">
                      <tr class="headingsearch">
                          <td class="auto-style13">
                              <asp:Label ID="Label5" runat="server" Text="Label" Font-Size="Small" Font-Bold="true">DATE FROM</asp:Label>
                          </td><td class="auto-style8">-</td>
                          <td class="auto-style16">
                              <asp:Label ID="Label6" runat="server" Text="Label" Font-Size="Small" Font-Bold="true">DATE TO</asp:Label>
                          </td>
                          <td class="auto-style7">
                              <asp:Label ID="Label1" runat="server" Text="Label" Font-Size="Small" Font-Bold="true">SCORE CARD</asp:Label>
                          </td>
                          <td class="auto-style4">
                              <asp:Label ID="Label2" runat="server" Text="Label" Font-Size="Small" Font-Bold="true">QA</asp:Label>
                          </td>
                          
                          <td class="auto-style6">
                              <asp:Label ID="Label4" runat="server" Text="Label" Font-Size="Small" Font-Bold="true">AGENT GROUP</asp:Label>
                          </td>
                          <td class="auto-style6">
                              <asp:Label ID="Label7" runat="server" Text="Label" Font-Size="Small" Font-Bold="true">MISSED POINTS</asp:Label>
                          </td>

                            <td class="auto-style6">
                              <asp:Label ID="Label8" runat="server" Text="Label" Font-Size="Small" Font-Bold="true">MISSED  CALIB POINTS</asp:Label>
                          </td>

                          <td class="auto-style1">&nbsp;</td>
                      </tr>
                      <tr>
                          <td class="auto-style13">
                              <asp:TextBox ID="txtDatefrom" runat="server"></asp:TextBox>
                          </td>
                          <td class="auto-style8"></td>
                          <td class="auto-style16">
                              <asp:TextBox ID="txtDateTo" runat="server"></asp:TextBox>
                          </td>
                          <td class="auto-style7">

                              <asp:DropDownList ID="txtscorecard" runat="server" DataTextField="short_name" DataValueField="ID" AppendDataBoundItems="true">
                                  <items>
                                      <asp:ListItem Text="Select" Value="">Select</asp:ListItem>
                                  </items>
                                  

                              </asp:DropDownList>
                             <%-- <asp:TextBox ID="txtscorecard" runat="server"></asp:TextBox>--%>
                             


                          </td>
                          <td class="auto-style4">
                              <%--<asp:TextBox ID="txtqa" runat="server" ></asp:TextBox>--%>
                              <asp:DropDownList ID="txtqa" runat="server" DataTextField="reviewer" DataValueField="reviewer">
                                  <items>
                                      <asp:ListItem Text="Select" Value="">Select</asp:ListItem>
                                  </items>                                 

                              </asp:DropDownList>
                               <asp:HiddenField ID="HiddenQAs" runat="server" />
                          </td>
                          
                          <td class="auto-style6">
                             <%-- <asp:TextBox ID="txtefficnum" runat="server"></asp:TextBox>--%>
                               <asp:DropDownList ID="txtefficnum" runat="server" DataTextField="AGENT_GROUP" DataValueField="AGENT_GROUP" AppendDataBoundItems="true">
                                  <items>
                                      <asp:ListItem Text="Select" Value="">Select</asp:ListItem>
                                  </items>
                                  

                              </asp:DropDownList>
                          </td>
                           <td class="auto-style6">
                             <%-- <asp:TextBox ID="txtmissedpoints" runat="server" ></asp:TextBox>--%>
                                <asp:DropDownList ID="txtmissedpoints" runat="server" DataTextField="F_ID" DataValueField="F_ID">
                               <%-- <asp:DropDownList ID="txtmissedpoints" runat="server">--%>
                                  <items>
                                      <asp:ListItem Text="Select" Value="">Select</asp:ListItem>
                                  </items>
                              </asp:DropDownList>
                                <asp:HiddenField ID="Hiddenmissed" runat="server" />
                          </td>


                            <td class="auto-style6">
                             <%-- <asp:TextBox ID="txtmissedpoints" runat="server" ></asp:TextBox>--%>
                                <asp:DropDownList ID="ddlMissedCalib" runat="server" DataTextField="q_short_name" DataValueField="q_ID">
                               <%-- <asp:DropDownList ID="txtmissedpoints" runat="server">--%>
                                  <items>
                                      <asp:ListItem Text="Select" Value="">Select</asp:ListItem>
                                  </items>
                              </asp:DropDownList>

                                <asp:SqlDataSource ID="dsMissedCalib" SelectCommand="select distinct q_short_name + ' (' + short_name + ')' as ddl_display, short_name, section_order, q_order, q_short_name, questions.ID
                                        from vwForm join form_q_scores on f_id = form_id join questions on questions.id = question_id
                                        join scorecards on scorecards.ID = scorecard
                                        join sections on sections.ID = questions.section
                                        where review_date between @start and @end
                                        and scorecard in (select user_scorecard from userapps where username = @username)
                                        and 1 = case when @scorecard is not null and @scorecard <> scorecard then 0 else 1 end
                                        order by short_name, section_order, q_order, q_short_name"  runat="server">
                                    <SelectParameters>
                                       <asp:ControlParameter Name="start" ControlID="txtDatefrom" />
                                        <asp:ControlParameter Name="end" ControlID="txtDateTo" />
                                        <asp:ControlParameter Name="scorecard" ControlID="txtscorecard" />
                                        <asp:Parameter Name="username" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                          </td>



                          <td class="auto-style1">
                              <asp:Button ID="btnsearch" runat="server" Text="SEARCH" Font-Bold="true" BackColor="#41637c" BorderColor="#999999" ForeColor="White" OnClick="btnsearch_Click" />
                          </td>
                      </tr>
                      <tr>
                          <td class="auto-style9" colspan="2">
                               <asp:Label ID="Label3" runat="server" Font-Bold="true" Font-Size="Small" Text="Label">NOTES</asp:Label>
                               <asp:TextBox ID="txtcommentsspotcheck" runat="server"></asp:TextBox>
                          
                         
                          </td>
                          <td class="auto-style15">
                              <br />
                               <asp:Button ID="btnallspotcheck" runat="server" BackColor="#41637c" BorderColor="#999999" Font-Bold="true" ForeColor="White" Text="SPOT CHECK" />
                             
                          </td>
                          <td class="auto-style11" colspan="3">
                              <asp:Label ID="labelcount" runat="server" Text=""></asp:Label>
                             
                          </td>
                          <td>
                               <asp:Button ID="btnshowall" runat="server" BackColor="#41637c" BorderColor="#999999" Font-Bold="true" ForeColor="White" Text="SHOW ALL" OnClick="btnshowall_Click" Visible="False" />
                          </td>
                      </tr>
                      <tr>
                          <td class="auto-style13">&nbsp;</td>
                          <td class="auto-style8">&nbsp;</td>
                          <td class="auto-style16">&nbsp;</td>
                          <td class="auto-style6">&nbsp;</td>
                          <td class="auto-style1">&nbsp;</td>
                      </tr>
                  </table>
        <%-- ////////////////////////--%>
                 <%-- <div id="modal_dialog" style="display: none;width:500px;">
    <table>
        <tr>
            <td>ADD NOTES/COMMENTS</td>
            <td> <asp:TextBox ID="TTXTINDSPOTPOPUP" runat="server"></asp:TextBox></td>

        </tr>
        <tr>
             <td> <asp:Button ID="btnnotesubmit" runat="server" Text="SUBMIT" Font-Bold="true" BackColor="#41637c" BorderColor="#999999" ForeColor="White" OnClick="btnnotesubmit_Click"/></td>
            <td><asp:HiddenField ID="hdnfid" runat="server" /></td>
        </tr>
    </table>
                      </div>--%>

                <%--  //////////////////////--%>
         <div style="font-size: xx-small">
             
             <asp:GridView ID="GridViewcall1" CssClass="detailsTable" AllowSorting="True" Style="font-size: xx-small;" runat="server" 
                 AutoGenerateColumns="False" DataKeyNames="F_ID" OnRowDataBound="GridViewcall1_RowDataBound" 
                 PageSize="50" OnSelectedIndexChanged="GridViewcall1_SelectedIndexChanged"  OnPageIndexChanging="OnPageIndexChanging" OnSorting="GridViewcall1_Sorting">
                <Columns>
                    <asp:TemplateField>                       
                        <ItemTemplate>
                            <asp:Button ID="spotcheckbtn" CssClass="spotlist" runat="server" Text="SPOT CHECK" Font-Bold="true" BackColor="#41637c" BorderColor="#999999" ForeColor="White"/>
                           <%-- <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                  <%--  <asp:HyperLinkField DataNavigateUrlFields="F_ID" DataNavigateUrlFormatString="edit_metadata.aspx?XID={0}" Text="Edit" />--%>
                     <asp:TemplateField> 
                         <HeaderTemplate>
                              <asp:Label ID="noteshead" runat="server" Text="NOTES"></asp:Label>
                         </HeaderTemplate>                      
                        <ItemTemplate>
                           <asp:TextBox id="txtindivnotes" CssClass="indnotesval" TextMode="multiline" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    

                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input type="checkbox" name="select-all" id="select-all" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDel" runat="server"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="appname" HeaderText="appname" SortExpression="appname" />
                  
                    <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
                    <asp:BoundField DataField="reviewer" HeaderText="reviewer" SortExpression="reviewer" />
                   <asp:BoundField DataField="AGENT" HeaderText="AGENT" SortExpression="AGENT" />
                   <%-- <asp:BoundField DataField="TIMESTAMP" HeaderText="TIMESTAMP" SortExpression="TIMESTAMP" />
                    <asp:BoundField DataField="AGENT_GROUP" HeaderText="AGENT GROUP" SortExpression="AGENT_GROUP" />--%>
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                    
                 <%--<asp:BoundField DataField="call_length" HeaderText="call length" SortExpression="call_length" />--%>
                    
                   <asp:BoundField DataField="review_date" HeaderText="review date" InsertVisible="False" ReadOnly="True" SortExpression="review_date" />

                    <asp:BoundField DataField="Efficiency" HeaderText="Efficiency" SortExpression="Efficiency" />
                    <asp:BoundField DataField="call length" HeaderText="call length" SortExpression="call length" />
                    <asp:BoundField DataField="pass_fail" HeaderText="Result" SortExpression="pass_fail" />
                    <asp:BoundField DataField="total_score" HeaderText="total score" SortExpression="total_score" />
                     <%-- <asp:BoundField DataField="address" HeaderText="Address" SortExpression="address" />
                      <asp:BoundField DataField="phone" HeaderText="phone" SortExpression="phone" />
                    <asp:BoundField DataField="call_date" HeaderText="call_date" SortExpression="call_date" />
                    <asp:BoundField DataField="date_added" HeaderText="date_added" SortExpression="date_added" />
                    <asp:BoundField DataField="website" HeaderText="website" SortExpression="website" />
                    <asp:CommandField ShowDeleteButton="true" DeleteText="Delete" /> --%>
                </Columns>
                <%-- <PagerStyle CssClass="paging-table" />--%>
            </asp:GridView>
              <table>
                            <tr>
             <asp:Repeater ID="rptPager" runat="server">
<ItemTemplate> 
    <%# Int32.Parse(Eval("Value").ToString()) % 50 == 1 ? "</tr><tr>" : "" %>
    <td class="jdstabl">
   <asp:LinkButton ID="lnkPage" runat="server" Text = '<%#Eval("Text") %>' CommandArgument = '<%# Eval("Value") %>' Enabled = '<%# Eval("Enabled") %>' OnClick = "Page_Changed"></asp:LinkButton>

</td>
</ItemTemplate>                
</asp:Repeater> </tr>
             </table>
             

            
             
           

             </div>
                  </section>


                 

             </ContentTemplate>
            
         </asp:UpdatePanel>

    
<script type="text/javascript">



    $("#ContentPlaceHolder1_txtDatefrom").datepicker({
        dateFormat: 'yy-mm-dd',
        //dateFormat: 'yy-mm-dd'
        maxDate: new Date()
    });

    $("#ContentPlaceHolder1_txtDateTo").datepicker({
        dateFormat: 'yy-mm-dd',
        //dateFormat: 'yy-mm-dd'
        maxDate: new Date()
    });
                     var prm = Sys.WebForms.PageRequestManager.getInstance();
                     if (prm != null) {
                         prm.add_endRequest(function (sender, e) {
                             if (sender._postBackSettings.panelsToUpdate != null) {
                                 $("#ContentPlaceHolder1_txtDatefrom").datepicker({
                                     //dateFormat: 'yy-mm-dd'
                                     dateFormat: 'yy-mm-dd',
                                     maxDate: new Date()
                                 });

                                 $("#ContentPlaceHolder1_txtDateTo").datepicker({
                                     //dateFormat: 'yy-mm-dd'
                                     dateFormat: 'yy-mm-dd',
                                     maxDate: new Date()
                                 });

                                 /////////////////////////////////
                                 $(document).ready(function () {
                                     if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {
                                     }
                                     else {

                                         GetMyQuestions();
                                         GetQas();
                                     }
                                 });

                                 $('#ContentPlaceHolder1_txtqa').on('click', function () {
                                     $('#ContentPlaceHolder1_HiddenQAs').val($('#ContentPlaceHolder1_txtqa').val());
                                     if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {
                                        
                                         alert('select DATE FROM and DATE TO for QA.');
                                     }
                                 });

                                 $('#ContentPlaceHolder1_txtDatefrom').on('change', function () {
                                     if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {
                                     }
                                     else {

                                         GetMyQuestions();
                                         GetQas();
                                     }
                                 });
                                 $('#ContentPlaceHolder1_txtDateTo').on('change', function () {
                                     if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {
                                         //alert('select DATE FROM and DATE TO for missed points.');
                                     }
                                     else {

                                         GetMyQuestions();
                                         GetQas();
                                     }
                                 });

                                 //$('#ContentPlaceHolder1_txtscorecard').on('change', function () {
                                 //    if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "" || $('#ContentPlaceHolder1_txtscorecard').val() == "") {
                                 //        //alert('select DATE FROM and DATE TO for missed points.');
                                 //    }
                                 //    else {

                                 //        GetMyQuestions();
                                 //    }
                                 //});

                                 $('#ContentPlaceHolder1_txtmissedpoints').on('click', function () {
                                     $('#ContentPlaceHolder1_Hiddenmissed').val($('#ContentPlaceHolder1_txtmissedpoints').val());
                                     if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {
                                         //$('#ContentPlaceHolder1_txtmissedpoints').hide();
                                         alert('select DATE FROM and DATE TO for missed points.');
                                     }
                                     else {
                                         // $('#ContentPlaceHolder1_txtmissedpoints').show();
                                         //GetMyQuestions();
                                     }
                                 });
                                 GetMyQuestions();
                                 GetQas();
                                 ///////////////////////////////

                                 $('.spotlist').click(function () {
                                     var idstring = $(this).parents('tr').attr('id');
                                     var usrname = $("#ContentPlaceHolder1_hdnIdusrname").val();
                                     var notes = $(this).parents("tr").find(".indnotesval").val();
                                     $.ajax({
                                         type: "POST",
                                         url: "Completed_Calls.aspx/addindividualspottable",
                                         data: '{isting: "' + idstring + '",curtname:"' + usrname + '",notes:"' + notes + '" }',
                                         contentType: "application/json; charset=utf-8",

                                         success: function (responsee) {
                                             //$('input[type="checkbox"]').each(function () {
                                             //});
                                             
                                             alert('successfully added to spot check');
                                         },
                                         failure: function (response) {

                                         }
                                     }); $(this).parents("tr").find(".indnotesval").val('');
                                     //return false;
                                 });


                                 // $('#select-all').live('click', function (event) {
                                 $('#select-all').click(function (event) {
                                     if (this.checked) {
                                         // Iterate each checkbox
                                         $(':checkbox').each(function () {
                                             this.checked = true;
                                         });
                                     }
                                     else {
                                         $(':checkbox').each(function () {
                                             this.checked = false;
                                         });
                                     }
                                 });




                                 // $('#ContentPlaceHolder1_btndelete').live('click', function () {
                                 $('#ContentPlaceHolder1_btnallspotcheck').click(function () {
                                     //alert("tt");
                                     var createrr = $("#ContentPlaceHolder1_hdnIdusrname").val();
                                     var istingg = "";
                                     var commenttext = $('#ContentPlaceHolder1_txtcommentsspotcheck').val();
                                     $('input[type="checkbox"]').each(function () {
                                         if ($(this).prop("checked") == true) {
                                             var s = $(this).parentsUntil("tr").parent().attr('id');

                                             if (s != undefined) {
                                                 istingg = istingg + s + ",";
                                                 //$(this).parentsUntil("tr").parent().hide();
                                             }

                                         }
                                     });
                                     $.ajax({
                                         type: "POST",
                                         url: "Completed_Calls.aspx/addspottable",
                                         data: '{isting: "' + istingg + '",curtname:"' + commenttext + '",creater:"' + createrr + '" }',
                                         contentType: "application/json; charset=utf-8",

                                         success: function (responsee) {
                                             //$('input[type="checkbox"]').each(function () {
                                             //});
                                             if (istingg != "") {
                                                 alert('successfully added to spot check');
                                                 $('#ContentPlaceHolder1_txtcommentsspotcheck').val("");
                                                 $('input[type="checkbox"]').each(function () {
                                                     if ($(this).prop("checked") == true) {
                                                         $(this).removeAttr('checked');
                                                     }
                                                 });

                                             }

                                         },
                                         failure: function (response) {

                                         }
                                     });
                                     return false;
                                 });


                                 



                             }
                         });
                     };

                    
    </script>
    <script>
        

       //// $('#ContentPlaceHolder1_btndelete').live('click', function () {
       //     $('#ContentPlaceHolder1_btndelete').click(function () {
       //     //alert("tt");
       //     var isting = "";
       //     $('input[type="checkbox"]').each(function () {
       //         if ($(this).prop("checked") == true) {
       //             var s = $(this).parentsUntil("tr").parent().attr('id');
                   
       //             if (s != undefined) {
       //                 isting = isting + s + ",";
       //                 $(this).parentsUntil("tr").parent().hide();
       //             }

       //         }
       //     });
       //     $.ajax({
       //         type: "POST",
       //         url: "Completed_Calls.aspx/DeleteCalls",
       //         data: '{isting: "' + isting + '" }',
       //         contentType: "application/json; charset=utf-8",

       //         success: function (responsee) {
       //             //$('input[type="checkbox"]').each(function () {
       //             //});
       //             //alert('success');
       //         },
       //         failure: function (response) {

       //         }
       //     });
       //     return false;
       // });



          
        $('.spotlist').click(function () {
            var idstring = $(this).parents('tr').attr('id');
            var usrname = $("#ContentPlaceHolder1_hdnIdusrname").val();
            var notes = $(this).parents("tr").find(".indnotesval").val();
            $.ajax({
                type: "POST",
                url: "Completed_Calls.aspx/addindividualspottable",
                data: '{isting: "' + idstring + '",curtname:"' + usrname + '",notes:"' + notes + '" }',
                contentType: "application/json; charset=utf-8",

                success: function (responsee) {
                    //$('input[type="checkbox"]').each(function () {
                    //});
                    $(this).parents("tr").find(".indnotesval").val('');
                    alert('successfully added to spot check');
                },
                failure: function (response) {

                }
            }); $(this).parents("tr").find(".indnotesval").val('');
            //return false;
        });


        // $('#select-all').live('click', function (event) {
        $('#select-all').click(function (event) {
            if (this.checked) {
                // Iterate each checkbox
                $(':checkbox').each(function () {
                    this.checked = true;
                });
            }
            else {
                $(':checkbox').each(function () {
                    this.checked = false;
                });
            }
        });




        // $('#ContentPlaceHolder1_btndelete').live('click', function () {
        $('#ContentPlaceHolder1_btnallspotcheck').click(function () {
            //alert("tt");
            var createrr = $("#ContentPlaceHolder1_hdnIdusrname").val();
            var istingg = "";
            var commenttext = $('#ContentPlaceHolder1_txtcommentsspotcheck').val();
            $('input[type="checkbox"]').each(function () {
                if ($(this).prop("checked") == true) {
                    var s = $(this).parentsUntil("tr").parent().attr('id');

                    if (s != undefined) {
                        istingg = istingg + s + ",";
                        //$(this).parentsUntil("tr").parent().hide();
                    }

                }
            });
            $.ajax({
                type: "POST",
                url: "Completed_Calls.aspx/addspottable",
                data: '{isting: "' + istingg + '",curtname:"' + commenttext + '",creater:"' + createrr + '" }',
                contentType: "application/json; charset=utf-8",

                success: function (responsee) {
                    //$('input[type="checkbox"]').each(function () {
                    //});
                    if (istingg != "") {
                        alert('successfully added to spot check');
                        $('#ContentPlaceHolder1_txtcommentsspotcheck').val("");
                        $('input[type="checkbox"]').each(function () {
                            if ($(this).prop("checked") == true) {
                                $(this).removeAttr('checked');
                            }
                        });

                    }

                },
                failure: function (response) {

                }
            });
            return false;
        });

        //////////////////////////////////////

        $(document).ready(function () {
            if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {
            }
            else {

                GetMyQuestions();
                GetQas();
            }
        });
      
        $('#ContentPlaceHolder1_txtDatefrom').on('change', function () {
            if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {
            }
            else {

                GetMyQuestions();
                GetQas();
            }
        });
        $('#ContentPlaceHolder1_txtDateTo').on('change', function () {
            if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {
                //alert('select DATE FROM and DATE TO for missed points.');
            }
            else {

                GetMyQuestions();
                GetQas();
            }
        });

        //$('#ContentPlaceHolder1_txtscorecard').on('change', function () {
        //    if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "" || $('#ContentPlaceHolder1_txtscorecard').val()=="") {
        //        //alert('select DATE FROM and DATE TO for missed points.');
        //    }
        //    else {

        //        GetMyQuestions();
        //    }
        //});

        $('#ContentPlaceHolder1_txtmissedpoints').on('click', function () {
            $('#ContentPlaceHolder1_Hiddenmissed').val($('#ContentPlaceHolder1_txtmissedpoints').val());
            if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {
                //$('#ContentPlaceHolder1_txtmissedpoints').hide();
                alert('select DATE FROM and DATE TO for missed points.');
            }
            else {
                // $('#ContentPlaceHolder1_txtmissedpoints').show();
                //GetMyQuestions();
            }
        });
        function GetMyQuestions() {

            $.ajax({
                type: "POST", //GET or POST or PUT or DELETE verb
                url: "/CDService.svc/GetMyQuestions", // Location of the service
                data: '{"start_date":"' + $('#ContentPlaceHolder1_txtDatefrom').val() + '","end_date":"' + $('#ContentPlaceHolder1_txtDateTo').val() + '"}', //Data sent to server
                contentType: "application/json; charset=utf-8", // content type sent to server
                dataType: "json", //Expected data format from server
                processdata: true, //True or False
                success: function (msg) {//On Successfull service call
                    var response = msg.d;
                    var jsonObj = eval(msg.d);

                    $("#ContentPlaceHolder1_txtmissedpoints option").remove();
                    //if ($('#ContentPlaceHolder1_Hiddenmissed').val() != "")
                    //{
                    //    $('#ContentPlaceHolder1_txtmissedpoints').append('<option value="">Select</option>');
                    //    $('#ContentPlaceHolder1_txtmissedpoints').append('<option selected="' + $('#ContentPlaceHolder1_Hiddenmissed').val() + '" value="' + $('#ContentPlaceHolder1_Hiddenmissed').val() + '">' + $('#ContentPlaceHolder1_Hiddenmissed').val() + '</option>');
                    //} else
                    //{
                    //    $('#ContentPlaceHolder1_txtmissedpoints').append('<option value="">Select</option>');
                    //}
                    $('#ContentPlaceHolder1_txtmissedpoints').append('<option value="">Select</option>');

                    $(jsonObj).each(function (index, element) {
                        if (element.value == $('#ContentPlaceHolder1_Hiddenmissed').val())
                        {
                            $('#ContentPlaceHolder1_txtmissedpoints').append('<option selected="' + element.value + '" value="' + element.value + '">' + element.text + '</option>');
                        }
                        else
                        {
                            $('#ContentPlaceHolder1_txtmissedpoints').append('<option value="' + element.value + '">' + element.text + '</option>');
                        }
                        
                    });

                },
                error: ServiceFailed// When Service call fails
            });
        //    var scorecard = $('#ContentPlaceHolder1_txtscorecard').val();
        //    var startdates = $('#ContentPlaceHolder1_txtDatefrom').val();
        //    var enddates = $('#ContentPlaceHolder1_txtDateTo').val();
        //    var usrname = $("#ContentPlaceHolder1_hdnIdusrname").val();
        //    if (scorecard == "")
        //    {
        //        scorecard = "0000";
        //    }

            //$.ajax({
            //    type: "POST", //GET or POST or PUT or DELETE verb
            //    url: "completed_calls.aspx/missedpopulate", // Location of the service
            //    data: '{"startdate":"' + startdates + '","enddate":"' + enddates + '",username:"' + usrname + '",scorecard:"' + scorecard + '"}', //Data sent to server
            //    contentType: "application/json; charset=utf-8", // content type sent to server
            //    dataType: "json", //Expected data format from server
            //    processdata: true, //True or False
            //    success: function (msg) {//On Successfull service call
            //        var response = msg.d;
            //        var jsonObj = eval(msg.d);

            //        $("#ContentPlaceHolder1_txtmissedpoints option").remove();
            //        $('#ContentPlaceHolder1_txtmissedpoints').append('<option value="">Select</option>');

            //        $(jsonObj).each(function (index, element) {
            //            $('#ContentPlaceHolder1_txtmissedpoints').append('<option value="' + element.value + '">' + element.text + '</option>');
            //        });

            //    },
            //    error: ServiceFailed// When Service call fails
            //});



        }
       
     //////////////////////////////////

        $('#ContentPlaceHolder1_txtqa').on('click', function () {
            $('#ContentPlaceHolder1_HiddenQAs').val($('#ContentPlaceHolder1_txtqa').val());
            if ($('#ContentPlaceHolder1_txtDatefrom').val() == "" || $('#ContentPlaceHolder1_txtDateTo').val() == "") {

                alert('select DATE FROM and DATE TO for QA.');
            }
        });
        function GetQas() {
            var startdates = $('#ContentPlaceHolder1_txtDatefrom').val();
            var enddates = $('#ContentPlaceHolder1_txtDateTo').val();
            //alert($('#ContentPlaceHolder1_HiddenQAs').val());
            $.ajax({
                type: "POST", //GET or POST or PUT or DELETE verb
                url: "completed_calls.aspx/QaPopulate", // Location of the service
                data: '{"startdate":"' + startdates + '","enddate":"' + enddates + '"}', //Data sent to server
                contentType: "application/json; charset=utf-8", // content type sent to server
                dataType: "json", //Expected data format from server
                processdata: true, //True or False
                success: function (msg) {//On Successfull service call
                    var response = msg.d;
                    var jsonObj = eval(msg.d);

                    $("#ContentPlaceHolder1_txtqa option").remove();
                    $('#ContentPlaceHolder1_txtqa').append('<option value="">Select</option>');
                    //if ($('#ContentPlaceHolder1_HiddenQAs').val() != "")
                    //{
                    //    $('#ContentPlaceHolder1_txtqa').append('<option value="">Select</option>');
                    //    $('#ContentPlaceHolder1_txtqa').append('<option selected="' + $('#ContentPlaceHolder1_HiddenQAs').val() + '" value="' + $('#ContentPlaceHolder1_HiddenQAs').val() + '">' + $('#ContentPlaceHolder1_HiddenQAs').val() + '</option>');
                        
                    //}
                    //else
                    //{
                    //    $('#ContentPlaceHolder1_txtqa').append('<option value="">Select</option>');
                    //}                  

                    $(jsonObj).each(function (index, element) {
                        if (element.value == $('#ContentPlaceHolder1_HiddenQAs').val())
                        {
                            $('#ContentPlaceHolder1_txtqa').append('<option selected="'+element.value+'" value="' + element.value + '">' + element.text + '</option>');
                        }else
                        {
                            $('#ContentPlaceHolder1_txtqa').append('<option value="' + element.value + '">' + element.text + '</option>');
                        }
                        //$('#ContentPlaceHolder1_txtqa').append('<option value="' + element.value + '">' + element.text + '</option>');
                    });

                },
                error: ServiceFailed// When Service call fails
            });
        }


    </script>



</asp:Content>


