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
    protected float _defendCooldown = 3f;
    protected float _defendTime = 4f;
    [SerializeField] protected bool _isDefending = false;
    protected bool _canDefend = true;

    public int GetHp{
        get => _currentHp;
    }
    public float AttackTime{
        get => _attackTime;
    }
    public abstract void Move();
    public void GetDamage(Character other){
        if(!(_isDefending && FaceToOther(other))){
            _currentHp -= _damage;
            if(_currentHp <= 0) Die();
        }
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
            StartCoroutine(sword.Use());
            yield return new WaitForSeconds(_attackCooldown);
            _isAttacking = false;
        }
    }

    public void Look(Vector3 target){
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
    }
    
    public IEnumerator Defend(){
        _canDefend = false;
        _isDefending = true;
        animator.SetBool("isDefending", true);
        yield return new WaitForSeconds(_defendTime);
        _isDefending = false;
        animator.SetBool("isDefending", false);
        yield return new WaitForSeconds(_defendCooldown);
        _canDefend = true;
    }
    
    public bool FaceToOther(Character other){
        Vector2 directionToOther = (other.transform.position - transform.position).normalized;
        Debug.Log(Vector2.Angle(transform.up, directionToOther));
        if(Vector2.Angle(transform.up, directionToOther) < 40f){
            return true;
        }
        return false;
    }
}
