using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Attributes;

namespace SwordNShield.UI
{
    public class HealthBarUI : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject Owner;
        [SerializeField] private Slider hpBar;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Health health;
        private Mover mover;
        private bool isMine;
        void Awake()
        {
            PhotonView pv = Owner.GetComponent<PhotonView>();
            if (pv != null) isMine = pv.IsMine;
            Image fillImage = hpBar.fillRect.GetComponent<Image>();
            fillImage.color = isMine ? Color.green : Color.red;
            hpBar.maxValue = health.MaxHP;
            mover = Owner.GetComponent<Mover>();
            if (mover != null) StartCoroutine(FollowOwner());
        }

        IEnumerator FollowOwner()
        {
            while (true)
            {
                hpBar.maxValue = health.MaxHP;
                hpBar.value = health.HP;
                hpBar.transform.rotation = Quaternion.identity;
                hpBar.transform.position = mover.transform.position + offset;
                if (photonView.IsMine) BgHealthBarUI.Instance.SetHPBar(health.MaxHP, health.HP);
                yield return null;
            }
        }
    }
}