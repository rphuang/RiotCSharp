using System;
using System.IO.Ports;
using LogLib;

namespace RiotArduinoLib
{
    /// <summary>
    /// ArduinoRequest is the base class to send/receive request to/from Arduino
    /// </summary>
    public abstract class ArduinoRequest
    {
        // constants
        public const string MessageHeader = "##";
        public const char MethodKeyDelimiter = '?';
        public const char KeyValueDelimiter = '=';
        public const char MessageEndDelimiter = '^';
        public static readonly string MessageTail = MessageEndDelimiter.ToString();

        /// <summary>
        /// configure for verbose output
        /// </summary>
        public bool Verbose { get; set; } = false;

        /// <summary>
        /// The timeout for request
        /// </summary>
        public int DefaultTimeoutMilliseconds { get; set; } = 5000;

        /// <summary>
        /// Get request with ArduinoMessage
        /// </summary>
        public abstract ArduinoMessage Get(ArduinoMessage message);

        /// <summary>
        /// Get request with key
        /// </summary>
        public abstract ArduinoMessage Get(string key);

        /// <summary>
        /// Post a ArduinoMessage
        /// </summary>
        public void Post(ArduinoMessage message)
        {
            Send(message.Method, message.Key, message.Value);
        }

        /// <summary>
        /// Post by resource key & value
        /// </summary>
        public void Post(string key, string value)
        {
            Send(ArduinoMessage.PostMethodName, key, value);
        }

        /// <summary>
        /// send a request with specified method, resource key, & value
        /// </summary>
        public abstract void Send(string method, string key, string value);

        /// <summary>
        /// base constructor
        /// </summary>
        protected ArduinoRequest(string name)
        {
            _name = name;
        }

        protected ArduinoMessage ParseMessage(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            // minimum requirements for a valid message
            //  at least 6 char - example: ##g?k^
            //  end with ^
            int msgLen = text.Length;
            int index = text.IndexOf(MessageHeader);
            if (msgLen < 6 || index < 0)
            {
                Log.Error("Invalid ROC message from {0}: {1}", _name, text);
                return null;
            }

            // traverse through the text to parse & validate the message
            ArduinoMessage msg = new ArduinoMessage();
            index += MessageHeader.Length;
            bool endOfText;
            msg.Method = ParseItem(text, out endOfText, ref index, MethodKeyDelimiter, MessageEndDelimiter);
            if (!endOfText)
            {
                msg.Key = ParseItem(text, out endOfText, ref index, KeyValueDelimiter, MessageEndDelimiter);
                if (!endOfText)
                {
                    msg.Value = ParseItem(text, out endOfText, ref index, MessageEndDelimiter, MessageEndDelimiter);
                }
            }

            if (string.IsNullOrEmpty(msg.Method) || string.IsNullOrEmpty(msg.Key))
            {
                Log.Error("Invalid ROC message from {0}: {1}", _name, text);
                return null;
            }

            return msg;
        }

        /// <summary>
        /// parse the text and get one of the message items - method, key, value
        /// returns false when reached msgEndChar or end of the text
        /// </summary>
        protected string ParseItem(string text, out bool endOfText, ref int index, char itemEndChar, char msgEndChar)
        {
            string item = string.Empty;
            endOfText = false;
            for (; index < text.Length; index++)
            {
                char val = text[index];
                if (val == itemEndChar || val == msgEndChar)
                {
                    index++;
                    endOfText = val == msgEndChar;
                    break;
                }
                item += val;
            }
            return item;
        }

        protected string _name;
    }
}
