using System;
using UnityEngine;

namespace Boss.Skills
{
    public abstract class BossSkill : MonoBehaviour
    {
        [field: SerializeField] public String BossSkillName { get; protected set; }
        [field: SerializeField] public SkillIndicator SkillIndicator { get; protected set; }
        protected Collider[] Colliders = new Collider[2];
        protected Vector3 TargetPosition;
        public int BossSkillHash { get; private set; }

        protected virtual void Awake()
        {
           BossSkillHash =  Animator.StringToHash(BossSkillName);
           if (SkillIndicator)
           {
               SkillIndicator.OnIndicatorComplete += OnIndicatorComplete;
           }
        }
        
        public abstract void Perform(Vector3 targetPosition);
        protected abstract void PerformIndicator();
        protected abstract void OnIndicatorComplete();
        protected abstract void PerformCollider();
        protected abstract void PerformParticle();
    }
}