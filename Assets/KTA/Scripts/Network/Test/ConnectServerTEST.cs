using Unity.Netcode;
using UnityEngine;

namespace KTA.Test
{
    public class ConnectServerTEST : MonoBehaviour
    {
        public void ConnectedToServer()
        {
            NetworkManager.Singleton.StartClient();
        }

        public void StartHost()
        {
            NetworkManager.Singleton.StartHost();
        }
    }
}
