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
            _Cellular=0,
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
                    case NetworkInterfaceType.Wireless80211:
                        return netType._WIFI;
                    default:
                        return netType._Cellular;                    
                } 
            }
        }

    }
}
