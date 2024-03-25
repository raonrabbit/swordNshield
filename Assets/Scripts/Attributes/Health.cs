using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using SwordNShield.Stats;

namespace SwordNShield.Attributes
{
    public class Health : MonoBehaviourPunCallbacks
    {
        [SerializeField]private float healthPoints = 200f;

        public float HP => healthPoints;
        public bool IsDead()
        {
            return healthPoints <= 0;
        }
        
        public void GetDamage(GameObject attacker, float damage)
        {
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
        }

        private void AwardExperience(GameObject attacker)
        {
            Experience experience = attacker.GetComponent<Experience>();
            if (experience == null) return;
            
        }
    }
}