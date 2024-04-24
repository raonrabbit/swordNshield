using Photon.Pun;
using UnityEngine;

namespace SwordNShield.Controller
{
    public class AnimationController : MonoBehaviourPunCallbacks
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void StartAnimation(AnimationClip clip)
        {
            animator.Play(clip.name, -1, 0f);
        }

        public void StopAnimation()
        {
            //animator.Play("Idle", -1, 0f);
            animator.CrossFade("Idle", 0.05f);
        }
        
        
        [PunRPC]
        public void PlayTriggerAnimation(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
