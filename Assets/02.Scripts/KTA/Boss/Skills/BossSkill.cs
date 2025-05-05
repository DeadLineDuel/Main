using System;
using Unity.Netcode;
using UnityEngine;

namespace Boss.Skills
{
    public abstract class BossSkill : NetworkBehaviour
    {
        [field: Header("Dependencies")] 
        [field: SerializeField] protected BossCore BossCore { get; set; }
        
        [field: Header("Skill Data")]
        [field: SerializeField] public String BossSkillName { get; protected set; }
        [field: SerializeField] public float IndicatorTime { get; protected set; }
        [field: SerializeField] public float EffectTime { get; protected set; }
        [field: SerializeField] public float SkillAnimationTime { get; protected set; }
        [field: SerializeField] protected float damageCoeff;

        protected int BossSkillHash { get; private set; }
        
        protected Collider[] Colliders = new Collider[2];

        
        protected virtual void Awake()
        {
           BossSkillHash = Animator.StringToHash(BossSkillName);
           SetSkillAnimationTime();
        }

        public abstract void ActivateSkill();
        [ClientRpc] protected virtual void ActivateIndicatorClientRpc() { }
        [ClientRpc] protected virtual void ActivateSkillEffectClientRpc() { }
        public virtual void ActivateDamageCollider(float bossAtk) { }
        
        private void SetSkillAnimationTime()
        {
            AnimationClip[] clips = BossCore.NetworkAnimator.Animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name == BossSkillName)
                {
                    float realTime = clip.length / (BossCore.NetworkAnimator.Animator.speed);
                    SkillAnimationTime = realTime;
                    Debug.Log(clip.name + " "  + SkillAnimationTime);
                }
            }
        }
    }
}