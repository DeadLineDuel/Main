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
    [SerializeField] private AudioClip matchmakingSound;    // TODO ���� �߰�
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

        Debug.Log("��Ī ��ư Ŭ��");
        UpdateMatchmakingUI(true);
        isMatchmaking = true;

        // PlayMatchmakingSound(matchmakingSound);
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

        // ���� �׵θ� �ű�� (���� ȿ��)
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

    private void PlayMatchmakingSound(AudioClip clip) {
        if (audioSource == null) return;
        if (clip == null) return;

        audioSource.PlayOneShot(clip);
    }
}
