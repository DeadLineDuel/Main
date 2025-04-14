using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTotem : MonoBehaviour
{
    public string buffName;
    public BuffTargetEnum targetType;
    public BuffTypeEnum buffType;
    public float buffDuration = 3f;
    public float buffValue;

    public int owner;   // TODO 네트워크 값에 따라 누구의 토템인지 넣고 그 사람만 공격할 수 있도록

    private Object_Base target;
    private BuffDebuff appliedBuff;

    private void Start() {
        StartCoroutine(ApplyBuffCoroutine());
    }

    private IEnumerator ApplyBuffCoroutine() {
        ApplyBuffToTarget(true);
        yield return new WaitForSeconds(buffDuration);
        Destroy(gameObject);
    }

    private void ApplyBuffToTarget(bool alive) {
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

        if (target == null) return;

        if (alive) {
            BuffDebuff existBuff = target.GetComponent<BuffDebuff>();
            if (existBuff == null || existBuff.buffName != buffName) {  // 동일 버프가 존재하지 않을 경우 버프 추가
                appliedBuff = target.gameObject.AddComponent<BuffDebuff>();
                appliedBuff.buffName = buffName;
                appliedBuff.buffType = buffType;
                appliedBuff.value = buffValue;

            }
        }
        else {
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
        ApplyBuffToTarget(false);
    }
}
