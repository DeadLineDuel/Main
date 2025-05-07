using System;
using System.Collections.Generic;
using Boss;
using Stats.Boss;
using Unity.Netcode;
using UnityEngine;


namespace KTA.Test
{
    public class TESTGamePlayManager : NetworkBehaviour
    {
        private static TESTGamePlayManager _instance;
        public static TESTGamePlayManager Instance => _instance;
        
        public Dictionary<ulong, NetworkObject> connectedPlayersId = new Dictionary<ulong, NetworkObject>();
        public Dictionary<ulong, BossCore> connectedBosses = new Dictionary<ulong, BossCore>();
        public Dictionary<ulong, ulong> connectedOpponents = new Dictionary<ulong, ulong>();
        
        [field: SerializeField] private GameObject bossPrefab;
        [field: SerializeField] private Transform bossSpawnPoint;

        [field: SerializeField] private NetworkVariable <float> serverStartTime = new NetworkVariable<float>(writePerm:NetworkVariableWritePermission.Server);
        [field: SerializeField] private NetworkVariable<float> gameTime = new NetworkVariable<float>(180f, writePerm:NetworkVariableWritePermission.Server);
        public float remainingTime;
        [field: SerializeField] private NetworkVariable<bool> isGameTimerRunning = new NetworkVariable<bool>(false, writePerm:NetworkVariableWritePermission.Server);
        
        public event Action OnBossWake;
        
        private void Awake()
        {
            // 싱글톤 패턴 구현
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            remainingTime = gameTime.Value;
        }

        private void Update()
        {
            if (isGameTimerRunning.Value && remainingTime > 0f)
            {
                remainingTime = (serverStartTime.Value + gameTime.Value) - NetworkManager.ServerTime.TimeAsFloat;
                if (remainingTime < 0f) remainingTime = 0f;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsServer) return;
            
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsServer) return;
            
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }

        private void OnClientConnected(ulong clientId)
        {
            if (IsServer)
            {
                connectedPlayersId[clientId] = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                SpawnBoss(clientId);
            
                // 클라이언트 2개가 연결되면
                if (NetworkManager.Singleton.ConnectedClients.Count == 2)
                {
                    var clientIds = NetworkManager.Singleton.ConnectedClientsIds;
                    ulong clientA = clientIds[0];
                    ulong clientB = clientIds[1];
                    connectedOpponents[clientA] = clientB;
                    connectedOpponents[clientB] = clientA;
                    
                    SyncOpponentsClientRpc(clientA, clientB);
                    SyncConnectedPlayerClientRpc(clientA, connectedPlayersId[clientA].NetworkObjectId);
                    SyncConnectedPlayerClientRpc(clientB, connectedPlayersId[clientB].NetworkObjectId);
                    SyncConnectedBossesClientRpc(clientA, connectedBosses[clientA].NetworkObjectId);
                    SyncConnectedBossesClientRpc(clientB, connectedBosses[clientB].NetworkObjectId);
                    SetUIClientRpc();
                }
            }
        }
        
        private void OnClientDisconnected(ulong clientId)
        {
            throw new System.NotImplementedException();
        }
        
        private void SpawnBoss(ulong clientId)
        {
            if (!IsServer) return;
    
            // 보스 스폰 지점에 보스 인스턴스화
            GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            NetworkObject bossNetObject = bossInstance.GetComponent<NetworkObject>();
        
            if (bossNetObject != null)
            {
                BossCore bossCore = bossInstance.GetComponent<BossCore>();
            
                connectedBosses.Add(clientId, bossCore);

                bossCore.BossCharacter.AssignedPlayerId.Value = clientId;
            
                bossNetObject.Spawn();

                SyncConnectedBossesClientRpc(clientId, bossNetObject.NetworkObjectId);
            
                // 보스 이벤트 설정
                BossStats bossStats = bossCore.BossStats;
                if (bossStats != null)
                {
                    // bossStats.CurrentHealth.OnValueChanged += UpdateBossHP;
                    bossStats.OnDeath += OnBossDefeated;
                }
            }
            else
            {
                Debug.LogError("보스 프리팹에 NetworkObject 컴포넌트가 없습니다");
                Destroy(bossInstance);
            }
        }

        [ClientRpc]
        private void SyncOpponentsClientRpc(ulong clientA, ulong clientB)
        {
            connectedOpponents[clientA] = clientB;
            connectedOpponents[clientB] = clientA;
        }

        [ClientRpc]
        private void SyncConnectedPlayerClientRpc(ulong clientId, ulong networkObjectId)
        {
            NetworkObject playerNetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
            if (playerNetObj != null)
            {
                connectedPlayersId[clientId] = playerNetObj;
            }
        }
    
        [ClientRpc]
        private void SyncConnectedBossesClientRpc(ulong clientId, ulong networkObjectId)
        {
            NetworkObject bossNetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
            if (bossNetObj != null)
            {
                BossCore bossCore = bossNetObj.GetComponent<BossCore>();
                if (bossCore != null)
                {
                    connectedBosses[clientId] = bossCore;
                }
            }
        }

        [ClientRpc]
        private void SetUIClientRpc()
        {
            TESTUIManager.Instance.Init();
        }
        
        private void OnBossDefeated()
        {
            throw new System.NotImplementedException();
        }

        public void OnBossWakeMessage()
        {
            if (!IsServer) return;
            OnBossWake?.Invoke();
            SetTimer();
        }

        private void SetTimer()
        {
            if (!IsServer) return;
            serverStartTime.Value = NetworkManager.ServerTime.TimeAsFloat;
            remainingTime = gameTime.Value;
            isGameTimerRunning.Value = true;
        }
    }
}