using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public GameObject RightHand;
    private float angle;
    private Vector2 target;
    private Vector2 mouse;

    //Animator 변수
    private bool isMoving;
    
    void Awake(){
        StartCoroutine(AttackCoroutine());
        animator = gameObject.GetComponent<Animator>();
    }

    void Update(){
        Move();
        Look();
    }

    public override void Move(){
        float moveHorizontal = Input.GetAxis(HORIZONTAL);
        float moveVertical = Input.GetAxis(VERTICAL);

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        if(movement != new Vector2(0, 0)) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);
        transform.position += new Vector3(movement.x, movement.y, 0) * _speed * Time.deltaTime;
    }

    public override void Look(){
        target = transform.position;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    IEnumerator AttackCoroutine(){
        while(true){
            if(Input.GetMouseButtonDown(0) && !_isAttacking){
                StartCoroutine(Attack());
            }
            yield return null;
        }
    }
}
