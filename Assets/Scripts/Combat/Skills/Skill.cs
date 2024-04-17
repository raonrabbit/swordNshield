using System;
using Photon.Pun;
using SwordNShield.Controller;
using UnityEngine;

namespace SwordNShield.Combat.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        public PlayerController Owner { get; set; }
        public event EventHandler PlaySkill;

        [Header("Default Settings")] 
        [SerializeField] protected SkillType skillType;
        [SerializeField] protected Sprite skillImage;
        [SerializeField] protected AnimationController animationController;
        [SerializeField] protected AnimationClip animationClip;
        [SerializeField] protected PhotonView photonView;
        [SerializeField] protected IndicatorType indicatorType;
        [SerializeField] protected GameObject indicator;
        [SerializeField] protected KeyCode keyCode;
        [SerializeField] protected float coolTime;
        [SerializeField] protected float actionTime;
        [SerializeField] protected bool canUseDuringPlaying;
        protected bool canExecute;
        protected bool isPlaying;
        
        
        public KeyCode GetKeyCode
        {
            get => keyCode;
            set => keyCode = value;
        }

        public SkillType SkillType => skillType;
        public Sprite SkillSprite => skillImage;
        public IndicatorType IndicatorType => indicatorType;
        public GameObject Indicator => indicator;
        public bool CanExecute => canExecute;
        public bool IsPlaying => isPlaying;
        public float CoolTime => coolTime;
        public float ActionTime => actionTime;
        public bool CanUseDuringPlaying => canUseDuringPlaying;

        public abstract void Play(Target target);
        /*
        public virtual void Play() { }
        public virtual void Play(Vector2? _) { }

        public virtual void Play(GameObject _) { }
        */

        protected void InvokeEvent()
        {
            PlaySkill!.Invoke(this, EventArgs.Empty);
        }
    }
}
