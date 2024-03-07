using System.Collections;
using UnityEngine;

public class DashAction : IAction
{
    public Character Owner { get; set ;}
    private float coolDownTime = 3f;
    private float actionTime = 0.5f;
    private float dashSpeed = 10f;
    private bool canExecute = true;
    private bool isDashing = false;
    private Vector2 dashDirection;

    public bool CanExecute{ get => canExecute; }
    public bool Playing{ get => isDashing; }
    public float CoolDownTime{ get => coolDownTime; }
    public float ActionTime{ get => actionTime; }

    public IEnumerator Execute(){
        dashDirection = Owner.dashDirection;
        float startTime = Time.time;
        canExecute = false;
        isDashing = true;
        while(Time.time - startTime < actionTime){
            Owner.RigidBody.velocity = dashDirection * dashSpeed;
            yield return null;
        }
        Owner.TargetPos = Owner.transform.position;
        Owner.RigidBody.velocity = Vector2.zero;
        isDashing = false;
        yield return new WaitForSeconds(coolDownTime);
        canExecute = true;
    }
}
