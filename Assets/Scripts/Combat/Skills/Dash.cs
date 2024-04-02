using System;
using UnityEngine;
using System.Collections;
using Photon.Pun;
using SwordNShield.Combat.Actions;
using SwordNShield.Controller;

namespace SwordNShield.Combat.Skills
{
    public class Dash : MonoBehaviour, ISkill
    {
        public PlayerController Owner { get; set; }
        
        public event EventHandler PlaySkill;
        [SerializeField] private Sprite skillImage;
        [SerializeField] private GameObject indicator;
        [SerializeField] private KeyCode keyCode = KeyCode.F;
        [SerializeField] private float coolTime = 3f;
        [SerializeField] private float actionTime = 0.5f;
        [SerializeField] private float dashSpeed = 10f;

        private bool canExecute = true;
        private bool isDashing;
        private Vector2 dashDirection;
        private Rigidbody2D rigidBody2D;
        private Mover mover;
        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            mover = GetComponent<Mover>();
        }

        public KeyCode GetKeyCode
        {
            get => keyCode;
            set => keyCode = value;
        }

        public Sprite SkillSprite => skillImage;
        public GameObject Indicator => indicator;
        public bool CanExecute => canExecute;
        public bool IsPlaying => isDashing;
        public float CoolTime => coolTime;
        public float ActionTime => actionTime;

        //Dash 실행
        public void Play(Vector2? _)
        {
            if (!canExecute) return;
            PlaySkill!.Invoke(this, EventArgs.Empty);
            dashDirection = (Owner.GetMouseRay() - (Vector2)transform.position).normalized;
            StartCoroutine(ExecuteDash(dashDirection));
            Owner.photonView.RPC("ExecuteDash", RpcTarget.All, dashDirection);
        }
        
        [PunRPC]
        public IEnumerator ExecuteDash(Vector2 dashDirection)
        {
            mover.CanMove = false;
            float startTime = Time.time;
            canExecute = false;
            isDashing = true;
            while(Time.time - startTime < actionTime){
                Owner.GetActionScheduler.CancelCurrentAction();
                rigidBody2D.velocity = dashDirection * dashSpeed;
                yield return null;
            }
            rigidBody2D.velocity = Vector2.zero;
            mover.CanMove = true;
            isDashing = false;
            yield return new WaitForSeconds(coolTime);
            canExecute = true;
        }
    }
}
