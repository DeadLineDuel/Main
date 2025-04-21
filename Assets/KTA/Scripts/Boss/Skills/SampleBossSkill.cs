using UnityEditor;
using UnityEngine;

namespace Boss.Skills
{
    public class SampleBossSkill : BossSkill
    {
        [field: SerializeField] private float radius;
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
            var size = Physics.OverlapSphereNonAlloc(TargetPosition, radius, Colliders);
            
            Vector3 forward = transform.forward;

            if (size > 0)
            {
                foreach (var col in Colliders)
                {
                    Vector3 dir = (col.transform.position - TargetPosition).normalized;
                    float angle = Vector3.Angle(forward, dir);
                    if (angle <= 90f) // 180도(반원) 이내
                    {
                        Debug.Log(col.gameObject.name);
                    }
                }   
            }
        }

        protected override void PerformParticle()
        {
            throw new System.NotImplementedException();
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(TargetPosition, radius);

            // 반원(180도) 외곽선 그리기 예시
            int segments = 32;
            float angleStep = 180f / segments;
            Vector3 forward = transform.forward;
            for (int i = 0; i < segments; i++)
            {
                float angleA = -90f + angleStep * i;
                float angleB = -90f + angleStep * (i + 1);
                Vector3 dirA = Quaternion.Euler(0, angleA, 0) * forward;
                Vector3 dirB = Quaternion.Euler(0, angleB, 0) * forward;
                Gizmos.DrawLine(TargetPosition + dirA * radius, TargetPosition + dirB * radius);
            }
        }
    }
}