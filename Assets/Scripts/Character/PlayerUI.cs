using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Image DashSkillImage;
    [SerializeField] private Image ShieldSkillImage;
    [SerializeField] private Image AttackSkillImage;
    private Dictionary<string, IAction> Actions;
    private void Awake(){
        if(character._photonView.IsMine){
            Actions = character.Actions;
            DashSkillImage = CoolTimeUI.Instance.DashSkillImage;
            DashSkillImage.fillAmount = 1;
            ShieldSkillImage = CoolTimeUI.Instance.ShieldSkillImage;
            ShieldSkillImage.fillAmount = 1;
            AttackSkillImage = CoolTimeUI.Instance.AttackSkillImage;
            AttackSkillImage.fillAmount = 1;
            PlayerController.OnDash += OnDash;
            PlayerController.OnShield += OnShield;
            PlayerController.OnAttack += OnAttack;
        }
    }

    private void OnDestroy(){
        if(character._photonView.IsMine){
            PlayerController.OnDash -= OnDash;
            PlayerController.OnShield -= OnShield;
            PlayerController.OnAttack -= OnAttack;
        }
    }

    private void OnDash(){ StartCoroutine(CoolDownImage(DashSkillImage, Actions["Dash"].ActionTime, Actions["Dash"].CoolDownTime)); }
    private void OnShield(){ StartCoroutine(CoolDownImage(ShieldSkillImage, Actions["Defend"].ActionTime, Actions["Defend"].CoolDownTime)); }
    private void OnAttack(){ StartCoroutine(CoolDownImage(AttackSkillImage, Actions["Attack"].ActionTime, Actions["Dash"].CoolDownTime)); }

    private IEnumerator CoolDownImage(Image skillImage, float actionTime, float coolDownTime){
        yield return new WaitForSeconds(actionTime);
        float currentCoolDown = 0;
        skillImage.fillAmount = 0;
        while(currentCoolDown < coolDownTime){
            currentCoolDown += Time.deltaTime;
            skillImage.fillAmount = currentCoolDown / coolDownTime;
            yield return null;
        }
        skillImage.fillAmount = 1;
    }
}