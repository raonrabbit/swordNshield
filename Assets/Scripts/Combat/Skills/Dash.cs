using System;
using UnityEngine;
using System.Collections;
using SwordNShield.Controller;

namespace SwordNShield.Combat.Skills
{
    public class Dash : MonoBehaviour, ISkill
    {
        public PlayerController Owner { get; set; }
        
        public event EventHandler PlaySkill;
        [SerializeField] private Sprite skillImage;
        [SerializeField] private KeyCode keyCode = KeyCode.F;
        [SerializeField] private float coolTime = 3f;
        [SerializeField] private float actionTime = 0.5f;
        [SerializeField] private float dashSpeed = 10f;

        private bool canExecute = true;
        private bool isDashing;
        private Vector2 dashDirection;
        private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public KeyCode GetKeyCode
        {
            get => keyCode;
            set => keyCode = value;
        }

        public Sprite SkillSprite => skillImage;
        public bool CanExecute => canExecute;
        public bool IsPlaying => isDashing;
        public float CoolTime => coolTime;
        public float ActionTime => actionTime;

        //Dash 실행
        public void Play()
        {
            StartCoroutine(Execute());
        }
        
        public IEnumerator Execute()
        {
            if (!canExecute) yield break;
            PlaySkill!.Invoke(this, EventArgs.Empty);
            Owner.GetMover.CanMove = false;
            dashDirection = (Owner.GetMouseRay() - (Vector2)transform.position).normalized;
            float startTime = Time.time;
            canExecute = false;
            isDashing = true;
            while(Time.time - startTime < actionTime){
                Owner.GetActionScheduler.CancelCurrentAction();
                rigidbody2D.velocity = dashDirection * dashSpeed;
                yield return null;
            }
            rigidbody2D.velocity = Vector2.zero;
            Owner.GetMover.CanMove = true;
            isDashing = false;
            yield return new WaitForSeconds(coolTime);
            canExecute = true;
        }
    }
}
