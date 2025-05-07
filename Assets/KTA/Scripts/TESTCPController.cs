using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace KTA.Test
{
    public class TESTCPController : NetworkBehaviour
    {
        public NetworkVariable<int> CP = new NetworkVariable<int>(0, writePerm: NetworkVariableWritePermission.Server);
    }
}
