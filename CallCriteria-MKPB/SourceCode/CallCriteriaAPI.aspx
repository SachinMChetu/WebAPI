<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="CallCriteriaAPI.aspx.vb" Inherits="CallCriteriaAPI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        p.MsoNormal {
            margin-top: 0in;
            margin-right: 0in;
            margin-bottom: 8.0pt;
            margin-left: 0in;
            line-height: 107%;
            font-size: 11.0pt;
            font-family: "Calibri",sans-serif;
        }

        p {
            margin-right: 0in;
            margin-left: 0in;
            font-size: 12.0pt;
            font-family: "Times New Roman",serif;
        }

        a:link {
            color: blue;
            text-decoration: underline;
            text-underline: single;
        }

        p.heading1 {
            margin-right: 0in;
            margin-left: 0in;
            font-size: 12.0pt;
            font-family: "Times New Roman",serif;
        }

        p.intro {
            margin-right: 0in;
            margin-left: 0in;
            font-size: 12.0pt;
            font-family: "Times New Roman",serif;
        }

        h2 {
            margin-right: 0in;
            margin-left: 0in;
            font-size: 18.0pt;
            font-family: "Times New Roman",serif;
            font-weight: bold;
        }

        h3 {
            margin-right: 0in;
            margin-left: 0in;
            font-size: 13.5pt;
            font-family: "Times New Roman",serif;
            font-weight: bold;
        }

        pre {
            margin-bottom: .0001pt;
            tab-stops: 45.8pt 91.6pt 137.4pt 183.2pt 229.0pt 274.8pt 320.6pt 366.4pt 412.2pt 458.0pt 503.8pt 549.6pt 595.4pt 641.2pt 687.0pt 732.8pt;
            font-size: 10.0pt;
            font-family: "Courier New";
            margin-left: 0in;
            margin-right: 0in;
            margin-top: 0in;
        }

        .auto-style1 {
            border-collapse: collapse;
            font-size: 11.0pt;
            font-family: Calibri, sans-serif;
            border: 1.0pt solid windowtext;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="main-container dash-modules general-button">

        <p align="center" style="text-align: center">
            <b><span style="font-size: 20.0pt; line-height: 107%">Call Criteria </span></b>
        </p>
        <p align="center" style="text-align: center">
            <b>Web Service Interface Document</b>
        </p>
        <p align="center" style="text-align: center">
            <b>&nbsp;</b>
        </p>
        <p align="center" style="text-align: center">
            <b>6/25/2015</b>
        </p>
        <p align="center" style="text-align: center">
            <b>Rev 2 12/10/2015<br />
                Rev 3 11/30/2016<br />
                Rev 4 07/10/2017
            </b>
            <p>
                <b>&nbsp;</b>
            </p>
            <p>
                <b>Introduction:</b>
            </p>
            <p>
                The web service for Call Criteria is used to send records individually to our system without the need to upload data and/or audio files and/or school or referential data.
            </p>
            <p>
                &nbsp;
            </p>
            <p>
                There are multiple services available.&nbsp; These can be found at the URL: <a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help">http://app.callcriteria.com/callcriteriaAPI.svc/json/help</a>
            </p>
            <p>
                <table class="detailsTable">
                    <tr>
                        <th>Uri</th>
                        <th>Method</th>
                        <th>Description</th>
                    </tr>
                    <tr>
                        <td>AddExistingSchool</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/AddExistingSchool"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/AddExistingSchool" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Add more data to an existing record after it has been received and processed.</td>
                    </tr>
                    <tr>
                        <td>AddRecord</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/AddRecord"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/AddRecord" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Use this to send us all data necessary to receive calls and meta-data associated with the call.</td>
                    </tr>
                    <tr>
                        <td>CreateScorecard</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/CreateScorecard"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/CreateScorecard" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>If necessary, use this to create a scorecard with your internal system.</td>
                    </tr>
                    <tr>
                        <td>GetAllRecords</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/GetAllRecords"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/GetAllRecords" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Returns all records associated with a specific scorecard</td>
                    </tr>
                    <tr>
                        <td>GetCallsLoaded</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/GetCallsLoaded"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/GetCallsLoaded" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Service at http://app.callcriteria.com/callcriteriaAPI.svc/json/GetCallsLoaded</td>
                    </tr>
                    <tr>
                        <td>GetRecord</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/GetRecord"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/GetRecord" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Service at http://app.callcriteria.com/callcriteriaAPI.svc/json/GetRecord</td>
                    </tr>
                    <tr>
                        <td>GetRecordID</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/GetRecordID"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/GetRecordID" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Service at http://app.callcriteria.com/callcriteriaAPI.svc/json/GetRecordID</td>
                    </tr>
                    <tr>
                        <td>getScore</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/getScore"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/getScore" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Service at http://app.callcriteria.com/callcriteriaAPI.svc/json/getScore</td>
                    </tr>
                    <tr>
                        <td>GetScorecardRecord</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/GetScorecardRecord"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/GetScorecardRecord" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Service at http://app.callcriteria.com/callcriteriaAPI.svc/json/GetScorecardRecord</td>
                    </tr>
                    <tr>
                        <td>GetScorecardRecordID</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/GetScorecardRecordID"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/GetScorecardRecordID" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Service at http://app.callcriteria.com/callcriteriaAPI.svc/json/GetScorecardRecordID</td>
                    </tr>
                    <tr>
                        <td>GetScorecardRecordJ</td>
                        <td title="http://app.callcriteria.com/callcriteriaAPI.svc/json/GetScorecardRecordJ"><a href="http://app.callcriteria.com/callcriteriaAPI.svc/json/help/operations/GetScorecardRecordJ" rel="operation" style="color: rgb(102, 153, 204); font-weight: bold; text-decoration: underline;">POST</a></td>
                        <td>Service at http://app.callcriteria.com/callcriteriaAPI.svc/json/GetScorecardRecordJ</td>
                    </tr>
                </table>
                <span>&nbsp; </span>
            </p>
            <p>
                Clicking on the POST method above will allow you to see how each items can be called and how the returned data can be interpreted.
            </p>
            <p>
                &nbsp;
            </p>
            <p>
                <b><span style="font-size: 14.0pt; line-height: 107%">Posting Data</span></b>
            </p>
            <p>
                Once we have created an account for you, you will get a scorecards ID, an appname (name of your application within Call Criteria) and an api key.&nbsp; To POST data to us, you would add the appname and apikey to the URL in this format:
            </p>
            <p>
                <a>http://app.callcriteria.com/callcriteriaAPI.svc/json/AddRecord?appname=&lt;appname&gt;&amp;apikey=&lt;apikey&gt;</a>
            </p>
            <p>
                or
            </p>
            <p>
                <a>http://app.callcriteria.com/callcriteriaAPI.svc/xml/AddRecord?appname=&lt;appname&gt;&amp;apikey=&lt;apikey&gt;</a>
            </p>
            <p>
                Post for data in either json format or XML format.&nbsp; Simple change the posting URL to use json or xml as above.
            </p>
            <p>
                Here&#39;s a sample CURL call you can use:<br />
                <br />
            </p>
            <p>
                <span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">curl -X POST -H &quot;Content-Type: application/json&quot; -d &#39;{&quot;api_key&quot;:&quot;<strong>&lt;apikey&gt;</strong>&quot;,&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">AGENT&quot;:&quot;your 
            agent name&quot;,&quot;AGENT_GROUP&quot;:&quot;VICI01&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">,&quot;AGENT_NAME&quot;:&quot;37465&quot;,&quot;CALL_</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">TIME&quot;:&quot;2017-05-25 
            14:18:02&quot;,&quot;CAMPAIGN&quot;:&quot;TSPURCA&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">,&quot;DISPOSITION&quot;:&quot;UPSCMB&quot;,&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">SESSION_ID&quot;:&quot;1495736258.24854&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">,&quot;TIMESTAMP&quot;:&quot;2017-05-21 
            15:09:20&quot;,&quot;appname&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;"><strong>&lt;appname&gt;</strong>&quot;,&quot;phone&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">4168091355&quot;,&quot;Email&quot;:&quot;</span><a href="mailto:angela@powerplusmobility.com" style="color: rgb(17, 85, 204); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);" target="_blank">angela@<wbr />powerplusmobility.com</a><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">&quot;,&quot;First_</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">Name&quot;:&quot;Angela&quot;,&quot;Last_Name&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">Mohan&quot;,&quot;City&quot;:&quot;Brampton&quot;,&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">State&quot;:&quot;ON&quot;,&quot;Zip&quot;:&quot;L6Y 
            2Z4&quot;,&quot;address&quot;:&quot;1 Villanova road&quot;,&quot;audios&quot;:[{&quot;audio_file&quot;:</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">&quot;20170525-141739_</span><a href="tel:(416)%20809-1355" style="color: rgb(17, 85, 204); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);" target="_blank" value="+14168091355">4168091355</a><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">_</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">37465-all.mp3&quot;,&quot;file_date&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">2017-05-25&quot;,&quot;order&quot;:&quot;1&quot;},{&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">audio_file&quot;:&quot;</span><a href="ftp://34.196.173.237/contacticsftproot/recordings/Call" style="color: rgb(17, 85, 204); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);" target="_blank">ftp://yourcompany.com/<wbr />recordings/</a><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">6_21_17/your_call_audio.</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">mp3&quot;,&quot;file_date&quot;:&quot;2017-05-25&quot;,</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">&quot;order&quot;:&quot;2&quot;}],&quot;call_date&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">2017-05-25&quot;,&quot;leadid&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">62701143&quot;,&quot;scorecard&quot;:&quot;<strong>&lt;scorecard&gt;</strong>&quot;,&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">OtherDataItems&quot;:[{&quot;key&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">order_id_1&quot;,&quot;label&quot;:&quot;order_id_</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">1&quot;,&quot;value&quot;:&quot;458340&quot;},{&quot;key&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">campaign_id_1&quot;,&quot;label&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">campaign_id_1&quot;,&quot;value&quot;:&quot;37&quot;},{</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">&quot;key&quot;:&quot;products_name_1_1&quot;,&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">label&quot;:&quot;products_name_1_1&quot;,&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">value&quot;:&quot;CC 
            ThinSecret Pure Garcinia Cambogia Trial (CA)&quot;},{&quot;key&quot;:&quot;products_price_</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">1_1&quot;,&quot;label&quot;:&quot;products_price_</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">1_1&quot;,&quot;value&quot;:&quot;2.00&quot;},{&quot;key&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">products_name_1_2&quot;,&quot;label&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">products_name_1_2&quot;,&quot;value&quot;:&quot;CC 
            Advanced Green Coffee 1 Bottle&quot;},{&quot;key&quot;:&quot;products_</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">price_1_2&quot;,&quot;label&quot;:&quot;products_</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">price_1_2&quot;,&quot;value&quot;:&quot;24.95&quot;},{&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">key&quot;:&quot;products_price_1_3&quot;,&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">label&quot;:&quot;products_price_1_3&quot;,&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">value&quot;:&quot;49.95&quot;},{&quot;key&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">products_name_1_3&quot;,&quot;label&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">products_name_1_3&quot;,&quot;value&quot;:&quot;CC 
            MaxEffect Cleanse Kit&quot;},{&quot;key&quot;:&quot;response_code_1&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">,&quot;label&quot;:&quot;response_code_1&quot;,&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">value&quot;:&quot;100&quot;},{&quot;key&quot;:&quot;Customer 
            Shipping Address&quot;,&quot;label&quot;:&quot;Customer Shipping Address&quot;,&quot;value&quot;:&quot;Angela Mohan, 1 Villanova road , Brampton, ON L6Y 2Z4&quot;},{&quot;key&quot;:&quot;Total Amount Billed&quot;,&quot;label&quot;:&quot;Total Amount Billed&quot;,&quot;value&quot;:&quot;76.90&quot;},{&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">key&quot;:&quot;script_ID&quot;,&quot;label&quot;:&quot;</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">script_ID&quot;,&quot;value&quot;:&quot;TSPURCA_</span><wbr style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial;" /><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">script.inc&quot;}]}&#39; 
            -s<span>&nbsp;</span></span><a data-saferedirecturl="https://www.google.com/url?hl=en&amp;q=http://app.callcriteria.com/CallCriteriaAPI.svc/json/AddRecord?appname%3Dcontactics%255C%26apikey%3D5897d06c-0641-40e3-a872-9a2fa14cfb54&amp;source=gmail&amp;ust=1499793473459000&amp;usg=AFQjCNFZ3fNdOpJ7fS99e53ZR-aLIVAEYw" href="http://app.callcriteria.com/CallCriteriaAPI.svc/json/AddRecord?appname=contactics%5C&amp;apikey=5897d06c-0641-40e3-a872-9a2fa14cfb54" style="color: rgb(17, 85, 204); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);" target="_blank">http://app.callcriteria.com/<wbr />CallCriteriaAPI.svc/json/<wbr />AddRecord?appname=<span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;"><strong>&lt;appname&gt;</strong></span>\&amp;<wbr />apikey=</a><span style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;"><strong>&lt;apikey&gt;</strong></span>
            </p>

            <h2>My Data</h2>
            Use the following to populate the data fields <b>&lt;appname&gt;</b>, <b>&lt;apikey&gt;</b> and <b>&lt;scorecard&gt;</b> <br />The Scorecard Description is there to show which scorecard the ID belongs to.
            <asp:GridView ID="gvApps" CssClass="detailsTable" runat="server" DataSourceID="dsMyApps" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="appname" HeaderText="Appname" />
                    <asp:BoundField DataField="api_key" HeaderText="ApiKey" />
                    <asp:BoundField DataField="user_scorecard" HeaderText="Scorecard" />
                    <asp:BoundField DataField="short_name" HeaderText="Scorecard Description" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsMyApps" ConnectionString="<%$ ConnectionStrings:estomes2ConnectionString %>" 
                SelectCommand="select distinct userapps.appname, api_key, short_name, user_scorecard from userapps join api_keys on api_keys.appname = userapps.appname join scorecards on scorecards.id = user_scorecard where username=@username and scorecards.active = 1 and api_keys.active = 1" runat="server">
                <SelectParameters>
                    <asp:Parameter Name="username" />
                </SelectParameters>
            </asp:SqlDataSource>



            <p>
                &nbsp;
            </p>
            <p>
                <b><span style="font-size: 14.0pt; line-height: 107%">AddRecord</span></b>
            </p>
            <p>
                <b>Usage:</b>
            </p>
            <p>
                The following fields are available, but not required, to post data to.<span>&nbsp; </span>If the data you are posting does not have the same fields, but you would like to use an existing field – feel free to map to that field.<span>&nbsp; </span>If it is necessary to add fields, please contact us at <a href="mailto:support@callcriteria.com">support@callcriteria.com</a> with any concerns or comments.
            </p>
            <p>
                &nbsp;
            </p>
            <p>
                Online docs are available with prototypes and samples of XML and JSON posts at <a href="https://app.callcriteria.com/callcriteriaAPI.svc/json/help">https://app.callcriteria.com/callcriteriaAPI.svc/json/help</a>

            </p>
            <p>
                &nbsp;
            </p>
            <p>
                <b><u>Inputs</u></b>
            </p>
            <p>
                <b>AddRecord Fields</b>
            </p>
            <table class="detailsTable">
                <tr style="mso-yfti-irow: 0; mso-yfti-firstrow: yes">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Field Name</b>
                        </p>
                    </td>
                    <td style="width: 63.0pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Type</b>
                        </p>
                    </td>
                    <td style="width: 45.0pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Size</b>
                        </p>
                    </td>
                    <td style="width: .75in; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Required</b>
                        </p>
                    </td>
                    <td style="width: 224.75pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Comments/Notes</b>
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 1">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            SESSION_ID
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Yes
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            The audio files will get renamed to the records’s ID.mp3 
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 2">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            AGENT
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Your agent’s name or ID. Used to sort and filter on the dashboard.
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 3">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            AGENT_NAME
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Can be the agent’s full name instead of their ID.<span>&nbsp; </span>Not used to sort or filter.
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 4">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            DISPOSITION
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Result of the call.<span>&nbsp; </span>
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 5">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            CAMPAIGN
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Is used as a filter on the dashboard. 
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 6">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            ANI
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            50
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            We primarily use phone – left here as a legacy data element
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 7">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            DNIS
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            50
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            We primarily use phone – left here as a legacy data element
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 8">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            TIMESTAMP
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Full date and time the call was made – display only.
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 9">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            TALK_TIME
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Talk time of the call – stored as 1/1/1899 + call length -- but is a string, so anything works!
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 10">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            CALL_TIME
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Date/Time
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Time of the call – stored as 12/30/1899 + call length - 1899-12-30 00:01:03.000 for example.
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 11">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            HANDLE_TIME
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                           String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Handle time of the call – stored as 1/1/1899 + call length -- but is a string, so anything works!
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 12">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            CALL_TYPE
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Type of Call
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 13">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            LIST_NAME
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            List from which the call was originated
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 14">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            leadid
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Float
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            ID number from source system.<span>&nbsp; </span>
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 15">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            AGENT_GROUP
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Group/Supervisor owning the agent.<span>&nbsp; </span>Can be filtered on dashboard.
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 16">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            HOLD_TIME
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Date/Time
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Hold time of call
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 17">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Email
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Email of lead
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 18">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            City
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            City of Lead
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 19">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            State
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            State of Lead
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 20">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Zip
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            20
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Zip/postal code of lead
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 21">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Datacapturekey
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Float
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Another lead ID
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 22">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Datacapture
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Float
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Another lead ID
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 23">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Status
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead status
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 24">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Program
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Program lead was interested in 
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 25">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Datacapture_status
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead ID status
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 26">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            num_of_schools
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Not used – moved to school object
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 27">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            EducationLevel
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s level of education
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 28">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            HighSchoolGradYear
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s GED/HS Grad year
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 29">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            DegreeStartTimeframe
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            255
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s desired start time
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 30">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            appname
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            50
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Yes
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Appname is provided to you when you signed up with Call Criteria.<span>&nbsp; </span>Every post must have an Appname associated with it.
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 31">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            First_Name
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            150
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s first name
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 32">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Last_Name
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            150
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s last name
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 33">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            address
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            200
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s street address
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 34">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            phone
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            50
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s phone
                        </p>
                    </td>
                </tr>

                <tr style="mso-yfti-irow: 36">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            sort_order
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            10
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Insert lower numbers to have these reviewed earlier
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 37">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            scorecard
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Int
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Yes
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Use the integer provided to assign this record directly to a specific scorecard
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 38">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            call_date
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Date
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Date the call/record was created
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 39">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Citizenship
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s citizenship
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 40">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Military
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s Military 
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 41">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            profile_id
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s internal profile ID
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 42">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Schools
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Many to one object
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Add as many schools objects as needed
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 43">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            audios
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Many to one object
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Yes
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Add as many audio files as needed
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 44; mso-yfti-lastrow: yes">
                    <td style="width: 80.75pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="108">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            OtherDataItems
                        </p>
                    </td>
                    <td style="width: 63.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="84">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Many to one object
                        </p>
                    </td>
                    <td style="width: 45.0pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="60">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: .75in; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 224.75pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="300">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Add as many “other” data items – see below
                        </p>
                    </td>
                </tr>
            </table>
            <p>
                <b>&nbsp;</b>
            </p>
            <p>
                <b>&nbsp;</b>
            </p>
            <p>
                <b>Schools (0 to n SchoolItem objects)</b>
            </p>
            <p>
                <b>SchoolItem Object</b>
            </p>
            <table class="detailsTable">
                <tr style="mso-yfti-irow: 0; mso-yfti-firstrow: yes">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Field Name</b>
                        </p>
                    </td>
                    <td style="width: 61.85pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Type</b>
                        </p>
                    </td>
                    <td style="width: 44.25pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Size</b>
                        </p>
                    </td>
                    <td style="width: 53.95pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Required</b>
                        </p>
                    </td>
                    <td style="width: 218.8pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Comments/Notes</b>
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 1">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            School
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            School name
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 2">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            College
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            200
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            College name
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 3">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            DegreeOfInterest
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s degree interest
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 4">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            AOI1
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            200
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s area of interest 1
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 5">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            AOI2
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s area of interest 2
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 6">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            L1_SubjectName
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s subject interest 1
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 7">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            L2_SubjectName
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s subject interest 2
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 8">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Modality
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s modality – online/campus/both
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 9">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Portal
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s portal source
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 10; mso-yfti-lastrow: yes">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            TCPA
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            8000
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Lead’s TCPA 
                        </p>
                    </td>
                </tr>
            </table>
            <p>
                <b>&nbsp;</b>
            </p>
            <p>
                <b>Audios (0 to n AudioFile objects)</b>
            </p>
            <p>
                <b>AudioFile Object</b>
            </p>
            <table class="detailsTable">
                <tr style="mso-yfti-irow: 0; mso-yfti-firstrow: yes">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Field Name</b>
                        </p>
                    </td>
                    <td style="width: 61.85pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Type</b>
                        </p>
                    </td>
                    <td style="width: 44.25pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Size</b>
                        </p>
                    </td>
                    <td style="width: 53.95pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Required</b>
                        </p>
                    </td>
                    <td style="width: 218.8pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Comments/Notes</b>
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 1">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            audio_file
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            500
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Yes
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Name of the audio file – http/ftp/sftp protocols are all supported.<span>&nbsp; </span>Please contact me for other protocols
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 2">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            order
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Int
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            &nbsp;
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            If you have the files in order, populate this 
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 3; mso-yfti-lastrow: yes">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            file_date
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            If you have the files in chronological order, populate this
                        </p>
                    </td>
                </tr>
            </table>
            <p>
                <b>&nbsp;</b>
            </p>
            <p>
                <b>&nbsp;</b>
            </p>
            <p>
                <b>OtherDataItems (0 to n OtherData objects)</b>
            </p>
            <p>
                <b>OtherData Object</b>
            </p>
            <table class="detailsTable">
                <tr style="mso-yfti-irow: 0; mso-yfti-firstrow: yes; height: 19.75pt">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt; height: 19.75pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Field Name</b>
                        </p>
                    </td>
                    <td style="width: 61.85pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt; height: 19.75pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Type</b>
                        </p>
                    </td>
                    <td style="width: 44.25pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt; height: 19.75pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Size</b>
                        </p>
                    </td>
                    <td style="width: 53.95pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt; height: 19.75pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Required</b>
                        </p>
                    </td>
                    <td style="width: 218.8pt; border: solid windowtext 1.0pt; border-left: none; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt; height: 19.75pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            <b>Comments/Notes</b>
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 1">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            key
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Bolded value presented to our QA for data key name “Mortgage” for instance
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 2">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            value
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            1000
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Data value itself “$350k” for instance
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 3">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            type
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Default to “string”<span>&nbsp; </span>-- more values in the future.
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 4">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            school
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            100
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Post if this data element is directly related to a specific school.
                        </p>
                    </td>
                </tr>
                <tr style="mso-yfti-irow: 5; mso-yfti-lastrow: yes">
                    <td style="width: 88.65pt; border: solid windowtext 1.0pt; border-top: none; mso-border-top-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="118">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            label
                        </p>
                    </td>
                    <td style="width: 61.85pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="82">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            String
                        </p>
                    </td>
                    <td style="width: 44.25pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="59">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            2000
                        </p>
                    </td>
                    <td style="width: 53.95pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="72">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            No
                        </p>
                    </td>
                    <td style="width: 218.8pt; border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; mso-border-top-alt: solid windowtext .5pt; mso-border-left-alt: solid windowtext .5pt; mso-border-alt: solid windowtext .5pt; padding: 0in 5.4pt 0in 5.4pt"
                        valign="top" width="292">
                        <p style="margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal">
                            Description if needed for the key
                        </p>
                    </td>
                </tr>
            </table>
            <p>
                <b>&nbsp;</b>
            </p>
            <p>
                <b>
                    <br />
                    <br style="mso-special-character: line-break" />
                    <![if !supportLineBreakNewLine]>
            <br style="mso-special-character: line-break" />
                    <![endif]></b>
            </p>
            <p>
                <b>Revision History:</b>
            </p>
            <p>
                <b>Rev 2 – 12/10/2015 – Added https:// protocol for posting.</b>
            </p>
            <p>
                <b>Rev 3 – 11/30/2016 – Corrected case for the posting values and added new values for schools object</b>
            </p>
            <p>
                <strong>Rev 4 - 7/10/2017 - Converted to REST instead of SOAP calls, added online version, added CURL call</strong>
            </p>
    </section>
</asp:Content>

