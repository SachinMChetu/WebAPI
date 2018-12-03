using DAL.Models;
using DAL.Models.Guidelines;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi.Code;
using static DAL.Models.SectionInfoStat;

namespace WebApi.DataLayer
{
#if DEBUG

#else
    [Authorize]
#endif


    /// <summary>
    /// GuidelinesLayer
    /// </summary>
    public class GuidelinesLayer
    {
        /// <summary>
        /// GetGuidelines
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <param name="reviewId"></param>
        /// <param name="listenId"></param>
        /// <param name="username"></param>
        /// <param name="reset"></param>
        /// <returns></returns>
        public dynamic GetGuidelines(int? scorecardId, int? reviewId, int? listenId, string username, int reset = 0)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand("[GetGuidelineScorecardInfo]");
                //sqlComm.Parameters.AddWithValue("@scorecard", callInfo.scorecadId);
                sqlComm.Parameters.AddWithValue("@scorecard", scorecardId);
                sqlComm.Parameters.AddWithValue("@callId", reviewId);
                sqlComm.Parameters.AddWithValue("@listenId", listenId);
                sqlComm.Parameters.AddWithValue("@userName", username);
                sqlComm.Parameters.AddWithValue("@restartID", reset);
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandTimeout = 41;
                sqlComm.Connection = sqlCon;

                GuidelineScorecardInfo guidelineScorecardInfo = new GuidelineScorecardInfo();
                try
                {
                    sqlCon.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlComm);
                    DataSet ds = new DataSet();

                    sda.Fill(ds);
                    GlScorecard scorecard = new GlScorecard();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt0 = ds.Tables[0];

