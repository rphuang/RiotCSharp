# RiotCSharp
RIOT (REST IOT) is intended to use a simple protocol to build an extensible and flexible system that can communicate among heterogeneous environments and devices.
RiotCSharp is a repository for C# projects that support RIOT protocol. It is backward compatible with the Riot protocol implemented in https://github.com/rphuang/riot.
The initial C# version supports the following features.
* Control Arduino I/O using a simplified REST protocol - ROC (Rest on Cable)
* Windows web service that supports
    * Windows system information, CPU perf data, memory usage, and storage data
    * Bridge service to other RIOT services (such as Pi servers from https://github.com/rphuang/riot)
    * I/O control to Arduino connected via USB

# Getting Started
1. download/clone the respository. These steps assume that the code is under \src\RiotCSharp.
2. download/clone the https://github.com/rphuang/LibsCSharp and save under \src\LibsCSharp.
3. deploy Arduino code
    1. From Windows Explorer, Send to->Compressed (zipped) folder, for the four folders in \src\RiotCSharp\arduino\libsSrc.
    2. with Arduino IDE, Sketch->Include Library->Add ZIP Libraries to add those four .zip files.
    3. open the file \src\RiotCSharp\arduino\examples\RocSample\RocSample.ino
    4. deploy to Arduino
4. Edit app.config file \src\RiotCSharp\RiotServiceWin\App.config for the serial port in "Address:com6"
5. build the solution with Visual Studio
6. run RiotServiceWin
7. from web browser, goto http://localhost:8000/arduino/13
8. fron httpie, enter command: http localhost:8000/arduino/13 value=1

# ROC (Rest on Cable)
ROC is a simplified RIOT protocol that supports comminucation with Arduino using USB serial port. See RocProtocol.md for details.

# Issues, Notes, and ToDos
* support Raspberry Pi and move Windows specific code out of RiotServiceCore.
* support bi-direction request between host and Arduino  
* add RiotProtocol.md for more details.
