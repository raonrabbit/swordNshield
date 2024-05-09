using System.Collections;
using Photon.Pun;
using SwordNShield.Combat.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace SwordNShield.UI
{
    public class NickNameUI : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject Owner;
        [SerializeField] private Text nickName;
        [SerializeField] private Vector3 offset;
        private Mover mover;
        private bool isMine;
        void Awake()
        {
            PhotonView pv = Owner.GetComponent<PhotonView>();
            if (pv != null) isMine = pv.IsMine;
            nickName.text = isMine ? PhotonNetwork.NickName : pv.Owner.NickName;
            nickName.color = isMine ? Color.green : Color.red;
            
            mover = Owner.GetComponent<Mover>();
            if (mover != null) StartCoroutine(FollowOwner());
        }

        IEnumerator FollowOwner()
        {
            while (true)
            {
                nickName.transform.rotation = Quaternion.identity;
                nickName.transform.position = mover.transform.position + offset - new Vector3(0, 0.3f, 0);
                yield return null;
            }
        }
    }
}