using System.Collections;
using UnityEngine;
using Unity.Netcode;

namespace Boss.Skills
{
    public class BossSkill_Slash : BossSkill
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
            skillIndicator.ActivateIndicator(bossPos, 180f, 0.5f, 0f);
        }

        [ClientRpc]
        protected override void ActivateSkillEffectClientRpc()
        {
            if (!BossCore.BossCharacter.IsClientBoss) return;
            skillEffectParticle.Play();
        }

        public override void ActivateDamageCollider(float bossAtk)
        {
            if (!IsServer) return;  // On Server
            
            int layerMask = LayerMask.GetMask("Player");
            var size = Physics.OverlapSphereNonAlloc(bossPos, radius, Colliders, layerMask);
            
            Vector3 forward = transform.forward;
            
            if (size > 0)
            {
                for  (int i = 0; i < size; i++) // Do not use foreach on NonAlloc
                {
                    Vector3 dir = (Colliders[i].transform.position - bossPos).normalized;
                    float angle = Vector3.Angle(forward, dir);
                    if (angle <= 90f) // half circle
                    {
                        Debug.Log("[Boss] Hit Object : " + Colliders[i].gameObject.name);
                        if (Colliders[i].TryGetComponent<IDamageable>(out var damageable))
                        {
                            damageable.TakeDamage(bossAtk * damageCoeff);
                        }
                    }
                }   
            }
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(bossPos, radius);
        
            // Draw Half Circle
            int segments = 32;
            float angleStep = 180f / segments;
            Vector3 forward = transform.forward;
            for (int i = 0; i < segments; i++)
            {
                float angleA = -90f + angleStep * i;
                float angleB = -90f + angleStep * (i + 1);
                Vector3 dirA = Quaternion.Euler(0, angleA, 0) * forward;
                Vector3 dirB = Quaternion.Euler(0, angleB, 0) * forward;
                Gizmos.DrawLine(bossPos + dirA * radius, bossPos + dirB * radius);
            }
        }
    }
}