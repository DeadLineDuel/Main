using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace Boss.Skills
{
    public class BossSkill_GroundHit : BossSkill
    {
        [field: Header("Dependencies")]
        [field: SerializeField] private ParticleSystem skillEffectParticle;
        [field: SerializeField] private SkillIndicator skillIndicator;
        
        [field: Header("Skill Data")]
        [field: SerializeField] private float radius;
        
        private Vector3 bossPos;

        public override void ActivateSkill()
        {
            if (!IsServer) return;  // On Server
            
            bossPos = BossCore.transform.position;
            StartCoroutine(ExecuteSkillSequence());
        }

        private IEnumerator ExecuteSkillSequence()
        {
            BossCore.NetworkAnimator.SetTrigger(BossSkillHash);
            
            // Play Indicator
            yield return new WaitForSeconds(IndicatorTime);
            ActivateIndicatorClientRpc();
            
            // Play Effect and Damage Collider
            yield return new WaitForSeconds(EffectTime);
            ActivateSkillEffectClientRpc();
            ActivateDamageCollider(BossCore.BossStats.Atk.Value);
        }
        
        [ClientRpc]
        protected override void ActivateIndicatorClientRpc()
        {
            if (!BossCore.BossCharacter.IsClientBoss) return;
            skillIndicator.ActivateIndicator(bossPos, 360f, 1.6f, 0f);
        }

        [ClientRpc]
        protected override void ActivateSkillEffectClientRpc()
        {
            if (!BossCore.BossCharacter.IsClientBoss) return;
            skillEffectParticle.Play();
        }

        public override void ActivateDamageCollider(float bossAtk)
        {
            if (!IsServer) return;
            
            int layerMask = LayerMask.GetMask("Player");
            var size = Physics.OverlapSphereNonAlloc(bossPos, radius, Colliders, layerMask);
                
            Vector3 forward = transform.forward;
                
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
            Gizmos.DrawWireSphere(bossPos, radius);
        }
    }
}