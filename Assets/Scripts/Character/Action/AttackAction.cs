using System.Collections;
using UnityEngine;

public class AttackAction : IAction
{
    public Character Owner { get; set ;}
    private float coolDownTime = 1.5f;
    private float actionTime = 0.2f;
    private bool canExecute = true;
    private bool isAttacking = false;

    public bool CanExecute{ get => canExecute; }
    public bool Playing{ get => isAttacking; }
    public float CoolDownTime{ get => coolDownTime; }
    public float ActionTime{ get => actionTime; }

    public IEnumerator Execute(){
        canExecute = false;
        Owner.GetAnimator.SetTrigger("attack");
        Owner._photonView.RPC("PlayTriggerAnimation", Photon.Pun.RpcTarget.All, "attack");
        yield return new WaitForSeconds(coolDownTime);
        canExecute = true;
    }
}
