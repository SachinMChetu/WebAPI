<%@ Page Language="VB" AutoEventWireup="false" CodeFile="jdefault.aspx.vb" Inherits="jdefault" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body {
            background-color: #eee;
            margin: 0;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 16px;
        }
        h3 {
            font-style: italic;
        }
        .main {
            margin: 0 auto;
            width: 1024px;
        }
        .header {
            width: 1024px;
            height: 823px;
            background-image: url(images/callcriteria.png)
        }
        .content {
            background-color: #fff;
            padding: 30px;
        }
        .computation {
            width: 100%;
        }
        .w45 {
            width: 45%;
        }
        .w10 {
            width: 10%;
        }
        .computation2 {
            width: 100%;
        }
        .computation2 td:first-child {
            width: 70%;
        }
        .right {
            text-align: right;
        }
        .italic {
            font-style: italic;
        }
        input[type=number] {
            width: 36px;
        }
        p {
            text-align: justify;
        }
        .fs18 {
            font-size: 20px;
        }
        .hover {

        }
        .blue_circle {
            width: 370px;
            height: 297px;
            background-image: url(images/blue_circle.gif);
            background-size: 100%;
        }
        .green_circle {
            width: 370px;
            height: 297px;
            background-image: url(images/green_circle.gif);
            background-size: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <div class="header">

        </div>

        <div class="content">
            <h3>About PointQA:</h3>
            <p>PointQA provides contact center quality and performance management services.</p>
            <p>We evaluate and analyze agent-customer interactions to unlock key behavioral and procedural drivers of sales, retention, and service.</p>
            <p>Our team is made up of independent work at home individuals who work together to provide our clients with top grade Quality Assurance service.</p>
            <br />
            <h3>We want you:</h3>
            <p>We are looking for QUALITY ANALYSTS to fill our demands in our growing business.</p>
            <p>If you have the qualifications that we are looking for, then PointQA needs you!</p>
            <ul>
                <li>6 mos call center experience or the equivalent</li>
                <li>at least high school graduate</li>
                <li>willing to work on flexible schedules (40 hours per week)</li>
                <li>with exceptional listening and analytical skills</li>
                <li>flexible, detailed, and able to successfully adapt to change</li>
                <li>ability to work independently</li>
            </ul>
            <br />
            <h3>Compensation Package:</h3>
            <p>We offer variable pay performance, which basically means that the more productive and accurate you are, the more you get per work hour! We also provide tenure based bonus! And will weekly pay out turn you on? And YES you get to work in the comfort of your own home, no dress codes!</p>
            <p>Here’s a sample calculator for you to play with:</p>
            <table class="computation">
                <tr>
                    <td class="w45">
                        <p><strong>Review Time</strong> – hours spent listening and scoring</p>
                        <p><strong>Call Time</strong> – total hours of calls listened to (this determines the hours you get paid with)</p>
                        <p><strong>Tenure Based</strong> – the more time you work for us, the more hour base pay rate you’ll get!</p>
                        <p><strong>Calibration Score</strong> – your scored calibration (done by your Team Lead on a weekly basis), the more you score, the higher your hourly base</p>
                    </td>
                    <td class="w10"></td>
                    <td class="w45">
                        <table class="computation2">
                            <tr>
                                <td>Enter Your Start Date (mm/dd/yyyy)</td>
                                <td class="right"><input type="date" id="startdate" value="" /></td>
                            </tr>
                            <tr>
                                <td>Enter your review time (hh:mm:ss)</td>
                                <td class="right">
                                    <input type="number" id="rhh" value="00" min="0" />:<input type="number" id="rmm" value="00" min="0" max="59" />:<input type="number" id="rss" value="00" min="0" max="59" />
                                </td>
                            </tr>
                            <tr>
                                <td>Enter your call time (hh:mm:ss)</td>
                                <td class="right">
                                    <input type="number" id="chh" value="00" min="0" />:<input type="number" id="cmm" value="00" min="0" max="59" />:<input type="number" id="css" value="00" min="0" max="59" />
                                </td>
                            </tr>
                            <tr>
                                <td>Enter Your Calibration Score</td>
                                <td class="right"><input type="number" id="calibscore" value="0" min="0" max="100" />%</td>
                            </tr>
                            <tr>
                                <td>Call Speed</td>
                                <td class="right"><span id="speed"></span>%</td>
                            </tr>
                        </table>
                        <br />
                        <table class="computation2">
                            <tr>
                                <td colspan="2" style="text-align:center;">Results</td>
                            </tr>
                            <tr>
                                <td>Base Hourly</td>
                                <td class="right">$<span id="base">1.50</span></td>
                            </tr>
                            <tr>
                                <td>Efficiency Factor</td>
                                <td class="right"><span id="eff"></span>%</td>
                            </tr>
                            <tr>
                                <td>Calibration Factor</td>
                                <td class="right"><span id="calib"></span>%</td>
                            </tr>
                            <tr>
                                <td>Calculated Hourly</td>
                                <td class="right">$<span id="hourly"></span></td>
                            </tr>
                            <tr>
                                <td>Paid Time (hh:mm:ss)</td>
                                <td class="right"><span id="time">00:00:00</span></td>
                            </tr>
                            <tr>
                                <td>Total Pay in USD</td>
                                <td class="right">$<span id="totalUS"></span></td>
                            </tr>
                            <tr>
                                <td>Total Pay in PHP ($1 = P<span id="forex"></span>)</td>
                                <td class="right">P<span id="totalPH"></span></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="right italic">*Calculated weekly</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <p><strong><u>Speed Evaluating Calls</u></strong> = The amount of time the call is / the amount of time you spent reviewing it. For example, if the call lasts for 10 minutes and you spend 5 minutes reviewing it, then the speed is 10 minutes / 5 minutes = 200% speed.</p>
            <p><strong><u>Calibration %</u></strong> = Each week, you will have a number of your calls blindly evaluated by your team lead. The score is based on how well your answers match up with your team leads. (Calibration Lookup Table is to the right). You will match up what calibration score you receive and then add in that % to the formula above. <span class="italic">For example, if you’re calibration score is a 94% - then you’re calculated calibration % would be 101%.</span></p>
            <p><strong><u>Reviewed (Worked) Hours</u></strong> = The way we calculate the amount of hours you work is based on the time you spend reviewing calls. We do not use any timer to monitor this, but our system knows when you start and stop calls.</p>
            <br />
            <h3>Referral Program:</h3>
            <p>If you refer someone to us that passes all of the tests and stays active for at least 1 full month, both YOU and the new referral will receive a $25 bonus EACH on your next paycheck! It's as simple as that. Just make sure to tell your team lead in advance of your referral so we know who to send that bonus to!</p>
            <br />
            <h2 class="italic">So what are you waiting for?</h2>
            <p class="fs18">Here’s our no sweat hiring process to join our team!</p>
            <div class="hover blue_circle">
                <span>Sample</span>
            </div>
            <div class="hover green_circle">

            </div>
        </div>
    </div>

    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script>
        $(document).ready(function () {
            $(".hover").hover(function () {
                $(this).filter(':not(:animated)').animate({ width: '410px', height: '337px' }, 'normal');
            },function () {
                $(this).animate({ width: '370px', height: '297px' }, 'normal');
            });

            var USDtoPHP = 46.2;
            $('#forex').text(USDtoPHP);

            $('#startdate, #rhh, #rmm, #rss, #chh, #cmm, #css, #calibscore').keyup(function () {
                compute();
            }).change(function () {
                compute();
            });

            $('#rmm, #rss, #cmm, #css').keyup(function () {
                check($(this));
                compute();
            }).change(function () {
                check($(this));
                compute();
            });

            function compute() {
                $('#speed').text(((convertHHMMSSToSeconds($('#chh').val() + ':' + $('#cmm').val() + ':' + $('#css').val()) / convertHHMMSSToSeconds($('#rhh').val() + ':' + $('#rmm').val() + ':' + $('#rss').val())) * 100).toFixed(2));

                var today = new Date();
                var hired = new Date($('#startdate').val());

                var ytoday = today.getFullYear();
                var yhired = hired.getFullYear();
                var mtoday = today.getMonth();
                var mhired = hired.getMonth();
                if (mtoday === 0) {
                    mtoday++;
                    mhired++;
                }

                var months = (ytoday - yhired) * 12 + (mtoday - mhired);
                if (months < 0)
                    months = 0;
                var base = 1.50;
                $('#base').text((base + (.10 * Math.floor(months / 3))).toFixed(2));
                $('#eff').text(($('#speed').text() - 100).toFixed(2) / 2 + 100);
                if (parseFloat($('#calibscore').val()) <= 59.99)
                    $('#calib').text('55');
                else if (parseFloat($('#calibscore').val()) <= 69.99)
                    $('#calib').text('65');
                else if (parseFloat($('#calibscore').val()) <= 79.99)
                    $('#calib').text('75');
                else if (parseFloat($('#calibscore').val()) <= 84.99)
                    $('#calib').text('85');
                else if (parseFloat($('#calibscore').val()) <= 89.99)
                    $('#calib').text('90');
                else if (parseFloat($('#calibscore').val()) <= 92.99)
                    $('#calib').text('95');
                else if (parseFloat($('#calibscore').val()) <= 96.99)
                    $('#calib').text('101');
                else if (parseFloat($('#calibscore').val()) <= 98.99)
                    $('#calib').text('102.50');
                else
                    $('#calib').text('105');

                $('#hourly').text(($('#base').text() * ($('#eff').text()/100) * ($('#calib').text()/100)).toFixed(2));
                $('#time').text($('#rhh').val() + ':' + $('#rmm').val() + ':' + $('#rss').val());
                $('#totalUS').text(((convertHHMMSSToSeconds($('#time').text()) / 60 / 60) * $('#hourly').text()).toFixed(2));
                $('#totalPH').text(($('#totalUS').text() * USDtoPHP).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            }

            function check(obj) {
                if (parseInt(obj.val()) < 10)
                    obj.val('0' + obj.val());
            }

            function convertHHMMSSToSeconds(time) {
                var a = time.split(':');
                return (+a[0]) * 60 * 60 + (+a[1]) * 60 + (+a[2]);
            }

            function convertSecondsToHHMMSS(seconds) {
                var h = parseInt(seconds / 3600);
                var m = parseInt(seconds / 60) % 60;
                var s = seconds % 60;
                return (h) + ":" + (m < 10 ? "0" + m : m) + ":" + (s < 10 ? "0" + s : s);
            }
        });
    </script>

    </form>
</body>
</html>
