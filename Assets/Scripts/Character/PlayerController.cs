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
        if (!photonView.IsMine && !_actions["Dash"].Playing)
        {
            //if((transform.position - _currentPosition).sqrMagnitude >= 10) transform.position = _currentPosition;
            transform.position = Vector3.Lerp(transform.position, _currentPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, _currentRotation, Time.deltaTime * 20);
            /*
            if (Vector2.Distance(transform.position, targetPos) < 0.2f) _rigidBody2D.velocity = Vector2.zero;
            else
            {
                Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
                Vector2 newPosition = Vector2.Lerp(transform.position, direction * _speed, Time.fixedDeltaTime);
                _rigidBody2D.MovePosition(newPosition);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, _currentRotation, Time.deltaTime * 15);
            */
        }
    }
    void FixedUpdate(){
        if(photonView.IsMine){
            //마우스 방향 바라보기 & 키보드로 이동
            /*
            float moveHorizontal = Input.GetAxis(HORIZONTAL);
            float moveVertical = Input.GetAxis(VERTICAL);

            movement = new Vector2(moveHorizontal, moveVertical);
            movement = movement.normalized;
            if(Input.GetKey(KeyCode.Space) && _actions["Dash"].CanExecute) {
                OnDash?.Invoke();
                Dash(movement);
                _photonView.RPC("Dash", RpcTarget.All, movement);
            }
            if(!_actions["Dash"].Playing) Move();
            Look(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            */
            
            // 우클릭으로 이동 & 우클릭 방향 바라보기
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
            }

            if (!_actions["Dash"].Playing)
            {
                if (Vector2.Distance(transform.position, targetPos) < 0.2f) _rigidBody2D.velocity = Vector2.zero;
                else Move();
                if(_rigidBody2D.velocity != Vector2.zero) animator.SetBool("isMoving", true);
                else animator.SetBool("isMoving", false);
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
