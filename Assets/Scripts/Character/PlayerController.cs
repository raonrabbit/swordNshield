using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : Character
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private Vector2 movement;

    public GameObject RightHand;
    private float angle;
    private Vector2 player;

    //Animator 변수
    private bool isMoving;

    //스킬 쿨타임 표시
    public delegate void PlayerSkill();
    public static event PlayerSkill OnDash;
    public static event PlayerSkill OnShield;
    public static event PlayerSkill OnAttack;

    public static event Action OnDeath;
    new void Awake(){
        base.Awake();
        animator = gameObject.GetComponent<Animator>();
        if(_photonView.IsMine){
            StartCoroutine(AttackCoroutine());
            StartCoroutine(DefendCoroutine());
        }
    }

    void FixedUpdate(){
        if(photonView.IsMine){
            float moveHorizontal = Input.GetAxis(HORIZONTAL);
            float moveVertical = Input.GetAxis(VERTICAL);

            movement = new Vector2(moveHorizontal, moveVertical);
            movement = movement.normalized;
            if(Input.GetKey(KeyCode.Space) && _canDash) {
                OnDash?.Invoke();
                Dash(movement);
                _photonView.RPC("Dash", RpcTarget.All, movement);
            }
            if(!_isDashing) Move();
            Look(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else{
            if(!_isDashing){
                if((transform.position - _currentPosition).sqrMagnitude >= 2) transform.position = _currentPosition;
                else transform.position = Vector3.Lerp(transform.position, _currentPosition, Time.deltaTime * 10);
                transform.rotation = Quaternion.Lerp(transform.rotation, _currentRotation, Time.deltaTime * 10);
            }
        }
    }
    [PunRPC]
    public override void Die()
    {
        if(photonView.IsMine) OnDeath?.Invoke();
        base.Die();
    }

    public override void Move(){
        if(movement != new Vector2(0, 0)) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);
        _rigidBody2D.velocity = new Vector3(movement.x, movement.y, 0) * _speed;
        //transform.position += new Vector3(movement.x, movement.y, 0) * _speed * Time.deltaTime;
    }

    IEnumerator AttackCoroutine(){
        while(true){
            if(Input.GetMouseButtonDown(0) && !_isAttacking){
                if(photonView.IsMine) OnAttack?.Invoke();
                StartCoroutine(Attack());
            }
            yield return null;
        }
    }

    IEnumerator DefendCoroutine(){
        while(true){
            if(_canDefend && Input.GetKeyDown(KeyCode.LeftShift)){
                OnShield?.Invoke();
                StartCoroutine(Defend());
            }
            yield return null;
        }
    }
}
