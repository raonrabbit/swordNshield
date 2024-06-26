using System.Collections;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Skills;
using UnityEngine;

namespace SwordNShield.Combat.States
{
    public class Stun : MonoBehaviour, IState
    {
        private StateType stateType = StateType.Stun;
        [SerializeField] private Mover mover;
        [SerializeField] private Rotater rotater;
        [SerializeField] private Attacker attacker;
        [SerializeField] private ActionScheduler actionScheduler;
        [SerializeField] private SkillScheduler skillScheduler;
        [SerializeField] private Rigidbody2D rigidBody2D;
        [SerializeField] private GameObject stunEffect;
        private Coroutine state;
        private float duration;

        public StateType Type => stateType;
        
        public void SetState(float rate, float time)
        {
            if (state == null)
            {
                duration = time;
                state = StartCoroutine(Execute());
                return;
            }

            if (duration < time)
            {
                duration = time;
            }
        }

        private IEnumerator Execute()
        {
            actionScheduler.StartAction(null);
            rigidBody2D.velocity = new Vector2(0f, 0f);
            stunEffect.SetActive(true);
            float originalMass = rigidBody2D.mass;
            while (duration > 0)
            {
                rigidBody2D.mass = 10000;
                mover.CanMove = false;
                rotater.CanRotate = false;
                attacker.Cancel();
                attacker.CanAttack = false;
                actionScheduler.CanAction = false;
                skillScheduler.CanUseSkill = false;
                duration -= Time.deltaTime;
                yield return null;
            }
            state = null;
            rigidBody2D.mass = originalMass;
            mover.CanMove = true;
            rotater.CanRotate = true;
            attacker.CanAttack = true;
            actionScheduler.CanAction = true;
            skillScheduler.CanUseSkill = true;
            stunEffect.SetActive(false);
            duration = 0;
        }
    }
}
