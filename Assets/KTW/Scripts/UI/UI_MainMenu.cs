using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour 
{
    [Header("UI Assign")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;
    // [SerializeField] private float loadingValue = 0.0f;  // Value�� UI���� �������� ��������? �ϴ�

    private void Start() {
        startButton.onClick.AddListener(() => OnClickGameStartButton());
        settingButton.onClick.AddListener(() => OnClickSettingButton());
        SetLoadingProgress(0);
    }


    private void OnClickGameStartButton() {
        Debug.Log("���� ��ư Ŭ��");
    }

    private void OnClickSettingButton() {
        Debug.Log("���� ��ư Ŭ��");
    }

    /// <summary>
    /// �ܺο��� �ε� �� ����
    /// </summary>
    public void SetLoadingProgress(float value) {
        if (loadingSlider == null || loadingText == null) {
            Debug.LogError("UI_MainMenu | SetLoadingProgress | UI is not assigned");
        }
        value = Mathf.Clamp01(value);
        loadingSlider.value = value;
        loadingText.text = $"{Mathf.RoundToInt(value * 100)}%";
    }
}
