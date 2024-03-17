using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerController : Character
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    //private Vector2 movement;

    public GameObject RightHand;
    private Camera mainCamera;

    //스킬 쿨타임 표시
    public delegate void PlayerSkill();
    public static event PlayerSkill OnDash;
    public static event PlayerSkill OnShield;
    public static event PlayerSkill OnAttack;

    public static event Action OnDeath;
    new void Awake(){
        mainCamera = Camera.main;
        base.Awake();
        animator = gameObject.GetComponent<Animator>();
        if(_photonView.IsMine){
            StartCoroutine(AttackCoroutine());
            StartCoroutine(DefendCoroutine());
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButton(1))
            {
                targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Look(targetPos);
            }
            
            // F키로 Dash, 마우스 클릭 방향으로 Dash
            if (Input.GetKey(KeyCode.F) && _actions["Dash"].CanExecute)
            {
                OnDash?.Invoke();
                Vector2 dashDir = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                Dash(dashDir.normalized);
                photonView.RPC("Dash", RpcTarget.All, dashDir.normalized);
            }
            
            if(_rigidBody2D.velocity != Vector2.zero) animator.SetBool("isMoving", true);
            else animator.SetBool("isMoving", false);
        }
    }
    void FixedUpdate(){
        if(photonView.IsMine){
            if (!_actions["Dash"].Playing)
            {
                if (Vector2.Distance(transform.position, targetPos) < 0.2f)
                {
                    targetPos = transform.position;
                    _rigidBody2D.velocity = Vector2.zero;
                }
                else Move();
            }
        }
        else if (!_actions["Dash"].Playing)
        {
            if (Vector2.Distance(transform.position, targetPos) < 0.2f) _rigidBody2D.velocity = Vector2.zero;
            else Move();
            transform.rotation = Quaternion.Lerp(transform.rotation, _currentRotation, Time.fixedDeltaTime * 18);
        }
    }
    [PunRPC]
    public override void Die()
    {
        if(photonView.IsMine) OnDeath?.Invoke();
        base.Die();
    }

    public override void Move(){
        //_rigidBody2D.velocity = new Vector3(movement.x, movement.y, 0) * _speed;
        
        //마우스 클릭 위치와 Player 간의 방향을 계산 후 이동
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        _rigidBody2D.velocity = direction * _speed;
    }

    IEnumerator AttackCoroutine(){
        while(true){
            if(Input.GetMouseButtonDown(0) && _actions["Attack"].CanExecute){
                if(photonView.IsMine) OnAttack?.Invoke();
                StartCoroutine(sword.Use());
                StartCoroutine(_actions["Attack"].Execute());
            }
            yield return null;
        }
    }

    IEnumerator DefendCoroutine(){
        while(true){
            if(_actions["Defend"].CanExecute && Input.GetKeyDown(KeyCode.LeftShift)){
                OnShield?.Invoke();
                StartCoroutine(_actions["Defend"].Execute());
            }
            yield return null;
        }
    }
}
