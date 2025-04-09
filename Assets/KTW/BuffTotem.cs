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
}
