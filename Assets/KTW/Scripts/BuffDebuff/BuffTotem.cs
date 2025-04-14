using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTotem : MonoBehaviour
{
    public BuffTargetEnum targetType;
    public BuffTypeEnum effectType;
    //public float duration = 10f;
    public float effectInterval = 1f;
    public float effectValue;

    public int owner;   // TODO 네트워크 값에 따라 누구의 토템인지

    private void Start() {
        
    }

    private void ApplyBuffToTarget() {
        switch (targetType) {
            case BuffTargetEnum.Player:
                ApplyToPlayer();
                break;

            case BuffTargetEnum.Enemy:
                ApplyToEnemy();
                break;

            case BuffTargetEnum.Boss:
                ApplyToBoss();
                break;
        }
    }

    private void ApplyToPlayer() {

    }

    private void ApplyToEnemy() {

    }

    private void ApplyToBoss() {

    }
}
