using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Attributes;

namespace SwordNShield.UI
{
    public class HealthBarUI : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Slider hpBar;
        [SerializeField] private Text nickName;
        [SerializeField] private Mover mover;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Health health;

        private

            void Awake()
        {
            Image fillImage = hpBar.fillRect.GetComponent<Image>();
            fillImage.color = photonView.IsMine ? Color.green : Color.red;
            hpBar.maxValue = health.MaxHP;
        }

        void Update()
        {
            hpBar.maxValue = health.MaxHP;
            hpBar.value = health.HP;
            hpBar.transform.rotation = Quaternion.identity;
            hpBar.transform.position = mover.transform.position + offset;
            nickName.transform.rotation = Quaternion.identity;
            nickName.transform.position = mover.transform.position + offset - new Vector3(0, 0.3f, 0);
            if (photonView.IsMine) BgHealthBarUI.Instance.SetHPBar(health.MaxHP, health.HP);
        }
    }
}