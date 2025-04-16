using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffTotem : MonoBehaviour
{
    [Header("Buff Data")]
    public string buffName;
    public BuffTargetEnum targetType;
    public BuffTypeEnum buffType;
    public float buffDuration = 10f;
    public float buffValue;

    [Header("Network")]
    public int owner;   // TODO 네트워크 값에 따라 누구의 토템인지 넣고 그 사람만 공격할 수 있도록 해야할 듯

    private Object_Base target;
    private BuffDebuff appliedBuff;

    public UI_BuffDebuff uiBuffDebuff;

    private void Start() {
        StartCoroutine(BuffDurationRoutine());
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
            .Any(buff => buff.buffName == buffName);    // 동일 버프가 존재하는지 체크

        // 해당 버프가 존재하지 않을 때 컴포넌트 추가
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
}
