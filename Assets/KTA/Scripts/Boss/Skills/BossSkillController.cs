using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Skills
{
    public class BossSkillController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public bool IsSkillActive { get; private set; } = false;

        [SerializeField] private List<BossSkill> skills =  new List<BossSkill>();
        [SerializeField] private List<BossSkill> skillsPrefab =  new List<BossSkill>();
        private BossSkill currentSkill;

        private void Awake()
        {
            skills.Add(Instantiate(skillsPrefab[0]));
        }

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

        private void OnEffect()
        {
            print("OnEffect");
            currentSkill.Perform(gameObject.transform.position);
        }
        
        private void OnAnimationEnd()
        {
            IsSkillActive = false;
        }

    }
}