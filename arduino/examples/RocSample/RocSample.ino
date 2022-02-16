//#include "LowPower.h"
#include "module.h"
#include "onboardLedModule.h"
#include "rocMessageModule.h"
#include "rocHandler.h"
#include "rocIOHandler.h"

// global definitions/constants
#define LoopDelay 100  // this is the delay for each loop
#define AlwaysAck false      // if true send ack back for every valid console ROC message

// decalre modules that will be used
RocMessageModule rocmodule(AlwaysAck);
RocIOHandler rocIOHandler;

void setup() {
  Serial.begin(9600); // opens serial port, sets data rate to 57600 baud
  Module::s_loopCount = 0;
  Module::s_loopCycle = LoopDelay;
  RocHandler::s_verbose = false;
  // initialize modules
  Module::initializeModules();
}

void loop() {
  Module::s_loopCount++;
  // Serial.println(Module::s_loopCount);
 
  // run all the modules
  Module::runModules();
  delay(LoopDelay);  // remove the idle/sleep time and overhead
}
