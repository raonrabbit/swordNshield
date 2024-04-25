using System;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Attributes;
using SwordNShield.Combat.Skills;
using SwordNShield.UI;

namespace SwordNShield.Controller
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private SkillScheduler skillScheduler;
        [SerializeField] private ActionScheduler actionScheduler;
        private Stat stat;
        private Health health;
        private Attacker attacker;
        private Mover mover;
        private Rotater rotater;
        private List<Skill> playerSkills;
        static public event Action OnDeath ;
        
        private float raycastRadius;
        private float speed;
        private float rotateSpeed;
        private bool defaultRotation;
        private bool defaultMoving;

        public bool DefaultRotation
        {
            get => defaultRotation;
            set => defaultRotation = value;
        }
        public bool DefaultMoving
        {
            get => defaultMoving;
            set => defaultMoving = value;
        }

        public Mover GetMover => mover;
        private void Awake()
        {
            stat = GetComponent<Stat>();
            health = GetComponent<Health>();
            attacker = GetComponent<Attacker>();
            mover = GetComponent<Mover>();
            rotater = GetComponent<Rotater>();
            actionScheduler = GetComponent<ActionScheduler>();
            playerSkills = skillScheduler.GetPlayerSkills();
            speed = stat.MoveSpeed;
            rotateSpeed = stat.RotateSpeed;
            defaultRotation = true;
            defaultMoving = true;
        }
        private void Start()
        {
            if (!photonView.IsMine) this.enabled = false;
        }
        public ActionScheduler GetActionScheduler => actionScheduler;

        private void Update()
        {
            if (health.IsDead())
            {
                OnDeath?.Invoke();
                CursorManager.Instance.SetCursor(CursorType.Default);
                PhotonNetwork.Destroy(gameObject);
                return;
            }
            UseSkills();
            if (PlayerDefaultAttack()) return;
            PlayerMovement();
        }

        private void FixedUpdate()
        {
            
        }
        
        private void UseSkills()
        {
            foreach (Skill skill in playerSkills)
            {
                if (Input.GetKeyDown(skill.GetKeyCode))
                {
                    StartCoroutine(skillScheduler.StartSkill(skill));
                }
            }
        }

        private bool PlayerDefaultAttack()
        {
            if (Input.GetMouseButton(1))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(GetMouseRay(), Vector2.zero);
                foreach (var hit in hits)
                {
                    if (hit.transform == this.transform) return false;
                    Health target = hit.transform.GetComponent<Health>();
                    if (target == null) continue;
                    attacker.StartAttack(target);
                    return true;
                }
            }

            return false;
        }

        //Player 이동
        private void PlayerMovement()
        {
            if (Input.GetMouseButton(1))
            {
                CursorManager.Instance.SetCursor(CursorType.Default);
                Vector2 target = RaycastTarget();
                float angle = rotater.CalculateAngle(target, transform.position);
                rotater.Rotate(angle, rotateSpeed);
                if(defaultMoving) mover.StartMoveAction(target, speed);
            }
        }

        private Vector2 RaycastTarget()
        {
            RaycastHit2D hit = Physics2D.Raycast(GetMouseRay(), Vector2.zero);
            Vector2 target = hit.point;
            return target;
        }

        public Vector2 GetMouseRay()
        {
            return Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}