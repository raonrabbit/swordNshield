using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Character
{
    [SerializeField] private float detectRadius = 5f;
    private Transform target;
    private string targetTag;
    private Vector3 randomPosition;

    private void Awake(){
        targetTag = "Character";
        animator = gameObject.GetComponent<Animator>();
        StartCoroutine(CreateRandomPosition());
    }

    private void Update(){
        target = FoundTarget();
        Move();
    }

    public IEnumerator CreateRandomPosition(){
        while(true){
            System.Random randomX = new System.Random();
            System.Random randomY = new System.Random();
            randomPosition = transform.position;
            randomPosition += new Vector3(randomX.Next(-5, 5), randomY.Next(-5, 5), transform.position.z);
            yield return new WaitForSeconds(4f);
        }
    }

    public override void Move(){
        if(Vector2.Distance(transform.position, randomPosition) < 0.1f){
            animator.SetBool("isMoving", false);
        }
        else{
            Look(randomPosition);
            animator.SetBool("isMoving", true);
            Vector3 direction = (randomPosition - transform.position).normalized;
            transform.position +=  direction * (_speed * Time.deltaTime);
        }
    }
    
    public override void Look(Vector3 target){
        Vector3 direction = target - transform.position;
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
