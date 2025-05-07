using Unity.Netcode;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode.Transports.UTP;


namespace KTA.Test
{
    public class ConnectServerTEST : MonoBehaviour
    {
        public string hostIp;
        public ushort port = 7777;
        public TMP_InputField inputField;
        private void Awake()
        {
            hostIp = GetLocalIPAddress();
            Debug.Log(hostIp);
        }
        
        public void ConnectedToServer()
        {
            string ip = inputField.text.Trim();
            var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            unityTransport.SetConnectionData(ip, port);
            NetworkManager.Singleton.StartClient();
        }

        public void StartHost()
        {
            var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            unityTransport.SetConnectionData("0.0.0.0", port); // 모든 IP에서 수신
            NetworkManager.Singleton.StartHost();
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new System.Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
