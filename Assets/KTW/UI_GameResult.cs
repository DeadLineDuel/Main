using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameResult : UI_Base
{
    public Button toLobbyButton;
    public Button viewStatsButton;

    private void Start() {
        BindEvent(toLobbyButton, OnClickToLobbyButton);
        BindEvent(viewStatsButton, OnClickViewStatsButton);
    }

    

    public void OnClickToLobbyButton() {
        Debug.Log("�κ� ��ưŬ��");
    }

    public void OnClickViewStatsButton() {
        Debug.Log("���� ��ưŬ��");
    }
}
