using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Boss.Skills
{
    public class BossSkill_Slash : BossSkill
    {
        [field: SerializeField] private float radius;
        [field: SerializeField] private ParticleSystem hitParticle;
        [field: SerializeField] private ParticleSystem skillEffectParticle;
        public override void Perform(Vector3 targetPosition)
        {
            this.TargetPosition = targetPosition;
            gameObject.transform.position = targetPosition;
            PerformIndicator();
        }

        protected override void PerformIndicator()
        {
            SkillIndicator.ActivateIndicator(TargetPosition, 180f, 0.5f);
        }

        protected override void OnIndicatorComplete()
        {
            PerformCollider();
            PerformParticle();
        }

        protected override void PerformCollider()
        {
            int layerMask = LayerMask.GetMask("Player");
            var size = Physics.OverlapSphereNonAlloc(TargetPosition, radius, Colliders, layerMask);
            
            Vector3 forward = transform.forward;

            Debug.Log(size);
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    var col = Colliders[i];
                    Debug.Log(col);
                    Vector3 dir = (col.transform.position - TargetPosition).normalized;
                    float angle = Vector3.Angle(forward, dir);
                    if (angle <= 90f) // 180도(반원) 이내
                    {
                        Instantiate(hitParticle, col.bounds.center, Quaternion.identity);
                    }
                }
            }
        }

        protected override void PerformParticle()
        {
            skillEffectParticle.Play();
        }
    }
}