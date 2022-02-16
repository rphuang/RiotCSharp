using LogLib;
using System.IO.Ports;

namespace RiotArduinoLib
{
    /// <summary>
    /// SerialportRequest implements the send/receive functions for Arduino via serial port
    /// </summary>
    public class SerialportRequest : ArduinoRequest
    {
        public const int DefaultBaudRate = 9600;

        /// <summary>
        /// construct with port name and DefaultBaudRate
        /// </summary>
        public SerialportRequest(string portName) : base(portName)
        {
            _port = new SerialPort(portName, DefaultBaudRate);
            _port.ReadTimeout = DefaultTimeoutMilliseconds;
            _port.WriteTimeout = DefaultTimeoutMilliseconds;
        }

        /// <summary>
        /// construct with port name and baud rate
        /// </summary>
        public SerialportRequest(string portName, int baudRate) : base(portName)
        {
            _port = new SerialPort(portName, baudRate);
            _port.ReadTimeout = DefaultTimeoutMilliseconds;
            _port.WriteTimeout = DefaultTimeoutMilliseconds;
        }

        /// <summary>
        /// construct with a SerialPort
        /// </summary>
        public SerialportRequest(SerialPort port) : base(port.PortName)
        {
            _port = port;
        }

        /// <summary>
        /// Send a Get request and return response
        /// </summary>
        public override ArduinoMessage Get(ArduinoMessage message)
        {
            if (!string.Equals(ArduinoMessage.GetMethodName, message.Method))
            {
                throw new Exception("Method is not supported: " + message.Method);
            }
            if (!message.IsValid)
            {
                // unknown key path
                throw new Exception("Unknown key: " + message.Key);
            }

            Send(ArduinoMessage.GetMethodName, message.Key, null);
            return ReadMessage();
        }

        /// <summary>
        /// Send a Get message based on key and return response
        /// </summary>
        public override ArduinoMessage Get(string key)
        {
            Send(ArduinoMessage.GetMethodName, key, null);
            return ReadMessage();
        }

        /// <summary>
        /// send (post) a ROC message with value
        /// </summary>
        public override void Send(string method, string key, string value)
        {
            string msg;
            if (string.IsNullOrEmpty(value)) msg = string.Format("{0}?{1}", method, key);
            else msg = string.Format("{0}?{1}={2}", method, key, value);
            Send(msg, false);
        }

        /// <summary>
        /// send ROC message
        /// </summary>
        /// <param name="content">the content of ROC message (without header and end char)</param>
        /// <param name="getResponse">whether to get response or not.</param>
        /// <returns>returns RocMessage received if getResponse is true otherwise null. </returns>
        public ArduinoMessage Send(string content, bool getResponse)
        {
            string msg = string.Format("{0}{1}{2}", MessageHeader, content, MessageEndDelimiter);
            SendText(msg);
            if (getResponse) return ReadMessage();
            return null;
        }

        protected ArduinoMessage ReadMessage()
        {
            try
            {
                string text = GetText();
                return ParseMessage(text);
            }
            catch (TimeoutException)
            {
                Log.Error("Timeout while reading from {0}", _name);
            }
            catch (Exception err)
            {
                Log.Error("Exception while reading from {0}: {1}", _name, err.ToString());
            }
            return null;
        }

        /// <summary>
        /// get text from the serial port
        /// </summary>
        protected string GetText()
        {
            try
            {
                _port.Open();
                string text = _port.ReadTo(MessageTail);
                _port.Close();
                Log.Action("Received from {0} message: {1}", _port.PortName, text.Replace("\n", "\\n").Replace("\r", "\\r"));
                text = text.Trim();
                int index = text.LastIndexOf(MessageHeader);
                if (index >= 0)
                {
                    text = text.Substring(index) + MessageEndDelimiter;
                }
                return text;
            }
            catch (Exception err)
            {
                if (err is TimeoutException)
                {
                    string text = _port.ReadExisting();
                    Log.Info("ReadExisting text: {0}", text.Replace("\n", "\\n").Replace("\r", "\\r"));
                }
                _port.Close();
                Log.Error("{0} read exception: {1}", _port.PortName, err.ToString());
            }
            return null;
        }

        /// <summary>
        /// send text to the IO source.
        /// </summary>
        protected void SendText(string msg)
        {
            Log.Action("Sending to {0} message: {1}", _port.PortName, msg);
            try
            {
                _port.Open();
                _port.WriteLine(msg);
                _port.Close();
            }
            catch (Exception err)
            {
                Log.Error("{0} write exception: {1}", _port.PortName, err.ToString());
            }
            _port.Close();
        }

        private SerialPort _port;
    }
}
