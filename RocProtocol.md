# ROC (REST on Cable)
ROC is a peer-to-peer request protocol that allows different machines and platforms to exchange data by request. ROC is especially designed for communicating and controlling IO devices such as Arduino and Raspberry PI.

# Goals and Principles
* Simplicity
* Flexibility
* Extensibility
* Bidirectional or peer-to-peer communication. That means any party can act as client (sending request) and server (responding request).

# Protocol Format
The basic format for communication message is:
```
  ##<method>?<key>=<value>^
```
* ```##``` - defines the beginning of the message
* method – defines one of the methods
  *	g – get method. This is to request (or read) data/information from the receiver. It is similar to the get method in http.
  *	P – post/put method. This is to post (or write) data/information to the receiver. It is similar to the post method in http.
  *	a – acknowledge the request sent from the other side. 
  * r - reply a get request
* ? – delimiter between method and key
* key – defines the key for the target resource
* = - delimiter between key and value. Optional for get method.
* value – defines the value/payload for the message. Optional for get method.
* ^ - end of message

# Methods
The protocol supports both request and response. For request, there are two methods (verbs) – get and post. For response, the receiver can respond with one of the followings ack, error, or reply.
## Get method
The get method is used to request data/information from the receiver. In general, it should not change the state of the receiver. Examples:
* Get the digital/analog data from Arduino
* Custom read to get information. This requires custom code to handle the action.

## Post method
The post method is intended for posting data or sending command to the receiver. In general, this may change the state of the receiver. Examples:
* Set the digital/analog data for Arduino
* Custom command for complex operation. This requires custom code to handle the command.

## Reply method
This is used to reply the result of get request.

## Acknowledge Method
This is simply to acknowledge of the message received. It should contain the same string as the original message except the method name changed.

## Error Method
Send an error message back.

# Keys
The key in the message is a unique ID. Similar to the REST protocol, the key is used to determine the path to the target resource and intention. For example, assume an Arduino is connected to /dev/ttyUSB1, then the following path is to get the LED value from that Arduino. Please see next section on how the IO pins are defined for Arduino.
```
 ##g?/dev/ttyUSB1/gpio/13/value^
```
In order to minimize the length of the request. The followings are supported by the protocol.
* The default property is value. So, the last portion “/value” is optional.
* Gpio is the default resource for Arduino (and Pi). So, the “/gpio” is also optional.

By applying the above defaults, the request can be just:
```
 ##g?/dev/ttyUSB1/13^
```
The following request can be used to get the value for pin 16 in the Pi when it is sent directly to the Pi.
```
 ##g?16^
```

# Post a Command
The key can be used to support an invocation of an operation on the entity when sending a Post. For example, the following is to invoke DoSomthing on the Item1 entity.
```
 ##p?/resources/Item1/DoSomthing^
```
# Keys for Built-in Arduino Support
This section defines the built-in keys – both commands and I/O keys.

## Arduino I/O Keys
The protocol defines a list of special keys for all the standard Arduino I/O based on the pin numbers. Note digital pin can be simplified to the pin number.
* Digital value read write – 2, d3, d04, …
* Analog value read write - a1, a02, …
* Digital pin mode - m9, f10, …

Examples:
* Read digital pin 13: ##g?d13^
* Write digital pin 13: ##p?d13=1^
* Write PWM pin 10: ##p?a10=255^
* Read analog pin 3: ##g?a3^
* Read digital mode for pin 9: ##g?m9^

## Arduino System Commands
### Reset
This is to reset/restart the Arduino device.
```
 ##p?_reset^
```
### Get System Time
This is to get the system time. This is intended for Arduino to request time from computer. For Arduino, it will respond with _tick.
```
 ##g?_time^
```
### Post System Time
This is to set the system time in Arduino.
```
 ##p?_time=0^
```
### Get Clock Tick
This is to get low level clock ticks or milliseconds in Arduino.
```
 ##g?_tick^
```

