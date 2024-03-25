using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NickNameUI : MonoBehaviourPunCallbacks
{
    [SerializeField]private Text nickName;

    void Awake()
    {
        nickName.text = photonView.IsMine ? PhotonNetwork.NickName : photonView.Owner.NickName;
        nickName.color = photonView.IsMine ? Color.green : Color.red;
    }
}
