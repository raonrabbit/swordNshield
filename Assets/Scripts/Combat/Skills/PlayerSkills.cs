using System.Collections.Generic;
using UnityEngine;
using SwordNShield.Controller;

namespace SwordNShield.Combat.Skills
{
    public class PlayerSkills : MonoBehaviour
    {
        [SerializeField] private List<ISkill> playerSkills = new List<ISkill>();
        private PlayerController playerController;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            ISkill[] skills = GetComponents<ISkill>();
            foreach (var skill in skills)
            {
                skill.Owner = playerController;
                playerSkills.Add(skill);
            }
        }

        public List<ISkill> GetPlayerSkills() => playerSkills;
    }
}
