#include "module.h"

// static variables definition
// defines the time for each loop cycle in Milliseconds
int Module::s_loopCycle = 100;

// the current loop count
unsigned long Module::s_loopCount = 0;

// verbose mode for the modules
bool Module::s_verbose = false;

// modules for registered concrete Module
int Module::s_moduleCount = 0;
Module* Module::s_modules[MaxModules];

// constructor
Module::Module()
{
    if (s_moduleCount < MaxModules)
    {
        s_modules[s_moduleCount] = this;
        //Serial.println(F("add module: ") + String(this->name()) + F(" index: ") + String(s_moduleCount));
        s_moduleCount++;
    }
    _nextRunCount = 0;
}

// get the name of the module
String Module::name()
{
    return F("Module");
}

// run the module in the main loop()
void Module::run()
{
    // check whether it is time to run
    if (_nextRunCount > 0 && s_loopCount < _nextRunCount) return;
    // time to run the module
    runModule();
}

// set the next run time
void Module::setDelay(int milliseconds)
{
    _nextRunCount = s_loopCount + milliseconds / s_loopCycle;
}

// static method to initialized all registered modules
void Module::initializeModules()
{
    for (int jj = 0; jj < s_moduleCount; jj++)
    {
        //Serial.println(F("initialize module ") + s_modules[jj]->name());
        s_modules[jj]->initialize();
    }
}

// static method to run all registered modules
void Module::runModules()
{
    for (int jj = 0; jj < s_moduleCount; jj++)
    {
        s_modules[jj]->run();
    }
}
