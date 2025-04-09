using System;
using UnityEngine;

namespace Boss.Skills
{
    public abstract class BossSkill : MonoBehaviour
    {
        [field: SerializeField] public String BossSkillName { get; private set; }

        public int BossSkillHash { get; private set; }

        protected virtual void Awake()
        {
           BossSkillHash =  Animator.StringToHash(BossSkillName);
        }

        public abstract void Perform();
    }
}