using SwordNShield.Attributes;
using UnityEngine;
using UnityEngine.UI;
using SwordNShield.Movement;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Text nickName;
    [SerializeField] private Mover mover;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Health health;
    void Update(){
        hpBar.value = health.HP;
        hpBar.transform.rotation = Quaternion.identity;
        hpBar.transform.position = mover.transform.position + offset;
        nickName.transform.rotation = Quaternion.identity;
        nickName.transform.position = mover.transform.position + offset - new Vector3(0, 0.3f, 0);
    }
}