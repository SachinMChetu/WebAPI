using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data;
using System.Web;
using HttpMultipartParser;
using WebApi.DataLayer;

namespace WebApi.DataLayerlo
{
    // Represents JSON
    public class ChoosenFilesList
    {
        public List<ChoosenFileInfo> choosenFiles;
        public string xcc_id;
    }
    // Represents JSON
    public class ChoosenFileInfo
    {
        public string fileName;
        public string type;
        public string url;
    }

    /// <summary>
    /// 
    /// </summary>
    public class MultiformUploadRequest
    {
        private Dictionary<string, FileStream> filestreamsByName = new Dictionary<string, FileStream>();
        private StreamingMultipartFormDataParser parser;
        public ChoosenFilesList choosenFiles = null;
        // Public MA As CCInternalAPI.Maudio = New CCInternalAPI.Maudio
        public List<string> audio_url = new List<string>();
        private DataTable xcc_dt;

        /// <summary>
        /// MultiformUploadRequest
        /// </summary>
        /// <param name="input"></param>
        /// <param name="boundary"></param>
        public MultiformUploadRequest(Stream input, string boundary)
        {
            parser = new StreamingMultipartFormDataParser(input, boundary, Encoding.UTF8, 1024 * 1024);
            parser.FileHandler = new StreamingMultipartFormDataParser.FileStreamDelegate(FileStreamHandler);
            parser.ParameterHandler = new StreamingMultipartFormDataParser.ParameterDelegate(ParameterHandler);
            parser.StreamClosedHandler = new StreamingMultipartFormDataParser.StreamClosedDelegate(StreamClosedHandler);

            parser.Run();
        }

        /// <summary>
        /// FileStreamHandler
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <param name="contentDisposition"></param>
        /// <param name="buffer"></param>
        /// <param name="bytes"></param>
        private void FileStreamHandler(string name, string fileName, string contentType, string contentDisposition, byte[] buffer, int bytes)
        {
            // move sql query out of this funtion 
            string appname = xcc_dt.Rows[0]["appname"].ToString();
            string calldate = Strings.Format(xcc_dt.Rows[0]["call_date"], "MM_dd_yyyy");
            string fullname = HttpContext.Current.Server.MapPath(@"\audio\" + appname + @"\" + calldate + @"\" + fileName);
            if (filestreamsByName.ContainsKey(fileName) == false)
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("/audio/" + appname + "/" + calldate)) == false)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/audio/" + appname + "/" + calldate));
                FileStream fs = new FileStream(fullname, FileMode.Create, FileAccess.ReadWrite);
                filestreamsByName.Add(fileName, fs);
            }
            filestreamsByName[fileName].Write(buffer, 0, bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        private void ParameterHandler(HttpMultipartParser.ParameterPart param)
        {
            if (param.Name == "mediaoptions")
            {
                string chFilesJson = param.Data;
                choosenFiles = JsonConvert.DeserializeObject<ChoosenFilesList>(chFilesJson);
                xcc_dt = Common.GetTable("select appname, call_date from xcc_report_new where id=" + choosenFiles.xcc_id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StreamClosedHandler()
        {
            string appname = xcc_dt.Rows[0]["appname"].ToString();
            string calldate = Strings.Format(xcc_dt.Rows[0]["call_date"], "MM_dd_yyyy");
            foreach (ChoosenFileInfo fileInfo in choosenFiles.choosenFiles)
            {
                if (fileInfo.type == "DISTANT")
                    audio_url.Add(fileInfo.url); // https:..... ft...
                else
                    audio_url.Add(HttpContext.Current.Server.MapPath(@"\audio\" + appname + @"\" + calldate + @"\" + fileInfo.fileName));
            }
            foreach (FileStream fs in filestreamsByName.Values)
                fs.Close();
        }
    }
}