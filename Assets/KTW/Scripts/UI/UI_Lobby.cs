using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour {
    public List<Button> characterButtons;
    public Image selectedCharacterBorderImage;
    [SerializeField] private int selectedCharacterIndex = -1;
    private bool isMatchmaking = false;

    public Button matchmakingButton;
    public Button cancleMatchmakingButton;
    public TextMeshProUGUI matchingTextObject;

    private void Start() {
        matchmakingButton.onClick.AddListener(() => OnClickMatchmakingtButton());
        cancleMatchmakingButton.onClick.AddListener(() => OnClickCanclematchMakingtButton());

        for (int i = 0; i < characterButtons.Count; i++) {
            int index = i; // Value Capture
            characterButtons[i].onClick.AddListener(() => OnCharacterSelected(index));
        }
        selectedCharacterBorderImage.gameObject.SetActive(false);
    }

    private void UpdateMatchmakingUI(bool isMatching) {
        matchmakingButton.gameObject.SetActive(!isMatching);
        cancleMatchmakingButton.gameObject.SetActive(isMatching);
        matchingTextObject.gameObject.SetActive(isMatching);
    }

    private void OnClickMatchmakingtButton() {
        if (selectedCharacterIndex == -1) return;

        Debug.Log("매칭 버튼 클릭");
        UpdateMatchmakingUI(true);
        isMatchmaking = true;
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

        // Move Red Border
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
}
