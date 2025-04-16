using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuffDebuff : MonoBehaviour
{
    [Header("UI Assign")]
    [SerializeField] private TMP_Text cpText;
    [SerializeField] private Button[] tabButtons; // Player Enemy Boss
    [SerializeField] private ScrollRect[] scrollls;
    [SerializeField] private Button applyButton;
    [SerializeField] private Button minimizeButton;
    [SerializeField] private Button minimizeRestoreButton;
    [SerializeField] private CanvasGroup minimizeCanvasGroup;
    [SerializeField] private CanvasGroup minimizeRestoreCanvasGroup;
    [SerializeField] private GameObject mainPanel;  // 최소화할 때 사라지게할 패널
    [SerializeField] private RectTransform backgroundRect;

    private int selectedTabIndex = 0;

    [Header("Setting")]
    [SerializeField] private float minimizedAlpha = 0.3f;
    [SerializeField] private float minimizedRestoreAlpha = 1.0f;
    [SerializeField] private float minimizedHeight = 70.0f;
    [SerializeField] private float minimizedRestoreHeight = 600.0f;
    [SerializeField] private int currentCP = 10; // TEST
    [SerializeField] private Color selectedTabColor = Color.red;
    [SerializeField] private Color tabColor = Color.white;
    [SerializeField] private Color selectedItemColor = Color.yellow;
    [SerializeField] private Color itemColor = Color.white;
    
    private BuffDebuffItem selectedItem;
    
    
    [Header("Buff Data")]
    [SerializeField] private TextAsset buffJson;
    private BuffDatabase buffDatabase;
    [SerializeField] private GameObject buffItemPrefab;
    [SerializeField] private GameObject totemPrefab;

    public List<BuffTotem> spawnedTotem = new List<BuffTotem>();

    private void Start() {
        LoadBuffDataFromJson();
        CreateScrollViewContentUI();
        BindButtonEvent();
        SetCPText(currentCP);
        SelectTab(selectedTabIndex);    // Initial Value -> 0
    }

    private void BindButtonEvent() {
        // Player Enemy Boss 탭
        for (int i = 0; i < tabButtons.Length; i++) {
            int index = i; // Value Capture
            tabButtons[i].onClick.AddListener(() => SelectTab(index));
        }
        applyButton.onClick.AddListener(() => ClickApplyButton());  // 적용 버튼

        // 최소화
        minimizeButton.onClick.AddListener(() => ClickMinimizeButton());
        minimizeRestoreButton.onClick.AddListener(() => ClickMinimizeRestoreButton());
    }

    private void LoadBuffDataFromJson() {
        if (buffJson == null) {
            Debug.LogError("UI_BuffDebuff | Buff JSON file does not assigned");
            return;
        }
        buffDatabase = JsonUtility.FromJson<BuffDatabase>(buffJson.text);
    }

    private void CreateScrollViewContentUI() {
        for (int i = 0; i < scrollls.Length; i++) {
            Transform content = scrollls[i].content;
            List<BuffData> buffs = i switch {
                0 => buffDatabase.playerBuffs,
                1 => buffDatabase.enemyBuffs,
                2 => buffDatabase.bossBuffs,
                _ => new List<BuffData>()
            };
            foreach (BuffData data in buffs) {
                CreateBuffItemUI(data, content);
            }
        }
    }

    private void CreateBuffItemUI(BuffData data, Transform parent) {
        GameObject itemObject = Instantiate(buffItemPrefab, parent);
        BuffDebuffItem item = itemObject.GetComponent<BuffDebuffItem>();
        
        item.Init(data, this);
    }


    private void SelectTab(int index) {
        selectedTabIndex = index;
        DeselectItem();
        for (int i = 0; i < scrollls.Length; i++) {
            scrollls[i].gameObject.SetActive(false);
            tabButtons[i].GetComponent<Image>().color = tabColor;
        }
        scrollls[index].gameObject.SetActive(true);
        tabButtons[index].GetComponent<Image>().color = selectedTabColor;
    }

    public void SetCPText(int value) {
        cpText.text = value.ToString();
    }

    public void SelectItem(BuffDebuffItem item) {
        DeselectItem(); // 기존 아이템 선택 해제
        selectedItem = item;
        item.GetComponent<Image>().color = selectedItemColor;
    }

    private void DeselectItem() {
        if (selectedItem != null) {
            selectedItem.GetComponent<Image>().color = itemColor;
            selectedItem = null;
        }
    }

    private void ClickApplyButton() {
        if (selectedItem == null) return;
        if (currentCP < selectedItem.buffCost) return;
        if (CheckExistSameBuff()) {
            DeselectItem();
            Debug.Log("이 버프는 중복임!");
            return;
        }

        currentCP -= selectedItem.buffCost;
        SetCPText(currentCP);
        SummonTotem();
        DeselectItem();
    }

    private bool CheckExistSameBuff() {
        return spawnedTotem.Any(totem => totem.buffName == selectedItem.buffName);
    }

    public void SummonTotem() {
        GameObject newTotem = Instantiate(totemPrefab, GetTotemSpawnPosition(), Quaternion.identity);
        BuffTotem totem = newTotem.GetComponent<BuffTotem>();

        // 토템 설정
        totem.buffName = selectedItem.buffName;
        totem.targetType = selectedItem.target;
        totem.buffType = selectedItem.type;
        totem.buffValue = selectedItem.value;
        
        totem.uiBuffDebuff = this;
        spawnedTotem.Add(totem);
    }

    public void RemoveTotem(BuffTotem totem) {
        if (spawnedTotem.Contains(totem)) {
            spawnedTotem.Remove(totem);
        }
    }

    public Vector3 GetTotemSpawnPosition() {
        // TODO 토템 소환 위치 플레이어위치?
        return Vector3.zero;
    }

    private void ClickMinimizeButton() {
        SetMainPanelView(false);
        SetCanvasGroupState(minimizeCanvasGroup, false);
        SetCanvasGroupState(minimizeRestoreCanvasGroup, true);
        SetBackgroundHeight(minimizedHeight);
    }

    private void ClickMinimizeRestoreButton() {
        SetMainPanelView(true);
        SetCanvasGroupState(minimizeCanvasGroup, true);
        SetCanvasGroupState(minimizeRestoreCanvasGroup, false);
        SetBackgroundHeight(minimizedRestoreHeight);
    }

    private void SetMainPanelView(bool active) {
        mainPanel.SetActive(active);
    }

    private void SetBackgroundHeight(float height) {
        backgroundRect.sizeDelta = new Vector2(backgroundRect.sizeDelta.x, height);
    }

    private void SetCanvasGroupState(CanvasGroup canvasGroup, bool active) {
        canvasGroup.alpha = active ? minimizedRestoreAlpha : minimizedAlpha;
        canvasGroup.interactable = active;
    }
}
