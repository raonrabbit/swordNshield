using System.Collections;
using SwordNShield.Function;
using UnityEngine;

namespace SwordNShield.Movement
{
    public class Rotater : MonoBehaviour, Function.IAction
    {
        [SerializeField] private Animator animator;
        private ActionScheduler actionScheduler;
        private Coroutine RotateTowards = null;
        private bool isRotating;
        private bool canRotate;
        
        void Awake()
        {
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

        public void StartRotateAction(Vector2 target)
        {
            if(RotateTowards!=null) StopCoroutine(RotateTowards);
            RotateTowards = StartCoroutine(Look(target));
        }
        
        public IEnumerator Look(Vector2 target){
            float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            while (Quaternion.Angle(transform.rotation, targetRotation) > 1.0f)
            {
                isRotating = true;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 30f * Time.deltaTime);
                yield return null;
            }
            isRotating = false;
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