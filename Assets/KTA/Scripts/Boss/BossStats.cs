using UnityEngine;

namespace Stats.Boss
{
    public class BossStats : MonoBehaviour
    {
        [SerializeField] private BossInitStatData initStatData;

        public Stat MaxHealth;
        public Stat Atk;
        public Stat Speed;
        public Stat AtkSpeed;
        public float Health {private set; get;}
        
        public bool IsDead => Health <= 0f;
        
        private void Start()
        {
            MaxHealth = new Stat(initStatData.MaxHealth);
            Atk = new Stat(initStatData.Atk);
            Speed = new Stat(initStatData.Speed);
            AtkSpeed = new Stat(initStatData.AtkSpeed);
            Health = MaxHealth.CurrentStat;
        }
    }
}