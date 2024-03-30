using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using SwordNShield.Attributes;
using SwordNShield.Combat;
using SwordNShield.Movement;
using SwordNShield.Combat.Skills;
using SwordNShield.Function;
using SwordNShield.UI;

namespace SwordNShield.Controller
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        private Stat stat;
        private Health health;
        private Attacker attacker;
        private Mover mover;
        private Rotater rotater;
        private List<ISkill> playerSkills;
        public ActionScheduler actionScheduler;
        private SkillScheduler skillScheduler;
        static public event Action OnDeath ;
        
        private float raycastRadius;
        private float speed;
        private float rotateSpeed;

        public Mover GetMover => mover;
        private void Awake()
        {
            stat = GetComponent<Stat>();
            health = GetComponent<Health>();
            attacker = GetComponent<Attacker>();
            mover = GetComponent<Mover>();
            rotater = GetComponent<Rotater>();
            playerSkills = GetComponent<PlayerSkills>().GetPlayerSkills();
            actionScheduler = GetComponent<ActionScheduler>();
            skillScheduler = GetComponent<SkillScheduler>();
            speed = stat.MoveSpeed;
            rotateSpeed = stat.RotateSpeed;
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
            foreach (ISkill skill in playerSkills)
            {
                if (Input.GetKeyDown(skill.GetKeyCode))
                {
                    skillScheduler.StartSkill(skill);
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
                rotater.StartRotateAction(angle, rotateSpeed);
                mover.StartMoveAction(target, speed);
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