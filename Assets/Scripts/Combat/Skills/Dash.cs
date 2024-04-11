using UnityEngine;
using System.Collections;
using Photon.Pun;
using SwordNShield.Combat.Actions;

namespace SwordNShield.Combat.Skills
{
    public class Dash : Skill
    {
        [Header("Unique Settings")]
        [SerializeField] private float dashSpeed = 10f;

        [SerializeField] private Rigidbody2D rigidBody2D;
        [SerializeField] private Mover mover;

        void Awake()
        {
            canExecute = true;
        }
        //Dash 실행
        public override void Play(Vector2? position)
        {
            if (!canExecute) return;
            if (photonView.IsMine) InvokeEvent();
            Vector2 dashDirection = ((Vector2)position - (Vector2)Owner.transform.position).normalized;
            StartCoroutine(ExecuteCoroutine(dashDirection));
            //photonView.RPC("ExecuteDash", RpcTarget.All, dashDirection);
        }
        
        [PunRPC]
        public void ExecuteDash(Vector2 dashDirection)
        {
            StartCoroutine(ExecuteCoroutine(dashDirection));
        }
        
        public IEnumerator ExecuteCoroutine(Vector2 dashDirection)
        {
            mover.CanMove = false;
            float startTime = Time.time;
            canExecute = false;
            isPlaying = true;
            while(Time.time - startTime < actionTime){
                Owner.GetActionScheduler.CancelCurrentAction();
                rigidBody2D.velocity = dashDirection * dashSpeed;
                yield return null;
            }
            rigidBody2D.velocity = Vector2.zero;
            mover.CanMove = true;
            isPlaying = false;
            yield return new WaitForSeconds(coolTime);
            canExecute = true;
        }
    }
}
