using System.Collections;
using System.Collections.Generic;
using Boss;
using Unity.Netcode;
using UnityEngine;

public class TestManager : NetworkBehaviour
{
    [field: SerializeField] private GameObject bossPrefab;
    private Dictionary<ulong, NetworkObject> playerBossMap = new();
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += AssignBossToPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }
    }

    private void AssignBossToPlayer(ulong clientId)
    {
        if (!IsServer) return;
        // 보스 생성 및 설정
        GameObject boss = Instantiate(bossPrefab);
        NetworkObject bossNetObj = boss.GetComponent<NetworkObject>();
        //bossNetObj.SpawnWithOwnership(clientId);
        bossNetObj.Spawn();
        BossCharacter bossCharacter = bossNetObj.GetComponent<BossCharacter>();
        bossCharacter.AssignedPlayerId.Value = clientId;
        
        playerBossMap[clientId] = bossNetObj;
        
        StartCoroutine(WakeBoss(bossNetObj.GetComponent<BossStateMachine>()));
        //StartCoroutine(KillBoss(bossNetObj.GetComponent<BossStateMachine>()));
    }

    private IEnumerator WakeBoss(BossStateMachine BossFSM)
    {
        Debug.Log(BossFSM.gameObject.name);
        yield return new WaitForSeconds(3f);
        BossFSM.OnWakeMessage();
    }

    private IEnumerator KillBoss(BossStateMachine BossFSM)
    {
        yield return new WaitForSeconds(10f);
        BossFSM.BossStats.KillBossTest();
    }
    
    private void OnClientDisconnect(ulong clientId)
    {
        if (playerBossMap.TryGetValue(clientId, out var boss))
        {
            boss.Despawn();
            playerBossMap.Remove(clientId);
        }
    }
}
