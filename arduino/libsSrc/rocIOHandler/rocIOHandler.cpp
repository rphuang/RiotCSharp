/*
RocIOHandler.cpp - handles ROC request for Arduino I/O and system.
Created by RPHuang, Jan 15, 2019.
Released into the public domain.
*/

#include "rocIOHandler.h"

// name of the handler (used only for debug/diagnostic purpose)
String RocIOHandler::name()
{
  return F("RocIOHandler");
}

// Restarts program from beginning but does not reset the peripherals and registers
void softwareReset()
{
  asm volatile ("  jmp 0");
}

// reply ROC message with the clock ticks
bool replyTicks()
{
    Serial.println(String(F("##r?_tick=")) + String(millis()) + "^");
    return true;
}

// set the system date time
bool setSystemTime(String value)
{
    // todo
    return false;
}

// parse pin number from key
int getPinNumberFromKey(String key, int startIndex)
{
    String pinText = key.substring(startIndex);
    return pinText.toInt();
}

// reply ROC get message with specified IO value
bool replyIOValue(bool isDigital, int pin, String key)
{
    int ioValue;
    if (isDigital) ioValue = digitalRead(pin);
    else ioValue = analogRead(pin);
    Serial.println(String(F("##r?")) + key + String(F("=")) + String(ioValue) + "^");
    return true;
}

// process IO related ROC message
bool processIO(bool isRead, String key, String value)
{
    //Serial.println(String(F("processIO isRead: ")) + String(isRead) + String(F(" key: ")) + key + String(F(" value: ")) + value);
    int intValue = value.toInt();
    int pin = 0;
    bool isDigital = true;
    bool isMode = false;
    char ioKey = key[0];
    // possible keys for pin: 3, d3, d03, a3, m3
    if (isDigit(ioKey))
    {
        pin = getPinNumberFromKey(key, 0);
    }
    else
    {
        pin = getPinNumberFromKey(key, 1);
        switch (ioKey)
        {
        case 'a': // analog signal
            isDigital = false;
            break;
        case 'd': // digital signal
            break;
        case 'm':
            // todo: read pin mode https://arduino.stackexchange.com/questions/13165/how-to-read-pinmode-for-digital-pin
            // for now only post id supported: INPUT = 0, OUTPUT = 1, INPUT_PULLUP = 2
            if (isRead) return false;
            pinMode(pin, intValue);
            return true;
            break;
        default:
            return false;
            break;
        }
    }
    if (pin > 0)
    {
        //    Serial.println(
        //        String(F("processIO isRead: ")) + String(isRead) + 
        //        String(F(" isDigita: ")) + String(isDigital) + 
        //        String(F(" pin: ")) + String(pin));
        if (isRead) replyIOValue(isDigital, pin, key);
        else
        {
            if (isDigital)
            {
                pinMode(pin, OUTPUT);
                digitalWrite(pin, intValue);
            }
            else analogWrite(pin, intValue);
        }
        return true;
    }
    return false;
}

// concrete implementation to process ROC request
bool RocIOHandler::processMessage(String method, String key, String value)
{
    if (s_verbose) Serial.println(String(F("RocIOHandler: processing method: ")) + method + String(F(" key: ")) + key + String(F(" value: ")) + value);

    // determine whether this is read or write
    bool isRead;
    if (method[0] == 'g') isRead = true;
    else if (method[0] == 'p') isRead = false;
    else return false;

    int pin;
    // check whether this is one of the system command
    if (key == F("_reset"))  // reset
    {
        // note that current implementation ignores the method
        int delayValue = value.toInt();
        delay(delayValue);
        softwareReset();
    }
    else if (key == F("_time"))  // time
    {
        if (isRead) return replyTicks();
        else return setSystemTime(value);
    }
    else if (key == F("_tick"))  // tick
    {
        // note that current implementation ignores the method
        return replyTicks();
    }
    else
    {
        return processIO(isRead, key, value);
    }
    return false;
}
