using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using SwordNShield.Controller;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Attributes;
using SwordNShield.Combat.Skills;

namespace SwordNShield.Class.Warrior
{
    public class ShieldAttack : MonoBehaviourPunCallbacks, ISkill
    {
        public PlayerController Owner { get; set; }
        public event EventHandler PlaySkill;
        [SerializeField] private GameObject ShieldEffect;
        [SerializeField] private GameObject ShieldFire;
        [SerializeField] private AnimationClip animationClip;
        [SerializeField] private GameObject indicator;
        [SerializeField] private Sprite skillImage;
        [SerializeField] private KeyCode keyCode;
        [SerializeField] private float coolTime;
        [SerializeField] private float actionTime;
        [SerializeField] private float damage;
        [SerializeField] private float distance;
        [SerializeField] private float duration;

        private AnimationController animationController;
        private Rotater rotater;
        private Health health;
        private Attacker attacker;
        private bool canExecute = true;
        private bool isPlaying;

        void Awake()
        {
            animationController = GetComponent<AnimationController>();
            health = GetComponent<Health>();
            attacker = GetComponent<Attacker>();
            rotater = GetComponent<Rotater>();
            health.OnDamageReceived += DefendForward;
            SpriteRenderer indicatorSprite = indicator.GetComponent<SpriteRenderer>();
            indicatorSprite.size = new Vector2(indicatorSprite.size.x, distance);
        }

        public KeyCode GetKeyCode
        {
            get => keyCode;
            set => keyCode = value;
        }

        public Sprite SkillSprite => skillImage;
        public GameObject Indicator => indicator;
        public bool CanExecute => canExecute;
        public bool IsPlaying => isPlaying;
        public float CoolTime => coolTime;
        public float ActionTime => actionTime;

        public void Play(Vector2? position)
        {
            if (!canExecute) return;
            PlaySkill!.Invoke(this, EventArgs.Empty);
            Vector2 direction = (Vector2)position! - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            StartCoroutine(ExecuteShieldAttack(angle));
            Owner.photonView.RPC("ExecuteShieldAttack", RpcTarget.All, angle);
        }

        [PunRPC]
        public IEnumerator ExecuteShieldAttack(float angle)
        {
            animationController.StartAnimation(animationClip);
            rotater.CanRotate = false;
            attacker.CanAttack = false;
            canExecute = false;
            isPlaying = true;
            health.CanGetDamage = false;
            ShieldEffect.SetActive(true);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            GameObject instance = Instantiate(ShieldFire, transform.position, transform.rotation);
            FlyingShield flyingShield = instance.GetComponent<FlyingShield>();
            flyingShield.Play(gameObject, distance, duration, damage);
            yield return new WaitForSeconds(actionTime);
            isPlaying = false;
            health.CanGetDamage = true;
            rotater.CanRotate = true;
            attacker.CanAttack = true;
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
