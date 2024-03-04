using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeUI : MonoBehaviour
{
    private static CoolTimeUI instance = null;
    public Image DashSkillImage;
    public Image ShieldSkillImage;
    public Image AttackSkillImage;

    void Awake(){
        if(instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public static CoolTimeUI Instance{
        get => instance;
    }
}
