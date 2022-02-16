using RiotData;
using ServiceStack;
using ServiceStack.Configuration;

namespace RiotService
{
    /// <summary>
    /// Define the Web Services AppHost
    /// </summary>
    public class AppHost : AppSelfHostBase
    {
        public AppHost()
          : base("HttpListener Self-Host", typeof(FallbackService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            // force the default to json
            SetConfig(new HostConfig
            {
                DefaultContentType = MimeTypes.Json
            });

            // configure the routes from app.config
            IAppSettings appSettings = new AppSettings();
            List<string> keys = appSettings.GetAllKeys();
            foreach (string key in keys)
            {
                NodeInfo info = appSettings.Get<NodeInfo>(key);
                if (string.IsNullOrEmpty(info.Name)) info.Name = key;
                switch (info.ServiceType)
                {
                    case ServiceType.Arduino:
                        AddRoutes(typeof(ArduinoDto), info);
                        break;
                    case ServiceType.RiotService:
                        AddRoutes(typeof(RiotBridgeDto), info);
                        break;
                    case ServiceType.PiGpio:
                        AddRoutes(typeof(GpioDto), info);
                        break;
                    case ServiceType.System:
                        AddRoutes(typeof(SystemDto), info);
                        break;
                }
                NodeInfo.ServiceNodeInfoDict.Add(key, info);
                LogLib.Log.Info("AppHost configured {0} type: {1} route: {2} ", key, info.ServiceType, info.Routes);
            }

            // add these filters so we can use Request.GetRawBody() to get json body
            PreRequestFilters.Add((httpReq, httpRes) => {
                httpReq.UseBufferedStream = true;  // Buffer Request Input
                httpRes.UseBufferedStream = true;  // Buffer Response Output
            });
        }

        private void AddRoutes(Type type, NodeInfo info)
        {
            string[] vs = info.Routes.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach(string route in vs) Routes.Add(type, route, info.Methods);
        }
    }
}
