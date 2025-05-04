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
        [ClientRpc] public virtual void ActivateIndicatorClientRpc() { }
        [ClientRpc] public virtual void ActivateSkillEffectClientRpc() { }
        public virtual void ActivateDamageCollider(float bossAtk) { }
        
        private void SetSkillAnimationTime()
        {
            AnimationClip[] clips = BossCore.NetworkAnimator.Animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name == BossSkillName)
                {
                    AnimatorStateInfo stateInfo = BossCore.NetworkAnimator.Animator.GetCurrentAnimatorStateInfo(0); // 0은 레이어 인덱스
                    float realTime = clip.length / (BossCore.NetworkAnimator.Animator.speed * stateInfo.speed);
                    SkillAnimationTime = realTime;
                    Debug.Log(clip.name + " "  + SkillAnimationTime);
                }
            }
        }
    }
}