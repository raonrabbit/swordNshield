using System.Collections;
using SwordNShield.Class.Warrior;
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
    
    public override void Play(Vector2? position)
    {
        if (!canExecute) return;
        if (photonView.IsMine) InvokeEvent();
        Vector2 direction = (Vector2)position! - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        StartCoroutine(ExecuteCoroutine(angle));
        //photonView.RPC("ExecuteShieldAttack", RpcTarget.All, angle);
    }
    
    public IEnumerator ExecuteCoroutine(float angle)
    {
        //animationController.StartAnimation(animationClip);
        canExecute = false;
        isPlaying = true;
        Owner.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject instance = Instantiate(ShieldFire, Owner.transform.position, Owner.transform.rotation);
        FlyingShield flyingShield = instance.GetComponent<FlyingShield>();
        flyingShield.StunTime = stunTime;
        flyingShield.Play(photonView, distance, duration, damage);
        yield return new WaitForSeconds(actionTime);
        isPlaying = false;
        yield return new WaitForSeconds(coolTime);
        canExecute = true;
    }
}
