using System.Collections;
using SwordNShield.Function;
using UnityEngine;

namespace SwordNShield.Movement
{
    public class Mover : MonoBehaviour, Function.IAction
    {
        [SerializeField] private Rigidbody2D rigidbody2D;
        [SerializeField] private Animator animator;
        private ActionScheduler actionScheduler;
        private Coroutine MoveTowards = null;
        private bool canMove;
        
        void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            canMove = true;
        }

        void Update()
        {
            UpdateAnimation();
        }

        public bool CanMove
        {
            set => canMove = value;
        }
        
        public void StartMoveAction(Vector2 destination, float speed)
        {
            if (!canMove) return;
            if(MoveTowards!=null) StopCoroutine(MoveTowards);
            actionScheduler.StartAction(this);
            MoveTowards = StartCoroutine(MoveTo(destination, speed));
        }
        public IEnumerator MoveTo(Vector2 destination, float speed)
        {
            while (Vector2.Distance(rigidbody2D.position, destination) > 0.1f)
            {
                Vector2 direction = (destination - rigidbody2D.position).normalized;
                rigidbody2D.velocity = direction * speed;
                yield return null;
            }
            Cancel();
        }
        

        public void Cancel()
        {
            rigidbody2D.velocity = Vector2.zero;
            if (MoveTowards == null) return;
            StopCoroutine(MoveTowards);
        }

        private void UpdateAnimation()
        {
            animator.SetBool("isMoving", rigidbody2D.velocity!=Vector2.zero);
        }
    }
}