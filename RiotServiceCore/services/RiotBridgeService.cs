using HttpLib;
using RiotData;
using System.Text;

namespace RiotService
{
    /// <summary>
    /// RiotBridgeService is a bridge between two Riot services
    /// upon receiving request, it forward the request from the client to the target Riot service defined in NodeInfo
    /// </summary>
    class RiotBridgeService : ServiceBase
    {
        /// <summary>
        /// Process the Get requests
        /// </summary>
        public object Get(RiotBridgeDto request)
        {
            if (!Entry()) return ExitWithErrorResponse(400, "Bad Request");

            HttpLib.HttpClient client = new HttpLib.HttpClient(RequestNodeInfo.Address, RequestNodeInfo.Account);
            HttpResponse response = client.GetResponse(GetRequestQuery(request));
            if (response == null)
            {
                return ExitWithErrorResponse(400, "Bad Request");
            }
            else if (response.Success)
            {
                Exit((int)response.Response.StatusCode);
                return response.Result;
            }
            return ExitWithErrorResponse(400, response.ErrorMessage);
        }

        /// <summary>
        /// Process the post requests
        /// </summary>
        public object Post(RiotBridgeDto request)
        {
            if (!Entry()) return ExitWithErrorResponse(400, "Bad Request");

            HttpLib.HttpClient client = new HttpLib.HttpClient(RequestNodeInfo.Address, RequestNodeInfo.Account);
            string json = Request.GetRawBody();
            //string json = GetJson(Request.InputStream);
            HttpResponse response = client.Post(GetRequestQuery(request), json);
            if (response == null)
            {
                return ExitWithErrorResponse(400, "Bad Request");
            }
            else if (response.Success)
            {
                Exit((int)response.Response.StatusCode);
                return response.Result;
            }
            return ExitWithErrorResponse(400, response.ErrorMessage);
        }

        /// <summary>
        /// get the request path and query string to forward to RIOT service
        /// </summary>
        protected string GetRequestQuery(RiotBridgeDto request)
        {
            string queryString = Request.QueryString.ToString();
            if (string.IsNullOrEmpty(queryString)) return request.RiotRequest;
            return $"{request.RiotRequest}?{queryString}";
        }

        protected string GetJson(Stream inputStream)
        {
            string result = null;
            // read the stream associated with the response.
            using (StreamReader readStream = new StreamReader(inputStream, Encoding.UTF8))
            {
                result = readStream.ReadToEnd();
            }
            return result;
        }
    }
}
