using Photon.Pun;
using UnityEngine;
using SwordNShield.Stats;

namespace SwordNShield.Attributes
{
    public class Health : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Stat stat;
        private float healthPoints;
        private float maxHP;
        public delegate void DamageReceived(GameObject attacker, float damage);
        public event DamageReceived OnDamageReceived;
        private bool canGetDamage;
        public float HP => healthPoints;
        public float MaxHP => maxHP;

        private void Awake()
        {
            maxHP = stat.HP;
            healthPoints = maxHP;
            canGetDamage = true;
        }

        public bool CanGetDamage
        {
            get => canGetDamage;
            set => canGetDamage = value;
        }
        public bool IsDead()
        {
            return healthPoints <= 0;
        }
        
        public void GetDamage(GameObject attacker, float damage)
        {
            if (!canGetDamage)
            {
                OnDamageReceived?.Invoke(attacker, damage);
                return;
            }
            photonView.RPC("PunGetDamage", RpcTarget.All, damage);
            if (IsDead())
            {
                AwardExperience(attacker);
            }
        }

        [PunRPC]
        private void PunGetDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            DamageTextManager.Instance.GetDamageText(transform.position, damage);
        }

        public void AwardExperience(GameObject attacker)
        {
            Experience experience = attacker.GetComponent<Experience>();
            if (experience == null) return;
            
        }
    }
}