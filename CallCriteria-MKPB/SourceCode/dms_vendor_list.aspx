<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="dms_vendor_list.aspx.vb" Inherits="dms_vendor_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>DMS Vendor List</h2>

        <div class="dash-module dmsvendors" data-uiname="DMSVendors">
    <div class='datapointbig'>
        <table>
            <tbody></tbody>
        </table>
    </div>
    <div class="moduleTitle">
        DMS 100% Lender List <div class="loading">
            <img src="/img/gear.gif" />
        </div>
        <a class="expand-notif" style="cursor: pointer; float: right;"><i class="fa fa-external-link"></i></a>
    </div>
   
    <div class="moduleContent" style="overflow: auto; font-size:smaller">
        Add/Remove Lender names<br />
        <table class="detailsTable" cellpadding="0px" cellspacing="0px" id="DMSVendors" width="100%">
            <tbody></tbody>
        </table>
        <hr />
        Add Lender
        <table>
            <tr>
                <td>New Lender: </td>
                <td><input type="text" id="addVendor" /></td>
            </tr>
            <tr>
                <td>Scorecard: </td>
                <td><select id="vendor_scorecard"></select></td>
            </tr>

        </table>

        <button type="button" onclick="addvendor();">Add</button>

        <script>
            $(document).ready(function () {

                load_scorecards();
                getvendors();
             
            })

            

            function getvendors() {
                $('.dmsvendors .loading').show();
                $('#DMSVendors tbody').empty();
                $.ajax({
                    type: "POST", //GET or POST or PUT or DELETE verb
                    url: "/CDService.svc/LoadDMSVendors", // Location of the service
                    contentType: "application/json; charset=utf-8", // content type sent to server
                    dataType: "json", //Expected data format from server
                    processdata: true, //True or False
                    success: function (msg) {//On Successfull service call

                        var jsonObj = msg.d
                        $(jsonObj).each(function (index, element) {
                            $('#DMSVendors').append('<tr><td>' + element.vendor + '</td><td>' + element.scorecard + '</td><td><span onclick="deletevendor(\'' + element.vendor + '\');">X</span></td>/tr>');
                        });

                        $('.dmsvendors .loading').hide();
                    },
                    error: ServiceFailed// When Service call fails
                });

            }

            function load_scorecards() {
                $.ajax({
                    type: "POST", //GET or POST or PUT or DELETE verb
                    url: "/CDService.svc/GetAppnames", // Location of the service
                    contentType: "application/json; charset=utf-8", // content type sent to server
                    data: '{"filter":" "}', //Data sent to server
                    dataType: "json", //Expected data format from server
                    processdata: true, //True or False
                    success: function (msg) {//On Successfull service call
                        var response = msg.d;

                        var jsonObj = eval(msg.d);

                        $("#vendor_scorecard option").remove();
                        $('#vendor_scorecard').append('<option value="">(select)</option>');

                        $(jsonObj).each(function (index, element) {
                            $('#vendor_scorecard').append('<option value="' + element.value + '">' + element.text.substring(0,25) + '</option>');
                        });

                       

                    },
                    error: ServiceFailed// When Service call fails
                });

            }


            function addvendor(vendor) {
                $('.caliListen .loading').show();
                $('#clientTable').empty();
                $.ajax({
                    type: "POST", //GET or POST or PUT or DELETE verb
                    url: "/CDService.svc/AddDMSVendors", // Location of the service
                    contentType: "application/json; charset=utf-8", // content type sent to server
                    data: '{"vendor":"' + $('#addVendor').val() + '","scorecard":"' + $('#vendor_scorecard').val() + '"}', //Data sent to server
                    dataType: "json", //Expected data format from server
                    processdata: true, //True or False
                    success: function (msg) {//On Successfull service call

                        getvendors();
                    },
                    error: ServiceFailed// When Service call fails
                });

            }


            function deletevendor(vendor) {
                $('.caliListen .loading').show();
                $('#clientTable').empty();
                $.ajax({
                    type: "POST", //GET or POST or PUT or DELETE verb
                    url: "/CDService.svc/DeleteDMSVendors", // Location of the service
                    contentType: "application/json; charset=utf-8", // content type sent to server
                    data: '{"vendor":"' + vendor + '"}', //Data sent to server
                    dataType: "json", //Expected data format from server
                    processdata: true, //True or False
                    success: function (msg) {//On Successfull service call

                        getvendors();
                    },
                    error: ServiceFailed// When Service call fails
                });

            }


        </script>

    </div>
</div>


    </section>
</asp:Content>

