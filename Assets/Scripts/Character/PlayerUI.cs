using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Text nickName;
    [SerializeField] private Character character;
    [SerializeField] private Vector3 offset;
    void Update(){
        hpBar.value = character.GetHp;
        hpBar.transform.rotation = Quaternion.identity;
        hpBar.transform.position = character.transform.position + offset;
        nickName.transform.rotation = Quaternion.identity;
        nickName.transform.position = character.transform.position + offset - new Vector3(0, 0.3f, 0);
    }
}