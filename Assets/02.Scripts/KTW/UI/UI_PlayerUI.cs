using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlayerUI : MonoBehaviour
{
    [Header("UI Assign")]
    [SerializeField] private Slider playerHPSlider;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI aspText;
    [SerializeField] private TextMeshProUGUI coolText;
    [SerializeField] private Button[] skillButton;
    [SerializeField] private TextMeshProUGUI[] skillCoolTexts;
    [SerializeField] private Image[] skillButtonImages;
    [SerializeField] private Button healItemButton;
    [SerializeField] private TextMeshProUGUI itemAmountText;

    [Header("Skill Description")]
    [SerializeField] private GameObject skillDescriptionPanel;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;

    [Header("Player Info")]
    [SerializeField] private TextMeshProUGUI playerIdText; // 플레이어 ID를 표시할 텍스트 UI 추가

    private Coroutine[] cooldownCoroutines = new Coroutine[4];
    private int coolTimeReduction = 0;
    private string playerId; // 플레이어 ID 저장 변수

    private const float UIHPBarAnimationDuration = 0.3f;


    private void Start() {
        InitializedButton();
        skillDescriptionPanel.SetActive(false);
        // 테스트 코드는 실제 환경에서는 주석 처리
        // UpdateStatTexts(100, 100, 1, -2);
        // StartSkillCooldown(0, 5);
        // StartSkillCooldown(1, 10);
        // StartSkillCooldown(2, 15);
        // StartSkillCooldown(3, 20);
    }

    private void InitializedButton() {
        healItemButton.onClick.AddListener(() => OnClickHealItemButton());
        skillButton[0].onClick.AddListener(() => OnClickQButton());
        skillButton[1].onClick.AddListener(() => OnClickWButton());
        skillButton[2].onClick.AddListener(() => OnClickEButton());
        skillButton[3].onClick.AddListener(() => OnClickRButton());

        // 마우스 호버 이벤트
        for (int i = 0; i < skillButton.Length; i++) {
            int index = i; // Value Capture
            AddMouseHoverEventSkillDescrption(index);
        }
    }

    /// <summary>
    /// 플레이어 ID 설정 및 UI 업데이트
    /// </summary>
    public void SetPlayerID(string id)
    {
        playerId = id;
        
        // 플레이어 ID UI 업데이트
        if (playerIdText != null)
        {
            playerIdText.text = id;
        }
        
        Debug.Log($"UI_PlayerUI | 플레이어 ID 설정: {id}");
    }

    /// <summary>
    /// UI 플레이어 체력 업데이트
    /// </summary>
    /// <param name="hpValue">0~1 사이의 값</param>
    public void UpdatePlayerHP(float hpValue) {
        playerHPSlider.DOValue(hpValue, UIHPBarAnimationDuration).SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// 회복 아이템 남은 사용 횟수
    /// </summary>
    /// <param name="count"></param>
    public void UpdateItemAmount(int count) {
        itemAmountText.text = count.ToString();
    }


    // 플레이어 스킬 버튼 이벤트
    public void OnClickQButton() {
        Debug.Log("Pressed Q");
        OnSkillButtonPressed(0);
    }

    public void OnClickWButton() {
        Debug.Log("Pressed W");
        OnSkillButtonPressed(1);
    }

    public void OnClickEButton() {
        Debug.Log("Pressed E");
        OnSkillButtonPressed(2);
    }

    public void OnClickRButton() {
        Debug.Log("Pressed R");
        OnSkillButtonPressed(3);
    }

    public void OnSkillButtonPressed(int skillIndex) {
        // 플레이어 스킬 사용 로직 연결
        // 네트워크 환경에서는 서버에 스킬 사용 요청
        if (!string.IsNullOrEmpty(playerId))
        {
            // 여기에 스킬 사용 서버 요청 로직 추가
            Debug.Log($"UI_PlayerUI | 플레이어 {playerId}가 스킬 {skillIndex} 사용 요청");
        }
    }


    public void OnClickHealItemButton() {
        Debug.Log("UI_PlayerUI | OnClickHealItemButton");
        // 필요시 힐 아이템 사용 서버 요청 로직 추가
    }

    /// <summary>
    /// 공격력 방어력 공격속도 쿨타임보너스 float 형태로 입력받고 업데이트
    /// </summary>
    public void UpdateStatTexts(float atk, float def, float asp, int cool) {
        string sign = cool >= 0 ? "+": (cool < 0 ? "-" : "");   // + (양수) - 음수

        atkText.text = $"ATK : {atk}";
        defText.text = $"DEF : {def}";
        aspText.text = $"ASP : {asp:0.00}";
        coolText.text = $"Cool {sign}{Mathf.Abs(cool)}"; // +- 1초 2초 이런식
        coolTimeReduction = cool;
    }

    /// <summary>
    /// 외부에서 스킬 쿨타임을 줄이고 싶을 때 호출
    /// skillIndex: 0=Q, 1=W, 2=E, 3=R
    /// cooldownSeconds: 쿨타임(초)
    /// </summary>
    public void StartSkillCooldown(int skillIndex, int cooldownSeconds) {
        if (!IsValidSkillIndex(skillIndex)) return;

        // 기존 쿨타임 코루틴 체크
        if (cooldownCoroutines[skillIndex] != null) {
            StopCoroutine(cooldownCoroutines[skillIndex]);
        }
        cooldownCoroutines[skillIndex] = StartCoroutine(SkillCooldownCoroutine(skillIndex, cooldownSeconds));
    }

    private bool IsValidSkillIndex(int index) {
        return index >= 0 && index < skillButton.Length;

    }

    private IEnumerator SkillCooldownCoroutine(int skillIndex, int cooldownSeconds) {
        TextMeshProUGUI targetText = skillCoolTexts[skillIndex];
        Image targetImage = skillButtonImages[skillIndex];
        Color originalColor= targetImage.color;
        Color fadedColor = targetImage.color; fadedColor.a = 0.5f;

        targetImage.color = fadedColor;


        float remainingTime = Mathf.Max(cooldownSeconds - coolTimeReduction, 0);    // 쿨타임 보너스 적용
        int displayedTime = Mathf.CeilToInt(remainingTime);

        targetText.text = displayedTime.ToString();

        while (remainingTime > 0f) {
            remainingTime -= Time.deltaTime;
            displayedTime = Mathf.CeilToInt(remainingTime);

            targetText.text = displayedTime.ToString();

            yield return null;
        }


        targetText.text = "";
        targetImage.color = originalColor;

        cooldownCoroutines[skillIndex] = null;
    }

    private void AddMouseHoverEventSkillDescrption(int index) {
        EventTrigger trigger = skillButton[index].gameObject.GetComponent<EventTrigger>();
        if (trigger == null) trigger = skillButton[index].gameObject.AddComponent<EventTrigger>();

        // PointerEnter 이벤트
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => ShowSkillDescription(index));
        trigger.triggers.Add(entryEnter);

        // PointerExit 이벤트
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => HideSkillDescription());
        trigger.triggers.Add(entryExit);
    }

    private void ShowSkillDescription(int skillIndex) {
        string characterName = GetCurrentCharacterName();
        CharacterSkillData data = SkillDataLoader.Instance.LoadSkillData(characterName);
        if (data != null && skillIndex < data.skillNames.Length) {
            skillNameText.text = data.skillNames[skillIndex];
            skillDescriptionText.text = data.skillDescriptions[skillIndex];
            skillDescriptionPanel.SetActive(true);
            // 패널 위치 조정 (버튼 옆에 표시)
            //Vector3 buttonPos = skillButton[skillIndex].transform.position;
            //skillDescriptionPanel.transform.position = new Vector3(
            //    buttonPos.x + 200,
            //    buttonPos.y,
            //    buttonPos.z
            //);
        }
    }

    private void HideSkillDescription() {
        skillDescriptionPanel.SetActive(false);
    }

    private string GetCurrentCharacterName() {
        // 현재는 Cosmo만 사용
        return "Cosmo";
    }
}