
using StateMachine;
using Stats;
using UnityEngine;
using UnityEngine.Rendering;

namespace Boss
{
    public class BossIdleState : BossBaseState
    {
        private readonly int locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int speedHash = Animator.StringToHash("Speed");
        private const float CrossFadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        private const float IdleDuration = 1f;
        private float stateEnterTime;
        public BossIdleState(BossStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Idle State");
            StateMachine.Animator.CrossFadeInFixedTime(locomotionHash, CrossFadeDuration);
            stateEnterTime = Time.time;
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.Animator.SetFloat(speedHash, 0f, AnimatorDampTime, deltaTime);

            TurnToPlayer();
            if (!IsPlayerInRange()) // and Player Not Dead
            {
                StateMachine.SwitchState(StateMachine.ChaseState);
            }
            
            if (Time.time - stateEnterTime < IdleDuration)
            {
                return;
            }
            
            if (IsPlayerInRange()) // and Player Not Dead
            {
                StateMachine.SwitchState(StateMachine.AttackState);
                return;
            }
        }

        public override void Exit() { }
    }

    public class BossWakeState : BossBaseState
    {
        public BossWakeState(BossStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Wake State");
            throw new System.NotImplementedException();
        }

        public override void Tick(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
    }

    public class BossChaseState : BossBaseState
    {
        private readonly int locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int speedHash = Animator.StringToHash("Speed");
        private const float CrossFadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        public BossChaseState(BossStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Chase State");
            StateMachine.Animator.CrossFadeInFixedTime(locomotionHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (IsPlayerInRange())
            {
                StateMachine.SwitchState(StateMachine.IdleState);
                return;
            }

            TurnToPlayer();
            MoveToPlayer(deltaTime);
            
            StateMachine.Animator.SetFloat(speedHash, 1f, AnimatorDampTime, deltaTime);
        }

        public override void Exit()
        {
            StateMachine.NavMeshAgent.ResetPath();
            StateMachine.NavMeshAgent.velocity = Vector3.zero;
        }

        private void MoveToPlayer(float deltaTime)
        {
            if (StateMachine.Player)
            {
                StateMachine.NavMeshAgent.destination = StateMachine.Player.transform.position;
                Move(StateMachine.NavMeshAgent.desiredVelocity.normalized * StateMachine.MovementSpeed, deltaTime);
            }

            StateMachine.NavMeshAgent.velocity = StateMachine.CharacterController.velocity;
        }
    }

    public class BossAttackState : BossBaseState
    {
        public BossAttackState(BossStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Attack State");
            StateMachine.BossSkillController.ActivateSkill();
        }

        public override void Tick(float deltaTime)
        {
            if (!StateMachine.BossSkillController.IsSkillActive)
            {
                StateMachine.SwitchState(StateMachine.IdleState);
            }
        }

        public override void Exit() { }
    }

    public class BossDeathState : BossBaseState
    {
        public BossDeathState(BossStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Tick(float deltaTime) { }

        public override void Exit() { }
    }

}
