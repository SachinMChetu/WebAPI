<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="edit_apps.aspx.vb" Inherits="edit_apps" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        
        $(document).ready(function () {
            $('#move-up').click(moveUp);
            $('#move-down').click(moveDown);
        });

        function moveUp() {
             $('#ContentPlaceHolder1_ListBox1 :selected').each(function (i, selected) {
                if (!$(this).prev().length) return false;
                $(this).insertBefore($(this).prev());
            });
            $('#ContentPlaceHolder1_ListBox1').focus().blur();
            $('#ContentPlaceHolder1_btnSave').prop('disabled', false);
        }

        function moveDown() {
            $($('#ContentPlaceHolder1_ListBox1 :selected').get().reverse()).each(function (i, selected) {
                if (!$(this).next().length) return false;
                $(this).insertAfter($(this).next());
            });
            $('#ContentPlaceHolder1_ListBox1').focus().blur();
            $('#ContentPlaceHolder1_btnSave').prop('disabled', false);
        }

    </script>

    <section class="main-container listening-actions">

        <h1 class="section-title"><i class="fa fa-desktop"></i>
            <asp:Label ID="lblUser" runat="server" Text=""></asp:Label>'s Apps</h1>
        <!-- close general-filter -->
        <br />
        <br />


        <br />
        Check the Apps this user is authorized to work on.
        <div class="table-outline">


            <table>
                <tr>
                    <td>User Apps</td>
                    <td></td>
                    <td>Available Apps</td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="ListBox1" Height="150" Width="300" SelectionMode="Multiple" DataSourceID="dsIn" DataTextField="appname" DataValueField="appname" runat="server"></asp:ListBox>

                        <asp:SqlDataSource ID="dsIn" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                            SelectCommand="select app_settings.appname from app_settings  join UserApps	
                                on userApps.appname = app_settings.appname and username = @user   order by user_priority">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="user" Name="user" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <br />
                        <div>
                            <input type="button" class="main-cta" id="move-up" value="^" /></div>
                        <div>
                            <input type="button" class="main-cta" id="move-down" value="v" /></div>
                         <asp:Button ID="btnSave" Enabled="false" runat="server" CssClass="main-cta" Text="Save" />
                    </td>

                    <td align="center">
                        <asp:Button ID="btnRemove" runat="server" CssClass="main-cta" Text=">" /><br />
                        <asp:Button ID="btnRemoveAll" runat="server" CssClass="main-cta" Text=">>" /><br />
                        <asp:Button ID="btnAdd" runat="server" CssClass="main-cta" Text="<" /><br />
                        <asp:Button ID="btnAddAll" runat="server" CssClass="main-cta" Text="<<" /><br />


                    </td>
                    <td>
                        <asp:ListBox ID="ListBox2" Height="150" Width="300" SelectionMode="Multiple" DataSourceID="dsOut" DataTextField="appname" DataValueField="appname" runat="server"></asp:ListBox>

                        <asp:SqlDataSource ID="dsOut" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                            SelectCommand="select app_settings.appname from app_settings left join UserApps	
                                on userApps.appname = app_settings.appname and username = @user 
                             where username is null and active=1 order by app_settings.appname">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="user" Name="user" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                    </td>
                </tr>
            </table>

            <asp:CheckBoxList ID="CheckBoxList1" DataSourceID="dsUserApps" Visible="false" RepeatColumns="5" AutoPostBack="true" DataTextField="appname" DataValueField="checked" runat="server"></asp:CheckBoxList>

            <asp:SqlDataSource ID="dsUserApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" runat="server"
                SelectCommand="select app_settings.appname, case when username is null then 0 else 1 end as checked from app_settings  left join UserApps	
                                on userApps.appname = app_settings.appname and username = @user order by app_settings.appname">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="user" Name="user" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>

      
    </section>

</asp:Content>

