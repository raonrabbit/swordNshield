using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Character
{
    [SerializeField] private float detectRadius = 4f;
    [SerializeField] private Collider2D selfCollider;
    [SerializeField] private Transform target;
    private string targetTag;
    private Vector3 randomDirection;

    private void Awake(){
        targetTag = "Character";
        animator = gameObject.GetComponent<Animator>();
        selfCollider = gameObject.GetComponent<Collider2D>();
        StartCoroutine(CreateRandomPosition());
        StartCoroutine(AttackCoroutine());
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
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
    }

    private Transform FoundTarget(){
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, detectRadius);
        float closestDistance = float.MaxValue;
        Transform closestTransform = null;

        foreach(Collider2D col in cols){
            if(col == selfCollider || !col.CompareTag(targetTag)) continue;
            float distance = Vector2.Distance(transform.position, col.transform.position);
            if(distance < closestDistance){
                closestDistance = distance;
                closestTransform = col.transform;
            }
        }
        return closestTransform;
    }

    private void FollowTarget(){
        animator.SetBool("isMoving", true);
        transform.position = Vector2.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
    }

    private IEnumerator AttackCoroutine(){
        while(true){
            if(target != null && Vector2.Distance(transform.position, target.position) < sword.SwordLength){
                Look(target.position);
                StartCoroutine(Attack());
            }
            yield return null;
        }
    }
}
