using System;
using System.Collections;
using Photon.Pun;
using SwordNShield.Combat;
using UnityEngine;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Attributes;
using SwordNShield.Combat.Skills;

namespace SwordNShield.Class.Warrior
{
    public class DamageReflection : Skill
    {
        [Header("Unique Settings")]
        [SerializeField] private GameObject ShieldEffect;
        [SerializeField] private float reflectDamage;
        [SerializeField] private Health health;
        [SerializeField] private Rotater rotater;
        private Rigidbody2D rigidbody;

        void Awake()
        {
            canExecute = true;
            health.OnDamageReceived += DefendForward;
        }
        
        public override void Play(Target target)
        {
            Vector2 direction = target.VectorTarget;
            if (!canExecute) return;
            if (photonView.IsMine) InvokeEvent();
            //Vector2 direction = (Vector2)position! - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
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
            float startTime = Time.time;
            while (Time.time - startTime <= actionTime)
            {
                Owner.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                yield return null;
            }
            //yield return new WaitForSeconds(actionTime);
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
            else
            {
                photonView.RPC("PunGetDamage", RpcTarget.All, (float)Math.Truncate(damage / 3));
                Health attackerHealth = attacker.GetComponent<Health>();
                if (attackerHealth == null) return;
                attackerHealth.GetDamage(gameObject, reflectDamage);
                
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
