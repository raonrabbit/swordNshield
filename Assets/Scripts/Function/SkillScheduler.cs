using SwordNShield.Combat;
using UnityEngine;

public class SkillScheduler : MonoBehaviour
{
    private ISkill currentSkill;
    [SerializeField] private IndicatorManager indicatorManager;

    public void StartSkill(ISkill skill)
    {
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
