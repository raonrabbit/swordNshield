using System.Collections;
using SwordNShield.Combat;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Attributes;
using SwordNShield.Combat.Skills;
using SwordNShield.Controller;
using UnityEngine;

public class SwordOfJudgement : Skill
{
    [Header("Unique Settings")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Attacker attacker;
    [SerializeField] private Mover mover;
    [SerializeField] private Rotater rotater;
    [SerializeField] private Laser laser;
    [SerializeField] private float damage;
    [SerializeField] private float width;
    [SerializeField] private float distance;
    [SerializeField] private float rotateSpeed;

    void Awake()
    {
        laser.SetLaserSize(width, distance);
        if (indicator != null)
        {
            SpriteRenderer indicatorSprite = indicator.GetComponent<SpriteRenderer>();
            indicatorSprite.size = new Vector2(width, distance);
        }
        canExecute = true;
    }
    
    public override void Play(Target target)
    {
        if (!canExecute) return;
        if (photonView.IsMine) InvokeEvent();
        Vector2 direction = target.VectorTarget;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        StartCoroutine(ExecuteCoroutine(angle));
    }

    public IEnumerator ExecuteCoroutine(float angle)
    {
        if (photonView.IsMine) playerController.DefaultRotation = false;

        canExecute = false;
        mover.Cancel();
        mover.CanMove = false;
        attacker.Cancel();
        attacker.CanAttack = false;
        Owner.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        animationController.StartAnimation(animationClip);
        
        float duration = actionTime;
        float nextDamageTime = 0f;
        
        while (duration > 0)
        {
            mover.CanMove = false;
            
            laser.gameObject.SetActive(true);
            if (photonView.IsMine)
            {
                Vector2 mousePosition = playerController.GetMouseRay();
                //Debug.Log(mousePosition);
                rotater.Rotate(rotater.CalculateAngle(mousePosition, transform.position), rotateSpeed);

                if (Time.time >= nextDamageTime)
                {
                    GiveDamage();
                    nextDamageTime = Time.time + 0.1f;
                }
            }
            duration -= Time.deltaTime;
            yield return null;
        }
        laser.gameObject.SetActive(false);
        animationController.StopAnimation();
        mover.CanMove = true;
        attacker.CanAttack = true;
        
        yield return new WaitForSeconds(CoolTime);
        canExecute = true;
        
        yield return null;
    }

    private void GiveDamage()
    {
        Vector2 box = new Vector2(width, width);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, box, 0, transform.up, distance);

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == Owner.gameObject) continue;
                Health health = hit.collider.GetComponent<Health>();
                if (health != null)
                {
                    health.GetDamage(Owner.gameObject, damage);
                }
            }
        }
    }
}
