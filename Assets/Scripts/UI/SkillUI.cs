using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using SwordNShield.Combat.Skills;
using UnityEngine.UI;

namespace SwordNShield.UI
{
    public class SkillUI : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private SkillScheduler skillScheduler;
        private List<Skill> playerSkillList;
        private Dictionary<KeyCode, Skill> skillsByKeyCode;
        private Dictionary<KeyCode, Image> skillImages;
        
        private void Awake()
        {
            if (!photonView.IsMine)
            {
                this.enabled = false;
                return;
            }
            playerSkillList = skillScheduler.GetPlayerSkills();
            SetSkill();
        }
        
        private void SetSkill()
        {
            skillsByKeyCode = new Dictionary<KeyCode, Skill>();
            skillImages = new Dictionary<KeyCode, Image>
            {
                { KeyCode.Q, CoolTimeUI.Instance.QSkillImage },
                { KeyCode.W, CoolTimeUI.Instance.WSkillImage },
                { KeyCode.E, CoolTimeUI.Instance.ESkillImage },
                { KeyCode.R, CoolTimeUI.Instance.RSkillImage },
                { KeyCode.D, CoolTimeUI.Instance.DSkillImage },
                { KeyCode.F, CoolTimeUI.Instance.FSkillImage }
            };
            foreach (var skill in playerSkillList)
            {
                if (skillImages.ContainsKey(skill.GetKeyCode))
                {
                    skillsByKeyCode[skill.GetKeyCode] = skill;
                    skill.PlaySkill += OnSkillUse;
                    skillImages[skill.GetKeyCode].sprite = skill.SkillSprite;
                    skillImages[skill.GetKeyCode].fillAmount = 1;
                }
            }
        }

        private void OnDestroy()
        {
            if (skillsByKeyCode == null) return;
            foreach (var skill in skillsByKeyCode.Values)
            {
                skill.PlaySkill -= OnSkillUse;
            }
        }

        private void OnSkillUse(object sender, EventArgs e)
        {
            if (sender is Skill skill && skillImages.ContainsKey(skill.GetKeyCode))
            {
                StartCoroutine(CoolDownImage(skillImages[skill.GetKeyCode], skill.ActionTime, skill.CoolTime));
            }
        }

        private IEnumerator CoolDownImage(Image skillImage, float actionTime, float coolDownTime){
            yield return new WaitForSeconds(actionTime);
            float currentCoolDown = 0;
            skillImage.fillAmount = 0;
            while(currentCoolDown < coolDownTime){
                currentCoolDown += Time.deltaTime;
                skillImage.fillAmount = currentCoolDown / coolDownTime;
                yield return null;
            }
            skillImage.fillAmount = 1;
        }
    }
}