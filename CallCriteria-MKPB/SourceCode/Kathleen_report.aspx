<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Kathleen_report.aspx.vb" Inherits="Kathleen_report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Start Date:
            <asp:Calendar ID="textbox1" runat="server"></asp:Calendar>
            <br />
            End Date:
            <asp:Calendar ID="textbox2" runat="server"></asp:Calendar>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Go" />
            <asp:GridView ID="GridView1" AllowSorting="true" AutoGenerateColumns="false"  DataSourceID="dsCalls" runat="server">
                <Columns>
                    <asp:BoundField DataField="total_questions" SortExpression="total_questions" HeaderText="Total Questions" />
                    <asp:BoundField DataField="total_right" SortExpression="total_right" HeaderText="# Right" />
                    <asp:BoundField DataField="total_wrong" SortExpression="total_wrong" HeaderText="# Wrong" />
                    <asp:BoundField DataField="q_short_name" SortExpression="q_short_name" HeaderText="Question" />
                    <asp:BoundField DataField="agent" SortExpression="agent" HeaderText="Agent" />
                </Columns>

            </asp:GridView>
            <asp:SqlDataSource ID="dsCalls" runat="server" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>"
                SelectCommand="select count(*) as total_questions, sum(case when right_answer =1 then 1 else 0 end) as total_right, 
            sum(case when right_answer = 0 then 1 else 0 end) as total_wrong, q_short_name, agent, section_order, q_order
            from vwForm join form_q_scores on form_q_scores.forM_id = vwForm.f_id
            join questions on questions.ID = form_q_scores.question_id 
            join sections on sections.ID = questions.section
            join question_answers on question_answers.id = form_q_scores.question_answered
            where vwForm.appname = 'edsoup' and convert(date,call_date) between @start and @end
            group by  q_short_name, agent, section_order, q_order order by agent, section_order, q_order">
                <SelectParameters>
                    <asp:ControlParameter ControlID="textbox1" PropertyName="SelectedDate" Name="start" />
                    <asp:ControlParameter ControlID="textbox2" PropertyName="SelectedDate" Name="end" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
