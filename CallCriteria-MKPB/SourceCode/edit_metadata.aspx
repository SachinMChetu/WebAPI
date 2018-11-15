<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="edit_metadata.aspx.vb" Inherits="manual_call" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button" />


    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <asp:HiddenField ID="hdnUserName" runat="server" />
    <asp:HiddenField ID="hdnXCCID" runat="server" />

    <asp:FormView ID="fvMeta" DataSourceID="dsMeta" DefaultMode="Edit" DataKeyNames="ID" runat="server">

        <InsertItemTemplate>
            INSERT MODE
            <table>

                <tr>
                    <td><strong>Standard Data</strong></td>
                </tr>
                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td>Appname</td>
                    <td>

                        <asp:Label ID="ddlAppname" runat="server" Text='<%#Bind("appname")%>'></asp:Label>

                    </td>

                    <td>Scorecard</td>
                    <td>
                        <%#Eval("short_name")%>

                    </td>

                    <td>Call Date</td>
                    <td>
                        <asp:TextBox ID="txtCall_Date" CssClass="hasDatePicker" runat="server" Text='<%#Bind("call_date") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Date Required" Text="*" ForeColor="Red" ControlToValidate="txtCall_Date"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>SESSION_ID</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSESSION_ID" Text='<%#Bind("SESSION_ID") %>' />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Session Required" Text="*" ForeColor="Red" ControlToValidate="txtSESSION_ID"></asp:RequiredFieldValidator>
                    </td>

                    <td>AGENT</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAGENT" Text='<%#Bind("AGENT") %>' /></td>

                    <td>AGENT_NAME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAGENT_NAME" Text='<%#Bind("AGENT_NAME") %>' /></td>

                    <td>DISPOSITION</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDISPOSITION" Text='<%#Bind("DISPOSITION") %>' /></td>
                </tr>
                <tr>
                    <td>CAMPAIGN</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCAMPAIGN" Text='<%#Bind("CAMPAIGN") %>' /></td>

                    <td>ANI</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtANI" Text='<%#Bind("ANI") %>' /></td>

                    <td>DNIS</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDNIS" Text='<%#Bind("DNIS") %>' /></td>

                    <td>TIMESTAMP</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTIMESTAMP" Text='<%#Bind("TIMESTAMP") %>' /></td>
                </tr>
                <tr>
                    <td>TALK_TIME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTALK_TIME" Text='<%#Bind("TALK_TIME") %>' /></td>

                    <td>CALL_TIME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCALL_TIME" Text='<%#Bind("CALL_TIME") %>' /></td>

                    <td>HANDLE_TIME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtHANDLE_TIME" Text='<%#Bind("HANDLE_TIME") %>' /></td>

                    <td>CALL_TYPE</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCALL_TYPE" Text='<%#Bind("CALL_TYPE") %>' /></td>
                </tr>
                <tr>
                    <td>LIST_NAME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtLIST_NAME" Text='<%#Bind("LIST_NAME") %>' /></td>

                    <td>leadid</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtleadid" Text='<%#Bind("leadid") %>' /></td>

                    <td>AGENT_GROUP</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAGENT_GROUP" Text='<%#Bind("AGENT_GROUP") %>' /></td>

                    <td>HOLD_TIME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtHOLD_TIME" Text='<%#Bind("HOLD_TIME") %>' /></td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEmail" Text='<%#Bind("Email") %>' /></td>

                    <td>City</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCity" Text='<%#Bind("City") %>' /></td>

                    <td>State</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtState" Text='<%#Bind("State") %>' /></td>

                    <td>Zip</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtZip" Text='<%#Bind("Zip") %>' /></td>
                </tr>
                <tr>
                    <td>Datacapturekey</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDatacapturekey" Text='<%#Bind("Datacapturekey") %>' /></td>

                    <td>Datacapture</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDatacapture" Text='<%#Bind("Datacapture") %>' /></td>

                    <td>Status</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtStatus" Text='<%#Bind("Status") %>' /></td>

                    <td>Program</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtProgram" Text='<%#Bind("Program") %>' /></td>
                </tr>
                <tr>
                    <td>Datacapture_Status</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDatacapture_Status" Text='<%#Bind("Datacapture_Status") %>' /></td>

                    <td>num_of_schools</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtnum_of_schools" Text='<%#Bind("num_of_schools") %>' /></td>

                    <td>EducationLevel</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEducationLevel" Text='<%#Bind("EducationLevel") %>' /></td>

                    <td>HighSchoolGradYear</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtHighSchoolGradYear" Text='<%#Bind("HighSchoolGradYear") %>' /></td>
                </tr>
                <tr>
                    <td>DegreeStartTimeframe</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDegreeStartTimeframe" Text='<%#Bind("DegreeStartTimeframe") %>' /></td>



                    <td>First_Name</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFirst_Name" Text='<%#Bind("First_Name") %>' /></td>

                    <td>Last_Name</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtLast_Name" Text='<%#Bind("Last_Name") %>' /></td>
                    <td>Must Review</td>
                    <td>
                        <asp:CheckBox runat="server" ID="TextBox1" Checked='<%#Bind("must_review") %>' /></td>

                </tr>
                <tr>
                    <td>address</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtaddress" Text='<%#Bind("address") %>' /></td>

                    <td>phone</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtphone" Text='<%#Bind("phone") %>' /></td>

                    <td>profile_id</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtprofile_id" Text='<%#Bind("profile_id") %>' /></td>
                    <td>sort_order</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtsort_order" Text='<%#Bind("sort_order") %>' /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><strong>Schools</strong></td>
                </tr>


                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>School</td>
                    <td>
                        <asp:TextBox runat="server" ID="schSchool" /></td>

                    <td>College</td>
                    <td>
                        <asp:TextBox runat="server" ID="schCollege" /></td>

                    <td>DegreeOfInterest</td>
                    <td>
                        <asp:TextBox runat="server" ID="schDegreeOfInterest" /></td>
                    <td>Modality</td>
                    <td>
                        <asp:TextBox runat="server" ID="schModality" /></td>
                </tr>
                <tr>
                    <td>AOI1</td>
                    <td>
                        <asp:TextBox runat="server" ID="schAOI1" /></td>

                    <td>AOI2</td>
                    <td>
                        <asp:TextBox runat="server" ID="schAOI2" /></td>

                    <td>L1_SubjectName</td>
                    <td>
                        <asp:TextBox runat="server" ID="schL1_SubjectName" /></td>

                    <td>L2_SubjectName</td>
                    <td>
                        <asp:TextBox runat="server" ID="schL2_SubjectName" /></td>

                </tr>

                <tr>
                    <td>Portal/Origin</td>
                    <td>
                        <asp:TextBox runat="server" ID="schOrigin" /></td>

                </tr>

                <tr>
                    <td>
                        <asp:Button ID="btnAddSchool" runat="server" Text="Add School" OnClick="btnAddSchool_Click" /></td>
                </tr>

                <tr>
                    <td colspan="8">
                        <asp:GridView ID="gvSchools" CssClass="detailsTable" AutoGenerateColumns="false" runat="server">
                            <Columns>
                                <asp:BoundField HeaderText="School" DataField="School" />
                                <asp:BoundField HeaderText="College" DataField="College" />
                                <asp:BoundField HeaderText="DegreeOfInterest" DataField="DegreeOfInterest" />
                                <asp:BoundField HeaderText="Modality" DataField="Modality" />
                                <asp:BoundField HeaderText="AOI1" DataField="AOI1" />
                                <asp:BoundField HeaderText="AOI2" DataField="AOI2" />
                                <asp:BoundField HeaderText="L1_SubjectName" DataField="L1_SubjectName" />
                                <asp:BoundField HeaderText="L2_SubjectName" DataField="L2_SubjectName" />
                                <asp:BoundField HeaderText="Portal/Origin" DataField="Origin" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>


                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><strong>Audio</strong></td>
                </tr>





                <tr>
                    <td>Audio File</td>
                    <td>

                        <asp:TextBox runat="server" ID="audfile_name" />
                        or
                <asp:FileUpload ID="audFUP" runat="server" /></td>




                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAddAudio" runat="server" Text="Add Audio" OnClick="btnAddAudio_Click" /></td>
                </tr>

                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td colspan="8">
                        <asp:GridView ID="gvAudio" CssClass="detailsTable" OnRowDeleting="gvAudio_RowDeleting" AutoGenerateColumns="false" runat="server">
                            <Columns>
                                <asp:BoundField HeaderText="File Name" DataField="file_name" />
                                <asp:BoundField HeaderText="File Date" DataField="file_date" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>



                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><strong>Other Data</strong></td>
                </tr>


                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>




                <tr>
                    <td>Key/Header</td>
                    <td>

                        <asp:TextBox runat="server" ID="othdata_key" /></td>

                    <td>Data/Value</td>
                    <td>
                        <asp:TextBox runat="server" ID="othdata_value" /></td>

                    <td>Data Type</td>
                    <td>
                        <asp:TextBox runat="server" ID="othdata_type" Text="String" /></td>


                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAddOther" runat="server" Text="Add Other Data" OnClick="btnAddOther_Click" /></td>
                </tr>


                <tr>
                    <td colspan="8">
                        <asp:GridView ID="gvOther" CssClass="detailsTable" AutoGenerateColumns="false" runat="server">
                            <Columns>
                                <asp:BoundField HeaderText="Key" DataField="key" />
                                <asp:BoundField HeaderText="Value" DataField="Value" />
                                <asp:BoundField HeaderText="Type" DataField="Type" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>

            </table>

            Enter all the data, schools, audio and other values first, then submit the final record or call.<br />
            <br />

            <asp:Button ID="btnSubmit" CommandName="Insert" runat="server" Text="Submit Call/Record" />

        </InsertItemTemplate>


        <EditItemTemplate>

            EDIT MODE
            <table>

                <tr>
                    <td><strong>Standard Data</strong></td>
                </tr>
                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td>Appname</td>
                    <td>
                        <asp:Label runat="server" Text='<%#Eval("appname")%>' ID="ddlAppname"></asp:Label>
                    </td>

                    <td>Scorecard</td>
                    <td>
                        <%#Eval("short_name")%>

                    </td>

                    <td>Call Date</td>
                    <td>
                        <asp:TextBox ID="txtCall_Date" CssClass="hasDatePicker" runat="server" Text='<%#Bind("call_date") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Date Required" Text="*" ForeColor="Red" ControlToValidate="txtCall_Date"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>SESSION_ID</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSESSION_ID" Text='<%#bind("SESSION_ID") %>' />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Session Required" Text="*" ForeColor="Red" ControlToValidate="txtSESSION_ID"></asp:RequiredFieldValidator>
                    </td>

                    <td>AGENT</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAGENT" Text='<%#Bind("AGENT") %>' /></td>

                    <td>AGENT_NAME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAGENT_NAME" Text='<%#Bind("AGENT_NAME") %>' /></td>

                    <td>DISPOSITION</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDISPOSITION" Text='<%#Bind("DISPOSITION") %>' /></td>
                </tr>
                <tr>
                    <td>CAMPAIGN</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCAMPAIGN" Text='<%#Bind("CAMPAIGN") %>' /></td>

                    <td>ANI</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtANI" Text='<%#Bind("ANI") %>' /></td>

                    <td>DNIS</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDNIS" Text='<%#Bind("DNIS") %>' /></td>

                    <td>TIMESTAMP</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTIMESTAMP" Text='<%#Bind("TIMESTAMP") %>' /></td>
                </tr>
                <tr>
                    <td>TALK_TIME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTALK_TIME" Text='<%#Bind("TALK_TIME") %>' /></td>

                    <td>CALL_TIME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCALL_TIME" Text='<%#Bind("CALL_TIME") %>' /></td>

                    <td>HANDLE_TIME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtHANDLE_TIME" Text='<%#Bind("HANDLE_TIME") %>' /></td>

                    <td>CALL_TYPE</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCALL_TYPE" Text='<%#Bind("CALL_TYPE") %>' /></td>
                </tr>
                <tr>
                    <td>LIST_NAME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtLIST_NAME" Text='<%#Bind("LIST_NAME") %>' /></td>

                    <td>leadid</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtleadid" Text='<%#Bind("leadid") %>' /></td>

                    <td>AGENT_GROUP</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAGENT_GROUP" Text='<%#Bind("AGENT_GROUP") %>' /></td>

                    <td>HOLD_TIME</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtHOLD_TIME" Text='<%#Bind("HOLD_TIME") %>' /></td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEmail" Text='<%#Bind("Email") %>' /></td>

                    <td>City</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCity" Text='<%#Bind("City") %>' /></td>

                    <td>State</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtState" Text='<%#Bind("State") %>' /></td>

                    <td>Zip</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtZip" Text='<%#Bind("Zip") %>' /></td>
                </tr>
                <tr>
                    <td>Datacapturekey</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDatacapturekey" Text='<%#Bind("Datacapturekey") %>' /></td>

                    <td>Datacapture</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDatacapture" Text='<%#Bind("Datacapture") %>' /></td>

                    <td>Status</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtStatus" Text='<%#Bind("Status") %>' /></td>

                    <td>Program</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtProgram" Text='<%#Bind("Program") %>' /></td>
                </tr>
                <tr>
                    <td>Datacapture_Status</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDatacapture_Status" Text='<%#Bind("Datacapture_Status") %>' /></td>

                    <td>num_of_schools</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtnum_of_schools" Text='<%#Bind("num_of_schools") %>' /></td>

                    <td>EducationLevel</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEducationLevel" Text='<%#Bind("EducationLevel") %>' /></td>

                    <td>HighSchoolGradYear</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtHighSchoolGradYear" Text='<%#Bind("HighSchoolGradYear") %>' /></td>
                </tr>
                <tr>
                    <td>DegreeStartTimeframe</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDegreeStartTimeframe" Text='<%#Bind("DegreeStartTimeframe") %>' /></td>


                    <td>First_Name</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFirst_Name" Text='<%#Bind("First_Name") %>' /></td>

                    <td>Last_Name</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtLast_Name" Text='<%#Bind("Last_Name") %>' /></td>
                    <td>Must Review</td>
                    <td>
                        <asp:CheckBox runat="server" ID="TextBox1" Checked='<%#Bind("must_review") %>' /></td>
                </tr>
                <tr>
                    <td>address</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtaddress" Text='<%#Bind("address") %>' /></td>

                    <td>phone</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtphone" Text='<%#Bind("phone") %>' /></td>

                    <td>profile_id</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtprofile_id" Text='<%#Bind("profile_id") %>' /></td>
                    <td>sort_order</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtsort_order" Text='<%#Bind("sort_order") %>' /></td>
                </tr>

                <tr>
                    <td>website</td>
                    <td colspan="5">
                        <asp:TextBox runat="server" ID="txtwebsite" Text='<%#Bind("website") %>' /></td>

                </tr>


                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><strong>Schools</strong></td>
                </tr>


                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>School</td>
                    <td>
                        <asp:TextBox runat="server" ID="schSchool" /></td>

                    <td>College</td>
                    <td>
                        <asp:TextBox runat="server" ID="schCollege" /></td>

                    <td>DegreeOfInterest</td>
                    <td>
                        <asp:TextBox runat="server" ID="schDegreeOfInterest" /></td>
                    <td>Modality</td>
                    <td>
                        <asp:TextBox runat="server" ID="schModality" /></td>
                </tr>
                <tr>
                    <td>AOI1</td>
                    <td>
                        <asp:TextBox runat="server" ID="schAOI1" /></td>

                    <td>AOI2</td>
                    <td>
                        <asp:TextBox runat="server" ID="schAOI2" /></td>

                    <td>L1_SubjectName</td>
                    <td>
                        <asp:TextBox runat="server" ID="schL1_SubjectName" /></td>

                    <td>L2_SubjectName</td>
                    <td>
                        <asp:TextBox runat="server" ID="schL2_SubjectName" /></td>

                </tr>
                <tr>
                    <td>Portal/Origin</td>
                    <td>
                        <asp:TextBox runat="server" ID="schOrigin" /></td>
                </tr>

                <tr>
                    <td>
                        <asp:Button ID="btnAddSchool" runat="server" Text="Add School" OnClick="btnAddSchool_Click" /></td>
                </tr>

                <tr>
                    <td colspan="8">
                        <asp:GridView ID="gvSchools" CssClass="detailsTable" DataKeyNames="ID" AutoGenerateColumns="False" runat="server" DataSourceID="dsSchools">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                                <asp:BoundField HeaderText="School" DataField="School" />
                                <asp:BoundField HeaderText="College" DataField="College" />
                                <asp:BoundField HeaderText="DegreeOfInterest" DataField="DegreeOfInterest" />
                                <asp:BoundField HeaderText="Modality" DataField="Modality" />
                                <asp:BoundField HeaderText="AOI1" DataField="AOI1" />
                                <asp:BoundField HeaderText="AOI2" DataField="AOI2" />
                                <asp:BoundField HeaderText="L1_SubjectName" DataField="L1_SubjectName" />
                                <asp:BoundField HeaderText="L2_SubjectName" DataField="L2_SubjectName" />
                                <asp:BoundField HeaderText="Portal/Origin" DataField="origin" />

                            </Columns>
                        </asp:GridView>

                        <asp:SqlDataSource ID="dsSchools" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                            DeleteCommand="DELETE FROM [School_X_Data] WHERE [id] = @id"
                            InsertCommand="INSERT INTO [School_X_Data] ([School], [AOI1], [AOI2], [L1_SubjectName], [L2_SubjectName], [Modality], [xcc_id], [College], [DegreeOfInterest], [origin]) 
                            VALUES (@School, @AOI1, @AOI2, @L1_SubjectName, @L2_SubjectName, @Modality, @xcc_id, @College, @DegreeOfInterest, @origin)"
                            SelectCommand="SELECT * FROM [School_X_Data] where xcc_id = @pending_id"
                            UpdateCommand="UPDATE [School_X_Data] SET  [School] = @School, [AOI1] = @AOI1, [AOI2] = @AOI2, [L1_SubjectName] = @L1_SubjectName, [L2_SubjectName] = @L2_SubjectName, [Modality] = @Modality,[College] = @College, [DegreeOfInterest] = @DegreeOfInterest, origin=@origin WHERE [id] = @id">
                            <DeleteParameters>
                                <asp:Parameter Name="id" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <%--<asp:QueryStringParameter Name="pending_id" QueryStringField="ID" Type="Int32" />--%>
                                <asp:ControlParameter ControlID="hdnxccid" name="pending_id" />

                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="School" Type="String" />
                                <asp:Parameter Name="AOI1" Type="String" />
                                <asp:Parameter Name="AOI2" Type="String" />
                                <asp:Parameter Name="L1_SubjectName" Type="String" />
                                <asp:Parameter Name="L2_SubjectName" Type="String" />
                                <asp:Parameter Name="Modality" Type="String" />
                                <asp:ControlParameter ControlID="hdnxccid" name="xcc_id" />
                                <asp:Parameter Name="College" Type="String" />
                                <asp:Parameter Name="DegreeOfInterest" Type="String" />
                                <asp:Parameter Name="origin" Type="String" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="School" Type="String" />
                                <asp:Parameter Name="AOI1" Type="String" />
                                <asp:Parameter Name="AOI2" Type="String" />
                                <asp:Parameter Name="L1_SubjectName" Type="String" />
                                <asp:Parameter Name="L2_SubjectName" Type="String" />
                                <asp:Parameter Name="Modality" Type="String" />
                                <asp:Parameter Name="xcc_id" Type="Int32" />
                                <asp:Parameter Name="College" Type="String" />
                                <asp:Parameter Name="DegreeOfInterest" Type="String" />
                                <asp:Parameter Name="origin" Type="String" />
                                <asp:Parameter Name="id" Type="Int32" />
                            </UpdateParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>


                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><strong>Audio</strong></td>
                </tr>





                <tr>
                    <td>Audio File</td>
                    <td colspan="8">
                        <asp:HiddenField ID="hdnExistingFile" Value='<%#Eval("audio_link")%>' runat="server" />
                        <asp:TextBox runat="server" ID="audfile_name" />
                        or
                <asp:FileUpload ID="audFUP" runat="server" />
                        File Order:
                        <asp:TextBox ID="txtOrder" runat="server"></asp:TextBox>
                        <asp:Button ID="btnAddAudio" runat="server" Text="Add Audio" OnClick="btnAddAudio_Click" /></td>

                    <td>
                        <asp:DropDownList ID="ddlAddExisting" runat="server" Visible="false">
                            <asp:ListItem Text="(Select)" Value=""></asp:ListItem>
                            <asp:ListItem Text="Don't Merge" Value=""></asp:ListItem>
                            <asp:ListItem>Add to Front</asp:ListItem>
                            <asp:ListItem>Add to Rear</asp:ListItem>
                            <asp:ListItem>Replace Audio</asp:ListItem>
                        </asp:DropDownList></td>


                </tr>
                <tr>
                    <td colspan="8">
                        <asp:GridView ID="gvAudio" CssClass="detailsTable" AutoGenerateColumns="False" OnRowDeleting="gvAudio_RowDeleting" runat="server">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                                <asp:BoundField HeaderText="File Name" HtmlEncode="false" DataField="file_name" />
                                <asp:BoundField HeaderText="File Date" DataField="file_date" />
                                <asp:BoundField HeaderText="File Order" DataField="file_order" />
                            </Columns>
                        </asp:GridView>
                        <%--  <asp:SqlDataSource ID="dsAudios" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                            SelectCommand="SELECT * FROM [AudioData] WHERE ([final_xcc_id] = @ID)">

                            <SelectParameters>
                                <asp:QueryStringParameter Name="id" QueryStringField="ID" Type="Int32" />
                            </SelectParameters>

                        </asp:SqlDataSource>--%>
                        The initial file is the existing audio.  Enter the link or upload a file, add an Order and hit Add Audio.  When all files are added and arranged, click Merge/Create Audio.  For files before the existing file use smaller numbers.
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Button ID="btnMerge" runat="server" Text="Merge/Create Audio" OnClick="btnMerge_Click" />
                    </td>
                </tr>

                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>





                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2"><strong>Other Data</strong></td>
                </tr>


                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>




                <tr>
                    <td>Key/Header</td>
                    <td>

                        <asp:TextBox runat="server" ID="othdata_key" /></td>

                    <td>Data/Value</td>
                    <td>
                        <asp:TextBox runat="server" ID="othdata_value" /></td>

                    <td>Data Type</td>
                    <td>
                        <asp:TextBox runat="server" ID="othdata_type" Text="String" /></td>


                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAddOther" runat="server" Text="Add Other Data" OnClick="btnAddOther_Click" /></td>
                </tr>


                <tr>
                    <td colspan="8">
                        <asp:GridView ID="gvOther" CssClass="detailsTable" AutoGenerateColumns="False" runat="server">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                                <asp:BoundField HeaderText="Key" DataField="data_key" />
                                <asp:BoundField HeaderText="Value" DataField="data_value" />
                                <asp:BoundField HeaderText="Type" DataField="data_type" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="dsOther" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
                            DeleteCommand="DELETE FROM [otherFormData] WHERE [id] = @id"
                            InsertCommand="INSERT INTO [otherFormData] ([form_id], [data_key], [data_value], [data_type], [xcc_id]) VALUES (@form_id, @data_key, @data_value, @data_type, @xcc_id)" 
                            SelectCommand="SELECT * FROM [otherFormData] WHERE ([xcc_id] = @xcc_id)" 
                            UpdateCommand="UPDATE [otherFormData] SET [data_key] = @data_key, [data_value] = @data_value, [data_type] = @data_type WHERE [id] = @id">
                            <DeleteParameters>
                                <asp:Parameter Name="id" Type="Int32" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="form_id" Type="Int32" />
                                <asp:Parameter Name="data_key" Type="String" />
                                <asp:Parameter Name="data_value" Type="String" />
                                <asp:Parameter Name="data_type" Type="String" />
                                <asp:Parameter Name="xcc_id" Type="Int32" />
                            </InsertParameters>
                            <SelectParameters>
                                <%--<asp:QueryStringParameter Name="xcc_id" QueryStringField="ID" Type="Int32" />--%>
                                <asp:ControlParameter ControlID="hdnXCCId" Name="xcc_id" />
                            </SelectParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="form_id" Type="Int32" />
                                <asp:Parameter Name="data_key" Type="String" />
                                <asp:Parameter Name="data_value" Type="String" />
                                <asp:Parameter Name="data_type" Type="String" />
                                <asp:Parameter Name="xcc_id" Type="Int32" />
                                <asp:Parameter Name="id" Type="Int32" />
                            </UpdateParameters>

                        </asp:SqlDataSource>

                         <asp:GridView ID="gvCurrent" CssClass="detailsTable" DataKeyNames="ID" AutoGenerateColumns="False" runat="server" DataSourceID="dsOther">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                                <asp:BoundField HeaderText="Key" DataField="data_key" />
                                <asp:BoundField HeaderText="Value" DataField="data_value" />
                                <asp:BoundField HeaderText="Type" DataField="data_type" />
                            </Columns>
                        </asp:GridView>
                       
                    </td>
                </tr>

            </table>

            Enter all the data, schools, audio and other values first, then submit the final record or call.<br />
            <br />

            <asp:Button ID="btnSubmit" CommandName="Update" OnClick="btnSubmit_Click" runat="server" Text="Update Call/Record" />
            <asp:Button ID="btnSubReset" CommandName="Update" OnClick="btnSubReset_Click" runat="server" Text="Update and Reset Call/Record" />
            <br />
            * Update will update data on a call whether it has been processed or not. Update and reset will set the call to be processed again if it had already been scored.

        </EditItemTemplate>
    </asp:FormView>
    <asp:SqlDataSource ID="dsMeta" runat="server" ConnectionString="<%$ ConnectionStrings:estomesManual %>"
        DeleteCommand="DELETE FROM [XCC_REPORT_NEW] WHERE [ID] = @ID"
        InsertCommand="INSERT INTO [XCC_REPORT_NEW] ([SESSION_ID], [AGENT], [DISPOSITION], [CAMPAIGN], [ANI], [DNIS], [TIMESTAMP], [TALK_TIME], [CALL_TIME], [HANDLE_TIME], [CALL_TYPE], [LIST_NAME], 
        [leadid], [AGENT_GROUP], [HOLD_TIME], [DATE], [Email], [City], [State], [Zip], [Datacapturekey], [Datacapture], [Status], [Program], [Datacapture_Status], [num_of_schools], [MAX_REVIEWS], 
        [review_started], [Number_of_Schools], [EducationLevel], [HighSchoolGradYear], [DegreeStartTimeframe], [appname], [First_Name], [Last_Name], [address], [phone], [call_date],
         [audio_link], [profile_id], [audio_user], [audio_password], [bad_call], [bad_call_who], [bad_call_date], [bad_call_delete_date], [bad_call_reason], [to_upload], [pending_id], [date_added],
         [AreaOfInterest], [ProgramsOfInterestType], [Citizenship], [DegreeOfInterest], [Gender], [Military], [secondphone], [agent_name], [bad_call_accepted], [bad_call_accepted_who], [scorecard], 
        [Notes], [uploaded], [text_only], [file_missing], [media_id], [fileUrl], [statusMessage], [mediaId], [requestStatus], [fileStatus], [response], [website], [schools_loaded], [compliance_sheet]) 
        VALUES (@SESSION_ID, @AGENT, @DISPOSITION, @CAMPAIGN, @ANI, @DNIS, @TIMESTAMP, @TALK_TIME, @CALL_TIME, @HANDLE_TIME, @CALL_TYPE, @LIST_NAME, @leadid, @AGENT_GROUP, @HOLD_TIME, @DATE, @Email, @City, 
        @State, @Zip, @Datacapturekey, @Datacapture, @Status, @Program, @Datacapture_Status, @num_of_schools, @MAX_REVIEWS, @review_started, @Number_of_Schools, @EducationLevel, @HighSchoolGradYear, 
        @DegreeStartTimeframe, @appname, @First_Name, @Last_Name, @address, @phone, @call_date, @audio_link, @profile_id, @audio_user, @audio_password, @bad_call, @bad_call_who, @bad_call_date, 
        @bad_call_delete_date, @bad_call_reason, @to_upload, @pending_id, @date_added, @AreaOfInterest, @ProgramsOfInterestType, @Citizenship, @DegreeOfInterest, @Gender, @Military, @secondphone,
         @agent_name, @bad_call_accepted, @bad_call_accepted_who, @scorecard, @Notes, @uploaded, @text_only, @file_missing, @media_id, @fileUrl, @statusMessage, @mediaId, @requestStatus, @fileStatus,
         @response, @website, @schools_loaded, @compliance_sheet)"
        SelectCommand="SELECT * FROM [XCC_REPORT_NEW] join scorecards on scorecards.id = scorecard 
        where xcc_report_new.id = @XID or xcc_report_new.id = (Select x_id from vwForm where f_id = @ID)
        or xcc_report_new.session_id =@SID"
        UpdateCommand="UPDATE [XCC_REPORT_NEW] SET [SESSION_ID] = @SESSION_ID, [AGENT] = @AGENT, [DISPOSITION] = @DISPOSITION, [CAMPAIGN] = @CAMPAIGN, [ANI] = @ANI, [DNIS] = @DNIS, [TIMESTAMP] = @TIMESTAMP, 
        [TALK_TIME] = @TALK_TIME, [CALL_TIME] = @CALL_TIME, [HANDLE_TIME] = @HANDLE_TIME, [CALL_TYPE] = @CALL_TYPE, [LIST_NAME] = @LIST_NAME, [leadid] = @leadid, [AGENT_GROUP] = @AGENT_GROUP, 
        [HOLD_TIME] = @HOLD_TIME, [Email] = @Email, [City] = @City, [State] = @State, [Zip] = @Zip, [Datacapturekey] = @Datacapturekey, [Datacapture] = @Datacapture, 
        [Status] = @Status, [Program] = @Program, [Datacapture_Status] = @Datacapture_Status, [num_of_schools] = @num_of_schools, call_date=@call_date,
        [EducationLevel] = @EducationLevel, [HighSchoolGradYear] = @HighSchoolGradYear, [DegreeStartTimeframe] = @DegreeStartTimeframe,
        [First_Name] = @First_Name, [Last_Name] = @Last_Name,must_review=@must_review, [address] = @address, [phone] = @phone, [profile_id] = @profile_id, agent_name=@agent_name,
        sort_order=@sort_order, [website] = @website WHERE [ID] = @ID">
        <DeleteParameters>
            <asp:Parameter Name="ID" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="ID" QueryStringField="ID" DefaultValue="0" />
            <asp:QueryStringParameter Name="XID" QueryStringField="XID" DefaultValue="0" />
            <asp:QueryStringParameter Name="SID" QueryStringField="SID" DefaultValue="0" />
        </SelectParameters>

        <InsertParameters>
            <asp:Parameter Name="SESSION_ID" Type="String" />
            <asp:Parameter Name="AGENT" Type="String" />
            <asp:Parameter Name="DISPOSITION" Type="String" />
            <asp:Parameter Name="CAMPAIGN" Type="String" />
            <asp:Parameter Name="ANI" Type="String" />
            <asp:Parameter Name="DNIS" Type="String" />
            <asp:Parameter Name="TIMESTAMP" Type="String" />
            <asp:Parameter Name="TALK_TIME" Type="String" />
            <asp:Parameter Name="CALL_TIME" Type="String" />
            <asp:Parameter Name="HANDLE_TIME" Type="String" />
            <asp:Parameter Name="CALL_TYPE" Type="String" />
            <asp:Parameter Name="LIST_NAME" Type="String" />
            <asp:Parameter Name="leadid" Type="String" />
            <asp:Parameter Name="AGENT_GROUP" Type="String" />
            <asp:Parameter Name="HOLD_TIME" Type="String" />
            <asp:Parameter Name="DATE" Type="DateTime" />
            <asp:Parameter Name="Email" Type="String" />
            <asp:Parameter Name="City" Type="String" />
            <asp:Parameter Name="State" Type="String" />
            <asp:Parameter Name="Zip" Type="String" />
            <asp:Parameter Name="Datacapturekey" Type="Double" />
            <asp:Parameter Name="Datacapture" Type="Double" />
            <asp:Parameter Name="Status" Type="String" />
            <asp:Parameter Name="Program" Type="String" />
            <asp:Parameter Name="Datacapture_Status" Type="String" />
            <asp:Parameter Name="num_of_schools" Type="String" />
            <asp:Parameter Name="MAX_REVIEWS" Type="Int32" />
            <asp:Parameter Name="review_started" Type="DateTime" />
            <asp:Parameter Name="Number_of_Schools" Type="String" />
            <asp:Parameter Name="EducationLevel" Type="String" />
            <asp:Parameter Name="HighSchoolGradYear" Type="String" />
            <asp:Parameter Name="DegreeStartTimeframe" Type="String" />
            <asp:Parameter Name="appname" Type="String" />
            <asp:Parameter Name="First_Name" Type="String" />
            <asp:Parameter Name="Last_Name" Type="String" />
            <asp:Parameter Name="address" Type="String" />
            <asp:Parameter Name="phone" Type="String" />
            <asp:Parameter Name="call_date" Type="DateTime" />
            <asp:Parameter Name="audio_link" Type="String" />
            <asp:Parameter Name="profile_id" Type="String" />
            <asp:Parameter Name="audio_user" Type="String" />
            <asp:Parameter Name="audio_password" Type="String" />
            <asp:Parameter Name="bad_call" Type="Int32" />
            <asp:Parameter Name="bad_call_who" Type="String" />
            <asp:Parameter Name="bad_call_date" Type="DateTime" />
            <asp:Parameter Name="bad_call_delete_date" Type="DateTime" />
            <asp:Parameter Name="bad_call_reason" Type="String" />
            <asp:Parameter Name="to_upload" Type="Boolean" />
            <asp:Parameter Name="pending_id" Type="Int32" />
            <asp:Parameter Name="date_added" Type="DateTime" />
            <asp:Parameter Name="AreaOfInterest" Type="String" />
            <asp:Parameter Name="ProgramsOfInterestType" Type="String" />
            <asp:Parameter Name="Citizenship" Type="String" />
            <asp:Parameter Name="DegreeOfInterest" Type="String" />
            <asp:Parameter Name="Gender" Type="String" />
            <asp:Parameter Name="Military" Type="String" />
            <asp:Parameter Name="secondphone" Type="String" />
            <asp:Parameter Name="agent_name" Type="String" />
            <asp:Parameter Name="bad_call_accepted" Type="DateTime" />
            <asp:Parameter Name="bad_call_accepted_who" Type="String" />
            <asp:Parameter Name="scorecard" Type="Int32" />
            <asp:Parameter Name="Notes" Type="String" />
            <asp:Parameter Name="uploaded" Type="DateTime" />
            <asp:Parameter Name="text_only" Type="String" />
            <asp:Parameter Name="file_missing" Type="Boolean" />
            <asp:Parameter Name="media_id" Type="String" />
            <asp:Parameter Name="fileUrl" Type="String" />
            <asp:Parameter Name="statusMessage" Type="String" />
            <asp:Parameter Name="mediaId" Type="String" />
            <asp:Parameter Name="requestStatus" Type="String" />
            <asp:Parameter Name="fileStatus" Type="String" />
            <asp:Parameter Name="response" Type="String" />
            <asp:Parameter Name="website" Type="String" />
            <asp:Parameter Name="schools_loaded" Type="Int32" />
            <asp:Parameter Name="compliance_sheet" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="SESSION_ID" Type="String" />
            <asp:Parameter Name="AGENT" Type="String" />
            <asp:Parameter Name="DISPOSITION" Type="String" />
            <asp:Parameter Name="CAMPAIGN" Type="String" />
            <asp:Parameter Name="ANI" Type="String" />
            <asp:Parameter Name="DNIS" Type="String" />
            <asp:Parameter Name="TIMESTAMP" Type="String" />
            <asp:Parameter Name="TALK_TIME" Type="String" />
            <asp:Parameter Name="CALL_TIME" Type="String" />
            <asp:Parameter Name="HANDLE_TIME" Type="String" />
            <asp:Parameter Name="CALL_TYPE" Type="String" />
            <asp:Parameter Name="LIST_NAME" Type="String" />
            <asp:Parameter Name="leadid" Type="String" />
            <asp:Parameter Name="AGENT_GROUP" Type="String" />
            <asp:Parameter Name="HOLD_TIME" Type="String" />
            <asp:Parameter Name="DATE" Type="DateTime" />
            <asp:Parameter Name="Email" Type="String" />
            <asp:Parameter Name="City" Type="String" />
            <asp:Parameter Name="State" Type="String" />
            <asp:Parameter Name="Zip" Type="String" />
            <asp:Parameter Name="Datacapturekey" Type="Double" />
            <asp:Parameter Name="Datacapture" Type="Double" />
            <asp:Parameter Name="Status" Type="String" />
            <asp:Parameter Name="Program" Type="String" />
            <asp:Parameter Name="Datacapture_Status" Type="String" />
            <asp:Parameter Name="num_of_schools" Type="String" />
            <asp:Parameter Name="Number_of_Schools" Type="String" />
            <asp:Parameter Name="EducationLevel" Type="String" />
            <asp:Parameter Name="HighSchoolGradYear" Type="String" />
            <asp:Parameter Name="DegreeStartTimeframe" Type="String" />
            <asp:Parameter Name="appname" Type="String" />
            <asp:Parameter Name="First_Name" Type="String" />
            <asp:Parameter Name="Last_Name" Type="String" />
            <asp:Parameter Name="address" Type="String" />
            <asp:Parameter Name="phone" Type="String" />
            <asp:Parameter Name="call_date" Type="DateTime" />
            <asp:Parameter Name="audio_link" Type="String" />
            <asp:Parameter Name="profile_id" Type="String" />
            <asp:Parameter Name="audio_user" Type="String" />
            <asp:Parameter Name="audio_password" Type="String" />
            <asp:Parameter Name="AreaOfInterest" Type="String" />
            <asp:Parameter Name="ProgramsOfInterestType" Type="String" />
            <asp:Parameter Name="Citizenship" Type="String" />
            <asp:Parameter Name="DegreeOfInterest" Type="String" />
            <asp:Parameter Name="Gender" Type="String" />
            <asp:Parameter Name="Military" Type="String" />
            <asp:Parameter Name="secondphone" Type="String" />
            <asp:Parameter Name="agent_name" Type="String" />
            <asp:Parameter Name="scorecard" Type="Int32" />
            <asp:Parameter Name="Notes" Type="String" />
            <asp:Parameter Name="uploaded" Type="DateTime" />
            <asp:Parameter Name="text_only" Type="String" />
            <asp:Parameter Name="file_missing" Type="Boolean" />
            <asp:Parameter Name="response" Type="String" />
            <asp:Parameter Name="website" Type="String" />
            <asp:Parameter Name="schools_loaded" Type="Int32" />
            <asp:Parameter Name="compliance_sheet" Type="String" />
            <asp:QueryStringParameter Name="id" QueryStringField="ID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>



</asp:Content>
