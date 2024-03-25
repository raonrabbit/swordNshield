using System;
using SwordNShield.Combat;
using SwordNShield.Controller;
using UnityEngine;

public class ShieldAttack : MonoBehaviour, ISkill
{
    public PlayerController Owner { get; set; }
    public event EventHandler PlaySkill;
    [SerializeField] private Sprite skillImage;
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private float coolTime;
    [SerializeField] private float actionTime;

    private bool canExecute = true;
    private bool isPlaying;
    private ISkill _skillImplementation;

    public KeyCode GetKeyCode
    {
        get => keyCode;
        set => keyCode = value;
    }

    public Sprite SkillSprite => skillImage;
    public bool CanExecute => canExecute;
    public bool IsPlaying => isPlaying;
    public float CoolTime => coolTime;
    public float ActionTime => actionTime;

    public void Play()
    {
        if (!canExecute) return;
        PlaySkill!.Invoke(this, EventArgs.Empty);
        
    }
}
