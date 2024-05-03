using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text roomNumber;
    [SerializeField] private TMP_Text playerCount;

    public string RoomNumber
    {
        set => roomNumber.text = value;
    }

    public string PlayerCount
    {
        set => playerCount.text = value;
    }
    public void OnClickEnterRoom()
    {
        PhotonNetwork.JoinRoom(roomNumber.text);
    }
}
