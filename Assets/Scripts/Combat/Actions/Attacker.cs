using System.Collections;
using Photon.Pun;
using SwordNShield.Combat.Attributes;
using UnityEngine;

namespace SwordNShield.Combat.Actions
{
    public class Attacker : MonoBehaviourPunCallbacks, IAction
    {
        private AttackType attackType;
        private bool canAttack;
        private bool canMove;
        private Rigidbody2D rigidBody2D;
        private Rotater rotater;
        private Stat stat;
        private ActionScheduler actionScheduler;
        private Coroutine AttackCoroutine;

        public bool CanAttack
        {
            get => canAttack;
            set => canAttack = value;
        }

        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            rotater = GetComponent<Rotater>();
            stat = GetComponent<Stat>();
            
            canMove = stat.MoveSpeed != 0;
            attackType = stat.AttackType;
            canAttack = true;
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public void StartAttack(Health target)
        {
            if (!canAttack) return;
            if (AttackCoroutine != null) Cancel();
            actionScheduler.StartAction(this);
            AttackCoroutine = StartCoroutine(Attack(target));
        }

        public void Cancel()
        {
            if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
        }

        private IEnumerator Attack(Health target)
        {
            while (target != null)
            {
                if (Vector2.Distance(transform.position, target.transform.position) <= stat.AttackRange)
                {
                    float angle = rotater.CalculateAngle(target.transform.position, transform.position);
                    rotater.Rotate(angle, stat.RotateSpeed);
                    if (canAttack)
                    {
                        photonView.RPC("PlayTriggerAnimation", RpcTarget.All, "attack");
                        if (target == null || target.IsDead()) Cancel();

                        switch (attackType)
                        {
                            case AttackType.Melee:
                                target.GetDamage(gameObject, stat.AttackDamage);
                                break;
                            
                            case AttackType.Range:
                                ThrowBullet(target);
                                break;
                        }
                        
                        StartCoroutine(AttackDelay());
                    }
                }
                else
                {
                    if (canMove)
                    {
                        float angle = rotater.CalculateAngle(target.transform.position, transform.position);
                        rotater.Rotate(angle, stat.RotateSpeed);
                        Vector2 direction = ((Vector2)target.transform.position - rigidBody2D.position).normalized;
                        rigidBody2D.velocity = direction * stat.MoveSpeed;
                    }
                }

                yield return null;
            }
        }

        private IEnumerator AttackDelay()
        {
            canAttack = false;
            yield return new WaitForSeconds(stat.AttackCoolTime);
            canAttack = true;
        }

        private void ThrowBullet(Health target)
        {
            photonView.RPC("ThrowBulletRPC", RpcTarget.All, target.photonView.ViewID);
        }
        [PunRPC]
        private void ThrowBulletRPC(int targetViewID)
        {
            PhotonView targetView = PhotonView.Find(targetViewID);
            GameObject instance = Instantiate(stat.Bullet, transform.position + stat.BulletPosition, transform.rotation);
            Health health = targetView.GetComponent<Health>();
            Bullet bullet = instance.GetComponent<Bullet>();
            bullet.Owner = gameObject;
            bullet.Damage = stat.AttackDamage;
            bullet.Speed = stat.BulletSpeed;
            bullet.FollowTarget(health);
        }
    }
}