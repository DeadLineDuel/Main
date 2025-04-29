using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultManager : MonoBehaviour
{
    [Header("UI Assign")]
    [SerializeField] private UI_GameResult _uiGameResult;

    // 어딘가의 외부에서 한번에 관리하는게 좋을지도?
    [Header("Data Key from PlayerPrefs")]
    [SerializeField] private string _winnerKey = "WinnerPlayerId";
    [SerializeField] private string _player1IdKey = "Player1Id";
    [SerializeField] private string _player2IdKey = "Player2Id";
    [SerializeField] private string _player1TimeKey = "Player1Time";
    [SerializeField] private string _player2TimeKey = "Player2Time";
    [SerializeField] private string _player1BossHPKey = "Player1BossHP";
    [SerializeField] private string _player2BossHPKey = "Player2BossHP";
    [SerializeField] private string _player1DeathsKey = "Player1Deaths";
    [SerializeField] private string _player2DeathsKey = "Player2Deaths";

    private string _localPlayerId; // 현재 클라이언트의 플레이어 ID 
    
    private void Start() {
        InitializePlayerIdentity();
        // TODO 데이터 로드
        // LoadAndDisplayResults();

        _uiGameResult.UpdateResultInfoFromServer(
            "test", "test2",
            "player1ID", 30, 30, 30,
            "2dddd", 50, 50, 100
        );
    }

    private void InitializePlayerIdentity() {
        // TODO 이건 다른 방식으로 가져와야
        _localPlayerId = PlayerPrefs.GetString("LocalPlayerId", "Player"); // 기본값 1
    }

    private void LoadAndDisplayResults() {
        // 결과 데이터 로드
        string winnerId = PlayerPrefs.GetString(_winnerKey, "Player");

        string player1Id = PlayerPrefs.GetString(_player1IdKey, "Player 1");
        int player1Time = PlayerPrefs.GetInt(_player1TimeKey, 0);
        float player1BossHP = PlayerPrefs.GetFloat(_player1BossHPKey, 0f);
        int player1Deaths = PlayerPrefs.GetInt(_player1DeathsKey, 0);

        string player2Id = PlayerPrefs.GetString(_player2IdKey, "Player 2");
        int player2Time = PlayerPrefs.GetInt(_player2TimeKey, 0);
        float player2BossHP = PlayerPrefs.GetFloat(_player2BossHPKey, 0f);
        int player2Deaths = PlayerPrefs.GetInt(_player2DeathsKey, 0);

        bool isLocalWinner = (winnerId == _localPlayerId);

        _uiGameResult.UpdateResultInfoFromServer(
            _localPlayerId, winnerId,
            player1Id, player1Time, player1BossHP, player1Deaths,
            player2Id, player2Time, player2BossHP, player2Deaths
        );

        ClearSavedData();
    }

    // 저장했던 데이터 삭제
    private void ClearSavedData() {
        PlayerPrefs.DeleteKey(_winnerKey);
        PlayerPrefs.DeleteKey(_player1IdKey);
        PlayerPrefs.DeleteKey(_player2IdKey);
        PlayerPrefs.DeleteKey(_player1TimeKey);
        PlayerPrefs.DeleteKey(_player2TimeKey);
        PlayerPrefs.DeleteKey(_player1BossHPKey);
        PlayerPrefs.DeleteKey(_player2BossHPKey);
        PlayerPrefs.DeleteKey(_player1DeathsKey);
        PlayerPrefs.DeleteKey(_player2DeathsKey);
    }

    // TODO 이전 씬에서 아래와 같은 메서드 작성
    private void SaveGameResults(int winnerId) {
        PlayerPrefs.SetString("WinnerPlayerId", "Winner");

        PlayerPrefs.SetString("Player1Id", "Player 1");
        PlayerPrefs.SetInt("Player1Time", 350);
        PlayerPrefs.SetFloat("Player1BossHP", 35.5f);
        PlayerPrefs.SetInt("Player1Deaths", 2);

        PlayerPrefs.SetString("Player2Id", "Player 2");
        PlayerPrefs.SetInt("Player2Time", 280);
        PlayerPrefs.SetFloat("Player2BossHP", 80.2f);
        PlayerPrefs.SetInt("Player2Deaths", 4);

        PlayerPrefs.Save();
    }
}
