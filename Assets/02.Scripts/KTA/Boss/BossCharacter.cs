using Unity.Netcode;
using UnityEngine;

namespace Boss
{
    public class BossCharacter : NetworkBehaviour
    {
        [field: SerializeField] public NetworkVariable<ulong> AssignedPlayerId = new();
        [field: SerializeField] private Transform targetPlayer;
        [field: SerializeField] public TransparencyController transparencyController;
        
        public bool IsClientBoss => AssignedPlayerId.Value == NetworkManager.Singleton.LocalClientId;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Debug.Log($"[Boss] Boss {gameObject.name} AssignedPlayerId: {AssignedPlayerId.Value}, LocalClientId: {NetworkManager.Singleton.LocalClientId}");

            if (IsClient)
            {
                if (AssignedPlayerId.Value == NetworkManager.Singleton.LocalClientId)
                {
                    transparencyController.SetToOpaque();
                }
                else
                {
                    transparencyController.SetToTransparent();
                }
            }
        }
        
        public bool GetTargetPlayer(out Transform targetPlayer)
        {
            if (this.targetPlayer == null)
            {
                //this.targetPlayer = GamePlayManager.Instance.GetBoss(NetworkManager.Singleton.LocalClientId).transform;
                this.targetPlayer = TempGetTargetPlayer();
            }
            
            targetPlayer = this.targetPlayer;
            return targetPlayer != null;
        }
        
        public Vector3 GetTargetPosition()
        {
            return targetPlayer.position;
        }

        private Transform TempGetTargetPlayer()
        {
            foreach (NetworkObject netObj in FindObjectsOfType<NetworkObject>())
            {
                if (netObj.gameObject.CompareTag("Player"))
                {
                    return netObj.gameObject.transform;
                }
            }
            return null;
        }
        
    }
}