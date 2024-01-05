using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Animator animator;
    protected bool _isDead = false;
    [SerializeField] protected float _speed = 2f;
    protected int _hp = 100;
    protected int _damage = 40;
    protected float _attackCooldown = 2f;
    protected bool _isAttacking = false;
    protected Collider _attackCollider;

    public abstract void Move();
    public IEnumerator Attack(){
        _isAttacking = true;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(_attackCooldown);
        _isAttacking = false;
    }

    public abstract void Look();
    public void GetDamage(){
        _hp -= _damage;
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
}
