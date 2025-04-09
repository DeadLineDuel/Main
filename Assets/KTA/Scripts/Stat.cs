using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineRunner;
using UnityEngine.Events;

namespace Stats
{
    public class Stat
    {
        public float BaseStat { get; private set; }
        public float CurrentStat { get; private set; }
        
        private Dictionary<StatModifier.StatOperator, List<StatModifier>> activeModifiers = new Dictionary<StatModifier.StatOperator, List<StatModifier>>();
        
        public Stat(float value)
        {
            activeModifiers.Add(StatModifier.StatOperator.Plus, new List<StatModifier>());
            activeModifiers.Add(StatModifier.StatOperator.Multiply, new List<StatModifier>());
            activeModifiers.Add(StatModifier.StatOperator.Overwrite, new List<StatModifier>());
            
            BaseStat = value;
            CurrentStat = value;
        }

        public void SetBaseStat(float value)
        {
            BaseStat = value;
            CurrentStat = CalcCurrentStat();
        }
        
        public void ApplyModifier(StatModifier modifier)
        {
            AddModifier(modifier);
        }

        public void ApplyModifier(StatModifier modifier, float duration)
        {
            CoroutineRunner.CoroutineRunner.Instance.StartCoroutine(CoroutineModifier(modifier, duration));
        }

        private IEnumerator CoroutineModifier(StatModifier modifier, float duration)
        {
            AddModifier(modifier);
            yield return new WaitForSeconds(duration);
            RemoveModifier(modifier);
        }
        
        private void AddModifier(StatModifier modifier)
        {
            if (!activeModifiers.ContainsKey(modifier.op))
            {
                activeModifiers[modifier.op] = new List<StatModifier>();
            }
            activeModifiers[modifier.op].Add(modifier);
            CurrentStat = CalcCurrentStat();
        }

        public void RemoveModifier(StatModifier modifier)
        {
            if (activeModifiers.ContainsKey(modifier.op))
            {
                activeModifiers[modifier.op].Remove(modifier);
                CurrentStat = CalcCurrentStat();
            }
        }

        private float CalcCurrentStat()
        {
            float result = BaseStat;
            
            if (activeModifiers[StatModifier.StatOperator.Overwrite].Count > 0)
            {
                result = activeModifiers[StatModifier.StatOperator.Overwrite]
                    [activeModifiers[StatModifier.StatOperator.Overwrite].Count - 1].value;

                return result;
            }

            foreach (var modifier in activeModifiers[StatModifier.StatOperator.Plus])
            {
                result += modifier.value;
            }

            foreach (var modifier in activeModifiers[StatModifier.StatOperator.Multiply])
            {
                result *= modifier.value;
            }

            return result;
        }
    }

    [System.Serializable]
    public class StatModifier
    {
        public enum StatOperator
        {
            Plus,
            Multiply,
            Overwrite
        }

        public StatOperator op {get; private set;}
        public float value {get; private set;}

        public StatModifier(StatOperator op, float value)
        {
            this.op = op;
            this.value = value;
        }
    }
}