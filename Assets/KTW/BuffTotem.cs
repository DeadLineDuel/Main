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

    public int owner;   // TODO ��Ʈ��ũ ���� ���� ������ ��������

    private void Start() {
        
    }
}
