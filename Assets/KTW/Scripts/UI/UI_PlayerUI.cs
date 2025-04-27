using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    

    private Coroutine[] cooldownCoroutines = new Coroutine[4];
    private float coolTimeMultiplier = 1.0f;

    private const float UIHPBarAnimationDuration = 0.3f;


    private void Start() {
        InitializedButton();

        // TODO TEST
        UpdateStatTexts(100, 100, 1, 0.5f);
        StartSkillCooldown(0, 5);
        StartSkillCooldown(1, 10);
        StartSkillCooldown(2, 15);
        StartSkillCooldown(3, 20);
    }

    private void InitializedButton() {
        healItemButton.onClick.AddListener(() => OnClickHealItemButton());
        skillButton[0].onClick.AddListener(() => OnClickQButton());
        skillButton[1].onClick.AddListener(() => OnClickWButton());
        skillButton[2].onClick.AddListener(() => OnClickEButton());
        skillButton[3].onClick.AddListener(() => OnClickRButton());
    }

    private void Update() {
        // TODO TEST
        if (Input.GetKeyDown(KeyCode.A)) {
            UpdatePlayerHP(0.0f);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            UpdatePlayerHP(0.3f);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            UpdatePlayerHP(0.8f);
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            UpdatePlayerHP(1.0f);
        }
    }

    /// <summary>
    /// UI 플레이어 체력 설정
    /// </summary>
    /// <param name="hpValue">0~1사이의 값</param>
    public void UpdatePlayerHP(float hpValue) {
        playerHPSlider.DOValue(hpValue, UIHPBarAnimationDuration).SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// 회복 아이템 사용 가능 횟수
    /// </summary>
    /// <param name="count"></param>
    public void UpdateItemAmount(int count) {
        itemAmountText.text = count.ToString();
    }


    // TODO Player의 버튼에 연결
    public void OnClickQButton() {
        Debug.Log("Pressd Q");
    }

    public void OnClickWButton() {
        Debug.Log("Pressd W");
    }

    public void OnClickEButton() {
        Debug.Log("Pressd E");
    }

    public void OnClickRButton() {
        Debug.Log("Pressd R");
    }

    public void OnSkillButtonPressed(int skillIndex) {
        // TODO 플레이어 쪽 구현 보고 위나 아래거로 수정.
    }


    public void OnClickHealItemButton() {
        Debug.Log("UI_PlayerUI | OnClickHealItemButton");
    }

    /// <summary>
    /// 공격력 방어력 공격속도 쿨타임배수를 float 형태로 입력받고 업데이트
    /// </summary>
    public void UpdateStatTexts(float atk, float def, float asp, float cool) {
        atkText.text = $"ATK : {atk}";
        defText.text = $"DEF : {def}";
        aspText.text = $"ASP : {asp:0.00}";
        coolText.text = $"Cool x {cool:0.00}";
        coolTimeMultiplier = cool;
    }

    /// <summary>
    /// 외부에서 스킬 쿨타임을 시작할 때 호출
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


        float remainingTime = cooldownSeconds * coolTimeMultiplier;
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
}
