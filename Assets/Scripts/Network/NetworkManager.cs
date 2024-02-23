using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField nameInput;
    public GameObject lobbyPanel;
    public GameObject restartPanel;
    public CinemachineVirtualCamera virtualCamera;

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster(){
        PhotonNetwork.LocalPlayer.NickName = nameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions{ MaxPlayers = 6 }, null);
    }

    public override void OnJoinedRoom(){
        lobbyPanel.SetActive(false);
        Spawn();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
    }

    public void Spawn(){
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        lobbyPanel.SetActive(false);
        virtualCamera.Follow = player.transform;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        lobbyPanel.SetActive(true);
        restartPanel.SetActive(false);
    }
}