using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : UI_Base 
{
    public Button startButton;
    public Button settingButton;

    private void Start() {
        BindEvent(startButton, OnClickGameStartButton);
        BindEvent(settingButton, OnClickSettingButton);
    }



    public void OnClickGameStartButton() {
        Debug.Log("로비 버튼클릭");
    }

    public void OnClickSettingButton() {
        Debug.Log("스탯 버튼클릭");
    }
}
