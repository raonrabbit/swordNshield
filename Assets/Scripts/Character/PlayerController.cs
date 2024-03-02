using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerController : Character
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private Vector2 movement;

    public GameObject RightHand;

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
            if(Input.GetKey(KeyCode.Space) && _actions["Dash"].CanExecute) {
                OnDash?.Invoke();
                Dash(movement);
                _photonView.RPC("Dash", RpcTarget.All, movement);
            }
            if(!_actions["Dash"].Playing) Move();
            Look(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else{
            if(!_actions["Dash"].Playing){
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
