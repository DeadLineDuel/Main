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

    private void Start() {
        nameText.text = buffName;
        costText.text = buffCost.ToString();

        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        uiController = FindObjectOfType<UI_BuffDebuff>();
    }

    public void OnClick() {
        uiController.SelectItem(this);        
    }
}
