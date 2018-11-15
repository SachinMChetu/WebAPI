<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="json_reader.aspx.vb" Inherits="json_reader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">
        <h2>JSOn reader</h2>
        Paste JSON here
        <asp:TextBox ID="TextBox1" TextMode="MultiLine" runat="server"></asp:TextBox>

        <button onclick="parse()" type="button">Go</button>
        <script>
            function parse() {
                var jdata = jQuery.parseJSON($('#ContentPlaceHolder1_TextBox1').val());
                //alert(jdata);
               

                jdata.forEach(getData);

                function getData(item, index) {
                    var names = Object.keys(item);
                    names.forEach(showData);

                    function showData(itemname, index) {
                        if (itemname.indexOf('author_name') > 0)
                            $('#text_target').append(item[itemname] + '<br>');
                        if (itemname.indexOf('_text') > 0)
                            $('#text_target').append(item[itemname] + '<br><br>');

                    }
                   
                }

            }
        </script>

        <div id="text_target"></div>
    </section>
</asp:Content>

