/*
onboardLedModule.cpp - module to send heart beat (blink) on board LED.
Created by RPHuang, Jan 14, 2019.
Released into the public domain.
*/

#include "onboardLedModule.h"

#define ledOnTime 100  // time for led on
#define ledOffTime 2900  // time for led off

// constructor
OnboardLedModule::OnboardLedModule()
{
}

// get the name of the module
String OnboardLedModule::name()
{
    return F("OnboardLedModule");
}

// initialize the onboardLedModule during setup by the main
void OnboardLedModule::initialize()
{
    // initialize digital pin LED_BUILTIN as an output.
    pinMode(LED_BUILTIN, OUTPUT);
    digitalWrite(LED_BUILTIN, HIGH);   // turn the LED on
    onboardLedState = true;
}

// run the onboardLedModule in the main loop()
void OnboardLedModule::runModule()
{
    onboardLedState = !onboardLedState;
    digitalWrite(LED_BUILTIN, onboardLedState);   // turn the LED on/off
    if (onboardLedState) setDelay(ledOnTime);
    else setDelay(ledOffTime);
}
