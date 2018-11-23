using DAL.GenericRepository;
using DAL.Models;
using Itenso.TimePeriod;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Export
{
    public class ExportMyPayCode
    {
        public dynamic ExportMyPay(string weekEnd, string userName)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {

                //Filter f = new Filter() { filters = filters.filters, range = filters.range };
                string sql = "getPaymentInfo";
                SqlCommand sqlComm;

                sqlComm = new SqlCommand(sql, sqlCon) { CommandTimeout = 60 };
                sqlComm.Parameters.AddWithValue("@userName", userName);
                sqlComm.Parameters.AddWithValue("@weekEnd", weekEnd);
                sqlComm.CommandType = CommandType.StoredProcedure;
                MyPay myPay = new MyPay();

                var callsLeftLst = new List<CallsLeft>();
                var pendingCalls = new List<PendingCall>();

                var qaPaymentInfo = new List<PaymentInfo>();
                var calibratorPaymentInfo = new List<PaymentInfo>();
                var notificationPaymentInfo = new List<PaymentInfo>();

                List<ScorecardPaymentInfo> niWeekInfo = new List<ScorecardPaymentInfo>();
                List<ScorecardPaymentInfo> qaweekInfo = new List<ScorecardPaymentInfo>();
                List<ScorecardPaymentInfo> finalInfo = new List<ScorecardPaymentInfo>();
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    while (reader.Read())
                    {
                        myPay.startDate = DateTime.Parse(reader.GetValue(0).ToString());
                        myPay.weeks = new List<List<ScorecardPaymentInfo>>();
                    }
                    if (reader.NextResult())
                    {

                        while (reader.Read())
                        {
                            try
                            {
                                var pinfo = new PaymentInfo()
                                {
                                    totalCallTime = float.Parse(reader.GetValue(reader.GetOrdinal("callTime")).ToString()),
                                    totalReviewTime = float.Parse(reader.GetValue(reader.GetOrdinal("reviewtime")).ToString()),
                                    periodEnding = DateTime.Parse(reader.GetValue(reader.GetOrdinal("Week_ending_date")).ToString()),
                                    baseRate = float.Parse(reader.GetValue(reader.GetOrdinal("base")).ToString()),
                                    score = (float)Math.Round(decimal.Parse(reader.GetValue(reader.GetOrdinal("calibrationScore")).ToString()), 2),
                                    disputeCost = float.Parse(reader.GetValue(reader.GetOrdinal("dispute_cost")).ToString()),
                                    disputeCount = int.Parse(reader.GetValue(reader.GetOrdinal("num_disputes")).ToString()),
                                    totalBadCallReviewTime = float.Parse(reader.GetValue(reader.GetOrdinal("bad_reviewtime")).ToString()),
                                    totalBadCallTime = float.Parse(reader.GetValue(reader.GetOrdinal("bad_call_lenght")).ToString()),
                                    percentChange = float.Parse(reader.GetValue(reader.GetOrdinal("percent_change")).ToString()),
                                    calibrationCount = float.Parse(reader.GetValue(reader.GetOrdinal("calibrationsCount")).ToString()),
                                    startDate = DateTime.Parse(reader.GetValue(reader.GetOrdinal("startDate")).ToString()),
                                    payType = reader.GetValue(reader.GetOrdinal("pay_type")).ToString(),
                                    qaPay = reader.GetValue(reader.GetOrdinal("qa_pay")).ToString(),
                                    numberCalls = int.Parse(reader.GetValue(reader.GetOrdinal("number_calls")).ToString()),
                                    totalPay = 0,
                                    adjustedRate = 0,
                                    callSpeed = 0,
                                };
                                ScorecardInfo scorecardInfo = new ScorecardInfo()
                                {
                                    scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecard")).ToString()),
                                    scorecardName = reader.GetValue(reader.GetOrdinal("short_name")).ToString()

                                };
                                if (scorecardInfo.scorecardId == 491)
                                {

                                }
                                ScorecardPaymentInfo spinf = new ScorecardPaymentInfo() { scorecard = scorecardInfo, qaPaymentInfo = pinfo, weekEnd = pinfo.periodEnding };
                                qaweekInfo.Add(spinf);

                                pinfo.baseRate = (float)Math.Round((decimal)(1.6f + 0.05f * new DateDiff(pinfo.startDate, pinfo.periodEnding).Months), 8);

                                pinfo.baseRate += (new DateDiff(pinfo.startDate, pinfo.periodEnding).Days < 0) ? .05f : 0.0f;

                                if (pinfo.baseRate > 3)
                                {
                                    pinfo.baseRate = 3;
                                }


                                pinfo.disputeCost = pinfo.baseRate * pinfo.disputeCost;

                                //pinfo.disputeCost = pinfo.baseRate * .2f;

                                pinfo.callSpeed = (new DateDiff(pinfo.startDate, pinfo.periodEnding).Days > 14) ? (float)Math.Round(((pinfo.totalCallTime - pinfo.totalBadCallTime) / 3600) / ((pinfo.totalReviewTime - pinfo.totalBadCallReviewTime) / 3600) * 100, 8) : 100.0f;


                                if (float.IsNaN(pinfo.callSpeed))
                                {
                                    pinfo.callSpeed = 100;
                                }

                                pinfo.paymentRate = 100;
                                if (pinfo.callSpeed > 100)
                                {
                                    pinfo.paymentRate = (float)Math.Round(((pinfo.callSpeed - 100) / 2 + 100) / 100, 8);
                                }
                                else
                                {
                                    pinfo.paymentRate = (float)Math.Round(pinfo.callSpeed / 100, 8);
                                }
                                if (pinfo.calibrationCount != 0)
                                {
                                    try
                                    {
                                        pinfo.adjustedRate = (float)Math.Round((decimal)(pinfo.baseRate * (pinfo.paymentRate) * (1 + pinfo.percentChange / 100)), 2);
                                    }
                                    catch 
                                    {
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        pinfo.adjustedRate = (float)Math.Round((decimal)(pinfo.baseRate * (pinfo.paymentRate)), 2);
                                    }
                                    catch 
                                    {
                                    }
                                }




                                switch (pinfo.payType)
                                {
                                    case "Per Item":
                                        if ((float)Convert.ToSingle(pinfo.qaPay) > 0)
                                        {
                                            pinfo.totalPay = pinfo.baseRate / Convert.ToSingle(pinfo.qaPay) * (int)pinfo.numberCalls - (pinfo.disputeCost * pinfo.disputeCount);
                                            pinfo.adjustedRate = pinfo.baseRate;
                                            pinfo.callSpeed = 100;

                                        }
                                        else
                                        {
                                            pinfo.totalPay = Convert.ToSingle(pinfo.qaPay) * (int)pinfo.numberCalls - (pinfo.disputeCost * pinfo.disputeCount);
                                            pinfo.adjustedRate = pinfo.baseRate;
                                            pinfo.callSpeed = 100;

                                        }

                                        break;
                                    case "Per Call Time":
                                        pinfo.totalPay = (pinfo.adjustedRate * (pinfo.totalCallTime / 3600) - (pinfo.disputeCost * pinfo.disputeCount));
                                        break;
                                    default:
                                        pinfo.totalPay = (pinfo.adjustedRate * (pinfo.totalReviewTime / 3600) - (pinfo.disputeCost * pinfo.disputeCount));
                                        break;
                                }



                            }
                            catch 
                            {

                            }
                        }
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var pinfo = new PaymentInfo()
                            {
                                totalCallTime = (float)Math.Round(decimal.Parse(reader.GetValue(reader.GetOrdinal("callTime")).ToString()), 2),
                                totalReviewTime = (float)Math.Round(decimal.Parse(reader.GetValue(reader.GetOrdinal("reviewtime")).ToString()), 2),
                                periodEnding = DateTime.Parse(reader.GetValue(reader.GetOrdinal("Week_ending_date")).ToString()),
                                baseRate = float.Parse(reader.GetValue(reader.GetOrdinal("base")).ToString()),
                                score = (float)Math.Round(decimal.Parse(reader.GetValue(reader.GetOrdinal("calibrationScore")).ToString()), 2),
                                disputeCost = float.Parse(reader.GetValue(reader.GetOrdinal("dispute_cost")).ToString()),
                                disputeCount = int.Parse(reader.GetValue(reader.GetOrdinal("num_disputes")).ToString()),
                                percentChange = float.Parse(reader.GetValue(reader.GetOrdinal("percent_change")).ToString()),
                                //totalBadCallReviewTime = float.Parse(reader.GetValue(reader.GetOrdinal("bad_reviewtime")).ToString()),
                                //totalBadCallTime = float.Parse(reader.GetValue(reader.GetOrdinal("bad_call_lenght")).ToString()),

                                complitedNotification = int.Parse(reader.GetValue(reader.GetOrdinal("notificationComplited")).ToString()),
                                totalPay = 0,
                                adjustedRate = 0,
                                callSpeed = 0,
                            };
                            //float  efficiency = (pinfo.totalCallTime - pinfo.totalBadCallTime) / (pinfo.totalReviewTime - pinfo.totalBadCallReviewTime) * 100f;
                            pinfo.callSpeed = (pinfo.totalCallTime - pinfo.totalBadCallTime) / (pinfo.totalReviewTime - pinfo.totalBadCallReviewTime) * 100;
                            if (pinfo.totalCallTime != 0)
                            {
                                if (pinfo.score > 0)
                                {
                                    pinfo.adjustedRate = (float)(pinfo.baseRate * (1 + pinfo.percentChange / 100));
                                }
                                else
                                {
                                    pinfo.adjustedRate = pinfo.baseRate;
                                }
                            }
                            else
                            {
                                pinfo.adjustedRate = pinfo.baseRate;
                            }
                            pinfo.totalPay = (float)Math.Round((decimal)(pinfo.adjustedRate * pinfo.totalReviewTime / 3600 + pinfo.complitedNotification * .4), 2);
                            pinfo.adjustedRate = (float)Math.Round(pinfo.adjustedRate, 2);
                            qaPaymentInfo.Add(pinfo);

                            ScorecardInfo scorecardInfo = new ScorecardInfo()
                            {
                                scorecardId = int.Parse(reader.GetValue(reader.GetOrdinal("scorecard")).ToString()),
                                scorecardName = reader.GetValue(reader.GetOrdinal("short_name")).ToString()
                            };
                            ScorecardPaymentInfo spinf = new ScorecardPaymentInfo() { scorecard = scorecardInfo, calibratorPaymentInfo = pinfo, weekEnd = pinfo.periodEnding };

                            if (qaweekInfo.Count == 0)
                            {
                                finalInfo.Add(spinf);
                            }
                            else
                            {

                                ScorecardPaymentInfo merged = new ScorecardPaymentInfo()
                                {
                                    calibratorPaymentInfo = spinf.calibratorPaymentInfo,
                                    scorecard = spinf.scorecard,
                                    weekEnd = spinf.weekEnd,
                                    qaPaymentInfo = (from qai in qaweekInfo
                                                     where
                              (qai.scorecard.scorecardId == spinf.scorecard.scorecardId && qai.weekEnd == spinf.weekEnd)
                                                     select qai.qaPaymentInfo).FirstOrDefault()

                                };
                                finalInfo.Add(merged);
                            }
                        }
                    }
                    if (reader.NextResult())
                    {
                        myPay.weekEnds = new List<string>();
                        while (reader.Read())
                        {
                            myPay.weekEnds.Add(reader.GetFieldValue<string>(reader.GetOrdinal("endDate")));
                        }
                    }

                    finalInfo.AddRange(
                            (
                                from info in qaweekInfo
                                where !finalInfo.Any(f => f.scorecard.scorecardId == info.scorecard.scorecardId && f.weekEnd == info.weekEnd)
                                select info
                            ).ToList()
                        );


                    try
                    {
                        myPay.weeks = (from f in finalInfo group f by f.weekEnd into g select g.ToList()).ToList();
                    }
                    catch
                    {
                        return new MyPay();
                    }
                    //return myPay;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                List<PropertieName> propNames = new List<PropertieName>();


                foreach (var item in myPay.weeks)
                {
                    foreach (var i in item)
                    {
                        if (i.qaPaymentInfo != null && i.calibratorPaymentInfo == null)
                        {
                            propNames = new List<PropertieName>
                    {
                        new PropertieName { propName = "Week end", propValue = "weekEnd", propPosition = 1 },
                        new PropertieName { propName = "Scorecard name", propValue = "scorecardName", propPosition = 2 },
                        new PropertieName { propName = "Scorecard Id", propValue = "scorecardId", propPosition = 3 },


                        new PropertieName { propName = "QA adjusted rate", propValue = "adjustedRate", propPosition = 4 },
                        new PropertieName { propName = "QA base rate", propValue = "baseRate", propPosition = 5 },
                        new PropertieName { propName = "QA calibration count", propValue = "calibrationCount", propPosition = 6 },
                        new PropertieName { propName = "QA call speed", propValue = "callSpeed", propPosition = 7 },
                        new PropertieName { propName = "QA complited notification", propValue = "complitedNotification", propPosition = 8 },
                        new PropertieName { propName = "QA complited cost", propValue = "complitedСost", propPosition = 9 },
                        new PropertieName { propName = "QA dispute cost", propValue = "disputeCost", propPosition = 10 },
                        new PropertieName { propName = "QA dispute count", propValue = "disputeCount", propPosition = 11 },
                        new PropertieName { propName = "QA payment rate", propValue = "paymentRate", propPosition = 12 },
                        new PropertieName { propName = "QA percent сhange", propValue = "percentChange", propPosition = 13 },
                        new PropertieName { propName = "QA period ending", propValue = "periodEnding", propPosition = 14 },
                        new PropertieName { propName = "QA Calibration Score", propValue = "score", propPosition = 15 },
                        new PropertieName { propName = "QA start date", propValue = "startDate", propPosition = 16 },
                        new PropertieName { propName = "QA total bad call review time", propValue = "totalBadCallReviewTime", propPosition = 17 },
                        new PropertieName { propName = "QA total bad call time", propValue = "totalBadCallTime", propPosition = 18 },
                        new PropertieName { propName = "QA total call time", propValue = "totalCallTime", propPosition = 19 },
                        new PropertieName { propName = "QA total pay", propValue = "totalPay", propPosition = 20 },
                        new PropertieName { propName = "QA total review time", propValue = "totalReviewTime", propPosition = 21 }
                        };
                          //  break;
                        }
                        if (i.calibratorPaymentInfo != null && i.qaPaymentInfo == null)
                        {
                            propNames = new List<PropertieName>
                     {
                        new PropertieName { propName = "Week end", propValue = "weekEnd", propPosition = 1 },
                        new PropertieName { propName = "Scorecard name", propValue = "scorecardName", propPosition = 2 },
                        new PropertieName { propName = "Scorecard Id", propValue = "scorecardId", propPosition = 3 },
                         new PropertieName { propName = "Calibrator adjusted rate", propValue = "CadjustedRate", propPosition = 4 },
                        new PropertieName { propName = "Calibrator base rate", propValue = "CbaseRate", propPosition = 5 },
                        new PropertieName { propName = "Calibrator calibration count", propValue = "CcalibrationCount", propPosition = 6 },
                        new PropertieName { propName = "Calibrator call speed", propValue = "CcallSpeed", propPosition = 7 },
                        new PropertieName { propName = "Calibrator complited notification", propValue = "CcomplitedNotification", propPosition = 8 },
                        new PropertieName { propName = "Calibrator complited cost", propValue = "CcomplitedСost", propPosition = 9 },
                        new PropertieName { propName = "Calibrator dispute cost", propValue = "CdisputeCost", propPosition = 10 },
                        new PropertieName { propName = "Calibrator dispute count", propValue = "CdisputeCount", propPosition = 11 },
                        new PropertieName { propName = "Calibrator payment rate", propValue = "CpaymentRate", propPosition = 12 },
                        new PropertieName { propName = "Calibrator percent сhange", propValue = "CpercentChange", propPosition = 13 },
                        new PropertieName { propName = "Calibrator period ending", propValue = "CperiodEnding", propPosition = 14 },
                        new PropertieName { propName = "Calibrator Recalibration Score", propValue = "Cscore", propPosition = 15 },
                        new PropertieName { propName = "Calibrator start date", propValue = "CstartDate", propPosition = 16 },
                        new PropertieName { propName = "Calibrator total bad call review time", propValue = "CtotalBadCallReviewTime", propPosition = 17 },
                        new PropertieName { propName = "Calibrator total bad call time", propValue = "CtotalBadCallTime", propPosition = 18 },
                        new PropertieName { propName = "Calibrator total call time", propValue = "CtotalCallTime", propPosition = 19 },
                        new PropertieName { propName = "Calibrator total pay", propValue = "CtotalPay", propPosition = 20 },
                        new PropertieName { propName = "Calibrator total review time", propValue = "CtotalReviewTime", propPosition = 21 }
                     };
                          //  break;
                        }
                        if (i.qaPaymentInfo != null && i.calibratorPaymentInfo != null)
                        {
                            propNames = new List<PropertieName>
                    {
                        new PropertieName { propName = "Week end", propValue = "weekEnd", propPosition = 1 },
                        new PropertieName { propName = "Scorecard name", propValue = "scorecardName", propPosition = 2 },
                        new PropertieName { propName = "Scorecard Id", propValue = "scorecardId", propPosition = 3 },


                        new PropertieName { propName = "QA adjusted rate", propValue = "adjustedRate", propPosition = 4 },
                        new PropertieName { propName = "QA base rate", propValue = "baseRate", propPosition = 5 },
                        new PropertieName { propName = "QA calibration count", propValue = "calibrationCount", propPosition = 6 },
                        new PropertieName { propName = "QA call speed", propValue = "callSpeed", propPosition = 7 },
                        new PropertieName { propName = "QA complited notification", propValue = "complitedNotification", propPosition = 8 },
                        new PropertieName { propName = "QA complited cost", propValue = "complitedСost", propPosition = 9 },
                        new PropertieName { propName = "QA dispute cost", propValue = "disputeCost", propPosition = 10 },
                        new PropertieName { propName = "QA dispute count", propValue = "disputeCount", propPosition = 11 },
                        new PropertieName { propName = "QA payment rate", propValue = "paymentRate", propPosition = 12 },
                        new PropertieName { propName = "QA percent сhange", propValue = "percentChange", propPosition = 13 },
                        new PropertieName { propName = "QA period ending", propValue = "periodEnding", propPosition = 14 },
                        new PropertieName { propName = "QA Calibration Score", propValue = "score", propPosition = 15 },
                        new PropertieName { propName = "QA start date", propValue = "startDate", propPosition = 16 },
                        new PropertieName { propName = "QA total bad call review time", propValue = "totalBadCallReviewTime", propPosition = 17 },
                        new PropertieName { propName = "QA total bad call time", propValue = "totalBadCallTime", propPosition = 18 },
                        new PropertieName { propName = "QA total call time", propValue = "totalCallTime", propPosition = 19 },
                        new PropertieName { propName = "QA total pay", propValue = "totalPay", propPosition = 20 },
                        new PropertieName { propName = "QA total review time", propValue = "totalReviewTime", propPosition = 21 },





                        new PropertieName { propName = "Calibrator adjusted rate", propValue = "CadjustedRate", propPosition = 22 },
                        new PropertieName { propName = "Calibrator base rate", propValue = "CbaseRate", propPosition = 23 },
                        new PropertieName { propName = "Calibrator calibration count", propValue = "CcalibrationCount", propPosition = 24 },
                        new PropertieName { propName = "Calibrator call speed", propValue = "CcallSpeed", propPosition = 26 },
                        new PropertieName { propName = "Calibrator complited notification", propValue = "CcomplitedNotification", propPosition = 27 },
                        new PropertieName { propName = "Calibrator complited cost", propValue = "CcomplitedСost", propPosition = 28 },
                        new PropertieName { propName = "Calibrator dispute cost", propValue = "CdisputeCost", propPosition = 29 },
                        new PropertieName { propName = "Calibrator dispute count", propValue = "CdisputeCount", propPosition = 30 },
                        new PropertieName { propName = "Calibrator payment rate", propValue = "CpaymentRate", propPosition = 31 },
                        new PropertieName { propName = "Calibrator percent сhange", propValue = "CpercentChange", propPosition = 32 },
                        new PropertieName { propName = "Calibrator period ending", propValue = "CperiodEnding", propPosition = 33 },
                        new PropertieName { propName = "Calibrator Recalibration Score", propValue = "Cscore", propPosition = 34 },
                        new PropertieName { propName = "Calibrator start date", propValue = "CstartDate", propPosition = 35 },
                        new PropertieName { propName = "Calibrator total bad call review time", propValue = "CtotalBadCallReviewTime", propPosition = 36 },
                        new PropertieName { propName = "Calibrator total bad call time", propValue = "CtotalBadCallTime", propPosition = 37 },
                        new PropertieName { propName = "Calibrator total call time", propValue = "CtotalCallTime", propPosition = 38 },
                        new PropertieName { propName = "Calibrator total pay", propValue = "CtotalPay", propPosition = 39 },
                        new PropertieName { propName = "Calibrator total review time", propValue = "CtotalReviewTime", propPosition = 40 }
                    };
                           // break;
                        }
                    }
                }

                List<ExportMyPayModel> exportMyPayModel = new List<ExportMyPayModel>();
                try
                {
                    foreach (var item in myPay.weeks)
                    {
                        foreach (var i in item)
                        {
                            if (i.qaPaymentInfo != null && i.calibratorPaymentInfo == null)
                            {
                                exportMyPayModel.Add(new ExportMyPayModel
                                {
                                    weekEnd = i.weekEnd,
                                    scorecardName = i.scorecard.scorecardName,
                                    scorecardId = i.scorecard.scorecardId,

                                    adjustedRate = i.qaPaymentInfo.adjustedRate,
                                    baseRate = i.qaPaymentInfo.baseRate,
                                    calibrationCount = i.qaPaymentInfo.calibrationCount,
                                    callSpeed = i.qaPaymentInfo.callSpeed,
                                    complitedNotification = i.qaPaymentInfo.complitedNotification,
                                    complitedСost = i.qaPaymentInfo.complitedСost,
                                    disputeCost = i.qaPaymentInfo.disputeCost,
                                    disputeCount = i.qaPaymentInfo.disputeCount,
                                    paymentRate = i.qaPaymentInfo.paymentRate,
                                    percentChange = i.qaPaymentInfo.percentChange,
                                    periodEnding = i.qaPaymentInfo.periodEnding,
                                    score = i.qaPaymentInfo.score,
                                    startDate = i.qaPaymentInfo.startDate,
                                    totalBadCallReviewTime = i.qaPaymentInfo.totalBadCallReviewTime,
                                    totalBadCallTime = i.qaPaymentInfo.totalBadCallTime,
                                    totalCallTime = i.qaPaymentInfo.totalCallTime,
                                    totalPay = (float?)Math.Round((double)i.qaPaymentInfo.totalPay),
                                    totalReviewTime = i.qaPaymentInfo.totalReviewTime,
                                });
                            }
                            if (i.calibratorPaymentInfo != null && i.qaPaymentInfo == null)
                            {
                                exportMyPayModel.Add(new ExportMyPayModel
                                {
                                    weekEnd = i.weekEnd,
                                    scorecardName = i.scorecard.scorecardName,
                                    scorecardId = i.scorecard.scorecardId,

                                    CadjustedRate = i.calibratorPaymentInfo.adjustedRate,
                                    CbaseRate = i.calibratorPaymentInfo.baseRate,
                                    CcalibrationCount = i.calibratorPaymentInfo.calibrationCount,
                                    CcallSpeed = i.calibratorPaymentInfo.callSpeed,
                                    CcomplitedNotification = i.calibratorPaymentInfo.complitedNotification,
                                    CcomplitedСost = i.calibratorPaymentInfo.complitedСost,
                                    CdisputeCost = i.calibratorPaymentInfo.disputeCost,
                                    CdisputeCount = i.calibratorPaymentInfo.disputeCount,
                                    CpaymentRate = i.calibratorPaymentInfo.paymentRate,
                                    CpercentChange = i.calibratorPaymentInfo.percentChange,
                                    CperiodEnding = i.calibratorPaymentInfo.periodEnding,
                                    Cscore = i.calibratorPaymentInfo.score,
                                    CstartDate = i.calibratorPaymentInfo.startDate,
                                    CtotalBadCallReviewTime = i.calibratorPaymentInfo.totalBadCallReviewTime,
                                    CtotalBadCallTime = i.calibratorPaymentInfo.totalBadCallTime,
                                    CtotalCallTime = i.calibratorPaymentInfo.totalCallTime,
                                    CtotalPay = i.calibratorPaymentInfo.totalPay,
                                    CtotalReviewTime = i.calibratorPaymentInfo.totalReviewTime
                                });
                            }
                            if (i.qaPaymentInfo != null && i.calibratorPaymentInfo != null)
                            {
                                exportMyPayModel.Add(new ExportMyPayModel
                                {
                                    weekEnd = i.weekEnd,
                                    scorecardName = i.scorecard.scorecardName,
                                    scorecardId = i.scorecard.scorecardId,

                                    adjustedRate = i.qaPaymentInfo.adjustedRate,
                                    baseRate = i.qaPaymentInfo.baseRate,
                                    calibrationCount = i.qaPaymentInfo.calibrationCount,
                                    callSpeed = i.qaPaymentInfo.callSpeed,
                                    complitedNotification = i.qaPaymentInfo.complitedNotification,
                                    complitedСost = i.qaPaymentInfo.complitedСost,
                                    disputeCost = i.qaPaymentInfo.disputeCost,
                                    disputeCount = i.qaPaymentInfo.disputeCount,
                                    paymentRate = i.qaPaymentInfo.paymentRate,
                                    percentChange = i.qaPaymentInfo.percentChange,
                                    periodEnding = i.qaPaymentInfo.periodEnding,
                                    score = i.qaPaymentInfo.score,
                                    startDate = i.qaPaymentInfo.startDate,
                                    totalBadCallReviewTime = i.qaPaymentInfo.totalBadCallReviewTime,
                                    totalBadCallTime = i.qaPaymentInfo.totalBadCallTime,
                                    totalCallTime = i.qaPaymentInfo.totalCallTime,
                                    totalPay = i.qaPaymentInfo.totalPay,
                                    totalReviewTime = i.qaPaymentInfo.totalReviewTime,

                                    CadjustedRate = i.calibratorPaymentInfo.adjustedRate,
                                    CbaseRate = i.calibratorPaymentInfo.baseRate,
                                    CcalibrationCount = i.calibratorPaymentInfo.calibrationCount,
                                    CcallSpeed = i.calibratorPaymentInfo.callSpeed,
                                    CcomplitedNotification = i.calibratorPaymentInfo.complitedNotification,
                                    CcomplitedСost = i.calibratorPaymentInfo.complitedСost,
                                    CdisputeCost = i.calibratorPaymentInfo.disputeCost,
                                    CdisputeCount = i.calibratorPaymentInfo.disputeCount,
                                    CpaymentRate = i.calibratorPaymentInfo.paymentRate,
                                    CpercentChange = i.calibratorPaymentInfo.percentChange,
                                    CperiodEnding = i.calibratorPaymentInfo.periodEnding,
                                    Cscore = i.calibratorPaymentInfo.score,
                                    CstartDate = i.calibratorPaymentInfo.startDate,
                                    CtotalBadCallReviewTime = i.calibratorPaymentInfo.totalBadCallReviewTime,
                                    CtotalBadCallTime = i.calibratorPaymentInfo.totalBadCallTime,
                                    CtotalCallTime = i.calibratorPaymentInfo.totalCallTime,
                                    CtotalPay = i.calibratorPaymentInfo.totalPay,
                                    CtotalReviewTime = i.calibratorPaymentInfo.totalReviewTime
                                });
                            }

                        }
                    }

                    ExportHelper.Export(propNames, exportMyPayModel, "MyPay" + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Millisecond.ToString() + ".xlsx", "MyPay", userName);
                }
                catch (Exception ex) { throw ex; }
            }



            return "success";
        }
    }
}
