using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BuffTypeEnum{
    Attack,
    Defense,
    AttackSpeed,
    MoveSpeed,
    Cooltime,
}

public enum BuffTargetEnum {
    Player,
    Enemy,
    Boss
}

public class BuffDebuffItem : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private string buffName;
    [SerializeField] public int buffCost = 1;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;

    [Header("Effect")]
    public BuffTargetEnum target;
    public BuffTypeEnum type;
    public float value = 10;

    private UI_BuffDebuff uiController;

    public void Init(BuffData data) {
        buffName = data.name;
        buffCost = data.cost;
        target = (BuffTargetEnum)System.Enum.Parse(typeof(BuffTargetEnum), data.target);
        type = (BuffTypeEnum)System.Enum.Parse(typeof(BuffTypeEnum), data.type);
        value = data.value;
        SetUIText();
        SetButtonEvent();
    }

    private void SetUIText() {
        nameText.text = buffName;
        costText.text = buffCost.ToString();
    }

    private void SetButtonEvent() {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        uiController = FindObjectOfType<UI_BuffDebuff>();
    }

    public void OnClick() {
        uiController.SelectItem(this);        
    }
}
