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
    [SerializeField] private Button quitButton;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;
    // [SerializeField] private float loadingValue = 0.0f;  // Value를 UI에서 관리하지 않을지도? 일단

    private void Start() {
        startButton.onClick.AddListener(() => OnClickGameStartButton());
        settingButton.onClick.AddListener(() => OnClickSettingButton());
        quitButton.onClick.AddListener(() => OnClickQuitButton());
        SetLoadingProgress(0);
    }


    private void OnClickGameStartButton() {
        Debug.Log("UI_MainMenu | OnClickGameStartButton");
        GameMainManager.Instance.OnStartGameClicked(); // 다른 UI 접근은 매니저에서 처리

    }

    private void OnClickSettingButton() {
        Debug.Log("세팅 버튼 미구현");
    }

    private void OnClickQuitButton() {
        Debug.Log("UI_MainMenu | OnClickQuitButton");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    /// <summary>
    /// 외부에서 로딩 바 설정
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
