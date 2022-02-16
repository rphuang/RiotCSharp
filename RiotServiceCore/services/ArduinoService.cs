using RiotArduinoLib;
using RiotData;

namespace RiotService
{
    class ArduinoService : ServiceBase
    {
        /// <summary>
        /// Process the Get requests for arduino
        /// </summary>
        public object Get(ArduinoDto request)
        {
            if (!Entry()) return ExitWithErrorResponse(400, "Bad Request");

            string key = string.Empty;
            if (string.IsNullOrEmpty(request.Prop)) key = request.Name;
            else key = $"{request.Name}/{request.Prop}";
            ArduinoMessage message = new ArduinoMessage(ArduinoMessage.GetMethodName, key);
            SerialportRequest arduinoRequest = ArduinoRequest;
            ArduinoMessage responseMsg = arduinoRequest.Get(message);
            if (responseMsg != null)
            {
                Exit(200);
                return new GpioResponse { Pin = responseMsg.Key, Property = responseMsg.PropertyName, Value = responseMsg.Value };
            }
            return ExitWithErrorResponse(500, "Service Not Available");
        }

        /// <summary>
        /// Process the post requests
        /// </summary>
        public object Post(ArduinoDto request)
        {
            if (!Entry()) return ExitWithErrorResponse(400, "Bad Request");
            if (string.IsNullOrEmpty(request.Value)) return ExitWithErrorResponse(400, "Bad Request");

            ArduinoMessage message = new ArduinoMessage(ArduinoMessage.PostMethodName, $"{request.Name}/{request.Prop}", request.Value);
            SerialportRequest arduinoRequest = ArduinoRequest;
            arduinoRequest.Post(message);
            return ExitWithResponse(200);
        }

        private SerialportRequest ArduinoRequest
        {
            get
            {
                SerialportRequest serialportRequest = new SerialportRequest(RequestNodeInfo.Address);
                return serialportRequest;
            }
        }
    }
}
