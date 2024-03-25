using System;
using System.Collections;
using System.Numerics;
using Photon.Pun;
using SwordNShield.Attributes;
using SwordNShield.Combat;
using SwordNShield.Controller;
using SwordNShield.Function;
using SwordNShield.Movement;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class ShieldAttack : MonoBehaviour, ISkill
{
    public PlayerController Owner { get; set; }
    public event EventHandler PlaySkill;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private Sprite skillImage;
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private float coolTime;
    [SerializeField] private float actionTime;

    private AnimationScheduler animationScheduler;
    private Rotater rotater;
    private Health health;
    private bool canExecute = true;
    private bool isPlaying;
    private ISkill _skillImplementation;

    void Awake()
    {
        animationScheduler = GetComponent<AnimationScheduler>();
        health = GetComponent<Health>();
        rotater = GetComponent<Rotater>();
    }
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
        StartCoroutine(ExecuteShieldAttack());
        Owner.photonView.RPC("ExecuteShieldAttack", RpcTarget.All);
    }

    [PunRPC]
    public IEnumerator ExecuteShieldAttack()
    {
        rotater.CanRotate = false;
        animationScheduler.StartAnimation(animationClip);
        canExecute = false;
        isPlaying = true;
        health.CanGetDamage = false;
        yield return new WaitForSeconds(actionTime);
        health.CanGetDamage = true;
        rotater.CanRotate = true;
        yield return new WaitForSeconds(coolTime);
        canExecute = true;
    }
}
