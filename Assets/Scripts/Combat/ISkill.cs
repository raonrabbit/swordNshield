using System;
using UnityEngine;

namespace SwordNShield.Combat
{
    public interface ISkill
    {
        Controller.PlayerController Owner { get; set; }
        
        event EventHandler PlaySkill;
        void Play();
        KeyCode GetKeyCode { set; get; }
        Sprite SkillSprite { get; }
        bool CanExecute { get; }
        bool IsPlaying { get; }
        float CoolTime { get; }
        float ActionTime { get; }
    }
}
