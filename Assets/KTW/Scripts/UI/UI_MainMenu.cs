using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour 
{
    public Button startButton;
    public Button settingButton;

    private void Start() {
        startButton.onClick.AddListener(() => OnClickGameStartButton());
        settingButton.onClick.AddListener(() => OnClickSettingButton());
    }



    private void OnClickGameStartButton() {
        Debug.Log("시작 버튼 클릭");
    }

    private void OnClickSettingButton() {
        Debug.Log("세팅 버튼 클릭");
    }
}
