/*
RocIOHandler.h - handles ROC request for Arduino I/O and system.
Created by RPHuang, Jan 15, 2019.
Released into the public domain.
*/

#ifndef rocIOHandler_h
#define rocIOHandler_h

#include <Arduino.h>
#include "rochandler.h"

// RocIOHandler is derived from RocHandler to process request for Arduino I/O and system functions
class RocIOHandler : public RocHandler
{
public:
	// name of the handler (used only for debug/diagnostic purpose)
	virtual String name();
	// concrete class to implement 
	// returns true if the handler finished processing the specified (method, key, value).
	// otherwise returns false so other handler can continue the processing
	virtual bool processMessage(String method, String key, String value);
};

#endif 
