using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace KTA.Test
{
    public class TESTBossHPUI : MonoBehaviour
    {
        [Header("UI Assign")]
        [SerializeField] private Slider playerBossHPSlider;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private Slider enemyBossHPSlider;
        [SerializeField] private TextMeshProUGUI enemyNameText;
        
        [Header("Setting")]
        [SerializeField] private float minSliderValue = 0.05f; // �����̴��� �� �Ʒ��� ���� �Ǹ� �̻��ϰ� ǥ�õ�

        
        public void InitializedUI(string player1, string player2) {
            UpdatePlayerBossHP(1.0f);
            UpdateEnemyBossHP(1.0f);
            SetPlayerName(player1, player2);

        }

        public void UpdatePlayerBossHP(float hp) {
            ulong clientId = NetworkManager.Singleton.LocalClientId;
            float hpPercentage = hp / TESTGamePlayManager.Instance.connectedBosses[clientId].BossStats.MaxHealth.Value;
            float safeValue = Mathf.Max(hpPercentage, minSliderValue);
            playerBossHPSlider.DOValue(safeValue, 0.3f).SetEase(Ease.OutQuad);
        }

        public void UpdateEnemyBossHP(float hp) {
            ulong clientId = NetworkManager.Singleton.LocalClientId;
            ulong opponentId = TESTGamePlayManager.Instance.connectedOpponents[clientId];
            float hpPercentage = hp / TESTGamePlayManager.Instance.connectedBosses[opponentId].BossStats.MaxHealth.Value;
            float safeValue = Mathf.Max(hpPercentage, minSliderValue);
            enemyBossHPSlider.DOValue(safeValue, 0.3f).SetEase(Ease.OutQuad);
        }
        
        private void SetPlayerName(string player1, string player2) {
            playerNameText.text = $"{player1}'s Boss";
            enemyNameText.text = $"{player2}'s Boss";
        }
    }
}
