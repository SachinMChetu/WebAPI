using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using Amazon.S3.Model;

namespace DAL.Models
{

	public class AllUserCallDocuments
	{
		public bool allowUserCallDocuments { get; set; } = false;
		public List<baseUserCallDocuments> userDocs { get; set; } = new List<baseUserCallDocuments>();
		public static AllUserCallDocuments Create(IDataReader reader)
		{
			var userDoc = new AllUserCallDocuments();
			if (reader.Read())
			{
				userDoc.allowUserCallDocuments = reader.Get<bool>("allowDocumentUpload");
				if (reader.NextResult())
				{
					while (reader.Read())
					{
						userDoc.userDocs.Add(baseUserCallDocuments.Create(reader));
					}
				}
			}
			return userDoc;
		}
	}
	public class baseUserCallDocuments
	{
		public string fileName { get; set; } = "";
		public int ID { get; set; } = 0;
		public string s3Key { get; set; } = "";
        public string fileURL { get; set; } = "";
        public int f_ID { get; set; } = 0;
        public string description { get; set; } = "";
        public string url{get;set;} = "";
		public static baseUserCallDocuments Create(IDataRecord record)
		{
            var item = new baseUserCallDocuments();
            item.Load(record);
            return item;
		}

        private string awsAccessKey = ConfigurationManager.AppSettings["CCAWSAccessKey"];
        private string awsSecretAccessKey = ConfigurationManager.AppSettings["CCCAWSSecretKey"];
        private string awsBucketName = ConfigurationManager.AppSettings["awsBucketName"];

        private string GeneratePreSignedURL(string s3Key)
        {
            string urlString = "";
            try
            {
                using (IAmazonS3 client = new AmazonS3Client(awsAccessKey, awsSecretAccessKey, Amazon.RegionEndpoint.USWest2))
                {
                    GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
                    {
                        BucketName = awsBucketName,
                        Key = s3Key,
                        Expires = DateTime.Now.AddMinutes(30)
                    };
                    urlString = client.GetPreSignedURL(request1);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            return urlString;
        }

        public void Load(IDataRecord record)
        {
            fileName = record.GetValueOrDefault<string>("fileName", "");
            ID = record.Get<int>("ID");
            s3Key = record.GetValueOrDefault<string>("s3Key", "");
            fileURL = GeneratePreSignedURL(s3Key);
            f_ID = record.Get<int>("F_ID");
            description = record.GetValueOrDefault<string>("description", "");
            
            var s3Client = new AmazonS3Client(ConfigurationManager.AppSettings["CCAWSAccessKey"],ConfigurationManager.AppSettings["CCCAWSSecretKey"], Amazon.RegionEndpoint.USWest2);
            GetPreSignedUrlRequest URL_REQ = new GetPreSignedUrlRequest
            {
                Key = s3Key,
                BucketName = ConfigurationManager.AppSettings["awsBucketName"]+"/"+s3Key,
                Expires = DateTime.Now.AddHours(1)
            };
            url = s3Client.GetPreSignedURL(URL_REQ);

        }

		public int Save()
		{			
			using (var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
			{
				var cmd = new SqlCommand();
				cmd.CommandText = "insert into review_documents (filename,s3Key,F_ID,Description)  output INSERTED.ID values (@filename,@s3Key,@F_ID,@Description)";
				cmd.Parameters.AddWithValue("@filename", fileName);
				cmd.Parameters.AddWithValue("@s3Key", s3Key);
				cmd.Parameters.AddWithValue("@F_ID", f_ID);
                cmd.Parameters.AddWithValue("@description", description);
				cmd.Connection = con;
				con.Open();
				ID  = (int)cmd.ExecuteScalar();
			}
			return ID;
		}

		public virtual void Delete()
		{
			if (ID != 0)
			{
				using (var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CC_ProdConn"].ConnectionString))
				{
					using (var cmd = new SqlCommand())
					{
						cmd.Connection = con;
						cmd.CommandText = "delete from review_documents where id=@id";
						cmd.Parameters.AddWithValue("@id", ID);						
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
					}
				}
			}
		}
	}

	public class UploadUserCallDocuments : baseUserCallDocuments
	{
        private MemoryStream data { get; set; } = new MemoryStream();
		private string awsBucketName { get; set; } = "";
		private string awsAccessKey { get; set; } = "";
		private string awsSecretAccessKey { get; set; } = "";
        private string fileType { get; set; } = "";
        
		public async Task UploadtoAWS()
		{			
			s3Key = Guid.NewGuid().ToString() + fileType;
			using (var tu = new TransferUtility(new AmazonS3Client(awsAccessKey, awsSecretAccessKey, Amazon.RegionEndpoint.USWest2)))
			{
                await tu.UploadAsync(data, awsBucketName, s3Key);
            }
			Save();			
		}

        public void AddFile(byte[] arr, string fileEnding)
        {
            data = new MemoryStream(arr);
            fileType = fileEnding;
        }

        public async Task LoadFromAWS()
        {            
            using (IAmazonS3 client = new AmazonS3Client(awsAccessKey, awsSecretAccessKey, Amazon.RegionEndpoint.USWest2))
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest();
                getObjectRequest.BucketName = awsBucketName;
                getObjectRequest.Key = s3Key;

                using (var getObjectResponse = client.GetObject(getObjectRequest))
                {
                   await getObjectResponse.ResponseStream.CopyToAsync(data);
                }
            }            
        }

        public void DeleteFromAws()
		{
			using (var client = new AmazonS3Client(awsAccessKey, awsSecretAccessKey, Amazon.RegionEndpoint.USWest2))
			{				
				var delObj = new DeleteObjectRequest()
				{
					BucketName = awsBucketName,
					Key = s3Key
				};				
				client.DeleteObject(delObj);
			}
		}

		public UploadUserCallDocuments()
		{
			awsAccessKey = ConfigurationManager.AppSettings["CCAWSAccessKey"];
			awsSecretAccessKey = ConfigurationManager.AppSettings["CCCAWSSecretKey"];
			awsBucketName = ConfigurationManager.AppSettings["awsBucketName"];
		}

		public override void Delete()
		{
			DeleteFromAws();
			base.Delete();
		}
        /// <summary>
        ///    Loads the data from the record then grabs the stream from AWS.
        /// </summary>
        /// <param name="record">DB Record to fille the row</param>
        /// <returns></returns>
        public new static async Task<UploadUserCallDocuments> Create(IDataRecord record)
        {
            var u = new UploadUserCallDocuments();
            u.Load(record);
            await u.LoadFromAWS();
            return u;
        }

    }

}
