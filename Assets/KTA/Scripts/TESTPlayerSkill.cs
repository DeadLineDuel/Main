using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class TESTPlayerSkill : NetworkBehaviour
{
    protected static readonly int Move = Animator.StringToHash("CanMove");
    
    [SerializeField] protected TESTPlayerCore PlayerCore;
    protected Collider[] Colliders = new Collider[2];

    
    public abstract void ActivateSkill();
    public virtual void PlayEffect() { }
    [ServerRpc] public virtual void SkillHitServerRpc(ServerRpcParams rpcParams = default) { }

}
