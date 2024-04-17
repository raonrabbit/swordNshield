using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using SwordNShield.Controller;
using UnityEngine;

namespace SwordNShield.Combat.Skills
{
    public class SkillScheduler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private List<Skill> playerSkills = new();
        [SerializeField] private IndicatorManager indicatorManager;
        private Skill currentSkill;
        private bool canUseSkill;
        private Dictionary<string, Skill> skills = new();
        public List<Skill> GetPlayerSkills() => playerSkills;

        public bool CanUseSkill
        {
            get => canUseSkill;
            set => canUseSkill = value;
        }

        void Awake()
        {
            canUseSkill = true;
            foreach (var skill in playerSkills)
            {
                skill.Owner = playerController;
                skills.Add(skill.gameObject.name, skill);
            }
        }

        public IEnumerator StartSkill(Skill skill)
        {
            if (!canUseSkill || !skill.CanExecute) yield break;
            if (currentSkill != null && currentSkill.IsPlaying && !skill.CanUseDuringPlaying) yield break;
            
            if(!skill.CanUseDuringPlaying) currentSkill = skill;
            
            //즉발 스킬은 그냥 바로 Skill Play
            if (skill.SkillType == SkillType.Immediate)
            { 
                indicatorManager.DeActivateIndicator();
                PlaySkill(skill, new Target());
                yield break;
            }

            //스킬 타입이 NonTargetting일땐 IndicatorManager을 호출, 그리고 NonTargetting일땐 Target이 무조건 위치일수밖에 없음(혹은 각도)
            if (skill.SkillType == SkillType.NonTargetting)
            {
                Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                if (skill.IndicatorType == IndicatorType.None)
                {
                    targetPosition -= (Vector2)transform.position;
                    targetPosition = targetPosition.normalized;
                    Target target = new Target
                    {
                        VectorTarget = targetPosition
                    };
                    PlaySkill(skill, target);
                    yield break;
                }
                
                indicatorManager.ActivateIndicator(skill);
                while (currentSkill == skill)
                {
                    targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Input.GetMouseButton(1)) break;
                    if (Input.GetKeyUp(skill.GetKeyCode))
                    {
                        if (skill.IndicatorType == IndicatorType.Arrow)
                        {
                            targetPosition -= (Vector2)transform.position;
                            targetPosition = targetPosition.normalized;
                        }
                        Target target = new Target
                        {
                            VectorTarget = targetPosition
                        };
                        PlaySkill(skill, target);
                        break;
                    }
                    yield return null;
                }
                indicatorManager.DeActivateIndicator();
            }

            //이건 마우스 위에 다른 PhotonView 있는지 확인하고 PhotonView ID를 PunRPC에 넘기는 식으로 개발해야할듯 싶음
            if (skill.SkillType == SkillType.Targetting)
            {

            }
        }
        
        public void PlaySkill(Skill skill, Target? target)
        {
            string targetJson = JsonUtility.ToJson(target);
            photonView.RPC("PlaySkill_RPC", RpcTarget.All, skill.gameObject.name, targetJson);
        }
        
        //PunRPC를 오버로딩할 확률이 큼, 아니면 Vector2, int 등 여러 매개변수 처리하기 쉽지 않을듯
        [PunRPC]
        public void PlaySkill_RPC(string skillName, string targetJson)
        {
            Target target = JsonUtility.FromJson<Target>(targetJson);
            Skill skill = skills[skillName];
            skill.Play(target);
        }
    }
}
