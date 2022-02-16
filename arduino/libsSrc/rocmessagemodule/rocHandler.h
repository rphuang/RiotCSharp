/*
RocHandler.h - defines the handler and interface to handle ROC messages.
Created by RPHuang, Jan 15, 2019.
Released into the public domain.
*/

#ifndef rocHandler_h
#define rocHandler_h

#include <Arduino.h>

// define limitations to minimize the memory consumption
#define MaxRocHandlers 5    // number of RocHandler supported

// RocHandler is the base class for ROC message handler.
// It defines the handler interface to process ROC messages.
// The static members allow auto register handler instances and, for each ROC message, loop through all handlers until one can process it.
class RocHandler
{
public:
    // name of the handler (used only for debug/diagnostic purpose)
    virtual String name() = 0;

    // public static method to process the ROC message
    // by finding the first handler that handles the message
    // returns true if the message was successfully processed
    static bool processRocMessage(String method, String key, String value);

    // public static variable to enable verbose logging
    static bool s_verbose;

protected:
    // constructor
    RocHandler();
    // concrete class to implement 
    // returns true if the handler finished processing the specified (method, key, value).
    // otherwise returns false so other handler can continue the processing
    virtual bool processMessage(String method, String key, String value) = 0;
private:
    static int s_handlerCount;
    static RocHandler* s_handlers[MaxRocHandlers];
};

#endif 
