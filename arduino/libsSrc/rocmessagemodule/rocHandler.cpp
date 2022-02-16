/*
RocHandler.cpp - defines the handler and interface to handle ROC messages.
Created by RPHuang, Jan 15, 2019.
Released into the public domain.
*/

#include "rocHandler.h"

// static variables for the handlers
int RocHandler::s_handlerCount = 0;
RocHandler* RocHandler::s_handlers[MaxRocHandlers];
// public static variable to enable verbose logging
bool RocHandler::s_verbose = false;


// constructor
RocHandler::RocHandler()
{
    if (s_handlerCount < MaxRocHandlers)
    {
        s_handlers[s_handlerCount] = this;
        s_handlerCount++;
    }
}

// public static method to process the ROC message
// by finding the first handler that handles the message
// returns true if the message was successfully processed
bool RocHandler::processRocMessage(String method, String key, String value)
{
    if (s_verbose) Serial.println(String(F("Starting processRocMessage")));
    for (int jj = 0; jj < s_handlerCount; jj++)
    {
        if (s_verbose) Serial.println(String(F("Trying to process by handler ")) + s_handlers[jj]->name());
        if (s_handlers[jj]->processMessage(method, key, value))
        {
            if (s_verbose) Serial.println(String(F("Message successfully processed by handler ")) + s_handlers[jj]->name());
            break;
        }
    }
}
