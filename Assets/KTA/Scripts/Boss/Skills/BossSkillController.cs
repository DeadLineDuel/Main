using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Skills
{
    public class BossSkillController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public bool IsSkillActive { get; private set; } = false;
        
        [SerializeField] private List<BossSkill> skills;
        private BossSkill currentSkill;

        private BossSkill SelectSkill()
        {
            return skills[0];
        }
        
        public void ActivateSkill()
        {
            IsSkillActive = true;
            currentSkill = SelectSkill();
            animator.SetTrigger(currentSkill.BossSkillHash); // Play Skill Animation
        }

        private void OnSpawnEffect()
        {
            currentSkill.Perform();
        }
        
        private void OnAnimationEnd()
        {
            IsSkillActive = false;
        }

    }
}