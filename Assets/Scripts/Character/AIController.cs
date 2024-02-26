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
    int behavior;

    new void Awake(){
        base.Awake();
        targetTag = "Character";
        animator = gameObject.GetComponent<Animator>();
        selfCollider = gameObject.GetComponent<Collider2D>();
        if(_photonView.IsMine){
            StartCoroutine(CreateRandomPosition());
            StartCoroutine(AttackCoroutine());
            StartCoroutine(ChooseRandomBehavior());
        }
    }

    private void Update(){
        if(_photonView.IsMine){
            target = FoundTarget();

            if(HasTarget()){
                if(behavior == 0){
                    Move();
                }
                else if(behavior == 1){
                    FollowTarget();
                }
            }
            else{
                Move();
            }
        }
        else if((transform.position - _currentPosition).sqrMagnitude >= 100) transform.position = _currentPosition;
        else transform.position = Vector3.Lerp(transform.position, _currentPosition, Time.deltaTime * 10);
    }

    public IEnumerator CreateRandomPosition(){
        while(true){
            float randomAngle = Random.Range(0, 360);
            randomDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0).normalized;
            yield return new WaitForSeconds(Random.Range(1, 10));
        }
    }

    public IEnumerator ChooseRandomBehavior(){
        while(true){
            behavior = Random.Range(0, 2);
            yield return new WaitForSeconds(Random.Range(5, 10));
        }
    }

    private bool HasTarget(){
        return(target != null);
    }

    public override void Move(){
        Look(transform.position + randomDirection);
        animator.SetBool("isMoving", true);
        transform.position +=  randomDirection * (_speed * Time.deltaTime);
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
        float currentDistance = Vector2.Distance(transform.position, target.position);
        Look(target.position);
        if(sword.SwordLength < currentDistance){
            animator.SetBool("isMoving", true);
            transform.position = Vector2.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
        }
        else{
            animator.SetBool("isMoving", false);
        }
    }

    private IEnumerator AttackCoroutine(){
        while(true){
            if(target != null && Vector2.Distance(transform.position, target.position) < sword.SwordLength){
                StartCoroutine(Attack());
            }
            yield return null;
        }
    }
}
