namespace RiotArduinoLib
{
    /// <summary>
    /// ArduinoMessage, AKA ROC (Rest On Cable), defines a message exchange between Arduino and host computer via USB
    /// The message can be snet to/from Arduino. It contains three parts:
    ///   * method - the verb of the message can be: g (Get), p (Post/Put), r (reply), a (Ack), e (Error)
    ///   * key - the key (path) to the targeted resources with optional property type for the resource. System function keys: _reset, _time, _tick
    ///   * value - the value of the resource property
    /// Examples:
    ///   * get Msg sent to Arduino: new ArduinoMessage("g", "a03")
    ///   * get msg sent from Arduino to another Arduino: new ArduinoMessage("g", "com3/gpio/a03")
    /// </summary>
    public class ArduinoMessage
    {
        // constants
        public const string GetMethodName = "g";
        public const string PostMethodName = "p";
        public const string AckMethodName = "a";
        public const string ReplyMethodName = "r";
        public const string ErrorMethodName = "e";
        public const string ValuePathName = "value";
        public const string ModePathName = "mode";

        /// <summary>
        /// construct an empty message
        /// </summary>
        public ArduinoMessage()
        {
        }

        /// <summary>
        /// constructor with method, key
        /// </summary>
        public ArduinoMessage(string method, string key)
        {
            Method = method;
            InitializeFromKey(key);
        }

        /// <summary>
        /// constructor with method, key, and value
        /// </summary>
        public ArduinoMessage(string method, string key, string value)
        {
            Method = method;
            Value = value;
            InitializeFromKey(key);
        }

        /// <summary>
        /// whether the ArduinoMessage is valid
        /// </summary>
        public bool IsValid { get; set; } = false;

        /// <summary>
        /// Method for the ArduinoMessage. Standard methods: g (Get), p (Post/Put), r (reply), a (Ack), e (Error)
        /// </summary>
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// Key for the ArduinoMessage. 
        /// The key is the full path for the resource that includes ResourcePath, ResourceId, and PropertyName
        /// The formation of a key: ResourcePath/ResourceId/PropertyName
        /// Arduino specific system function keys: _reset, _time, _tick
        /// </summary>
        public string Key { get { return _key; } set { InitializeFromKey(value); } }

        /// <summary>
        /// the path of the resource exclude the ResourceId and PropertyName
        /// </summary>
        public string ResourcePath { get; set; } = string.Empty;

        /// <summary>
        /// the Id of the resource
        /// </summary>
        public string ResourceId { get; set; } = string.Empty;

        /// <summary>
        /// The propertyName: currently valid values are defined in ValuePathName and ModePathName
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;

        /// <summary>
        /// Value for the target property
        /// </summary>
        public string Value { get; set; } = string.Empty;

        private void InitializeFromKey(string key)
        {
            _key = key;
            if (string.IsNullOrEmpty(key))
            {
                IsValid = false;
                return;   // not a valid key
            }

            // two types of key
            //  - relative path: path does not start with "/". examples: gpio/3/value, 3/mode, 3
            //  - absolute path: path start with "/". examples: /gpio/3/mode, /dev/COM3/gpio/3/value, /dev/COM3/gpio/3
            ResourcePath = string.Empty;

            IsValid = true;
            string[] parts = key.Split(PathDelimiter, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                ResourcePath = string.Empty;
                ResourceId = parts[0];
                PropertyName = ValuePathName;
            }
            else
            {
                int index = parts.Length - 2;
                // determine whether a property name is specified
                string lastPart = parts[parts.Length - 1];
                if (string.Equals(ValuePathName, lastPart, StringComparison.OrdinalIgnoreCase))
                {
                    PropertyName = ValuePathName;
                }
                else if (string.Equals(ModePathName, lastPart, StringComparison.OrdinalIgnoreCase))
                {
                    PropertyName = ModePathName;
                }
                else
                {
                    index = parts.Length - 1;
                    PropertyName = ValuePathName;
                }
                ResourceId = parts[index];
                ResourcePath = string.Empty;
                for (int jj = 0; jj < index; jj++)
                {
                    ResourcePath += PathDelimiter + parts[jj];
                }
            }
        }

        protected static readonly char[] PathDelimiter = { '/' };
        private string _key;
    }
}
