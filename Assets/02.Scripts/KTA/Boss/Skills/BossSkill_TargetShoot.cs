using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Boss.Skills
{
    public class BossSkill_TargetShoot : BossSkill
    {
        [field: Header("Dependencies")]
        [field: SerializeField] private ParticleSystem skillEffectParticle;
        [field: SerializeField] private ParticleSystem skillIndicator;

        [field: Header("Skill Data")]
        [field: SerializeField] private float radius;
        
        private Vector3 targetPos;

        public override void ActivateSkill()
        {
            if (!IsServer) return;  // On Server
            
            // Get Target Pos
            StartCoroutine(ExecuteSkillSequence());
        }

        private IEnumerator ExecuteSkillSequence()
        {
            BossCore.NetworkAnimator.SetTrigger(BossSkillHash);

            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(IndicatorTime);
                targetPos = gameObject.transform.position;
                // targetPos = BossCore.BossCharacter.GetTargetPosition; TODO: Set Target
                ActivateIndicatorClientRpc();
                yield return new WaitForSeconds(EffectTime);
                ActivateSkillEffectClientRpc();
                ActivateDamageCollider(BossCore.BossStats.Atk.Value);
            }
        }
        
        [ClientRpc]
        public override void ActivateIndicatorClientRpc()
        {
            if (!BossCore.BossCharacter.IsClientBoss) return;
            skillIndicator.Play();
        }

        [ClientRpc]
        public override void ActivateSkillEffectClientRpc()
        {
            if (!BossCore.BossCharacter.IsClientBoss) return;;
            skillEffectParticle.Play();
        }

        public override void ActivateDamageCollider(float bossAtk)
        {
            if (!IsServer) return;
            
            int layerMask = LayerMask.GetMask("Player");
            var size = Physics.OverlapSphereNonAlloc(targetPos, radius, Colliders, layerMask);
            
            if (size > 0)
            {
                for  (int i = 0; i < size; i++) // Do not use foreach on NonAlloc
                {
                    Debug.Log("[Boss] Hit Object : " + Colliders[i].gameObject.name);
                    if (Colliders[i].TryGetComponent<IDamageable>(out var damageable))
                    {
                        damageable.TakeDamage(bossAtk * damageCoeff);
                    }
                }   
            }
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetPos, radius);
        }
    }
}
