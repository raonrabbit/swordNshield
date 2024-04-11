using System.Collections;
using Photon.Pun;
using UnityEngine;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Attributes;
using SwordNShield.Combat.Skills;

namespace SwordNShield.Class.Warrior
{
 public class SwirlAttack : Skill
{
    [Header("Unique Settings")]
    [SerializeField] private float timeBetweenDamages;
    [SerializeField] private float damagePerTime;
    [SerializeField] private float damageRange;
    [SerializeField] private GameObject swordTrail;
    [SerializeField] private Rotater rotater;
    [SerializeField] private Attacker attacker;

    void Awake()
    { 
        canExecute = true;
        if (swordTrail != null) swordTrail.SetActive(false);
    }
    
    
    public override void Play()
    {
        if (!canExecute) return;
        if (photonView.IsMine) InvokeEvent();
        StartCoroutine(ExecuteCoroutine());
        //photonView.RPC("ExecuteSwirlAttack", RpcTarget.All);
    }

    //[PunRPC]
    public void ExecuteSwirlAttack()
    {
        StartCoroutine(ExecuteCoroutine());
    }
    private IEnumerator ExecuteCoroutine()
    {
        rotater.CanRotate = false;
        attacker.CanAttack = false;
        animationController.StartAnimation(animationClip);
        canExecute = false;
        isPlaying = true;
        swordTrail.SetActive(true);
        float startTime = Time.time;
        while (Time.time - startTime <= actionTime)
        {
            if (this == null) yield break;
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
            if (hit.gameObject == Owner.gameObject) continue;
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.GetDamage(gameObject, damagePerTime);
            }
        }
    }
}   
}
