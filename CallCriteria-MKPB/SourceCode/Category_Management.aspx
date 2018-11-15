<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false"
    CodeFile="Category_Management.aspx.vb" Inherits="Category_Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="popup" id="add-category-popup">
        <div class="popup-container">
            <header class="popup-header">
                <h1>Add Category</h1>
                <a href="#" class="close-popup"><i class="fa fa-times"></i></a>
            
            <!-- close popup-header -->
            <div class="popup-body">
                <div class="formline">
                    <label>Name</label>
                    <asp:TextBox ID="txtNewCategoryName" runat="server" Style="width: 424px;"></asp:TextBox>
                </div>
                <!-- close formline -->
                <div class="two-halfs">
                    <div class="first-half">
                        <div class="formline">
                            <label>Order</label>
                            <asp:DropDownList ID="ddlNewOrder" runat="server" Style="width: 150px;">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <!-- close formline -->
                        <div class="formline">
                            <label>Priority</label>
                            <div class="priority-holder">
                                <div class="color red-color"></div>
                                <asp:DropDownList ID="ddlNewPriority" runat="server" Style="width: 150px;">
                                    <asp:ListItem data-color="red-color">Very High</asp:ListItem>
                                    <asp:ListItem data-color="dark-orange-color">High</asp:ListItem>
                                    <asp:ListItem data-color="orange-color">Medium</asp:ListItem>
                                    <asp:ListItem data-color="yellow-color">Normal</asp:ListItem>
                                    <asp:ListItem data-color="green-color">Low</asp:ListItem>
                                    <asp:ListItem data-color="neutral-color">None</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <!-- close priority-holder -->
                        </div>
                        <!-- close formline -->
                    </div>
                    <!-- close half -->


                    <div class="second-half">
                        <div class="formline">
                            <label>Autofail</label>
                            <div class="switch answer-no">
                                <div class="trigger"></div>
                            </div>
                            <!-- close switch -->
                        </div>
                        <!-- close formline -->

                        <div class="formline">
                            <label>Inverted <span>(Yes = Bad)</span></label>
                            <div class="switch answer-no">
                                <div class="trigger"></div>
                            </div>
                            <!-- close switch -->
                        </div>
                        <!-- close formline -->
                    </div>
                    <!-- close half -->


                </div>
                <!-- close two-halfs -->

            </div>
            <!-- close popup-body -->

            <div class="popup-footer">
                <div class="actions-in-right">
                    <a href="#" class="close-popup third-priority-buttom">Cancel</a>
                    <%--<button type="button" class="main-cta close-popup">Add Category</button>--%>
                    <asp:Button ID="Button1" CssClass="main-cta close-popup" runat="server" Text="Add Category" />
                </div>
                <!-- close actions-in-right -->
            </div>
            <!-- close popup-footer -->

        </div>
        <!-- close popup-container -->

    </div>
    <!-- close popup -->




    <section class="main-container">

        
            <h1 class="section-title"><i class="fa fa-pencil"></i>Category Management</h1>
            <a href="#" class="third-priority-buttom"><i class="fa fa-gear"></i>Settings</a>
        

        <div class="general-filter">
            <div class="yellow-container">
                <form action="" method="" class="">
                    <div class="field-holder search-holder">
                        <i class="fa fa-search"></i>
                        <input type="search" placeholder="Quick Search..." />
                    </div>
                    <!-- close search-holder -->

                    <div class="field-holder">
                        <i class="fa fa-list"></i>
                        <select>
                            <option>Priority</option>
                        </select>
                    </div>
                    <!-- close select-holder -->

                    <div class="field-holder">
                        <i class="fa fa-exclamation-circle"></i>
                        <select>
                            <option>Autofail</option>
                        </select>
                    </div>
                    <!-- close select-holder -->

                    <div class="field-holder">
                        <i class="fa fa-undo"></i>
                        <select>
                            <option>Inverted</option>
                        </select>
                    </div>
                    <!-- close select-holder -->


                    <button type="submit" class="secondary-cta">APPLY</button>
                </form>
            </div>
            <!-- close yellow-container -->

            <div class="applied-filters">
                <label>Aplied Filters:</label>

                <span>
                    <i class="fa fa-list"></i>
                    <em>Priority: <strong>N/A</strong></em>
                </span>


                <span>
                    <i class="fa fa-exclamation-circle"></i>
                    <em>Autofail: <strong>Yes</strong></em>
                </span>


                <span>
                    <i class="fa fa-undo"></i>
                    <em>Inverted: <strong>No</strong></em>
                </span>

            </div>
            <!-- close applied-filters -->
        </div>
        <!-- close general-filter -->






        <div class="panel-content move-content-up">

            <div class="main-cta-part">
                <div class="actions-in-right">
                    <button type="button" class="main-cta add-category-btn">Add Category</button>
                </div>
                <!-- close actions-in-right -->
            </div>
            <!-- close main-cta-part -->

            <div class="table-outline">
                <table>
                    <thead>
                        <tr>
                            <td style="width: 8%;"><span>Priority</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                            <td style="width: 56%;"><span>Name</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                            <td style="width: 6%;"><span>ID</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                            <td style="width: 10%;"><span>Autofail</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                            <td style="width: 10%;"><span>Inverted</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                            <td style="width: 10%;" class="text-align-center">Actions</td>
                        </tr>
                    </thead>


                    <tbody>
                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color yellow-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color yellow-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-neutral">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color red-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color orange-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color orange-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-neutral">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color green-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color dark-orange-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color yellow-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-neutral">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color green-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color green-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color red-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-neutral">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color yellow-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color green-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color red-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-yes">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-neutral">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>


                        <tr>
                            <td class="first-cell text-align-center">
                                <div class="color yellow-color"></div>
                            </td>
                            <td>Infraction</td>
                            <td class="text-align-right">3</td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="switch answer-no">
                                    <div class="trigger"></div>
                                </div>
                                <!-- close switch -->
                            </td>
                            <td>
                                <div class="row-actions">
                                    <a href="#" title="Edit"><i class="fa fa-pencil"></i></a>
                                    <a href="#" title="Remove"><i class="fa fa-times"></i></a>
                                </div>
                                <!-- close row-actions -->
                            </td>
                        </tr>
                    </tbody>

                    <tfoot>
                        <tr>
                            <td colspan="6">
                                <ul class="table-navigation">
                                    <li><a href="#">1</a></li>
                                    <li><a href="#">2</a></li>
                                    <li class="selected-page"><a href="#">3</a></li>
                                    <li><a href="#">4</a></li>
                                    <li><a href="#">5</a></li>
                                    <li><a href="#">6</a></li>
                                    <li><a href="#">7</a></li>
                                    <li><a href="#">8</a></li>
                                </ul>
                                <!-- close table-navigation -->


                                <div class="sort-options">
                                    <form action="" method="">
                                        <label>Show</label>
                                        <select>
                                            <option>10</option>
                                        </select>
                                        <label>Records per page</label>
                                        <button type="button" class="secondary-cta">SET</button>
                                    </form>
                                </div>
                                <!-- close sort-options -->
                            </td>
                        </tr>
                    </tfoot>

                </table>
            </div>
            <!-- close table-outline -->

            <div class="main-cta-part">
                <div class="actions-in-right">
                    <button type="button" class=" add-category-btn main-cta">Add Category</button>
                </div>
                <!-- close actions-in-right -->
            </div>
            <!-- close main-cta-part -->

        </div>
        <!-- close move-content-up -->

    </section>
    <!-- close main-container -->


    <div class="CatManagement-page">
        <div class="CatManagement-container">
            <div class="title">
                <span class="ico"></span>
                <h2>Category Management</h2>
                <div class="right">
                    <asp:Button ID="btnAddCategory" runat="server" Text="Add Category" CssClass="AddCatButton" />
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="CatManagement-data">
                <div style="overflow: auto;">
                    <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="id" DataSourceID="dsCategories2" ForeColor="#333333" GridLines="None"
                        Width="100%">
                        <%--<AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>
                        <Columns>
                            <asp:CommandField HeaderText="Edit" ShowEditButton="True" ButtonType="Image" EditImageUrl="~/images/edit-ico.png"
                                CancelImageUrl="~/images/delete1-ico.png" UpdateImageUrl="~/images/update.png" />
                            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True"
                                SortExpression="id" />
                            <asp:BoundField DataField="category" HeaderText="Category" SortExpression="category" />
                            <asp:BoundField DataField="category_order" HeaderText="Order" SortExpression="category_order" />
                            <asp:CheckBoxField DataField="autofail" HeaderText="Autofail" SortExpression="autofail" />
                            <asp:CheckBoxField DataField="inverted" HeaderText="Inverted (Yes is Bad)" SortExpression="inverted" />
                            <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ButtonType="Image"
                                DeleteImageUrl="~/images/delete1-ico.png" EditImageUrl="~/images/edit-ico.png"
                                CancelImageUrl="~/images/delete1-ico.png" UpdateImageUrl="~/images/update.png" />
                        </Columns>
                        <EditRowStyle CssClass="editRow" BackColor="#F8F3DB" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </div>
                <asp:SqlDataSource ID="dsCategories2" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                    DeleteCommand="DELETE FROM [Categories] WHERE [id] = @id" InsertCommand="INSERT INTO [Categories] ([category], [category_order], [inverted], [autofail]) VALUES (@category, @category_order, @inverted, @autofail)"
                    SelectCommand="SELECT * FROM [Categories]" UpdateCommand="UPDATE [Categories] SET [category] = @category, [category_order] = @category_order, [inverted] = @inverted, [autofail] = @autofail WHERE [id] = @id">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="category" Type="String" />
                        <asp:Parameter Name="category_order" Type="Int32" />
                        <asp:Parameter Name="inverted" Type="Boolean" />
                        <asp:Parameter Name="autofail" Type="Boolean" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="category" Type="String" />
                        <asp:Parameter Name="category_order" Type="Int32" />
                        <asp:Parameter Name="inverted" Type="Boolean" />
                        <asp:Parameter Name="autofail" Type="Boolean" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </div>
        </div>
    </div>
</asp:Content>
