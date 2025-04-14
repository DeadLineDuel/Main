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

        Debug.Log("��Ī ��ư Ŭ��");
        UpdateMatchmakingUI(true);
        isMatchmaking = true;
        // TODO ��ġ����ŷ

    }

    private void OnClickCanclematchMakingtButton() {
        Debug.Log("��Ī ��� ��ư Ŭ��");
        UpdateMatchmakingUI(false);
        isMatchmaking = false;
        // TODO ��Ī ���
    }

    private void OnCharacterSelected(int index) {
        if (isMatchmaking) return;

        Debug.Log($"ĳ���� {index + 1} ����");

        selectedCharacterIndex = index;

        // Move Red Border
        if (selectedCharacterBorderImage != null) {
            selectedCharacterBorderImage.gameObject.SetActive(true);
            Vector3 buttonPosition = characterButtons[index].GetComponent<RectTransform>().position;
            selectedCharacterBorderImage.rectTransform.position = buttonPosition;
        }
    }

    // �ܺο��� �� ������ ��?
    public int GetSelectedCharacter() {
        return selectedCharacterIndex;
    }
}
