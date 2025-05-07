using System;
using System.Collections.Generic;
using System.Linq;
using KTA.Test;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuffDebuff : NetworkBehaviour
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
    [SerializeField] private GameObject mainPanel;  // �ּ�ȭ�� �� ��������� �г�
    [SerializeField] private RectTransform backgroundRect;

    private int selectedTabIndex = 0;

    [Header("Setting")]
    [SerializeField] private float minimizedAlpha = 0.3f;
    [SerializeField] private float minimizedRestoreAlpha = 1.0f;
    [SerializeField] private float minimizedHeight = 70.0f;
    [SerializeField] private float minimizedRestoreHeight = 600.0f;
    [SerializeField] private float minimizedBackgroundPosY = 0.0f;
    [SerializeField] private float minimizedRestoreBackgroundPosY = 24.0f;
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

    [SerializeField] private TESTCPController CPController;

    // private void Start() {
    //     LoadBuffDataFromJson();
    //     CreateScrollViewContentUI();
    //     BindButtonEvent();
    //
    //     SelectTab(selectedTabIndex);    // Initial Value -> 0
    //
    //     AddCP(1000); // TODO TEST
    // }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        LoadBuffDataFromJson();
        CreateScrollViewContentUI();
        BindButtonEvent();

        SelectTab(selectedTabIndex);    // Initial Value -> 0
    }

    public void Init()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        CPController = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<TESTCPController>();
        CPController.CP.OnValueChanged += OnCPChange;
        Debug.Log("CPController" + CPController);
    }
    
    private void BindButtonEvent() {
        // Player Enemy Boss ��
        for (int i = 0; i < tabButtons.Length; i++) {
            int index = i; // Value Capture
            tabButtons[i].onClick.AddListener(() => SelectTab(index));
        }
        applyButton.onClick.AddListener(() => ClickApplyButton());  // ���� ��ư

        // �ּ�ȭ
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

    /// <summary>
    /// �ܺο��� �Է¹޾� CP�� ����
    /// </summary>
    /// <param name="value"></param>
    public void AddCP(int value) {
        currentCP += value;

        AddCPServerRpc(value);
    }

    [ServerRpc]
    public void AddCPServerRpc(int value)
    {
        if (CPController != null)
        {
            CPController.CP.Value += value;
        }
    }

    private void OnCPChange(int prev, int next)
    {
        SetCPText(next);
    }
    private void SetCPText(int value) {
        cpText.text = value.ToString();
    }

    public void SelectItem(BuffDebuffItem item) {
        DeselectItem(); // ���� ������ ���� ����
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
        if (CPController.CP.Value < selectedItem.buffCost) return;
        if (CheckExistSameBuff()) {
            DeselectItem();
            Debug.Log("�� ������ �ߺ���!");
            return;
        }

        AddCPServerRpc(-selectedItem.buffCost);
        currentCP -= selectedItem.buffCost;
        //SetCPText(currentCP);
        SummonTotem();
        DeselectItem();
    }

    private bool CheckExistSameBuff() {
        return spawnedTotem.Any(totem => totem.buffName == selectedItem.buffName);
    }

    public void SummonTotem() {
        TotemManager.Instance.SummonTotem(selectedItem);
    }

    private void ClickMinimizeButton() {
        SetMainPanelView(false);
        SetCanvasGroupState(minimizeCanvasGroup, false);
        SetCanvasGroupState(minimizeRestoreCanvasGroup, true);
        SetBackgroundHeight(minimizedHeight, minimizedBackgroundPosY);
    }

    private void ClickMinimizeRestoreButton() {
        SetMainPanelView(true);
        SetCanvasGroupState(minimizeCanvasGroup, true);
        SetCanvasGroupState(minimizeRestoreCanvasGroup, false);
        SetBackgroundHeight(minimizedRestoreHeight, minimizedRestoreBackgroundPosY);
    }

    private void SetMainPanelView(bool active) {
        mainPanel.SetActive(active);
    }

    private void SetBackgroundHeight(float height, float posY) {
        backgroundRect.sizeDelta = new Vector2(backgroundRect.sizeDelta.x, height);
        backgroundRect.anchoredPosition = new Vector2(backgroundRect.anchoredPosition.x, posY);
    }

    private void SetCanvasGroupState(CanvasGroup canvasGroup, bool active) {
        canvasGroup.alpha = active ? minimizedRestoreAlpha : minimizedAlpha;
        canvasGroup.interactable = active;
    }
}
