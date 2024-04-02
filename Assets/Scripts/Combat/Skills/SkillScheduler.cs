using SwordNShield.Combat;
using UnityEngine;

namespace SwordNShield.Combat.Skills
{
    public class SkillScheduler : MonoBehaviour
    {
        private ISkill currentSkill;
        private bool canUseSkill;
        [SerializeField] private IndicatorManager indicatorManager;

        public bool CanUseSkill
        {
            get => canUseSkill;
            set => canUseSkill = value;
        }

        void Awake()
        {
            canUseSkill = true;
        }

        public void StartSkill(ISkill skill)
        {
            if (!canUseSkill) return;
            if (currentSkill == null || !currentSkill.IsPlaying)
            {
                currentSkill = skill;
                if (skill.Indicator != null)
                {
                    indicatorManager.ActivateIndicator(skill);
                    return;
                }

                indicatorManager.DeActivateIndicator();
                currentSkill.Play();
            }
        }
    }
}
