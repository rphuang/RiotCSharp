/*
RocMessageModule.cpp - module to receive and process ROC messages from Serial.
Created by RPHuang, Jan 15, 2019.
Released into the public domain.
*/

#include "rocMessageModule.h"

// constructor
RocMessageModule::RocMessageModule(bool alwaysAck)
{
  _alwaysAck = alwaysAck;
}

// get the name of the module
String RocMessageModule::name()
{
  return F("Module");
}

// initialize the RocMessageModule during setup by the main
void RocMessageModule::initialize()
{
}

// run the RocMessageModule in the main loop()
void RocMessageModule::runModule()
{
    getMessageFromSerial();
    setDelay(RocMessageCycleTime);
}

void RocMessageModule::getMessageFromSerial()
{
    char method[3];
    char key[MaxMessageSize];
    char value[MaxMessageSize];
    // todo: handle MaxMessageSize limit
    while (Serial.available() > 0)  // if any data available
    {
        char incomingByte = Serial.read(); // read byte
        if (!_msgStarted)
        {
            // detect the start of ROC message - "##"
            if (incomingByte == '#' && _lastByte == '#')
            {
                _msgStarted = true;
                _indx = 0;
            }
        }
        else
        {
            _bytes[_indx] = incomingByte;
            _indx += 1;
            //_bytes[_indx] = 0; Serial.println(_bytes); delay(20);
            if (incomingByte == '^')
            {
                _msgStarted = false;
                _bytes[_indx] = 0;
                //Serial.println(_bytes);
                if (parseMsg(_bytes, method, key, value))
                {
                    // process the message
                    RocHandler::processRocMessage(method, key, value);

                    if (_alwaysAck)    // send acknowledge back
                    {
                        int jj = 4;
                        strcpy(_bytes, "##a?");
                        strcpy(&_bytes[jj], key);
                        jj += strlen(key);
                        if (strlen(value) > 0)
                        {
                            _bytes[jj++] = '=';
                            strcpy(&_bytes[jj], value);
                            jj += strlen(value);
                        }
                        _bytes[jj++] = '^';
                        _bytes[jj++] = 0;
                        Serial.println(_bytes);
                    }
                }
                else
                {
                    Serial.print("Error: ");
                    Serial.println(_bytes);
                }
            }
            else if (_indx > MaxMessageSize)
            {
                Serial.print("Error: Too many bytes: ");
                Serial.println(_bytes);
                _msgStarted = false;
            }
        }
        _lastByte = incomingByte;
    }
}

// ROC message format
// ##<method>?<key>=<value>^
// example: ##p?d13=1^   // post d13 to 1
boolean RocMessageModule::parseMsg(char* text, char* method, char* key, char* value)
{
    // minimum requirements for a valid message
    //  at least 4 char - example: g?k^
    //  end with ^
    int msgLen = strlen(text);
    if (msgLen < 4 || text[msgLen - 1] != '^' || text[1] != '?') return false;

    method[0] = text[0];
    method[1] = 0;
    int jj = 2;
    key[0] = 0;
    for (; jj < strlen(text); jj++)
    {
        char val = text[jj];
        if (val == '=' || val == '^')
        {
            key[jj - 2] = 0;
            break;
        }
        key[jj - 2] = val;
    }
    int valIndx = 0;
    value[0] = 0;
    for (jj++; jj < strlen(text); jj++)
    {
        char val = text[jj];
        if (val == '^')
        {
            value[valIndx] = 0;
            break;
        }
        value[valIndx] = val;
        valIndx++;
    }
    return true;
}
