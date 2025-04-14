using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameResult : MonoBehaviour
{
    public Button toLobbyButton;
    public Button viewStatsButton;

    private void Start() {
        toLobbyButton.onClick.AddListener(() => OnClickToLobbyButton());
        viewStatsButton.onClick.AddListener(() => OnClickViewStatsButton());
    }



    private void OnClickToLobbyButton() {
        Debug.Log("로비 버튼클릭");
    }

    private void OnClickViewStatsButton() {
        Debug.Log("스탯 버튼클릭");
    }
}
