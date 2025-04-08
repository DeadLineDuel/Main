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
        Debug.Log("�κ� ��ưŬ��");
    }

    private void OnClickViewStatsButton() {
        Debug.Log("���� ��ưŬ��");
    }
}
