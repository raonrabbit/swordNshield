using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Animator animator;
    protected bool _isDead = false;

    //기본 설정
    [SerializeField] protected float _speed = 2f;
    protected int _maxHp = 100;
    protected int _currentHp = 100;
    protected int _damage = 40;

    //공격
    protected float _attackCooldown = 2f;
    protected bool _isAttacking = false;
    protected Collider _attackCollider;

    //방어
    protected float _maxDefendTime = 3f;
    protected float _defendingTime;
    protected bool _isDefending = false;

    public abstract void Move();
    public abstract void Look();
    public void GetDamage(){
        _currentHp -= _damage;
    }
    public void Die(){
        if(!_isDead){
            _isDead = true;
            gameObject.SetActive(false);
        }
    }

    public void EnableAttackCollider(){
        _attackCollider.enabled = true;
    }

    public void DisableAttackCollider(){
        _attackCollider.enabled = false;
    }

    public IEnumerator Attack(){
        _isAttacking = true;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(_attackCooldown);
        _isAttacking = false;
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
