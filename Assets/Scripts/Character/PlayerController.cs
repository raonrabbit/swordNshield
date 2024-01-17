using System.Collections;
using System.Collections.Generic;
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
    
    void Awake(){
        animator = gameObject.GetComponent<Animator>();
        StartCoroutine(AttackCoroutine());
        StartCoroutine(DefendCoroutine());
    }

    void Update(){
        Move();
        Look(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public override void Move(){
        float moveHorizontal = Input.GetAxis(HORIZONTAL);
        float moveVertical = Input.GetAxis(VERTICAL);

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement = movement.normalized;
        if(movement != new Vector2(0, 0)) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);
        transform.position += new Vector3(movement.x, movement.y, 0) * _speed * Time.deltaTime;
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
            if(Input.GetKeyDown(KeyCode.LeftShift)){
                StartDefend();
            }

            if(Input.GetKeyUp(KeyCode.LeftShift)){
                EndDefend();
            }
            yield return null;
        }
    }
}
