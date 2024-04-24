using System.Collections;
using SwordNShield.Combat;
using SwordNShield.Combat.Actions;
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
    [SerializeField] private GameObject Lazer;
    [SerializeField] private float damage;
    [SerializeField] private float distance;
    [SerializeField] private float rotateSpeed;

    void Awake()
    {
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
        
        mover.Cancel();
        mover.CanMove = false;
        attacker.Cancel();
        attacker.CanAttack = false;
        Owner.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        animationController.StartAnimation(animationClip);
        
        float duration = actionTime;
        while (duration > 0)
        {
            mover.CanMove = false;
            
            if (photonView.IsMine)
            {
                Lazer.SetActive(true);
                Vector2 mousePosition = playerController.GetMouseRay();
                //Debug.Log(mousePosition);
                rotater.Rotate(rotater.CalculateAngle(mousePosition, transform.position), rotateSpeed);
            }
            duration -= Time.deltaTime;
            yield return null;
        }
        Lazer.SetActive(false);
        animationController.StopAnimation();
        mover.CanMove = true;
        attacker.CanAttack = true;
        
        yield return null;
    }
}
