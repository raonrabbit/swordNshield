using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Attributes;
using SwordNShield.Combat.Skills;

namespace SwordNShield.Class.Warrior
{
    public class ShieldAttack : Skill
    {
        [Header("Unique Settings")]
        [SerializeField] private GameObject ShieldEffect;
        [SerializeField] private GameObject ShieldFire;
        [SerializeField] private float damage;
        [SerializeField] private float distance;
        [SerializeField] private float duration;
        [SerializeField] private Health health;
        [SerializeField] private Rotater rotater;
        [SerializeField] private Attacker attacker;
        private Rigidbody2D rigidbody;

        void Awake()
        {
            canExecute = true;
            health.OnDamageReceived += DefendForward;
            SpriteRenderer indicatorSprite = indicator.GetComponent<SpriteRenderer>();
            indicatorSprite.size = new Vector2(indicatorSprite.size.x, distance);
        }
        
        public override void Play(Vector2? position)
        {
            if (!canExecute) return;
            if (photonView.IsMine) InvokeEvent();
            Vector2 direction = (Vector2)position! - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            StartCoroutine(ExecuteCoroutine(angle));
            //photonView.RPC("ExecuteShieldAttack", RpcTarget.All, angle);
        }

        [PunRPC]
        public void ExecuteShieldAttack(float angle)
        {
            StartCoroutine(ExecuteCoroutine(angle));
        }
        public IEnumerator ExecuteCoroutine(float angle)
        {
            animationController.StartAnimation(animationClip);
            rotater.CanRotate = false;
            canExecute = false;
            isPlaying = true;
            health.CanGetDamage = false;
            ShieldEffect.SetActive(true);
            Owner.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            GameObject instance = Instantiate(ShieldFire, Owner.transform.position, Owner.transform.rotation);
            FlyingShield flyingShield = instance.GetComponent<FlyingShield>();
            flyingShield.Play(photonView, distance, duration, damage);
            yield return new WaitForSeconds(actionTime);
            isPlaying = false;
            health.CanGetDamage = true;
            rotater.CanRotate = true;
            ShieldEffect.SetActive(false);
            yield return new WaitForSeconds(coolTime);
            canExecute = true;
        }

        private void DefendForward(GameObject attacker, float damage)
        {
            if (!isPlaying) return;
            if (!FaceToOther(attacker.transform.position))
            {
                photonView.RPC("PunGetDamage", RpcTarget.All, damage);
                if (health.IsDead())
                {
                    health.AwardExperience(attacker);
                }
            }
        }

        public bool FaceToOther(Vector2 otherPosition)
        {
            Vector2 directionToOther = (otherPosition - (Vector2)transform.position).normalized;
            if (Vector2.Angle(transform.up, directionToOther) < 80f)
            {
                return true;
            }

            return false;
        }
    }
}
