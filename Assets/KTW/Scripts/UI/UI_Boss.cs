using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Boss : MonoBehaviour
{
    [Header("UI Assign")]
    [SerializeField] private Slider playerBossHPSlider;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Slider enemyBossHPSlider;
    [SerializeField] private TextMeshProUGUI enemyNameText;

    [Header("Setting")]
    [SerializeField] private float minSliderValue = 0.05f; // 슬라이더는 이 아래의 값이 되면 이상하게 표시됨

    public void InitializedUI() {
        // TODO
        playerNameText.text = "플레이어이름";
        enemyNameText.text = "적 이름";
    }

    private void Update() {
        // FOR TEST
        if (Input.GetKeyDown(KeyCode.A)) {
            UpdatePlayerBossHP(1f);
            UpdateEnemyBossHP(1f);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            UpdatePlayerBossHP(0.5f);
            UpdateEnemyBossHP(0.5f);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            UpdatePlayerBossHP(0.1f);
            UpdateEnemyBossHP(0.1f);
        }

    }

    /// <summary>
    /// 플레이어 보스 HP 업데이트
    /// </summary>
    /// <param name="hpValue">0~1사이의 값으로 입력</param>
    public void UpdatePlayerBossHP(float hpValue) {
        float safeValue = Mathf.Max(hpValue, minSliderValue);
        playerBossHPSlider.DOValue(safeValue, 0.3f).SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// 적 보스 HP 업데이트
    /// </summary>
    /// <param name="hpValue">0~1사이의 값으로 입력</param>
    public void UpdateEnemyBossHP(float hpValue) {
        float safeValue = Mathf.Max(hpValue, minSliderValue);
        enemyBossHPSlider.DOValue(safeValue, 0.3f).SetEase(Ease.OutQuad);
    }
}
