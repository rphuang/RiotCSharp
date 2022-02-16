/*
onboardLedModule.h - module to send heart beat (blink) on board LED.
Created by RPHuang, Jan 14, 2019.
Released into the public domain.
*/

#ifndef onboardLedModule_h
#define onboardLedModule_h

#include "Arduino.h"
#include "module.h"

// onboardLedModule implements visual herat beat by blinking on board LED
class OnboardLedModule : public Module
{
public:
    // constructor
    OnboardLedModule();
    // get the name of the module
    virtual String name();
    // initialize the onboardLedModule during setup by the main
    virtual void initialize();
    // run the onboardLedModule in the main loop()
    virtual void runModule();
private:
    bool onboardLedState = true;
};

#endif // !onboardLedModule_h