                        scorecard.scorecardName = dt0.Rows[0].Field<string>("short_name");
                        scorecard.scorecardApp = dt0.Rows[0].Field<string>("appname");
                        scorecard.scorecardId = dt0.Rows[0].Field<int>("id");
                        scorecard.scorecardType = dt0.Rows[0].Field<string>("review_type");
                        guidelineScorecardInfo.scorecardInfo = new GIScorecardInfo();
                        guidelineScorecardInfo.scorecardInfo.scorecard = scorecard;

                    }
                    if (scorecardId != scorecard.scorecardId && scorecardId != 0)
                    {
                        throw new InvalidOperationException();
                        //return BadRequest("Selected scorecard does not contain record with this ID");
                    }
                    List<GlSection> sections = new List<GlSection>();
                    if (ds.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            GlSection section = new GlSection();
                            section.questions = new List<GLQuestionInfo>();

                            SectionInfo sec = new SectionInfo();
                            sec.sectionId = dr.Field<int>("id");
                            sec.sectionOrder = dr.Field<int?>("section_order");
                            sec.sectionName = dr.Field<string>("section");

                            section.sectionInfo = sec;

                            foreach (DataRow qdr in ds.Tables[2].Rows)
                            {

                                if (qdr.Field<int>("section") == sec.sectionId)
                                {
                                    GLQuestionInfo qi = new GLQuestionInfo();

                                    //FAQs
                                    List<FAQ> faqs = new List<FAQ>();
                                    foreach (DataRow fdr in ds.Tables[5].Rows)
                                    {
                                        FAQ faq = new FAQ();
                                        if (fdr.Field<int>("question_id") == qdr.Field<int>("id"))
                                        {
                                            faq.answerText = fdr.Field<string>("question_answer");
                                            faq.questionText = fdr.Field<string>("question_text");
                                            faq.wasEdited = fdr.Field<bool>("wasEdited");
                                            faqs.Add(faq);
                                        }

                                    }
                                    qi.faqs = faqs;

                                    //Instructions
                                    List<Instruction> instructions = new List<Instruction>();
                                    foreach (DataRow fdr in ds.Tables[6].Rows)
                                    {
                                        Instruction instruction = new Instruction();
                                        if (fdr.Field<int>("question_id") == qdr.Field<int>("id"))
                                        {
                                            instruction.answerText = fdr.Field<string>("answer_type");
                                            instruction.instructionText = fdr.Field<string>("question_text");
                                            instruction.wasEdited = fdr.Field<bool>("wasEdited");
                                            instructions.Add(instruction);
                                        }

                                    }
                                    qi.instructions = instructions;

                                    qi.useQuestion = qdr.Field<bool>("use_question");
                                    qi.commentAllowed = qdr.Field<bool>("comments_allowed");
                                    qi.isComposite = qdr.Field<int>("isComposite") == 1;
                                    qi.isLinked = qdr.Field<int>("isLinked") == 1;
                                    qi.isWide = qdr.Field<bool>("wide_q");
                                    qi.dropdownType = qdr.Field<string>("ddl_type");
                                    qi.linkedAnswerId = qdr.Field<int?>("linked_answer");
                                    qi.linkedCommentId = qdr.Field<int?>("linked_comment");
                                    //qi.linkedMetaData 
                                    qi.linkedVisible = qdr.Field<bool>("linked_visible");
                                    qi.questionId = qdr.Field<int>("id");
                                    try
                                    {
                                        qi.optionVerb = (qdr.Field<string>("options_verb")).ToLower();
                                        qi.questionType = (qdr.Field<string>("q_type")).ToLower();
                                    }
                                    catch
                                    {

                                    }
                                    qi.qustionShortName = qdr.Field<string>("q_short_name");
                                    if (qdr.Field<int>("isComposite") == 1)
                                    {
                                        try
                                        {
                                            CompositeQuestionInfo cqi = new CompositeQuestionInfo();
                                            CompositeQuestionInfo answers = new CompositeQuestionInfo();
                                            answers.answers = new List<Answer>();
                                            foreach (DataRow adr in ds.Tables[3].Rows)
                                            {
                                                if (adr.Field<int>("question_id") == qdr.Field<int>("id"))
                                                {
                                                    Answer answer = new Answer();
                                                    answer.answerId = adr.Field<int>("id");
                                                    answer.answerText = adr.Field<string>("answer_text");
                                                    answer.isRightAnswer = adr.Field<bool>("right_answer");
                                                    answer.points = adr.Field<double>("answer_points");



                                                    // if (adr.Field<string>("answer_text") == "No")
                                                    {
                                                        List<QuestionTemplateItem> qtis = new List<QuestionTemplateItem>();
                                                        foreach (DataRow odr in ds.Tables[7].Rows)
                                                        {

                                                            try
                                                            {
                                                                if (odr.Field<int>("question_id") == adr.Field<int>("question_id"))
                                                                {
                                                                    QuestionTemplateItem qti = new QuestionTemplateItem();
                                                                    qti.optionId = odr.Field<int>("id");
                                                                    qti.optionText = odr.Field<string>("option_text");
                                                                    qti.@checked = odr.Field<bool>("isChecked");
                                                                    qtis.Add(qti);
                                                                }
                                                            }
                                                            catch
                                                            {

                                                            }
                                                        }



                                                        answers.answers.Add(answer);
                                                        answers.comments = (qtis);
                                                    }

                                                }



                                                // qi.compositeQuestionInfo.answers
                                            }
                                            qi.compositeQuestionInfo = answers;
                                            section.questions.Add(qi);
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                    }
                                    else
                                    {
                                        SimpleQuestionInfo simpleQuestionInfo = new SimpleQuestionInfo();
                                        List<AnswerInfo> answerInfos = new List<AnswerInfo>();

                                        foreach (DataRow adr in ds.Tables[3].Rows)
                                        {
                                            try
                                            {
                                                AnswerInfo answerInfo = new AnswerInfo();
                                                if (adr.Field<int>("question_id") == qdr.Field<int>("id"))
                                                {
                                                    Answer answer = new Answer();
                                                    answer.answerId = adr.Field<int>("id");
                                                    answer.answerText = adr.Field<string>("answer_text");
                                                    answer.isRightAnswer = adr.Field<bool>("right_answer");
                                                    answer.points = adr.Field<double?>("answer_points");
                                                    if (reviewId == 0)
                                                        answerInfo.isAnswered = adr.Field<bool>("autoselect");
                                                    else answerInfo.isAnswered = adr.Field<bool>("isAnswered");


                                                    List<GLComment> glcs = new List<GLComment>();
                                                    foreach (DataRow cdr in ds.Tables[4].Rows)
                                                    {
                                                        try
                                                        {
                                                            if (cdr.Field<int?>("answer_id") == adr.Field<int?>("id"))
                                                            {
                                                                GLComment glc = new GLComment();
                                                                glc.commentId = cdr.Field<int>("id");
                                                                glc.commentText = cdr.Field<string>("comment");
                                                                glc.points = cdr.Field<int?>("comment_points");
                                                                glc.@checked = cdr.Field<bool>("checked");
                                                                glcs.Add(glc);
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                    }

                                                    answerInfo.answer = answer;
                                                    answerInfo.comments = glcs;
                                                    answerInfos.Add(answerInfo);
                                                }
                                                //ai.commentRequired
                                            }
                                            catch (Exception ex)
                                            {

                                            }


                                            // qi.compositeQuestionInfo.answers
                                        }

                                        simpleQuestionInfo.answers = answerInfos;

                                        qi.simpleQuestionInfo = simpleQuestionInfo;
                                        section.questions.Add(qi);
                                    }
                                }

                            }
                            sections.Add(section);

                        }

                        List<MetaDataItem> mdList = new List<MetaDataItem>();
                        try
                        {
                            for (int i = 0; i < ds.Tables[8].Columns.Count; i++)
                            {

                                try
                                {
                                    mdList.Add(new MetaDataItem()
                                    {
                                        name = ds.Tables[8].Columns[i].ColumnName.ToString(),
                                        value = ds.Tables[8].Rows[0][ds.Tables[8].Columns[i].ColumnName.ToString()]
                                    });
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            string audio = "";
                            try
                            {
                                if (scorecard.scorecardType != "website")
                                {
                                    audio = new AudioHelper().GetAudioFileNameByXId(Convert.ChangeType(mdList.Where(a => a.name.ToLower() == "ID".ToLower()).Select(a => a.value).First(), typeof(int)));
                                    mdList.Where(a => a.name.ToLower() == "audio_link".ToLower()).First(a => a.value = audio);
                                }
                            }
                            catch
                            {


                            }

                            guidelineScorecardInfo.metaData = mdList;
                        }
                        catch { }
                        try
                        {
                            List<SchoolDataItem> schoolDataItem = new List<SchoolDataItem>();
                            foreach (DataRow sdr in ds.Tables[10].Rows)
                            {

                                try
                                {
                                    schoolDataItem.Add(new SchoolDataItem()
                                    {
                                        id = sdr.Field<int?>("id"),
                                        school = sdr.Field<string>("School"),
                                        AOI1 = sdr.Field<string>("AOI1"),
                                        AOI2 = sdr.Field<string>("AOI2"),
                                        Modality = sdr.Field<string>("Modality"),
                                        xccId = sdr.Field<int?>("xcc_id"),
                                        L1_SubjectName = sdr.Field<string>("L1_SubjectName"),
                                        L2_SubjectName = sdr.Field<string>("L2_SubjectName"),
                                        College = sdr.Field<string>("College"),
                                        DegreeOfInterest = sdr.Field<string>("DegreeOfInterest"),
                                        origin = sdr.Field<string>("origin"),
                                        tcpa = sdr.Field<string>("tcpa"),
                                    });
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            guidelineScorecardInfo.schoolData = schoolDataItem;
                        }
                        catch { }
                        try
                        {
                            List<OtherDataItem> otherDataItem = new List<OtherDataItem>();
                            foreach (DataRow sdr in ds.Tables[9].Rows)
                            {

                                try
                                {
                                    otherDataItem.Add(new OtherDataItem()
                                    {//id	form_id	data_key	data_value	data_type	xcc_id	school_name	label
                                        id = sdr.Field<int?>("id"),
                                        dataKey = sdr.Field<string>("data_key"),
                                        dataType = sdr.Field<string>("data_type"),
                                        dataValue = sdr.Field<string>("data_value"),
                                        formId = sdr.Field<int?>("form_id"),
                                        label = sdr.Field<string>("label"),
                                        schoolName = sdr.Field<string>("school_name"),
                                        xccId = sdr.Field<int?>("xcc_id"),

                                    });
                                }
                                catch (Exception ex)
                                {

                                }
                            }



                            guidelineScorecardInfo.otherData = otherDataItem;

                             List<GropDownItemModelGuidelines> temlateItems = new List<GropDownItemModelGuidelines>();
                            foreach (DataRow item in ds.Tables[ds.Tables.Count-1].Rows)
                            {
                                try
                                {
                                    temlateItems.Add(new GropDownItemModelGuidelines
                                    {
                                        id = item.Field<int>("id"),
                                        qID = item.Field<int>("question_id"),
                                        value = item.Field<string>("value")
                                    });
                                       // temlateItems.Add(item.Field<string>("value"));
                                    
                                }
                                catch { }

                            }
                            List<string> results = new List<string>();
                            foreach (var sect in sections)
                            {
                                foreach (var que in sect.questions)
                                {
                                     foreach (var ans in que.simpleQuestionInfo.answers)
                                     {
                                        ans.answer.dropdownItems = new List<string>();
                                        foreach (var item in temlateItems)
                                        {
                                            if(item.qID == (que.questionId == null? 0 : que.questionId))
                                            {
                                                try
                                                {
                                                    results.Add(item.value);
                                                    ans.answer.dropdownItems.Add(item.value);
                                                }
                                                catch(Exception ex)
                                                {
                                                    throw ex;
                                                }
                                                
                                            }
                                        }
                                     }

                                }
                            }
                        }
                        catch { }
                        guidelineScorecardInfo.scorecardInfo.sections = sections;


                        return guidelineScorecardInfo;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return guidelineScorecardInfo;
            }
        }
        /// <summary>
        /// SaveSchoolDataInfo
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public dynamic SaveSchoolDataInfo(List<SchoolDataItem> payload)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();
                foreach (var data in payload)
                {
                    string sql;
                    SqlCommand reply;
                    if (data.id != null)
                    {
                        sql = "update school_clone set School = @School,	AOI1 = @AOI1,AOI2 = @AOI2,L1_SubjectName = @L1_SubjectName,L2_SubjectName = @L2_SubjectName,"
                            + "Modality = @Modality,College = @College,DegreeOfInterest = @DegreeOfInterest,origin = @origin,tcpa = @tcpa where id = @id";
                    }
                    else
                    {
                        sql = "insert into school_clone " +
                                    "(id,School,AOI1,AOI2,L1_SubjectName,L2_SubjectName,Modality,College,DegreeOfInterest,origin,tcpa,xcc_id)" +
                                    "values" +
                                    "((select max(id)+1000000 from School_X_Data ),@School,@AOI1, @AOI2, @L1_SubjectName,	@L2_SubjectName, @Modality, @College, @DegreeOfInterest,@origin,@tcpa,@xcc_id)";
                    }


                    reply = new SqlCommand(sql, sqlCon);
                    reply.CommandTimeout = 60;
                    if (data.id != null)
                    {
                        reply.Parameters.AddWithValue("id", data.id);
                    }
                    else
                    {
                        reply.Parameters.AddWithValue("xcc_id", data.xccId);
                    }
                    reply.CommandType = CommandType.Text;
                    reply.Parameters.AddWithValue("@School", (data.school == null) ? DBNull.Value : (object)data.school);
                    reply.Parameters.AddWithValue("@AOI1", (data.AOI1 == null) ? DBNull.Value : (object)data.AOI1);
                    reply.Parameters.AddWithValue("@AOI2", data.AOI2 == null ? DBNull.Value : (object)data.AOI2);
                    reply.Parameters.AddWithValue("@L1_SubjectName", data.L1_SubjectName == null ? DBNull.Value : (object)data.L1_SubjectName);
                    reply.Parameters.AddWithValue("@L2_SubjectName", data.L2_SubjectName == null ? DBNull.Value : (object)data.L2_SubjectName);
                    reply.Parameters.AddWithValue("@Modality", data.Modality == null ? DBNull.Value : (object)data.Modality);
                    reply.Parameters.AddWithValue("@College", data.College == null ? DBNull.Value : (object)data.College);
                    reply.Parameters.AddWithValue("@DegreeOfInterest", data.DegreeOfInterest == null ? DBNull.Value : (object)data.DegreeOfInterest);
                    reply.Parameters.AddWithValue("@origin", data.origin == null ? DBNull.Value : (object)data.origin);
                    reply.Parameters.AddWithValue("@tcpa", data.tcpa == null ? DBNull.Value : (object)data.tcpa);
                    try
                    {
                        reply.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }

                }
                sqlCon.Close();
                sqlCon.Dispose();
                var guidelines = new GuidelinesLayer();
                string userName;
                if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                try
                {
                    return guidelines.GetGuidelines(0, null, payload[0].xccId, userName);
                }
                catch (InvalidOperationException)
                {
                    return "Selected scorecard does not contain record with this ID";
                }
                catch (Exception)
                {
                    return new GuidelineScorecardInfo();
                }
            }
        }

        /// <summary>
        /// UpdateMetadataInfo
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public dynamic UpdateMetadataInfo(UpdateMetadataPayload payload)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();
                foreach (var data in payload.metaDataItems)
                {
                    string sql;
                    if ((data.value.GetType() == typeof(Int64)
                        || data.value.GetType() == typeof(float)
                        || data.value.GetType() == typeof(double)
                        || data.value.GetType() == typeof(Int32)
                        || data.value.GetType() == typeof(Int16)
                        || data.value.GetType() == typeof(decimal)) && (data.value as string).ToLower() != "appname" && (data.value as string).ToLower() != "id")
                    {
                        sql = "update xrn_clone set " + data.name + " = " + (data.value == null) ? DBNull.Value : data.value + " where  id=@id";
                    }
                    else if (data.value != null)
                    {
                        sql = "update xrn_clone set " + data.name + " = '" + data.value + "' where  id=@id";
                    }
                    else
                    {
                        sql = "update xrn_clone set " + data.name + " = NULL' where  id=@id";
                    }

                    SqlCommand reply;

                    reply = new SqlCommand(sql, sqlCon);
                    reply.CommandTimeout = 60;
                    reply.Parameters.AddWithValue("@id", payload.id);
                    reply.ExecuteNonQuery();

                }
                sqlCon.Close();
                sqlCon.Dispose();
                string userName;
                if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                var guidelines = new GuidelinesLayer();
                try
                {
                    return guidelines.GetGuidelines(0, null, payload.id, userName);
                }
                catch (InvalidOperationException)
                {
                    return ("Selected scorecard does not contain record with this ID");
                }
                catch (Exception)
                {
                    return new GuidelineScorecardInfo();
                }
            }


        }

        /// <summary>
        /// SaveOtherDataInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public dynamic SaveOtherDataInfo(List<OtherDataItem> info)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();
                foreach (var data in info)
                {
                    string sql;
                    SqlCommand reply;
                    if (data.id != null)
                    {
                        sql = "update other_clone set form_id=@formId,data_key=@dataKey,data_value=@dataValue,data_type=@dataType,school_name=@schoolName,label=@label"
                            + "where id = @id";
                    }
                    else
                    {
                        sql = "insert into other_clone " +
                                    "(id,form_id,data_key,data_value,data_type,xcc_id,school_name,label)" +
                                    "values" +
                                    "((select max(id)+1000000 from otherFormData ),@formId,@dataKey, @dataValue, @dataType,@xcc_id,@schoolName, @label )";
                    }


                    reply = new SqlCommand(sql, sqlCon);
                    reply.CommandTimeout = 60;
                    if (data.id != null)
                    {
                        reply.Parameters.AddWithValue("id", data.id);
                    }
                    else
                    {
                        reply.Parameters.AddWithValue("xcc_id", data.xccId);
                    }
                    reply.CommandType = CommandType.Text;
                    reply.Parameters.AddWithValue("@formId", (data.formId == null) ? DBNull.Value : (object)data.formId);
                    reply.Parameters.AddWithValue("@dataKey", (data.dataKey == null) ? DBNull.Value : (object)data.dataKey);
                    reply.Parameters.AddWithValue("@dataValue", data.dataValue == null ? DBNull.Value : (object)data.dataValue);
                    reply.Parameters.AddWithValue("@dataType", data.dataType == null ? DBNull.Value : (object)data.dataType);
                    reply.Parameters.AddWithValue("@schoolName", data.schoolName == null ? DBNull.Value : (object)data.schoolName);
                    reply.Parameters.AddWithValue("@label", data.label == null ? DBNull.Value : (object)data.label);
                    try
                    {
                        reply.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }

                }
                sqlCon.Close();
                sqlCon.Dispose();
                var guidelines = new GuidelinesLayer();
                string userName;
                if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                try
                {
                    return guidelines.GetGuidelines(0, null, info.First().xccId, userName);
                }
                catch (InvalidOperationException)
                {
                    return ("Selected scorecard does not contain record with this ID");
                }
                catch (Exception)
                {
                    return new GuidelineScorecardInfo();
                }
            }
        }

        /// <summary>
        /// RemoveOtherDataInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public dynamic RemoveOtherDataInfo(OtherDataItem info)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();

                string sql;
                SqlCommand reply;

                sql = "delete from other_clone where id = @id";

                reply = new SqlCommand(sql, sqlCon);
                reply.CommandTimeout = 60;

                reply.Parameters.AddWithValue("@id", info.id);

                try
                {
                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }

                sqlCon.Close();
                sqlCon.Dispose();
                string userName;
                if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                var guidelines = new GuidelinesLayer();
                try
                {
                    return guidelines.GetGuidelines(0, null, info.xccId, userName);
                }
                catch (InvalidOperationException)
                {
                    return ("Selected scorecard does not contain record with this ID");
                }
                catch (Exception)
                {
                    return new GuidelineScorecardInfo();
                }
            }
        }

        /// <summary>
        /// RemoveSchoolDataInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public dynamic RemoveSchoolDataInfo(SchoolDataItem info)
        {
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();

                string sql;
                SqlCommand reply;

                sql = "delete from school_clone where id = @id";

                reply = new SqlCommand(sql, sqlCon);
                reply.CommandTimeout = 60;

                reply.Parameters.AddWithValue("@id", info.id);

                try
                {
                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }

                sqlCon.Close();
                sqlCon.Dispose();
                string userName;
                if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
                {
                    userName = "test321";// HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
                var guidelines = new GuidelinesLayer();
                try
                {
                    return guidelines.GetGuidelines(0, null, info.xccId, userName);
                }
                catch (InvalidOperationException)
                {
                    return ("Selected scorecard does not contain record with this ID");
                }
                catch (Exception)
                {
                    return new GuidelineScorecardInfo();
                }
            }
        }

        /// <summary>
        /// UpdateScorecardStatus
        /// </summary>
        /// <param name="scorecardID"></param>
        /// <returns></returns>
        public dynamic UpdateScorecardStatus(int scorecardID)
        {
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();

                string sql;

                SqlCommand reply;

                sql = "delete from sc_update where reviewer = '" + userName + "' and sc_id = '" + scorecardID + "';";
                sql = "insert into sc_update (reviewer, sc_id, date_reviewed) select '" + userName + "','" + scorecardID + "', dbo.getMTDate()";

                reply = new SqlCommand(sql, sqlCon);
                reply.CommandTimeout = 60;

                try
                {
                    reply.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
            }
            return "OK";
        }

        /// <summary>
        /// UpdateScorecardStatus
        /// </summary>
        /// <param name="scorecardID"></param>
        /// <returns></returns>
        public dynamic GetQuestionHistory(int scorecardID)
        {
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();

                string sql;

                SqlCommand reply;

                sql = @" select u.*,updated_by " +
                        " from (select q.* ,max(g.updated_date) as updated_date from " +
                        " (select distinct q.id,q.q_short_name , cast( case when e.wasEdited=1 then 1 else 0 end as bit) as wasedited  from questions q " +
                        " left  join  " +
                        " (select * from   (" +
                        " select cast( case when date_reviewed is null  then 1  when   date_reviewed < dateadded then 1 else 0 end as bit) as [wasEdited],questions.ID " +
                        " from q_faqs with(nolock)  " +
                        " join questions on questions.ID = question_id " +
                        " left join (select sc_id,max(date_reviewed) as date_reviewed from sc_update where reviewer = @userName group by sc_id) a on a.sc_id = scorecard_id " +
                        " where question_id in (select id from questions where scorecard_id = @scorecardID and active = 1 ))c  where wasEdited=1) e on e.id=q.id " +
                        " where q.scorecard_id = @scorecardID and q.q_short_name is not null and active = 1 )q " +
                        " join guideline_updates g on g.question_id= q.id " +
                        " group by q.id,q.q_short_name,q.wasedited)u  " +
                        " join guideline_updates g on u.updated_date= g.updated_date " +
                        " order by g.updated_date desc";


                reply = new SqlCommand(sql, sqlCon);
                reply.CommandTimeout = 60;

                reply.Parameters.AddWithValue("@scorecardID", scorecardID);
                reply.Parameters.AddWithValue("@userName", userName);
                var reader = reply.ExecuteReader();

                List<QuestionHistory> questionHistory = new List<QuestionHistory>();
                while (reader.Read())
                {
                    try
                    {
                        questionHistory.Add(new QuestionHistory
                        {
                            id = reader.GetFieldValue<int>(reader.GetOrdinal("id")),
                            userName = reader.GetFieldValue<string>(reader.GetOrdinal("updated_by")),
                            questionName = reader.GetFieldValue<string>(reader.GetOrdinal("q_short_name")),
                            isNew = reader.GetFieldValue<bool>(reader.GetOrdinal("wasEdited")),
                            updatedDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("updated_Date")),

                        });
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return questionHistory;
            }
        }

        /// <summary>
        /// UpdateScorecardStatus
        /// </summary>
        /// <param name="scorecardID"></param>
        /// <returns></returns>
        public dynamic GetQuestionHistoryByQId(int scorecardID)
        {
            string userName;
            if (HttpContext.Current.Request.UrlReferrer.Host.Contains("localhost") && HttpContext.Current.Request.UrlReferrer.Port == 51268)
            {
                userName = "test321";// HttpContext.Current.User.Identity.Name;
            }
            else
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                sqlCon.Open();

                string sql;

                SqlCommand reply;

                sql = @"select id, q_short_name from   questions where id = @qID ; " +
                        " select id,updated_by,updated_date,  from_text,to_text,from_answer,to_answer from   guideline_updates   where question_id = @qID order by updated_date desc;";


                reply = new SqlCommand(sql, sqlCon);
                reply.CommandTimeout = 60;
                reply.Parameters.AddWithValue("@qID", scorecardID);
                var reader = reply.ExecuteReader();

                var guidelinesQHistoryInfo = new GuidelinesQHistoryInfo();
                while (reader.Read())
                {
                    try
                    {
                        guidelinesQHistoryInfo.id = reader.GetFieldValue<int>(reader.GetOrdinal("id"));
                        guidelinesQHistoryInfo.questionName = reader.GetFieldValue<string>(reader.GetOrdinal("q_short_name"));
                        guidelinesQHistoryInfo.changeList = new List<QChanges>();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            guidelinesQHistoryInfo.changeList.Add(new QChanges()
                            {
                                id = reader.GetFieldValue<int>(reader.GetOrdinal("id")),
                                updatedBy = reader.GetFieldValue<string>(reader.GetOrdinal("updated_by")),
                                updatedDate = reader.GetFieldValue<DateTime?>(reader.GetOrdinal("updated_date")),
                                fromText = reader.IsDBNull(reader.GetOrdinal("from_Text")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("from_Text")),
                                toText = reader.IsDBNull(reader.GetOrdinal("to_Text")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("to_Text")),
                                fromAnswer = reader.IsDBNull(reader.GetOrdinal("from_Answer")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("from_Answer")),
                                toAnswer = reader.IsDBNull(reader.GetOrdinal("to_Answer")) ? "" : reader.GetFieldValue<string>(reader.GetOrdinal("to_Answer")),

                            });

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                return guidelinesQHistoryInfo;
            }
        }

    }
}
