using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ��� ����ü�� ��ӽ�ų ��ũ��Ʈ, ���� �� ���� ����
/// </summary>
public class Object_Base : MonoBehaviour
{
    public float attack = 10f;
    public float defense = 5f;
    public float attackSpeed = 1f;
    public float moveSpeed = 5f;

    // ����/����� ����
    public void ApplyBuffDebuff(BuffTypeEnum effectType, float value) {
        switch (effectType) {
            case BuffTypeEnum.Attack:
                attack += value;
                Debug.Log($"�÷��̾� ���ݷ� {value} ���� �� ����: {attack}");
                break;

            case BuffTypeEnum.Defense:
                defense += value;
                Debug.Log($"�÷��̾� ���� {value} ���� �� ����: {defense}");
                break;

            case BuffTypeEnum.AttackSpeed:
                attackSpeed += value;
                Debug.Log($"�÷��̾� ���ݼӵ� {value} ���� �� ����: {attackSpeed}");
                break;

            case BuffTypeEnum.MoveSpeed:
                moveSpeed += value;
                Debug.Log($"�÷��̾� �̵��ӵ� {value} ���� �� ����: {moveSpeed}");
                break;
            case BuffTypeEnum.Cooltime:
                // ��Ÿ���� ������ ó�� (�ܼ� ���� ó�� +- ��� �Ұ���)
                break;
        }
    }
}
