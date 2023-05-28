using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Acorisoft.Miga.Doc.Networks
{
    public static class InternetHelper
    {
        /// <summary>
        /// 获得本地可用的IP
        /// </summary>
        /// <param name="addresses"></param>
        public static void GetAvailableIPAddress(out IPAddress[] addresses)
        {
            var array = from networkInterface 
                        in NetworkInterface.GetAllNetworkInterfaces() 
                        select networkInterface.GetIPProperties() 
                        into prop
                        where prop.GatewayAddresses.Count > 0 
                        select prop.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork) 
                        into address 
                        where address is not null select address.Address;
            addresses = array.ToArray();
        }
    }
}