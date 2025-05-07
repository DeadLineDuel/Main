using System;
using KTA.Test;
using Unity.Netcode;
using UnityEngine;

public class TESTUIManager : MonoBehaviour
{
    private static TESTUIManager _instance;
    public static TESTUIManager Instance => _instance;

    [field: SerializeField] public TESTBossHPUI  _playerBossHPUI { get; private set; }
    [field: SerializeField] public TESTTimerUI _timerUI { get; private set; }
    [field: SerializeField] public UI_BuffDebuff _buffDebuffUI { get; private set; }
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
    }

    public void Init()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ulong opponentId = TESTGamePlayManager.Instance.connectedOpponents[clientId];
        
        TESTGamePlayManager.Instance.connectedBosses[clientId].BossStats.CurrentHealth.OnValueChanged += OnClientBossHealthChange;
        TESTGamePlayManager.Instance.connectedBosses[opponentId].BossStats.CurrentHealth.OnValueChanged += OnOpponentBossHPChange;

        _buffDebuffUI.Init();
    }

    private void OnClientBossHealthChange(float prev, float next)
    {
        _playerBossHPUI.UpdatePlayerBossHP(next);
    }

    private void OnOpponentBossHPChange(float prev, float next)
    {
        _playerBossHPUI.UpdateEnemyBossHP(next);
    }
}
