using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingLobby : NetworkBehaviour
{
    [Header("Settings")]
    public string nextSceneName = "04.GamePlay";
    public int countdownSeconds = 15;

  
    private UI_Lobby uiLobby;

    private NetworkVariable<int> readyPlayers = new NetworkVariable<int>(0);
    private AsyncOperation asyncLoad;
    private bool isLoading = false;


    private void Start() {
        uiLobby = FindObjectOfType<UI_Lobby>();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

    }

    public override void OnNetworkSpawn() {
        if (IsServer) {
            StartCoroutine(LoadSceneCAsync());
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoaded;
        }
    }
    private IEnumerator LoadSceneCAsync() {
        isLoading = true;

        // 게임 플레이 씬 비동기 로드 (모든 클라이언트에게 자동 동기화)
        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // 최소 90% 로딩 대기
        while (!asyncLoad.isDone && asyncLoad.progress < 0.9f) {
            yield return null;
        }

        // 서버에서 씬 활성화 허용
        asyncLoad.allowSceneActivation = true;
        yield return asyncLoad;

        // 모든 클라이언트의 로딩 완료 대기
        yield return new WaitUntil(() => readyPlayers.Value >= NetworkManager.Singleton.ConnectedClients.Count);

        StartCountdownClientRpc();
        yield return new WaitForSeconds(countdownSeconds);

        // 최종 씬 전환
        NetworkManager.Singleton.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    [ClientRpc]
    private void StartCountdownClientRpc() {
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine() {
        for (int i = countdownSeconds; i > 0; i--) {
            if (uiLobby != null)
                uiLobby.SetCountdown(i);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode) {
        if (sceneName == nextSceneName) {
            readyPlayers.Value++;
            UpdatePlayerReadyStatusClientRpc(readyPlayers.Value, NetworkManager.Singleton.ConnectedClients.Count);
        }
    }

    [ClientRpc]
    private void UpdatePlayerReadyStatusClientRpc(int ready, int total) {
        if (uiLobby != null)
            uiLobby.SetPlayerReadyStatus(ready, total);
    }

    private void OnClientConnected(ulong clientId) {
        if (IsServer && isLoading) {
            NetworkManager.Singleton.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
        }
    }

    public override void OnDestroy() {
        if (NetworkManager.Singleton != null) {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }


}
