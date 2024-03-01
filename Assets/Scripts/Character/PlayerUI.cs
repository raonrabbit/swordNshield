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
    private void OnEnable(){
        if(character._photonView.IsMine){
            DashSkillImage = CoolTimeUI.Instance.DashSkillImage;
            ShieldSkillImage = CoolTimeUI.Instance.ShieldSkillImage;
            AttackSkillImage = CoolTimeUI.Instance.AttackSkillImage;
            PlayerController.OnDash += OnDash;
            PlayerController.OnShield += OnShield;
            PlayerController.OnAttack += OnAttack;
        }
    }


    private void OnDash(){
        StartCoroutine(CoolDownDash());
    }
    private IEnumerator CoolDownDash(){
        yield return new WaitForSeconds(character.DashTime);
        float currentCoolDown = 0;
        DashSkillImage.fillAmount = 0;
        while(currentCoolDown < character.DashCoolTime){
            currentCoolDown += Time.deltaTime;
            DashSkillImage.fillAmount = currentCoolDown / character.DashCoolTime;
            yield return null;
        }
        DashSkillImage.fillAmount = 1;
    }
    private void OnShield(){
        StartCoroutine(CoolDownShield());
    }
    private IEnumerator CoolDownShield(){
        yield return new WaitForSeconds(character.DefendTime);
        float currentCoolDown = 0;
        ShieldSkillImage.fillAmount = 0;
        while(currentCoolDown < character.DefendCoolTime){
            currentCoolDown += Time.deltaTime;
            ShieldSkillImage.fillAmount = currentCoolDown / character.DefendCoolTime;
            yield return null;
        }
        ShieldSkillImage.fillAmount = 1;
    }
    private void OnAttack(){
        StartCoroutine(CoolDownAttack());
    }
    private IEnumerator CoolDownAttack(){
        yield return new WaitForSeconds(character.AttackTime);
        float currentCoolDown = 0;
        AttackSkillImage.fillAmount = 0;
        while(currentCoolDown < character.AttackCooldown){
            currentCoolDown += Time.deltaTime;
            AttackSkillImage.fillAmount = currentCoolDown / character.AttackCooldown;
            yield return null;
        }
        AttackSkillImage.fillAmount = 1;
    }
}