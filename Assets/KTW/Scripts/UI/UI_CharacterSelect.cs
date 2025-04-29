using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelect : MonoBehaviour {
    [Header("UI Assign")]
    [SerializeField] private List<Button> characterButtons;
    [SerializeField] private Image selectedCharacterBorderImage;
    [SerializeField] private Button matchmakingButton;
    [SerializeField] private Button cancleMatchmakingButton;
    [SerializeField] private Button backToMainButton;
    [SerializeField] private TextMeshProUGUI matchingText;
    // [SerializeField] private GameObject matchingSpinnerObject;

    [Header("Matchmaking")]
    [SerializeField] private int selectedCharacterIndex = -1;
    [SerializeField] private bool isMatchmaking = false;
    private float matchmakingElapsed = 0f;
    private Coroutine matchmakingTimerCoroutine;

    [Header("Sound")]
    [SerializeField] private AudioClip characterSelectSound;
    [SerializeField] private AudioClip matchmakingSound;    // TODO ���� �߰�
    private AudioSource audioSource;

    private void Start() {
        InitalizedButtonsAndImages();
        InitState();
        UpdateMatchmakingUI(false);
        // audioSource = GetComponent<AudioSource>();
    }

    private void InitalizedButtonsAndImages() {
        matchmakingButton.onClick.AddListener(() => OnClickMatchmakingtButton());
        cancleMatchmakingButton.onClick.AddListener(() => OnClickCanclematchMakingtButton());
        backToMainButton.onClick.AddListener(() => OnClickBackToMainButton());

        for (int i = 0; i < characterButtons.Count; i++) {
            int index = i; // Value Capture
            characterButtons[index].onClick.AddListener(() => OnCharacterSelected(index));
            characterButtons[index].GetComponent<Image>().sprite = GameMainManager.Instance.GetCharacterPortraitImage(0);
            // TODO ĳ���� ��Ʈ���� ���� 0���� �Ǿ�����
        }
    }

    public void InitState() {
        selectedCharacterIndex = -1;
        selectedCharacterBorderImage.gameObject.SetActive(false);
        matchmakingButton.interactable = false;
    }

    private void UpdateMatchmakingUI(bool isMatching) {
        matchmakingButton.gameObject.SetActive(!isMatching);
        cancleMatchmakingButton.gameObject.SetActive(isMatching);
        matchingText.gameObject.SetActive(isMatching);
        // matchingSpinnerObject.gameObject.SetActive(isMatching);
    }

    private void OnClickMatchmakingtButton() {
        Debug.Log("Ui_CharacterSelect | OnClickMatchmakingtButton");
        if (selectedCharacterIndex == -1) return;

        UpdateMatchmakingUI(true);
        isMatchmaking = true;

        // Ÿ�̸�
        matchmakingElapsed = 0f;
        if (matchmakingTimerCoroutine != null) {
            StopCoroutine(matchmakingTimerCoroutine);
        }
        matchmakingTimerCoroutine = StartCoroutine(MatchmakingTimerRoutine());

        // PlayMatchmakingSound(matchmakingSound);
        GameMainManager.Instance.OnStartMatchmakingClicked();

    }

    private void OnClickCanclematchMakingtButton() {
        Debug.Log("��Ī ��� ��ư Ŭ��");
        UpdateMatchmakingUI(false);
        isMatchmaking = false;
        // TODO ��Ī ���
    }

    private void OnClickBackToMainButton() {
        Debug.Log("Ui_CharacterSelect | OnClickBackToMainButton");
        GameMainManager.Instance.OnBackToMainClicked(); // �ٸ� UI ������ �Ŵ������� ó��
        matchmakingButton.interactable = false;
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

        matchmakingButton.interactable = true;
        GameMainManager.Instance.SetSelectCharacterIndex(index);
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

    private IEnumerator MatchmakingTimerRoutine() {
        while (isMatchmaking) {
            matchmakingElapsed += Time.deltaTime;
            int seconds = Mathf.FloorToInt(matchmakingElapsed);
            matchingText.text = $"Find Match... {seconds}s";
            yield return null;
        }
    }
}
