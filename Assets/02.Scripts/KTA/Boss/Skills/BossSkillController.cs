using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Boss.Skills
{
    public class BossSkillController : NetworkBehaviour
    {
        [SerializeField] private BossCore bossCore;
        
        public NetworkVariable<bool> isSkillActive = new (writePerm: NetworkVariableWritePermission.Server);

        [SerializeField] private List<BossSkill> skillsPrefab = new List<BossSkill>();
        [SerializeField] private byte currentSkillIndex;
        private BossSkill currentSkill;

        private byte SelectSkill() // TODO : Need to Add Logic
        {
            return (byte) Random.Range(0, skillsPrefab.Count);
        }

        public void ActivateSkill()
        {
            if (!IsServer) return;  // Only on Server

            currentSkillIndex = SelectSkill();
            currentSkill = skillsPrefab[currentSkillIndex];
            SetCurrentSkillClientRpc(currentSkillIndex);
            
            currentSkill.ActivateSkill();
            StartCoroutine(SetIsSkillActive(currentSkill.SkillAnimationTime));
        }

        [ClientRpc]
        private void SetCurrentSkillClientRpc(byte skillIndex)
        {
            currentSkillIndex = skillIndex;
            currentSkill = skillsPrefab[skillIndex];
        }
        
        private IEnumerator SetIsSkillActive(float waitTime)
        {
            isSkillActive.Value = true;
            yield return new WaitForSeconds(waitTime);
            isSkillActive.Value = false;
        }
    }
}