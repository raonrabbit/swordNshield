using System;
using UnityEngine;

namespace SwordNShield.Stats
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float experiencePoints = 0;

        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained!();
        }
    }
}
