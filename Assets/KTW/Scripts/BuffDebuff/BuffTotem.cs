using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuffTotem : Object_Base
{
    [Header("Buff Data")]
    public string buffName;
    public BuffTargetEnum targetType;
    public BuffTypeEnum buffType;
    public float buffDuration = 10f;
    public float buffValue;

    [Header("Network")]
    public int owner;   // TODO ��Ʈ��ũ ���� ���� ������ �������� �ְ� �� ����� ������ �� �ֵ��� �ؾ��� ��

    [Header("HP")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private float maxHP = 100.0f;
    [SerializeField] private float currentHP = 100.0f;
    private const float UIHPBarAnimationDuration = 0.3f;

    private Object_Base target;
    private BuffDebuff appliedBuff;
    public UI_BuffDebuff uiBuffDebuff;

    private void Start() {
        StartCoroutine(BuffDurationRoutine());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            UpdateHP(10);
        }
    }

    private IEnumerator BuffDurationRoutine() {
        ApplyBuffToTarget();
        yield return new WaitForSeconds(buffDuration);
        Destroy(gameObject);
    }

    private void ApplyBuffToTarget() {
        switch (targetType) {
            case BuffTargetEnum.Player:
                target = FindPlayer();
                break;

            case BuffTargetEnum.Enemy:
                target = FindEnemy();
                break;

            case BuffTargetEnum.Boss:
                target = FindBoss();
                break;
        }

        if (target == null) {
            Debug.LogError($"BuffTotem | ({targetType}) target not found");
            return;
        }

        bool isApplied = target.GetComponents<BuffDebuff>()
            .Any(buff => buff.buffName == buffName);    // ���� ������ �����ϴ��� üũ

        // �ش� ������ �������� ���� �� ������Ʈ �߰�
        if (!isApplied) {
            appliedBuff = target.gameObject.AddComponent<BuffDebuff>();
            appliedBuff.buffName = buffName;
            appliedBuff.buffType = buffType;
            appliedBuff.value = buffValue;
        }
    }

    private void RemoveBuff() {
        if (appliedBuff != null) {
            Destroy(appliedBuff);
        }
    }

    private Object_Base FindPlayer() {
        return FindObjectOfType<Object_Base>();
    }

    private Object_Base FindEnemy() {
        return FindObjectOfType<Object_Base>();
    }

    private Object_Base FindBoss() {
        return FindObjectOfType<Object_Base>();
    }

    private void OnDestroy() {
        RemoveBuff();
        uiBuffDebuff.RemoveTotem(this);
    }

    /// <summary>
    /// ���ۿ� ������ ���� �� HP Bar�� �ݿ�. �ܿ� HP �˻��ϰ� �ı�
    /// </summary>
    /// <param name="damage">�ִ� ������</param>
    public override void UpdateHP(float damage) {
        base.UpdateHP(damage);

        currentHP = Mathf.Max(currentHP - damage, 0);
        hpSlider.DOValue(currentHP / maxHP, UIHPBarAnimationDuration).SetEase(Ease.OutQuad);

        if (currentHP <= 0) {
            Destroy(gameObject);
        }
    }
}
