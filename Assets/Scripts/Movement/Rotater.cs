using System.Collections;
using SwordNShield.Combat;
using SwordNShield.Function;
using UnityEngine;

namespace SwordNShield.Movement
{
    public class Rotater : MonoBehaviour, Function.IAction
    {
        private Rigidbody2D rigidbody2D;
        private Animator animator;
        private ActionScheduler actionScheduler;
        private Coroutine RotateTowards = null;
        private bool isRotating;
        private bool canRotate;
        
        void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            isRotating = false;
            canRotate = true;
        }

        void Update()
        {
            UpdateAnimation();
        }

        public bool CanRotate
        {
            set => canRotate = value;
        }

        public void StartRotateAction(float angle, float speed)
        {
            if(RotateTowards!=null) StopCoroutine(RotateTowards);
            RotateTowards = StartCoroutine(Look(angle, speed));
        }
        
        private IEnumerator Look(float angle, float speed){
            //Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            float angleDifference = Mathf.DeltaAngle(rigidbody2D.rotation, angle);
            while (Mathf.Abs(angleDifference) > 1.0f)
            {
                isRotating = true;
                float newAngle = Mathf.MoveTowardsAngle(rigidbody2D.rotation, angle, speed * Time.deltaTime);
                rigidbody2D.MoveRotation(newAngle);

                angleDifference = Mathf.DeltaAngle(rigidbody2D.rotation, angle);
                yield return null;
            }

            isRotating = false;
        }

        public float CalculateAngle(Vector2 A, Vector2 B)
        {
            return Mathf.Atan2(A.y - B.y, A.x - B.x) * Mathf.Rad2Deg - 90;
        }

        public void Cancel()
        {
            if (RotateTowards == null) return;
            StopCoroutine(RotateTowards);
        }

        private void UpdateAnimation()
        {
            animator.SetBool("isRotating", isRotating);
        }
    }
}