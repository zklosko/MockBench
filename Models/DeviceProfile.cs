using System;
using System.Collections.Generic;
using System.Text;

namespace MockBench.Models
{
    internal class DeviceProfile
    {
        public string DeviceName { get; set; } = "";
        public string DeviceIp { get; set; } = "";
        public int DevicePort { get; set; } = 1234;

        public DeviceProfile(string deviceName, string deviceIp, int devicePort)
        {
            DeviceName = deviceName;
            DeviceIp = deviceIp;
            DevicePort = devicePort;
        }
    }
}
