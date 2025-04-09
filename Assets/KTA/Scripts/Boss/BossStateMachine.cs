using System;
using Boss.Skills;
using UnityEngine;
using UnityEngine.AI;

namespace Boss
{
    public class BossStateMachine : StateMachine.StateMachine
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public BossSkillController  BossSkillController { get; private set; }
        [field: SerializeField] public float PlayerDetectRange { get; private set; } = 10f;
        [field: SerializeField] public float MovementSpeed { get; private set; } = 1f;
        [field: SerializeField] public GameObject Player { get;  private set; } // TODO : Need Change in Multiplayer
        
        public BossIdleState IdleState { get; private set; }
        public BossWakeState WakeState  { get; private set; }
        public BossChaseState ChaseState { get; private set; }
        public BossAttackState AttackState { get; private set; }

        public BossDeathState DeathState { get; private set; }
        
        private void Start()
        {
            IdleState = new BossIdleState(this);
            WakeState = new BossWakeState(this);
            ChaseState = new BossChaseState(this);
            AttackState = new BossAttackState(this);
            DeathState = new BossDeathState(this);
            
            // Get Player
            
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;
            
            //SwitchState(WakeState);
            SwitchState(IdleState);
        }

        private void OnEnable()
        {
            // TODO : Subscribe OnDeathMessage to Death
        }

        private void OnDisable()
        {
            // TODO : Unsubscribe OnDeathMessage to Death
        }

        private void OnDeathMessage()
        {
            SwitchState(DeathState);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, PlayerDetectRange);
        }
    }
}