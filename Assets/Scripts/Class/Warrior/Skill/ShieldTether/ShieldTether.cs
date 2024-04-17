using System.Collections;
using SwordNShield.Class.Warrior;
using SwordNShield.Combat;
using SwordNShield.Combat.Skills;
using UnityEngine;

public class ShieldTether : Skill
{
    [Header("Unique Settings")]
    [SerializeField] private GameObject ShieldFire;
    [SerializeField] private float damage;
    [SerializeField] private float distance;
    [SerializeField] private float duration;
    [SerializeField] private float stunTime;
    
    void Awake()
    {
        canExecute = true;
        SpriteRenderer indicatorSprite = indicator.GetComponent<SpriteRenderer>();
        indicatorSprite.size = new Vector2(indicatorSprite.size.x, distance);
    }
    
    public override void Play(Target target)
    {
        Vector2 direction = target.VectorTarget;
        if (!canExecute) return;
        if (photonView.IsMine) InvokeEvent();
        //Vector2 direction = position - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        StartCoroutine(ExecuteCoroutine(angle));
        //photonView.RPC("ExecuteShieldAttack", RpcTarget.All, angle);
    }
    
    public IEnumerator ExecuteCoroutine(float angle)
    {
        animationController.StartAnimation(animationClip);
        canExecute = false;
        isPlaying = true;
        Owner.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject instance = Instantiate(ShieldFire, Owner.transform.position + transform.up * 0.5f, Owner.transform.rotation);
        FlyingShield flyingShield = instance.GetComponent<FlyingShield>();
        flyingShield.owner = Owner.photonView;
        yield return new WaitForSeconds(actionTime);
        flyingShield.StunTime = stunTime;
        flyingShield.Play(photonView, distance - 0.5f, duration, damage);
        isPlaying = false;
        yield return new WaitForSeconds(coolTime);
        canExecute = true;
    }
}
