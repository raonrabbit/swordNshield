using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public GameObject RightHand;

    [SerializeField]
    private float speed = 5f;
    private float angle;
    private Vector2 target;
    private Vector2 mouse;
    private float weaponRotateAngle = 70f;
    private float rotationTime = 0.2f;
    
    void Awake(){
        StartCoroutine(AttackCoroutine());
    }
    void Update(){
        Move();
        Look();
    }

    public override void Move(){
        float moveHorizontal = Input.GetAxis(HORIZONTAL);
        float moveVertical = Input.GetAxis(VERTICAL);

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.position += new Vector3(movement.x, movement.y, 0) * speed * Time.deltaTime;
    }

    public override void Look(){
        target = transform.position;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    IEnumerator AttackCoroutine(){
        while(true){
            if(Input.GetMouseButtonDown(0)){
                Attack();
                yield return new WaitForSeconds(2f);
            }
            yield return null;
        }
    }

    public override void Attack(){
        StartCoroutine(SwingWeapon());
    }

    IEnumerator SwingWeapon(){
        Quaternion startRotation = Quaternion.Euler(new Vector3(3, 0, 0));
        Quaternion endRotation = Quaternion.Euler(new Vector3(0, 0, weaponRotateAngle));

        for(float t = 0; t < 1; t += Time.deltaTime / rotationTime)
        {
            RightHand.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        for(float t = 0; t < 1; t += Time.deltaTime / rotationTime)
        {
            RightHand.transform.localRotation = Quaternion.Lerp(endRotation, startRotation, t);
            yield return null;
        }
    }
}
