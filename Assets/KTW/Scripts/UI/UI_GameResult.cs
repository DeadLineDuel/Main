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

    [SerializeField] private GameObject winTextObject;
    [SerializeField] private GameObject loseTextObject;

    private void Start() {
        toLobbyButton.onClick.AddListener(() => OnClickToLobbyButton());
        viewStatsButton.onClick.AddListener(() => OnClickViewStatsButton());

        // TODO TEST
        SetWinTextSetActive(isWin:true);
    }



    private void OnClickToLobbyButton() {
        Debug.Log("�κ� ��ưŬ��");
    }

    private void OnClickViewStatsButton() {
        Debug.Log("���� ��ưŬ��");
    }

    public void SetWinTextSetActive(bool isWin) {
        winTextObject.SetActive(isWin);
        loseTextObject.SetActive(!isWin);
    }
}
