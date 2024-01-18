using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Sword sword;
    protected Animator animator;
    protected bool _isDead = false;

    //기본 설정
    [SerializeField] protected float _speed = 2f;
    protected int _maxHp = 100;
    [SerializeField] protected int _currentHp = 100;
    protected int _damage = 40;

    //공격
    protected float _attackCooldown = 2f;
    protected float _attackTime = 0.2f;
    protected bool _isAttacking = false;

    //방어
    protected float _maxDefendTime = 3f;
    protected float _defendingTime;
    protected bool _isDefending = false;

    public int GetHp{
        get => _currentHp;
    }
    public abstract void Move();
    public void GetDamage(){
        _currentHp -= _damage;
        if(_currentHp <= 0) Die();
    }
    public void Die(){
        if(!_isDead){
            _isDead = true;
            gameObject.SetActive(false);
        }
    }

    public IEnumerator Attack(){
        if(!_isAttacking){
            _isAttacking = true;
            animator.SetTrigger("attack");
            StartCoroutine(SwordColliderControl());
            yield return new WaitForSeconds(_attackCooldown);
            _isAttacking = false;
        }
    }

    private IEnumerator SwordColliderControl(){
        sword.EnableSwordCollider();
        yield return new WaitForSeconds(_attackTime);
        sword.DisableSwordCollider();
    }
    public void Look(Vector3 target){
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
    }
    
    public void StartDefend(){
        _isDefending = true;
        animator.SetBool("isDefending", true);
    }

    public void EndDefend(){
        _isDefending = false;
        animator.SetBool("isDefending", false);
    }
}
