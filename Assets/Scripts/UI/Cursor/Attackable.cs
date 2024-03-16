using Photon.Pun;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    private PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void OnMouseEnter()
    {
        if (photonView == null || !photonView.IsMine)
        {
            CursorManager.Instance.SetAttackCursor();
        }
    }

    private void OnMouseExit()
    {
        if (photonView == null || !photonView.IsMine)
        {
            CursorManager.Instance.SetDefaultCursor();
        }
    }
}
