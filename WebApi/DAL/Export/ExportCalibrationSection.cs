using DAL.Code;
using DAL.Extensions;
using DAL.GenericRepository;
using DAL.Models;
using DAL.Models.CalibrationModels;
using DAL.Models.ExportModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Export
{
    public class ExportCalibrationSection
    {
        public dynamic CalibrationSectionExport(Filter filters,string userName)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sqlComm = DashboardHelpers.GetFiltersParameters(filters, "[GetCalibratorSectionScores]", userName);
                sqlComm.Connection = sqlCon;
                try
                {
                    sqlCon.Open();
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    var sectioInfoRaw = new List<SectionInfoRaw>();
                    while (reader.Read())
                    {
                        try
                        {
                            sectioInfoRaw.Add(SectionInfoRaw.CreateRecord(reader));

                        }
                        catch
                        {

                        }
                    }

                    List<SectionsExportModel> sectionsExportModels = new List<SectionsExportModel>();
                    var propNames = new List<PropertieName>
                    {
                        new PropertieName { propName = "Scorecard Name", propValue = "scorecardName", propPosition = 1 },
                        new PropertieName { propName = "Section Name", propValue = "sectionName", propPosition = 2 },
                        new PropertieName { propName = "Question Name", propValue = "questionName", propPosition = 3 },
                        new PropertieName { propName = "isLinked", propValue = "isLinked", propPosition = 4 },
                        new PropertieName { propName = "Total Right", propValue = "totalRight", propPosition = 5 },
                        new PropertieName { propName = "Right Score, %", propValue = "rightScore", propPosition = 6 },
                        new PropertieName { propName = "Total Wrong", propValue = "totalWrong", propPosition = 7 },
                        new PropertieName { propName = "Wrong Score, %", propValue = "wrongScore", propPosition = 8 },
                        new PropertieName { propName = "Total", propValue = "total", propPosition = 9 }
                    };
                    foreach (var item in sectioInfoRaw)
                    {
                       // var tot = item.totalRight = item.totalWrong;
                        sectionsExportModels.Add(new SectionsExportModel
                        {
                            scorecardName = item.scorecardName,
                            sectionName = item.sectionName,
                            questionName = item.questionShortName,
                            isLinked = item.isLinked == true ? "Yes" : "No",
                            totalRight = item.totalRight,
                            totalWrong = item.totalWrong,
                            total = item.totalRight + item.totalWrong,
                            rightScore = (item.totalRight + item.totalWrong) == 0 ? 0 :Math.Round((double)item.totalRight / (item.totalRight + item.totalWrong) *100,2),
                            wrongScore = (item.totalRight + item.totalWrong) == 0 ? 0 : Math.Round((double)item.totalWrong / (item.totalRight + item.totalWrong) * 100,2)
                        });
                    }
                    ExportHelper.Export(propNames, sectionsExportModels, "CalibrationSection" + DateTime.Now.ToString("MM-dd-yyyy") + DateTime.Now.Second.ToString() + ".xlsx", "CalibrationSection", userName);
                    return "sucess";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
