using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApi.Code
{
    public class OptionsHttpMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
         HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Method == HttpMethod.Options)
                {
                    return Task.Factory.StartNew(() =>
                    {
                        //request.Headers.Referrer.AbsoluteUri
                        var resp = new HttpResponseMessage(HttpStatusCode.OK);
                        //string url = ;
                        resp.Headers.Add("Access-Control-Allow-Origin", request.Headers.GetValues("Origin").First());
                        resp.Headers.Add("Access-Control-Allow-Methods", "POST,OPTIONS,GET");

                        resp.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With,Access-Control-Allow-Methods,Access-Control-Allow-Credentials,Access-Control-Allow-Origin");

                        resp.Headers.Add("Access-Control-Allow-Credentials", "true");
                        return resp;
                    });
                }
            }
            catch { }
            try
            {
                if (!HttpContext.Current.Response.Headers.AllKeys.Contains("Access-Control-Allow-Origin"))
                {
                    string url = HttpContext.Current.Request.UrlReferrer.Scheme + "://"
                        + HttpContext.Current.Request.UrlReferrer.Host
                        + ((HttpContext.Current.Request.UrlReferrer.Port.ToString().Length > 2) ? (":" + HttpContext.Current.Request.UrlReferrer.Port.ToString()) : (""));
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", url);
                }
                if (!HttpContext.Current.Response.Headers.AllKeys.Contains("Access-Control-Allow-Methods"))
                {
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST,OPTIONS,GET");
                }
                if (!HttpContext.Current.Response.Headers.AllKeys.Contains("Access-Control-Allow-Headers"))
                {
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With,Access-Control-Allow-Methods,Access-Control-Allow-Credentials,Access-Control-Allow-Origin");
                }
                if (!HttpContext.Current.Response.Headers.AllKeys.Contains("Access-Control-Allow-Credentials"))
                {
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
                }
            }
            catch { }
            return base.SendAsync(request, cancellationToken);
        }
    }
}