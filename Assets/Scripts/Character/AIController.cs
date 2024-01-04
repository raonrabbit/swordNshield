using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Character
{
    [SerializeField] private float detectRadius = 5f;
    private Transform target;
    private Animator animator;
    private string targetTag;

    private void Awake(){
        targetTag = "Character";
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update(){
        Move();
        Look();
    }

    public override void Move(){
        if(target == null){
            target = FoundTarget();
        }
        else{
            FollowTarget();
        }
    }

    public override void Attack(){
        
    }

    public override void Look(){
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private Transform FoundTarget(){
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, detectRadius);

        if(cols.Length > 0){
            for(int i = 0; i < cols.Length; i++){
                if(cols[i].CompareTag(targetTag)){
                    Transform obj = cols[i].gameObject.transform;
                    return obj;
                }
            }
        }
        return null;
    }

    private void FollowTarget(){
        animator.SetBool("isMoving", true);
        transform.position = Vector2.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
    }
}
