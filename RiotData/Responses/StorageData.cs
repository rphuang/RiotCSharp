namespace RiotData
{
    /// <summary>
    /// response for storage data
    /// </summary>
    public class DriveData
    {
        /// <summary>
        /// the name of the drive
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// the label of the drive
        /// </summary>
        public string VolumeLabel { get; set; }

        /// <summary>
        /// the type of the drive
        /// </summary>
        public string DriveType { get; set; }

        /// <summary>
        /// the format of the drive
        /// </summary>
        public string DriveFormat { get; set; }

        /// <summary>
        /// the total size of the drive in MBytes
        /// </summary>
        public float TotalSize { get; set; }

        /// <summary>
        /// the free space of the drive in MBytes
        /// </summary>
        public float FreeSpace { get; set; }
    }
}
