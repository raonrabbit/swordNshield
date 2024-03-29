using Photon.Pun;
using UnityEngine;

namespace SwordNShield.Function
{
    public class AnimationScheduler : MonoBehaviourPunCallbacks
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
        
        
        [PunRPC]
        public void PlayTriggerAnimation(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
