using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Character
{
    [SerializeField] private float detectRadius = 5f;
    private Transform target;
    private string targetTag;
    private Vector3 randomDirection;

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
            float randomAngle = Random.Range(0, 360);
            randomDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0).normalized;
            yield return new WaitForSeconds(Random.Range(1, 10));
        }
    }

    public override void Move(){
        Look(randomDirection);
        animator.SetBool("isMoving", true);
        transform.position +=  randomDirection * (_speed * Time.deltaTime);
    }
    
    public override void Look(Vector3 target){
        Vector3 direction = target;
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
