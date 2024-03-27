using System.Collections;
using Photon.Pun;
using SwordNShield.Attributes;
using SwordNShield.Controller;
using SwordNShield.Function;
using SwordNShield.Movement;
using UnityEngine;

public class Attacker : MonoBehaviourPunCallbacks, IAction
{
    private bool canAttack;
    private bool canMove;
    private Rigidbody2D rigidBody2D;
    private Rotater rotater;
    private Stat stat;
    private ActionScheduler actionScheduler;
    private Coroutine AttackCoroutine;

    public bool CanAttack
    {
        get => canAttack;
        set => canAttack = value;
    }
    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        rotater = GetComponent<Rotater>();
        stat = GetComponent<Stat>();
        canMove = stat.MoveSpeed != 0;
        canAttack = true;
        actionScheduler = GetComponent<ActionScheduler>();
    }

    public void StartAttack(Health target)
    {
        if (!canAttack) return;
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
        while (true)
        {
            if (target == null) Cancel();
            if (Vector2.Distance(transform.position, target.transform.position) <= stat.AttackRange)
            {
                float angle = rotater.CalculateAngle(target.transform.position, transform.position);
                rotater.StartRotateAction(angle, stat.RotateSpeed);
                if (canAttack)
                {
                    photonView.RPC("PlayTriggerAnimation", RpcTarget.All, "attack");
                    target.GetDamage(gameObject, stat.AttackDamage);
                    if (target == null || target.IsDead()) Cancel();
                    StartCoroutine(AttackDelay());
                }
            }
            else
            {
                if (canMove)
                {
                    float angle = rotater.CalculateAngle(target.transform.position, transform.position);
                    rotater.StartRotateAction(angle, stat.RotateSpeed);
                    Vector2 direction = ((Vector2)target.transform.position - rigidBody2D.position).normalized;
                    rigidBody2D.velocity = direction * stat.MoveSpeed;
                }
            }
            yield return null;
        }
    }

    private IEnumerator AttackDelay()
    {
        canAttack = false;
        yield return new WaitForSeconds(stat.AttackCoolTime);
        canAttack = true;
    }
}
