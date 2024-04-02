using System.Collections;
using Photon.Pun;
using UnityEngine;
using SwordNShield.Combat.Attributes;
using SwordNShield.Combat.States;

namespace SwordNShield.Class.Warrior
{
    public class FlyingShield : MonoBehaviourPunCallbacks
    {
        private GameObject owner;
        private float damage;
        private PhotonView pv;
        private bool isMine;

        public void Play(GameObject attacker, float distance, float duration, float damage)
        {
            owner = attacker;
            pv = owner.GetComponent<PhotonView>();
            if (pv != null) isMine = pv.IsMine;
            this.damage = damage;
            StartCoroutine(Execute(distance, duration));
        }
        
        IEnumerator Execute(float distance, float duration)
        {
            Vector3 start = transform.position;
            Vector3 end = start + transform.up * distance;

            float currentTime = 0f;

            while (currentTime < duration)
            {
                transform.position = Vector3.Lerp(start, end, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
            Destroy(gameObject);
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            if (isMine) return;
            if (other.transform == owner.transform) return;
            Health health = other.GetComponent<Health>();
            StateScheduler stateScheduler = other.GetComponent<StateScheduler>();
            if (health != null)
            {
                health.GetDamage(owner, damage);
                stateScheduler.StartState(StateType.Stun, 0, 3f);
            }
        }
    }
}
