<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="admin_toolbox.aspx.vb" Inherits="admin_toolbox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container general-button dash-modules ">
        <h2>Admin Toolbox</h2>

        <table class="">
            <thead>
                <tr style="font-weight: bold;">
                    <td>Billing/Payroll</td>
                    <td>Monitoring/Performance</td>
                    <td>Edit/System</td>
                    <td>Tools</td>
                    <td>Bad Call/Spot Check/NI</td>

                </tr>
            </thead>
            <tr>
                <td style="vertical-align:top">
                    <button type="button" title="Show Income by Scorecard over a date range" onclick="location.href='monthly_cpm.aspx'">Monthly CPM</button><br />
                    <button type="button" title="Receive/Create Invoices -- drive what clients see as pending." onclick="location.href='update_billing.aspx'">Update Billing</button><br />
                    <button type="button" title="Pay worksheet" onclick="location.href='PayWorksheet2.0.aspx'">Pay Worksheet</button>

                </td>
                <td style="vertical-align:top">

                    <button type="button" title="Download Multiple Calls by Phone Number." onclick="location.href='bulk_numbers.aspx'">Bulk Phone Download</button><br />
                    <button type="button" title="Who is online and working -- similar to right_now, but QA only." onclick="location.href='currently_working.aspx'">Currently Working</button><br />
                    <button type="button" title="Deactivated Users and reason for deactivation -- ability to reset/reinstate." onclick="location.href='DeactivatedUsers.aspx'">Deactivated Users</button><br />
                    <button type="button" title="Hours worked and calls processed by Week Ending" onclick="location.href='Labor_load.aspx'">Labor Load</button><br />
                    <button type="button" title="Last 50 edits to calls -- points out who is editing on our system" onclick="location.href='last_edits.aspx'">Last Edits</button><br />
                    <button type="button" title="Pending calibration editor -- filter and delete." onclick="location.href='pending_calibrations.aspx'">Pending Calibrations</button><br />
                    <button type="button" title="Users on the system right now -- not QA specific.  Includes clients." onclick="location.href='right_now.aspx'">Right Now</button><br />
                    <button type="button" title="Summary and details about how supervisors are processing notifications by date." onclick="location.href='Supervisor_Notification_Report.aspx'">Supervisor Notification Report</button><br />
                    <button type="button" title="Summary about how trainees are progressing." onclick="location.href='trainee_status.aspx'">Training Status</button><br />
                    <button type="button" title="Details about how trainees are progressing." onclick="location.href='training_last.aspx'">Training Progress</button><br />
                    <button type="button" title="See scorecards like the QA do." onclick="location.href='listen_scorecard.aspx'">Listen Scorecard</button><br />
                    <button type="button" title="Report where QAs open calls within other calls." onclick="location.href='multiple_call_report.aspx'">Multiple Call Report</button><br />
                    <button type="button" title="Bigger Report with all answers than call details produces." onclick="location.href='ExpandedView_Detail.aspx'">Expanded View/Report</button><br />
                    <button type="button" title="Total QA hours for new hirse within last 90 days." onclick="location.href='qa_total_hours.aspx'">QA Total Hours</button><br />
                    <button type="button" title="Reviewed by date. Totals by day for QA and Calibrators" onclick="location.href='reviewed_by_date.aspx'">Reviewed By Date</button><br />
                    <button type="button" title="Page Activity by User" onclick="location.href='user_page_views.aspx'">Page Activity by User</button><br />
                    <button type="button" title="Training Calls Available" onclick="location.href='training_calls.aspx'">Training Calls</button><br />
                    <button type="button" title="Flagged Transcripts" onclick="location.href='Flagged_Transcripts.aspx'">Flagged Transcripts</button><br />
                    <button type="button" title="Edit Notification Profiles" onclick="location.href='edit_notifications.aspx'">Edit Notification Profiles</button><br />

                </td>
                <td style="vertical-align:top">
                    <button type="button" title="Edit/create columns users can select for their dashboard" onclick="location.href='column_editor.aspx'">Column Editor</button><br />
                    <button type="button" title="Edit/create rules for users' modules" onclick="location.href='module_maintenance.aspx'">Module Maintenance</button><br />
                    <button type="button" title="Reload all available audio for edufficient calls and look for matches" onclick="location.href='aws_test.aspx'">Edufficient Audio Refresh</button><br />
                    <button type="button" title="General loader for calls given a spreadsheet or CSV." onclick="location.href='batch_upload.aspx'">Batch Upload</button><br />
                    <button type="button" title="Update/create call center contacts, owners." onclick="location.href='call_center_management.aspx'">Call Center Maintenance</button><br />
                    <button type="button" title="Update any call with missing call length." onclick="location.href='call_time_update2.aspx'">Call Time Update</button><br />

                    <button type="button" title="For all pending calls, check to see if the audio file is really there.  Create SQL statements to reset if not." onclick="location.href='check_missing_audio.aspx'">Check Missing Audio</button><br />
                    <button type="button" title="Filter/Select/Edit/Spot Check compelted calls." onclick="location.href='Completed_Calls.aspx'">Completed Calls</button><br />
                    Call ID: <input type="text" id="edit_cali" size="10" /> Recal: <input type="checkbox" id="isRecal" /><br />
                    <button type="button" title="Edit calibration -- ?ID=XXXXX&isrecal=0 or ?ID=XXXXXX&isrecal=1" onclick="location.href='edit_calibration.aspx?ID=" + $('#edit_cali').val() + "&isRecal=" + $('#isRecal').val() + "'>Edit Calibration</button><br />
                    
                    <button type="button" title="Edit client calibration -- ?ID=XXXXX" onclick="location.href='edit_client_calibration.aspx'">Edit Client Calibration</button><br />
                    
                    <button type="button" title="Edit call -- ?ID=XXXXX if you have the call ID or ?XID=XXXXXX is you have the pending ID" onclick="location.href='edit_metadata.aspx'">Edit Call Data</button><br />
                    <button type="button" title="View Scorecards like the QAs do.  Tests out questions and looks/feel." onclick="location.href='listen_scorecard.aspx'">Listen Scorecard</button><br />

                    <button type="button" title="Page to run ad-hoc SQL." onclick="location.href='update_page.aspx'">Update SQL Page</button><br />
                    <button type="button" title="Manage Users by Scorecard." onclick="location.href='scorecard_mange_page.aspx'">Manage Users by Scorecard</button><br />
                    <button type="button" title="Keyword Maintenance" onclick="location.href='keywords.aspx'">Keyword Maintenance</button><br />
                    <button type="button" title="Whole bunch of stuff" onclick="location.href='clear_notifications.aspx'">Reset Many Items</button><br />
                    <%--<button type="button" title="Reset/Delete Calls" onclick="location.href='delete_reset_calls.aspx'">Reset or Delete Calls</button><br />--%>
                    <button type="button" title="Link Management" onclick="location.href='link_management.aspx'">Link Management</button><br />
                    <button type="button" title="Rejection Reason Management" onclick="location.href='edit_rejections.aspx'">Rejection Reason Management</button><br />
                    



                </td>
                <td style="vertical-align:top">
                    <button type="button" title="Load a call manually -- with school data and/or audio." onclick="location.href='manual_call.aspx'">Manual Call</button><br />
                    <button type="button" title="Regrade a call with new audio." onclick="location.href='regrade_call.aspx'">Regrade Call</button><br />
                    <button type="button" title="Copy client calibrations from one user to another." onclick="location.href='add_client_calibration.aspx'">Copy/Add Client Calibrations</button><br />
                    <button type="button" title="Delete Pending Calls." onclick="location.href='pending_calls2.aspx'">Filter and Delete Pending Calls</button><br />
                    <button type="button" title="Reset user's password or change user's username" onclick="location.href='change_password.aspx'">Change Password/Change Username</button><br />
                    <button type="button" title="Manage the sending of report emails to clients." onclick="location.href='MailerAdministration.aspx'">Mailer Administration</button><br />
                    <button type="button" title="Update all pending call lengths and return avg length" onclick="location.href='update_call_length.aspx'">Update Call Length</button><br />
                    <button type="button" title="Change answers for a scorecard over a date range." onclick="location.href='massUpdate.aspx'">Mass Update</button><br />
                    <button type="button" title="Manage how Notifications/Disputes are managed." onclick="location.href='edit_notifications.aspx'">Notification Flow</button><br />
                    <button type="button" title="Recertify QA" onclick="location.href='Recertify_QA.aspx'">Recertify QA</button><br />

                </td>
                <td style="vertical-align:top">
                    <button type="button" title="Spot Check Report" onclick="location.href='spot_check_report.aspx'"> Spot Check Report</button><br />
                    <button type="button" title="Open Spot Check items -- new items appear daily and ad hoc" onclick="location.href='spot_check.aspx'">Open Spot Check Items</button><br />
                    <button type="button" title="Rules engine for spot checkscreated daily." onclick="location.href='spotcheck_automation.aspx'">Spot Check Automation</button><br />
                    <button type="button" title="Page to filter and select spot check items." onclick="location.href='completed_calls.aspx'">Spot Check Filters</button><br />
                    <button type="button" title="Update and disposition bad calls." onclick="location.href='bad_call_report.aspx'">Bad Call Report</button><br />
                    <button type="button" title="Report accepted bad calls." onclick="location.href='bad_call_accepted.aspx'">Bad Call Accepted Report</button><br />
                    <button type="button" title="Not Interested Report." onclick="location.href='ni_data.aspx'">NI Report</button><br />
                    <button type="button" title="CS Training Report." onclick="location.href='cs_certification.aspx'">CS Training Report</button><br />
                </td>

            </tr>
        </table>


        <style>
            .general-button [type="button"] {
                background-color: #ffffff;
                color: #149bdf;
                display: inline-block;
                padding: 5px 5px;
                border: solid 1px #149bdf;
                border-radius: 3px;
                font-size: 12px;
                line-height: 12px;
                text-transform: uppercase;
                letter-spacing: 0.15em;
                margin: 3px 3px;
                cursor: pointer;
            }
        </style>


    </section>
</asp:Content>

