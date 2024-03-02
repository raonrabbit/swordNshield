using System.Collections;
using UnityEngine;

public class DefendAction : IAction
{
    public Character Owner { get; set ;}
    private float coolDownTime = 3f;
    private float actionTime = 4f;
    private bool canExecute = true;
    private bool isDefending = false;

    public bool CanExecute{ get => canExecute; }
    public bool Playing{ get => isDefending; }
    public float CoolDownTime{ get => coolDownTime; }
    public float ActionTime{ get => actionTime; }

    public IEnumerator Execute(){
        canExecute = false;
        isDefending = true;
        Owner.GetAnimator.SetBool("isDefending", true);
        yield return new WaitForSeconds(actionTime);
        isDefending = false;
        Owner.GetAnimator.SetBool("isDefending", false);
        yield return new WaitForSeconds(coolDownTime);
        canExecute = true;
    }
}
