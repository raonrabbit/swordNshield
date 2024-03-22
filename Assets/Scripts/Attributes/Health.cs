using UnityEngine;
using UnityEngine.Events;
using SwordNShield.Stats;

namespace SwordNShield.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField]private float healthPoints = 200f;
        [SerializeField] private UnityEvent onDie;

        public float HP() => healthPoints;
        public bool IsDead()
        {
            return healthPoints <= 0;
        }
        
        public void GetDamage(GameObject attacker, float damage)
        {
            healthPoints -= Mathf.Max(healthPoints - damage, 0);
            if (IsDead())
            {
                onDie.Invoke();
                AwardExperience(attacker);
            }
        }

        private void AwardExperience(GameObject attacker)
        {
            Experience experience = attacker.GetComponent<Experience>();
            if (experience == null) return;
            
        }
    }
}