using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Data;
using System.Data.SqlClient;

namespace WebApi.Code
{
    public class AudioHelper
    {
        public string GetAudioFileNameByFId(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.CommandText
                    = @"select onAWS, audio_link  ,scorecards.review_type,stream_only
                                from dbo.form_score3  
                                INNER JOIN dbo.XCC_REPORT_NEW ON dbo.form_score3.review_ID = dbo.XCC_REPORT_NEW.ID   
                                join scorecards on xcc_report_new.scorecard = scorecards.id   
                                join app_settings on app_settings.appname = xcc_report_new.appname where form_score3.id = " + id;

                sqlComm.Connection = sqlCon;
                sqlCon.Open();
                SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(sqlComm);
                adapter.Fill(dt);
            }
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["onAWS"].ToString() == "True")
                {
                    string audio_link = dt.Rows[0]["audio_link"].ToString();
                    IAmazonS3 s3Client;
                    s3Client = new AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings["CCAWSAccessKey"], System.Configuration.ConfigurationManager.AppSettings["CCCAWSSecretKey"], Amazon.RegionEndpoint.APSoutheast1);
                    GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest();
                    GetPreSignedUrlRequest URL_REQ = new GetPreSignedUrlRequest();
                    URL_REQ.BucketName = "callcriteriasingapore" + audio_link.Substring(0, audio_link.LastIndexOf("/")).Replace("/audio2/", "/audio/");
                    URL_REQ.Key = audio_link.Substring(audio_link.LastIndexOf("/") + 1);
                    URL_REQ.Expires = DateTime.Now.AddHours(1);
                    return s3Client.GetPreSignedURL(URL_REQ);
                }

                if (dt.Rows[0]["review_type"].ToString() == "website")
                {
                    return "/point1sec.mp3";
                }

                if (dt.Rows[0]["stream_only"].ToString() == "True")
                {
                    return dt.Rows[0]["audio_link"].ToString();
                }
            }
            return "/point1sec.mp3";
        }

        public string GetAudioFileNameByXId(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.CommandText
                    = @"select onAWS, audio_link from dbo.XCC_REPORT_NEW  where onAWS=1 and  id = " + id;

                sqlComm.Connection = sqlCon;
                sqlCon.Open();
                SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(sqlComm);
                adapter.Fill(dt);
            }
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["onAWS"].ToString() == "True")
                {
                    string audio_link = dt.Rows[0]["audio_link"].ToString();
                    IAmazonS3 s3Client;
                    s3Client = new AmazonS3Client(System.Configuration.ConfigurationManager.AppSettings["CCAWSAccessKey"], System.Configuration.ConfigurationManager.AppSettings["CCCAWSSecretKey"], Amazon.RegionEndpoint.APSoutheast1);
                    GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest();
                    GetPreSignedUrlRequest URL_REQ = new GetPreSignedUrlRequest();
                    URL_REQ.BucketName = "callcriteriasingapore" + audio_link.Substring(0, audio_link.LastIndexOf("/")).Replace("/audio2/", "/audio/");
                    URL_REQ.Key = audio_link.Substring(audio_link.LastIndexOf("/") + 1);
                    URL_REQ.Expires = DateTime.Now.AddHours(1);
                    return s3Client.GetPreSignedURL(URL_REQ);
                }

                if (dt.Rows[0]["review_type"].ToString() == "website")
                {
                    return "/point1sec.mp3";
                }


                return dt.Rows[0]["audio_link"].ToString();

            }
            return "/point1sec.mp3";
        }
    }
}