using System.Collections;
using Photon.Pun;
using SwordNShield.Combat.Attributes;
using UnityEngine;

namespace SwordNShield.Combat.Actions
{
    public class Bullet : MonoBehaviour
    {
        private GameObject owner;
        private Health target;
        private float damage;
        private float speed;

        public GameObject Owner
        {
            set => owner = value;
        }

        public float Damage
        {
            set => damage = value;
        }
        public float Speed
        {
            set => speed = value;
        }

        public void FollowTarget(Health target)
        {
            this.target = target;
            StartCoroutine(FollowTargetCoroutine());
        }
        public IEnumerator FollowTargetCoroutine()
        {
            while (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                yield return null;
            }
            if(gameObject != null) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (target == null) return;
            if (other.gameObject == target.gameObject)
            {
                if(target != null && owner.GetPhotonView().IsMine) target.GetDamage(owner, damage);
                if(gameObject != null) Destroy(gameObject);
            }
        }
    }   
}
