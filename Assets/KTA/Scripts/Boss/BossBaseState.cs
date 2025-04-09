using StateMachine;
using UnityEngine;

namespace Boss
{
    public abstract class BossBaseState : State
    {
        protected BossStateMachine StateMachine;

        public BossBaseState(BossStateMachine stateMachine)
        {
            this.StateMachine = stateMachine;
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            StateMachine.CharacterController.Move(motion * deltaTime);
        }

        protected void Turn(Vector3 lookPos)
        {
            lookPos.y = 0;
            // 현재 오브젝트의 방향
            Quaternion currentRotation = StateMachine.transform.rotation;
    
            // 목표 방향
            Quaternion targetRotation = Quaternion.LookRotation(lookPos);
    
            // 회전 속도 (초당 각도)
            float rotationSpeed = 120.0f;
            
            StateMachine.transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        protected void TurnToPlayer()
        {
            if (StateMachine.Player)
            {
                Turn(StateMachine.Player.transform.position - StateMachine.transform.position);                
            }
        }
        
        protected bool IsPlayerInRange()
        {
            if (!StateMachine.Player) return false;
            float distSqr = (StateMachine.Player.transform.position - StateMachine.transform.position).sqrMagnitude;
            
            return distSqr <= StateMachine.PlayerDetectRange * StateMachine.PlayerDetectRange;
        }
    }
}