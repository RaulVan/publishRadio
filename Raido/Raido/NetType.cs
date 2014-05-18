using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raido
{
    public class NetType
    {

        enum netType
        {
            _3G = 0,
            _2G,
            _WIFI,
            _NoNet
        }

        public netType GetNetType()
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
                return netType._NoNet;
            else
            {
                var info = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType;
                switch (info)
                {
                    case NetworkInterfaceType.MobileBroadbandCdma:
                        return netType._3G;
                    case NetworkInterfaceType.MobileBroadbandGsm:
                        return netType._2G;
                    case NetworkInterfaceType.Wireless80211:
                        return netType._WIFI;
                    case NetworkInterfaceType.Ethernet:
                        return "Ethernet";
                    case NetworkInterfaceType.None:
                        return "None";
                    default:
                        return "Other";
                } 
            }
        }

    }
}
