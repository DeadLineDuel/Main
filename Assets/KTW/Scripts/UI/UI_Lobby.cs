using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour {
    [Header("UI Assign")]
    [SerializeField] private List<Button> characterButtons;
    [SerializeField] private Image selectedCharacterBorderImage;
    [SerializeField] private Button matchmakingButton;
    [SerializeField] private Button cancleMatchmakingButton;
    [SerializeField] private TextMeshProUGUI matchingText;

    [Header("Matchmaking")]
    [SerializeField] private int selectedCharacterIndex = -1;
    [SerializeField] private bool isMatchmaking = false;

    [Header("Sound")]
    [SerializeField] private AudioClip characterSelectSound;
    [SerializeField] private AudioClip matchmakingSound;    // TODO 사운드 추가
    private AudioSource audioSource;

    private void Start() {
        InitalizedButtons();
        selectedCharacterBorderImage.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void InitalizedButtons() {
        matchmakingButton.onClick.AddListener(() => OnClickMatchmakingtButton());
        cancleMatchmakingButton.onClick.AddListener(() => OnClickCanclematchMakingtButton());

        for (int i = 0; i < characterButtons.Count; i++) {
            int index = i; // Value Capture
            characterButtons[i].onClick.AddListener(() => OnCharacterSelected(index));
        }
    }

    private void UpdateMatchmakingUI(bool isMatching) {
        matchmakingButton.gameObject.SetActive(!isMatching);
        cancleMatchmakingButton.gameObject.SetActive(isMatching);
        matchingText.gameObject.SetActive(isMatching);
    }

    private void OnClickMatchmakingtButton() {
        if (selectedCharacterIndex == -1) return;

        Debug.Log("매칭 버튼 클릭");
        UpdateMatchmakingUI(true);
        isMatchmaking = true;

        // PlayMatchmakingSound(matchmakingSound);
        // TODO 매치메이킹

    }

    private void OnClickCanclematchMakingtButton() {
        Debug.Log("매칭 취소 버튼 클릭");
        UpdateMatchmakingUI(false);
        isMatchmaking = false;
        // TODO 매칭 취소
    }

    private void OnCharacterSelected(int index) {
        if (isMatchmaking) return;

        Debug.Log($"캐릭터 {index + 1} 선택");

        selectedCharacterIndex = index;

        // 빨간 테두리 옮기기 (선택 효과)
        if (selectedCharacterBorderImage != null) {
            selectedCharacterBorderImage.gameObject.SetActive(true);
            Vector3 buttonPosition = characterButtons[index].GetComponent<RectTransform>().position;
            selectedCharacterBorderImage.rectTransform.position = buttonPosition;
        }
    }

    // 외부에서 값 가져갈 때?
    public int GetSelectedCharacter() {
        return selectedCharacterIndex;
    }

    private void PlayMatchmakingSound(AudioClip clip) {
        if (audioSource == null) return;
        if (clip == null) return;

        audioSource.PlayOneShot(clip);
    }
}
