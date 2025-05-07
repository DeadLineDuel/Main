using KTA.Test;
using Unity.Netcode;
using UnityEngine;

namespace KTA.Scripts
{
    public class TESTPlayerSkill_NormalAttack : TESTPlayerSkill
    {
        private static readonly int NormalAttack = Animator.StringToHash("NormalAttack");
        [field: SerializeField] private ParticleSystem skillEffectParticle;

        [field: Header("Skill Data")]
        [field: SerializeField] private float radius;

        private Vector3 PlayerPos;
        
        public override void ActivateSkill()
        {
            PlayerCore.NavMeshAgent.ResetPath();
            PlayerCore.NetworkAnimator.Animator.SetBool(Move, false);
            PlayerCore.NetworkAnimator.SetTrigger(NormalAttack);
            
            PlayerPos = PlayerCore.gameObject.transform.position;
        }

        public override void PlayEffect()
        {
            if (!IsOwner) return;
            skillEffectParticle.Play();
        }

        
        [ServerRpc]
        public override void SkillHitServerRpc(ServerRpcParams rpcParams = default)
        {
            if (!IsServer) return;  // On Server
            
            int layerMask = LayerMask.GetMask("Boss");
            var size = Physics.OverlapSphereNonAlloc(PlayerPos, radius, Colliders, layerMask);
            
            Vector3 forward = transform.forward;
            
            if (size > 0)
            {
                for  (int i = 0; i < size; i++) // Do not use foreach on NonAlloc
                {
                    Vector3 dir = (Colliders[i].transform.position - PlayerPos).normalized;
                    float angle = Vector3.Angle(forward, dir);
                    if (angle <= 60f) // half circle
                    {
                        Debug.Log("[Player] Hit Object : " + Colliders[i].gameObject.name);
                        if (Colliders[i].TryGetComponent<IDamageable>(out var damageable))
                        {
                            damageable.TakeDamage(5f);
                        }
                    }
                }   
            }
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PlayerPos, radius);
        
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
                Gizmos.DrawLine(PlayerPos + dirA * radius, PlayerPos+ dirB * radius);
            }
        }
        
    }
}
