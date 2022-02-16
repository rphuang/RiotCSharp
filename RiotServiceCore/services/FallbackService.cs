using RiotData;

namespace RiotService
{
    internal class FallbackService : ServiceBase
    {
        /// <summary>
        /// Process the Get requests for unhandled path
        /// </summary>
        public object Get(FallbackDto request)
        {
            if (!Entry()) { }

            if (string.IsNullOrEmpty(request.PathInfo))
            {
                return GetNodeList("/");
            }

            List<NodeData> nodes = GetNodeList("/" + request.PathInfo);
            if (nodes.Count > 0) return nodes;
            return ExitWithErrorResponse(400, "Bad Request");
        }

        private List<NodeData> GetNodeList(string root)
        {
            Microsoft.AspNetCore.Http.HttpRequest originalRequest = (Microsoft.AspNetCore.Http.HttpRequest)Request.OriginalRequest;
            string urlBase = $"{originalRequest.Scheme}://{originalRequest.Host.Value}";
            List<NodeData> nodeList = new List<NodeData>();
            foreach (var item in NodeInfo.ServiceNodeInfoDict)
            {
                if (item.Key.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                {
                    NodeInfo info = item.Value;
                    NodeData nodeData = new NodeData()
                    {
                        Name = info.Name,
                        Path = item.Key,
                        Type = info.ServiceType.ToString(),
                        Url = urlBase + item.Key,
                    };
                    nodeList.Add(nodeData);
                }
            }
            return nodeList;
        }
    }
}
