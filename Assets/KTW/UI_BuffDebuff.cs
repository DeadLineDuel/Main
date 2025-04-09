using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class UI_BuffDebuff : MonoBehaviour
{
    [SerializeField] private int currentCP = 10; // TEST
    public TMP_Text cpText;
    public Button[] tabButtons; // Player Enemy Boss

    public ScrollRect[] scrollls;
    public int selectedTabIndex = 0;


    public Button applyButton;

    [SerializeField] private Color selectedTabColor = Color.red;
    [SerializeField] private Color tabColor = Color.white;
    [SerializeField] private Color selectedItemColor = Color.yellow;
    [SerializeField] private Color itemColor = Color.white;
    private BuffDebuffItem selectedItem;
    public GameObject totemPrefab;

    private void Start() {
        for (int i = 0; i < tabButtons.Length; i++) {
            int index = i; // Value Capture
            tabButtons[i].onClick.AddListener(() => SelectTab(index));
        }
        applyButton.onClick.AddListener(() => ClickApplyButton());

        SetCPText(currentCP);
        SelectTab(selectedTabIndex);    // Initial Value -> 0
    }

    private void SelectTab(int index) {
        selectedTabIndex = index;
        DeselectItem();
        DeselectTab();
        scrollls[index].gameObject.SetActive(true);
        tabButtons[index].GetComponent<Image>().color = selectedTabColor;
    }

    private void DeselectTab() {
        for (int i = 0; i < scrollls.Length; i++) {
            scrollls[i].gameObject.SetActive(false);
            tabButtons[i].GetComponent<Image>().color = tabColor;
        }
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

        currentCP -= selectedItem.buffCost;
        SetCPText(currentCP);
        SummonTotem();
        DeselectItem();
    }

    public void SummonTotem() {
        GameObject newTotem = Instantiate(totemPrefab, GetTotemSpawnPosition(), Quaternion.identity);
        BuffTotem totem = newTotem.GetComponent<BuffTotem>();

        // 토템 설정
        totem.targetType = selectedItem.target;
        totem.effectType = selectedItem.type;
        totem.effectValue = selectedItem.value;
    }

    public Vector3 GetTotemSpawnPosition() {
        // TODO 토템 소환 위치 플레이어위치?
        return Vector3.zero;
    }
}
