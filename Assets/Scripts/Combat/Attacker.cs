using System;
using System.Collections;
using SwordNShield.Attributes;
using SwordNShield.Function;
using SwordNShield.Movement;
using UnityEngine;

public class Attacker : MonoBehaviour, IAction
{
    private bool canAttack;
    private bool canMove;
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private Rotater rotater;
    private Stat stat;
    private ActionScheduler actionScheduler;
    private Coroutine AttackCoroutine;

    public bool CanAttack => canAttack;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rotater = GetComponent<Rotater>();
        stat = GetComponent<Stat>();
        canMove = stat.MoveSpeed != 0;
        actionScheduler = GetComponent<ActionScheduler>();
    }

    public void StartAttack(Health target)
    {
        if(AttackCoroutine!=null) Cancel();
        actionScheduler.StartAction(this);
        AttackCoroutine = StartCoroutine(Attack(target));
    }

    public void Cancel()
    {
        if(AttackCoroutine != null) StopCoroutine(AttackCoroutine);
    }

    private IEnumerator Attack(Health target)
    {
        canAttack = true;
        float startTime = Time.time;
        while (true)
        {
            if (!canAttack && Time.time - startTime >= stat.AttackCoolTime)
            {
                canAttack = true;
            }
            if (Vector2.Distance(transform.position, target.transform.position) <= stat.AttackRange)
            {
                float angle = rotater.CalculateAngle(target.transform.position, transform.position);
                rotater.StartRotateAction(angle, stat.RotateSpeed);
                if (CanAttack)
                {
                    animator.SetTrigger("attack");
                    target.GetDamage(gameObject, stat.AttackDamage);
                    startTime = Time.time;
                    canAttack = false;
                }
            }
            else
            {
                if (canMove)
                {
                    float angle = rotater.CalculateAngle(target.transform.position, transform.position);
                    rotater.StartRotateAction(angle, stat.RotateSpeed);
                    Vector2 direction = ((Vector2)target.transform.position - rigidbody2D.position).normalized;
                    rigidbody2D.velocity = direction * stat.MoveSpeed;
                }
            }
            yield return null;
        }
    }
}
