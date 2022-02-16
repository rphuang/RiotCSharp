/*
module.h - defines Roc Module base class.
Created by RPHuang, Jan 13, 2019.
Released into the public domain.
*/

#ifndef module_h
#define module_h

#include "Arduino.h"

#define MaxModules 10        // number of modules to support


// Module is the base class for all the modules
// It defines the interfaces for all the concrete modules
// The static members allow auto register module instances and, for each cycle in main loop, invokes each module to run when the module is due to run.
class Module
{
public:
    // get the name of the module
    virtual String name();
    // initialize the module during setup by the main
    virtual void initialize() = 0;
    // run the module in the main loop()
    virtual void run();

    // static variables for clocking and whether to run the module
    static int s_loopCycle;     // defines the time for each loop cycle in Milliseconds
    static unsigned long s_loopCount;                  // the current loop count
    static bool s_verbose;        // verbose mode for the modules
    // static methods to initialize /run all registered modules
    static void initializeModules();
    static void runModules();

protected:
    // constructor
    Module();
    // module to implement during run
    virtual void runModule() = 0;
    // set the delay time for next run
    void setDelay(int milliseconds);

    // member variables
    // used to control when to run next time
    long _nextRunCount;
private:
    static int s_moduleCount;
    static Module* s_modules[MaxModules];
};

#endif // !module_h
