using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Character character;
    [SerializeField] private Vector3 offset;
    void Update(){
        hpBar.value = character.GetHp;
        hpBar.transform.rotation = Quaternion.identity;
        hpBar.transform.position = character.transform.position + offset;
    }
}