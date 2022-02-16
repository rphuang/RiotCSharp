using LogLib;
using RiotData;
using ServiceStack;

namespace RiotService
{
    /// <summary>
    /// provide base utilities for all concrete services
    /// </summary>
    public class ServiceBase : Service
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ServiceBase()
        {
        }

        // the keys in Request.Items
        protected const string Key4RequestId = "RiotRequestId";          // for the request ID
        protected const string Key4RequestNodeInfo = "RiotRequestNodeInfo"; // for the NodeInfo that matches the request
        protected const string Key4RequestMatchedPath = "RiotMatchedPath";  // the path that was used to match the NodeInfo
        protected const string Key4RequestUnmatchedPaths = "RiotUnmatchedPaths";  // the paths (in list) that was not used to match the NodeInfo

        /// <summary>
        /// get request ID (requires Entry call first)
        /// </summary>
        protected string RequestId { get { return GetFromRequestItems<string>(Key4RequestId); } }

        /// <summary>
        /// get NodeInfo that matches the request (requires Entry call first)
        /// </summary>
        protected NodeInfo RequestNodeInfo { get { return GetFromRequestItems<NodeInfo>(Key4RequestNodeInfo); } }

        /// <summary>
        /// get the path in the request that was used to match the NodeInfo (requires Entry call first)
        /// </summary>
        protected string RequestMatchedPath { get { return GetFromRequestItems<string>(Key4RequestMatchedPath); } }

        /// <summary>
        /// get the paths in the request that was not used to match the NodeInfo (requires Entry call first)
        /// </summary>
        protected string RequestUnmatchedPaths { get { return GetFromRequestItems<string>(Key4RequestUnmatchedPaths); } }

        /// <summary>
        /// this should be called for all services at the starting of request handling methods 
        ///  - create request ID and save in Request.Items with key RiotRequestId
        ///  - log the starting of http method
        ///  - find NodeInfo that matches the request and save following in the Request.Items
        ///     * RiotRequestNodeInfo: NodeInfo matched to this request
        ///     * RiotMatchedPath: the path that was used to match the NodeInfo
        ///     * RiotUnmatchedPaths: the path that was not used to match the NodeInfo
        /// </summary>
        protected bool Entry()
        {
            string requestId = Guid.NewGuid().ToString();
            Request.Items.Add(Key4RequestId, requestId);
            Log.Action($"{GetType().Name} Processing {Request.Verb} {Request.AbsoluteUri} ID: {requestId}");
            NodeInfo nodeInfo;
            string matchedPath;
            List<string> unmatchedPaths;
            if (GetNodeInfoFromRequest(out nodeInfo, out matchedPath, out unmatchedPaths))
            {
                Request.Items.Add(Key4RequestNodeInfo, nodeInfo);
                Request.Items.Add(Key4RequestMatchedPath, matchedPath);
                Request.Items.Add(Key4RequestUnmatchedPaths, unmatchedPaths);
                return true;
            }
            return false;
        }

        /// <summary>
        /// one of these exit methods should be called by concrete Service classes before exiting request handling methods
        ///   Exit - request handled successfully with non-generic response
        ///   ExitWithError - error in request handling with non-generic response
        ///   ExitWithResponse - request handled successfully and create a generic response
        ///   ExitWithErrorResponse - error in request handling and create a generic error response
        /// </summary>
        protected void Exit(int status)
        {
            Log.Pass($"Processed status: {status}. ID: {RequestId}");
        }

        /// <summary>
        /// one of these exit methods should be called by concrete Service classes before exiting request handling methods
        ///   Exit - request handled successfully with non-generic response
        ///   ExitWithError - error in request handling with non-generic response
        ///   ExitWithResponse - request handled successfully and create a generic response
        ///   ExitWithErrorResponse - error in request handling and create a generic error response
        /// </summary>
        protected void ExitWithError(int status, string message)
        {
            LogError($"Error status: {status} error: {message}");
        }

        /// <summary>
        /// one of these exit methods should be called by concrete Service classes before exiting request handling methods
        ///   Exit - request handled successfully with non-generic response
        ///   ExitWithError - error in request handling with non-generic response
        ///   ExitWithResponse - request handled successfully and create a generic response
        ///   ExitWithErrorResponse - error in request handling and create a generic error response
        /// </summary>
        protected Response ExitWithResponse(int status)
        {
            Log.Pass($"Processed status: {status}. ID: {RequestId}");
            return new Response { StatusCode = status };
        }

        /// <summary>
        /// one of these exit methods should be called by concrete Service classes before exiting request handling methods
        ///   Exit - request handled successfully with non-generic response
        ///   ExitWithError - error in request handling with non-generic response
        ///   ExitWithResponse - request handled successfully and create a generic response
        ///   ExitWithErrorResponse - error in request handling and create a generic error response
        /// </summary>
        protected Response ExitWithErrorResponse(int status, string message)
        {
            LogError($"Error status: {status} error: {message}");
            return new Response { StatusCode = status, ErrorMessage = message };
        }

        /// <summary>
        /// LogInfo should be used to log any info so RequestId will also be logged
        /// </summary>
        protected void LogInfo(string format, params object[] arg)
        {
            string msgFormat = $"{format} ID: {RequestId}";
            Log.Info(msgFormat, arg);
        }

        /// <summary>
        /// LogError should be used to log any error so RequestId will also be logged
        /// </summary>
        protected void LogError(string format, params object[] arg)
        {
            string msgFormat = $"{format} ID: {RequestId}";
            Log.Error(msgFormat, arg);
        }

        /// <summary>
        /// LogWarn should be used to log any warning so RequestId will also be logged
        /// </summary>
        protected void LogWarn(string format, params object[] arg)
        {
            string msgFormat = $"{format} ID: {RequestId}";
            Log.Warn(msgFormat, arg);
        }

        /// <summary>
        /// use the Request.PathInfo to find a matching NodeInfo to serve the request
        /// </summary>
        protected bool GetNodeInfoFromRequest(out NodeInfo handler, out string matchedPath, out List<string> unmatchedPaths)
        {
            handler = null;
            matchedPath = null;
            unmatchedPaths = null;
            // first: find handler that matches the whole path
            string pathInfo = this.Request.PathInfo;
            if (NodeInfo.ServiceNodeInfoDict.TryGetValue(pathInfo, out handler))
            {
                matchedPath = pathInfo;
            }
            else
            {
                // then remove the last segment of the path and find a match until reaching "/"
                while (true)
                {
                    int index = pathInfo.LastIndexOf('/');
                    if (index == 0)
                    {
                        if (NodeInfo.ServiceNodeInfoDict.TryGetValue("/", out handler)) matchedPath = "/";
                        break;
                    }
                    else
                    {
                        string unmatched = pathInfo.Substring(index + 1);
                        if (!string.IsNullOrEmpty(unmatched))
                        {
                            if (unmatchedPaths == null) unmatchedPaths = new List<string>();
                            unmatchedPaths.Insert(0, unmatched);
                        }
                        pathInfo = pathInfo.Substring(0, index);
                        if (NodeInfo.ServiceNodeInfoDict.TryGetValue(pathInfo, out handler))
                        {
                            matchedPath = pathInfo;
                            break;
                        }
                    }
                }
            }
            return handler != null && !string.IsNullOrEmpty(matchedPath);
        }

        protected T GetFromRequestItems<T>(string key)
        {
            object item;
            if (Request.Items.TryGetValue(key, out item)) return (T)item;
            return default(T);
        }
    }
}
