/*
RocMessageModule.h - module to receive and process ROC messages from Serial.
Created by RPHuang, Jan 15, 2019.
Released into the public domain.
*/

#ifndef rocMessageModule_h
#define rocMessageModule_h

#include <Arduino.h>
#include "module.h"
#include "rochandler.h"

// define limitations to minimize the memory consumption
#define MaxMessageSize 32          // the max size of any message in bytes 
#define RocMessageCycleTime 200    // the cycle (delay) time to process incoming message in milliseconds

// RocMessageModule is a Module that receives/parses ROC messages and invokes RocHandler to handle the message
class RocMessageModule : public Module
{
public:
    // constructor
    RocMessageModule(bool alwaysAck);
    // get the name of the module
    virtual String name();
    // initialize the RocMessageModule during setup by the main
    virtual void initialize();
    // run the RocMessageModule in the main loop()
    virtual void runModule();

private:
    // member variables
    char _bytes[MaxMessageSize+1];
    int _indx = 0;
    bool _msgStarted = false;
    char _lastByte = 0;
    bool _alwaysAck = false;
    // private method
    void getMessageFromSerial();
    boolean parseMsg(char* text, char* method, char* key, char* value);
};

#endif // !RocMessageModule_h
