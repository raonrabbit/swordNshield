using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : Character
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public GameObject RightHand;
    private float angle;
    private Vector2 player;

    //Animator 변수
    private bool isMoving;

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
            Move();
            Look(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else{
            if((transform.position - _currentPosition).sqrMagnitude >= 2) transform.position = _currentPosition;
            else transform.position = Vector3.Lerp(transform.position, _currentPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, _currentRotation, Time.deltaTime * 10);
        }
    }
    [PunRPC]
    public override void Die()
    {
        if(photonView.IsMine) OnDeath?.Invoke();
        base.Die();
    }

    public override void Move(){
        float moveHorizontal = Input.GetAxis(HORIZONTAL);
        float moveVertical = Input.GetAxis(VERTICAL);

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement = movement.normalized;
        if(movement != new Vector2(0, 0)) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);
        _rigidBody2D.velocity = new Vector3(movement.x, movement.y, 0) * _speed;
        //transform.position += new Vector3(movement.x, movement.y, 0) * _speed * Time.deltaTime;
    }

    IEnumerator AttackCoroutine(){
        while(true){
            if(Input.GetMouseButtonDown(0)){
                StartCoroutine(Attack());
            }
            yield return null;
        }
    }

    IEnumerator DefendCoroutine(){
        while(true){
            if(_canDefend && Input.GetKeyDown(KeyCode.LeftShift)){
                StartCoroutine(Defend());
            }
            yield return null;
        }
    }
}
