using RiotData;
using System.Diagnostics;

namespace RiotService
{
    public class SystemService : ServiceBase
    {
        /// <summary>
        /// Process the Get requests for system
        /// </summary>
        public object Get(SystemDto request)
        {
            if (!Entry()) return ExitWithErrorResponse(400, "Bad Request");

            InitializePerfCounters();
            if (string.IsNullOrEmpty(request.Category) || string.Equals(request.Category, "info", StringComparison.OrdinalIgnoreCase))
            {
                SystemInfo info = GetSystemInfo();
                info.StatusCode = 200;
                Exit(200);
                return info;
            }
            if (string.Equals(request.Category, "cpu", StringComparison.OrdinalIgnoreCase))
            {
                ProcessorData data = GetProcessorData(request.Details);
                Exit(200);
                return data;
            }
            if (string.Equals(request.Category, "storage", StringComparison.OrdinalIgnoreCase))
            {
                var data = GetStorageData(request.Details);
                Exit(200);
                return data;
            }
            if (string.Equals(request.Category, "memory", StringComparison.OrdinalIgnoreCase))
            {
                MemoryData data = GetMemoryData();
                data.StatusCode = 200;
                Exit(200);
                return data;
            }
            return ExitWithErrorResponse(400, "Bad Request");
        }


        private SystemInfo GetSystemInfo()
        {
            var os = Environment.OSVersion;
            SystemInfo info = new SystemInfo
            {
                MachineName = Environment.MachineName,
                //OSPlatform = os.Platform,
                OSVersion = os.VersionString,
                ProcessorArchitecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"),
                ProcessorModel = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"),
                ProcessorLevel = Environment.GetEnvironmentVariable("PROCESSOR_LEVEL"),
                ProcessorRevision = Environment.GetEnvironmentVariable("PROCESSOR_REVISION"),
                ProcessorCount = Environment.ProcessorCount,
                DotNetVersion = Environment.Version.ToString(),
                LogicalDrives = String.Join(", ", Environment.GetLogicalDrives()),
            };
            return info;
        }

        private ProcessorData GetProcessorData(string coreName)
        {
            ProcessorData processorTotal = null;
            Dictionary<string, ProcessorData> data = new Dictionary<string, ProcessorData>();
            foreach (var core in processorCounters)
            {
                string instanceName = core.Key;
                if (string.IsNullOrEmpty(coreName) || string.Equals(instanceName, coreName, StringComparison.OrdinalIgnoreCase))
                {
                    ProcessorData processorData = new ProcessorData();
                    List<PerformanceCounter> counterList = core.Value;
                    for (int jj = 0; jj < counterList.Count; jj++)
                    {
                        PerformanceCounter counter = counterList[jj];
                        float value = counter.NextValue();
                        switch (jj)
                        {
                            case 0:
                                processorData.Usage = value;
                                break;
                            case 1:
                                processorData.Idle = value;
                                break;
                            case 2:
                                processorData.UserUsage = value;
                                break;
                            case 3:
                                processorData.SystemUsage = value;
                                break;
                        }
                    }
                    if (string.Equals("_Total", instanceName, StringComparison.OrdinalIgnoreCase))
                    {
                        processorTotal = processorData;
                        processorTotal.Cores = data;
                    }
                    else if (string.IsNullOrEmpty(coreName)) data.Add(instanceName, processorData);
                    else
                    {
                        processorTotal = processorData;
                        break;
                    }
                }
            }
            return processorTotal;
        }

        private void InitializePerfCounters()
        {
            if (countersInitialized) return;

            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Processor");
            string[] counterNames = { "% Processor Time", "% Idle Time", "% User Time", "% Privileged Time" };
            string[] names = performanceCounterCategory.GetInstanceNames();
            foreach (string instanceName in names)
            {
                List<PerformanceCounter> counterList = new List<PerformanceCounter>();
                for (int jj = 0; jj < counterNames.Length; jj++)
                {
                    string counterName = counterNames[jj];
                    PerformanceCounter counter = new PerformanceCounter("Processor", counterName, instanceName);
                    float value = counter.NextValue();
                    counterList.Add(counter);
                }
                processorCounters.Add(instanceName, counterList);
            }
            countersInitialized = true;
        }

        private object GetStorageData(string driveName)
        {
            List<DriveData> storageData = new List<DriveData>();
            foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
            {
                if (string.IsNullOrEmpty(driveName) || string.Equals(drive.Name, driveName, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        DriveData driveData = new DriveData()
                        {
                            Name = drive.Name,
                            VolumeLabel = drive.VolumeLabel,
                            DriveType = drive.DriveType.ToString(),
                            DriveFormat = drive.DriveFormat.ToString(),
                            TotalSize = drive.TotalSize / MBytes,
                            FreeSpace = drive.AvailableFreeSpace / MBytes,
                        };
                        if (!string.IsNullOrEmpty(driveName)) return driveData;
                        storageData.Add(driveData);
                    }
                    catch
                    {
                    }
                }
            }
            return storageData;
        }

        private MemoryData GetMemoryData()
        {
            var gcMemoryInfo = GC.GetGCMemoryInfo();
            long installedMemory = gcMemoryInfo.TotalAvailableMemoryBytes;
            long used = gcMemoryInfo.MemoryLoadBytes;
            MemoryData memoryData = new MemoryData()
            {
                Total = installedMemory / MBytes,
                Available = availableMemoryCounter.NextValue(),
                Used = used / MBytes,
                Cached = cacheMemoryCounter.NextValue() / MBytes,
                SystemUsed = systemMemoryCounter.NextValue() / MBytes,
                UsedPercent = 100 * used / installedMemory,
                Free = (installedMemory - used) / MBytes,
            };
            return memoryData;
        }

        const float MBytes = 1024 * 1024;
        static bool countersInitialized;
        static Dictionary<string, List<PerformanceCounter>> processorCounters = new Dictionary<string,List<PerformanceCounter>>();
        //static PerformanceCounter cpuTotalCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        static PerformanceCounter availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        static PerformanceCounter cacheMemoryCounter = new PerformanceCounter("Memory", "Cache Bytes");
        static PerformanceCounter systemMemoryCounter = new PerformanceCounter("Memory", "System Code Total Bytes");
        //static PerformanceCounter totalMemoryCounter = new PerformanceCounter("Mono Memory", "Total Physical Memory");
    }
}
