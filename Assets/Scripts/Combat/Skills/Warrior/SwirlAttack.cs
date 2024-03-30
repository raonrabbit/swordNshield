using System;
using System.Collections;
using Photon.Pun;
using SwordNShield.Attributes;
using SwordNShield.Controller;
using SwordNShield.Function;
using SwordNShield.Movement;
using UnityEngine;

namespace SwordNShield.Combat.Skills
{
 public class SwirlAttack : MonoBehaviourPunCallbacks, ISkill
{
    public PlayerController Owner { get; set; }
    public event EventHandler PlaySkill;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private Sprite skillImage;
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private float coolTime;
    [SerializeField] private float actionTime;
    [SerializeField] private float timeBetweenDamages;
    [SerializeField] private float damagePerTime;
    [SerializeField] private float damageRange;
    [SerializeField] private GameObject swordTrail;
    
    private AnimationScheduler animationScheduler;
    private Rotater rotater;
    private Attacker attacker;
    private bool canExecute;
    private bool isPlaying;

    void Awake()
    { 
        animationScheduler = GetComponent<AnimationScheduler>();
        rotater = GetComponent<Rotater>();
        attacker = GetComponent<Attacker>();
        if (swordTrail != null) swordTrail.SetActive(false);
        canExecute = true;
    }

    public KeyCode GetKeyCode
    {
        get => keyCode;
        set => keyCode = value;
    }
    
    public Sprite SkillSprite => skillImage;
    public GameObject Indicator => null;
    public bool CanExecute => canExecute;
    public bool IsPlaying => isPlaying;
    public float CoolTime => coolTime;
    public float ActionTime => actionTime;
    
    public void Play(Vector2? _)
    {
        if (!canExecute) return;
        PlaySkill!.Invoke(this, EventArgs.Empty);
        StartCoroutine(ExecuteSwirlAttack());
        Owner.photonView.RPC("ExecuteSwirlAttack", RpcTarget.All);
    }

    [PunRPC]
    public IEnumerator ExecuteSwirlAttack()
    {
        rotater.CanRotate = false;
        attacker.CanAttack = false;
        animationScheduler.StartAnimation(animationClip);
        canExecute = false;
        isPlaying = true;
        swordTrail.SetActive(true);
        float startTime = Time.time;
        while (Time.time - startTime <= actionTime)
        {
            if(photonView.IsMine) GiveDamage();
            yield return new WaitForSeconds(timeBetweenDamages);
        }
        isPlaying = false;
        rotater.CanRotate = true;
        attacker.CanAttack = true;
        swordTrail.SetActive(false);
        yield return new WaitForSeconds(coolTime);
        canExecute = true;
    }

    public void GiveDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageRange);
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.GetDamage(gameObject, damagePerTime);
            }
        }
    }
}   
}
