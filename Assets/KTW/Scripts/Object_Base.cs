using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  모든 생명체를 상속시킬 스크립트, 스탯 등 정보 포함
/// </summary>
public class Object_Base : MonoBehaviour
{
    public float attack = 10f;
    public float defense = 5f;
    public float attackSpeed = 1f;
    public float moveSpeed = 5f;

    // 버프/디버프 적용
    public void ApplyBuffDebuff(BuffTypeEnum effectType, float value) {
        switch (effectType) {
            case BuffTypeEnum.Attack:
                attack += value;
                Debug.Log($"플레이어 공격력 {value} 변경 → 현재: {attack}");
                break;

            case BuffTypeEnum.Defense:
                defense += value;
                Debug.Log($"플레이어 방어력 {value} 변경 → 현재: {defense}");
                break;

            case BuffTypeEnum.AttackSpeed:
                attackSpeed += value;
                Debug.Log($"플레이어 공격속도 {value} 변경 → 현재: {attackSpeed}");
                break;

            case BuffTypeEnum.MoveSpeed:
                moveSpeed += value;
                Debug.Log($"플레이어 이동속도 {value} 변경 → 현재: {moveSpeed}");
                break;
            case BuffTypeEnum.Cooltime:
                // 쿨타임은 별개로 처리 (단순 스탯 처리 +- 계산 불가능)
                break;
        }
    }
}
