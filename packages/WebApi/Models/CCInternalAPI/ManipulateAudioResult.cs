using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    /// <summary>
    /// ManipulateAudioResult
    /// </summary>
    public struct ManipulateAudioResult
    {
        public bool IsSuccess;
        public string Message;
        public string Audio;
    }
}