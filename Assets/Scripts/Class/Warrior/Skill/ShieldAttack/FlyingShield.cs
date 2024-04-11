using System.Collections;
using Photon.Pun;
using UnityEngine;
using SwordNShield.Combat.Attributes;
using SwordNShield.Combat.States;

namespace SwordNShield.Class.Warrior
{
    public class FlyingShield : MonoBehaviourPunCallbacks
    {
        public PhotonView owner;
        private float damage;
        private bool isMine;

        public void Play(PhotonView attacker, float distance, float duration, float damage)
        {
            owner = attacker;
            isMine = owner.IsMine;
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
            StateScheduler stateScheduler = other.GetComponentInChildren<StateScheduler>();
            if (health != null) health.GetDamage(owner.gameObject, damage);
            if(stateScheduler != null) stateScheduler.StartState(StateType.Stun, 0, 3f);
        }
    }
}
