//#include "LowPower.h"
#include "module.h"
#include "onboardLedModule.h"
#include "rocMessageModule.h"
#include "rocIOHandler.h"

// global definitions/constants
#define LoopDelay 100  // this is the delay for each loop
#define AlwaysAck false      // if true send ack back for every valid console ROC message

// decalre modules that will be used
OnboardLedModule ledmodule;
RocMessageModule rocmodule(AlwaysAck);
RocIOHandler rocIOHandler;

void setup() {
  Serial.begin(9600); // opens serial port, sets data rate to 57600 baud
  Module::s_loopCount = 0;
  Module::s_loopCycle = LoopDelay;
  // initialize modules
  Module::initializeModules();
}

void loop() {
  Module::s_loopCount++;
  delay(60);
  
  // run all the modules
  Module::runModules();
  delay(LoopDelay - 60);  // remove the idle/sleep time and overhead
}
