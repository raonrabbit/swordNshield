using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using SwordNShield.Attributes;
using SwordNShield.Combat;
using SwordNShield.Movement;
using SwordNShield.Combat.Skills;
using SwordNShield.Function;

namespace SwordNShield.Controller
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        private Health health;
        private Mover mover;
        private Rotater rotater;
        private List<ISkill> playerSkills;
        public ActionScheduler actionSheduler;
        
        [SerializeField] private float raycastRadius = 1f;
        [SerializeField] private float speed = 1f;
        
        private void Awake()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            rotater = GetComponent<Rotater>();
            playerSkills = GetComponent<PlayerSkills>().GetPlayerSkills();
            actionSheduler = GetComponent<ActionScheduler>();
        }
        private void Start()
        {
            if (!photonView.IsMine) this.enabled = false;
        }
        
        public Mover GetMover => mover;
        public Health GetHealth => health;
        public ActionScheduler GetActionScheduler => actionSheduler;

        private void Update()
        {
            if (health.IsDead())
            {
                Function.CursorManager.Instance.SetCursor(CursorType.Default);
                return;
            }
            UseSkills();
        }

        private void FixedUpdate()
        {
            PlayerMovement();
        }
        
        private void UseSkills()
        {
            foreach (ISkill skill in playerSkills)
            {
                if (Input.GetKeyDown(skill.GetKeyCode))
                {
                    skill.Play();
                }
            }
        }

        //Player 이동
        private void PlayerMovement()
        {
            if (Input.GetMouseButton(1))
            {
                Vector2 target = RaycastTarget();
                rotater.StartRotateAction(target);
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